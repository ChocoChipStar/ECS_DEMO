using System.Collections.Generic;
using UnityEngine;

public class AudioManager_V2 : Singleton_V2<AudioManager_V2>
{
    [SerializeField, Header("使用するAudioSouceをアタッチしてください")]
    private AudioSource audioSource;

    // ランダム値取得
    private int GetRandomValue(int soundLength)
    {
        return Random.Range(0, soundLength);
    }

    /// <summary>
    /// ランダムにSE再生
    /// </summary>
    /// <param name="audioClips"><ランダム実行したいAudioClip[]を入力/param>
    public void RandomPlayOneShot(AudioClip[] audioClips)
    {
        audioSource.PlayOneShot(audioClips[GetRandomValue(audioClips.Length)]);
    }
}
