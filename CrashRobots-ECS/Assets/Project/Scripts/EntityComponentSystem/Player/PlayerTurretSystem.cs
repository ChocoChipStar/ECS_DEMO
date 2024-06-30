using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerTurretSystem : ISystem
{
    private float measureTime;

    private BulletData bulletData;

    private DynamicBuffer<BulletBuffer> bulletBuffer;

    private EntityCommandBuffer entityCommandBuffer;

    private const float FiringInterval = 0.025f;

    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletData>();
        state.RequireForUpdate<PlayerParamsData>();
    }

    [BurstCompile]
    private void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var bulletEntity = SystemAPI.GetSingletonEntity<BulletData>();
        if (!state.EntityManager.HasComponent<BulletBuffer>(bulletEntity))
        {
            entityCommandBuffer.AddBuffer<BulletBuffer>(bulletEntity);
        }
        else
        {
            bulletBuffer = state.EntityManager.GetBuffer<BulletBuffer>(bulletEntity);
        }

        var playerData = SystemAPI.GetSingleton<PlayerParamsData>();
        if (!playerData.isPressedSpace)
        {
            return;
        }

        measureTime += SystemAPI.Time.DeltaTime;
        if (measureTime <= FiringInterval)
        {
            return;
        }
        measureTime = 0.0f;

        bulletData = SystemAPI.GetSingleton<BulletData>();
        var transform = SystemAPI.GetComponentRW<LocalTransform>(bulletEntity);

        // ��A�N�e�B�u��Ԃ�entity��BulletBuffer�ɑ��
        if (SystemAPI.HasComponent<Disabled>(bulletEntity))
        {
            if(!IsEntityInBulletBuffer(bulletEntity))
            {
                bulletBuffer.Add(new BulletBuffer() { entity = bulletEntity });
            }
        }

        SpawnBullet(state);
    }

    /// <summary> bulletBuffer ���ɓ����G���e�B�e�B�����݂��邩�m�F���܂� </summary>
    private bool IsEntityInBulletBuffer(Entity entity)
    {
        for (int i = 0; i < bulletBuffer.Length; i++)
        {
            if (bulletBuffer[i].entity == entity)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary> �e�̐����������s���܂� </summary>
    private void SpawnBullet(SystemState state)
    {
        foreach (var worldTransform in SystemAPI.Query<RefRO<LocalToWorld>>().WithAll<GunPortTag>())
        {
            if (!bulletBuffer.IsEmpty)
            {
                var poolEntity = bulletBuffer[0].entity;

                // �A�N�e�B�u��Ԃɂ���BulletBuffer����폜
                entityCommandBuffer.RemoveComponent<Disabled>(poolEntity);
                bulletBuffer.RemoveAt(0);

                InitializedBulletPosition(state, poolEntity, worldTransform);
                continue;
            }

            // �e�𐶐��i��A�N�e�B�u��Ԃ�entity����̂����Ȃ��ꍇ�̂݁j
            var instance = state.EntityManager.Instantiate(bulletData.bulletEntity);
            InitializedBulletPosition(state, instance, worldTransform);
        }
    }

    /// <summary> �G���e�B�e�B�̏������W��ݒ肵�܂� </summary>
    private void InitializedBulletPosition(SystemState state, Entity entity, RefRO<LocalToWorld> worldTransform)
    {
        // �������W�ݒ�
        var instanceTransform = SystemAPI.GetComponentRW<LocalTransform>(entity);
        instanceTransform.ValueRW.Position = worldTransform.ValueRO.Position;
        instanceTransform.ValueRW.Rotation = quaternion.EulerXYZ(new float3(0, math.radians(90), 0));

        // �e�ۂ̕�����ݒ�
        var direction = math.normalize(worldTransform.ValueRO.Position - new float3(0, 0, 0));
        var instanceEntityData = SystemAPI.GetComponentRW<BulletParamsData>(entity);
        instanceEntityData.ValueRW.direction = new float3(direction.x, 0.0f, direction.z);
    }
}
