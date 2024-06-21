using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float rotationSpeed;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var data = new PlayerParamsData()
            {
                rotationSpeed = authoring.rotationSpeed
            };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
        }
    }
}

public struct PlayerParamsData : IComponentData
{
    public float rotationSpeed;
    public float horizontal;
    public bool isPressedSpace;
}
