using Unity.Entities;
using UnityEngine;

public class SpawnConfigAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxCount;
    public int maxSpawnRadius;
    public int minSpawnRadius;

    private class Baker : Baker<SpawnConfigAuthoring>
    {
        public override void Bake(SpawnConfigAuthoring authoring)
        {
            var configData = new SpawnData
            {
                EnemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
                MaxCount = authoring.maxCount,
                MaxSpawnRadius = authoring.maxSpawnRadius,
                MinSpawnRadius = authoring.minSpawnRadius,
            };
            AddComponent(GetEntity(TransformUsageFlags.None), configData);
        }
    }
}

public struct SpawnData : IComponentData
{
    public Entity EnemyPrefab;
    public int MaxCount;
    public int MaxSpawnRadius;
    public int MinSpawnRadius;
}

