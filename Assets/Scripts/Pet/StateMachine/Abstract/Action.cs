using UnityEngine;

public abstract class Action : ScriptableObject
{
    public abstract bool Check(StateMachine owner);

    public virtual void DrawGizmos(StateMachine owner)
    {
    }
}