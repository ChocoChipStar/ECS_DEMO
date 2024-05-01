using UnityEngine;

public class ReflectScrapsInScore : MonoBehaviour
{
    // ScoreManagerŽæ“¾
    ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = ScoreManager.instance;
    }

    private void OnDisable()
    {
        Debug.Log("disable");
        scoreManager.AddScore();
    }
}
