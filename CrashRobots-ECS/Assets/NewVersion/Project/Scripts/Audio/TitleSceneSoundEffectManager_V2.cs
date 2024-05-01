using UnityEngine;

public class TitleSceneSoundEffectManager_V2 : AudioManager_V2
{
    [SerializeField, Header("�{�^������������SE���A�^�b�`���Ă�������")]
    private AudioClip[] buttonPushSE;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEButtonPush() { RandomPlayOneShot(buttonPushSE); }
}
