using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    // AudioSouce�擾
    [SerializeField, Tooltip("SE�p��AudioSouce���A�^�b�`���Ă�������")]
    private AudioSource audioSouce;

    // GameStartSE�擾
    [SerializeField,Tooltip("GameStart SE���A�^�b�`���Ă�������")]
    private AudioClip gameStartSE;

    void Update()
    {
        TransitionToMainScene();
    }

    // �{�^��������
    private bool pressedButton = true;
    // �b���𐔂���
    private float trasitionTimer = 0;
    // �V�[���ǂݍ���
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
