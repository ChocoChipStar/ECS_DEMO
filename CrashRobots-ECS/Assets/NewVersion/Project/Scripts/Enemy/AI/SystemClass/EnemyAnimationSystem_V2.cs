using UnityEngine;

public class EnemyAnimationSystem_V2 : EnemyParentClass_V2
{
    [SerializeField]
    private Animator animator;

    [SerializeField, Header("âÒì]ÇÃã≠Ç≥Å@(èâë¨âÒì]ë¨ìx)")]
    private float oneActionRotationStrength;
    public float OneActionRotationStrength { get; private set; }

    [SerializeField, Header("âÒì]ÇÃí∑Ç≥Å@(ïb)")]
    private float oneActionDuration;
    public float OneActionDuration { get; private set; }

    [Tooltip("1ïbÇÃÉtÉåÅ[ÉÄêî")]
    private const float ONE_SECONDS_FRAME = 50.0f;

    public override void Initialized()
    {
        Walk(false);
        Attack(false);
    }

    private void OnEnable()
    {
        Initialized();
    }

    private void Update()
    {
        MoveAnimSystem();
        AttackAnimSystem();
    }

    private void MoveAnimSystem()
    {
        if (Script.Enemy.MoveSystem.enabled)
            Walk(true);
        else                            
            Walk(false);
    }

    private void AttackAnimSystem()
    {
        if (Script.Enemy.SspinSystem == null) return;
        if (Script.Enemy.AttackSystem.IsTackleMotion) Attack(true);
        else Attack(false);
    }

    public void Walk    (bool isActive) { animator.SetBool("IsWalk",    isActive); }

    public void Attack  (bool isActive) { animator.SetBool("IsAttack",  isActive); }

    public bool IsEndedAnim(string animName)
    {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != animName) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) return false;
        return true;
    }
}
