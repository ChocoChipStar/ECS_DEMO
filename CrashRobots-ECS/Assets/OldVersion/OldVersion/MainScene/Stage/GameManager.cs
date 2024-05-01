using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GAME_STATUS { Play, Clear, Pause, GameOver };
    public static GAME_STATUS status;

    [SerializeField]
    TextMeshProUGUI timeText;

    [SerializeField]
    private GameObject resultManager;

    public float countdownMinutes = 3;
    private float countdownSeconds;
    
    private float timelimit = 30;
    // Start is called before the first frame update
    void Start()
    {
        //timeText = GetComponent<TextMeshProUGUI>();
        countdownSeconds = countdownMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownSeconds > 0)
        {
            countdownSeconds -= Time.deltaTime;
            var span = new TimeSpan(0, 0, (int)countdownSeconds);
            timeText.text = span.ToString(@"mm\:ss");

            if (countdownSeconds <= 0)
            {
                // 0•b‚É‚È‚Á‚½‚Æ‚«‚Ìˆ—
                resultManager.SetActive(true);
            }
        }
    }
}
