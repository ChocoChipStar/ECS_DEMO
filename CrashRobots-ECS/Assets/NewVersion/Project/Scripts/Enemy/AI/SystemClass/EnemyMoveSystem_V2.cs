using UnityEngine;

public class EnemyMoveSystem_V2 : EnemyParentClass_V2
{
    // 移動速度
    [SerializeField, Header("移動速度を設定してください")]
    private float speed = 5.0f;
    private float Speed { get { return speed; } }
    // 感知距離
    [SerializeField, Header("感知距離を設定してください")]
    private float sensingDistance = 20.0f;
    private float SensingDistance { get { return sensingDistance; } }


    /// <summary>
    /// 
    /// </summary>
    public enum MoveState
    {
        /// <summary>
        /// 近寄る範囲外
        /// </summary>
        notApproach,
        /// <summary>
        /// 近寄る
        /// </summary>
        Approach
    }
    private MoveState state = new MoveState();



    public override void Initialized()
    {
        SetStartingPos();
    }

    private void OnDisable()
    {
        Data.SetVelocity(Vector3.zero);
    }

    void Update()
    {
        FreezPositionY();

        var distanceFromPlayer = Script.CalDistanceFromPlayer(transform.position);

        if (!IsSensing(distanceFromPlayer)) return;

        // プレイヤーに向かって直進
        var velocity = distanceFromPlayer.normalized * Speed;

        Data.SetVelocity(velocity);
    }



    // ======================================================================================================
    // ここから関数
    // ======================================================================================================


    #region SetStartingPos関数関連

    // 初期位置取得
    private Vector3 startingPos = Vector3.zero;
    // Startでのみ実行   初期設定false
    private bool executionOneTime = false;
    /// <summary>
    /// 初期位置取得
    /// </summary>
    private void SetStartingPos()
    {
        if (executionOneTime) return;

        startingPos = transform.position;
        executionOneTime = true;
    }

    #endregion


    /// <summary>
    /// 
    /// </summary>
    public void MoveSystem()
    {
        FreezPositionY();

        var distanceFromPlayer = Script.CalDistanceFromPlayer(transform.position);

        switch (state)
        {
            case MoveState.notApproach:
                if (IsSensing(distanceFromPlayer))
                    JumpNextState(ref state);

                break;

            case MoveState.Approach:
                // プレイヤーに向かって直進
                var velocity = distanceFromPlayer.normalized * Speed;
                Data.SetVelocity(velocity);

                if (!IsSensing(distanceFromPlayer))
                    JumpNextState(ref state);

                break;
        }
    }



    /// <summary>
    /// 範囲内感知フラグ
    /// </summary>
    /// <param name="distanceFromPlayer">プレイヤーとの距離を入力してください</param>
    /// <returns>　true -> 範囲内です　</returns>
    public bool IsSensing(Vector3 distanceFromPlayer)
    {
        var sensingPlayer = distanceFromPlayer.magnitude <= SensingDistance;

        return sensingPlayer;
    }

    /// <summary>
    /// position.y 固定
    /// </summary>
    private void FreezPositionY()
    {
        var position = transform.position;
        position.y = startingPos.y;
        transform.position = position;
    }
}
