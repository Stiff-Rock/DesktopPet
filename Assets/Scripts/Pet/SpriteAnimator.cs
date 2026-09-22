using System;
using DG.Tweening;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float walkTweenDuration = 0.1f;
    [SerializeField] private Vector2 walkTweenScale;

    [Header("References")]
    [SerializeField] private InteractionHandler interactionHandler;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;

        if (!interactionHandler)
        {
            Debug.LogError($"SpriteAnimator's InteractionHandler is null in GameObject '{gameObject.name}'");
            return;
        }

        interactionHandler.OnDragEvent += PetStretch;
    }

    private void PetStretch(PointerEventData data)
    {
        Debug.Log($"PetStretch: {data.delta}");
    }

    public void WalkStretch()
    {
        // Make it pivot from the base
        Sequence squash = DOTween.Sequence();
        squash.Append(transform.DOScale(originalScale * walkTweenScale, walkTweenDuration))
        .SetEase(Ease.OutQuad)
        .SetId(gameObject)
        .SetAutoKill(true)
        .Append(transform.DOScale(originalScale, walkTweenDuration)
        .SetEase(Ease.OutBack)
        .SetId(gameObject)
        .SetAutoKill(true))
        .SetLink(gameObject);
    }

    private void OnEnable()
    {
        interactionHandler.OnDragEvent += PetStretch;
    }

    private void OnDisable()
    {
        interactionHandler.OnDragEvent -= PetStretch;
    }
}
