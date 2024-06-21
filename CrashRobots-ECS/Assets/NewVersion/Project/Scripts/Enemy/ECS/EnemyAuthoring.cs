using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float moveSpeed;

    private class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var paramsData = new EnemyParamsData
            {
                MoveSpeed = authoring.moveSpeed,
            };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), paramsData);
        }
    }
}

public struct EnemyParamsData : IComponentData
{
    public float MoveSpeed;
}