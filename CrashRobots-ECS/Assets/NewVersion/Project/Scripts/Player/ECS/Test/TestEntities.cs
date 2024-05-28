using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestEntities : MonoBehaviour
{
    private EntityManager entityManager;
    private Entity entity;

    void Start()
    {
        // エンティティの生成
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld));
        entity = entityManager.CreateEntity(archetype);

#if UNITY_EDITOR
        entityManager.SetName(entity,"TEST ENITITY");
#endif
    }

    private void Update()
    {
        if(Keyboard.current.yKey.wasPressedThisFrame)
        {
            entityManager.DestroyEntity(entity);
        }
    }
}