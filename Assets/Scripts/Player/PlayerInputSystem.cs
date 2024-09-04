using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
[BurstCompile]
public partial class PlayerInputSystem : SystemBase { 
    private DefaultControls _inputActions;

    protected override void OnCreate() {
        RequireForUpdate<PlayerInput>();
        RequireForUpdate<PlayerMovement>();
        _inputActions = new DefaultControls();
    }

    protected override void OnStartRunning() {
        _inputActions.Enable();
    }

    [BurstCompile]
    protected override void OnUpdate() {
        var thrustInput = _inputActions.Player.Thrust.ReadValue<float>();
        var turnInput = _inputActions.Player.Turn.ReadValue<float>();
        var fireInput = _inputActions.Player.Fire.IsPressed();

        new UpdatePlayerInputJob {
            ThrustInput = thrustInput,
            TurnInput = turnInput,
            FireInput = fireInput,
        }.Schedule();
    }

    protected override void OnStopRunning() {
        _inputActions.Disable();
    }
}

[BurstCompile]
public partial struct UpdatePlayerInputJob : IJobEntity {
    public float ThrustInput;
    public float TurnInput;
    public bool FireInput;

    [BurstCompile]
    private void Execute(ref PlayerInput playerInput) {
        playerInput = new PlayerInput {
            ThrustInput = ThrustInput,
            TurnInput = TurnInput,
            FireInput = FireInput,
        };
    }
}
