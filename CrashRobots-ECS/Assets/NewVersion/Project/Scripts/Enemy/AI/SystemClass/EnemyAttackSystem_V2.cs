using System;
using UnityEngine;

public class EnemyAttackSystem_V2 : EnemyParentClass_V2
{

    // 突進攻撃準備距離
    [SerializeField, Header("突進攻撃準備距離を設定してください")]
    private float prepTackleDistance = 10.0f;
    // 突進攻撃準備時間
    [SerializeField, Header("突進攻撃準備時間を設定してください")]
    private float prepTackleTime = 1.0f;
    // 突進速度
    [SerializeField, Header("突進移動の速度を設定してください")]
    private float movingTackleSpeed = 10.0f;
    // 突進距離
    [SerializeField, Header("突進移動の距離を設定してください")]
    private float tackleDistance = 10.0f;

    public enum TackleState
    {
        /// <summary>
        /// タックル範囲外
        /// </summary>
        notSencing,

        /// <summary>
        /// 止まってる状態
        /// </summary>
        stop,

        /// <summary>
        /// Tackle開始時
        /// </summary>
        startTackle,

        /// <summary>
        /// Tackle中
        /// </summary>
        isTackle,

        /// <summary>
        /// Tackle終了時
        /// </summary>
        endTackle
    }
    private TackleState state = new TackleState();

    public override void Initialized()
    {
        state             = TackleState.notSencing;   // ステート初期化
        isTackleMotion          = false;                    // 突進フラグオフ
        startPositionOfTackle   = Vector3.zero;             // 突進開始ポジション初期化
        tackleDirection         = Vector3.zero;             // 突進方向初期化
        prepTackleTimeTimer     = 0.0f;                     // 突進準備時間をリセット
    }

    private void OnEnable()
    {
        Initialized();
    }

    private void OnDisable()
    {
        EnableSpin(false);
    }

    void Update()
    {
        AttackSystem();
    }



    // ================================================================================================================================
    // 下から関数
    // ================================================================================================================================


    #region

    private void AttackSystem()
    {
        // Playerとの距離取得
        var distanceFromPlayer = Script.CalDistanceFromPlayer(transform.position);

        switch (state)
        {
            case TackleState.notSencing:

                if (IsSencingTackle(distanceFromPlayer))
                    JumpNextState(ref state);

                break;


            case TackleState.stop:

                if (IsPrepTackleTime(distanceFromPlayer))
                    JumpNextState(ref state);

                break;


            case TackleState.startTackle:

                StartTackle();

                EnableSpin(true);

                JumpNextState(ref state);

                break;


            case TackleState.isTackle:

                IsTackleMotionStartDistance();

                // Tackleモーション中はPlayerを見ない
                Script.Enemy.ViewPointSystem.SetEnableLockViwePoint(true);

                // 攻撃中のマテリアルに変更
                Script.Enemy.MaterialManager.EnableAttackMaterial();

                if (!CalIsTackleDistance(startPositionOfTackle, tackleDistance))
                    JumpNextState(ref state);

                break;


            case TackleState.endTackle:

                Script.ViewPointSystemEnabled(true);

                EnableSpin(false);

                JumpNextState(ref state);

                break;


            default:

                break;
        }

    }

    #endregion


    #region notSencing

    /// <summary>
    /// 突進攻撃の範囲内に感知
    /// </summary>
    /// <param name="distanceFromPlayer">プレイヤーとの距離を入力してください</param>
    /// <returns></returns>
    private bool IsSencingTackle(Vector3 distanceFromPlayer)
    {
        // 設定した範囲内に居ればtrue 
        var isPreparationTackle = distanceFromPlayer.magnitude <= prepTackleDistance;

        if (isPreparationTackle) Script.MoveSystemEnabled(false);  // MoveSystemオフ
        else Script.MoveSystemEnabled(true);  // MoveSystemオン

        return isPreparationTackle;
    }

    #endregion



    #region stop

    // 突撃準備時間計測
    private float prepTackleTimeTimer = 0.0f;

    /// <summary>
    /// 突進攻撃準備
    /// </summary>
    /// <returns> true -> 準備時間経過 </returns>
    private bool IsPrepTackleTime(Vector3 distanceFromPlayer)
    {
        // 準備時間が過ぎたらtrue
        var preparationTimeElapsed = TimerCount(prepTackleTime, ref prepTackleTimeTimer);
        // falseの時
        if (!preparationTimeElapsed)
        {
            Data.SetVelocity(Vector3.zero);         // 移動を止める
            Script.MoveSystemEnabled(false);        // MoveSystemをオフ
            tackleDirection = distanceFromPlayer;   // 突進攻撃方向更新
        }
        else
        {
            prepTackleTimeTimer = 0;
        }

        return preparationTimeElapsed;
    }

    #endregion



    #region startTackle

    /// <summary>
    /// タックル開始時
    /// </summary>
    private void StartTackle()
    {
        isTackleMotion = true;                      // 突進フラグを立てる
        prepTackleTimeTimer = 0.0f;                 // 準備時間をリセット
        startPositionOfTackle = transform.position; // 突進開始位置セット
    }


    #endregion



    #region isTackle

    // 突進中フラグ
    private bool isTackleMotion = false;
    public bool IsTackleMotion { get { return isTackleMotion; } }

    // 突進開始距離
    private Vector3 startPositionOfTackle = Vector3.zero;

    // 突進方向
    private Vector3 tackleDirection = Vector3.zero;

    /// <summary>
    /// 突進モーション開始距離計測
    /// </summary>
    private void IsTackleMotionStartDistance(/*bool isTackleMotion*/)
    {
        //// isTackleがfalseならreturn
        //if (!isTackleMotion) return;

        // 突進移動用の速度に切り替え
        var velocity = tackleDirection.normalized * movingTackleSpeed;
        Data.SetVelocity(velocity);
    }

    /// <summary>
    /// 突進モーション開始距離計測
    /// </summary>
    /// <param name="startPosition">突進攻撃開始位置を入力してください</param>
    /// <param name="tackleDistance">突進攻撃開始位置を入力してください</param>
    /// <returns> true = 入力した目標距離に到達 </returns>
    private bool CalIsTackleDistance(Vector3 startPosition, float tackleDistance)
    {
        // 突進モーション開始距離計算
        var calTackleDistance = transform.position - startPosition;
        var isTackleMotion = calTackleDistance.magnitude <= tackleDistance;
        return isTackleMotion;
    }

    /// <summary>
    /// enemyAttackBySspinSystemがある場合Spinオンオフ
    /// </summary>
    private void EnableSpin(bool enable)
    {
        // enemyAttackBySspinSystemがないならreturn
        if (Script.Enemy.SspinSystem == null) return;
        if (enable)
            StartCoroutine(Script.Enemy.SspinSystem.SpinSystem());      // SpinSystem()起動
        else
            Script.Enemy.SspinSystem.BreakeSpinSystem();                // SpinSystem()を抜ける
    }

    #endregion


}
