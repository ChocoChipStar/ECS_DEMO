using UnityEngine;
using UnityEngine.InputSystem;

public class TrashRotationTest : MonoBehaviour
{
    // 必要な変数 ===============================================================================
    // 回転の最低速
    [SerializeField, Tooltip("ローテーションの最低スピードを決めてください")]
    private float rotationMin;

    // 回転の最高速
    [SerializeField, Tooltip("ローテーションの最大スピードを決めてください")]
    private float rotationMax;

    // 飛ぶ距離
    [SerializeField, Tooltip("吹っ飛ぶ距離を決めてください")]
    private float distanceToFly;
    // 飛ぶ直前の位置
    private Vector3 posBeforeFly;

    // Rigitbody取得
    private Rigidbody rigidbody;

    //BoxCollider取得
    private BoxCollider boxCollider;

    // 回転速度
    private Vector3 rotationSpeed = Vector3.zero;

    // トルクの速度付加
    private Vector3 torqueSpeed = Vector3.zero;

    // 飛んでるかどうか
    private bool isFly = false;


    // 動く方向
    private Vector3 direction;
    //// 動く方向を定める
    //public void SetDirection(Vector3 hitDirection)
    //{
    //    direction = hitDirection;
    //}


    // 不必要な変数 =============================================================================
    [SerializeField, Tooltip("吹っ飛ぶ強さを決めてください")]
    private Vector2 flyPower;

    [SerializeField, Tooltip("吹っ飛ぶ高さを決めてください")]
    private float flyHigh;

    private void OnEnable()
    {
        direction = this.transform.position - PlayerScriptsManager.Instance.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Rigitbody取得
        rigidbody = GetComponent<Rigidbody>();

        //BoxCollider取得
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // 飛んでいるとき
        if (isFly)
        {
            // ランダムで決定した数値通りに回転する
            transform.Rotate(rotationSpeed);

            // 飛んだ距離の計算し一定に達したら飛んでるフラグを解除する
            Vector3 distanceFlown = transform.position - posBeforeFly;
            if (distanceToFly <= distanceFlown.magnitude)
            {
                IsFlyEqualFalse();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 地面以外に当たった時
        if (other.gameObject.layer != 9)
        {
            // isFlyがtrueならオブジェクトを消してfalseになる
            if (isFly)
            {
                Destroy(other.gameObject);
                IsFlyEqualFalse();
            }
        }
    }

    // isFlyをオンにするときの挙動
    public void IsFlyEqualTrue()
    {
        isFly = true;

        // 高さを固定する
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY; 

        // istriggerをオンにする
        boxCollider.isTrigger = true;

        // 回転の方向と速度をランダムで決定
        rotationSpeed.x = Random.Range(rotationMin, rotationMax);
        rotationSpeed.y = Random.Range(rotationMin, rotationMax);
        rotationSpeed.z = Random.Range(rotationMin, rotationMax);

        // AddForceで飛んでいく方向を決定
        Vector3 impulsPower = Vector3.zero;
        impulsPower.y = flyHigh;
        impulsPower.x = flyPower.x;
        impulsPower.z = flyPower.y;
        rigidbody.AddForce(impulsPower, ForceMode.Impulse);

        // 飛ぶ直前の位置記憶
        posBeforeFly = transform.position;

    }

    // isFlyをオフにするときの挙動
    private void IsFlyEqualFalse()
    {
        isFly = false;

        // 高さを自由にする
        rigidbody.constraints = RigidbodyConstraints.None;

        // isTriggerをオフにする
        boxCollider.isTrigger = false;

        // Rigitbodyでの回転の計算に切り替える
        rigidbody.AddTorque(DirectionOfRotation(rotationSpeed), ForceMode.Impulse);
    }

    // 回転の方向とトルク用の最大値を抽出
    private Vector3 DirectionOfRotation(Vector3 rotation)
    {
        // directionOfRotationの正負を抽出
        Vector3 directionOfRotation = Vector3.zero;
        directionOfRotation.x = rotation.x / Mathf.Abs(rotation.x);
        directionOfRotation.y = rotation.y / Mathf.Abs(rotation.y);
        directionOfRotation.z = rotation.z / Mathf.Abs(rotation.z);
        // Torqueの最大値である7.0ｆを掛ける
        float seven = 7.0f;
        directionOfRotation *= 7.0f;
        return directionOfRotation;
    }
}
