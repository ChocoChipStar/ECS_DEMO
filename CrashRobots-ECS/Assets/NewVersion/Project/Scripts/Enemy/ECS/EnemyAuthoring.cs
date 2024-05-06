using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float moveSpeed;

    public float attackDamage;
    public float attackRange;
}

public class EnemyBake : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        var paramsData = new ParamsData
        {
            MoveSpeed = authoring.moveSpeed,
            AttackDamage = authoring.attackDamage,
            AttackRange = authoring.attackRange,
        };
        AddComponent(GetEntity(TransformUsageFlags.None), paramsData);
    }
}

public struct ParamsData : IComponentData
{
    public float MoveSpeed;

    public float AttackDamage;
    public float AttackRange;   
}