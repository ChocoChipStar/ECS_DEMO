using UnityEngine;

public class PlayerDataManager : GenericSingleton<PlayerDataManager>
{
    [SerializeField,Header("�ړ����x�̒l��ݒ�ł��܂�")]
    private float assignMoveSpeed;
    public float moveSpeed
    {
        get { return assignMoveSpeed; } 
        private set { assignMoveSpeed = value; }
    }

    [SerializeField,Header("�q�b�g�X�g�b�v���鎞�Ԃ�ݒ�ł��܂�")]
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
