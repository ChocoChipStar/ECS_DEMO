using Unity.Entities;
using UnityEngine;

public class GunPortAuthoring : MonoBehaviour 
{
    private class Baker : Baker<GunPortAuthoring>
    {
        public override void Bake(GunPortAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new GunPortTag());
        }
    }
}

public struct GunPortTag : IComponentData { };
