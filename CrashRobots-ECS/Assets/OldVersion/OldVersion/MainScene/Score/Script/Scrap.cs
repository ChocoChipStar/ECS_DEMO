using UnityEngine;

public class Scrap : MonoBehaviour
{
    // ScoreUI�ɏW�܂鑬�x
    [SerializeField, Tooltip("ScoreUI�ɏW�܂鑬�x��ݒ肵�Ă�������")]
    private float scrapAssembleSpeed;

    // �I�u�W�F�N�g���f���J����
    private Camera targetCamera;

    // ScoreUI�̈ʒu
    private RectTransform scoreUI;

    // �������ꂽ�ʒu�ۑ�
    private Vector3 firstPos;

    // �����_���Ȋp�x
    private float scatterAngle;

    // �U��΂�����~�܂�^�C�}�[
    private float stopTimer = 0.0f;

    // �X�R�A�ɓ�����
    public bool inScore {  get; private set; }

    public void Initialize(RectTransform scoreUI, Camera targetCamera = null)
    {
        this.scoreUI = scoreUI;
        this.targetCamera = targetCamera != null ? targetCamera : Camera.main;

        InitializeSetting();

        AssenbleToScore();
    }

    private void Awake()
    {
        // �J�������w�肳��Ă��Ȃ���΃��C���J�����ɂ���
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void Update()
    {
        if (IsStopTimer())
            AssenbleToScore();
        else
            ScrapScttered();

        if (inScore)
        {
            ScoreManager.instance.AddScore();
            Destroy(gameObject);
        }
    }
    // �����ݒ�
    private void InitializeSetting()
    {
        // ��������Ĉʒu�ۑ�
        firstPos = transform.position;
        // �����_���Ȋp�x�𐶐�
        scatterAngle = Random.Range(0.0f, 360.0f);
    }

    // �����_���Ȋp�x�ɎU��΂�
    private void ScrapScttered()
    {
        // ���g�̈ʒu
        var trasformPos = transform.position;

        // �p�x���x�N�g���ɒ���
        var scatterVec2 = Vector2.zero;
        scatterVec2.x = Mathf.Cos(scatterAngle);
        scatterVec2.y = Mathf.Sin(scatterAngle);
        scatterVec2.Normalize();
        // Vector3�ɕϊ�
        var scatterVec3 = Vector3.zero;
        scatterVec3.x = scatterVec2.x;
        scatterVec3.z = scatterVec2.y;

        float scatterSpeed = 15 * Time.deltaTime;

        if (!WasSctter())
            // �U��΂�
            trasformPos += scatterVec3 * scatterSpeed;

        // ���g�ɒl��Ԃ�
        transform.position = trasformPos;
    }

    // �U��΂�ƈ�莞�Ԏ~�܂�
    private bool IsStopTimer()
    {
        if (WasSctter())
            // �^�C�}�[�i�s
            stopTimer += Time.deltaTime;
        // ��莞�Ԃœ���
        var isStopTimer = stopTimer > 1.0f;
        if (isStopTimer) return true;

        return false;
    }

    // ��苗���܂ŎU��΂�������
    private bool WasSctter()
    {
        // ���g�̈ʒu
        var trasformPos = transform.position;
        // �U��΂鋗��   
        var scatterDis = 2.0f;
        // �U��΂���
        var wasSctter = (firstPos - trasformPos).magnitude > scatterDis;

        return wasSctter;
    }

    // ScoreUI�ɏW�܂�
    private void AssenbleToScore()
    {
        // ���g�̈ʒu
        var transformPos = transform.position;

        // ScrapUI�̈ʒu
        var scoreUIScreenPos = scoreUI.position;
        // ���g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = targetCamera.WorldToScreenPoint(transformPos);

        //  �X�N���[�����W��ł̎��g����ScrapUI�ւ̃x�N�g��
        var moveScreenVec = scoreUIScreenPos - targetScreenPos;
        // �\���߂Â��Ă����瓮���Ȃ�
        if (moveScreenVec.magnitude < 5) { inScore = true; return; }
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
}
