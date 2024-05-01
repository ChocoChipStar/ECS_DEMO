using UnityEngine;
using UnityEngine.UI;

public class ScrapUIManeger : MonoBehaviour
{
    // �摜�擾
    [SerializeField, Tooltip("�g�p����摜��K�v���A�^�b�`���Ă�������")]
    private Sprite[] scrapSprite;

    // Image�擾
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        RandomSprite();
    }

    private void RandomSprite() { image.sprite = scrapSprite[Random.Range(0, scrapSprite.Length)]; }
}
