using UnityEngine;
using UnityEngine.InputSystem;

public class TrashDivision : MonoBehaviour
{
    // �K�v�ȕϐ� ===============================================================================
    // �����蔻�肪�L���ȋ�����
    public void SetDistanceFly(float enableColliderDistance)
    {
        this.enableColliderDistance = enableColliderDistance;
    }
    private float enableColliderDistance;
    // ��Ԓ��O�̈ʒu
    private Vector3 posBeforeFly;

    
    // ��]�̍Œᑬ
    private float rotationMin;
    // ��]�̍ō���
    private float rotationMax;
    // ��]���x�擾
    public void RotateSpeed(float rotationMin, float rotationMax)
    {
        // �ō����擾
        this.rotationMin = rotationMin;
        // �Œᑬ�擾
        this.rotationMax = rotationMax;
    }

    // ��]���x
    private Vector3 rotationSpeed = Vector3.zero;
    // �g���N�̑��x�t��
    private Vector3 riditbodyTorqueSpeed = new Vector3(15.0f, 15.0f, 15.0f);

    // ��������
    private Vector3 hitDirection;
    // ��ԗ�
    private float flyPower;
    // ���������Ɨ͂��߂�
    public void SetDirection(Vector3 hitDirection, float flyPower)
    {
        hitDirection.Normalize();
        this.hitDirection = hitDirection;
        this.flyPower = flyPower;
    }

    // ���ł邩�ǂ���
    private bool isFly = false;

    // ����܂ł̎���
    private float brokenTime;
    // ����܂ł̎��Ԍv���p
    private float brokenTimeTimer;
    // ����܂ł̎��Ԏ擾
    public void SetBrokenTime(float brokenTime)
    {
        this.brokenTime = brokenTime;
    }

    // Rigitbody�擾
    private Rigidbody rigidbody;
    //BoxCollider�擾
    private MeshCollider meshCollider;

    // ScrapGenerate�擾
    private ScrapGenerate scrapGenerate;
    Transform scrapClone;

    void Awake()
    {
        // Rigitbody�擾
        rigidbody = GetComponent<Rigidbody>();
        //BoxCollider�擾
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnEnable()
    {
        IsFlyEqualTrue();
    }

    void Start()
    {
        // TrashChengesToScrap�擾
        scrapGenerate = ScrapGenerate.instance;
        //// ScrapClone����
        //scrapClone = scrapGenerate.ScrapUIGenerateSystem(transform.position);
        //// ScrapClone��ҋ@��Ԃɂ���
        //scrapClone.gameObject.SetActive(false);
    }

        //Debug.Log("Start");
        //Debug.Log("scrapClone");

    void Update()
    {
        //���ł���Ƃ�
        if (isFly)
        {
            // �����_���Ō��肵�����l�ʂ�ɉ�]����
            transform.Rotate(rotationSpeed);

            // ��񂾋����̌v�Z�����ɒB��������ł�t���O����������
            Vector3 distanceFlown = transform.position - posBeforeFly;
            if (enableColliderDistance <= distanceFlown.magnitude)
            {
                IsFlyEqualFalse();
            }
        }
        else
        {
            TrashDestroy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �n�ʈȊO�ɓ���������
        bool notGround = other.gameObject.tag != "Ground";
        // �e�������I�u�W�F�N�g�ȊO�ɓ���������
        bool notSameRoot = this.transform.root != other.transform.root;
        // �n�ʂƐe�������I�u�W�F�N�g�ȊO�ɓ���������
        bool notGroundAndNotSameRoot = notGround && notSameRoot;
        // TrashDivision�ȊO�ɓ���������
        bool notTrashDivision = other.gameObject.layer != 8;

        if (notGroundAndNotSameRoot && notTrashDivision) 
        {
            // isFly��true�Ȃ�I�u�W�F�N�g��������false�ɂȂ�
            if (isFly)
            {
                //Destroy(other.gameObject);
                other.transform.root.GetComponent<Trash>().ExplosionTrash();
                IsFlyEqualFalse();
            }
        }
    }

    // isFly���I���ɂ���Ƃ��̋���
    private void IsFlyEqualTrue()
    {
        isFly = true;

        // �������Œ肷��
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY;

        // istrigger���I���ɂ���
        meshCollider.isTrigger = true;

        // ��]�̕����Ƒ��x�������_���Ō���
        rotationSpeed.x = Random.Range(rotationMin, rotationMax);
        rotationSpeed.y = Random.Range(rotationMin, rotationMax);
        rotationSpeed.z = Random.Range(rotationMin, rotationMax);

        // AddForce�Ŕ��ł�������������
        Vector3 impulsPower = Vector3.zero;
        impulsPower = hitDirection * flyPower;
        rigidbody.AddForce(impulsPower, ForceMode.Impulse);
        //rigidbody.AddForce(hitDirection * 10, ForceMode.Impulse);

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
        meshCollider.isTrigger = false;

        // Rigitbody�ł̉�]�̌v�Z�ɐ؂�ւ���
        rigidbody.AddTorque(DirectionOfRotation(riditbodyTorqueSpeed), ForceMode.Impulse);
    }

    //��]�̕����ƃg���N�p�̍ő�l�𒊏o
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

    // ���Ԍo�߂�Destroy
    private void TrashDestroy()
    {
        brokenTimeTimer += Time.deltaTime;

        Debug.Log(gameObject.name);

        bool brokenTimeElapsed = brokenTimeTimer >= brokenTime;

        if (!brokenTimeElapsed) return;

        Debug.Log("�f�X�g���C");

        //// ScrapClone���N������
        //scrapClone.gameObject.SetActive(true);

        // ScrapClone���N������
        scrapClone = ScrapGenerate.instance.ScrapUIGenerateSystem(transform.position);

        Destroy(gameObject);
        // Destroy(gameObject.transform.root.gameObject);

        // ��ꂽ���Ƃ�ʒm����
        transform.root.GetComponent<Trash>().DestroyTrash(); 
    }
}
