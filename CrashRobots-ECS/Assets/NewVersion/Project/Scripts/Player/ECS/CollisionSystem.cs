using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
public partial struct CollisionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerParamsData>();
        state.RequireForUpdate<EnemyParamsData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        var enemiesJob = new EnemiesJob
        {
            bulletsGroup = SystemAPI.GetComponentLookup<BulletParamsData>(),
            enemyGroup = SystemAPI.GetComponentLookup<EnemyParamsData>(),
            entityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
        };
        state.Dependency = enemiesJob.Schedule(simulation, state.Dependency);

        JobHandle.ScheduleBatchedJobs();
    }

    [BurstCompile]
    private struct EnemiesJob : ITriggerEventsJob
    {
        public ComponentLookup<BulletParamsData> bulletsGroup;
        public ComponentLookup<EnemyParamsData> enemyGroup;

        public EntityCommandBuffer entityCommandBuffer;

        [BurstCompile]
        public void Execute(TriggerEvent triggerEvent)
        {
            // EntityA��B�ɂ͏Փ˂�����̃G���e�B�e�B���i�[����Ă���
            // ������ EntityA�͓��������Ώە� EntityB�͓��������������g

            var aIsEnemy = enemyGroup.HasComponent(triggerEvent.EntityA);
            var bIsEnemy = enemyGroup.HasComponent(triggerEvent.EntityB);

            // A��B�ǂ�������G�l�~�[�ł͂Ȃ��ꍇ���^�[������
            if (!(aIsEnemy ^ bIsEnemy))
                return;

            var aIsBullets = bulletsGroup.HasComponent(triggerEvent.EntityA);
            var bIsBullets = bulletsGroup.HasComponent(triggerEvent.EntityB);

            // A��B�ǂ�������e�ł͂Ȃ��ꍇ���^�[������
            if (!(aIsBullets ^ bIsBullets))
                return;

            // EntityA���G�l�~�[�̏ꍇ�AenemyEntity��EntityA��������
            var enemyEntity = aIsEnemy ? triggerEvent.EntityA : triggerEvent.EntityB;
            var bulletEntity = bIsBullets ? triggerEvent.EntityB : triggerEvent.EntityA;

            // Disabled�܂���Destroy���g���B
            // ��x�ɔ�\���ɂ�����@��������Ȃ��ׁA�ċA������Disabled���Destroy�̕����y��
            entityCommandBuffer.DestroyEntity(enemyEntity);
            entityCommandBuffer.AddComponent<Disabled>(bulletEntity);
        }
    }
}
