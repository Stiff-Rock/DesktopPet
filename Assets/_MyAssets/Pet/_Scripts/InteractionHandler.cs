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

        CurrentInteractions |= eventData.button switch
        {
            PointerEventData.InputButton.Left => InteractionType.WasLeftClicked,
            PointerEventData.InputButton.Middle => InteractionType.WasMiddleClicked,
            PointerEventData.InputButton.Right => InteractionType.WasRightClicked,
            _ => InteractionType.None
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CurrentInteractions |= InteractionType.IsHovered;
        CursorManager.Instance.SetGameCursor(GameCursor.Hand);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CurrentInteractions &= ~InteractionType.IsHovered;
        CursorManager.Instance.SetGameCursor(GameCursor.Arrow);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurrentInteractions |= InteractionType.IsDragging;
        CursorManager.Instance.SetGameCursor(GameCursor.OpenHand);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CurrentInteractions &= ~InteractionType.IsDragging;
        CursorManager.Instance.SetGameCursor(GameCursor.Arrow);
    }

    private void LateUpdate()
    {
        CurrentInteractions &= ~InteractionType.WasLeftClicked;
        CurrentInteractions &= ~InteractionType.WasMiddleClicked;
        CurrentInteractions &= ~InteractionType.WasRightClicked;
    }
}

[Flags]
public enum InteractionType
{
    None = 0,
    WasLeftClicked = 1 << 0,
    WasMiddleClicked = 1 << 1,
    WasRightClicked = 1 << 2,
    IsHovered = 1 << 3,
    IsDragging = 1 << 4,
    IsPressed = 1 << 5,
}

public interface IInteractions
{
    public InteractionType CurrentInteractions { get; }
}