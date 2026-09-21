using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionHandler : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public bool WasClicked { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        WasClicked = true;
    }

    private void LateUpdate()
    {
        WasClicked = false;
    }
}
