using UnityEngine;
using PlayerData = PlayerDataManager;
using PlayerScripts = PlayerScriptsManager;

public class AnimationSystem : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    private const float ANIMATION_PLAYBACK_TIME = 1.0f;
    private const float ANIMATION_CRASH_TIME = 0.55f;

    private bool isCrashed = false;

    private void Update()
    {
        ConfirmationAnimation();
    }

    private void ConfirmationAnimation()
    {
        var playerAnimation = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hammer");
        var animationTime = playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (!playerAnimation)
            return;

        if (animationTime >= ANIMATION_PLAYBACK_TIME)
        {
            playerAnimator.SetBool("IsSwing", false);
            isCrashed = false;
        }
        else if (animationTime >= ANIMATION_CRASH_TIME && !isCrashed)
        {
            PlayerScripts.Instance.swingHammerSystem.SetSwingHammerActive(true);
            PlayerScripts.Instance.hitStopSystem.StartHitStop(PlayerData.Instance.hitStopTime);
            isCrashed = true;
        }
    }

    public void RunningAnimation(bool isActive)
    {
        playerAnimator.SetBool("IsRunning", isActive);
    }

    public void SwingHammerAnimation(bool isActive)
    {
        playerAnimator.SetBool("IsSwing", isActive);
    }

    public bool GetSwingHammerAnimation()
    {
        return playerAnimator.GetBool("IsSwing");
    }
}
