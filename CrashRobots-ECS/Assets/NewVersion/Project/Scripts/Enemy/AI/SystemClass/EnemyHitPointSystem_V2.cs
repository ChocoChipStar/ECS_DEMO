using UnityEngine;
using PlayerData = PlayerDataManager_V2;
using PlayerScript = PlayerScriptManager_V2;


public class EnemyHitPointSystem_V2 : EnemyParentClass_V2
{
    [SerializeField, Header("�ϋv�͂�ݒ肵�Ă�������")]
    private int hitPoint = 3;

    public override void Initialized() { }

    private void OnEnable()
    {
        Initialized();
    }

    // �_���[�W���󂯂����̋���
    public void DamegeSystem()
    {
        Script.Enemy.StanSystem.EnabledStan();  // ���G��Ԃɂ���
        StartCoroutine(Script.Enemy.MaterialManager.EnableDamageMaterial()); // DamageMaterial�N��
        if (IsShield()) return;         // Shield������Ȃ�return
        hitPoint--;                     // HP�����炷
        BecomeZeroHitPoint(hitPoint);   // 0HP����
    }

    // Shield������Ƃ�true
    private bool IsShield()
    {
        if (Script.Enemy.ShieldSystem == null) return false;
        Script.Enemy.ShieldSystem.DamegeSystem(out var shieldHP);
        if (shieldHP < 0) return false;
        return true;
    }

    // 0HP����
    private void BecomeZeroHitPoint(int hitPoint)
    {
        if (hitPoint > 0) return;     // HP��0�łȂ��Ȃ�return

        var socoreManager = ScoreManager_V2.Instance;
        socoreManager.AddKillCount();    // �L�����J�E���g
        socoreManager.AddScore(100);     // �X�R�A���Z

        // �q�b�g�X�g�b�v����
        var playerData = PlayerData.Instance;
        var playerScript = PlayerScript.Instance;
        StartCoroutine(playerScript.HitStopSystem.HitStop(playerData.DamageHitStopDuration, playerData.DamageHitStopStrengh, null));

        // �J�����h�ꔭ��
        playerScript.CameraSystem.StartCoroutine(
            playerScript.CameraSystem.ShakeSystem(playerData.DamageShakeDuration, playerData.DamageShakeStrength)
            );

        // ��������
        Script.Enemy.DestroySystem.ExplodeDestroyEnemy();
    }
}
