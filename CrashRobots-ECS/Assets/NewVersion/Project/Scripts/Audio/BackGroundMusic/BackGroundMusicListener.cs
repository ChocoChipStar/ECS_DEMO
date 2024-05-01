using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicListener : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip clip;

    private void Awake()
    {
        PlayBackGroundMusic();
    }

    private void PlayBackGroundMusic()
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
