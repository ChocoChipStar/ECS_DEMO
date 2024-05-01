using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Trash : MonoBehaviour
{
    // Rigitbody取得用
    private Rigidbody rigidbody;

    //// velocity用
    //public Vector3 velocity { get; private set; }

    // TrashOrigin取得
    [SerializeField, Header("TrashOriginをアタッチしてください")]
    private GameObject trashOrigin;
    // TrashOriginの子供のTransform取得
    private Transform[] trashOriginChildren;

    // TrashDizision取得
    [SerializeField, Header("TrashDivisionをアタッチしてください")]
    private GameObject[] trashDivision;
    // TrashDivisionのスクリプト取得
    private TrashDivision[] trashDivisionScript;

    // TrashDirectionが飛んでいく角度
    [SerializeField, Header("TrashDirectionが飛んでいく角度を決めてください")]
    private float flyAngle;
    // TrashDirectionが飛んでいく距離
    [SerializeField, Header("TrashDirectionの当たり判定が有効な距離を設定してください")]
    private float enableColliderDistance;
    // TrashDirectionを飛ばす力
    [SerializeField, Header("TrashDirectionを飛ばす力を設定してください")]
    private float flyPower;

    // 回転の最低速
    [SerializeField, Header("ローテーションの最低スピードを決めてください")]
    private float rotationMin;
    // 回転の最高速
    [SerializeField, Header("ローテーションの最大スピードを決めてください")]
    private float rotationMax;

    // 壊れるまでの時間
    [SerializeField, Header("壊れるまでの時間を設定してください")]
    private float brokenTimer;

    private ScoreManager scoreManager;

    // ヒットした方角取得用
    public Vector3 hitDirection { get; private set; }

    // DivisionがDestroyした数
    private int divisionDestroyNum;
    // Division分DestroyしてたらDestroy
    public void DestroyTrash()
    {
        divisionDestroyNum++;
        if (trashDivision.Length == divisionDestroyNum)
            Destroy(gameObject);
    }

    void Start()
    {
        // Rigitbody取得
        rigidbody = GetComponent<Rigidbody>();

        // ScoreManeger取得
        scoreManager = ScoreManager.instance;

        // trashOriginChildrenの数を決める
        trashOriginChildren = new Transform[trashOrigin.transform.childCount];
        // trashOriginChildren取得
        for (int i = 0; i < trashOrigin.transform.childCount; i++)
        {
            trashOriginChildren[i] = trashOrigin.transform.GetChild(i);
        }

        // trashDivisionScriptの数を決める
        trashDivisionScript = new TrashDivision[trashDivision.Length];
        // trashDivisionScript取得
        for (int i = 0; i < trashDivision.Length; i++)
        {
            // TrashDivision取得
            trashDivisionScript[i] = trashDivision[i].GetComponent<TrashDivision>();

            // 回転速度設定
            trashDivisionScript[i].RotateSpeed(rotationMax, rotationMin);

            // 当たり判定が有効な距離を設定します
            trashDivisionScript[i].SetDistanceFly(enableColliderDistance);

            // 壊れるまでの時間を設定します
            trashDivisionScript[i].SetBrokenTime(brokenTimer);

        }
    }

    public void ExplosionTrash()
    {
        // x軸方向に速度上書き
        //var vel = rigidbody.velocity;
        //vel.x = virtualHammer.speed;
        //velocity = vel;

        hitDirection = transform.position - PlayerDataManager.Instance.transform.position;

        // hitDirectionを角度に変換
        float hitAngle = Vec3ToAngle(hitDirection);

        // 一番最初の角度を決めます
        float firstAngle = -(trashDivision.Length - 1) * flyAngle * 0.5f;

        // divisionをoriginと同じ場所、同じ角度にする
        InitializecTransform();

        for (int i = 0; i < trashDivision.Length; i++)
        {
            float eachAngle = hitAngle + flyAngle * i + firstAngle;

            // 飛ぶ方向と力を設定します
            trashDivisionScript[i].SetDirection(AngleToVec3(eachAngle), flyPower);
        }

        // trashDivisionをON
        for (int i = 0; i < trashDivision.Length; i++)
            trashDivision[i].SetActive(true);
        // trashOriginをOff
        trashOrigin.SetActive(false);

        // 壊れた時のSE再生
        SoundManager.Instance.BrokenSound();
    }

    // divisionをoriginと同じ場所、同じ角度にする
    private void InitializecTransform()
    {
        for (int i = 0; i < trashDivision.Length; i++)
        {
            string divisionName = trashDivision[i].transform.name;
            Vector3 divisionPosition = trashDivision[i].transform.position;
            Quaternion divisionRotetion = trashDivision[i].transform.rotation;
            for (int j = 0; j < trashOriginChildren.Length; j++)
            {
                string originName = trashOriginChildren[j].transform.name;
                Vector3 originPosition = trashOriginChildren[i].transform.position;
                Quaternion originRotetion = trashOriginChildren[i].transform.rotation;

                // divisionをoriginと同じ場所、同じ角度にする
                if (divisionName == originName)
                {
                    divisionPosition = originPosition;
                    divisionRotetion = originRotetion;
                }
            }
            trashDivision[i].transform.position = divisionPosition;
            trashDivision[i].transform.rotation = divisionRotetion;
        }
    }

        // Vector3を角度に直す関数
        float Vec3ToAngle(Vector3 vec)
    {
        float angle = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;
        return angle;
    }

    Vector3 AngleToVec3(float angle)
    {
        float rad = angle * Mathf.Deg2Rad; //角度をラジアン角に変換

        //rad(ラジアン角)から発射用ベクトルを作成
        Vector3 direction = Vector3.zero;
        direction.z = (float)Math.Sin(rad);
        direction.x = (float)Math.Cos(rad);
        direction.Normalize();

        return direction;
    }
}

