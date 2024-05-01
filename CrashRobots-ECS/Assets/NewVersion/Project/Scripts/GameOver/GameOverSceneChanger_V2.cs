using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverSceneChanger_V2 : MonoBehaviour
{
    [SerializeField, Tooltip("ContinuePickArrowをアタッチしてください")]
    private GameObject continuePickArrow;
    [SerializeField, Tooltip("TitlePickArrowをアタッチしてください")]
    private GameObject titlePickArrow;

    // Gamupad取得
    private Gamepad gamepad = null;

    void Update()
    {
        if (!ControllerCurrent()) return;

        SceneChange();
    }

    private bool ControllerCurrent()
    {
        if (Gamepad.current == null) return false;
        gamepad = Gamepad.current;
        return true;
    }

    private bool SideInput()
    {
        //if (Mathf.Abs(gamepad.leftStick.y.value) > 0.3f) return true;
        if (gamepad.dpad.left.wasPressedThisFrame) return true;
        if (gamepad.dpad.right.wasPressedThisFrame) return true;
        if (Keyboard.current.dKey.wasReleasedThisFrame) return true;
        if (Keyboard.current.aKey.wasReleasedThisFrame) return true;
        if (Keyboard.current.rightArrowKey.wasReleasedThisFrame) return true;
        if (Keyboard.current.leftArrowKey.wasReleasedThisFrame) return true;
        if (KnockDownLeftstick())return true;
        return false;
    }
    private bool knockDownOneTime = false;
    private bool KnockDownLeftstick()
    {
        Vector2 leftStickValue = Gamepad.current.leftStick.ReadValue();
        var knockDownLeftstick = Mathf.Abs(leftStickValue.x) >= 0.5;
        if (knockDownLeftstick)
        {
            if (knockDownOneTime){
                knockDownOneTime = false;
                return true;
            }
        }
        else
        {
            knockDownOneTime = true;
        }
        return false;
    }


    private bool DicideInput()
    {
        if (gamepad.buttonEast.wasPressedThisFrame) return true;
        return false;
    }

    // シーン選択
    private enum PickScene { continueScene, titleScene }
    private PickScene pickScene = PickScene.continueScene;

    private void PickSceneIsRetry()
    {
        if (SideInput()) pickScene = PickScene.titleScene;
        if (DicideInput()) SceneManager.LoadScene("MainScene");
        continuePickArrow.SetActive(true);
        titlePickArrow.SetActive(false);
    }

    private void PickSceneIsTitle()
    {
        if (SideInput()) pickScene = PickScene.continueScene;
        if (DicideInput()) SceneManager.LoadScene("TitleScene");
        continuePickArrow.SetActive(false);
        titlePickArrow.SetActive(true);
    }

    private void SceneChange()
    {
        switch (pickScene)
        {
            case PickScene.continueScene:
                PickSceneIsRetry();
                break;
            case PickScene.titleScene:
                PickSceneIsTitle();
                break;
        }
    }
}
