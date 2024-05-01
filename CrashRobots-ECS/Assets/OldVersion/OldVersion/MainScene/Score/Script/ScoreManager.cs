using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // �V���O���g����
    public static ScoreManager instance;

    [SerializeField, Tooltip("ScoreUI���A�^�b�`���Ă�������")]
    private TextMeshProUGUI scoreUI;

    [SerializeField, Tooltip("���|�C���g����邩���߂Ă�������")]
    private int addPoint;

    // ���݃X�R�A
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

    // ��ʂɕ\�������鐔��
    private int displayScore = 0;
    // �X�R�A�����Z���Ă���
    private void MultiplyScore()
    {
        displayScore++;
        displayScore = Mathf.Min(displayScore, score);
        scoreUI.text = "SCORE" + displayScore.ToString();
    }
}
