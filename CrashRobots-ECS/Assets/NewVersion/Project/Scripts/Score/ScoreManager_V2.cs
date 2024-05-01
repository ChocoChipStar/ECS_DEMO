using TMPro;
using UnityEngine;

public class ScoreManager_V2 : Singleton_V2<ScoreManager_V2>
{
    [SerializeField, Header("ScoreUI���A�^�b�`���Ă�������")]
    public TextMeshProUGUI scoreUI;
    [SerializeField,Header("Canvas���A�^�b�`���Ă�������")]
    public RectTransform canvas;

    // ���݃X�R�A
    private static int score = 0;
    /// <summary>
    /// �X�R�A���Z
    /// </summary>
    public void AddScore(int addPoint) { score += addPoint; }
    /// <summary>
    /// ���݃X�R�A�擾
    /// </summary>
    /// <returns><���݃X�R�A/returns>
    public static int GetScore(){ return score; }

    private void Start()
    {
        score = 0;
    }

    void Update()
    {
        MultiplyScore();
    }

    // ��ʂɕ\�������鐔��
    private int displayScore = 0;
    // �X�R�A�����Z���Ă���
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
