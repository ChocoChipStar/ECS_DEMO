using UnityEngine;

public class EnemyVisibleChecker : MonoBehaviour
{
    // カメラ内外フラグ
    private bool isInsideCamera;
    public bool IsInsideCamera { get { return isInsideCamera; } }

    // Camera取得
    private Camera camera;

    private void Initialized(Camera camera = null)
    {
        // camera取得
        this.camera = camera != null ? camera : Camera.main;
        // isInsideCameraオフ
        isInsideCamera = JudgOnVisible();
    }

    private void Start()
    {
        Initialized();
    }

    private void Update()
    {
        // trueになったら処理を終わる
        if (isInsideCamera) return;
        // 画面内判定
        isInsideCamera = JudgOnVisible();
    }

    private bool JudgOnVisible()
    {
        var cameraTransform = camera.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;
        // オブジェクトの位置
        var myWorldPos = gameObject.transform.position;
        // カメラからターゲットへのベクトル
        var targetDir = myWorldPos - cameraTransform.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // 前方でないならfalse
        if (!isFront) return false;

        // オブジェクトのワールド座標→スクリーン座標変換
        var myScreenPos = camera.WorldToScreenPoint(myWorldPos);

        // オブジェクトが画面内かどうか判定
        var isMyPosXIntoScreen = 0 <= myScreenPos.x && myScreenPos.x <= camera.scaledPixelWidth;
        var isMyPosYIntoScreen = 0 <= myScreenPos.y && myScreenPos.y <= camera.scaledPixelHeight;
        var isMyPosintoScreen  = isMyPosXIntoScreen && isMyPosYIntoScreen;

        // 画面内で無いならfalse
        if (!isMyPosintoScreen) return false;

        // 前方かつ画面内
        return true;
    }
}