using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour {
    [SerializeField] private float thrustPower = 5.0f;
    public float ThrustPower => thrustPower;
    
    [SerializeField] private float brakePower = 2.5f;
    public float BrakePower => brakePower;

    [SerializeField] private float turnSpeed = 45.0f;
    public float TurnSpeed => turnSpeed;
}

public class PlayerBaker : Baker<PlayerAuthoring> {
    public override void Bake(PlayerAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent<PlayerTag>(entity);
        AddComponent<PlayerInput>(entity);
        AddComponent(entity, new PlayerMovement() {
            ThrustPower = authoring.ThrustPower,
            TurnSpeed = authoring.TurnSpeed,
            BrakePower = authoring.BrakePower,
        });
    }
}
