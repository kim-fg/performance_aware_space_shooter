using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnemySpawnerAuthoring : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

    [SerializeField] private float spawnDelay = 1.0f;
    public float SpawnDelay => spawnDelay;

    [SerializeField] private float offsetRadius = 100.0f;
    public float OffsetRadius => offsetRadius;
}

public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring> {
    public override void Bake(EnemySpawnerAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.None);
        
        AddComponent(entity, new EnemySpawner(
            GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic), 
            authoring.SpawnDelay,
            authoring.OffsetRadius
        ));
    }
}

public struct EnemySpawner : IComponentData {
    public readonly Entity Prefab;
    public readonly float SpawnDelay;
    public readonly float OffsetRadius;
    public float TimeSinceLastSpawn;
    public Random Random;

    public EnemySpawner(Entity prefab, float spawnDelay, float offsetRadius) {
        Prefab = prefab;
        SpawnDelay = spawnDelay;
        OffsetRadius = offsetRadius;
        TimeSinceLastSpawn = 0.0f;
        Random = new Random(1337);
    }
}
