using UnityEngine;

public class PlayerCollisionManager_V2 : MonoBehaviour
{
    [SerializeField]
    private string groundTagName;

    [SerializeField]
    private string enemyTagName;

    [SerializeField]
    private string unbreakableTagName;

    [Tooltip("�f�[�^�}�l�[�W���[�̃C���X�^���X")]
    private PlayerDataManager_V2 playerData;

    [Tooltip("�X�N���v�g�}�l�[�W���[�̃C���X�^���X")]
    private PlayerScriptManager_V2 playerScript;

    private bool isPerformance = false;

    private void Awake()
    {
        playerData = PlayerDataManager_V2.Instance;
        playerScript = PlayerScriptManager_V2.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collisionObj = collision.gameObject;

        if (!collisionObj.CompareTag(enemyTagName))
            return;

        if (!playerScript.AttackSystem.IsAttack)
        {
            playerScript.HitPointSystem.TakenDamage();
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var tagIsEnemy = other.transform.root.CompareTag(enemyTagName);
        if (tagIsEnemy)
        AttackingTrigger(other.gameObject);
    }

    private void AttackingTrigger(GameObject triggerObj)
    {
        if (triggerObj.gameObject.CompareTag(unbreakableTagName))
            playerScript.MoveSystem.ReflectSystem(triggerObj);

        if (!playerScript.AttackSystem.IsAttack)
            isPerformance = false;

        if (!triggerObj.transform.root.CompareTag(enemyTagName))
            return;

        if (playerScript.AttackSystem.IsAttack)
        {
            var enemyColSystem = triggerObj.transform.root.GetComponent<EnemyColliderSystem_V2>();

            var isCollision = enemyColSystem.isCollision;
            if (playerScript.AttackSystem.IsAttack)
            {
                if (isCollision)
                    return;

                StartCoroutine(playerScript.HitStopSystem.HitStop(
                    playerData.AttackHitStopDuration, playerData.AttackHitStopStrength, triggerObj.gameObject)
                    );

                StartCoroutine(playerScript.HitStopSystem.HitStop(
                    playerData.AttackHitStopDuration, playerData.AttackHitStopStrength, triggerObj.gameObject)
                    );

                playerScript.CameraSystem.StartCoroutine(
                    playerScript.CameraSystem.ShakeSystem(playerData.AttackShakeDuration, playerData.AttackShakeStrength)
                    );

                isCollision = true;
            }
            else isCollision = false;

            enemyColSystem.isCollision = isCollision;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var enemyColSystem = other.transform.root.GetComponent<EnemyColliderSystem_V2>();

        if (other.transform.root.CompareTag(enemyTagName) && !enemyColSystem.isCollision) enemyColSystem.isCollision = false;
    }
}
