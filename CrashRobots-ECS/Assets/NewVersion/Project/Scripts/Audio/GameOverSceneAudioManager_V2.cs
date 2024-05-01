using UnityEngine;

public class GameOverSceneAudioManager_V2 : AudioManager_V2
{
    [SerializeField, Header("ゲームオーバー時のSEをアタッチしてください")]
    private AudioClip[] gameOverSE;
    /// <summary>
    /// Playerの被ダメージ時のSE再生
    /// </summary>
    public void PlaySEEGameOver() { RandomPlayOneShot(gameOverSE); }

    [SerializeField, Header("ボタンを押すSEをアタッチして下さい")]
    private AudioClip[] pushButtonSE;
    /// <summary>
    /// Playerの被ダメージ時のSE再生
    /// </summary>
    public void PlaySEPushButton() { RandomPlayOneShot(pushButtonSE); }
}
