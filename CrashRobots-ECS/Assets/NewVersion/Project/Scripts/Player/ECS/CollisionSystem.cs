using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public partial struct CollisionSystem : ISystem
{
    private Entity BufferEntity;

    public void OnCreate(ref SystemState state)
    {
        BufferEntity = state.EntityManager.CreateEntity(typeof(EnemyBufferElement));
    }

    public void OnUpdate(ref SystemState state)
    {
        DynamicBuffer<EnemyBufferElement> buffer = state.EntityManager.GetBuffer<EnemyBufferElement>(BufferEntity);
        var playerPos = PlayerDataManager_V2.Instance.transform.position;
        var playerRadius = 1.0f;
        for (int i = 0; i < buffer.Length; ++i)
        {
            Entity entity = buffer[i].Enemy;
            var enemyPos = SystemAPI.GetComponentRW<LocalTransform>(entity).ValueRO.Position;
            var enemyRadius = buffer[i].CollisionRadius;

            var x = (playerPos.x - enemyPos.x) * (playerPos.x - enemyPos.x);
            var z = (playerPos.z - enemyPos.z) * (playerPos.z - enemyPos.z);
            var radius = (playerRadius + enemyRadius) * (playerRadius + enemyRadius);

            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                state.EntityManager.DestroyEntity(entity);
            }

            //DynamicBuffer<EnemyBufferElement> buffer = state.EntityManager.GetBuffer<EnemyBufferElement>(enemyEntity);
            //if(Keyboard.current.yKey.wasPressedThisFrame)
            //{
            //buffer.Clear();

            // }
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
