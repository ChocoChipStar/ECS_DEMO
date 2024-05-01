using UnityEngine;

public class ScrapUIDisplay : MonoBehaviour
{
    // �I�u�W�F�N�g���f���J����
    private Camera targetCamera;

    // UI��\��������ΏۃI�u�W�F�N�g
    private Transform scrap;

    // Scrap�X�N���v�g�擾
    private Scrap scrapScript;

    // �\������UI
    [SerializeField,Tooltip("�\������UI���A�^�b�`���Ă�������")] 
    private Transform targetUI;

    private RectTransform parentUI;


    // ���������\�b�h�iPrefab���琶�����鎞�ȂǂɎg���j
    public void Initialize(Transform scrap, Camera targetCamera = null)
    {
        this.scrap = scrap;
        this.targetCamera = targetCamera != null ? targetCamera : Camera.main;

        // ScrapScript�ۑ�
        scrapScript = scrap.GetComponent<Scrap>();

        OnUpdatePosition();
    }

    private void Awake()
    {
        // �J�������w�肳��Ă��Ȃ���΃��C���J�����ɂ���
        if (targetCamera == null)
            targetCamera = Camera.main;

        // �eUI��RectTransform��ێ�
        parentUI = targetUI.parent.GetComponent<RectTransform>();

    }

    // UI�̈ʒu�𖈃t���[���X�V
    private void Update()
    {
        OnUpdatePosition();

        if (scrapScript.inScore) gameObject.SetActive(false);
    }

    // UI�̈ʒu���X�V����
    private void OnUpdatePosition()
    {
        var cameraTransform = targetCamera.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;
        // �I�u�W�F�N�g�̈ʒu
        var targetWorldPos = scrap.position;
        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetWorldPos - cameraTransform.position;

        // ���ς��g���ăJ�����O�����ǂ����𔻒�
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�����O���Ȃ�UI�\���A����Ȃ��\��
        targetUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = targetCamera.WorldToScreenPoint(targetWorldPos);

        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransform�̃��[�J�����W���X�V
        targetUI.localPosition = uiLocalPos;
    }
}
