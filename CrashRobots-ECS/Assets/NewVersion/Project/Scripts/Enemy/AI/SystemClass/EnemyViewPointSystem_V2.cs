using UnityEngine;

public class EnemyViewPointSystem_V2 : EnemyParentClass_V2
{
    Quaternion Rotation { get { return Data.Rigidbody.rotation; } set { Data.Rigidbody.rotation = value; } }

    public override void Initialized()
    {
        enableLockViewPoint = false;
    }


    private void OnEnable()
    {
        Initialized();
    }

    private void Update()
    {
        LockViewPoint(enableLockViewPoint);
    }
        
    /// <summary>
    /// LockRotation�̃I���I�t
    /// </summary>
    private bool enableLockViewPoint = false;

    /// <summary>
    /// enableLockViewPoint�̒l��ݒ肷��֐�
    /// </summary>
    /// <param name="enabled"> enableLockViewPoint = enabled�@�Ƃ��܂� </param>
    public void SetEnableLockViwePoint(bool enabled) { enableLockViewPoint = enabled; }

    // ���b�N���̉�]
    private Quaternion lockRotation = Quaternion.identity;

    /// <summary>
    /// �����������Œ艻���Ă�����
    /// </summary>
    /// <param name="enabled"></param>
    private void LockViewPoint(bool enabled)
    {
        if (enabled)
        {
            LockRotation(true);
            LookPlayerSystem(false);
        }
        else
        {
            LockRotation(false);
            LookPlayerSystem(true);
        }
    }

    /// <summary>
    /// �v���C���[�̕����������֐�
    /// </summary>
    /// <param name="lookPlayerSystemEnabled"> true -> �v���C���[�������� </param>
    private void LookPlayerSystem(bool lookPlayerSystemEnabled)
    {
        // �t���O�������ĂȂ����return
        if (!lookPlayerSystemEnabled) return;

        // Player�Ƃ̋����v�Z
        var distanceFromPlayer = Script.CalDistanceFromPlayer(transform.position);

        // Player�����m���ĂȂ�������return
        if (!Script.Enemy.MoveSystem.IsSensing(distanceFromPlayer)) return;

        // Player�̕���������
        var eulerAngle = Rotation.eulerAngles;

        eulerAngle.x   = 0.0f; 
        eulerAngle.z   = 0.0f;
        eulerAngle.y   = Vec3ToAngle(distanceFromPlayer);

        Rotation       = Quaternion.Euler(eulerAngle);
    }

    /// <summary>
    /// ��]���~�߂�֐�
    /// </summary>
    /// <param name="enabled"> true -> ��]��~ </param>
    private void LockRotation(bool enabled)
    {
        if (enabled)    Rotation = lockRotation;
        else            lockRotation = Rotation;
    }
}
