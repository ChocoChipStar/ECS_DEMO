using UnityEngine;

public class TitleSceneSoundEffectManager_V2 : AudioManager_V2
{
    [SerializeField, Header("ボタンを押す時のSEをアタッチしてください")]
    private AudioClip[] buttonPushSE;
    /// <summary>
    /// Playerの被ダメージ時のSE再生
    /// </summary>
    public void PlaySEButtonPush() { RandomPlayOneShot(buttonPushSE); }
}
