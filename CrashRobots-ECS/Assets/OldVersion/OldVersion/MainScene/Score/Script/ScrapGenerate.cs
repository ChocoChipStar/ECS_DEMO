using UnityEngine;

public class ScrapGenerate : MonoBehaviour
{
    // シングルトン化
    public static ScrapGenerate instance;

    // Scrapプレハブ取得
    [SerializeField, Tooltip("Scrapプレハブをアタッチしてください")]
    private Transform scrapPrefab;
    // ScoreUI取得
    [SerializeField, Tooltip("ScoreUIをアタッチして下さい")]
    private RectTransform scoreUI;
    // Canvas取得
    [SerializeField, Tooltip("Canvasをアタッチしてください")]
    private RectTransform canvas;
    // ScrapUIDisplay取得
    [SerializeField, Tooltip("ScrapUIDisplayをアタッチしてください")]
    private ScrapUIDisplay scrapUIDisplay;

    private void Awake()
    {
        // 親子関係解除
        transform.parent = null;
        // Canvasの子供ではなくなるためTransformに切り替え
        gameObject.AddComponent<Transform>();
        Destroy(GetComponent<RectTransform>());

        // シングルトン化
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

    // 必要オブジェクト生成
    public Transform ScrapUIGenerateSystem(Vector3 position)
    {
        // Scrap生成
        var scrapClone = Instantiate(scrapPrefab, position, Quaternion.identity);
        var scrapCloneScript = scrapClone.GetComponent<Scrap>();
        scrapCloneScript.Initialize(scoreUI);
        // ScrapUI生成、初期化
        var marker = Instantiate(scrapUIDisplay, canvas);
        marker.Initialize(scrapClone);

        return scrapClone;
    }
}
