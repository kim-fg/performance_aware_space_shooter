using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial class SpawnEnemySystem : SystemBase {
    private Entity _player;
    private Random _random;
    
    protected override void OnStartRunning() {
       _player = SystemAPI.GetSingletonEntity<PlayerTag>();
       _random = new Random(1337);
    }

    [BurstCompile]
    protected override void OnUpdate() {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var playerTransform = SystemAPI.GetComponent<LocalToWorld>(_player);
        var playerPosition = playerTransform.Position;
        
        new SpawnEnemyJob {
            Random = _random,
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = playerPosition,
            Ecb = ecb,
        }.Schedule();
    }
}

[BurstCompile]
public partial struct SpawnEnemyJob : IJobEntity {
    public Random Random;
    public float DeltaTime;
    public float3 PlayerPosition;
    public EntityCommandBuffer Ecb;

    [BurstCompile]
    private void Execute(ref EnemySpawner enemySpawner) {
        enemySpawner.TimeSinceLastSpawn += DeltaTime;
        
        if (enemySpawner.TimeSinceLastSpawn < enemySpawner.SpawnDelay) {
            return;
        }

        var entity = Ecb.Instantiate(enemySpawner.Prefab);

        var randomOffset = math.normalize(Random.NextFloat3()) ;
        randomOffset.y = 0.0f;
        randomOffset *= enemySpawner.OffsetRadius;
        var spawnPosition = PlayerPosition + randomOffset;
        
        Ecb.SetComponent(entity, LocalTransform.FromPosition(spawnPosition));
        
        enemySpawner.TimeSinceLastSpawn = 0.0f;
    }
}