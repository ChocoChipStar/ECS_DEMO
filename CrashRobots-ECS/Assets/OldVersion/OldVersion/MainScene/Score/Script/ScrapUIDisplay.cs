using UnityEngine;

public class ScrapUIDisplay : MonoBehaviour
{
    // オブジェクトを映すカメラ
    private Camera targetCamera;

    // UIを表示させる対象オブジェクト
    private Transform scrap;

    // Scrapスクリプト取得
    private Scrap scrapScript;

    // 表示するUI
    [SerializeField,Tooltip("表示するUIをアタッチしてください")] 
    private Transform targetUI;

    private RectTransform parentUI;


    // 初期化メソッド（Prefabから生成する時などに使う）
    public void Initialize(Transform scrap, Camera targetCamera = null)
    {
        this.scrap = scrap;
        this.targetCamera = targetCamera != null ? targetCamera : Camera.main;

        // ScrapScript保存
        scrapScript = scrap.GetComponent<Scrap>();

        OnUpdatePosition();
    }

    private void Awake()
    {
        // カメラが指定されていなければメインカメラにする
        if (targetCamera == null)
            targetCamera = Camera.main;

        // 親UIのRectTransformを保持
        parentUI = targetUI.parent.GetComponent<RectTransform>();

    }

    // UIの位置を毎フレーム更新
    private void Update()
    {
        OnUpdatePosition();

        if (scrapScript.inScore) gameObject.SetActive(false);
    }

    // UIの位置を更新する
    private void OnUpdatePosition()
    {
        var cameraTransform = targetCamera.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;
        // オブジェクトの位置
        var targetWorldPos = scrap.position;
        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
        targetUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = targetCamera.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        targetUI.localPosition = uiLocalPos;
    }
}
