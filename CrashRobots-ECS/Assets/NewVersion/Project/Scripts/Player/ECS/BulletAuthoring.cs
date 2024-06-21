using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;

    private class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var bulletData = new BulletData
            {
                bulletEntity = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                bulletSpeed = authoring.bulletSpeed,
            };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), bulletData);
        }
    }
}

public struct BulletData : IComponentData 
{
    public Entity bulletEntity;
    public float bulletSpeed;
};

[InternalBufferCapacity(1000)]
public struct BulletBuffer : IBufferElementData
{
    public Entity entity;
}
