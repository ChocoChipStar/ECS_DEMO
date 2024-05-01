using UnityEngine;

public class PlayerScriptsManager : GenericSingleton<PlayerScriptsManager>
{
    [SerializeField]
    private SwingHammerSystem assignSwingHammerSystem;
    public SwingHammerSystem swingHammerSystem
    { 
        get { return assignSwingHammerSystem; }
        private set { assignSwingHammerSystem = value;}
    }

    [SerializeField]
    private AnimationSystem assignAnimationSystem;
    public AnimationSystem animationSystem
    {
        get { return assignAnimationSystem; }
        private set { assignAnimationSystem = value;}
    }

    [SerializeField]
    private HitStopSystem assignHitStopSystem;
    public HitStopSystem hitStopSystem
    {
        get { return assignHitStopSystem; }
        private set { assignHitStopSystem = value;}
    }
}
