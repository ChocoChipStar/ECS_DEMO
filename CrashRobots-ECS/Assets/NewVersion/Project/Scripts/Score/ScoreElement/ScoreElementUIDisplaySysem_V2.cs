using UnityEngine;

public class ScoreElementUIDisplaySysem_V2 : MonoBehaviour
{
    [SerializeField, Tooltip("スコアの元となるUIをアタッチしてください")]
    private Transform scoreElementUI;
    // scoreElementCanvas取得
    private RectTransform scoreElementCanvas;
    // オブジェクトを映すカメラ
    private Camera camera;
    // UIを表示させる対象オブジェクト
    private Transform scoreElementObject;
    // Scrapスクリプト取得
    private ScoreElementObject_V2 scoreElementObject_V2;


    // 初期化メソッド（Prefabから生成する時などに使う）
    public void Initialize(Transform scoreElementObject, Camera camera = null)
    {
        this.scoreElementObject = scoreElementObject;
        this.camera = camera != null ? camera : Camera.main;
        // ScrapScript保存
        scoreElementObject_V2 = scoreElementObject.GetComponent<ScoreElementObject_V2>();
        // 位置初期化
        OnUpdatePosition();
    }

    private void Awake()
    {
        // カメラが指定されていなければメインカメラにする
        if (camera == null)
            camera = Camera.main;

        // 親UIのRectTransformを保持
        scoreElementCanvas = scoreElementUI.parent.GetComponent<RectTransform>();
    }

    // UIの位置を毎フレーム更新
    private void Update()
    {
        OnUpdatePosition();

        //if (scrapScript.inScore) gameObject.SetActive(false);
    }

    // UIの位置を更新する
    private void OnUpdatePosition()
    {
        var cameraTransform = camera.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;
        // オブジェクトの位置
        var targetWorldPos = scoreElementObject.position;
        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
        scoreElementUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = camera.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            scoreElementCanvas,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        scoreElementUI.localPosition = uiLocalPos;
    }
}
