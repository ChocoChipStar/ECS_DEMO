using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct EnemySpawnSystem : ISystem
{
    private float measureTime;

    private SpawnData config;

    private const float SpawnInterval = 0.01f;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //measureTime += SystemAPI.Time.DeltaTime;
        //if (measureTime <= SpawnInterval)
        //{
        //    return;  
        //}
        //measureTime = 0.0f;

        foreach (var configData in SystemAPI.Query<RefRO<SpawnData>>())
        {
            config = configData.ValueRO;
        }

        for(int i = 0; i < config.MaxCount; ++i)
        {
            // �h�[�i�c�̓����ƊO���̍��W�����߂�
            var maxRadius = Mathf.Pow(config.MaxSpawnRadius, 2);
            var minRadius = Mathf.Pow(config.MinSpawnRadius, 2);

            var randomPos = new float3();
            for (int j = 0; j < 100; ++j)
            {
                // �~�͈͓̔����烉���_�����W���v�Z����
                var randomX = UnityEngine.Random.Range(-config.MaxSpawnRadius, config.MaxSpawnRadius);
                var randomZ = UnityEngine.Random.Range(-config.MaxSpawnRadius, config.MaxSpawnRadius);

                // ���S�n�_����ǂ̂��炢���ꂽ�����ɒ��I���ꂽ�����ׂ�
                var xAbs = Math.Pow(randomX, 2);
                var zAbs = Math.Pow(randomZ, 2);

                // �ŏ����a�������ɂ��邩�𒲂ׂ�
                if (maxRadius > xAbs + zAbs && xAbs + zAbs > minRadius)
                {
                    randomPos = new float3(randomX, 0.0f, randomZ);
                    break;
                }
            }

            var instance = state.EntityManager.Instantiate(config.EnemyPrefab);

            // �������W�ݒ�
            var transform = SystemAPI.GetComponentRW<LocalTransform>(instance);
            transform.ValueRW = LocalTransform.FromPositionRotation
            (
                randomPos, transform.ValueRO.Rotation
            );
        }

        state.Enabled = false;
    }
}
