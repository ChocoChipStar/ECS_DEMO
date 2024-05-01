using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerData = PlayerDataManager_V2;
using PlayerScript = PlayerScriptManager_V2;

public class AttackSystem_V2 : MonoBehaviour
{
    [SerializeField, Header("突進攻撃の最低チャージ時間　(秒)")]
    private float minChargeTime;
    public float MinChargeTime { get { return minChargeTime; } }

    [SerializeField, Header("突進攻撃の最大チャージ時間　(秒)")]
    private float maxChargeTime;
    public float MaxChargeTime { get { return maxChargeTime; } }

    [SerializeField, Header("回転減速率　(%)　現在使用していません")]
    private float rotationDecelerationRate;
    public float RotationDecelerationRate { get { return rotationDecelerationRate; } private set { rotationDecelerationRate = value; } }

    [SerializeField, Header("チャージ中の移動速度減速率　(%)")]
    private float chargingRate;
    public float ChargingRate { get { return chargingRate; } private set { chargingRate = value; } }

    [SerializeField, Header("チャージ時間 × 突進攻撃の威力（代入値）")]
    private float lungeAttack;
    public float LungeAttack { get { return lungeAttack; } private set { lungeAttack = value; } }

    [Tooltip("攻撃チャージ経過時間")]
    private float chargeElapsedTime;

    [Tooltip("現在の攻撃判定")]
    private bool isAttack = false;
    public bool IsAttack { get { return isAttack; } }

    [Tooltip("通常移動速度パーセンテージ")]
    private const float BASE_SPEED_PERCENTAGE = 100.0f;

    [Tooltip("データマネージャーのインスタンス")]
    private PlayerData playerData;

    [Tooltip("スクリプトマネージャーのインスタンス")]
    private PlayerScript playerScript;

    [Tooltip("攻撃のクールタイム判定")]
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

        // チャージ時間が最大時間を上回っていたら最大時間に固定する
        if(chargeElapsedTime > MaxChargeTime)
            chargeElapsedTime = MaxChargeTime;

        // StartCoroutine(AttackedInterval(chargeElapsedTime));

        if (chargeElapsedTime >= MinChargeTime)
        {
            // 現在のエネルギーから突進攻撃が可能かどうかを確認
            if (!playerScript.EnergySystem.ChargeAttackConsumeEnergy(chargeElapsedTime))
            {
                InitializedSystem();
                return;
            }

            playerData.RigidBody.AddForce(playerData.ModelTransform.forward * chargeElapsedTime * LungeAttack, ForceMode.Impulse);
        }
        else
        {
            // 現在のエネルギーから突進攻撃が可能かどうかを確認
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

        // チャージ時間が最大時間を上回っていたら最大時間に固定する
        if (chargeElapsedTime > MaxChargeTime)
            chargeElapsedTime = MaxChargeTime;

        // StartCoroutine(AttackedInterval(chargeElapsedTime));

        if (chargeElapsedTime >= MinChargeTime)
        {
            // 現在のエネルギーから突進攻撃が可能かどうかを確認
            if (!playerScript.EnergySystem.ChargeAttackConsumeEnergy(chargeElapsedTime))
            {
                InitializedSystem();
                return;
            }

            playerData.RigidBody.AddForce(playerData.ModelTransform.forward * chargeElapsedTime * LungeAttack, ForceMode.Impulse);
        }
        else
        {
            // 現在のエネルギーから突進攻撃が可能かどうかを確認
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
    /// 攻撃後のクールタイム処理を行います
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
    /// 攻撃アクションのシステム初期化を行います
    /// </summary>
    public void InitializedSystem()
    {
        playerScript.MoveSystem.SetMoveSpeed(BASE_SPEED_PERCENTAGE);
        isAttack = false;
        chargeElapsedTime = 0.0f;
    }
}
