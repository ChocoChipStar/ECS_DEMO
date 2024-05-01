using TMPro;
using UnityEngine;

public class ResultScoreSystem_V2 : MonoBehaviour
{
    // ResultManager_V2取得
    private ResultManager_V2 resultManager;
    [SerializeField,Header("スコアをアタッチして下さい")]    
    private TextMeshProUGUI scoretext;

    private const float THREE_FRAME_VALUE = 150.0f;


    void Start()
    {
        resultManager = ResultManager_V2.Instance;
    }


    void FixedUpdate()
    {
        MultiplyScore();
    }

    // スコアが加算され切った
    public bool stopMultiplyScore { get; set; }

    // 画面に表示させる数字
    private int displayScore = 0;
    // スコアを加算していく
    private void MultiplyScore()
    {
        var score = ScoreManager_V2.GetScore();

        displayScore = displayScore + (score / (int)THREE_FRAME_VALUE);
        displayScore = Mathf.Min(displayScore, score);
        scoretext.text = displayScore.ToString();

        stopMultiplyScore = displayScore == score;
        resultManager.soundEffectManager.AddScoreSEManeger(stopMultiplyScore);
    }
}
