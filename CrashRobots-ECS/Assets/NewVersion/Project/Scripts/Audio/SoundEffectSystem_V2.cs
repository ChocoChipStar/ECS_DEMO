using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundEffectSystem_V2 : MonoBehaviour
{
    private string currentSceneName;
    public string CurrentSceneName {  get { return currentSceneName; } }

    private void Awake()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }
}
