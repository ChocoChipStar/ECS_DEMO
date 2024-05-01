using UnityEngine;
using PlayerData = PlayerDataManager_V2;
using PlayerScript = PlayerScriptManager_V2;


public class EnemyHitPointSystem_V2 : EnemyParentClass_V2
{
    [SerializeField, Header("耐久力を設定してください")]
    private int hitPoint = 3;

    public override void Initialized() { }

    private void OnEnable()
    {
        Initialized();
    }

    // ダメージを受けた時の挙動
    public void DamegeSystem()
    {
        Script.Enemy.StanSystem.EnabledStan();  // 無敵状態にする
        StartCoroutine(Script.Enemy.MaterialManager.EnableDamageMaterial()); // DamageMaterial起動
        if (IsShield()) return;         // Shieldがあるならreturn
        hitPoint--;                     // HPを減らす
        BecomeZeroHitPoint(hitPoint);   // 0HP挙動
    }

    // Shieldがあるときtrue
    private bool IsShield()
    {
        if (Script.Enemy.ShieldSystem == null) return false;
        Script.Enemy.ShieldSystem.DamegeSystem(out var shieldHP);
        if (shieldHP < 0) return false;
        return true;
    }

    // 0HP挙動
    private void BecomeZeroHitPoint(int hitPoint)
    {
        if (hitPoint > 0) return;     // HPが0でないならreturn

        var socoreManager = ScoreManager_V2.Instance;
        socoreManager.AddKillCount();    // キル数カウント
        socoreManager.AddScore(100);     // スコア加算

        // ヒットストップ発生
        var playerData = PlayerData.Instance;
        var playerScript = PlayerScript.Instance;
        StartCoroutine(playerScript.HitStopSystem.HitStop(playerData.DamageHitStopDuration, playerData.DamageHitStopStrengh, null));

        // カメラ揺れ発生
        playerScript.CameraSystem.StartCoroutine(
            playerScript.CameraSystem.ShakeSystem(playerData.DamageShakeDuration, playerData.DamageShakeStrength)
            );

        // 爆発する
        Script.Enemy.DestroySystem.ExplodeDestroyEnemy();
    }
}
