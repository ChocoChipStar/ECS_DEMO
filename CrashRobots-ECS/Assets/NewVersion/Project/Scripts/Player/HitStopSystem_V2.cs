using System.Collections;
using UnityEngine;

public class HitStopSystem_V2 : MonoBehaviour
{
    [Tooltip("true->�q�b�g�X�g�b�v�����s�� false->����s")]
    private bool isHitStop;
    public bool IsHitStop { get { return isHitStop; } }

    /// <summary>
    /// �q�b�g�X�g�b�v�̏�����s���܂�
    /// </summary>
    /// <param name="duration">�q�b�g�X�g�b�v�̒��������i�b�j</param>
    /// <param name="strength">�q�b�g�X�g�b�v�̋�������</param>
    /// <param name="enemyObj">�q�b�g�����G�̃I�u�W�F�N�g����</param>
    /// <returns></returns>
    public IEnumerator HitStop(float duration, float strength, GameObject enemyObj)
    {
        isHitStop = true;
        Time.timeScale = strength;

        yield return new WaitForSeconds(duration);

        Time.timeScale = 1.0f;
        isHitStop = false;

        if (enemyObj != null)
            DestroyEnemy(enemyObj);
    }

    /// <summary>
    /// �v���C���[���U���œ|�����G�l�~�[��폜���鏈������s���܂�
    /// </summary>
    /// <param name="enemyObj">�v���C���[���|�����G�l�~�[����</param>
    private void DestroyEnemy(GameObject enemyObj)
    {
        if (!PlayerScriptManager_V2.Instance.AttackSystem.IsAttack)
            return;

        var enemyObject = enemyObj.transform.root;
        var enemyDamageSystem = enemyObject.GetComponent<EnemyHitPointSystem_V2>();
        enemyDamageSystem.DamegeSystem();
    }
}
