using UnityEngine;

public class EnemyFormationBreakSystem_V2 : MonoBehaviour
{
    [SerializeField]
    private EnemyVisibleChecker visibleChecker;

    public bool IsInsideCamera { get { return visibleChecker.IsInsideCamera; } }

    /// <summary>
    /// ��������V�X�e��
    /// </summary>
    public void ParentSeparationSystem()
    {
        // �e�����Ȃ�������return
        if (transform.parent == null) return;
        // �e�q�֌W����
        transform.parent = null;
    }
}
