using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextDrawer : MonoBehaviour
{
    public static TextDrawer instance;

    [SerializeField]
    private TextMeshProUGUI enemyCountText;
    
    [SerializeField]
    private TextMeshProUGUI bulletCountText;

    [SerializeField]
    private TextMeshProUGUI playerHealthText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemyCountText(int value)
    {
        enemyCountText.SetText("ENEMIES:" + value.ToString());
    }

    public void SetBulletCountText(int value)
    {
        bulletCountText.SetText("BULLETS:" + value.ToString());
    }

    public void SetPlayerHealthText(int value)
    {
        playerHealthText.SetText("HP:" + value.ToString());
    }
}
