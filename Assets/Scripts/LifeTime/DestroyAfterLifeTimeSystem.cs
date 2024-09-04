using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

// YES this is fucky, but i couldnt get it to work any other way..

[BurstCompile]
public partial class DestroyAfterLifeTimeSystem : SystemBase {
    
    [BurstCompile]
    protected override void OnUpdate() {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entities.ForEach((Entity entity, in LifeTime lifeTime) => {
            if (lifeTime.ElapsedSeconds >= lifeTime.MaxSeconds) {
                // im using ecb here to not make structural changes until safe
                ecb.DestroyEntity(entity);
            }
        }).Schedule();

        Dependency.Complete();
        
        ecb.Playback(EntityManager);
        
        ecb.Dispose();
    }
}