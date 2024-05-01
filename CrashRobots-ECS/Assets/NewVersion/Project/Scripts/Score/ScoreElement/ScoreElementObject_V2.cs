using UnityEngine;

public class ScoreElementObject_V2 : MonoBehaviour
{
    [SerializeField, Header("ScoreUIに集まる速度を設定してください")]
    private float scrapAssembleSpeed;
    [SerializeField, Header("散らばるの距離")]
    private float scatterDis = 2.0f;
    [SerializeField, Header("散らばる速度")]
    private float scatterSpeed = 15.0f;
    [SerializeField, Header("散らばった後止まっている時間")]
    private float stopSctterTime = 1.0f;
    // 生成された位置保存
    private Vector3 firstPos;
    // ランダムな角度
    private float scatterAngle;

    // オブジェクトを映すカメラ
    private Camera targetCamera;
    // ScoreUIの位置
    private RectTransform scoreUIRectTransform;
    public void Initialize(RectTransform scoreUI = null, Camera targetCamera = null)
    {
        // scoreUI取得
        this.scoreUIRectTransform = scoreUI != null ? scoreUI : ScoreManager_V2.Instance.scoreUI.rectTransform;
        // camera取得
        this.targetCamera = targetCamera != null ? targetCamera : Camera.main;
        // 生成された位置保存
        firstPos = transform.position;
        // ランダムな角度を生成
        scatterAngle = Random.Range(0.0f, 360.0f);
    }

    private void Awake()
    {
        Initialize();
    }

    void Update()
    {
        // Position取得
        var position = transform.position;

        // ランダムな角度に散らばる
        ScoreElementSctteredSystem(position, out var wasSctter);

        // 散らばって無かったらreturn
        if (!wasSctter) return;
        // 止まってる間の時間計測
        if (IsStop()) return;

        // scoreUIに向かって集まる
        AssenbleToScoreUISystem(out var isInScore);

        // 集まって無かったらreturn
        if (!isInScore) return;

        // スコア加算
        ScoreManager_V2.Instance.AddScore(100);
        // 自信を削除
        Destroy(gameObject);
    }

    // ランダムな角度に散らばる
    private void ScoreElementSctteredSystem(Vector3 position, out bool wasSctter)
    {
        // 一定距離まで散らばったらtrue
        wasSctter = WasSctter(position);
        // 散らばった後ならreturn
        if (wasSctter) return;

        // Vector3に変換
        var scatterVec3 = Vector3.zero;
        scatterVec3.x = Mathf.Cos(scatterAngle);
        scatterVec3.z = Mathf.Sin(scatterAngle);
        scatterVec3.Normalize();

        // 散らばる
        position += scatterVec3 * scatterSpeed * Time.deltaTime;
        // 自身に値を返す
        transform.position = position;
    }
    // 一定距離まで散らばったらtrue
    private bool WasSctter(Vector3 position)
    {
        // 散らばった
        var wasSctter = (firstPos - position).magnitude > scatterDis;
        return wasSctter;
    }


    // ScoreUIに集まる
    private void AssenbleToScoreUISystem(out bool isInScore)
    {
        // 自身の位置
        var transformPos = transform.position;

        // ScrapUIの位置
        var scoreUIScreenPos = scoreUIRectTransform.position;
        // 自身のワールド座標→スクリーン座標変換
        var targetScreenPos = targetCamera.WorldToScreenPoint(transformPos);

        //  スクリーン座標上での自身からScrapUIへのベクトル
        var moveScreenVec = scoreUIScreenPos - targetScreenPos;
        // 十分近づいていたら動かない
        if (moveScreenVec.magnitude < 5) { isInScore = true; return; }
        else isInScore = false;
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
    // 散らばった後止まるタイマー
    private float stopSctterTimeTimer = 0.0f;
    // StopTimeTimerが一定時間超えるとtrue
    private bool IsStop()
    {
        // 時間計測
        stopSctterTimeTimer += Time.deltaTime;
        // 指定の時間を超えたらtrue
        var isStopTimerElpsed = stopSctterTimeTimer > stopSctterTime;
        return isStopTimerElpsed;
    }
}
