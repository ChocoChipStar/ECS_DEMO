using UnityEngine;

public class ShieldBlownAwaySystem_V2 : EnemyParentClass_V2
{
    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private Collider collider;

    public override void Initialized()
    {
        
    }

    /// <summary>
    /// �N����������΂�
    /// </summary>
    /// <param name="impulsePower">            �Ԃ���΂�����                  </param>
    /// <param name="impulseAfterDestroyTime"> �Ԃ����ł������܂ł̎���  </param>
    public void BrownAwayMyselfSystem(Vector3 impulsePower,float impulseAfterDestroyTime)
    {
        rigidbody.AddForce(impulsePower, ForceMode.Impulse);    // ������΂�
        Destroy(gameObject, impulseAfterDestroyTime);           // ���g������
        collider.enabled = false;                               // �����蔻�薳����
        transform.parent = null;                                // �G�l�~�[�Ƃ̐e�q�֌W����
    }
}
