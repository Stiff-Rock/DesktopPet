using DG.Tweening;
using UnityEngine;

public class IdleTweenAnimation : StateMachineBehaviour
{
    [Header("Animation Settings")]
        [SerializeField] private AnimationsSettings animSettings;

    private Vector3 originalScale;
    private Tween tween;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // The parent is always going to be the Sprite Pivot, which has the transform that we want to acquire
        Transform pivotTransform = animator.gameObject.transform.parent;
        originalScale = pivotTransform.localScale;

        Vector3 targetScale = originalScale * animSettings.IdleTweenScaleMod;
        tween = DOTween.Sequence()
            .SetId(animator.gameObject)
            .SetAutoKill(true)
            .SetLink(animator.gameObject)
            .Append(pivotTransform.DOScale(targetScale, animSettings.IdleTweenSequenceDuration).SetEase(animSettings.IdleEase))
            .Append(pivotTransform.DOScale(originalScale, animSettings.IdleTweenSequenceDuration).SetEase(animSettings.IdleEase))
            .SetLoops(-1);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tween != null && tween.active)
        {
            tween.Kill();
            tween = null;
            animator.gameObject.transform.parent.localScale = originalScale;
        }
    }
}
