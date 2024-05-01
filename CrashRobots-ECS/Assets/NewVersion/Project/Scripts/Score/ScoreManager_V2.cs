using TMPro;
using UnityEngine;

public class ScoreManager_V2 : Singleton_V2<ScoreManager_V2>
{
    [SerializeField, Header("ScoreUIをアタッチしてください")]
    public TextMeshProUGUI scoreUI;
    [SerializeField,Header("Canvasをアタッチしてください")]
    public RectTransform canvas;

    // 現在スコア
    private static int score = 0;
    /// <summary>
    /// スコア加算
    /// </summary>
    public void AddScore(int addPoint) { score += addPoint; }
    /// <summary>
    /// 現在スコア取得
    /// </summary>
    /// <returns><現在スコア/returns>
    public static int GetScore(){ return score; }

    private void Start()
    {
        score = 0;
    }

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

    private int enemyKillCount = 0;
    public int GetEnemyKillCount() {  return enemyKillCount; }
    public void AddKillCount() { enemyKillCount++; }
    public void InitializeKillCount() { enemyKillCount = 0; }
}
