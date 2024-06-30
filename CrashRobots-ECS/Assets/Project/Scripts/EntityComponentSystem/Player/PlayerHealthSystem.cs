using Unity.Burst;
using Unity.Entities;
using UnityEngine.SceneManagement;

public partial struct PlayerHealthSystem : ISystem
{
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerParamsData>();
    }

    private void OnUpdate(ref SystemState state)
    {
        var playerData = SystemAPI.GetSingleton<PlayerParamsData>();
        if (playerData.health <= 0)
        {
            playerData.health = 0;
            playerData.SetHealthText();
            SceneManager.LoadScene("GameOverScene");
        }

        playerData.SetHealthText();
    }
}
