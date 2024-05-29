using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public partial struct CollisionSystem : ISystem
{
    private Entity bufferEntity;

    public void OnCreate(ref SystemState state)
    {
        var query = state.EntityManager.CreateEntityQuery(typeof(EnemyBufferElement));
        bufferEntity = query.GetSingletonEntity();
    }

    public void OnUpdate(ref SystemState state)
    {
        DynamicBuffer<EnemyBufferElement> buffer = state.EntityManager.GetBuffer<EnemyBufferElement>(bufferEntity);
        var playerPos = PlayerDataManager_V2.Instance.transform.position;
        var playerRadius = 1.0f;

        foreach(var enemyBufferElement in buffer)
        {
            var entity = enemyBufferElement.Enemy;
            var enemyPos = SystemAPI.GetComponentRW<LocalTransform>(entity).ValueRO.Position;
            var enemyRadius = enemyBufferElement.CollisionRadius;

            var x = (playerPos.x - enemyPos.x) * (playerPos.x - enemyPos.x);
            var z = (playerPos.z - enemyPos.z) * (playerPos.z - enemyPos.z);
            var radius = (playerRadius + enemyRadius) * (playerRadius + enemyRadius);

            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                state.EntityManager.DestroyEntity(entity);
            }
        }

        foreach (var (paramsData,transform) in SystemAPI.Query<RefRO<ParamsData>,RefRW<LocalTransform>>())
        {
            var pos = transform.ValueRO.Position;

            if(Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                pos = Vector3.zero;
            }

            transform.ValueRW.Position = pos;
        }
    }
}
