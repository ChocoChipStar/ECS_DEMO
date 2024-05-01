using UnityEngine;
using UnityEngine.UI;

public class ScrapUIManeger : MonoBehaviour
{
    // 画像取得
    [SerializeField, Tooltip("使用する画像を必要分アタッチしてください")]
    private Sprite[] scrapSprite;

    // Image取得
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        RandomSprite();
    }

    private void RandomSprite() { image.sprite = scrapSprite[Random.Range(0, scrapSprite.Length)]; }
}
