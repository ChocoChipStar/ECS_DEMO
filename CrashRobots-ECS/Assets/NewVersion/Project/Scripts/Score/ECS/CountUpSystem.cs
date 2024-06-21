using Unity.Entities;
using Unity.Transforms;

public partial struct CountUpSystem : ISystem
{
    private int enemyCount;
    private int bulletCount;

    private void OnUpdate(ref SystemState state)
    {
        enemyCount = 0;
        foreach(var (enemyData, entity) in SystemAPI.Query<RefRO<EnemyParamsData>>().WithEntityAccess())
        {
            enemyCount++;
        }

        bulletCount = 0;
        foreach(var (bulletData, entity) in SystemAPI.Query<RefRO<BulletParamsData>>().WithEntityAccess())
        {
            bulletCount++;
        }

        foreach(var scoreData in SystemAPI.Query<RefRW<ScoreData>>())
        {
            scoreData.ValueRW.enemyCountValue = enemyCount;
            scoreData.ValueRW.SetEnemyCountText();

            scoreData.ValueRW.bulletCountValue = bulletCount;
            scoreData.ValueRW.SetBulletCountText();
        }
    }
}
