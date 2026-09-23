using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationsSettings", menuName = "ScriptableObjects/AnimationsSettings")]
public class AnimationsSettings : ScriptableObject
{
    [field: Header("Animation Settings")]
    // Idle
    [field: SerializeField] public float IdleTweenSequenceDuration { get; private set; }
    [field: SerializeField] public Vector2 IdleTweenScaleMod { get; private set; }
    [field: SerializeField] public Ease IdleEase { get; private set; }

    [field: Space(10)]

    // Walk
    [field: SerializeField] public float WalkTweenSequenceDuration { get; private set; }
    [field: SerializeField] public Vector2 WalkTweenScaleMod { get; private set; }
    [field: SerializeField] public Ease WalkEase { get; private set; }

    [field: Space(10)]

    // Bark
    [field: SerializeField] public float BarkTweenSequenceDuration { get; private set; }
    [field: SerializeField] public Vector2 BarkTweenScaleMod { get; private set; }
    [field: SerializeField] public Ease BarkEase { get; private set; }


}
