using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (paramsData, transform) in SystemAPI.Query<RefRW<PlayerParamsData>, RefRW<LocalTransform>>())
        {
            transform.ValueRW.Rotation = math.mul
            (
                transform.ValueRW.Rotation, quaternion.RotateY(paramsData.ValueRO.horizontal * paramsData.ValueRO.rotationSpeed)
            );
        }
    }
}
