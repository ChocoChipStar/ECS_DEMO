using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))] // �N���������A�b�v�f�[�g�����s����
[UpdateAfter(typeof(TitleConfigDataSystem))] // ConfigData���擾���Ă���A�b�v�f�[�g���s
public partial struct EnemySpawnSystem : ISystem
{
    private float measureTime;

    private bool isExecute;

    private SpawnConfigData spawnData;

    private TitleConfigData titleData;

    private EntityCommandBuffer entityCommandBuffer;

    private DynamicBuffer<EnemyBuffer> enemyBuffer;

    private const float SpawnInterval = 0.015f;

    private void OnCreate(ref SystemState state)
    {
        state.Enabled = true;
        state.RequireForUpdate<SpawnConfigData>();
        state.RequireForUpdate<TitleConfigData>();
    }

    [BurstCompile]
    private void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var spawnEntity = SystemAPI.GetSingletonEntity<SpawnConfigData>();
        if (!state.EntityManager.HasComponent<EnemyBuffer>(spawnEntity))
        {
            entityCommandBuffer.AddBuffer<EnemyBuffer>(spawnEntity);
        }
        else
        {
            enemyBuffer = state.EntityManager.GetBuffer<EnemyBuffer>(spawnEntity);
        }

        spawnData = SystemAPI.GetSingleton<SpawnConfigData>();
        titleData = SystemAPI.GetSingleton<TitleConfigData>();

        if (!isExecute)
        {
            state = InitializeSpawn(state);
            isExecute = true;
            return;
        }

        measureTime += SystemAPI.Time.DeltaTime;
        if (measureTime <= SpawnInterval)
        {
            return;
        }
        measureTime = 0.0f;

        EnemySpawner(state);
    }

    /// <summary> SpawnLoop���I�t�̏ꍇ�Ɉ�x�̂ݎ��s���鐶������ </summary>
    public SystemState InitializeSpawn(SystemState state)
    {
        if (!titleData.isSpawnLoop)
        {
            for (int i = 0; i < 5000; ++i)
            {
                EnemySpawner(state);
            }

            state.Enabled = false;
            return state;
        }

        return state;
    }

    /// <summary> �G�𐶐������W�̐ݒ���s���܂� </summary>
    private void EnemySpawner(SystemState state)
    {
        SearchDisabledEnemy(state);

        if (!enemyBuffer.IsEmpty)
        {
            PoolingEnemy(state);
            return;
        }

        var instance = state.EntityManager.Instantiate(spawnData.enemyPrefab);

        var transform = SystemAPI.GetComponentRW<LocalTransform>(instance);
        var enemyData = SystemAPI.GetComponentRW<EnemyParamsData>(instance);
        
        transform.ValueRW.Position = CalculateSpawnPosition();
        var randomSpeed = UnityEngine.Random.Range(enemyData.ValueRO.moveMinSpeed, enemyData.ValueRO.moveMaxSpeed);
        enemyData.ValueRW.moveSpeed = randomSpeed;
    }

    /// <summary> ��A�N�e�B�u��Ԃ̃G�l�~�[���ăA�N�e�B�u�����ď��������܂� </summary>
    private void PoolingEnemy(SystemState state)
    {
        var poolEntity = enemyBuffer[0].entity;

        var poolTransform = SystemAPI.GetComponentRW<LocalTransform>(poolEntity);
        poolTransform.ValueRW.Position = CalculateSpawnPosition();

        entityCommandBuffer.RemoveComponent<Disabled>(poolEntity);
        enemyBuffer.RemoveAt(0);
    }

    /// <summary> ��A�N�e�B�u��Ԃ̃G�l�~�[��T�����Ĕz��ɑ�����܂� </summary>
    private void SearchDisabledEnemy(SystemState state)
    {
        foreach (var (enemyData, entity) in SystemAPI.Query<RefRW<EnemyParamsData>>().WithEntityAccess().WithAll<Disabled>())
        {
            if (!SystemAPI.HasComponent<Disabled>(entity))
            {
                continue;
            }

            if(!IsEntityInEnemyBuffer(entity))
            {
                enemyBuffer.Add(new EnemyBuffer() { entity = entity });
            } 
        }
    }

    /// <summary> enemyBuffer ���ɓ����G���e�B�e�B�����݂��邩�m�F���܂� </summary>
    private bool IsEntityInEnemyBuffer(Entity entity)
    {
        for (int i = 0; i < enemyBuffer.Length; i++)
        {
            if (enemyBuffer[i].entity == entity)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary> �G�̐������W���v�Z���܂� </summary>
    /// <returns> �������W��Ԃ��܂��ifloat3�j </returns>
    private float3 CalculateSpawnPosition()
    {
        // �h�[�i�c�̓����ƊO���̍��W�����߂�
        var maxRadius = Mathf.Pow(spawnData.maxSpawnRadius, 2);
        var minRadius = Mathf.Pow(spawnData.minSpawnRadius, 2);

        for (int i = 0; i < 100; i++)
        {
            // �~�͈͓̔����烉���_�����W���v�Z����
            var randomX = UnityEngine.Random.Range(-spawnData.maxSpawnRadius, spawnData.maxSpawnRadius);
            var randomZ = UnityEngine.Random.Range(-spawnData.maxSpawnRadius, spawnData.maxSpawnRadius);

            // ���S�n�_����ǂ̂��炢���ꂽ�����ɒ��I���ꂽ�����ׂ�
            var xAbs = Math.Pow(randomX, 2);
            var zAbs = Math.Pow(randomZ, 2);

            // �ŏ����a���O���ōő唼�a�������ɂ��邩�𒲂ׂ�
            if (maxRadius > xAbs + zAbs && xAbs + zAbs > minRadius)
            {
                return new float3(randomX, 0.0f, randomZ);
            }
        }

        return float3.zero;
    }
}
