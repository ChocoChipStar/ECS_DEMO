using Unity.Entities;

public partial struct TitleConfigDataSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.Enabled = true;
        state.RequireForUpdate<TitleConfigData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach(var configData in SystemAPI.Query<RefRW<TitleConfigData>>())
        {
            configData.ValueRW.spawnCount = ConfigUIController.SpawnCount;
            configData.ValueRW.isUseEffects = ConfigUIController.IsUseEffects;
            configData.ValueRW.isDisplayEntitiesCount = ConfigUIController.IsDisplayEntitiesCount;
            configData.ValueRW.isSpawnLoop = ConfigUIController.IsSpawnLoop;
        }

        state.Enabled = false;
    }
}
