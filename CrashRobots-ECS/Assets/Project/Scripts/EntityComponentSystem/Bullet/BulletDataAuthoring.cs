using Unity.Entities;
using UnityEngine;

public class BulletDataAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;

    private class Baker : Baker<BulletDataAuthoring>
    {
        public override void Bake(BulletDataAuthoring authoring)
        {
            var bulletData = new PrefabData
            {
                bulletPrefabEntity = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
            };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), bulletData);
        }
    }
}

public struct BulletDataTAG : IComponentData { }

public struct PrefabData : IComponentData 
{
    public Entity bulletPrefabEntity;
};

[InternalBufferCapacity(1000)]
public struct BulletBuffer : IBufferElementData
{
    public Entity entity;
}
