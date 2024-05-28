using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxCount = 100;

    public int maxSpawnRadius;
    public int minSpawnRadius;
}

public class ConfigBake : Baker<ConfigAuthoring>
{
    public override void Bake(ConfigAuthoring authoring)
    {
        var data = new ConfigData
        {
            EnemyPrefab = GetEntity(authoring.enemyPrefab,TransformUsageFlags.Dynamic),
            MaxCount = authoring.maxCount,
            MaxSpawnRadius = authoring.maxSpawnRadius,
            MinSpawnRadius = authoring.minSpawnRadius,
        };
        AddComponent(GetEntity(TransformUsageFlags.None), data);
    }
}

public struct ConfigData : IComponentData
{
    public Entity EnemyPrefab;
    public int MaxCount;
    public int MaxSpawnRadius;
    public int MinSpawnRadius;
}