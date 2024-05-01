using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDataManager_V2 : Singleton_V2<PlayerDataManager_V2>
{
    [SerializeField,Header("ダメージ時の画面揺れの長さ　(秒)")]
    private float damageShakeDuration;
    public float DamageShakeDuration { get { return damageShakeDuration; } }

    [SerializeField, Header("ダメージ時の画面揺れの強さ　(マグニチュード)")]
    private float damageShakeStrength;
    public float DamageShakeStrength { get {  return damageShakeStrength; } }

    [SerializeField,Header("攻撃ヒット時の画面揺れの長さ　(秒)")]
    private float attackShakeDuration;
    public float AttackShakeDuration { get { return attackShakeDuration; } }

    [SerializeField, Header("攻撃ヒット時の画面揺れの強さ　(マグニチュード)")]
    private float attackShakeStrength;
    public float AttackShakeStrength { get { return attackShakeStrength; } }

    [SerializeField,Header("ダメージ時のヒットストップの長さ　(秒)")]
    private float damageHitStopDuration;
    public float DamageHitStopDuration { get { return damageHitStopDuration; } }

    [SerializeField, Header("ダメージ時のヒットストップの強さ　(スロー倍率)")]
    private float damageHitStopStrengh;
    public float DamageHitStopStrengh { get { return damageHitStopStrengh; } }

    [SerializeField,Header("攻撃ヒット時のヒットストップの長さ　(秒)")]
    private float attackHitStopDuration;
    public float AttackHitStopDuration { get { return attackHitStopDuration; } }

    [SerializeField, Header("攻撃ヒット時のヒットストップの強さ　(スロー倍率)")]
    private float attackHitStopStrength;
    public float AttackHitStopStrength { get { return attackHitStopStrength; } }

    private Rigidbody rigidBody;
    public Rigidbody RigidBody { get { return rigidBody; } private set { rigidBody = value; } }

    private new Transform transform;
    public Transform Transform { get { return transform; } private set { transform = value; } }

    private Transform modelTransform;
    public Transform ModelTransform { get { return modelTransform; } private set { modelTransform = value; } }

    private void Awake()
    {
        Time.timeScale = 1.0f;
        rigidBody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        modelTransform = Transform.GetChild(0).transform;
    }

    /// <summary>
    /// パーセンテージを倍率に変換します
    /// </summary>
    /// <param name="originValue">倍率に変換したいパーセンテージの値を代入</param>
    /// <returns>代入されたパーセンテージ値を基に倍率値を返します</returns>
    public float ConvertPercentageValue(float originValue)
    {
        return originValue / 100.0f;
    }

    /// <summary>
    /// コントローラーが接続されいるかの判定を行います
    /// </summary>
    /// <returns>true->接続中 false->非接続</returns>
    public bool GetActiveController()
    {
        if (Gamepad.current == null)
            return false;

        return true;
    }
}
