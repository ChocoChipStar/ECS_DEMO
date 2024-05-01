using UnityEngine;

public class MainSceneSoundEffectManager_V2 : AudioManager_V2
{
    [Header("Player")]
    [SerializeField, Header("Player�̍U������SE���A�^�b�`���Ă�������")]
    private AudioClip[] PlayerAttackSE;
    /// <summary>
    /// Player�̍U������SE�Đ�
    /// </summary>
    public void PlaySEPlayerAttack() { RandomPlayOneShot(PlayerAttackSE); }

    [SerializeField, Header("Player�̉�]�U������SE���A�^�b�`���Ă�������")]
    private AudioClip[] PlayerSpinAttackSE;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEPlayerSpinAttack() { RandomPlayOneShot(PlayerSpinAttackSE); }

    [SerializeField, Header("Player�̔�_���[�W����SE���A�^�b�`���Ă�������")]
    private AudioClip[] PlayerDamagedSE;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEPlayerDamaged() { RandomPlayOneShot(PlayerDamagedSE); }

    [Header("Enemy")]

    [SerializeField, Header("Enemy�̍U������SE���A�^�b�`���Ă�������")]
    private AudioClip[] EnemyTackleSE;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEEnemyAttack() { RandomPlayOneShot(EnemyTackleSE); }

    [SerializeField, Header("Enemy�̍U������SE���A�^�b�`���Ă�������")]
    private AudioClip[] EnemySpinAttackSE;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEEnemySpinAttack() { RandomPlayOneShot(EnemySpinAttackSE); }

    [SerializeField, Header("Enemy�̔�_���[�W����SE���A�^�b�`���Ă�������")]
    private AudioClip[] EnemyDamagedSE;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEEnemyDamaged() { RandomPlayOneShot(EnemyDamagedSE); }
}
