using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ResultManager : MonoBehaviour
{
    [Header("AudioSouce")]
    // AudioSouce取得
    [SerializeField, Tooltip("SE用のAudioSouceをアタッチしてください")]
    private AudioSource audioSource;

    [SerializeField]
    private ScoreManager scoreManager;

    [Header("Score")]
    // ScoreUI取得
    [SerializeField, Tooltip("Scoreをアタッチしてください")]
    private RectTransform score;
    // ScoreUItext取得
    private TextMeshProUGUI scoretext;
    // ゲーム中獲得したスコア
    [SerializeField, Tooltip("ゲーム中獲得したスコアを入力してください")]
    private int myScore;
    // AddScoreSE取得
    [SerializeField, Tooltip("AddScore SEをアタッチして下さい")]
    private AudioClip addScoreSE;

    [Header("Highilight")]
    // Highilighitアニメーション取得   
    [SerializeField, Tooltip("Highilighitをアタッチしてください")]
    private Animator highilightAnim;
    // HigherHighilighitSE取得
    [SerializeField, Tooltip("HigherHighilighit SEをアタッチしてください")]
    private AudioClip higherHighilitSE;
    // LowerHighilighitSE取得
    [SerializeField, Tooltip("LowerHighilighit SEをアタッチしてください")]
    private AudioClip lowerHighilitSE;

    [Header("Rank")]
    // RankUI取得
    [SerializeField, Tooltip("Rankをアタッチしてください")]
    private RectTransform rank;
    // RankUIAnimator取得
    private Animator rankAnim;
    // RankUIText取得
    private TextMeshProUGUI ranktext;
    // RankDicideSE取得
    [SerializeField, Tooltip("RankDicide SEをアタッチして下さい")]
    private AudioClip rankDicideSE;
    // Rank設定
    [System.Serializable]
    public struct RankAndItsScore
    {
        [SerializeField, Tooltip("ランクを設定してください")]
        public string rank;
        [SerializeField, Tooltip("ランクに至るための変数を設定してください")]
        public int rankScore;
    }
    [SerializeField, Tooltip("ランクとそのランクに至るための変数を設定してください")]
    public List<RankAndItsScore> rankAndItsScore = new List<RankAndItsScore>();

    void Awake()
    {
        // ScoreUItext取得
        scoretext = score.GetComponent<TextMeshProUGUI>();
        // RankUIのそれぞれのコンポーネント取得   
        rankAnim = rank.GetComponent<Animator>();
        ranktext = rank.GetComponent<TextMeshProUGUI>();
        // AudioSource取得
        audioSource = GetComponent<AudioSource>();

        myScore = scoreManager.GetScore();
        DicideRank();
    }


    void Update()
    {
        MultiplyScore();
        OnAnimation();
    }

    // スコアが加算され切った
    private bool stopMultiplyScore { get; set; }

    // 画面に表示させる数字
    private int displayScore = 0;
    // スコアを加算していく
    private void MultiplyScore()
    {
        displayScore++;
        displayScore = Mathf.Min(displayScore, myScore);
        scoretext.text = displayScore.ToString();

        stopMultiplyScore = displayScore == myScore;

        AddScoreSEManeger();
    }

    // スコア加算を一回だけ動かす
    private bool playAddScoreSE = false;
    private bool stopAddScoreSE = false;
    // スコア加算中のSE
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
    // アニメーションを順番に行う
    private void OnAnimation()
    {
        if (stopMultiplyScore)
            BehaviorWhenDisplayingHighlighit();

        if (GetEndOfAnim(highilightAnim, "HighilighitingAnim"))
            BehaviorWhenDisplayingRank();
    }


    // 獲得スコアがどのランクに分類されるか
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

    // ハイライトを一回だけ動かす
    private bool playHighilighit = false;
    // ハイライトを表示時の挙動
    private void BehaviorWhenDisplayingHighlighit()
    {
        if (playHighilighit) return;

        highilightAnim.SetTrigger("Highiliting");
        audioSource.clip = null;
        audioSource.PlayOneShot(higherHighilitSE);
        playHighilighit = true;
    }

    // ランクを一回だけ動かす
    private int playRank = 5;
    // ランクを表示時の挙動
    private void BehaviorWhenDisplayingRank()
    {
        if (playRank == 0) return;

        rankAnim.SetTrigger("RankDicision");
        audioSource.clip = null;
        audioSource.PlayOneShot(rankDicideSE);
        playRank--;
    }

    // アニメーションが終わったかどうか
    private bool GetEndOfAnim(Animator animator, string name)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(name)) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) return false;

        return true;
    }
}
