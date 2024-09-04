using Unity.Entities;
using UnityEditor.SceneManagement;
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
        
        AddComponent(entity, new Blaster {
            ProjectilePrefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
            FireDelay = authoring.FireDelay,
            // Initialize as equal to firedelay so player doesnt need to wait
            TimeSinceLastShot = authoring.FireDelay
        });
        AddComponent<FireProjectileTag>(entity);
    }
}

public struct Blaster : IComponentData {
    public Entity ProjectilePrefab;
    public float FireDelay;
    public float TimeSinceLastShot;
}

public struct FireProjectileTag : IComponentData, IEnableableComponent { }
