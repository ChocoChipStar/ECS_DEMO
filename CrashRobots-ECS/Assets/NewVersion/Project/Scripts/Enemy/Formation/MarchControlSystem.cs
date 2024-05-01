using UnityEngine;

public class MarchControlSystem : MonoBehaviour
{
    [SerializeField, Header("進軍速度を設定してください")]
    private float marchSpeed;

    [SerializeField, Header("進軍開始点を設定してください")]
    private MarchingPoint marchingPoint;
    // 開始地点選択
    public enum MarchingPoint { front, back, side ,stand }

    // StageScrollTest取得
    private StageScrollTest stageScrollSystem;
    // StageScrollTest取得関数
    private void GetStageScrollSystem() { stageScrollSystem = EnemyManager_V2.Instance.StageScrollSystem; }

    private int ChildConut { get { return transform.childCount; } }


    // MoveSystem取得
    EnemyMoveSystem_V2[]            enemyMoveSystem;
    // InvincibleSystem取得
    EnemyStanSystem_V2[]      enemyInvincibleSystem;
    // FormationBreakSystem取得
    EnemyFormationBreakSystem_V2[]  enemyFormationBreakSystem;

    void Start()
    {
        // StageScrollTest取得
        GetStageScrollSystem();
        // MoveSystem取得
        GetSystem(ref enemyMoveSystem);
        // InvincibleSystem取得
        GetSystem(ref enemyInvincibleSystem);
        // EnemyInvincibleSystem取得
        GetSystem(ref enemyFormationBreakSystem);
    }


    void FixedUpdate()
    {
        // 隊列解除
        FormationBreakSystem();

        // 孤独死
        DyingAloneSystem();

        // 行進させる
        //MarchCotrolSystem(default);
    }

    /// <summary>
    /// 隊列解除
    /// </summary>
    private void FormationBreakSystem()
    {
        //for (int i = 0; i < childConut; i++) Debug.Log(!enemyFormationBreakSystem[i].IsInsideCamera);
        // エネミーの数取得
        var childCount = ChildConut;

        // カメラ外ならretuen
        for (int i = 0; i < childCount; i++)
        {
            if (enemyFormationBreakSystem[i] == null) return;
            if (!enemyFormationBreakSystem[i].IsInsideCamera) return;
            // 親子関係解除
            for (int j = 0; j < childCount; j++)
            {
                enemyInvincibleSystem[i].Initialized();
                enemyFormationBreakSystem[j].ParentSeparationSystem();
            }
        }
    }

    // 子がいなくなったら消滅
    private void DyingAloneSystem()
    {
        var childCount = ChildConut;
        if (childCount != 0) return; 
        Destroy(gameObject);
    }

    // MoveSystem取得
    private void GetSystem<T>(ref T[] type)
    {
        // エネミーの数取得
        var childCount = ChildConut;
        // エネミーの数分MoveSystemの要素数取得
        type = new T[ChildConut];

        // 数分MoveSystem取得
        for (int i = 0; i < ChildConut; i++)
        {
            // エネミー取得
            var enemy = transform.GetChild(i);
            // MoveSstemが無かったらcontinue
            if (enemy.GetComponent<T>() == null) continue;
            // MoveSystem取得
            type[i] = enemy.GetComponent<T>();
        }
    }

    // MarchSystem制御
    private void MarchCotrolSystem(MarchingPoint marchingPoint)
    {
        if (marchingPoint == default) marchingPoint = this.marchingPoint;

        // スクロールの速度取得
        var ScrollSpeed = EnemyManager_V2.Instance.StageScrollSystem.ScrollSpeed;

        for (int i = 0; i < enemyMoveSystem.Length; i++)
        {
            // MoveSystem取得
            var moveSystem = enemyMoveSystem[i];
            // 行進方向と速度計算
            var marchVec = marchSpeed * GetMarchDirection();
            var moveVec = marchVec;

            if (marchingPoint == MarchingPoint.side)
            {
                // ステージの速度取得
                var scrollSpeed = stageScrollSystem.ScrollSpeed;
                // ステージに合わせて行進
                marchVec = marchVec + (Vector3.zero * scrollSpeed);
            }

            //// 行進させる
            //moveSystem.MarchSystem(moveVec);

            // 行進中無敵化
            enemyInvincibleSystem[i].EnabledStan();
        }
    }

    // 進行方向判定
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
