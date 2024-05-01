using TMPro;
using UnityEngine;

public class ResultScoreSystem_V2 : MonoBehaviour
{
    // ResultManager_V2�擾
    private ResultManager_V2 resultManager;
    [SerializeField,Header("�X�R�A���A�^�b�`���ĉ�����")]    
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

    // �X�R�A�����Z����؂���
    public bool stopMultiplyScore { get; set; }

    // ��ʂɕ\�������鐔��
    private int displayScore = 0;
    // �X�R�A�����Z���Ă���
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
