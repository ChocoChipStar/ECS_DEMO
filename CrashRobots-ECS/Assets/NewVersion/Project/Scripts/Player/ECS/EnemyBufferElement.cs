using Unity.Entities;


[InternalBufferCapacity(5)]
public struct EnemyBufferElement : IBufferElementData
{
    public int EnemyID;
    public Entity Enemy;
    public float CollisionRadius;
}