using UnityEngine;

public class ScoreElementObject_V2 : MonoBehaviour
{
    [SerializeField, Header("ScoreUI�ɏW�܂鑬�x��ݒ肵�Ă�������")]
    private float scrapAssembleSpeed;
    [SerializeField, Header("�U��΂�̋���")]
    private float scatterDis = 2.0f;
    [SerializeField, Header("�U��΂鑬�x")]
    private float scatterSpeed = 15.0f;
    [SerializeField, Header("�U��΂�����~�܂��Ă��鎞��")]
    private float stopSctterTime = 1.0f;
    // �������ꂽ�ʒu�ۑ�
    private Vector3 firstPos;
    // �����_���Ȋp�x
    private float scatterAngle;

    // �I�u�W�F�N�g���f���J����
    private Camera targetCamera;
    // ScoreUI�̈ʒu
    private RectTransform scoreUIRectTransform;
    public void Initialize(RectTransform scoreUI = null, Camera targetCamera = null)
    {
        // scoreUI�擾
        this.scoreUIRectTransform = scoreUI != null ? scoreUI : ScoreManager_V2.Instance.scoreUI.rectTransform;
        // camera�擾
        this.targetCamera = targetCamera != null ? targetCamera : Camera.main;
        // �������ꂽ�ʒu�ۑ�
        firstPos = transform.position;
        // �����_���Ȋp�x�𐶐�
        scatterAngle = Random.Range(0.0f, 360.0f);
    }

    private void Awake()
    {
        Initialize();
    }

    void Update()
    {
        // Position�擾
        var position = transform.position;

        // �����_���Ȋp�x�ɎU��΂�
        ScoreElementSctteredSystem(position, out var wasSctter);

        // �U��΂��Ė���������return
        if (!wasSctter) return;
        // �~�܂��Ă�Ԃ̎��Ԍv��
        if (IsStop()) return;

        // scoreUI�Ɍ������ďW�܂�
        AssenbleToScoreUISystem(out var isInScore);

        // �W�܂��Ė���������return
        if (!isInScore) return;

        // �X�R�A���Z
        ScoreManager_V2.Instance.AddScore(100);
        // ���M���폜
        Destroy(gameObject);
    }

    // �����_���Ȋp�x�ɎU��΂�
    private void ScoreElementSctteredSystem(Vector3 position, out bool wasSctter)
    {
        // ��苗���܂ŎU��΂�����true
        wasSctter = WasSctter(position);
        // �U��΂�����Ȃ�return
        if (wasSctter) return;

        // Vector3�ɕϊ�
        var scatterVec3 = Vector3.zero;
        scatterVec3.x = Mathf.Cos(scatterAngle);
        scatterVec3.z = Mathf.Sin(scatterAngle);
        scatterVec3.Normalize();

        // �U��΂�
        position += scatterVec3 * scatterSpeed * Time.deltaTime;
        // ���g�ɒl��Ԃ�
        transform.position = position;
    }
    // ��苗���܂ŎU��΂�����true
    private bool WasSctter(Vector3 position)
    {
        // �U��΂���
        var wasSctter = (firstPos - position).magnitude > scatterDis;
        return wasSctter;
    }


    // ScoreUI�ɏW�܂�
    private void AssenbleToScoreUISystem(out bool isInScore)
    {
        // ���g�̈ʒu
        var transformPos = transform.position;

        // ScrapUI�̈ʒu
        var scoreUIScreenPos = scoreUIRectTransform.position;
        // ���g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = targetCamera.WorldToScreenPoint(transformPos);

        //  �X�N���[�����W��ł̎��g����ScrapUI�ւ̃x�N�g��
        var moveScreenVec = scoreUIScreenPos - targetScreenPos;
        // �\���߂Â��Ă����瓮���Ȃ�
        if (moveScreenVec.magnitude < 5) { isInScore = true; return; }
        else isInScore = false;
        // ���K��
        moveScreenVec.Normalize();

        // �W�܂�X�s�[�h
        var scrapAssembleSpeed = this.scrapAssembleSpeed * 100;
        // ���g�̃X�N���[���ʒu��ScrapUI�Ɍ����ē�����
        targetScreenPos += moveScreenVec * scrapAssembleSpeed * Time.deltaTime;

        // ���g�̃X�N���[�����W�����[���h���W�ϊ�
        transformPos = targetCamera.ScreenToWorldPoint(targetScreenPos);

        // ���g�̈ʒu��Ԃ�
        transform.position = transformPos;

    }
    // �U��΂�����~�܂�^�C�}�[
    private float stopSctterTimeTimer = 0.0f;
    // StopTimeTimer����莞�Ԓ������true
    private bool IsStop()
    {
        // ���Ԍv��
        stopSctterTimeTimer += Time.deltaTime;
        // �w��̎��Ԃ𒴂�����true
        var isStopTimerElpsed = stopSctterTimeTimer > stopSctterTime;
        return isStopTimerElpsed;
    }
}
