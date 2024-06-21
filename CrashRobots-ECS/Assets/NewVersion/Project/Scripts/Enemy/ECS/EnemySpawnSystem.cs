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
            // ドーナツの内側と外側の座標を決める
            var maxRadius = Mathf.Pow(config.MaxSpawnRadius, 2);
            var minRadius = Mathf.Pow(config.MinSpawnRadius, 2);

            var randomPos = new float3();
            for (int j = 0; j < 100; ++j)
            {
                // 円の範囲内からランダム座標を計算する
                var randomX = UnityEngine.Random.Range(-config.MaxSpawnRadius, config.MaxSpawnRadius);
                var randomZ = UnityEngine.Random.Range(-config.MaxSpawnRadius, config.MaxSpawnRadius);

                // 中心地点からどのくらい離れた距離に抽選されたか調べる
                var xAbs = Math.Pow(randomX, 2);
                var zAbs = Math.Pow(randomZ, 2);

                // 最小半径より内側にいるかを調べる
                if (maxRadius > xAbs + zAbs && xAbs + zAbs > minRadius)
                {
                    randomPos = new float3(randomX, 0.0f, randomZ);
                    break;
                }
            }

            var instance = state.EntityManager.Instantiate(config.EnemyPrefab);

            // 初期座標設定
            var transform = SystemAPI.GetComponentRW<LocalTransform>(instance);
            transform.ValueRW = LocalTransform.FromPositionRotation
            (
                randomPos, transform.ValueRO.Rotation
            );
        }

        state.Enabled = false;
    }
}
