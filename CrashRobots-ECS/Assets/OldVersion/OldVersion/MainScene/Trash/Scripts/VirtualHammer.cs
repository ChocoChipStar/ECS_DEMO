using UnityEngine;

public class VirtualHammer : MonoBehaviour
{
    // 速度選定
    [SerializeField, Tooltip("仮想ハンマーの速度を設定して下さい")]
    public float speed;

    // 速度Vector3変換用
    public Vector3 speedV3 { get; set; } = Vector3.zero;

    // Rigitbody取得
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        // speedをVector3に変換
        var speedVec3 = speedV3;
        speedVec3.x = speed;
        speedV3 = speedVec3;

        // Rigitbodyに適用
        var velocity = rigidbody.velocity;
        velocity = speedV3;
        rigidbody.velocity = velocity;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // Trashに当たったら止まる
    //    if (other.gameObject.layer == 7)
    //        speedV3 = Vector3.zero;
    //}
}
