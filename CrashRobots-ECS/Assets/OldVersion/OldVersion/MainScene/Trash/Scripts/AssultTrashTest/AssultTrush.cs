using ExplosionSample;
using UnityEngine;

public class AssultTrush : MonoBehaviour
{
    // Origin�擾
    [SerializeField, Header("Origin���A�^�b�`���Ă�������")]
    private GameObject origin;
    // Origin��MeshCollider�擾
    private MeshCollider[] originMeshCollider;

    // Division�擾
    [SerializeField, Header("Division���A�^�b�`���Ă�������")]
    private GameObject division;


    // �����蔻�肪�L���ȋ���
    [SerializeField, Header("�����蔻�肪�L���ȋ�����ݒ肵�Ă�������")]
    private float enableColliderDistance;
    // ��΂���
    [SerializeField, Header("��΂��͂�ݒ肵�Ă�������")]
    private float flyPower;

    // ��]�̍Œᑬ
    [SerializeField, Header("��]�̍Œᑬ��ݒ肵�Ă�������")]
    private float rotationMin;
    // ��]�̍ō���
    [SerializeField, Header("��]�̍ō�����ݒ肵�Ă�������")]
    private float rotationMax;
    // ��]���x
    private Vector3 rotationSpeed = Vector3.zero;
    // �g���N�̑��x�t��
    private Vector3 riditbodyTorqueSpeed = new Vector3(15.0f, 15.0f, 15.0f);

    // ���ł���t���O
    private bool isFly = false;
    // ���ł���t���O���Z�b�g����
    private void SetIsFly(bool isFly) { this.isFly = isFly; }
    // ���ł���t���O���擾����
    private bool GetIsFly() { return isFly; }

    //��Ԓ��O�̈ʒu
    private Vector3 posBeforeFly = Vector3.zero;
    // ��Ԓ��O�̈ʒu���Z�b�g����
    private void SetPosBeforeFly(Vector3 posBeforeFly) { this.posBeforeFly = posBeforeFly; }
    // ��Ԓ��O�̈ʒu���擾����
    private Vector3 GetPosBeforeFly() { return posBeforeFly; }


    // Rigitbody�擾
    private Rigidbody rigitbody;


    void Awake()
    {
        // �I�u�W�F�N�g�̏�ԏ�����
        origin.SetActive(true);
        division.SetActive(false);

        // origin��rigitbody�擾
        rigitbody = origin.GetComponent<Rigidbody>();
        // origin��MeshCollider�擾
        int originChildNum = origin.transform.childCount;
        originMeshCollider = new MeshCollider[originChildNum];
        for (int i = 0; i < originChildNum; i++)
        {
            var originchild = origin.transform.GetChild(i);
            originMeshCollider[i] = originchild.GetComponent<MeshCollider>();
        }

    }


    void Update()
    {
        if (GetIsFly())
        {
            // ��]����
            transform.Rotate(rotationSpeed);

            if (AfterCertainDistance())
            {
                SetIsFly(false);
                Broken(origin, division, true);
            }
        }

        //DestroyGrace();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �n�ʂ܂��͐e�������I�u�W�F�N�g�ȊO�ɓ���������
        var notGround = other.gameObject.tag != "Ground";
        var notPlayer = other.gameObject.layer != 6;
        if (notGround && notPlayer) 
        {
            // isFly��true�Ȃ�I�u�W�F�N�g��������false�ɂȂ�
            if (GetIsFly())
            {
                CollisionTrash(other);
                StateReset();
                SetIsFly(false);
            }
        }
    }

    private void OnDestroy()
    {
        ScoreManager.instance.AddScore();
    }

    // �^���������ł���
    public void AssultTrash(Vector3 crasherPosition)
    {
        // �����������肩��������v�Z����
        var hitDirection = transform.position - crasherPosition;
        hitDirection.Normalize();

        // division�̈ʒu������
        InitializecDivisionTransform();
        // ��Ԓ��O�̈ʒu�L��
        SetPosBeforeFly(transform.position);
        // ��]���x�ݒ�
        RandomRotate();
        // ��΂�
        AddForceImpulse(hitDirection);
        // ���ł���t���O�𗧂Ă�
        SetIsFly(true);

        // �������Œ肷��
        rigitbody.constraints = RigidbodyConstraints.FreezePositionY;
        // istrigger���I���ɂ���
        for (int i = 0; i < origin.transform.childCount; i++)
        {
            originMeshCollider[i].isTrigger = true;
        }
    }

    // division�̈ʒu������
    private void InitializecDivisionTransform()
    {
        // ���ꂼ���trasform�擾
        var originTrasform = origin.transform;
        var divisionTrasform = division.transform;

        // ���ꂼ��̃|�W�V�����擾
        var originPos = originTrasform.position;
        var divisionPos = divisionTrasform.position;
        // division�̈ʒu�����킹��
        divisionPos = originPos;
        division.transform.position = divisionPos;

        // ���ꂼ��̉�]�擾
        var originRotate = originTrasform.rotation;
        var divisionRotate = divisionTrasform.rotation;
        // division�̉�]�����킹��
        divisionRotate = originRotate;
        division.transform.rotation = divisionRotate;
    }
    // ��]���x�ݒ�
    private void RandomRotate()
    {
        // ��]�̕����Ƒ��x�������_���Ō���
        rotationSpeed.x = Random.Range(rotationMin, rotationMax);
        rotationSpeed.y = Random.Range(rotationMin, rotationMax);
        rotationSpeed.z = Random.Range(rotationMin, rotationMax);
    }
    // ��΂�
    private void AddForceImpulse(Vector3 hitDirection)
    {
        // AddForce�Ŕ��ł�������������
        Vector3 impulsPower = Vector3.zero;
        impulsPower = hitDirection * flyPower;
        // ��΂�
        rigitbody.AddForce(impulsPower, ForceMode.Impulse);
    }

    // ��苗���i�񂾂��ǂ����̃t���O
    private bool AfterCertainDistance()
    {
        Vector3 distanceFlown = transform.position - GetPosBeforeFly();
        var AfterCertainDistance = enableColliderDistance <= distanceFlown.magnitude;
        return AfterCertainDistance;
    }

    // ��Ԃ�߂�
    private void StateReset() 
    {
        // ���������R�ɂ���
        rigitbody.constraints = RigidbodyConstraints.None;

        for (int i = 0; i < origin.transform.childCount; i++)
        {
            // isTrigger���I�t�ɂ���
            originMeshCollider[i].isTrigger = true;
        }

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

    // Trash�ƂԂ���������
    public void CollisionTrash(Collider other)
    {
        if (other.gameObject.layer != 7) return;

        var colTrash = other.transform.root.GetComponent<AssultTrush>();

        Broken(origin, division, true);
        Broken(colTrash.origin, colTrash.division, false);
    }
    // ���鋓��
    private void Broken(GameObject originGameObject, GameObject divisionGameObject, bool isBomb)
    {
        originGameObject.SetActive(false);
        divisionGameObject.SetActive(true);

        // �g���N�̑��x�t��
        var riditbodyTorqueSpeed = new Vector3(15.0f, 15.0f, 15.0f);

        // Rigitbody�ł̉�]�̌v�Z�ɐ؂�ւ���
        var divisionChildCount = divisionGameObject.transform.childCount;
        for (int i = 0; i < divisionChildCount; i++)  {
            var divisionChild = divisionGameObject.transform.GetChild(i);
            var divisionChildRigidbody = divisionChild.GetComponent<Rigidbody>();
            divisionChildRigidbody.AddTorque(DirectionOfRotation(riditbodyTorqueSpeed), ForceMode.Impulse);
        }

        // ���g���甚������
        if (isBomb) Bomb.instance.Explode(transform.position);
    }

    // Destory�܂ł̃^�C�}�[
    private float destroyTimer = 0;
    // destroy�܂ł̎��Ԍv��
    private void DestroyGrace()
    {
        bool originActive = origin.activeSelf;
        bool divisionActive = division.activeSelf;

        if (!originActive && divisionActive) destroyTimer += Time.deltaTime;

        if (destroyTimer > 2) Destroy(gameObject);
    }

}
