using UnityEngine;


public class AnimationSystem_V2 : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [Tooltip("現在の回転判定")]
    private bool isRotation = false;
    public bool IsRotation { get { return isRotation; } }

    [Tooltip("回転モーション経過時間")]
    private float rotationElapsedTime;

    [SerializeField, Header("回転の強さ　(初速回転速度)")]
    private float oneActionRotationStrength;
    public float OneActionRotationStrength { get { return oneActionRotationStrength; } private set { oneActionRotationStrength = value; } }

    [SerializeField, Header("回転の長さ　(秒)")]
    private float oneActionDuration;
    public float OneActionDuration { get { return oneActionDuration; } private set { oneActionRotationStrength = value; } }

    [Tooltip("1秒のフレーム数")]
    private const float ONE_SECONDS_FRAME = 50.0f;

    [Tooltip("データマネージャーのインスタンスを取得")]
    private PlayerDataManager_V2 playerData;
    [Tooltip("スクリプトマネージャーのインスタンスを取得")]
    private PlayerScriptManager_V2 playerScript;

    private void Awake()
    {
        playerData   = PlayerDataManager_V2.Instance;
        playerScript = PlayerScriptManager_V2.Instance;
    }

    private void FixedUpdate()
    {
        CurrentAnimationSystem();

        RotateAnimation();
    }

    /// <summary>
    /// 現在の状態を基にアニメーションの再生を行います
    /// </summary>
    private void CurrentAnimationSystem()
    {
        if (playerScript.MoveSystem.IsMoveKeyAndButtonPressed())
            SetAnimationWalk(true);
        else
            SetAnimationWalk(false);

        if (!playerScript.AttackSystem.IsAttack)
        {
            SetAnimationAttack(false);
            return;
        }

        SetAnimationAttack(true);

        var currentClipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (currentClipName != "AttackFirst")
            return;

        if (currentStateInfo.normalizedTime < 1.0f)
            return;

        isRotation = true;
    }

    /// <summary>
    /// アニメーション【歩き】の状態を変更します
    /// </summary>
    /// <param name="isActive">true->再生 false->非再生</param>
    public void SetAnimationWalk(bool isActive)
    {
        animator.SetBool("IsWalk", isActive);
    }

    /// <summary>
    /// アニメーション【攻撃】の状態を変更します
    /// </summary>
    /// <param name="isActive">true->再生 false->非再生</param>
    public void SetAnimationAttack(bool isActive)
    {
        animator.SetBool("IsAttack", isActive);
    }

    /// <summary>
    /// プレイヤー回転時（攻撃時）のアニメーション処理を行います
    /// </summary>
    private void RotateAnimation()
    {
        if (!IsRotation)
            return;

        rotationElapsedTime += Time.deltaTime;

        if (rotationElapsedTime >= OneActionDuration)
        {
            InitializedRotateAnimation();
            playerScript.AttackSystem.InitializedSystem();
            return;
        }

        playerData.ModelTransform.RotateAround(
            playerData.ModelTransform.position, Vector3.up, (-OneActionRotationStrength / ONE_SECONDS_FRAME)
        );
    }

    /// <summary>
    /// 回転時アニメーションの初期化を行います
    /// </summary>
    private void InitializedRotateAnimation()
    {
        isRotation = false;
        rotationElapsedTime = 0.0f;
    }
}
