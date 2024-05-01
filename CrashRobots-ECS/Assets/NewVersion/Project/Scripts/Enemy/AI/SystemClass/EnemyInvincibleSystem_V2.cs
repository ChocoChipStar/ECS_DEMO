using System.Collections;
using UnityEngine;

public class EnemyStanSystem_V2 : EnemyParentClass_V2
{
    [SerializeField, Tooltip("無敵時間を設定してください")]
    private float stanTime = 0.5f;
    // スタン時間のタイマー
    private float stanTimeTimer = 0;

    // スタンフラグ
    private bool isStan;

    /// <summary>
    /// スタン起動
    /// </summary>
    public void EnabledStan() { isStan = true; }

    public override void Initialized()
    {
        stanTimeTimer = 0;
        isStan = false;
    }

    private void OnEnable()
    {
        Initialized();
    }

    private void OnDisable()
    {
        DisableStanBehavior();
    }

    private void Update()
    {
        // スタン中じゃ無ければreturn
        if (!isStan) return;

        StanBehavior();

        // スタン時間計測
        if (!TimerCountAndReset(stanTime, ref stanTimeTimer)) return;

        DisableStanBehavior();

        // スタン解除
        isStan = false;
    }

    /// <summary>
    /// スタン有効化
    /// </summary>
    public void StanBehavior()
    {
        // マテリアル差し替え
        Script.Enemy.MaterialManager.EnableStanMaterial();
        // 各システムオフ
        SetEachSystemAndColliderEnable(false);
        // 視点固定
        Script.Enemy.ViewPointSystem.SetEnableLockViwePoint(true);

    }

    /// <summary>
    /// スタン解除
    /// </summary>
    private void DisableStanBehavior()
    {
        // 各システムオン
        SetEachSystemAndColliderEnable(true);
        // 視点固定解除
        Script.Enemy.ViewPointSystem.SetEnableLockViwePoint(false);
    }

    /// <summary>
    /// 各関係システムオンオフ
    /// </summary>
    /// <param name="enable"> ture -> system.enabled = true , collider.enabled = true </param>
    private void SetEachSystemAndColliderEnable(bool enable)
    {
        Script.AttackSystemEnabled      (enable);
        Script.MoveSystemEnabled        (enable);
        Script.ViewPointSystemEnabled   (enable);
        Script.SpinSystemEnabled        (enable);
        Script.Enemy.ColliderSystem.EnableCollider(enable);
    }

    /// <summary>
    /// 移動禁止
    /// </summary>
    /// <param name="enableLock"></param>
    /// <returns></returns>
    private IEnumerator MoveLockSystem(bool enableLock)
    {
        while (enableLock)
        {
            var velocity = Data.Rigidbody.velocity;
            if (velocity.magnitude != 0) velocity = Vector3.zero;
            Data.Rigidbody.velocity = velocity;


            yield return null;
        }
    }

}
