using Unity.Entities;
using UnityEngine;

public class SpawnConfigAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxSpawnRadius;
    public int minSpawnRadius;

    private class Baker : Baker<SpawnConfigAuthoring>
    {
        public override void Bake(SpawnConfigAuthoring authoring)
        {
            var spawnConfigData = new SpawnConfigData
            {
                enemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
                maxSpawnRadius = authoring.maxSpawnRadius,
                minSpawnRadius = authoring.minSpawnRadius,
            };
            AddComponent(GetEntity(TransformUsageFlags.None), spawnConfigData);
        }
    }
}

public struct SpawnConfigData : IComponentData
{
    public Entity enemyPrefab;
    public int maxSpawnRadius;
    public int minSpawnRadius;
}

[InternalBufferCapacity(1000)]
public struct EnemyBuffer : IBufferElementData
{
    public Entity entity;
}

