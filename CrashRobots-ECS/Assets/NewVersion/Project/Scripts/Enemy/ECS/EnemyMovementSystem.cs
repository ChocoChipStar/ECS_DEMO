using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct EnemyMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (paramsData,transform) in SystemAPI.Query<RefRO<ParamsData>,RefRW<LocalTransform>>())
        {
            // 現在の座標を取得
            var myPos = transform.ValueRO.Position;
            // プレイヤーインスタンス取得
            float3 playerPos = PlayerDataManager_V2.Instance.transform.position;

            var direction = playerPos - myPos;
            var radian = Mathf.Atan2(-direction.z, direction.x);
            var degree = radian * Mathf.Rad2Deg + 90.0f; 

            myPos += direction * paramsData.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;
            
            transform.ValueRW.Position = myPos;
            transform.ValueRW.Rotation = Quaternion.Euler(0.0f, degree, 0.0f);
        }
    }
}
