using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpawnEnemySystem : ISystem {
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var ecb = GetEntityCommandBuffer(ref state);
        var player = SystemAPI.GetSingletonEntity<PlayerTag>();;
        var playerTransform = SystemAPI.GetComponent<LocalToWorld>(player);
        var playerPosition = playerTransform.Position;
        
        new SpawnEnemyJob {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = playerPosition,
            Ecb = ecb,
        }.ScheduleParallel();
    }
    
    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state) {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
}

[BurstCompile]
public partial struct SpawnEnemyJob : IJobEntity {
    public float DeltaTime;
    public float3 PlayerPosition;
    public EntityCommandBuffer.ParallelWriter Ecb;

    [BurstCompile]
    private void Execute([ChunkIndexInQuery] int chunkIndex, ref EnemySpawner enemySpawner) {
        enemySpawner.TimeSinceLastSpawn += DeltaTime;
        
        if (enemySpawner.TimeSinceLastSpawn < enemySpawner.SpawnDelay) {
            return;
        }

        for (int i = 0; i < enemySpawner.SpawnsPerWave; i++) {
            var randomOffset = enemySpawner.Random.NextFloat3Direction();
            randomOffset *= enemySpawner.OffsetRadius;
            randomOffset.y = 0.0f;
            var spawnPosition = PlayerPosition + randomOffset;

            var directionToPlayer = math.normalize(PlayerPosition - spawnPosition);
            var spawnRotation = quaternion.LookRotation(
                directionToPlayer, 
                new float3(0.0f, 1.0f, 0.0f) //float3.Y doesnt exist >:(
            );
        
            var entity = Ecb.Instantiate(chunkIndex, enemySpawner.Prefab);
            Ecb.SetComponent(
                chunkIndex + i,
                entity, 
                LocalTransform.FromPositionRotation(
                    spawnPosition, 
                    spawnRotation
                )
            );
            Ecb.SetComponent(
                chunkIndex + i,
                entity,
                new PhysicsVelocity {
                    Angular = float3.zero,
                    Linear = directionToPlayer * 3f,
                }
            );
            Ecb.AddComponent(
                chunkIndex + i,
                entity,
                new LifeTime(20.0f)
            );
        }
        
        enemySpawner.TimeSinceLastSpawn = 0.0f;
    }
}