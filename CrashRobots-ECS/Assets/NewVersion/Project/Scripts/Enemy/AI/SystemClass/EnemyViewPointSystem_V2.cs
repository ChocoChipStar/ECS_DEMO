using UnityEngine;

public class EnemyViewPointSystem_V2 : EnemyParentClass_V2
{
    Quaternion Rotation { get { return Data.Rigidbody.rotation; } set { Data.Rigidbody.rotation = value; } }

    public override void Initialized()
    {
        enableLockViewPoint = false;
    }


    private void OnEnable()
    {
        Initialized();
    }

    private void Update()
    {
        LockViewPoint(enableLockViewPoint);
    }
        
    /// <summary>
    /// LockRotationのオンオフ
    /// </summary>
    private bool enableLockViewPoint = false;

    /// <summary>
    /// enableLockViewPointの値を設定する関数
    /// </summary>
    /// <param name="enabled"> enableLockViewPoint = enabled　とします </param>
    public void SetEnableLockViwePoint(bool enabled) { enableLockViewPoint = enabled; }

    // ロック時の回転
    private Quaternion lockRotation = Quaternion.identity;

    /// <summary>
    /// 向く方向を固定化している状態
    /// </summary>
    /// <param name="enabled"></param>
    private void LockViewPoint(bool enabled)
    {
        if (enabled)
        {
            LockRotation(true);
            LookPlayerSystem(false);
        }
        else
        {
            LockRotation(false);
            LookPlayerSystem(true);
        }
    }

    /// <summary>
    /// プレイヤーの方向を向く関数
    /// </summary>
    /// <param name="lookPlayerSystemEnabled"> true -> プレイヤー側を向く </param>
    private void LookPlayerSystem(bool lookPlayerSystemEnabled)
    {
        // フラグが立ってなければreturn
        if (!lookPlayerSystemEnabled) return;

        // Playerとの距離計算
        var distanceFromPlayer = Script.CalDistanceFromPlayer(transform.position);

        // Playerを感知してなかったらreturn
        if (!Script.Enemy.MoveSystem.IsSensing(distanceFromPlayer)) return;

        // Playerの方向を向く
        var eulerAngle = Rotation.eulerAngles;

        eulerAngle.x   = 0.0f; 
        eulerAngle.z   = 0.0f;
        eulerAngle.y   = Vec3ToAngle(distanceFromPlayer);

        Rotation       = Quaternion.Euler(eulerAngle);
    }

    /// <summary>
    /// 回転を止める関数
    /// </summary>
    /// <param name="enabled"> true -> 回転停止 </param>
    private void LockRotation(bool enabled)
    {
        if (enabled)    Rotation = lockRotation;
        else            lockRotation = Rotation;
    }
}
