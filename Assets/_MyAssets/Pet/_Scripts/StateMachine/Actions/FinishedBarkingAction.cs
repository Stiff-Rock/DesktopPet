using UnityEngine;

[CreateAssetMenu(fileName = "FinishedBarkingAction", menuName = "ScriptableObjects/Actions/FinishedBarkingAction")]
public class FinishedBarkingAction : Action
{
    public override bool Check(StateMachine owner)
    {
        return FinishedBarkingAnim(owner);
    }

    private bool FinishedBarkingAnim(StateMachine owner)
    {
        AnimatorStateInfo stateInfo = owner.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Bark") && !owner.Animator.IsInTransition(0))
        {
            return stateInfo.normalizedTime >= 1.0f;
        }
        else return false;
    }
}
