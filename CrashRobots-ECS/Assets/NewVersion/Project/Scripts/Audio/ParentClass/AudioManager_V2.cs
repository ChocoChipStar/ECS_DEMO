using System.Collections.Generic;
using UnityEngine;

public class AudioManager_V2 : Singleton_V2<AudioManager_V2>
{
    [SerializeField, Header("�g�p����AudioSouce���A�^�b�`���Ă�������")]
    private AudioSource audioSource;

    // �����_���l�擾
    private int GetRandomValue(int soundLength)
    {
        return Random.Range(0, soundLength);
    }

    /// <summary>
    /// �����_����SE�Đ�
    /// </summary>
    /// <param name="audioClips"><�����_�����s������AudioClip[]�����/param>
    public void RandomPlayOneShot(AudioClip[] audioClips)
    {
        audioSource.PlayOneShot(audioClips[GetRandomValue(audioClips.Length)]);
    }
}
