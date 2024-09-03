using Unity.Entities;

public struct PlayerTag : IComponentData { /* Empty on purpose */ }

public struct PlayerMovement : IComponentData {
    public float ThrustPower;
    public float TurnSpeed;
    public float BrakePower;
}

public struct PlayerInput : IComponentData {
    public float ThrustInput;
    public float TurnInput;
    public bool FireInput;
}
