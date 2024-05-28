//using Unity.Entities;
//using UnityEngine.InputSystem;

//public partial struct TestSystem : ISystem
//{
//    public void OnCreate(ref SystemState state)
//    {
//        state.RequireForUpdate<TestData>();
//    }

//    public void OnUpdate(ref SystemState state)
//    {
//        var test = SystemAPI.GetSingleton<TestData>();
//        state.EntityManager.Instantiate(test.TestEntity);

//    }
//}
