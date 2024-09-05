using Unity.Entities;
using UnityEngine;

public class BlasterAuthoring : MonoBehaviour {
    [SerializeField] private GameObject projectilePrefab;
    public GameObject ProjectilePrefab => projectilePrefab;

    [SerializeField] private float fireDelay = 0.5f;
    public float FireDelay => fireDelay;
}

public class BlasterBaker : Baker<BlasterAuthoring> {
    public override void Bake(BlasterAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        // This is fucky but what can you do..
        // player input should only be a component anyway
        if (GetComponentInParent<PlayerAuthoring>()) {
            AddComponent<PlayerInput>(entity);
        }
        
        AddComponent(entity, new Blaster(
            GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
            authoring.FireDelay
        ));
        AddComponent<FireProjectileTag>(entity);
    }
}

public struct Blaster : IComponentData {
    public readonly Entity ProjectilePrefab;
    public readonly float FireDelay;
    public float TimeSinceLastShot;

    public Blaster(Entity projectilePrefab, float fireDelay) {
        ProjectilePrefab = projectilePrefab;
        FireDelay = fireDelay;
        // Initialize as equal to firedelay so player doesnt need to wait
        TimeSinceLastShot = fireDelay;
    }
}

public struct FireProjectileTag : IComponentData, IEnableableComponent { }
