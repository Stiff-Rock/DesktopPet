using UnityEngine;

[CreateAssetMenu(fileName = "InteractedAction", menuName = "ScriptableObjects/Actions/InteractedAction")]
public class InteractedAction : Action
{
    [SerializeField] private InteractionType listenFor = InteractionType.None;

    public override bool Check(StateMachine owner)
    {
        if (owner.TryGetComponent(out InteractionHandler interactionHandler))
        {
            return (listenFor & interactionHandler.CurrentInteractions) != 0;
        }

        else return false;
    }
}
