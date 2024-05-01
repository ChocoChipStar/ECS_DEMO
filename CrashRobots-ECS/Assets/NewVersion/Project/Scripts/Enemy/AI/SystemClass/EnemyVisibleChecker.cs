using UnityEngine;

public class EnemyVisibleChecker : MonoBehaviour
{
    // �J�������O�t���O
    private bool isInsideCamera;
    public bool IsInsideCamera { get { return isInsideCamera; } }

    // Camera�擾
    private Camera camera;

    private void Initialized(Camera camera = null)
    {
        // camera�擾
        this.camera = camera != null ? camera : Camera.main;
        // isInsideCamera�I�t
        isInsideCamera = JudgOnVisible();
    }

    private void Start()
    {
        Initialized();
    }

    private void Update()
    {
        // true�ɂȂ����珈�����I���
        if (isInsideCamera) return;
        // ��ʓ�����
        isInsideCamera = JudgOnVisible();
    }

    private bool JudgOnVisible()
    {
        var cameraTransform = camera.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;
        // �I�u�W�F�N�g�̈ʒu
        var myWorldPos = gameObject.transform.position;
        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = myWorldPos - cameraTransform.position;

        // ���ς��g���ăJ�����O�����ǂ����𔻒�
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �O���łȂ��Ȃ�false
        if (!isFront) return false;

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var myScreenPos = camera.WorldToScreenPoint(myWorldPos);

        // �I�u�W�F�N�g����ʓ����ǂ�������
        var isMyPosXIntoScreen = 0 <= myScreenPos.x && myScreenPos.x <= camera.scaledPixelWidth;
        var isMyPosYIntoScreen = 0 <= myScreenPos.y && myScreenPos.y <= camera.scaledPixelHeight;
        var isMyPosintoScreen  = isMyPosXIntoScreen && isMyPosYIntoScreen;

        // ��ʓ��Ŗ����Ȃ�false
        if (!isMyPosintoScreen) return false;

        // �O������ʓ�
        return true;
    }
}