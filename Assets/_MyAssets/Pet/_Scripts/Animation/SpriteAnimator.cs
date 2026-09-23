using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnimationsSettings animSettings;
    [SerializeField] private InteractionHandler interactionHandler;
    [SerializeField] private PetSettings petSettings;
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (!interactionHandler)
        {
            Debug.LogError($"SpriteAnimator's InteractionHandler is null in GameObject '{gameObject.name}'");
            return;
        }

        interactionHandler.OnDragEvent += PetStretch;
    }

    public void WalkStretch()
    {
        Vector3 targetScale = petSettings.GetCurrentScale() * animSettings.WalkTweenScaleMod;

        DOTween.Sequence()
            .SetId(gameObject)
            .SetAutoKill(true)
            .SetLink(gameObject)
            .SetEase(animSettings.WalkEase)
            .Append(pivotTransform.DOScale(targetScale, animSettings.WalkTweenSequenceDuration))
            .Append(pivotTransform.DOScale(petSettings.GetCurrentScale(), animSettings.WalkTweenSequenceDuration));
    }

    public void BarkStretch()
    {
        Vector3 targetScale = petSettings.GetCurrentScale() * animSettings.BarkTweenScaleMod;

        DOTween.Sequence()
            .SetId(gameObject)
            .SetAutoKill(true)
            .SetLink(gameObject)
            .SetEase(animSettings.BarkEase)
            .Append(pivotTransform.DOScale(targetScale, animSettings.BarkTweenSequenceDuration))
            .Append(pivotTransform.DOScale(petSettings.GetCurrentScale(), animSettings.BarkTweenSequenceDuration));
    }

    private void PetStretch(PointerEventData data)
    {
        Debug.Log($"PetStretch: {data.delta}");
    }

    #region Events

    private void OnEnable()
    {
        interactionHandler.OnDragEvent += PetStretch;
    }

    private void OnDisable()
    {
        interactionHandler.OnDragEvent -= PetStretch;
    }

    #endregion
}
