using System;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;


[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct EnemySpawnSystem : ISystem
{
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
        var randomPos = new Vector3();

        foreach (var entity in instance)
        {
            // 中心点プレイヤー座標から円状にランダム配置
            for (int i = 0; i < 100; ++i)
            {
                var x = random.Next(-config.MaxSpawnRadius, config.MaxSpawnRadius);
                var z = random.Next(-config.MinSpawnRadius, config.MinSpawnRadius);

                var xAbs = Math.Abs(Math.Pow(x, 2));
                var zAbs = Math.Abs(Math.Pow(z, 2));

                if (maxRadius > xAbs + zAbs && xAbs + zAbs > minRadius)
                {
                    randomPos = new Vector3(x, 0.0f, z) + PlayerDataManager_V2.Instance.transform.position;
                    break;
                }
            }

            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);

            transform.ValueRW = LocalTransform.FromPositionRotation
            (
                new Vector3(randomPos.x, 0, randomPos.z),transform.ValueRO.Rotation
            );
        }

        state.Enabled = false;
    }
}
