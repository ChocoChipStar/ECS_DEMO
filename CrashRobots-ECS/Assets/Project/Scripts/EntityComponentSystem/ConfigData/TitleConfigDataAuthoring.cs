using JetBrains.Annotations;
using Unity.Entities;
using UnityEngine;

public class TitleConfigDataAuthoring : MonoBehaviour
{
    private int spawnCount;
    private bool isUseEffects;
    private bool isDisplayEntitiesCount;
    private bool isSpawnLoop;

    private class Baker : Baker<TitleConfigDataAuthoring>
    {
        public override void Bake(TitleConfigDataAuthoring authoring)
        {
            var titleConfigData = new TitleConfigData
            {
                spawnCount = authoring.spawnCount,
                isUseEffects = authoring.isUseEffects,
                isDisplayEntitiesCount = authoring.isDisplayEntitiesCount,
                isSpawnLoop = authoring.isSpawnLoop
            };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), titleConfigData);
        }
    }
}

public struct TitleConfigData : IComponentData
{
    public int spawnCount;

    public bool isUseEffects;
    public bool isDisplayEntitiesCount;
    public bool isSpawnLoop;
}
