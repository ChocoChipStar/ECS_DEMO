using Unity.Entities;
using UnityEngine;

public class ScoreAuthoring : MonoBehaviour
{
    private int enemyCountValue;
    private int bulletCountValue;

    private class Baker : Baker<ScoreAuthoring>
    {
        public override void Bake(ScoreAuthoring scoreAuthoring)
        {
            var data = new ScoreData
            {
                enemyCountValue = scoreAuthoring.enemyCountValue,
                bulletCountValue = scoreAuthoring.bulletCountValue
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}

public struct ScoreData : IComponentData
{
    public int enemyCountValue;
    public int bulletCountValue;

    public void SetEnemyCountText()
    {
        ScoreGameObject.instance.SetEnemyCountText(enemyCountValue);
    }

    public void SetBulletCountText()
    {
        ScoreGameObject.instance.SetBulletCountText(bulletCountValue);
    }
}
