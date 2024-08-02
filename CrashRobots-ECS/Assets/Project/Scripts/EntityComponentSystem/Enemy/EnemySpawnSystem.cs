using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(TitleConfigDataSystem))] // ConfigDataを取得してからアップデート実行
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
        state.RequireForUpdate<SpawnConfigData>();
        state.RequireForUpdate<TitleConfigData>();
    }

    private void OnStartRunning(ref SystemState state)
    {
        // CommandBuffer作成のために必要なシングルトンを取得する
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        // ecbSingletonからアンマネージドのentityCommandBufferを作成する

        // entityCommandBufferとは
        // 「エンティティの作成」など、Job処理中に実行できない命令を保持するバッファのこと
        // チャンク構造の変更が必要な命令は全て、Job処理中に実行できない命令など
        entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
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

        SearchDisabledEnemy(state);
        //EnemySpawner(state);

        var spawnJob = new EnemySpawnJob
        {

        };
        state.Dependency = spawnJob.Schedule(state.Dependency);
        JobHandle.ScheduleBatchedJobs();
    }

    private partial struct EnemySpawnJob : IJobEntity
    {
        private void Execute()
        {
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
    }

    /// <summary> SpawnLoopがオフの場合に一度のみ実行する生成処理 </summary>
    [BurstCompile]
    public SystemState InitializeSpawn(SystemState state)
    {
        if (!titleData.isSpawnLoop)
        {
            for (int i = 0; i < titleData.spawnCount; ++i)
            {
                EnemySpawner(state);
            }

            state.Enabled = false;
            return state;
        }

        return state;
    }

    /// <summary> 敵を生成し座標の設定を行います </summary>
    [BurstCompile]
    private void EnemySpawner(SystemState state)
    {

    }

    /// <summary> 非アクティブ状態のエネミーを再アクティブ化して初期化します </summary>
    private void PoolingEnemy(SystemState state)
    {
        var poolEntity = enemyBuffer[0].entity;

        var poolTransform = SystemAPI.GetComponentRW<LocalTransform>(poolEntity);
        poolTransform.ValueRW.Position = CalculateSpawnPosition();

        entityCommandBuffer.RemoveComponent<Disabled>(poolEntity);
        enemyBuffer.RemoveAt(0);
    }

    /// <summary> 非アクティブ状態のエネミーを探索して配列に代入します </summary>
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

    /// <summary> enemyBuffer 内に同じエンティティが存在するか確認します </summary>
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

    /// <summary> 敵の生成座標を計算します </summary>
    /// <returns> 生成座標を返します（float3） </returns>
    private float3 CalculateSpawnPosition()
    {
        // ドーナツの内側と外側の座標を決める
        var maxRadius = Mathf.Pow(spawnData.maxSpawnRadius, 2);
        var minRadius = Mathf.Pow(spawnData.minSpawnRadius, 2);

        for (int i = 0; i < 100; i++)
        {
            // 円の範囲内からランダム座標を計算する
            var randomX = UnityEngine.Random.Range(-spawnData.maxSpawnRadius, spawnData.maxSpawnRadius);
            var randomZ = UnityEngine.Random.Range(-spawnData.maxSpawnRadius, spawnData.maxSpawnRadius);

            // 中心地点からどのくらい離れた距離に抽選されたか調べる
            var xAbs = Math.Pow(randomX, 2);
            var zAbs = Math.Pow(randomZ, 2);

            // 最小半径より外側で最大半径より内側にいるかを調べる
            if (maxRadius > xAbs + zAbs && xAbs + zAbs > minRadius)
            {
                return new float3(randomX, 0.0f, randomZ);
            }
        }

        return float3.zero;
    }
}
