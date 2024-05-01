using UnityEngine;

public class Scrap : MonoBehaviour
{
    // ScoreUIに集まる速度
    [SerializeField, Tooltip("ScoreUIに集まる速度を設定してください")]
    private float scrapAssembleSpeed;

    // オブジェクトを映すカメラ
    private Camera targetCamera;

    // ScoreUIの位置
    private RectTransform scoreUI;

    // 生成された位置保存
    private Vector3 firstPos;

    // ランダムな角度
    private float scatterAngle;

    // 散らばった後止まるタイマー
    private float stopTimer = 0.0f;

    // スコアに入った
    public bool inScore {  get; private set; }

    public void Initialize(RectTransform scoreUI, Camera targetCamera = null)
    {
        this.scoreUI = scoreUI;
        this.targetCamera = targetCamera != null ? targetCamera : Camera.main;

        InitializeSetting();

        AssenbleToScore();
    }

    private void Awake()
    {
        // カメラが指定されていなければメインカメラにする
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void Update()
    {
        if (IsStopTimer())
            AssenbleToScore();
        else
            ScrapScttered();

        if (inScore)
        {
            ScoreManager.instance.AddScore();
            Destroy(gameObject);
        }
    }
    // 初期設定
    private void InitializeSetting()
    {
        // 生成されて位置保存
        firstPos = transform.position;
        // ランダムな角度を生成
        scatterAngle = Random.Range(0.0f, 360.0f);
    }

    // ランダムな角度に散らばる
    private void ScrapScttered()
    {
        // 自身の位置
        var trasformPos = transform.position;

        // 角度をベクトルに直す
        var scatterVec2 = Vector2.zero;
        scatterVec2.x = Mathf.Cos(scatterAngle);
        scatterVec2.y = Mathf.Sin(scatterAngle);
        scatterVec2.Normalize();
        // Vector3に変換
        var scatterVec3 = Vector3.zero;
        scatterVec3.x = scatterVec2.x;
        scatterVec3.z = scatterVec2.y;

        float scatterSpeed = 15 * Time.deltaTime;

        if (!WasSctter())
            // 散らばる
            trasformPos += scatterVec3 * scatterSpeed;

        // 自身に値を返す
        transform.position = trasformPos;
    }

    // 散らばると一定時間止まる
    private bool IsStopTimer()
    {
        if (WasSctter())
            // タイマー進行
            stopTimer += Time.deltaTime;
        // 一定時間で動く
        var isStopTimer = stopTimer > 1.0f;
        if (isStopTimer) return true;

        return false;
    }

    // 一定距離まで散らばった判定
    private bool WasSctter()
    {
        // 自身の位置
        var trasformPos = transform.position;
        // 散らばる距離   
        var scatterDis = 2.0f;
        // 散らばった
        var wasSctter = (firstPos - trasformPos).magnitude > scatterDis;

        return wasSctter;
    }

    // ScoreUIに集まる
    private void AssenbleToScore()
    {
        // 自身の位置
        var transformPos = transform.position;

        // ScrapUIの位置
        var scoreUIScreenPos = scoreUI.position;
        // 自身のワールド座標→スクリーン座標変換
        var targetScreenPos = targetCamera.WorldToScreenPoint(transformPos);

        //  スクリーン座標上での自身からScrapUIへのベクトル
        var moveScreenVec = scoreUIScreenPos - targetScreenPos;
        // 十分近づいていたら動かない
        if (moveScreenVec.magnitude < 5) { inScore = true; return; }
        // 正規化
        moveScreenVec.Normalize();

        // 集まるスピード
        var scrapAssembleSpeed = this.scrapAssembleSpeed * 100;
        // 自身のスクリーン位置をScrapUIに向けて動かす
        targetScreenPos += moveScreenVec * scrapAssembleSpeed * Time.deltaTime;

        // 自身のスクリーン座標→ワールド座標変換
        transformPos = targetCamera.ScreenToWorldPoint(targetScreenPos);

        // 自身の位置を返す
        transform.position = transformPos;

    }
}
