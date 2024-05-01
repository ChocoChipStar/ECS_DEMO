using ExplosionSample;
using UnityEngine;

public class EnemyDestroySystem_V2 : EnemyParentClass_V2
{
   public override void Initialized() { }

    private void OnEnable()
    {
        Initialized();
    }

    public void EnemyDestroy()
    {
        Script.AttackSystemEnabled(false);
        Script.MoveSystemEnabled(false);
        Destroy(gameObject);
    }

    // ���������֔�΂���
    [SerializeField, Header("���������֔�΂��͂�ݒ肵�Ă�������")]
    private float horizontalFlyPower = 30.0f;
    // ���������֔�΂���
    [SerializeField, Header("������֔�΂��͂�ݒ肵�Ă�������")]
    private float upFlyPower = 2.0f;
    // ��΂����t���O
    private bool isFly { get; set; } = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (!isFly) return;
        //if (!IsFall()) return;
        if (collision.gameObject.tag != "Ground") return;

        // ExplodeDestroyEnemy();
    }

    // 1�t���[���O�̍���
    private float beforeOneFrameHight = 0.0f;
    // �����Ă锻��
    private bool IsFall()
    {
        Debug.Log(Data.Rigidbody.velocity.normalized);
        var isfall = Data.Rigidbody.velocity.y < 0;
        return isfall;
    }

    /// <summary>
    /// ������΂����
    /// </summary>
    /// <param name="crasherPosition"><��΂��x�N�g���������͎��g�̃|�W�V����/param>
    /// <param name="directInputVector"><��΂��x�N�g������͂����Ȃ��true�����/param>
    public void BlownAway(Vector3 crasherPosition,bool directInputVector)
    {
        Script.AttackSystemEnabled(false);
        Script.MoveSystemEnabled(false);

        var hitDirection = crasherPosition;
        if (!directInputVector)
            // �����������肩��������v�Z����
            hitDirection = transform.position - crasherPosition;
        hitDirection.Normalize();

        //// division�̈ʒu������
        //InitializecDivisionTransform();

        // AddForce�Ŕ��ł�������������
        Vector3 impulsPower = Vector3.zero;
        impulsPower = hitDirection * horizontalFlyPower;
        impulsPower.y = upFlyPower;
        // ��΂�
        Data.Rigidbody.AddForce(impulsPower, ForceMode.Impulse);
        // FreezRotation����������
        Data.Rigidbody.constraints = RigidbodyConstraints.None;
        // ��]������
        Data.Rigidbody.AddTorque(RandomRotate(), ForceMode.Impulse);
        // ��΂���
        isFly = true;
    }
    /// <summary>
    /// �����̃����_��
    /// </summary>
    private int RandomPulsOrMinus()
    {
        int random = 0;
        while (random == 0) random = Random.Range(-1, 1);

        return random;
    }
    /// <summary>
    /// ��]���x�ݒ�
    /// </summary>
    private Vector3 RandomRotate()
    {
        var randomRotate = new Vector3(7.0f, 7.0f, 7.0f);
        randomRotate.x *= RandomPulsOrMinus();
        randomRotate.y *= RandomPulsOrMinus();
        randomRotate.z *= RandomPulsOrMinus();

        return randomRotate;
    }

    private int firstColThrough = 0;
    /// <summary>
    /// ��������
    /// </summary>
    public void ExplodeDestroyEnemy()
    {
        EnemyManager_V2.Instance.Bomb.Explode(transform.position);
        Destroy(gameObject);
    }

    //// division�̈ʒu������
    //private void InitializecDivisionTransform()
    //{
    //    // ���ꂼ���trasform�擾
    //    var originTrasform = origin.transform;
    //    var divisionTrasform = division.transform;

    //    // ���ꂼ��̃|�W�V�����擾
    //    var originPos = originTrasform.position;
    //    var divisionPos = divisionTrasform.position;
    //    // division�̈ʒu�����킹��
    //    divisionPos = originPos;
    //    division.transform.position = divisionPos;

    //    // ���ꂼ��̉�]�擾
    //    var originRotate = originTrasform.rotation;
    //    var divisionRotate = divisionTrasform.rotation;
    //    // division�̉�]�����킹��
    //    divisionRotate = originRotate;
    //    division.transform.rotation = divisionRotate;
    //}

    //// Trash�ƂԂ���������
    //public void CollisionTrash(Collider other)
    //{
    //    if (other.gameObject.layer != 7) return;

    //    var colTrash = other.transform.root.GetComponent<AssultTrush>();

    //    Broken(origin, division, true);
    //    Broken(colTrash.origin, colTrash.division, false);
    //}
    //// ���鋓��
    //private void Broken(GameObject originGameObject, GameObject divisionGameObject, bool isBomb)
    //{
    //    originGameObject.SetActive(false);
    //    divisionGameObject.SetActive(true);

    //    // �g���N�̑��x�t��
    //    var riditbodyTorqueSpeed = new Vector3(15.0f, 15.0f, 15.0f);

    //    // Rigitbody�ł̉�]�̌v�Z�ɐ؂�ւ���
    //    var divisionChildCount = divisionGameObject.transform.childCount;
    //    for (int i = 0; i < divisionChildCount; i++)
    //    {
    //        var divisionChild = divisionGameObject.transform.GetChild(i);
    //        var divisionChildRigidbody = divisionChild.GetComponent<Rigidbody>();
    //        divisionChildRigidbody.AddTorque(DirectionOfRotation(riditbodyTorqueSpeed), ForceMode.Impulse);
    //    }

    //    // ���g���甚������
    //    if (isBomb) Bomb.instance.Explode(transform.position);
    //}

}
