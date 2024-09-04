using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct PlayerBlasterSystem : ISystem {
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach (var (playerInput, entity) in SystemAPI.Query<RefRO<PlayerInput>>().WithAll<Blaster>().WithEntityAccess()) {
            SystemAPI.SetComponentEnabled<FireProjectileTag>(entity, playerInput.ValueRO.FireInput);
        }
    }
}

// todo! THIS SHOULDNT BE HERE
//
// public struct EnemyInput : IComponentData {
//     public bool FireInput;
// }
//
// public partial struct EnemyBlasterSystem : ISystem {
//     public void OnUpdate(ref SystemState state) {
//         foreach (var (enemyInput, entity) in SystemAPI.Query<RefRO<EnemyInput>>().WithAll<Blaster, FireProjectileTag>().WithEntityAccess()) {
//             SystemAPI.SetComponentEnabled<FireProjectileTag>(entity, enemyInput.ValueRO.FireInput);
//         }
//     }
// }
