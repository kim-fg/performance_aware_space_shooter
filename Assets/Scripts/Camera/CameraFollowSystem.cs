using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup)), UpdateAfter(typeof(PlayerMoveSystem))]
[BurstCompile]
public partial class CameraFollowSystem : SystemBase {
    private Entity _player;
    private Transform _cameraTransform;

    protected override void OnStartRunning() {
        RequireForUpdate<PlayerTag>();
        
        _player = SystemAPI.GetSingletonEntity<PlayerTag>();
        _cameraTransform = Camera.main?.transform.root; // YES this is ugly, but does it really matter?
        // if it does matter: https://docs.unity3d.com/Packages/com.unity.entities@0.17/api/Unity.Entities.EntityManager.AddComponentObject.html
    }

    [BurstCompile]
    protected override void OnUpdate() {
        var playerTransform = SystemAPI.GetComponent<LocalTransform>(_player);
        var playerPosition = playerTransform.Position;
        
        _cameraTransform.position = playerPosition;
    }
}
