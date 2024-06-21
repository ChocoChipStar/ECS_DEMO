using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct PlayerTurretSystem : ISystem
{
    private float measureTime;

    private Entity bulletEntity;

    private ComponentLookup<BulletParamsData> bulletsGroup;

    private DynamicBuffer<BulletBuffer> bulletBuffer;

    private const float FiringInterval = 0.025f;

    private void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (bulletData, entity) in SystemAPI.Query<RefRW<BulletData>>().WithEntityAccess())
        {
            bulletEntity = bulletData.ValueRO.bulletEntity;

            entityCommandBuffer.AddBuffer<BulletBuffer>(entity);
            if(state.EntityManager.HasComponent<BulletBuffer>(entity))
            {
                // entityがBulletBufferを持っていたらbulletBuffer変数に代入
                bulletBuffer = state.EntityManager.GetBuffer<BulletBuffer>(entity);
            }
        }

        foreach(var paramsData in SystemAPI.Query<RefRO<PlayerParamsData>>())
        {
            if (!paramsData.ValueRO.isPressedSpace)
            {
                // スペースキーが押されていなかったらリターンする
                return;
            }
        }

        measureTime += SystemAPI.Time.DeltaTime;
        if (measureTime <= FiringInterval)
        {
            // 発射間隔時間未満の場合はリターンする
            return;
        }
        measureTime = 0.0f;

        foreach (var (bulletTrans, paramsData, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BulletParamsData>>().WithEntityAccess().WithAll<Disabled>())
        {
            if (SystemAPI.HasComponent<Disabled>(entity))
            {
                // 非アクティブ状態のentityをBulletBufferに代入
                bulletBuffer.Add(new BulletBuffer() { entity = entity });
            }
        }

        foreach (var worldTransform in SystemAPI.Query<RefRO<LocalToWorld>>().WithAll<GunPortTag>())
        {
            if (bulletBuffer.Length != 0)
            {
                var poolEntity = bulletBuffer[0].entity;

                // アクティブ状態にしてBulletBufferから削除
                entityCommandBuffer.RemoveComponent<Disabled>(poolEntity);
                bulletBuffer.RemoveAt(0);

                var poolTransform = SystemAPI.GetComponentRW<LocalTransform>(poolEntity);
                var poolEntityData = SystemAPI.GetComponentRW<BulletParamsData>(poolEntity);
                InitializedBulletPosition(worldTransform, poolTransform, poolEntityData);

                continue;
            }

            // 弾を生成（非アクティブ状態のentityが一体もいない場合のみ）
            var instance = state.EntityManager.Instantiate(bulletEntity);
            var instanceTransform = SystemAPI.GetComponentRW<LocalTransform>(instance);
            var instanceEntityData = SystemAPI.GetComponentRW<BulletParamsData>(instance);
            InitializedBulletPosition(worldTransform, instanceTransform, instanceEntityData);
        }
    }

    /// <summary> エンティティの初期座標を設定します </summary>
    private void InitializedBulletPosition(RefRO<LocalToWorld> worldTransform, RefRW<LocalTransform> transform, RefRW<BulletParamsData> paramsData)
    {
        // 初期座標設定
        transform.ValueRW.Position = worldTransform.ValueRO.Position;
        transform.ValueRW.Rotation = quaternion.EulerXYZ(new float3(0, math.radians(90), 0));

        // 弾丸の方向を設定
        var direction = math.normalize(worldTransform.ValueRO.Position - new float3(0, 0, 0));
        paramsData.ValueRW.direction = new float3(direction.x, 0.0f, direction.z);
    }
}
