using UnityEngine;

public class ScoreElementUIDisplaySysem_V2 : MonoBehaviour
{
    [SerializeField, Tooltip("�X�R�A�̌��ƂȂ�UI���A�^�b�`���Ă�������")]
    private Transform scoreElementUI;
    // scoreElementCanvas�擾
    private RectTransform scoreElementCanvas;
    // �I�u�W�F�N�g���f���J����
    private Camera camera;
    // UI��\��������ΏۃI�u�W�F�N�g
    private Transform scoreElementObject;
    // Scrap�X�N���v�g�擾
    private ScoreElementObject_V2 scoreElementObject_V2;


    // ���������\�b�h�iPrefab���琶�����鎞�ȂǂɎg���j
    public void Initialize(Transform scoreElementObject, Camera camera = null)
    {
        this.scoreElementObject = scoreElementObject;
        this.camera = camera != null ? camera : Camera.main;
        // ScrapScript�ۑ�
        scoreElementObject_V2 = scoreElementObject.GetComponent<ScoreElementObject_V2>();
        // �ʒu������
        OnUpdatePosition();
    }

    private void Awake()
    {
        // �J�������w�肳��Ă��Ȃ���΃��C���J�����ɂ���
        if (camera == null)
            camera = Camera.main;

        // �eUI��RectTransform��ێ�
        scoreElementCanvas = scoreElementUI.parent.GetComponent<RectTransform>();
    }

    // UI�̈ʒu�𖈃t���[���X�V
    private void Update()
    {
        OnUpdatePosition();

        //if (scrapScript.inScore) gameObject.SetActive(false);
    }

    // UI�̈ʒu���X�V����
    private void OnUpdatePosition()
    {
        var cameraTransform = camera.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;
        // �I�u�W�F�N�g�̈ʒu
        var targetWorldPos = scoreElementObject.position;
        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetWorldPos - cameraTransform.position;

        // ���ς��g���ăJ�����O�����ǂ����𔻒�
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�����O���Ȃ�UI�\���A����Ȃ��\��
        scoreElementUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = camera.WorldToScreenPoint(targetWorldPos);

        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            scoreElementCanvas,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransform�̃��[�J�����W���X�V
        scoreElementUI.localPosition = uiLocalPos;
    }
}
