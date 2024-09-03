using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
[BurstCompile]
public partial class PlayerInputSystem : SystemBase { 
    private DefaultControls _inputActions;
    //private Entity _player;

    protected override void OnCreate() {
        //RequireForUpdate<PlayerTag>();
        RequireForUpdate<PlayerInput>();
        RequireForUpdate<PlayerMovement>();
        _inputActions = new DefaultControls();
    }

    protected override void OnStartRunning() {
        _inputActions.Enable();
        //_player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    [BurstCompile]
    protected override void OnUpdate() {
        var thrustInput = _inputActions.Player.Thrust.ReadValue<float>();
        var turnInput = _inputActions.Player.Turn.ReadValue<float>();
        var fireInput = _inputActions.Player.Fire.ReadValue<bool>();
        
        SystemAPI.SetSingleton(new PlayerInput() {
            FireInput = fireInput,
            ThrustInput = thrustInput,
            TurnInput = turnInput,
        });
    }

    protected override void OnStopRunning() {
        _inputActions.Disable();
        //_player = Entity.Null; 
    }
}
