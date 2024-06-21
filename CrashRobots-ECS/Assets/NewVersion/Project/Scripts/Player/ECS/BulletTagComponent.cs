using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletTagComponent : MonoBehaviour
{
    public float bulletSpeed;

    private class Baker : Baker<BulletTagComponent>
    {
        public override void Bake(BulletTagComponent authoring)
        {
            var paramsData = new BulletParamsData
            {
                bulletSpeed = authoring.bulletSpeed,
            };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), paramsData);
        }
    }
}

public struct BulletParamsData : IComponentData 
{
    public float bulletSpeed;
    public float3 direction;
};