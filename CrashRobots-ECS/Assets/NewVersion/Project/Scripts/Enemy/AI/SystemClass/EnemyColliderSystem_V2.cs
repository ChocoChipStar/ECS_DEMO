using UnityEngine;

public class EnemyColliderSystem_V2 : EnemyParentClass_V2
{
    public bool isCollision { get; set; } = false;


    [SerializeField] public Collider[] bodyCollider;
    [SerializeField] public Collider[] shieldColloder;

    public override void Initialized()
    {
        
    }

    private void OnEnable()
    {
        Initialized();
    }

    public void EnableCollider(bool enabled)
    {
        for (int i = 0; i < bodyCollider.Length; i++)
        {
            if (bodyCollider[i] == null) return;
            bodyCollider[i].enabled = enabled;
        }

        for (int i = 0; i < shieldColloder.Length; i++)
        {
            if (shieldColloder[i] == null) return;
            shieldColloder[i].enabled = enabled;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }
}
