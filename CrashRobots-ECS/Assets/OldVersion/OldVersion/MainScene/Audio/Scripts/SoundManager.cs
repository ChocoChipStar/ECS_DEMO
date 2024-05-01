using UnityEngine;

public class SoundManager : GenericSingleton<SoundManager>
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField, Header("���鎞�p�̉����A�^�b�`���Ă�������")]
    private AudioClip[] brokenSound;

    [SerializeField, Header("�����������p�̉����A�^�b�`���Ă�������")]
    private AudioClip[] hitSound;

    public void BrokenSound()
    {
        audioSource.PlayOneShot(brokenSound[GetRandomValue(brokenSound.Length)]);
    }

    public void HitSound()
    {
        audioSource.PlayOneShot(hitSound[GetRandomValue(hitSound.Length)]);
    }

    private int GetRandomValue(int soundLength)
    {
        return Random.Range(0, soundLength);
    }

}
