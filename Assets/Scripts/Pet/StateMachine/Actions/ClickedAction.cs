using UnityEngine;

[CreateAssetMenu(fileName = "ClickedAction", menuName = "ScriptableObjects/Actions/ClickedAction")]
public class ClickedAction : Action
{
    public override bool Check(StateMachine owner)
    {
        if (owner.TryGetComponent(out InteractionHandler interactionHandler))
        {
            return interactionHandler.WasClicked;
        }
        else return false;
    }
}
