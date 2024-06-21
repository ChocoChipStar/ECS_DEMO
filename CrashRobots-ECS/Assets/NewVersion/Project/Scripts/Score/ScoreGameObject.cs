using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreGameObject : MonoBehaviour
{
    public static ScoreGameObject instance;

    [SerializeField]
    private TextMeshProUGUI enemyCountText;
    
    [SerializeField]
    private TextMeshProUGUI bulletCountText;

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
        enemyCountText.text = value.ToString();
    }

    public void SetBulletCountText(int value)
    {
        bulletCountText.text = value.ToString();
    }
}
