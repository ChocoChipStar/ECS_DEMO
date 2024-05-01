using UnityEngine;

public class EnemyShieldSystem_V2 : EnemyParentClass_V2
{
    [SerializeField] private GameObject noRigitbodyShield;
    [SerializeField] private GameObject rigitbodyShield;

    [SerializeField, Header("Shieldの耐久力を設定してください")]
    private int setShieldHitPoint = 1;
    // 変動させるHP
    private int shieldHitPoint = 0;
    // Startでのみ実行   初期設定false
    private bool executionOneTime = false;

    [SerializeField, Header("ぶっ飛ばす力")]
    private float impulsePower = 10.0f;

    [SerializeField, Header("ぶっ飛ばして残しておく時間")]
    private float impulseAfterDestroyTime = 3.0f;

    /// <summary>
    /// Start時のみ初期化
    /// </summary>
    private void StartInitialized()
    {
        if (executionOneTime) return;
        executionOneTime = true;

        shieldHitPoint = setShieldHitPoint;
        noRigitbodyShield.SetActive(true);
        rigitbodyShield.  SetActive(false);
    }

    public override void Initialized()
    {
        StartInitialized();
    }

    private void OnEnable()
    {
        Initialized();
    }

    /// <summary>
    /// シールドにダメージを与える
    /// </summary>
    public void DamegeSystem(out int shieldHitPoint)
    {
        this.shieldHitPoint--;
        shieldHitPoint = this.shieldHitPoint;
        if (this.shieldHitPoint > 0) return;
        BlowAwaySystem();
    }

    /// <summary>
    /// ぶっ飛ばす
    /// </summary>
    private void BlowAwaySystem()
    {
        if (noRigitbodyShield == null) return;
        if (rigitbodyShield   == null) return;

        noRigitbodyShield.SetActive(false);
        rigitbodyShield  .SetActive(true);

        var impulseVec = -transform.forward.normalized;
        var impulsePower = this.impulsePower * impulseVec;
        rigitbodyShield.GetComponent<ShieldBlownAwaySystem_V2>().BrownAwayMyselfSystem(impulsePower, impulseAfterDestroyTime);
    }
}
