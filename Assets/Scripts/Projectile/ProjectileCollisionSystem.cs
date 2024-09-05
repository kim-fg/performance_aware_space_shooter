using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct ProjectileCollisionSystem : ISystem {
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<SimulationSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        state.Dependency = new ProjectileCollisionJob() {
            Ecb = ecb,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        
        state.Dependency.Complete();
        
        ecb.Playback(state.EntityManager);
        
        ecb.Dispose();
    }
}

[BurstCompile]
public struct ProjectileCollisionJob : ICollisionEventsJob {
    public EntityCommandBuffer Ecb;
    
    public void Execute(CollisionEvent collisionEvent) {
        Ecb.DestroyEntity(collisionEvent.EntityA);
        Ecb.DestroyEntity(collisionEvent.EntityB);
    }
}