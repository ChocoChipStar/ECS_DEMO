using UnityEngine;
using UnityEngine.InputSystem;

public class TtashTest : MonoBehaviour
{
    [SerializeField, Tooltip("Scrap�𐶐�����ʂ�ݒ肵�Ă�������")]
    private int generateScrapNum;

    [SerializeField, Tooltip("ScrapPrefab���A�^�b�`���Ă�������")]
    private Transform scrapPrefab;

    // ScoreManeger�擾
    private ScoreManager scoreManeger;

    // ScrapGenerate�擾
    private ScrapGenerate scrapGenerate;

    private void Awake()
    {
        scrapGenerate = GetComponent<ScrapGenerate>();
    }

    private void Start()
    {
        scoreManeger = ScoreManager.instance;
    }

    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            BreakMySelf();
        }
    }

    // ��ꂽ��
    private void BreakMySelf()
    {
        for (int i = 0; i < generateScrapNum; i++)
        {
            scrapGenerate.ScrapUIGenerateSystem(transform.position);
        }
        scoreManeger.AddScore();

        gameObject.SetActive(false);
    }
}
