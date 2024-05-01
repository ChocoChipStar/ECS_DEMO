using System.Collections;
using UnityEngine;

public class EnemyStanSystem_V2 : EnemyParentClass_V2
{
    [SerializeField, Tooltip("���G���Ԃ�ݒ肵�Ă�������")]
    private float stanTime = 0.5f;
    // �X�^�����Ԃ̃^�C�}�[
    private float stanTimeTimer = 0;

    // �X�^���t���O
    private bool isStan;

    /// <summary>
    /// �X�^���N��
    /// </summary>
    public void EnabledStan() { isStan = true; }

    public override void Initialized()
    {
        stanTimeTimer = 0;
        isStan = false;
    }

    private void OnEnable()
    {
        Initialized();
    }

    private void OnDisable()
    {
        DisableStanBehavior();
    }

    private void Update()
    {
        // �X�^�������ᖳ�����return
        if (!isStan) return;

        StanBehavior();

        // �X�^�����Ԍv��
        if (!TimerCountAndReset(stanTime, ref stanTimeTimer)) return;

        DisableStanBehavior();

        // �X�^������
        isStan = false;
    }

    /// <summary>
    /// �X�^���L����
    /// </summary>
    public void StanBehavior()
    {
        // �}�e���A�������ւ�
        Script.Enemy.MaterialManager.EnableStanMaterial();
        // �e�V�X�e���I�t
        SetEachSystemAndColliderEnable(false);
        // ���_�Œ�
        Script.Enemy.ViewPointSystem.SetEnableLockViwePoint(true);

    }

    /// <summary>
    /// �X�^������
    /// </summary>
    private void DisableStanBehavior()
    {
        // �e�V�X�e���I��
        SetEachSystemAndColliderEnable(true);
        // ���_�Œ����
        Script.Enemy.ViewPointSystem.SetEnableLockViwePoint(false);
    }

    /// <summary>
    /// �e�֌W�V�X�e���I���I�t
    /// </summary>
    /// <param name="enable"> ture -> system.enabled = true , collider.enabled = true </param>
    private void SetEachSystemAndColliderEnable(bool enable)
    {
        Script.AttackSystemEnabled      (enable);
        Script.MoveSystemEnabled        (enable);
        Script.ViewPointSystemEnabled   (enable);
        Script.SpinSystemEnabled        (enable);
        Script.Enemy.ColliderSystem.EnableCollider(enable);
    }

    /// <summary>
    /// �ړ��֎~
    /// </summary>
    /// <param name="enableLock"></param>
    /// <returns></returns>
    private IEnumerator MoveLockSystem(bool enableLock)
    {
        while (enableLock)
        {
            var velocity = Data.Rigidbody.velocity;
            if (velocity.magnitude != 0) velocity = Vector3.zero;
            Data.Rigidbody.velocity = velocity;


            yield return null;
        }
    }

}
