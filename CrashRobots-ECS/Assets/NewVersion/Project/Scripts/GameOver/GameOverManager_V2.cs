using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOverManager_V2 : MonoBehaviour
{
    private float leftStickX;

    private bool isLeft;
    private bool isWait = false;

    private void Start()
    {
        isWait = false;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        isWait = true;
    }

    private void Update()
    {
        if (!isWait)
            return;

        leftStickX = Gamepad.current.leftStick.ReadValue().x;

        if (leftStickX < 0 || Keyboard.current.aKey.wasPressedThisFrame)
            isLeft = true;
        else if (leftStickX > 0 || Keyboard.current.dKey.wasPressedThisFrame)
            isLeft = false;

        if (Keyboard.current.spaceKey.wasReleasedThisFrame || Gamepad.current.bButton.wasReleasedThisFrame)
        {
            if (isLeft)
                OnClickTitle();
            else
                OnClickContinue();
        }
    }


    public void OnClickTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickContinue()
    {
        SceneManager.LoadScene("MainScene");
    }
}
