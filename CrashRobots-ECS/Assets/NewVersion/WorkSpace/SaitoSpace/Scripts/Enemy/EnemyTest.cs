using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class EnemyTest : EnemyParentClass_V2
{
    public override void Initialized()
    {
        GetES();
    }

    private void Start()
    {
        Initialized();
    }

    public void Update()
    {
        OnOffSytem(ref es.attackSystem);
    }

    /// <summary>
    /// Yボタンでスクリプトオンオフ
    /// </summary>
    /// <typeparam name="T">クラス名</typeparam>
    /// <param name="system">T型のクラス変数</param>
    private void OnOffSytem<T>(ref T system) where T : MonoBehaviour
    {
        var current = Keyboard.current;
        if (current.yKey.isPressed) system.enabled = false;
        else system.enabled = true;
    }

    public struct ES
    {
        public EnemyMoveSystem_V2       moveSystem;
        public EnemyAttackSystem_V2     attackSystem;
        public EnemyDestroySystem_V2    destroySystem;
        public EnemyHitPointSystem_V2   hitPointSystem;
        public EnemyMaterialManager_V2  materialManager;
        public EnemyViewPointSystem_V2  viewPointSystem;
        public EnemyAnimationSystem_V2  animationSystem;
        public EnemySpinSystem_V2       spinSystem;
        public EnemyStanSystem_V2 invincibleSystem;
        public EnemyColliderSystem_V2   colliderSystem;
    }
    private ES es = new ES();

    private void GetES()
    {
        AutoGetEnemyCompornent(ref es.moveSystem);
        AutoGetEnemyCompornent(ref es.attackSystem);
        AutoGetEnemyCompornent(ref es.destroySystem);
        AutoGetEnemyCompornent(ref es.hitPointSystem);
        AutoGetEnemyCompornent(ref es.materialManager);
        AutoGetEnemyCompornent(ref es.viewPointSystem);
        AutoGetEnemyCompornent(ref es.animationSystem);
        AutoGetEnemyCompornent(ref es.spinSystem);
        AutoGetEnemyCompornent(ref es.invincibleSystem);
        AutoGetEnemyCompornent(ref es.colliderSystem);
    }
}
