using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemyMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (paramsData,transform,physicsVelocity) in SystemAPI.Query<RefRO<ParamsData>,RefRW<LocalTransform>,RefRW<PhysicsVelocity>>())
        {
            // 現在の座標を取得
            var myPos = transform.ValueRO.Position;
            var velocity = physicsVelocity.ValueRO.Linear;
            // プレイヤーインスタンス取得
            float3 playerPos = PlayerDataManager_V2.Instance.transform.position;

            var direction = playerPos - myPos;
            var radian = Mathf.Atan2(-direction.z, direction.x);
            var degree = radian * Mathf.Rad2Deg + 90.0f;

            velocity = new Vector3(direction.x, 0.0f, direction.z) * paramsData.ValueRO.MoveSpeed;

            if (physicsVelocity.ValueRO.Linear.y < 0.0f)
            {
                velocity.y = physicsVelocity.ValueRO.Linear.y;
            }

            physicsVelocity.ValueRW.Linear = velocity;
            physicsVelocity.ValueRW.Angular = Vector3.zero;
            transform.ValueRW.Rotation = Quaternion.Euler(0.0f, degree, 0.0f);

            
        }
    }
}
