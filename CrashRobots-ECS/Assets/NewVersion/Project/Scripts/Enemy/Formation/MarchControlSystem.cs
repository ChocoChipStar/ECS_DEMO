using UnityEngine;

public class MarchControlSystem : MonoBehaviour
{
    [SerializeField, Header("�i�R���x��ݒ肵�Ă�������")]
    private float marchSpeed;

    [SerializeField, Header("�i�R�J�n�_��ݒ肵�Ă�������")]
    private MarchingPoint marchingPoint;
    // �J�n�n�_�I��
    public enum MarchingPoint { front, back, side ,stand }

    // StageScrollTest�擾
    private StageScrollTest stageScrollSystem;
    // StageScrollTest�擾�֐�
    private void GetStageScrollSystem() { stageScrollSystem = EnemyManager_V2.Instance.StageScrollSystem; }

    private int ChildConut { get { return transform.childCount; } }


    // MoveSystem�擾
    EnemyMoveSystem_V2[]            enemyMoveSystem;
    // InvincibleSystem�擾
    EnemyStanSystem_V2[]      enemyInvincibleSystem;
    // FormationBreakSystem�擾
    EnemyFormationBreakSystem_V2[]  enemyFormationBreakSystem;

    void Start()
    {
        // StageScrollTest�擾
        GetStageScrollSystem();
        // MoveSystem�擾
        GetSystem(ref enemyMoveSystem);
        // InvincibleSystem�擾
        GetSystem(ref enemyInvincibleSystem);
        // EnemyInvincibleSystem�擾
        GetSystem(ref enemyFormationBreakSystem);
    }


    void FixedUpdate()
    {
        // �������
        FormationBreakSystem();

        // �ǓƎ�
        DyingAloneSystem();

        // �s�i������
        //MarchCotrolSystem(default);
    }

    /// <summary>
    /// �������
    /// </summary>
    private void FormationBreakSystem()
    {
        //for (int i = 0; i < childConut; i++) Debug.Log(!enemyFormationBreakSystem[i].IsInsideCamera);
        // �G�l�~�[�̐��擾
        var childCount = ChildConut;

        // �J�����O�Ȃ�retuen
        for (int i = 0; i < childCount; i++)
        {
            if (enemyFormationBreakSystem[i] == null) return;
            if (!enemyFormationBreakSystem[i].IsInsideCamera) return;
            // �e�q�֌W����
            for (int j = 0; j < childCount; j++)
            {
                enemyInvincibleSystem[i].Initialized();
                enemyFormationBreakSystem[j].ParentSeparationSystem();
            }
        }
    }

    // �q�����Ȃ��Ȃ��������
    private void DyingAloneSystem()
    {
        var childCount = ChildConut;
        if (childCount != 0) return; 
        Destroy(gameObject);
    }

    // MoveSystem�擾
    private void GetSystem<T>(ref T[] type)
    {
        // �G�l�~�[�̐��擾
        var childCount = ChildConut;
        // �G�l�~�[�̐���MoveSystem�̗v�f���擾
        type = new T[ChildConut];

        // ����MoveSystem�擾
        for (int i = 0; i < ChildConut; i++)
        {
            // �G�l�~�[�擾
            var enemy = transform.GetChild(i);
            // MoveSstem������������continue
            if (enemy.GetComponent<T>() == null) continue;
            // MoveSystem�擾
            type[i] = enemy.GetComponent<T>();
        }
    }

    // MarchSystem����
    private void MarchCotrolSystem(MarchingPoint marchingPoint)
    {
        if (marchingPoint == default) marchingPoint = this.marchingPoint;

        // �X�N���[���̑��x�擾
        var ScrollSpeed = EnemyManager_V2.Instance.StageScrollSystem.ScrollSpeed;

        for (int i = 0; i < enemyMoveSystem.Length; i++)
        {
            // MoveSystem�擾
            var moveSystem = enemyMoveSystem[i];
            // �s�i�����Ƒ��x�v�Z
            var marchVec = marchSpeed * GetMarchDirection();
            var moveVec = marchVec;

            if (marchingPoint == MarchingPoint.side)
            {
                // �X�e�[�W�̑��x�擾
                var scrollSpeed = stageScrollSystem.ScrollSpeed;
                // �X�e�[�W�ɍ��킹�čs�i
                marchVec = marchVec + (Vector3.zero * scrollSpeed);
            }

            //// �s�i������
            //moveSystem.MarchSystem(moveVec);

            // �s�i�����G��
            enemyInvincibleSystem[i].EnabledStan();
        }
    }

    // �i�s��������
    private Vector3 GetMarchDirection()
    {
        switch (marchingPoint)
        {
            case MarchingPoint.front:               return Vector3.back;
            case MarchingPoint.back:                return Vector3.forward;
            case MarchingPoint.side:
                var positionX = transform.position.x;
                var cameraPosX = Camera.main.transform.position.x;
                if (positionX < cameraPosX)         return Vector3.right;
                else if (positionX > cameraPosX)    return Vector3.left;
                else                                return Vector3.zero;
            case MarchingPoint.stand:               return Vector3.zero;
            default:                                return Vector3.zero;
        }
    }

}
