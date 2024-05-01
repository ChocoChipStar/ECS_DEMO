using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // シングルトン化
    public static ScoreManager instance;

    [SerializeField, Tooltip("ScoreUIをアタッチしてください")]
    private TextMeshProUGUI scoreUI;

    [SerializeField, Tooltip("何ポイント入れるか決めてください")]
    private int addPoint;

    // 現在スコア
    private int score = 0;
    public void AddScore()
    {
        score += addPoint;
    }

    public int GetScore()
    {
        return score;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MultiplyScore();
    }

    // 画面に表示させる数字
    private int displayScore = 0;
    // スコアを加算していく
    private void MultiplyScore()
    {
        displayScore++;
        displayScore = Mathf.Min(displayScore, score);
        scoreUI.text = "SCORE" + displayScore.ToString();
    }
}
