using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;
using PlayerData = PlayerDataManager_V2;
using PlayerScript = PlayerScriptManager_V2;

#nullable enable
public class MoveSystem_V2 : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�̈ړ����x")]
    private float baseSpeed;

    [Tooltip("���݂̈ړ����x")]
    private float currentSpeed;
    public float CurrentSpeed { get { return currentSpeed; } private set { currentSpeed = value; } }

    [SerializeField, Header("�v���C���[�̍ő�ړ����x")]
    private float baseMaxSpeed;

    [Tooltip("���݂̃v���C���[�ō����x")]
    private float maxCurrentSpeed;
    public float MaxCurrentSpeed { get { return maxCurrentSpeed; } private set { maxCurrentSpeed = value; } }

    [SerializeField, Header("�u���[�L�R�͒l")]
    private float breakValue;

    [Tooltip("1�t���[���O�̃v���C���[���W�̒l����")]
    private Vector3 lastPosition = Vector3.zero;

    [Tooltip("�f�[�^�}�l�[�W���[�̃C���X�^���X�擾")]
    private PlayerData playerData = null;
    [Tooltip("�X�N���v�g�}�l�[�W���[�̃C���X�^���X�擾")]
    private PlayerScript playerScript = null;

    [Tooltip("���ˏ������s���� true->���s false->����s")]
    private bool isReflect = false;
    [Tooltip("���˃x�N�g���̒l����")]
    private Vector3 reflectionDirection = Vector3.zero;

    private RaycastHit rayHit;

    [Tooltip("���C���[�ˏo�����l")]
    private float layerDistance = 100.0f;

    [Tooltip("�s�����C���[�ԍ�")]
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
        ReflectSystem(null);

        MovementBorderSystem();

        if (PlayerScript.Instance.AttackSystem.IsAttack)
            return;
        
        KeyboardSystem();

        ControllerSystem();

        BreakSystem();

        DirectionSystem();
    }

    /// <summary>
    /// �L�[�{�[�h����ł̏㉺���E�ړ��̏������s���܂�
    /// </summary>
    private void KeyboardSystem()
    {
        if (!IsMoveKeyAndButtonPressed())
            return;

        var keyCurrent = Keyboard.current;

        if (playerData.RigidBody.velocity.magnitude >= MaxCurrentSpeed)
            return;

        if (keyCurrent.wKey.isPressed || keyCurrent.upArrowKey.isPressed)
            playerData.RigidBody.AddForce(0.0f, 0.0f, CurrentSpeed,ForceMode.Impulse);

        if (keyCurrent.sKey.isPressed || keyCurrent.downArrowKey.isPressed)
            playerData.RigidBody.AddForce(0.0f,0.0f, -CurrentSpeed,ForceMode.Impulse);

        if (keyCurrent.aKey.isPressed || keyCurrent.leftArrowKey.isPressed)
            playerData.RigidBody.AddForce(-CurrentSpeed, 0.0f, 0.0f, ForceMode.Impulse);

        if (keyCurrent.dKey.isPressed || keyCurrent.rightArrowKey.isPressed)
            playerData.RigidBody.AddForce(CurrentSpeed, 0.0f, 0.0f,ForceMode.Impulse);
    }

    /// <summary>
    /// �R���g���[���[����ł̏㉺���E�ړ��̏������s���܂�
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
    /// �ړ������ɍ��킹�ăv���C���[�̌�����ύX���鏈�����s���܂�
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
    /// �ړ����E�͈͂��w�肵�A�͈͂𒴂��Ȃ��悤�ɂ��鏈�����s���܂��B
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
    /// �ړ��̃x�[�X���x�ƍő呬�x�̊�����ݒ肵�܂��B
    /// </summary>
    /// <param name="magnificationValue">�ύX��̃p�[�Z���e�[�W����</param>
    public void SetMoveSpeed(float magnificationValue)
    {
        currentSpeed = baseSpeed * playerData.ConvertPercentageValue(magnificationValue);
        maxCurrentSpeed = baseMaxSpeed * playerData.ConvertPercentageValue(magnificationValue);
    }

    /// <summary>
    /// �v���C���[���ړ��������͎��ɒn�ʂ̖��C�͂���������
    /// </summary>
    private void BreakSystem()
    {
        if (IsMoveKeyAndButtonPressed())
            playerData.RigidBody.drag = 0.0f;
        else
            playerData.RigidBody.drag = breakValue;
    }

    /// <summary>
    /// �U�����e���ꂽ��̈ړ��������s���܂�
    /// </summary>
    /// <param name="targetObj">�v���C���[�ɏՓ˂����s��I�u�W�F�N�g����</param>
    public void ReflectSystem(GameObject? targetObj)
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
    /// �ړ����͑�����s���Ă��邩�m�F���܂�
    /// </summary>
    /// <returns>true->�ړ�������͎� false->����͎�</returns>
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
