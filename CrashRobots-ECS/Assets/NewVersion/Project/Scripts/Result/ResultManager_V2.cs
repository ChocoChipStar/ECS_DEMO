using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager_V2 : GenericSingleton<ResultManager_V2>
{
    public ResultSceneSoundEffectManager_V2 soundEffectManager { get; set; }
    public ResultAnimationManager_V2 animationManager { get; set; }
    public ResultRankSystem_V2 rankSystem { get; set; }
    public ResultScoreSystem_V2 scoreSystem { get; set; }

    private void Awake()
    {
        soundEffectManager = ResultSceneSoundEffectManager_V2.Instance;
        animationManager = GetT<ResultAnimationManager_V2>();
        rankSystem = GetT<ResultRankSystem_V2>();
        scoreSystem = GetT<ResultScoreSystem_V2>();
    }

    private T GetT<T>()
    {
        if (GetComponent<T>() == null) return default;
        return GetComponent<T>();
    }

    public void OnClickTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickRetryButton()
    {
        SceneManager.LoadScene("MainScene");
    }
}
