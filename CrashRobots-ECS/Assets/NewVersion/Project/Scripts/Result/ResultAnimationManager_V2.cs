using UnityEngine;

public class ResultAnimationManager_V2 : MonoBehaviour
{
    // ResultManager_V2
    ResultManager_V2 resultManager;
    [SerializeField,Header("HighlightAnimatorをアタッチしてください")]
    private Animator highlightAnimator;

    private void Start()
    {
        resultManager = ResultManager_V2.Instance;
    }

    private void Update()
    {
        OnAnimation();
    }

    // アニメーションを順番に行う
    private void OnAnimation()
    {
        if (resultManager.scoreSystem.stopMultiplyScore)
            BehaviorWhenDisplayingHighlighit();
    }

    // ハイライトを一回だけ動かす
    private bool playHighilighit = false;
    // ハイライトを表示時の挙動
    private void BehaviorWhenDisplayingHighlighit()
    {
        if (playHighilighit) return;

        highlightAnimator.SetTrigger("Highiliting");
        playHighilighit = true;
    }

    // アニメーションが終わったかどうか
    private bool GetEndOfAnim(Animator animator, string name)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(name)) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) return false;

        return true;
    }

}
