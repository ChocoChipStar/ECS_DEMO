using UnityEngine;
using UnityEngine.InputSystem;
using PlayerData = PlayerDataManager;
using PlayerScripts = PlayerScriptsManager;

public class MoveSystem : MonoBehaviour
{
    [Header("一フレーム前の座標")]
    private Vector3 lastPlayerPosition;

    private void Update()
    {
        InputActionSystem();
    }

    private void InputActionSystem()
    {
        var playerData = PlayerData.Instance;
        var playerScripts = PlayerScripts.Instance;

        KeyboardAction(playerData, playerScripts);

        if (Gamepad.current == null || Keyboard.current.anyKey.isPressed)
            return;

        ControllerAction(playerData, playerScripts);
    }

    private void ControllerAction(PlayerData playerData, PlayerScripts playerScripts)
    {
        if (Gamepad.current.aButton.wasPressedThisFrame && !playerScripts.animationSystem.GetSwingHammerAnimation())
            playerScripts.animationSystem.SwingHammerAnimation(true);

        var GamepadValue = Gamepad.current.leftStick.ReadValue();

        if (GamepadValue == Vector2.zero || playerScripts.animationSystem.GetSwingHammerAnimation())
        {
            playerScripts.animationSystem.RunningAnimation(false);
            return;
        }

        float stickAngle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;

        if (stickAngle < 0)
            stickAngle += 360;

        if(stickAngle != 0)
            playerData.transform.rotation = Quaternion.Euler(0, stickAngle, 0);

        playerData.transform.position += new Vector3
            (GamepadValue.x, 0, GamepadValue.y) * playerData.moveSpeed * Time.deltaTime;

        playerScripts.animationSystem.RunningAnimation(true);
    }

    private void KeyboardAction(PlayerData playerData, PlayerScripts playerScripts)
    {
        if (playerScripts.animationSystem.GetSwingHammerAnimation())
            return;

        var input = Keyboard.current;

        if (input.wKey.isPressed)
            playerData.transform.position += new Vector3(0.0f, 0.0f, playerData.moveSpeed) * Time.deltaTime;

        if (input.aKey.isPressed)
            playerData.transform.position += new Vector3(-playerData.moveSpeed, 0.0f, 0.0f) * Time.deltaTime;

        if (input.sKey.isPressed)
            playerData.transform.position += new Vector3(0.0f, 0.0f, -playerData.moveSpeed) * Time.deltaTime;

        if (input.dKey.isPressed)
            playerData.transform.position += new Vector3(playerData.moveSpeed, 0.0f, 0.0f) * Time.deltaTime;

        Vector3 differenceValue = PlayerData.Instance.transform.position - lastPlayerPosition;

        if (differenceValue.magnitude > 0.01f)
            playerData.transform.rotation = Quaternion.LookRotation(differenceValue);

        if (!input.wKey.isPressed && !input.aKey.isPressed && !input.sKey.isPressed && !input.dKey.isPressed)
            playerScripts.animationSystem.RunningAnimation(false);
        else
            playerScripts.animationSystem.RunningAnimation(true);

        if (input.spaceKey.wasPressedThisFrame && !playerScripts.animationSystem.GetSwingHammerAnimation())
            playerScripts.animationSystem.SwingHammerAnimation(true);

        lastPlayerPosition = playerData.transform.position;
    }
}
