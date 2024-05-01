using UnityEngine;

public class EnemyScriptManager_V2 : EnemyFanction_V2
{
    private PlayerDataManager_V2 player;
    // playerPosition�擾
    public Vector3 GetPlayerPosition() { return player.transform.position; }
    /// <summary>
    /// プレイヤーとのベクトルの長さを計算します
    /// 基準はplayerです
    /// </summary>
    /// <param name="position"> Vector3型の距離を計算したい対象を選んでください </param>
    /// <returns></returns>
    public Vector3 CalDistanceFromPlayer(Vector3 position) { return player.transform.position - position; }



    // EnemyScript取得
    [System.Serializable]
    public struct EnemyScript
    {
        // EnemyMoveSystem_V2�擾
        [SerializeField]
        public EnemyMoveSystem_V2       moveSystem;
        public EnemyMoveSystem_V2       MoveSystem          { get { return moveSystem; } }

        // EnemyAttackSystem_V2�擾
        [SerializeField]
        public EnemyAttackSystem_V2     attackSystem;
        public EnemyAttackSystem_V2     AttackSystem        { get { return attackSystem; } }

        // EnemyDestroySystem_V2�擾
        [SerializeField]
        public EnemyDestroySystem_V2    destroySystem;
        public EnemyDestroySystem_V2    DestroySystem       { get{ return destroySystem; } }

        // EnemyHPSystem_V2�擾
        [SerializeField]
        public EnemyHitPointSystem_V2   hitPointSystem;
        public EnemyHitPointSystem_V2   HitPointSystem      { get { return hitPointSystem; } }

        // EnemyMaterialmanager�擾
        [SerializeField]
        public EnemyMaterialManager_V2  materialManager;
        public EnemyMaterialManager_V2  MaterialManager     { get { return materialManager; } }

        // EnemyRotationSystem_V2取得
        [SerializeField]
        public EnemyViewPointSystem_V2  viewPointSystem;
        public EnemyViewPointSystem_V2  ViewPointSystem     { get { return viewPointSystem; } }

        // EnemyAnimationSystem_V2取得
        [SerializeField]
        public EnemyAnimationSystem_V2  animationSystem;
        public EnemyAnimationSystem_V2  AnimationSystem     { get { return animationSystem; } }

        // EnemyAttackBySspinSystem取得
        [SerializeField]
        public EnemySpinSystem_V2       spinSystem;
        public EnemySpinSystem_V2       SspinSystem         { get { return spinSystem; } }

        // EnemyInvincibleSystem_V2取得
        [SerializeField]
        public EnemyStanSystem_V2       stanSystem;
        public EnemyStanSystem_V2       StanSystem          { get { return stanSystem; } }

        // Collider取得
        [SerializeField]
        public EnemyColliderSystem_V2   colliderSystem;
        public EnemyColliderSystem_V2   ColliderSystem      { get { return colliderSystem; } }

        // EnemyShieldSystem_V2取得
        [SerializeField]
        public EnemyShieldSystem_V2     shieldSystem;
        public EnemyShieldSystem_V2     ShieldSystem        { get { return shieldSystem; } }

        // ShieldBlownAwaySystem_V2
        [SerializeField]
        public ShieldBlownAwaySystem_V2 shieldBlownAway;
        public ShieldBlownAwaySystem_V2 ShieldBlownAway     { get { return shieldBlownAway; } }
    }
    // EnemyScript取得
    [SerializeField]
    private EnemyScript enemy = new EnemyScript();
    public EnemyScript Enemy { get { return enemy; } }


    private void Awake()
    {
        player = PlayerDataManager_V2.Instance;
    }

    private void Start()
    {
        InitializedEachEnemySystem();
    }

    /// <summary>
    /// スクリプト一斉初期化
    /// </summary>
    private void InitializedEachEnemySystem()
    {
        InitializedEnemySystem(ref enemy.moveSystem);
        InitializedEnemySystem(ref enemy.attackSystem);
        InitializedEnemySystem(ref enemy.destroySystem);
        InitializedEnemySystem(ref enemy.hitPointSystem);
        InitializedEnemySystem(ref enemy.materialManager);
        InitializedEnemySystem(ref enemy.viewPointSystem);
        InitializedEnemySystem(ref enemy.animationSystem);
        InitializedEnemySystem(ref enemy.spinSystem);
        InitializedEnemySystem(ref enemy.stanSystem);
        InitializedEnemySystem(ref enemy.colliderSystem);
        InitializedEnemySystem(ref enemy.shieldSystem);
    }

    private void InitializedEnemySystem<T>(ref T enemySystem) where T : EnemyParentClass_V2
    {
        if (enemySystem == null) return;
        enemySystem.Initialized();
    }

    /// <summary>
    /// AIのオンオフが可能です
    /// </summary>
    /// <param name="enabled">各システムの enabled を操作出来ます</param>
    public void AIEnabled(bool enabled)
    {
        MoveSystemEnabled       (enabled);
        AttackSystemEnabled     (enabled);
        DestroySystemEnabled    (enabled);
        HitPointSystemEnabled   (enabled);
        MaterialManagerEnabled  (enabled);
        ViewPointSystemEnabled  (enabled);
        AnimationSystemEnabled  (enabled);
        SpinSystemEnabled       (enabled);
        StanSystemEnabled       (enabled);
        ColliderSystemEnabled   (enabled);
        ShieldSystemEnabled     (enabled);
    }



    #region 各SystemEnabled

    /// <summary>
    /// moveSytem オンオフ
    /// </summary>
    /// <param name="enabled"> moveSystem.enabled = enabled </param>
    public void MoveSystemEnabled       (bool enabled) { EnemySytemEnabled(ref enemy.moveSystem,        enabled); }

    /// <summary>
    /// attackSystem オンオフ
    /// </summary>
    /// <param name="enabled"> attackSystem.enabled = enabled </param>
    public void AttackSystemEnabled     (bool enabled) { EnemySytemEnabled(ref enemy.attackSystem,      enabled); }

    /// <summary>
    /// destroySystem オンオフ
    /// </summary>
    /// <param name="enabled"> destroySystem.enabled = enabled </param>
    public void DestroySystemEnabled    (bool enabled) { EnemySytemEnabled(ref enemy.destroySystem,     enabled); }

    /// <summary>
    /// hitPointSystem オンオフ
    /// </summary>
    /// <param name="enabled"> hitPointSystem.enabled = enabled </param>
    public void HitPointSystemEnabled   (bool enabled) { EnemySytemEnabled(ref enemy.hitPointSystem,    enabled); }

    /// <summary>
    /// materialManager オンオフ
    /// </summary>
    /// <param name="enabled"> materialManager.enabled = enabled </param>
    public void MaterialManagerEnabled  (bool enabled) { EnemySytemEnabled(ref enemy.materialManager,   enabled); }

    /// <summary>
    /// viewPointSystem オンオフ
    /// </summary>
    /// <param name="enabled"> materialManager.enabled = enabled </param>
    public void ViewPointSystemEnabled  (bool enabled) { EnemySytemEnabled(ref enemy.viewPointSystem,   enabled); }

    /// <summary>
    /// animationSystem オンオフ
    /// </summary>
    /// <param name="enabled"> animationSystem.enabled = enabled </param>
    public void AnimationSystemEnabled  (bool enabled) { EnemySytemEnabled(ref enemy.animationSystem,   enabled); }

    /// <summary>
    /// spinSystem オンオフ
    /// </summary>
    /// <param name="enabled"> spinSystem.enabled = enabled </param>
    public void SpinSystemEnabled       (bool enabled) { EnemySytemEnabled(ref enemy.spinSystem,        enabled); }

    /// <summary>
    /// invincibleSystem オンオフ
    /// </summary>
    /// <param name="enabled"> invincibleSystem.enabled = enabled </param>
    public void StanSystemEnabled       (bool enabled) { EnemySytemEnabled(ref enemy.stanSystem,        enabled); }

    /// <summary>
    /// colliderSystem オンオフ
    /// </summary>
    /// <param name="enabled"> colliderSystem.enabled = enabled </param>
    public void ColliderSystemEnabled   (bool enabled) { EnemySytemEnabled(ref enemy.colliderSystem,    enabled); }

    /// <summary>
    /// shieldSystem オンオフ
    /// </summary>
    /// <param name="enabled"> shieldSystem.enabled = enabled </param>
    public void ShieldSystemEnabled     (bool enabled) { EnemySytemEnabled(ref enemy.shieldSystem,      enabled); }

    #endregion

}
