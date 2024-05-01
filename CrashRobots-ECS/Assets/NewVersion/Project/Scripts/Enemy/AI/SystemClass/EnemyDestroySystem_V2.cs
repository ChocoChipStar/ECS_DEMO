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

    // 水平方向へ飛ばす力
    [SerializeField, Header("水平方向へ飛ばす力を設定してください")]
    private float horizontalFlyPower = 30.0f;
    // 水平方向へ飛ばす力
    [SerializeField, Header("上方向へ飛ばす力を設定してください")]
    private float upFlyPower = 2.0f;
    // 飛ばしたフラグ
    private bool isFly { get; set; } = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (!isFly) return;
        //if (!IsFall()) return;
        if (collision.gameObject.tag != "Ground") return;

        // ExplodeDestroyEnemy();
    }

    // 1フレーム前の高さ
    private float beforeOneFrameHight = 0.0f;
    // 落ちてる判定
    private bool IsFall()
    {
        Debug.Log(Data.Rigidbody.velocity.normalized);
        var isfall = Data.Rigidbody.velocity.y < 0;
        return isfall;
    }

    /// <summary>
    /// 吹っ飛ばされる
    /// </summary>
    /// <param name="crasherPosition"><飛ばすベクトルもしくは自身のポジション/param>
    /// <param name="directInputVector"><飛ばすベクトルを入力したならばtrueを入力/param>
    public void BlownAway(Vector3 crasherPosition,bool directInputVector)
    {
        Script.AttackSystemEnabled(false);
        Script.MoveSystemEnabled(false);

        var hitDirection = crasherPosition;
        if (!directInputVector)
            // 当たった相手から方向を計算する
            hitDirection = transform.position - crasherPosition;
        hitDirection.Normalize();

        //// divisionの位置初期化
        //InitializecDivisionTransform();

        // AddForceで飛んでいく方向を決定
        Vector3 impulsPower = Vector3.zero;
        impulsPower = hitDirection * horizontalFlyPower;
        impulsPower.y = upFlyPower;
        // 飛ばす
        Data.Rigidbody.AddForce(impulsPower, ForceMode.Impulse);
        // FreezRotationを解除する
        Data.Rigidbody.constraints = RigidbodyConstraints.None;
        // 回転させる
        Data.Rigidbody.AddTorque(RandomRotate(), ForceMode.Impulse);
        // 飛ばした
        isFly = true;
    }
    /// <summary>
    /// 正負のランダム
    /// </summary>
    private int RandomPulsOrMinus()
    {
        int random = 0;
        while (random == 0) random = Random.Range(-1, 1);

        return random;
    }
    /// <summary>
    /// 回転速度設定
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
    /// 爆発する
    /// </summary>
    public void ExplodeDestroyEnemy()
    {
        EnemyManager_V2.Instance.Bomb.Explode(transform.position);
        Destroy(gameObject);
    }

    //// divisionの位置初期化
    //private void InitializecDivisionTransform()
    //{
    //    // それぞれのtrasform取得
    //    var originTrasform = origin.transform;
    //    var divisionTrasform = division.transform;

    //    // それぞれのポジション取得
    //    var originPos = originTrasform.position;
    //    var divisionPos = divisionTrasform.position;
    //    // divisionの位置を合わせる
    //    divisionPos = originPos;
    //    division.transform.position = divisionPos;

    //    // それぞれの回転取得
    //    var originRotate = originTrasform.rotation;
    //    var divisionRotate = divisionTrasform.rotation;
    //    // divisionの回転を合わせる
    //    divisionRotate = originRotate;
    //    division.transform.rotation = divisionRotate;
    //}

    //// Trashとぶつかった挙動
    //public void CollisionTrash(Collider other)
    //{
    //    if (other.gameObject.layer != 7) return;

    //    var colTrash = other.transform.root.GetComponent<AssultTrush>();

    //    Broken(origin, division, true);
    //    Broken(colTrash.origin, colTrash.division, false);
    //}
    //// 壊れる挙動
    //private void Broken(GameObject originGameObject, GameObject divisionGameObject, bool isBomb)
    //{
    //    originGameObject.SetActive(false);
    //    divisionGameObject.SetActive(true);

    //    // トルクの速度付加
    //    var riditbodyTorqueSpeed = new Vector3(15.0f, 15.0f, 15.0f);

    //    // Rigitbodyでの回転の計算に切り替える
    //    var divisionChildCount = divisionGameObject.transform.childCount;
    //    for (int i = 0; i < divisionChildCount; i++)
    //    {
    //        var divisionChild = divisionGameObject.transform.GetChild(i);
    //        var divisionChildRigidbody = divisionChild.GetComponent<Rigidbody>();
    //        divisionChildRigidbody.AddTorque(DirectionOfRotation(riditbodyTorqueSpeed), ForceMode.Impulse);
    //    }

    //    // 自身から爆発生成
    //    if (isBomb) Bomb.instance.Explode(transform.position);
    //}

}
