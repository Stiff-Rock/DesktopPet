using UnityEngine;

[System.Serializable]
struct StateParameters
{
    [SerializeField] private Action action;

    [SerializeField] private bool actionValue;

    [SerializeField] private State nextState;

    public readonly Action GetAction()
    {
        return action;
    }

    public readonly bool GetActionValue()
    {
        return actionValue;
    }

    public readonly State GetNextState()
    {
        return nextState;
    }
}

public abstract class State : ScriptableObject
{
    [field: SerializeField] public bool IsInterrupting { get; private set; }
    [SerializeField] private StateParameters[] parameters;

    public virtual void OnStateEnter(StateMachine owner) { }

    public virtual void OnStateExit(StateMachine owner) { }

    public virtual void OnStatePause(StateMachine owner) { }

    public virtual void OnStateResume(StateMachine owner) { }

    public virtual State Run(StateMachine owner)
    {
        foreach (StateParameters param in parameters)
        {
            if (param.GetAction().Check(owner) == param.GetActionValue())
            {
                return param.GetNextState();
            }
        }

        return null;
    }

    public void DrawGizmos(StateMachine owner)
    {
        foreach (StateParameters param in parameters)
        {
            if (param.GetAction() != null)
                param.GetAction().DrawGizmos(owner);
        }
    }
}