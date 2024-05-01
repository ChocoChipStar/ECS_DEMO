using System.Collections;
using UnityEngine;
using PlayerData = PlayerDataManager_V2;

public class CameraSystem_V2 : MonoBehaviour
{
    [SerializeField, Header("カメラの高さを設定できます")]
    private float cameraPosY;

    [SerializeField, Header("カメラの引き具合を調整できます")]
    private float cameraPosZ;

    [SerializeField,Header("カメラアングルを設定できます")]
    private float cameraAngle;

    [SerializeField]
    private Camera mainCamera;

    private float ALLOWABLE_LIMIT = 15.0f;

    private void Update()
    {
        var currentPos = mainCamera.transform.position;
        var currentRot = mainCamera.transform.eulerAngles;

        currentPos = new Vector3(0.0f, cameraPosY, 0.0f);
        currentRot.x = cameraAngle;

        mainCamera.transform.position = currentPos;
        mainCamera.transform.eulerAngles = currentRot;
    }

    /// <summary>
    /// メインカメラの右上端ワールド座標を取得します
    /// </summary>
    public Vector3 GetCameraRightTop()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(Screen.width , Screen.height, mainCamera.farClipPlane - ALLOWABLE_LIMIT));
    }

    /// <summary>
    /// メインカメラの左下端ワールド座標を取得します
    /// </summary>
    public Vector3 GetCameraLeftBottom()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.farClipPlane - ALLOWABLE_LIMIT));
    }

    /// <summary>
    /// 画面揺れの処理を行います
    /// </summary>
    /// <param name="duration">画面揺れの長さを代入（秒）</param>
    /// <param name="magnitude">画面揺れの強さを代入</param>
    /// <returns></returns>
    public IEnumerator ShakeSystem(float duration, float magnitude)
    {
        var initialPosition  = mainCamera.transform.position;
        var elpTime = 0.0f;

        while(elpTime < duration)
        {
            mainCamera.transform.position = initialPosition + Random.insideUnitSphere * magnitude;
            elpTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = initialPosition;
    }
}
