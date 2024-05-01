using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerData = PlayerDataManager_V2;
using PlayerScript = PlayerScriptManager_V2;

public class AttackSystem_V2 : MonoBehaviour
{
    [SerializeField, Header("�ːi�U���̍Œ�`���[�W���ԁ@(�b)")]
    private float minChargeTime;
    public float MinChargeTime { get { return minChargeTime; } }

    [SerializeField, Header("�ːi�U���̍ő�`���[�W���ԁ@(�b)")]
    private float maxChargeTime;
    public float MaxChargeTime { get { return maxChargeTime; } }

    [SerializeField, Header("��]�������@(%)�@���ݎg�p���Ă��܂���")]
    private float rotationDecelerationRate;
    public float RotationDecelerationRate { get { return rotationDecelerationRate; } private set { rotationDecelerationRate = value; } }

    [SerializeField, Header("�`���[�W���̈ړ����x�������@(%)")]
    private float chargingRate;
    public float ChargingRate { get { return chargingRate; } private set { chargingRate = value; } }

    [SerializeField, Header("�`���[�W���� �~ �ːi�U���̈З́i����l�j")]
    private float lungeAttack;
    public float LungeAttack { get { return lungeAttack; } private set { lungeAttack = value; } }

    [Tooltip("�U���`���[�W�o�ߎ���")]
    private float chargeElapsedTime;

    [Tooltip("���݂̍U������")]
    private bool isAttack = false;
    public bool IsAttack { get { return isAttack; } }

    [Tooltip("�ʏ�ړ����x�p�[�Z���e�[�W")]
    private const float BASE_SPEED_PERCENTAGE = 100.0f;

    [Tooltip("�f�[�^�}�l�[�W���[�̃C���X�^���X")]
    private PlayerData playerData;

    [Tooltip("�X�N���v�g�}�l�[�W���[�̃C���X�^���X")]
    private PlayerScript playerScript;

    [Tooltip("�U���̃N�[���^�C������")]
    private bool isInterval = false;


    private void Awake()
    {
        playerData = PlayerData.Instance;
        playerScript = PlayerScript.Instance;
    }

    private void Update()
    {
        InputKeyButtonSystem();

        InputPadButtonSystem();
    }

    private void InputKeyButtonSystem()
    {
        if (IsAttack)
            return;

        var keyCurrent = Keyboard.current;

        if (keyCurrent.spaceKey.wasPressedThisFrame)
            playerScript.MoveSystem.SetMoveSpeed(ChargingRate);

        if (keyCurrent.spaceKey.isPressed)
            chargeElapsedTime += Time.deltaTime;

        if (!keyCurrent.spaceKey.wasReleasedThisFrame)
            return;

        // �`���[�W���Ԃ��ő厞�Ԃ������Ă�����ő厞�ԂɌŒ肷��
        if(chargeElapsedTime > MaxChargeTime)
            chargeElapsedTime = MaxChargeTime;

        // StartCoroutine(AttackedInterval(chargeElapsedTime));

        if (chargeElapsedTime >= MinChargeTime)
        {
            // ���݂̃G�l���M�[����ːi�U�����\���ǂ������m�F
            if (!playerScript.EnergySystem.ChargeAttackConsumeEnergy(chargeElapsedTime))
            {
                InitializedSystem();
                return;
            }

            playerData.RigidBody.AddForce(playerData.ModelTransform.forward * chargeElapsedTime * LungeAttack, ForceMode.Impulse);
        }
        else
        {
            // ���݂̃G�l���M�[����ːi�U�����\���ǂ������m�F
            if (!playerScript.EnergySystem.AttackConsumeEnergy())
            {
                InitializedSystem();
                return;
            }
        }

        playerScript.EffectSystem.Rotation();
        isAttack = true;
    }

    private void InputPadButtonSystem()
    {
        if (!playerData.GetActiveController() || IsAttack)
            return;

        var padCurrent = Gamepad.current;

        if (padCurrent.bButton.wasPressedThisFrame)
            playerScript.MoveSystem.SetMoveSpeed(ChargingRate);

        if (padCurrent.bButton.isPressed)
            chargeElapsedTime += Time.deltaTime;

        if (!padCurrent.bButton.wasReleasedThisFrame)
            return;

        // �`���[�W���Ԃ��ő厞�Ԃ������Ă�����ő厞�ԂɌŒ肷��
        if (chargeElapsedTime > MaxChargeTime)
            chargeElapsedTime = MaxChargeTime;

        // StartCoroutine(AttackedInterval(chargeElapsedTime));

        if (chargeElapsedTime >= MinChargeTime)
        {
            // ���݂̃G�l���M�[����ːi�U�����\���ǂ������m�F
            if (!playerScript.EnergySystem.ChargeAttackConsumeEnergy(chargeElapsedTime))
            {
                InitializedSystem();
                return;
            }

            playerData.RigidBody.AddForce(playerData.ModelTransform.forward * chargeElapsedTime * LungeAttack, ForceMode.Impulse);
        }
        else
        {
            // ���݂̃G�l���M�[����ːi�U�����\���ǂ������m�F
            if (!playerScript.EnergySystem.AttackConsumeEnergy())
            {
                InitializedSystem();
                return;
            }
        }

        playerScript.EffectSystem.Rotation();
        isAttack = true;
    }

    /// <summary>
    /// �U����̃N�[���^�C���������s���܂�
    /// </summary>
    /// <param name="intervalTime"></param>
    /// <returns></returns>
    private IEnumerator AttackedInterval(float intervalTime)
    {
        isInterval = true;

        yield return new WaitForSeconds(intervalTime);

        isInterval = false;
    }

    /// <summary>
    /// �U���A�N�V�����̃V�X�e�����������s���܂�
    /// </summary>
    public void InitializedSystem()
    {
        playerScript.MoveSystem.SetMoveSpeed(BASE_SPEED_PERCENTAGE);
        isAttack = false;
        chargeElapsedTime = 0.0f;
    }
}
