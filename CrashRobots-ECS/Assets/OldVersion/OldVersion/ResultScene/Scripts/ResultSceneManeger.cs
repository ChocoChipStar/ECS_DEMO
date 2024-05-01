using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SceneManagement;

public class ResultSceneManeger : MonoBehaviour
{
    // RetryPickArrow取得
    [SerializeField, Tooltip("RetryPickArrowをアタッチしてください")]
    private GameObject retryPickArrow;

    // RetryPickArrow取得
    [SerializeField, Tooltip("TitlePickArrowをアタッチしてください")]
    private GameObject titlePickArrow;

    // シーン選択
    private enum PickScene { retry,title }
    private PickScene pickScene = PickScene.retry;

    // Gamupad取得
    private Gamepad gamepad = null;

    void Awake()
    {

    }


    void Update()
    {
        if (!ControllerCurrent()) return;

        SceneChange();
    }

    private bool ControllerCurrent()
    {
        if (Gamepad.current == null) return false;

        return true;
    }

    private bool SideInput()
    {
        //if (Mathf.Abs(gamepad.leftStick.y.value) > 0.3f) return true;
        if (gamepad.dpad.left.wasPressedThisFrame) return true;
        if (gamepad.dpad.right.wasPressedThisFrame) return true;
        return false;
    }

    private bool DicideInput()
    {
        if (gamepad.buttonEast.wasPressedThisFrame) return true;
        return false;
    }

    private void PickSceneIsRetry()
    {
        if (SideInput()) pickScene = PickScene.title;
        if (DicideInput()) SceneManager.LoadScene("MainScene");
        retryPickArrow.SetActive(true);
        titlePickArrow.SetActive(false);
    }

    private void PickSceneIsTitle()
    {
        if (SideInput()) pickScene = PickScene.retry;
        if (DicideInput()) SceneManager.LoadScene("TitleScene");
        retryPickArrow.SetActive(false);
        titlePickArrow.SetActive(true);
    }

    private void SceneChange()
    {
        switch (pickScene)
        {
            case PickScene.retry:
                PickSceneIsRetry();
                break;
            case PickScene.title:
                PickSceneIsTitle();
                break;
        }
    }
}
