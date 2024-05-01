using UnityEngine;
using UnityEngine.UIElements;

public class EnemyGenerateSystem_V2 : MonoBehaviour
{
    // �L���郉�C��
    public enum ExpandingLine { Up, Down, Right, Left }
    [System.Serializable]
    public struct EnemyStatus
    {
        [SerializeField, Header("���O����͂��Ă�������")]
        public string name;
        [SerializeField, Header("Enemy���A�^�b�`���ĉ�����")]
        public GameObject gameObject;
        [SerializeField, Header("�����X�p������͂��Ă�������[s]")]
        public float generateCoolTime;
        // Timer
        public float generateCoolTimeTimer { get; set; }
        [SerializeField, Header("�����ɐ�������m��[%]")]
        public float GeneerateProbability;
        [SerializeField, Header("�L���郉�C����ݒ肵�Ă�������")]
        public ExpandingLine expandingLine;
        [SerializeField, Header("�ŏ�Position")]
        public float minPos;
        [SerializeField, Header("�ő�Position")]
        public float maxPos;
    }
    [SerializeField, Header("Enemy�̃X�e�[�^�X����͂��Ă�������")]
    private EnemyStatus[] status;

    private bool isFirst = false;

    void Update()
    {
        for(int i  = 0; i < status.Length; i++)
        {
            GenerateSystem(i);
        }
    }

    public bool firstGanarate { get; private set; } = false;
    /// <summary>
    /// �����V�X�e��
    /// </summary>
    /// <param name="i"></param>
    private void GenerateSystem(int i)
    {
        if(isFirst)
        {
            var CoolTimeElapsed = CoolTimeTimerCountSystem(i) >= status[i].generateCoolTime;
            if (!CoolTimeElapsed) return;
            if (!RandomGenerate(i)) { ResetCoolTimeTimer(i); return; }
            firstGanarate = true;
            GetGeneratePositionAndQuaternion(i, out var generatePos, out var generateQuaternion);
            var gameObjectPrefab = Instantiate(status[i].gameObject, transform.position + generatePos, generateQuaternion);
            ResetCoolTimeTimer(i);
        }
        else
        {
            if (!RandomGenerate(i)) { ResetCoolTimeTimer(i); return; }
            firstGanarate = true;
            GetGeneratePositionAndQuaternion(i, out var generatePos, out var generateQuaternion);
            var gameObjectPrefab = Instantiate(status[i].gameObject, transform.position + generatePos, generateQuaternion);
            ResetCoolTimeTimer(i);

            isFirst = true;
        }
        
    }
    /// <summary>
    /// ���Ԍv��
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private float CoolTimeTimerCountSystem(int i)
    {
        status[i].generateCoolTimeTimer += Time.deltaTime;
        return status[i].generateCoolTimeTimer;
    }
    /// <summary>
    /// Timer���Z�b�g
    /// </summary>
    /// <param name="i"></param>
    private void ResetCoolTimeTimer(int i)
    {
        status[i].generateCoolTimeTimer = 0;
    }
    /// <summary>
    /// �����_��
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private bool RandomGenerate(int i)
    {
        var random = Random.Range(0.0f, 100.0f);
        var isGenerateSuccess = random < status[i].GeneerateProbability;
        return isGenerateSuccess;
    }
    private void GetGeneratePositionAndQuaternion(int i, out Vector3 GeneratePos, out Quaternion quaternion)
    {
        GeneratePos = Vector3.zero;
        quaternion = Quaternion.identity;
        var angle = quaternion.eulerAngles;
        switch (status[i].expandingLine)
        {
            case ExpandingLine.Up:
                GeneratePos.x = RandomOneLinePosition(i);
                angle.y = 180.0f;
                quaternion = Quaternion.Euler(angle);
                return;
            case ExpandingLine.Down:
                GeneratePos.x = RandomOneLinePosition(i);
                angle.y = 0.0f;
                quaternion = Quaternion.Euler(angle);
                return;
            case ExpandingLine.Right:
                GeneratePos.z = RandomOneLinePosition(i);
                angle.y = -90.0f;
                quaternion = Quaternion.Euler(angle);
                return;
            case ExpandingLine.Left:
                GeneratePos.z = RandomOneLinePosition(i);
                angle.y = 90.0f;
                quaternion = Quaternion.Euler(angle);
                return;
            default:
                return;
        }
    }
    private float RandomOneLinePosition(int i)
    {
        var randomOneLinePosition = 0.0f;
        randomOneLinePosition = Random.Range(status[i].minPos, status[i].maxPos);
        return randomOneLinePosition;
    }

}
