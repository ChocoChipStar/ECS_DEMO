using UnityEngine;

public class PlayerScriptManager_V2 : Singleton_V2<PlayerScriptManager_V2>
{
    private MoveSystem_V2 moveSystem;
    public MoveSystem_V2 MoveSystem { get { return moveSystem; } }

    private AttackSystem_V2 attackSystem;
    public AttackSystem_V2 AttackSystem { get { return attackSystem; } }

    private AnimationSystem_V2 animationSystem;
    public AnimationSystem_V2 AnimationSystem { get { return animationSystem; } }

    private HitPointSystem_V2 hitPointSystem;
    public HitPointSystem_V2 HitPointSystem { get { return hitPointSystem; } }

    private EnergySystem_V2 energySystem;
    public EnergySystem_V2 EnergySystem { get { return energySystem;} }

    private CameraSystem_V2 cameraSystem;
    public CameraSystem_V2 CameraSystem { get { return cameraSystem;} }

    private HitStopSystem_V2 hitStopSystem;
    public HitStopSystem_V2 HitStopSystem { get { return hitStopSystem; } }

    private EffectSystem_V2 effectSystem;
    public EffectSystem_V2 EffectSystem { get { return effectSystem; } }

    private void Awake()
    {
        moveSystem      = GetComponent<MoveSystem_V2>();
        attackSystem    = GetComponent<AttackSystem_V2>();
        animationSystem = GetComponent<AnimationSystem_V2>();
        hitPointSystem  = GetComponent<HitPointSystem_V2>();
        energySystem    = GetComponent<EnergySystem_V2>();
        cameraSystem    = GetComponent<CameraSystem_V2>();
        hitStopSystem   = GetComponent<HitStopSystem_V2>();
        effectSystem    = GetComponent<EffectSystem_V2>();
    }
}
