using UnityEngine;

public class EnemyDataManager_V2 : EnemyFanction_V2
{
    // Rigitbody取得
    private Rigidbody rigidbody;
    public  Rigidbody Rigidbody { get { return rigidbody; } }

    // MeshRenderer取得
    private MeshRenderer meshRenderer;
    public  MeshRenderer MeshRenderer { get { return meshRenderer; } }


    private void Awake()
    {
        Initialized();
    }

    public override void Initialized()
    {
        AutoGetEnemyCompornent(ref rigidbody);
        AutoGetEnemyCompornent(ref meshRenderer);
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// Rigidbody.velocityセット関数
    /// </summary>
    /// <param name="velocity">Rigidbody.velocity に入れる数字</param>
    public void SetVelocity(Vector3 velocity) { rigidbody.velocity = velocity; }

    /// <summary>
    /// Material差し替え関数
    /// </summary>
    /// <param name="material"> material 入れてください </param>
    public void SetMaterial(Material material) { meshRenderer.material = material; }


}
