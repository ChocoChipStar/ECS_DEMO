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
            // EntityAとBには衝突した二つのエンティティが格納されている
            // ※推測 EntityAは当たった対象物 EntityBは当たった自分自身

            var aIsEnemy = enemyGroup.HasComponent(triggerEvent.EntityA);
            var bIsEnemy = enemyGroup.HasComponent(triggerEvent.EntityB);

            // AとBどちらもがエネミーではない場合リターンする
            if (!(aIsEnemy ^ bIsEnemy))
                return;

            var aIsBullets = bulletsGroup.HasComponent(triggerEvent.EntityA);
            var bIsBullets = bulletsGroup.HasComponent(triggerEvent.EntityB);

            // AとBどちらもが弾ではない場合リターンする
            if (!(aIsBullets ^ bIsBullets))
                return;

            // EntityAがエネミーの場合、enemyEntityにEntityAを代入する
            var enemyEntity = aIsEnemy ? triggerEvent.EntityA : triggerEvent.EntityB;
            var bulletEntity = bIsBullets ? triggerEvent.EntityB : triggerEvent.EntityA;

            // DisabledまたはDestroyを使う。
            // 一度に非表示にする方法が見つからない為、再帰処理のDisabledよりDestroyの方が軽い
            entityCommandBuffer.DestroyEntity(enemyEntity);
            entityCommandBuffer.AddComponent<Disabled>(bulletEntity);
        }
    }
}
