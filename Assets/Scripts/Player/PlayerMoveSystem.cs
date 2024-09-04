using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
[BurstCompile]
public partial struct PlayerMoveSystem : ISystem {
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        new PlayerMoveJob {
            DeltaTime = deltaTime
        }.Schedule();
    }
}

[BurstCompile]
public partial struct PlayerMoveJob : IJobEntity {
    public float DeltaTime;

    [BurstCompile]
    private void Execute(PlayerMovement playerMovement, LocalTransform localTransform, ref PhysicsVelocity physicsVelocity, in PlayerInput playerInput) {
        // apply thrust
        if (playerInput.Thrust) {
            physicsVelocity.Linear += localTransform.Forward() * playerMovement.ThrustPower * DeltaTime;
            return;
        }
        
        // brake
        physicsVelocity.Linear = Vector3.MoveTowards(
            physicsVelocity.Linear, Vector3.zero, 
            DeltaTime * playerMovement.BrakePower
        );
    }
}