using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ResultManager : MonoBehaviour
{
    [Header("AudioSouce")]
    // AudioSouce�擾
    [SerializeField, Tooltip("SE�p��AudioSouce���A�^�b�`���Ă�������")]
    private AudioSource audioSource;

    [SerializeField]
    private ScoreManager scoreManager;

    [Header("Score")]
    // ScoreUI�擾
    [SerializeField, Tooltip("Score���A�^�b�`���Ă�������")]
    private RectTransform score;
    // ScoreUItext�擾
    private TextMeshProUGUI scoretext;
    // �Q�[�����l�������X�R�A
    [SerializeField, Tooltip("�Q�[�����l�������X�R�A����͂��Ă�������")]
    private int myScore;
    // AddScoreSE�擾
    [SerializeField, Tooltip("AddScore SE���A�^�b�`���ĉ�����")]
    private AudioClip addScoreSE;

    [Header("Highilight")]
    // Highilighit�A�j���[�V�����擾   
    [SerializeField, Tooltip("Highilighit���A�^�b�`���Ă�������")]
    private Animator highilightAnim;
    // HigherHighilighitSE�擾
    [SerializeField, Tooltip("HigherHighilighit SE���A�^�b�`���Ă�������")]
    private AudioClip higherHighilitSE;
    // LowerHighilighitSE�擾
    [SerializeField, Tooltip("LowerHighilighit SE���A�^�b�`���Ă�������")]
    private AudioClip lowerHighilitSE;

    [Header("Rank")]
    // RankUI�擾
    [SerializeField, Tooltip("Rank���A�^�b�`���Ă�������")]
    private RectTransform rank;
    // RankUIAnimator�擾
    private Animator rankAnim;
    // RankUIText�擾
    private TextMeshProUGUI ranktext;
    // RankDicideSE�擾
    [SerializeField, Tooltip("RankDicide SE���A�^�b�`���ĉ�����")]
    private AudioClip rankDicideSE;
    // Rank�ݒ�
    [System.Serializable]
    public struct RankAndItsScore
    {
        [SerializeField, Tooltip("�����N��ݒ肵�Ă�������")]
        public string rank;
        [SerializeField, Tooltip("�����N�Ɏ��邽�߂̕ϐ���ݒ肵�Ă�������")]
        public int rankScore;
    }
    [SerializeField, Tooltip("�����N�Ƃ��̃����N�Ɏ��邽�߂̕ϐ���ݒ肵�Ă�������")]
    public List<RankAndItsScore> rankAndItsScore = new List<RankAndItsScore>();

    void Awake()
    {
        // ScoreUItext�擾
        scoretext = score.GetComponent<TextMeshProUGUI>();
        // RankUI�̂��ꂼ��̃R���|�[�l���g�擾   
        rankAnim = rank.GetComponent<Animator>();
        ranktext = rank.GetComponent<TextMeshProUGUI>();
        // AudioSource�擾
        audioSource = GetComponent<AudioSource>();

        myScore = scoreManager.GetScore();
        DicideRank();
    }


    void Update()
    {
        MultiplyScore();
        OnAnimation();
    }

    // �X�R�A�����Z����؂���
    private bool stopMultiplyScore { get; set; }

    // ��ʂɕ\�������鐔��
    private int displayScore = 0;
    // �X�R�A�����Z���Ă���
    private void MultiplyScore()
    {
        displayScore++;
        displayScore = Mathf.Min(displayScore, myScore);
        scoretext.text = displayScore.ToString();

        stopMultiplyScore = displayScore == myScore;

        AddScoreSEManeger();
    }

    // �X�R�A���Z����񂾂�������
    private bool playAddScoreSE = false;
    private bool stopAddScoreSE = false;
    // �X�R�A���Z����SE
    private void AddScoreSEManeger()
    {
        if (stopMultiplyScore)
        {
            if (stopAddScoreSE) return;

            audioSource.clip = null; 
            audioSource.loop = false;
            audioSource.Stop();
            stopAddScoreSE = true;
        }
        else
        {
            if (playAddScoreSE) return;

            audioSource.clip = addScoreSE;
            audioSource.loop = true;
            audioSource.PlayDelayed(0.0f);
            playAddScoreSE = true;
        }
    }
    // �A�j���[�V���������Ԃɍs��
    private void OnAnimation()
    {
        if (stopMultiplyScore)
            BehaviorWhenDisplayingHighlighit();

        if (GetEndOfAnim(highilightAnim, "HighilighitingAnim"))
            BehaviorWhenDisplayingRank();
    }


    // �l���X�R�A���ǂ̃����N�ɕ��ނ���邩
    private void DicideRank()
    {
        for (int i = 0; i < rankAndItsScore.Count; i++)
        {
            bool higherRank = rankAndItsScore[i].rankScore <= myScore;
            if (!higherRank) continue;

            ranktext.text = rankAndItsScore[i].rank;
            return;
        }
    }

    // �n�C���C�g����񂾂�������
    private bool playHighilighit = false;
    // �n�C���C�g��\�����̋���
    private void BehaviorWhenDisplayingHighlighit()
    {
        if (playHighilighit) return;

        highilightAnim.SetTrigger("Highiliting");
        audioSource.clip = null;
        audioSource.PlayOneShot(higherHighilitSE);
        playHighilighit = true;
    }

    // �����N����񂾂�������
    private int playRank = 5;
    // �����N��\�����̋���
    private void BehaviorWhenDisplayingRank()
    {
        if (playRank == 0) return;

        rankAnim.SetTrigger("RankDicision");
        audioSource.clip = null;
        audioSource.PlayOneShot(rankDicideSE);
        playRank--;
    }

    // �A�j���[�V�������I��������ǂ���
    private bool GetEndOfAnim(Animator animator, string name)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(name)) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) return false;

        return true;
    }
}
