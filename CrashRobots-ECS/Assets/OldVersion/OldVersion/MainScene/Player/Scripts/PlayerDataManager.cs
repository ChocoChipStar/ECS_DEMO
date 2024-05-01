using UnityEngine;

public class PlayerDataManager : GenericSingleton<PlayerDataManager>
{
    [SerializeField,Header("移動速度の値を設定できます")]
    private float assignMoveSpeed;
    public float moveSpeed
    {
        get { return assignMoveSpeed; } 
        private set { assignMoveSpeed = value; }
    }

    [SerializeField,Header("ヒットストップする時間を設定できます")]
    private float assignHitStopTime;
    public float hitStopTime
    {
        get { return assignHitStopTime; }
        private set { assignHitStopTime = value; }
    }

    [SerializeField]
    private Rigidbody assignRigidBody;
    public Rigidbody rigidBody
    {
        get { return assignRigidBody; }  
        private set {  assignRigidBody = value; } 
    }

    [SerializeField]
    private Transform assignTransform;
    public new Transform transform 
    { 
        get { return assignTransform; }  
        private set { assignTransform = value; } 
    }
}
