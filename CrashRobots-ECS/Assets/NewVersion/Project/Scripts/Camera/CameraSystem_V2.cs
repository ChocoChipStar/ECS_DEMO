using System.Collections;
using UnityEngine;
using PlayerData = PlayerDataManager_V2;

public class CameraSystem_V2 : MonoBehaviour
{
    [SerializeField, Header("�J�����̍�����ݒ�ł��܂�")]
    private float cameraPosY;

    [SerializeField, Header("�J�����̈�����𒲐��ł��܂�")]
    private float cameraPosZ;

    [SerializeField,Header("�J�����A���O����ݒ�ł��܂�")]
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
    /// ���C���J�����̉E��[���[���h���W���擾���܂�
    /// </summary>
    public Vector3 GetCameraRightTop()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(Screen.width , Screen.height, mainCamera.farClipPlane - ALLOWABLE_LIMIT));
    }

    /// <summary>
    /// ���C���J�����̍����[���[���h���W���擾���܂�
    /// </summary>
    public Vector3 GetCameraLeftBottom()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.farClipPlane - ALLOWABLE_LIMIT));
    }

    /// <summary>
    /// ��ʗh��̏������s���܂�
    /// </summary>
    /// <param name="duration">��ʗh��̒��������i�b�j</param>
    /// <param name="magnitude">��ʗh��̋�������</param>
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
