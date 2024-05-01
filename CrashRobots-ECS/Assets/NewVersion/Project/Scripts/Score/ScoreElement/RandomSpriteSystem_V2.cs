using UnityEngine;
using UnityEngine.UI;

public class RandomSpriteSystem_V2 : MonoBehaviour
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
        RandomSprite(scrapSprite);
    }
    // ランダムに画像表示
    private void RandomSprite(Sprite[] sprites) { image.sprite = sprites[Random.Range(0, sprites.Length)]; }
}
