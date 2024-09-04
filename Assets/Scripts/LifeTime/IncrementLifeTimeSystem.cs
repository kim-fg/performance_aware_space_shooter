using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct IncrementLifeTimeSystem : ISystem {
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        foreach (var lifeTime in SystemAPI.Query<RefRW<LifeTime>>()) {
            lifeTime.ValueRW.ElapsedSeconds += deltaTime;
        }
    }
}