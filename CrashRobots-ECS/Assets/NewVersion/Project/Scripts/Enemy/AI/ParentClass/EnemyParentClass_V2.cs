using System.Numerics;

public abstract class EnemyParentClass_V2 : EnemyFanction_V2
{
    // EnemyScriptManager_V2�擾
    private �@EnemyScriptManager_V2 script;
    protected EnemyScriptManager_V2 Script { get { return script; } }

    // EnemyDataManager_V2�擾
    private �@EnemyDataManager_V2 �@data;
    protected EnemyDataManager_V2 �@Data { get { return data; } }
    
    private void Awake()
    {
        AutoGetEnemyCompornent(ref script);
        AutoGetEnemyCompornent(ref data);
    }

    public abstract override void Initialized();

    
    /// <summary>
    /// �V�X�e���pVelocity
    /// </summary>
    public Vector3 Velocity { get; private set; }

    /// <summary>
    /// �V�X�e���pVelocity�Z�b�g�֐�
    /// </summary>

    protected void SetVelocity(Vector3 velocity) { Velocity = velocity; }
}
