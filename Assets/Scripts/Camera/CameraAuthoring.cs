using Unity.Entities;
using UnityEngine;

public class CameraAuthoring : MonoBehaviour {
    // pass smoothing values from inspector later   
}

public class CameraBaker : Baker<CameraAuthoring> {
    public override void Bake(CameraAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<CameraFollow>(entity);
    }
}
