using UnityEngine;

public class GameOverSceneAudioManager_V2 : AudioManager_V2
{
    [SerializeField, Header("�Q�[���I�[�o�[����SE���A�^�b�`���Ă�������")]
    private AudioClip[] gameOverSE;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEEGameOver() { RandomPlayOneShot(gameOverSE); }

    [SerializeField, Header("�{�^��������SE���A�^�b�`���ĉ�����")]
    private AudioClip[] pushButtonSE;
    /// <summary>
    /// Player�̔�_���[�W����SE�Đ�
    /// </summary>
    public void PlaySEPushButton() { RandomPlayOneShot(pushButtonSE); }
}
