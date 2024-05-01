using ExplosionSample;
using UnityEngine;

public class EnemyManager_V2 : Singleton_V2<EnemyManager_V2>
{
    [SerializeField]
    private EnemyGenerateSystem_V2 enemyGenerateSystem;
    public EnemyGenerateSystem_V2  EnemyGenerateSystem { get { return enemyGenerateSystem; } }
    [SerializeField]
    private Bomb_V2 bomb;
    public Bomb_V2  Bomb { get {  return bomb; } }
    [SerializeField]
    private StageScrollTest stageScrollSystem;
    public StageScrollTest StageScrollSystem { get {  return stageScrollSystem; } }
    [SerializeField]
    private Camera camera;
    public Camera Camera { get { return camera; } }
}
