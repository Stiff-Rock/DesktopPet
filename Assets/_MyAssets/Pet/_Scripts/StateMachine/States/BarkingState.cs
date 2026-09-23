using UnityEngine;

[CreateAssetMenu(fileName = "BarkingState", menuName = "ScriptableObjects/States/Barking")]
public class BarkingState : State
{
    [SerializeField] private AudioClip[] barkClips;
    private static readonly int IsBarkingHash = Animator.StringToHash("IsBarking");

    public override void OnStateEnter(StateMachine owner)
    {
        AudioClip randomBarkClip = barkClips[Random.Range(0, barkClips.Length)];
        owner.AudioSource.PlayOneShot(randomBarkClip);
        owner.Animator.SetBool(IsBarkingHash, true);
    }

    public override void OnStateExit(StateMachine owner)
    {
        owner.Animator.SetBool(IsBarkingHash, false);
    }
}
