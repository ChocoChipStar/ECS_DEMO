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
    public Camera mainCamera;

    private float ALLOWABLE_LIMIT = 15.0f;

    private void Update()
    {
        var instancePos = PlayerData.Instance.transform.position;
        var playerPos = new Vector2(instancePos.x, instancePos.z);
        var cameraPos = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);

        var distance = playerPos - cameraPos;
        var radian = Mathf.Atan2(distance.y, distance.x);
        var degree = radian * Mathf.Rad2Deg;

        var cameraRot = mainCamera.transform.eulerAngles;
        cameraRot.y = -degree + 90.0f;
        mainCamera.transform.eulerAngles = cameraRot;
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
