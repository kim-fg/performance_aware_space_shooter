using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct BlasterFireSystem : ISystem {
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach (var (blaster, transform, owner) in SystemAPI.Query<RefRW<Blaster>, RefRO<LocalToWorld>>().WithAll<FireProjectileTag>().WithEntityAccess()) {
            if (blaster.ValueRW.TimeSinceLastShot < blaster.ValueRW.FireDelay) {
                continue;
            }
            
            var projectile = state.EntityManager.Instantiate(blaster.ValueRO.ProjectilePrefab);
            
            var projectileTransform = LocalTransform.FromPositionRotation(transform.ValueRO.Position, transform.ValueRO.Rotation);
            SystemAPI.SetComponent(projectile, projectileTransform);

            blaster.ValueRW.TimeSinceLastShot = 0.0f;
        }
    }
}