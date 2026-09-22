using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionHandler : MonoBehaviour, IInteractions, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [field: SerializeField] public InteractionType CurrentInteractions { get; private set; }

    public event Action<PointerEventData> OnDragEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentInteractions.HasFlag(InteractionType.IsDragging)) return;

        CurrentInteractions |= InteractionType.WasClicked;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CurrentInteractions |= InteractionType.IsHovered;
        NativeCursorManager.Instance.SetCursor(WindowsCursor.Hand);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CurrentInteractions &= ~InteractionType.IsHovered;
        NativeCursorManager.Instance.SetCursor(WindowsCursor.StandardArrow);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurrentInteractions |= InteractionType.IsDragging;
        NativeCursorManager.Instance.SetCursor(WindowsCursor.OpenHand);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CurrentInteractions &= ~InteractionType.IsDragging;
        NativeCursorManager.Instance.SetCursor(WindowsCursor.StandardArrow);
    }

    private void LateUpdate()
    {
        CurrentInteractions &= ~InteractionType.WasClicked;
    }
}

[System.Flags]
public enum InteractionType
{
    None = 0,
    WasClicked = 1 << 0,
    IsHovered = 1 << 1,
    IsDragging = 1 << 2,
    IsPressed = 1 << 3,
}

public interface IInteractions
{
    public InteractionType CurrentInteractions { get; }
}