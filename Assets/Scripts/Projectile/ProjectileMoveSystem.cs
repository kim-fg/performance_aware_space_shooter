using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public partial struct ProjectileMoveSystem : ISystem{
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        // this looks stupid
        new ProjectileMoveJob().ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ProjectileMoveJob : IJobEntity {

    [BurstCompile]
    private void Execute(Projectile projectile, LocalTransform localTransform, ref PhysicsVelocity physicsVelocity) {
        physicsVelocity.Linear = localTransform.Forward() * projectile.Speed;
    }
}
