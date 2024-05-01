using UnityEngine;

public class EnemyFormationBreakSystem_V2 : MonoBehaviour
{
    [SerializeField]
    private EnemyVisibleChecker visibleChecker;

    public bool IsInsideCamera { get { return visibleChecker.IsInsideCamera; } }

    /// <summary>
    /// 隊列解除システム
    /// </summary>
    public void ParentSeparationSystem()
    {
        // 親が居なかったらreturn
        if (transform.parent == null) return;
        // 親子関係解除
        transform.parent = null;
    }
}
