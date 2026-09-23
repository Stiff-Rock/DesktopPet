using TMPro;
using UnityEngine;

public class PetSettings : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 defaultPetScale = new(0.3f, 0.3f, 1f);
    private Vector3 currentPetScale;
    [SerializeField] private float petScaleStep = 0.1f;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float maxScale = 1.0f;
    [SerializeField] private InteractionType menuInteraction = InteractionType.WasRightClicked;

    [Header("References")]
    [SerializeField] private GameObject canvasObj;
    [SerializeField] private TextMeshProUGUI scaleValueText;
    [SerializeField] private InteractionHandler interactionHandler;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float initialLocalY;
    private float unscaledSpriteHeight = 1f;

    private void Awake()
    {
        currentPetScale = defaultPetScale;
        initialLocalY = transform.localPosition.y;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            unscaledSpriteHeight = spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;
        }

        UpdateTransformScale();
    }

    private void Update()
    {
        if (interactionHandler != null && (interactionHandler.CurrentInteractions & menuInteraction) != 0)
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (canvasObj != null)
        {
            canvasObj.SetActive(!canvasObj.activeSelf);
        }
    }

    public void DecreasePetScale()
    {
        float newScale = Mathf.Clamp(currentPetScale.x - petScaleStep, minScale, maxScale);
        currentPetScale = new Vector3(newScale, newScale, 1f);
        UpdateTransformScale();
    }

    public void IncreasePetScale()
    {
        float newScale = Mathf.Clamp(currentPetScale.x + petScaleStep, minScale, maxScale);
        currentPetScale = new Vector3(newScale, newScale, 1f);
        UpdateTransformScale();
    }

    private void UpdateTransformScale()
    {
        transform.localScale = currentPetScale;

        float scaleDiffY = currentPetScale.y - defaultPetScale.y;
        float yOffset = scaleDiffY * unscaledSpriteHeight * 0.5f;

        Vector3 pos = transform.localPosition;
        pos.y = initialLocalY + yOffset;
        transform.localPosition = pos;

        if (scaleValueText != null)
        {
            scaleValueText.SetText($"{currentPetScale.x:F1}");
        }
    }

    public Vector3 GetCurrentScale()
    {
        return currentPetScale;
    }
}