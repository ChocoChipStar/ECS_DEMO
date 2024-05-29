using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct EnemySpawnSystem : ISystem
{
    public Entity[] instanceEntity;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ConfigData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<ConfigData>();

        var instance = state.EntityManager.Instantiate
        (
            config.EnemyPrefab, config.MaxCount, Allocator.Temp
        );

        double maxRadius = Mathf.Pow(config.MaxSpawnRadius, 2);
        double minRadius = Mathf.Pow(config.MinSpawnRadius, 2);

        var random = new System.Random();
        var randomPos = new float3();
        var enemyNumber = 0;

        foreach (var entity in instance)
        {
            // ���S�_�v���C���[���W����~��Ƀ����_���z�u
            for (int j = 0; j < 100; ++j)
            {
                var x = random.Next(-config.MaxSpawnRadius, config.MaxSpawnRadius);
                var z = random.Next(-config.MinSpawnRadius, config.MinSpawnRadius);

                var xAbs = Math.Abs(Math.Pow(x, 2));
                var zAbs = Math.Abs(Math.Pow(z, 2));

                if (maxRadius > xAbs + zAbs && xAbs + zAbs > minRadius)
                {
                    randomPos = new float3(x, 5.0f, z) /*+ PlayerDataManager_V2.Instance.transform.position*/;
                    break;
                }
            }

            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW = LocalTransform.FromPositionRotation
            (
                randomPos, transform.ValueRO.Rotation
            );
        }

        // BufferEntity�ɑ΂���Enemy��DynamicBuffer��ǉ�
        var bufferEntity = state.EntityManager.CreateEntity();
        state.EntityManager.AddBuffer<EnemyBufferElement>(bufferEntity);
        DynamicBuffer<EnemyBufferElement> enemyBuffer = state.EntityManager.GetBuffer<EnemyBufferElement>(bufferEntity);

        foreach (var entity in instance)
        {
#if UNITY_EDITOR
            state.EntityManager.SetName(entity, "Enemy" + enemyNumber);
#endif

            enemyBuffer.Add(new EnemyBufferElement { Enemy = entity, EnemyID = enemyNumber });
            enemyNumber++;
        }

        state.Enabled = false;
    }
}
