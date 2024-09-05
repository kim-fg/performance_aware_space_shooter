using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnEnemySystem : ISystem {
    private Entity _player;
    private Random _random;

    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<PlayerTag>();
        _player = SystemAPI.GetSingletonEntity<PlayerTag>();
        _random = new Random(1337);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var ecb = GetEntityCommandBuffer(ref state);
        var playerTransform = SystemAPI.GetComponent<LocalToWorld>(_player);
        var playerPosition = playerTransform.Position;
        
        new SpawnEnemyJob {
            Random = _random,
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
    public Random Random;
    public float DeltaTime;
    public float3 PlayerPosition;
    public EntityCommandBuffer.ParallelWriter Ecb;

    [BurstCompile]
    private void Execute([ChunkIndexInQuery] int chunkIndex, ref EnemySpawner enemySpawner) {
        enemySpawner.TimeSinceLastSpawn += DeltaTime;
        
        if (enemySpawner.TimeSinceLastSpawn < enemySpawner.SpawnDelay) {
            return;
        }

        var entity = Ecb.Instantiate(chunkIndex, enemySpawner.Prefab);

        var randomOffset = math.normalize(Random.NextFloat3()) ;
        randomOffset.y = 0.0f;
        randomOffset *= enemySpawner.OffsetRadius;
        var spawnPosition = PlayerPosition + randomOffset;
        
        Ecb.SetComponent(
            chunkIndex,
            entity, 
            LocalTransform.FromPosition(spawnPosition)
        );
        
        enemySpawner.TimeSinceLastSpawn = 0.0f;
    }
}