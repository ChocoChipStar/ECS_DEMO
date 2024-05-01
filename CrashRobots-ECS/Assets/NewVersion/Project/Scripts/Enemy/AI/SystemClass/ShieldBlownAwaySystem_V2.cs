using UnityEngine;

public class ShieldBlownAwaySystem_V2 : EnemyParentClass_V2
{
    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private Collider collider;

    public override void Initialized()
    {
        
    }

    /// <summary>
    /// 起動時吹っ飛ばす
    /// </summary>
    /// <param name="impulsePower">            ぶっ飛ばす強さ                  </param>
    /// <param name="impulseAfterDestroyTime"> ぶっ飛んでから壊れるまでの時間  </param>
    public void BrownAwayMyselfSystem(Vector3 impulsePower,float impulseAfterDestroyTime)
    {
        rigidbody.AddForce(impulsePower, ForceMode.Impulse);    // 吹っ飛ばす
        Destroy(gameObject, impulseAfterDestroyTime);           // 自身を消滅
        collider.enabled = false;                               // 当たり判定無効化
        transform.parent = null;                                // エネミーとの親子関係解除
    }
}
