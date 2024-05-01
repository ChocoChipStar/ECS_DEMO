using UnityEngine;


public class AnimationSystem_V2 : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [Tooltip("���݂̉�]����")]
    private bool isRotation = false;
    public bool IsRotation { get { return isRotation; } }

    [Tooltip("��]���[�V�����o�ߎ���")]
    private float rotationElapsedTime;

    [SerializeField, Header("��]�̋����@(������]���x)")]
    private float oneActionRotationStrength;
    public float OneActionRotationStrength { get { return oneActionRotationStrength; } private set { oneActionRotationStrength = value; } }

    [SerializeField, Header("��]�̒����@(�b)")]
    private float oneActionDuration;
    public float OneActionDuration { get { return oneActionDuration; } private set { oneActionRotationStrength = value; } }

    [Tooltip("1�b�̃t���[����")]
    private const float ONE_SECONDS_FRAME = 50.0f;

    [Tooltip("�f�[�^�}�l�[�W���[�̃C���X�^���X���擾")]
    private PlayerDataManager_V2 playerData;
    [Tooltip("�X�N���v�g�}�l�[�W���[�̃C���X�^���X���擾")]
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
    /// ���݂̏�Ԃ���ɃA�j���[�V�����̍Đ����s���܂�
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
    /// �A�j���[�V�����y�����z�̏�Ԃ�ύX���܂�
    /// </summary>
    /// <param name="isActive">true->�Đ� false->��Đ�</param>
    public void SetAnimationWalk(bool isActive)
    {
        animator.SetBool("IsWalk", isActive);
    }

    /// <summary>
    /// �A�j���[�V�����y�U���z�̏�Ԃ�ύX���܂�
    /// </summary>
    /// <param name="isActive">true->�Đ� false->��Đ�</param>
    public void SetAnimationAttack(bool isActive)
    {
        animator.SetBool("IsAttack", isActive);
    }

    /// <summary>
    /// �v���C���[��]���i�U�����j�̃A�j���[�V�����������s���܂�
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
    /// ��]���A�j���[�V�����̏��������s���܂�
    /// </summary>
    private void InitializedRotateAnimation()
    {
        isRotation = false;
        rotationElapsedTime = 0.0f;
    }
}
