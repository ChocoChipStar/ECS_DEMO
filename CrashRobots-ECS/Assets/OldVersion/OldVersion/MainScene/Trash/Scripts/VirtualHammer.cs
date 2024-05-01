using UnityEngine;

public class VirtualHammer : MonoBehaviour
{
    // ���x�I��
    [SerializeField, Tooltip("���z�n���}�[�̑��x��ݒ肵�ĉ�����")]
    public float speed;

    // ���xVector3�ϊ��p
    public Vector3 speedV3 { get; set; } = Vector3.zero;

    // Rigitbody�擾
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        // speed��Vector3�ɕϊ�
        var speedVec3 = speedV3;
        speedVec3.x = speed;
        speedV3 = speedVec3;

        // Rigitbody�ɓK�p
        var velocity = rigidbody.velocity;
        velocity = speedV3;
        rigidbody.velocity = velocity;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // Trash�ɓ���������~�܂�
    //    if (other.gameObject.layer == 7)
    //        speedV3 = Vector3.zero;
    //}
}
