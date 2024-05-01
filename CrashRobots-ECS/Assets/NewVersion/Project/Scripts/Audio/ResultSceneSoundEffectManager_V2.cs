using UnityEngine;

public class ResultSceneSoundEffectManager_V2 : GenericSingleton<ResultSceneSoundEffectManager_V2>
{
    [SerializeField, Header("�g�p����AudioSouce��A�^�b�`���Ă�������")]
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

    [SerializeField, Header("���U���g����SE��A�^�b�`���Ă�������")]
    private AudioClip[] inResultSoundEffect;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEInResult() { /*audioSource.clip = null*/; RandomPlayOneShot(inResultSoundEffect); }

    [SerializeField, Header("addScoreSE��A�^�b�`���Ă�������")]
    private AudioClip[] addScoreSoundEffect;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlayAddScoreSoundEffect() { RandomPlayOneShot(addScoreSoundEffect); }
    // �X�R�A���Z���񂾂�������
    private bool playAddScoreSE = false;
    private bool stopAddScoreSE = false;
    // �X�R�A���Z����SE
    public void AddScoreSEManeger(bool stopMultiplyScore)
    {
        if (stopMultiplyScore)
        {
            if (stopAddScoreSE) return;

            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.Stop();
            stopAddScoreSE = true;
        }
        else
        {
            if (playAddScoreSE) return;

            PlayAddScoreSoundEffect();
            audioSource.loop = true;
            audioSource.PlayDelayed(0.0f);
            playAddScoreSE = true;
        }
    }

    [SerializeField, Header("addScoreSE��A�^�b�`���Ă�������")]
    private AudioClip[] rankDicideSoundEffect;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlayRankDicideSoundEffect() { /*audioSource.clip = null*/; RandomPlayOneShot(rankDicideSoundEffect); }

    [SerializeField, Header("higherHighilitSE��A�^�b�`���Ă�������")]
    private AudioClip[] higherHighilitSoundEffect;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlayHigherHighilitSoundEffect() { /*audioSource.clip = null*/; RandomPlayOneShot(higherHighilitSoundEffect); }

    [SerializeField, Header("lowerHighilitSESE��A�^�b�`���Ă�������")]
    private AudioClip[] lowerHighilitSoundEffect;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlayLowerHighilitffectSoundEffect() { audioSource.clip = null; RandomPlayOneShot(lowerHighilitSoundEffect); }

    //[SerializeField, Header("���U���g����SE��A�^�b�`���Ă�������")]
    //private AudioClip[] S
    //
    //
    //oundEffect;
    ///// <summary>
    ///// Player�̔�_���[�W����SE�Đ�
    ///// </summary>
    //public void PlaySoundEffect() { RandomPlayOneShot(SoundEffect); }



}
