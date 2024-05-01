using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSpawn : MonoBehaviour
{
    public MeshRenderer mr;

    //オブジェクトプレハブ
    public GameObject PlayerObject;

    public GameObject refrigeratorPrefab;
    public GameObject carPrefab;

    GameObject[] tagObjects;

    //敵生成時間間隔
    [SerializeField]
    private float interval = 3;
    //経過時間
    private float time = 0f;

    public int upperlimit = 30;

    private float carpetSizX;
    private float carpetSizZ;

    private float mww = 3.5f; // 壁との余白(margin with wall)

    private float cameraX = 16;
    private float cameraY = 9;

    float cameraDistance;

    private bool dis;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;

        carpetSizX = transform.lossyScale.x;
        carpetSizZ = transform.lossyScale.z;

        cameraDistance = Mathf.Sqrt(Mathf.Pow(cameraX, 2.0f) + Mathf.Pow(cameraY, 2)); // √(cameraX^2 + cameraY^2)

    }

    // Update is called once per frame
    void Update()
    {
        tagObjects = GameObject.FindGameObjectsWithTag("CanBreakTrash");
        if (tagObjects.Length <= upperlimit)
        {
            //時間計測
            time += Time.deltaTime;

            //経過時間が生成時間になったとき(生成時間より大きくなったとき)
            if (time >= interval)
            {
                // 生成ポジション
                Vector3 pos = Vector3.zero;

                // オブジェクトがカメラ内かどうか
                if (IsObjectOnScreenOll()) dis = false;
                else
                {
                    // カメラ外になるまでランダム探索する
                    while (IsInScreen(pos))
                        pos = RandomPos(pos);

                    dis = true;
                }

                if (dis)
                {
                    RedSpownInstatiate(pos);
                }
                dis = false;
                //経過時間を初期化して再度時間計測を始める
                time = 0f;
            }
        }
    }

    // このオブジェクトが端から端までカメラ内かの判定
    private bool IsObjectOnScreenOll()
    {
        // オブジェクトのposition取得
        var positon = transform.position;
        // オブジェクトの角までの距離
        var cornerDistance = transform.lossyScale / 2;

        // 右上の角判定
        var righitUpCorner = positon + cornerDistance;
        righitUpCorner.x -= mww;
        righitUpCorner.y -= mww;
        var isRightUpCornerOnScreen = IsInScreen(righitUpCorner);
        // 左下の角判定
        var leftDownCorner = positon - cornerDistance;
        leftDownCorner.x += mww;
        leftDownCorner.y = -leftDownCorner.y + mww;
        var isLeftDownCornerOnScreen = IsInScreen(leftDownCorner);

        // このオブジェクトが端から端までカメラ内かの判定
        var isObjectOnScreenOll = isRightUpCornerOnScreen && isLeftDownCornerOnScreen;

        return isObjectOnScreenOll;
    }

    // ランダムなスポーンポジション習得
    private Vector3 RandomPos(Vector3 pos)
    {
        float carpetX = Random.Range(-carpetSizX / 2 + mww, carpetSizX / 2 - mww);
        float carpetZ = Random.Range(-carpetSizZ / 2 + mww, carpetSizZ / 2 - mww);
        pos = new Vector3(transform.position.x + carpetX, transform.position.y, transform.position.z + carpetZ);
        //float distance = Vector3.Distance(PlayerObject.transform.position, pos);

        return pos;
    }

    // カメラ内判定
    private bool IsInScreen(Vector3 pos)
    {
        // カメラposition取得
        var camera = Camera.main.transform;
        // カメラの向き
        var cameraDir = camera.forward;

        // posをスクリーン座標にする
        var screenPos = Camera.main.WorldToScreenPoint(pos);
        // 画面内か判定する
        bool ouScreenX = 0 <= screenPos.x && screenPos.x <= Screen.width;
        bool ouScreenY = 0 <= screenPos.y && screenPos.y <= Screen.height;
        bool onScreen = ouScreenY && ouScreenX;

        // カメラからオブジェクトへの向き
        var posDir = pos - camera.position;
        // posがカメラの前にあるかの判定
        bool cameraFront = Vector3.Dot(cameraDir, posDir) > 0;

        // 画面内かつカメラの前判定
        var onScreenAndCameraFront = onScreen && cameraFront;

        return onScreenAndCameraFront;
    }

    // スポーンさせる
    private void RedSpownInstatiate(Vector3 pos)
    {
        float rotateY = Random.Range(0, 360);

        float probability = Random.Range(1, 11);

        if (probability <= 4)
        {
            pos.y = 1.5f;
            //enemyをインスタンス化する(生成する)
            GameObject trash = Instantiate(refrigeratorPrefab, pos, Quaternion.Euler(-90, rotateY, 0));
        }
        else
        {
            //enemyをインスタンス化する(生成する)
            GameObject trash = Instantiate(carPrefab, pos, Quaternion.Euler(0, rotateY, 0));
        }
    }

}
