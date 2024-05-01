using UnityEngine;
using UnityEngine.InputSystem;

public class TtashTest : MonoBehaviour
{
    [SerializeField, Tooltip("Scrapを生成する量を設定してください")]
    private int generateScrapNum;

    [SerializeField, Tooltip("ScrapPrefabをアタッチしてください")]
    private Transform scrapPrefab;

    // ScoreManeger取得
    private ScoreManager scoreManeger;

    // ScrapGenerate取得
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

    // 壊れた時
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
