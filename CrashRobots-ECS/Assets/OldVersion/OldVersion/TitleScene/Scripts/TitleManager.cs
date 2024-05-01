using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    // AudioSouce取得
    [SerializeField, Tooltip("SE用のAudioSouceをアタッチしてください")]
    private AudioSource audioSouce;

    // GameStartSE取得
    [SerializeField,Tooltip("GameStart SEをアタッチしてください")]
    private AudioClip gameStartSE;

    void Update()
    {
        TransitionToMainScene();
    }

    // ボタン押した
    private bool pressedButton = true;
    // 秒数を数える
    private float trasitionTimer = 0;
    // シーン読み込み
    private void TransitionToMainScene()
    {
        var current = Gamepad.current;
        if (current == null) return;

        if (pressedButton)
        {
            if (current.buttonEast.wasPressedThisFrame)
            {
                pressedButton = false;
                audioSouce.PlayOneShot(gameStartSE);
            }
        }
        else
        {
            trasitionTimer += Time.deltaTime;
            if (trasitionTimer >= 1.3f)
                SceneManager.LoadScene("MainScene");
        }
    }
}
