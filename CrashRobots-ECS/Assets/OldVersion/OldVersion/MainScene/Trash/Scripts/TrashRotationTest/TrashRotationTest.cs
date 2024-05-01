using UnityEngine;
using UnityEngine.InputSystem;

public class TrashRotationTest : MonoBehaviour
{
    // �K�v�ȕϐ� ===============================================================================
    // ��]�̍Œᑬ
    [SerializeField, Tooltip("���[�e�[�V�����̍Œ�X�s�[�h�����߂Ă�������")]
    private float rotationMin;

    // ��]�̍ō���
    [SerializeField, Tooltip("���[�e�[�V�����̍ő�X�s�[�h�����߂Ă�������")]
    private float rotationMax;

    // ��ԋ���
    [SerializeField, Tooltip("������ԋ��������߂Ă�������")]
    private float distanceToFly;
    // ��Ԓ��O�̈ʒu
    private Vector3 posBeforeFly;

    // Rigitbody�擾
    private Rigidbody rigidbody;

    //BoxCollider�擾
    private BoxCollider boxCollider;

    // ��]���x
    private Vector3 rotationSpeed = Vector3.zero;

    // �g���N�̑��x�t��
    private Vector3 torqueSpeed = Vector3.zero;

    // ���ł邩�ǂ���
    private bool isFly = false;


    // ��������
    private Vector3 direction;
    //// �����������߂�
    //public void SetDirection(Vector3 hitDirection)
    //{
    //    direction = hitDirection;
    //}


    // �s�K�v�ȕϐ� =============================================================================
    [SerializeField, Tooltip("������ԋ��������߂Ă�������")]
    private Vector2 flyPower;

    [SerializeField, Tooltip("������ԍ��������߂Ă�������")]
    private float flyHigh;

    private void OnEnable()
    {
        direction = this.transform.position - PlayerScriptsManager.Instance.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Rigitbody�擾
        rigidbody = GetComponent<Rigidbody>();

        //BoxCollider�擾
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���ł���Ƃ�
        if (isFly)
        {
            // �����_���Ō��肵�����l�ʂ�ɉ�]����
            transform.Rotate(rotationSpeed);

            // ��񂾋����̌v�Z�����ɒB��������ł�t���O����������
            Vector3 distanceFlown = transform.position - posBeforeFly;
            if (distanceToFly <= distanceFlown.magnitude)
            {
                IsFlyEqualFalse();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �n�ʈȊO�ɓ���������
        if (other.gameObject.layer != 9)
        {
            // isFly��true�Ȃ�I�u�W�F�N�g��������false�ɂȂ�
            if (isFly)
            {
                Destroy(other.gameObject);
                IsFlyEqualFalse();
            }
        }
    }

    // isFly���I���ɂ���Ƃ��̋���
    public void IsFlyEqualTrue()
    {
        isFly = true;

        // �������Œ肷��
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY; 

        // istrigger���I���ɂ���
        boxCollider.isTrigger = true;

        // ��]�̕����Ƒ��x�������_���Ō���
        rotationSpeed.x = Random.Range(rotationMin, rotationMax);
        rotationSpeed.y = Random.Range(rotationMin, rotationMax);
        rotationSpeed.z = Random.Range(rotationMin, rotationMax);

        // AddForce�Ŕ��ł�������������
        Vector3 impulsPower = Vector3.zero;
        impulsPower.y = flyHigh;
        impulsPower.x = flyPower.x;
        impulsPower.z = flyPower.y;
        rigidbody.AddForce(impulsPower, ForceMode.Impulse);

        // ��Ԓ��O�̈ʒu�L��
        posBeforeFly = transform.position;

    }

    // isFly���I�t�ɂ���Ƃ��̋���
    private void IsFlyEqualFalse()
    {
        isFly = false;

        // ���������R�ɂ���
        rigidbody.constraints = RigidbodyConstraints.None;

        // isTrigger���I�t�ɂ���
        boxCollider.isTrigger = false;

        // Rigitbody�ł̉�]�̌v�Z�ɐ؂�ւ���
        rigidbody.AddTorque(DirectionOfRotation(rotationSpeed), ForceMode.Impulse);
    }

    // ��]�̕����ƃg���N�p�̍ő�l�𒊏o
    private Vector3 DirectionOfRotation(Vector3 rotation)
    {
        // directionOfRotation�̐����𒊏o
        Vector3 directionOfRotation = Vector3.zero;
        directionOfRotation.x = rotation.x / Mathf.Abs(rotation.x);
        directionOfRotation.y = rotation.y / Mathf.Abs(rotation.y);
        directionOfRotation.z = rotation.z / Mathf.Abs(rotation.z);
        // Torque�̍ő�l�ł���7.0�����|����
        float seven = 7.0f;
        directionOfRotation *= 7.0f;
        return directionOfRotation;
    }
}
