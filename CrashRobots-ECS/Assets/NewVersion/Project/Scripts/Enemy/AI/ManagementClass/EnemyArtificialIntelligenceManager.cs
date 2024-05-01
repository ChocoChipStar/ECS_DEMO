using UnityEngine;

public class EnemyArtificialIntelligenceManager : MonoBehaviour
{
    /// <summary>
    /// AI用のState
    /// </summary>
    public enum ArtIntelState
    {
        /// <summary>
        /// 移動State
        /// </summary>
        Move,
        /// <summary>
        /// 攻撃State
        /// </summary>
        Attack,
        /// <summary>
        /// 無敵State
        /// </summary>
        Invicible

    }
    private ArtIntelState state = new ArtIntelState();

    void Start()
    {
        
    }

    void Update()
    {
        switch (state)
        {
            case ArtIntelState.Move:
                
                break;

            case ArtIntelState.Attack:
                
                break;

            case ArtIntelState.Invicible:
                
                break;

            default:

                break;
        }
    }


    // ================================================================================
    // ここから関数
    // ================================================================================

    /// <summary>
    /// ArtIntelState変数を変更する関数
    /// </summary>
    /// <param name="state"> 移行したいStateを入力してください </param>
    private void ChangeState(ArtIntelState state)
    {
        this.state = state;
    }

}
