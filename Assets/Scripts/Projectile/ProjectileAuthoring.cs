using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour {
    [SerializeField] private int damage = 3;
    public int Damage => damage;
    
    [SerializeField] private float speed = 15f;
    public float Speed => speed;

    [SerializeField] private float lifeTime = 5f;
    public float LifeTime => lifeTime;
}

public class ProjectileBaker : Baker<ProjectileAuthoring> {
    public override void Bake(ProjectileAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent(entity, new Projectile(authoring.Damage, authoring.Speed));
        AddComponent(entity, new LifeTime(authoring.LifeTime));
    }
}

public struct Projectile : IComponentData {
    public int Damage { get; private set; }
    public float Speed { get; private set; }

    public Projectile(int damage, float speed) {
        Damage = damage;
        Speed = speed;
    }
}