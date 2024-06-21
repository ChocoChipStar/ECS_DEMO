using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct BulletMovementSystem : ISystem
{
    private float3 playerPos;

    private const float DestroyBoarder = 50.0f;

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform,paramsData) in SystemAPI.Query<RefRW<LocalTransform>,RefRO<PlayerParamsData>>())
        {
            playerPos = transform.ValueRO.Position;
        }

        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (localToWorld, transform, bulletData, entity) in SystemAPI.Query<RefRO<LocalToWorld>, RefRW<LocalTransform>, RefRO<BulletParamsData>>().WithEntityAccess())
        {
            var position = transform.ValueRO.Position;
            position += bulletData.ValueRO.direction * bulletData.ValueRO.bulletSpeed * SystemAPI.Time.DeltaTime;
            transform.ValueRW.Position = position;

            if (math.distance(playerPos, transform.ValueRO.Position) >= DestroyBoarder)
            {
                // 一定距離以上離れたら弾を非アクティブにする
                entityCommandBuffer.AddComponent<Disabled>(entity);
            }
        }
    }
}
