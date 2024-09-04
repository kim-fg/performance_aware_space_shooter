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
        var thrustInput = _inputActions.Player.Thrust.IsPressed();
        var turnInput = _inputActions.Player.Turn.ReadValue<float>();
        var fireInput = _inputActions.Player.Fire.IsPressed();

        new UpdatePlayerInputJob {
            Thrust = thrustInput,
            Turn = turnInput,
            Fire = fireInput,
        }.Schedule();
    }

    protected override void OnStopRunning() {
        _inputActions.Disable();
    }
}

[BurstCompile]
public partial struct UpdatePlayerInputJob : IJobEntity {
    public bool Thrust;
    public float Turn;
    public bool Fire;

    [BurstCompile]
    private void Execute(ref PlayerInput playerInput) {
        playerInput = new PlayerInput {
            Thrust = Thrust,
            TurnInput = Turn,
            FireInput = Fire,
        };
    }
}
