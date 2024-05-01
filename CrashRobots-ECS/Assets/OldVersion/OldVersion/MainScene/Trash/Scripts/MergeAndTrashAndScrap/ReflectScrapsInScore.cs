using UnityEngine;

public class ReflectScrapsInScore : MonoBehaviour
{
    // ScoreManager�擾
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
