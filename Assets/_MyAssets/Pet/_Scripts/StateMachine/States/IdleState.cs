using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "ScriptableObjects/States/Idle")]
public class IdleState : State
{
    // Animation
    private static readonly int IsIdleHash = Animator.StringToHash("IsIdle");
    private float pausedAnimTime;

    public override void OnStateEnter(StateMachine owner)
    {
        owner.Animator.SetBool(IsIdleHash, true);
        ResetAnimationTime();
    }

    public override void OnStateExit(StateMachine owner)
    {
        ResetAnimationTime();
    }

    public override void OnStatePause(StateMachine owner)
    {
        AnimatorStateInfo currentAnim = owner.Animator.GetCurrentAnimatorStateInfo(0);
        pausedAnimTime = currentAnim.normalizedTime % 1.0f;
    }

    public override void OnStateResume(StateMachine owner)
    {
        owner.Animator.Play(IsIdleHash, 0, pausedAnimTime);
    }

    private void ResetAnimationTime()
    {
        pausedAnimTime = 0;
    }
}
