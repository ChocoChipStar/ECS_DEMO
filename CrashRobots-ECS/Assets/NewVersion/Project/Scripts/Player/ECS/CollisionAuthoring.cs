//using System.Collections.Generic;
//using Unity.Entities;
//using UnityEngine;

//public class CollisionAuthoring : MonoBehaviour
//{
    
//}

//public class CollisionBaker : Baker<CollisionAuthoring>
//{
//    public override void Bake(CollisionAuthoring authoring)
//    {
//        var data = new CollisionData()
//        {
//            Enemy = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
//        };
//        AddComponent(GetEntity(TransformUsageFlags.None), data);
//    }
//}

//public struct CollisionData : IComponentData
//{
//    public Entity Enemy;
//}