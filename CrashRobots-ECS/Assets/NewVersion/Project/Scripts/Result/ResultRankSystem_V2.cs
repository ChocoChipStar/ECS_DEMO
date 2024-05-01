using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultRankSystem_V2 : MonoBehaviour
{
    // RankUI取得
    [SerializeField, Tooltip("Rankをアタッチしてください")]
    private RectTransform rank;
    // RankUIText取得
    private TextMeshProUGUI ranktext;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
