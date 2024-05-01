using UnityEngine;

public class EnemyAnimationSystem : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [Tooltip("åªç›ÇÃâÒì]îªíË")]
    private bool isRotation = false;
    public bool IsRotation { get; set; }

    [Tooltip("âÒì]ÉÇÅ[ÉVÉáÉìåoâﬂéûä‘")]
    private float rotationElapsedTime;

    [SerializeField, Header("âÒì]ÇÃã≠Ç≥Å@(èâë¨âÒì]ë¨ìx)")]
    private float oneActionRotationStrength;
    public float OneActionRotationStrength { get; private set ;}

    [SerializeField, Header("âÒì]ÇÃí∑Ç≥Å@(ïb)")]
    private float oneActionDuration;
    public float OneActionDuration { get { return oneActionDuration; } private set { oneActionRotationStrength = value; } }

    [Tooltip("1ïbÇÃÉtÉåÅ[ÉÄêî")]
    private const float ONE_SECONDS_FRAME = 50.0f;

    //private void FixedUpdate()
    //{
    //    PlayerBackSystem();

    //    RotationSystem();
    //}

    //private void PlayerBackSystem()
    //{
    //    var moveSystem = enemyMoveSystem;
    //    if (playerScript.MoveSystem.GetMoveKeyAndButtonIsPressed())
    //        Walk(true);
    //    else
    //        Walk(false);

    //    if (!playerScript.AttackSystem.IsAttack)
    //    {
    //        Attack(false);
    //        return;
    //    }

    //    Attack(true);

    //    if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "EnemyAttackFirst")
    //        return;

    //    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
    //        return;

    //    isRotation = true;
    //}

    //public void Walk(bool isActive) { animator.SetBool("IsWalk", isActive); }

    //public void Attack(bool isActive) { animator.SetBool("IsAttack", isActive); }

    //private void RotationSystem()
    //{
    //    if (!IsRotation)
    //        return;

    //    rotationElapsedTime += Time.deltaTime;

    //    if (rotationElapsedTime >= OneActionDuration)
    //    {
    //        InitializedSystem();
    //        playerScript.AttackSystem.InitializedSystem();
    //        return;
    //    }

    //    RotateAnimation();
    //}

    //public void RotateAnimation()
    //{
    //    playerData.ModelTransform.RotateAround(
    //        playerData.ModelTransform.position, Vector3.up, (-OneActionRotationStrength / ONE_SECONDS_FRAME)
    //    );
    //}

    //private void InitializedSystem()
    //{
    //    isRotation = false;
    //    rotationElapsedTime = 0.0f;
    //}
}
