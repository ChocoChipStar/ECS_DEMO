using UnityEngine;

public class ResultAnimationManager_V2 : MonoBehaviour
{
    // ResultManager_V2
    ResultManager_V2 resultManager;
    [SerializeField,Header("HighlightAnimator���A�^�b�`���Ă�������")]
    private Animator highlightAnimator;

    private void Start()
    {
        resultManager = ResultManager_V2.Instance;
    }

    private void Update()
    {
        OnAnimation();
    }

    // �A�j���[�V���������Ԃɍs��
    private void OnAnimation()
    {
        if (resultManager.scoreSystem.stopMultiplyScore)
            BehaviorWhenDisplayingHighlighit();
    }

    // �n�C���C�g����񂾂�������
    private bool playHighilighit = false;
    // �n�C���C�g��\�����̋���
    private void BehaviorWhenDisplayingHighlighit()
    {
        if (playHighilighit) return;

        highlightAnimator.SetTrigger("Highiliting");
        playHighilighit = true;
    }

    // �A�j���[�V�������I��������ǂ���
    private bool GetEndOfAnim(Animator animator, string name)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(name)) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) return false;

        return true;
    }

}
