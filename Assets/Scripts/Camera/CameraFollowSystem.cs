using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup)), UpdateAfter(typeof(PlayerMoveSystem))]
[BurstCompile]
public partial class CameraFollowSystem : SystemBase {
    private Entity _player;

    protected override void OnStartRunning() {
        RequireForUpdate<PlayerTag>();
        
        _player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    [BurstCompile]
    protected override void OnUpdate() {
        var playerTransform = SystemAPI.GetComponent<LocalTransform>(_player);
        var playerPosition = playerTransform.Position;

         new CameraFollowJob() {
            TargetPosition = playerPosition,
         }.Schedule();
    }
}

[BurstCompile]
public partial struct CameraFollowJob : IJobEntity {
    public float3 TargetPosition;
    
    [BurstCompile]
    private void Execute(CameraFollow cameraFollow, ref LocalTransform localTransform) {
        TargetPosition.y = 0.0f;
        localTransform.Position = TargetPosition;
    }
}
