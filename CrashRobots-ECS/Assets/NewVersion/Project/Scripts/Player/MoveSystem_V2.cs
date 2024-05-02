using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;
using PlayerData = PlayerDataManager_V2;
using PlayerScript = PlayerScriptManager_V2;

#nullable enable
public class MoveSystem_V2 : MonoBehaviour
{
    [SerializeField, Header("プレイヤーの移動速度")]
    private float baseSpeed = 0.0f;

    [Tooltip("現在の移動速度")]
    private float currentSpeed;
    public float CurrentSpeed { get { return currentSpeed; } private set { currentSpeed = value; } }

    [SerializeField, Header("プレイヤーの最大移動速度")]
    private float baseMaxSpeed = 0.0f;

    [Tooltip("現在のプレイヤー最高速度")]
    private float maxCurrentSpeed = 0.0f;
    public float MaxCurrentSpeed { get { return maxCurrentSpeed; } private set { maxCurrentSpeed = value; } }

    [SerializeField, Header("ブレーキ抗力値")]
    private float breakValue = 0.0f;

    [SerializeField]
    private Camera mainCamera = null;

    private float layerDistance = 100.0f;

    private PlayerData playerData = null;
    private PlayerScript playerScript = null;

    private bool isReflect = false;

    private Vector3 lastPosition = Vector3.zero;
    private Vector3 reflectionDirection = Vector3.zero;

    private RaycastHit rayHit;

    private const int UNBREAKABLE_LAYER_NUMBER = 12;


    private void Awake()
    {
        playerData = PlayerData.Instance;
        playerScript = PlayerScript.Instance;

        currentSpeed = baseSpeed;
        maxCurrentSpeed = baseMaxSpeed;
    }

    private void FixedUpdate()
    {
        ReflectSystem();

        // MovementBorderSystem();

        if (PlayerScript.Instance.AttackSystem.IsAttack)
            return;
        
        KeyboardSystem();

        ControllerSystem();

        BreakSystem();

        DirectionSystem();
    }

    /// <summary>
    /// キーボード操作での上下左右移動の処理を行います
    /// </summary>
    private void KeyboardSystem()
    {
        if (!IsMoveKeyAndButtonPressed())
            return;

        var keyCurrent = Keyboard.current;

        if (playerData.RigidBody.velocity.magnitude >= MaxCurrentSpeed)
            return;

        if (keyCurrent.wKey.isPressed || keyCurrent.upArrowKey.isPressed)
            playerData.RigidBody.AddForce(mainCamera.transform.forward * CurrentSpeed,ForceMode.Impulse);

        if (keyCurrent.sKey.isPressed || keyCurrent.downArrowKey.isPressed)
            playerData.RigidBody.AddForce(mainCamera.transform.forward * -CurrentSpeed,ForceMode.Impulse);

        if (keyCurrent.aKey.isPressed || keyCurrent.leftArrowKey.isPressed)
            playerData.RigidBody.AddForce(mainCamera.transform.right * -CurrentSpeed, ForceMode.Impulse);

        if (keyCurrent.dKey.isPressed || keyCurrent.rightArrowKey.isPressed)
            playerData.RigidBody.AddForce(mainCamera.transform.right * CurrentSpeed,ForceMode.Impulse);
    }

    /// <summary>
    /// コントローラー操作での上下左右移動の処理を行います
    /// </summary>
    private void ControllerSystem()
    {
        if (!playerData.GetActiveController())
            return;

        var gamepadValue = Gamepad.current.leftStick.ReadValue();

        if (playerData.RigidBody.velocity.magnitude >= MaxCurrentSpeed)
            return;

        playerData.RigidBody.AddForce(gamepadValue.x * CurrentSpeed, 0.0f, gamepadValue.y * CurrentSpeed, ForceMode.Impulse);
    }

    /// <summary>
    /// 移動方向に合わせてプレイヤーの向きを変更する処理を行います
    /// </summary>
    private void DirectionSystem()
    {
        var direction = playerData.Transform.position - lastPosition;

        if (direction.magnitude < 0.05f)
            return;
    
        var directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var eulerAngle = playerData.ModelTransform.eulerAngles;

        eulerAngle = new Vector3(0.0f, directionAngle, 0.0f);

        playerData.ModelTransform.eulerAngles = eulerAngle;
        lastPosition = playerData.Transform.position;
    }

    /// <summary>
    /// 移動限界範囲を指定し、範囲を超えないようにする処理を行います。
    /// </summary>
    private void MovementBorderSystem()
    {
        var currentPosition = playerData.Transform.position;
        var currentVelocity = playerData.RigidBody.velocity;

        var clampLeft   = playerScript.CameraSystem.GetCameraLeftBottom().x;
        var clampRight  = playerScript.CameraSystem.GetCameraRightTop().x;
        var clampUp     = playerScript.CameraSystem.GetCameraRightTop().z;
        var clampBottom = playerScript.CameraSystem.GetCameraLeftBottom().z;

        if (currentPosition.x < clampLeft)
        {
            currentPosition.x = clampLeft;
            currentVelocity = new Vector3(0.0f, currentVelocity.y, currentVelocity.z);
        }

        if (currentPosition.x > clampRight)
        {
            currentPosition.x = clampRight;
            currentVelocity = new Vector3(0.0f, currentVelocity.y, currentVelocity.z);
        }

        if (currentPosition.z > clampUp)
        {
            currentPosition.z = clampUp;
            currentVelocity = new Vector3(currentVelocity.x, currentVelocity.y, 0.0f);
        }

        if(currentPosition.z < clampBottom)
        {
            currentPosition.z = clampBottom;
            currentVelocity = new Vector3(currentVelocity.x, currentVelocity.y, 0.0f);
        }

        playerData.Transform.position = currentPosition;
        playerData.RigidBody.velocity = currentVelocity;
    }

    /// <summary>
    /// 移動のベース速度と最大速度の割合を設定します。
    /// </summary>
    /// <param name="magnificationValue">変更後のパーセンテージを代入</param>
    public void SetMoveSpeed(float magnificationValue)
    {
        currentSpeed = baseSpeed * playerData.ConvertPercentageValue(magnificationValue);
        maxCurrentSpeed = baseMaxSpeed * playerData.ConvertPercentageValue(magnificationValue);
    }

    /// <summary>
    /// プレイヤーが移動操作非入力時に地面の摩擦力を強くする
    /// </summary>
    private void BreakSystem()
    {
        if (IsMoveKeyAndButtonPressed())
            playerData.RigidBody.drag = 0.0f;
        else
            playerData.RigidBody.drag = breakValue;
    }

    /// <summary>
    /// 攻撃が弾かれた後の移動処理を行います
    /// </summary>
    /// <param name="targetObj">プレイヤーに衝突した不可壊オブジェクトを代入</param>
    public void ReflectSystem(GameObject? targetObj = null)
    {
        if(!playerScript.AttackSystem.IsAttack)
            isReflect = false;

        if(isReflect) 
            return;

        if (targetObj == null)
            return;

        var origin = playerData.transform.position;
        var direction = new Vector3(targetObj.transform.position.x, origin.y, targetObj.transform.position.z) - origin;
        int layerMask = 1 << UNBREAKABLE_LAYER_NUMBER;

        if (!Physics.Raycast(origin, direction, out rayHit, layerDistance,layerMask))
        {
            Debug.DrawRay(origin, direction * layerDistance, Color.blue, 5.0f, false);

            return;
        }

        var inputDirection = playerData.RigidBody.velocity;
        var normalDirection = rayHit.normal;

        reflectionDirection = Vector3.Reflect(inputDirection, normalDirection);

        playerData.RigidBody.AddForce(reflectionDirection * playerData.ConvertPercentageValue(8000), ForceMode.Impulse);

        isReflect = true;        
    }


    /// <summary>
    /// 移動入力操作を行っているか確認します
    /// </summary>
    /// <returns>true->移動操作入力時 false->非入力時</returns>
    public bool IsMoveKeyAndButtonPressed()
    {
        var keyCurrent = Keyboard.current;

        if(keyCurrent.wKey.isPressed)
            return true;

        if (keyCurrent.aKey.isPressed)
            return true;

        if (keyCurrent.sKey.isPressed)
            return true;

        if (keyCurrent.dKey.isPressed)
            return true;

        if (!playerData.GetActiveController())
            return false;

        if (Gamepad.current.leftStick.ReadValue() != Vector2.zero)
            return true;

        return false;
    }
}
