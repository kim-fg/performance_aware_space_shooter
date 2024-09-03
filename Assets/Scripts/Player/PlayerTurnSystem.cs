using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
[BurstCompile]
public partial struct PlayerTurnSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        new PlayerTurnJob {
            DeltaTime = deltaTime,
        }.Schedule();
    }
}

[BurstCompile]
public partial struct PlayerTurnJob : IJobEntity {
    public float DeltaTime;

    private void Execute(PlayerMovement playerMovement, ref LocalTransform localTransform, in PlayerInput playerInput) {
        var turnDelta = playerInput.TurnInput * playerMovement.TurnSpeed * DeltaTime;
        localTransform = localTransform.RotateY(Mathf.Deg2Rad * turnDelta);
    }
}
