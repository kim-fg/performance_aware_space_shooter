using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnerSystem : ISystem {
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var ecb = GetEntityCommandBuffer(ref state);

        new ProcessSpawnerJob {
            Ecb = ecb,
            ElapsedTime = SystemAPI.Time.ElapsedTime,
        }.ScheduleParallel();
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state) {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
}

public partial struct ProcessSpawnerJob : IJobEntity {
    public EntityCommandBuffer.ParallelWriter Ecb;
    public double ElapsedTime;

    private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spawner spawner) {
        if (spawner.NextSpawnTime > ElapsedTime) 
            return;

        var newEntity = Ecb.Instantiate(chunkIndex, spawner.Prefab);
        Ecb.SetComponent(
            chunkIndex,
            newEntity, 
            LocalTransform.FromPosition(spawner.SpawnPosition)
        );

        spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
    }
}