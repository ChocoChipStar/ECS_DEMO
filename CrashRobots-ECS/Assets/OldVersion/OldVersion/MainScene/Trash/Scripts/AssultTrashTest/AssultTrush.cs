using ExplosionSample;
using UnityEngine;

public class AssultTrush : MonoBehaviour
{
    // Origin取得
    [SerializeField, Header("Originをアタッチしてください")]
    private GameObject origin;
    // OriginのMeshCollider取得
    private MeshCollider[] originMeshCollider;

    // Division取得
    [SerializeField, Header("Divisionをアタッチしてください")]
    private GameObject division;


    // 当たり判定が有効な距離
    [SerializeField, Header("当たり判定が有効な距離を設定してください")]
    private float enableColliderDistance;
    // 飛ばす力
    [SerializeField, Header("飛ばす力を設定してください")]
    private float flyPower;

    // 回転の最低速
    [SerializeField, Header("回転の最低速を設定してください")]
    private float rotationMin;
    // 回転の最高速
    [SerializeField, Header("回転の最高速を設定してください")]
    private float rotationMax;
    // 回転速度
    private Vector3 rotationSpeed = Vector3.zero;
    // トルクの速度付加
    private Vector3 riditbodyTorqueSpeed = new Vector3(15.0f, 15.0f, 15.0f);

    // 飛んでいるフラグ
    private bool isFly = false;
    // 飛んでいるフラグをセットする
    private void SetIsFly(bool isFly) { this.isFly = isFly; }
    // 飛んでいるフラグを取得する
    private bool GetIsFly() { return isFly; }

    //飛ぶ直前の位置
    private Vector3 posBeforeFly = Vector3.zero;
    // 飛ぶ直前の位置をセットする
    private void SetPosBeforeFly(Vector3 posBeforeFly) { this.posBeforeFly = posBeforeFly; }
    // 飛ぶ直前の位置を取得する
    private Vector3 GetPosBeforeFly() { return posBeforeFly; }


    // Rigitbody取得
    private Rigidbody rigitbody;


    void Awake()
    {
        // オブジェクトの状態初期化
        origin.SetActive(true);
        division.SetActive(false);

        // originのrigitbody取得
        rigitbody = origin.GetComponent<Rigidbody>();
        // originのMeshCollider取得
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
            // 回転挙動
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
        // 地面または親が同じオブジェクト以外に当たった時
        var notGround = other.gameObject.tag != "Ground";
        var notPlayer = other.gameObject.layer != 6;
        if (notGround && notPlayer) 
        {
            // isFlyがtrueならオブジェクトを消してfalseになる
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

    // 真っすぐ飛んでいく
    public void AssultTrash(Vector3 crasherPosition)
    {
        // 当たった相手から方向を計算する
        var hitDirection = transform.position - crasherPosition;
        hitDirection.Normalize();

        // divisionの位置初期化
        InitializecDivisionTransform();
        // 飛ぶ直前の位置記憶
        SetPosBeforeFly(transform.position);
        // 回転速度設定
        RandomRotate();
        // 飛ばす
        AddForceImpulse(hitDirection);
        // 飛んでいるフラグを立てる
        SetIsFly(true);

        // 高さを固定する
        rigitbody.constraints = RigidbodyConstraints.FreezePositionY;
        // istriggerをオンにする
        for (int i = 0; i < origin.transform.childCount; i++)
        {
            originMeshCollider[i].isTrigger = true;
        }
    }

    // divisionの位置初期化
    private void InitializecDivisionTransform()
    {
        // それぞれのtrasform取得
        var originTrasform = origin.transform;
        var divisionTrasform = division.transform;

        // それぞれのポジション取得
        var originPos = originTrasform.position;
        var divisionPos = divisionTrasform.position;
        // divisionの位置を合わせる
        divisionPos = originPos;
        division.transform.position = divisionPos;

        // それぞれの回転取得
        var originRotate = originTrasform.rotation;
        var divisionRotate = divisionTrasform.rotation;
        // divisionの回転を合わせる
        divisionRotate = originRotate;
        division.transform.rotation = divisionRotate;
    }
    // 回転速度設定
    private void RandomRotate()
    {
        // 回転の方向と速度をランダムで決定
        rotationSpeed.x = Random.Range(rotationMin, rotationMax);
        rotationSpeed.y = Random.Range(rotationMin, rotationMax);
        rotationSpeed.z = Random.Range(rotationMin, rotationMax);
    }
    // 飛ばす
    private void AddForceImpulse(Vector3 hitDirection)
    {
        // AddForceで飛んでいく方向を決定
        Vector3 impulsPower = Vector3.zero;
        impulsPower = hitDirection * flyPower;
        // 飛ばす
        rigitbody.AddForce(impulsPower, ForceMode.Impulse);
    }

    // 一定距離進んだかどうかのフラグ
    private bool AfterCertainDistance()
    {
        Vector3 distanceFlown = transform.position - GetPosBeforeFly();
        var AfterCertainDistance = enableColliderDistance <= distanceFlown.magnitude;
        return AfterCertainDistance;
    }

    // 状態を戻す
    private void StateReset() 
    {
        // 高さを自由にする
        rigitbody.constraints = RigidbodyConstraints.None;

        for (int i = 0; i < origin.transform.childCount; i++)
        {
            // isTriggerをオフにする
            originMeshCollider[i].isTrigger = true;
        }

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

    // Trashとぶつかった挙動
    public void CollisionTrash(Collider other)
    {
        if (other.gameObject.layer != 7) return;

        var colTrash = other.transform.root.GetComponent<AssultTrush>();

        Broken(origin, division, true);
        Broken(colTrash.origin, colTrash.division, false);
    }
    // 壊れる挙動
    private void Broken(GameObject originGameObject, GameObject divisionGameObject, bool isBomb)
    {
        originGameObject.SetActive(false);
        divisionGameObject.SetActive(true);

        // トルクの速度付加
        var riditbodyTorqueSpeed = new Vector3(15.0f, 15.0f, 15.0f);

        // Rigitbodyでの回転の計算に切り替える
        var divisionChildCount = divisionGameObject.transform.childCount;
        for (int i = 0; i < divisionChildCount; i++)  {
            var divisionChild = divisionGameObject.transform.GetChild(i);
            var divisionChildRigidbody = divisionChild.GetComponent<Rigidbody>();
            divisionChildRigidbody.AddTorque(DirectionOfRotation(riditbodyTorqueSpeed), ForceMode.Impulse);
        }

        // 自身から爆発生成
        if (isBomb) Bomb.instance.Explode(transform.position);
    }

    // Destoryまでのタイマー
    private float destroyTimer = 0;
    // destroyまでの時間計測
    private void DestroyGrace()
    {
        bool originActive = origin.activeSelf;
        bool divisionActive = division.activeSelf;

        if (!originActive && divisionActive) destroyTimer += Time.deltaTime;

        if (destroyTimer > 2) Destroy(gameObject);
    }

}
