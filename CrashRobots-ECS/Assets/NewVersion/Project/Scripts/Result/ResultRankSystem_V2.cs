using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultRankSystem_V2 : MonoBehaviour
{
    // RankUI�擾
    [SerializeField, Tooltip("Rank���A�^�b�`���Ă�������")]
    private RectTransform rank;
    // RankUIText�擾
    private TextMeshProUGUI ranktext;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
