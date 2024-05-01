using UnityEngine;
using UnityEngine.InputSystem;

public class TrashDivision : MonoBehaviour
{
    // 必要な変数 ===============================================================================
    // 当たり判定が有効な距離得
    public void SetDistanceFly(float enableColliderDistance)
    {
        this.enableColliderDistance = enableColliderDistance;
    }
    private float enableColliderDistance;
    // 飛ぶ直前の位置
    private Vector3 posBeforeFly;

    
    // 回転の最低速
    private float rotationMin;
    // 回転の最高速
    private float rotationMax;
    // 回転速度取得
    public void RotateSpeed(float rotationMin, float rotationMax)
    {
        // 最高速取得
        this.rotationMin = rotationMin;
        // 最低速取得
        this.rotationMax = rotationMax;
    }

    // 回転速度
    private Vector3 rotationSpeed = Vector3.zero;
    // トルクの速度付加
    private Vector3 riditbodyTorqueSpeed = new Vector3(15.0f, 15.0f, 15.0f);

    // 動く方向
    private Vector3 hitDirection;
    // 飛ぶ力
    private float flyPower;
    // 動く方向と力を定める
    public void SetDirection(Vector3 hitDirection, float flyPower)
    {
        hitDirection.Normalize();
        this.hitDirection = hitDirection;
        this.flyPower = flyPower;
    }

    // 飛んでるかどうか
    private bool isFly = false;

    // 壊れるまでの時間
    private float brokenTime;
    // 壊れるまでの時間計測用
    private float brokenTimeTimer;
    // 壊れるまでの時間取得
    public void SetBrokenTime(float brokenTime)
    {
        this.brokenTime = brokenTime;
    }

    // Rigitbody取得
    private Rigidbody rigidbody;
    //BoxCollider取得
    private MeshCollider meshCollider;

    // ScrapGenerate取得
    private ScrapGenerate scrapGenerate;
    Transform scrapClone;

    void Awake()
    {
        // Rigitbody取得
        rigidbody = GetComponent<Rigidbody>();
        //BoxCollider取得
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnEnable()
    {
        IsFlyEqualTrue();
    }

    void Start()
    {
        // TrashChengesToScrap取得
        scrapGenerate = ScrapGenerate.instance;
        //// ScrapClone生成
        //scrapClone = scrapGenerate.ScrapUIGenerateSystem(transform.position);
        //// ScrapCloneを待機状態にする
        //scrapClone.gameObject.SetActive(false);
    }

        //Debug.Log("Start");
        //Debug.Log("scrapClone");

    void Update()
    {
        //飛んでいるとき
        if (isFly)
        {
            // ランダムで決定した数値通りに回転する
            transform.Rotate(rotationSpeed);

            // 飛んだ距離の計算し一定に達したら飛んでるフラグを解除する
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
        // 地面以外に当たった時
        bool notGround = other.gameObject.tag != "Ground";
        // 親が同じオブジェクト以外に当たった時
        bool notSameRoot = this.transform.root != other.transform.root;
        // 地面と親が同じオブジェクト以外に当たった時
        bool notGroundAndNotSameRoot = notGround && notSameRoot;
        // TrashDivision以外に当たった時
        bool notTrashDivision = other.gameObject.layer != 8;

        if (notGroundAndNotSameRoot && notTrashDivision) 
        {
            // isFlyがtrueならオブジェクトを消してfalseになる
            if (isFly)
            {
                //Destroy(other.gameObject);
                other.transform.root.GetComponent<Trash>().ExplosionTrash();
                IsFlyEqualFalse();
            }
        }
    }

    // isFlyをオンにするときの挙動
    private void IsFlyEqualTrue()
    {
        isFly = true;

        // 高さを固定する
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY;

        // istriggerをオンにする
        meshCollider.isTrigger = true;

        // 回転の方向と速度をランダムで決定
        rotationSpeed.x = Random.Range(rotationMin, rotationMax);
        rotationSpeed.y = Random.Range(rotationMin, rotationMax);
        rotationSpeed.z = Random.Range(rotationMin, rotationMax);

        // AddForceで飛んでいく方向を決定
        Vector3 impulsPower = Vector3.zero;
        impulsPower = hitDirection * flyPower;
        rigidbody.AddForce(impulsPower, ForceMode.Impulse);
        //rigidbody.AddForce(hitDirection * 10, ForceMode.Impulse);

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
        meshCollider.isTrigger = false;

        // Rigitbodyでの回転の計算に切り替える
        rigidbody.AddTorque(DirectionOfRotation(riditbodyTorqueSpeed), ForceMode.Impulse);
    }

    //回転の方向とトルク用の最大値を抽出
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

    // 時間経過でDestroy
    private void TrashDestroy()
    {
        brokenTimeTimer += Time.deltaTime;

        Debug.Log(gameObject.name);

        bool brokenTimeElapsed = brokenTimeTimer >= brokenTime;

        if (!brokenTimeElapsed) return;

        Debug.Log("デストロイ");

        //// ScrapCloneを起動する
        //scrapClone.gameObject.SetActive(true);

        // ScrapCloneを起動する
        scrapClone = ScrapGenerate.instance.ScrapUIGenerateSystem(transform.position);

        Destroy(gameObject);
        // Destroy(gameObject.transform.root.gameObject);

        // 壊れたことを通知する
        transform.root.GetComponent<Trash>().DestroyTrash(); 
    }
}
