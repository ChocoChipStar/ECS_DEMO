using UnityEngine;

public class MainSceneSoundEffectManager_V2 : AudioManager_V2
{
    [Header("Player")]
    [SerializeField, Header("Playerの攻撃時のSEをアタッチしてください")]
    private AudioClip[] PlayerAttackSE;
    /// <summary>
    /// Playerの攻撃時のSE再生
    /// </summary>
    public void PlaySEPlayerAttack() { RandomPlayOneShot(PlayerAttackSE); }

    [SerializeField, Header("Playerの回転攻撃時のSEをアタッチしてください")]
    private AudioClip[] PlayerSpinAttackSE;
    /// <summary>
    /// Playerの被ダメージ時のSE再生
    /// </summary>
    public void PlaySEPlayerSpinAttack() { RandomPlayOneShot(PlayerSpinAttackSE); }

    [SerializeField, Header("Playerの被ダメージ時のSEをアタッチしてください")]
    private AudioClip[] PlayerDamagedSE;
    /// <summary>
    /// Playerの被ダメージ時のSE再生
    /// </summary>
    public void PlaySEPlayerDamaged() { RandomPlayOneShot(PlayerDamagedSE); }

    [Header("Enemy")]

    [SerializeField, Header("Enemyの攻撃時のSEをアタッチしてください")]
    private AudioClip[] EnemyTackleSE;
    /// <summary>
    /// Playerの被ダメージ時のSE再生
    /// </summary>
    public void PlaySEEnemyAttack() { RandomPlayOneShot(EnemyTackleSE); }

    [SerializeField, Header("Enemyの攻撃時のSEをアタッチしてください")]
    private AudioClip[] EnemySpinAttackSE;
    /// <summary>
    /// Playerの被ダメージ時のSE再生
    /// </summary>
    public void PlaySEEnemySpinAttack() { RandomPlayOneShot(EnemySpinAttackSE); }

    [SerializeField, Header("Enemyの被ダメージ時のSEをアタッチしてください")]
    private AudioClip[] EnemyDamagedSE;
    /// <summary>
    /// Playerの被ダメージ時のSE再生
    /// </summary>
    public void PlaySEEnemyDamaged() { RandomPlayOneShot(EnemyDamagedSE); }
}
