using UnityEngine;
using UnityEngine.UI;

public class RandomSpriteSystem_V2 : MonoBehaviour
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
        RandomSprite(scrapSprite);
    }
    // �����_���ɉ摜�\��
    private void RandomSprite(Sprite[] sprites) { image.sprite = sprites[Random.Range(0, sprites.Length)]; }
}
