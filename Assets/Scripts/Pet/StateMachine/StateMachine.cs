using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [Header("State Machine Settings")]
    [SerializeField] private State entryPoint;
    [SerializeField] private State currentState;
    public State PausedState { get; private set; }

    [Header("References")]
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    private void Start()
    {
        currentState = entryPoint;
        currentState.OnStateEnter(this);
    }

    private void Update()
    {
        RunState();
    }

    private void RunState()
    {
        State nextState = currentState.Run(this);
        if (!nextState) return;

        if (nextState != currentState)
        {
            if (nextState.IsInterrupting)
            {
                PausedState = currentState;
                currentState.OnStatePause(this);
            }
            else if (currentState) currentState.OnStateExit(this);

            if (PausedState == nextState)
                nextState.OnStateResume(this);
            else
                nextState.OnStateEnter(this);

            currentState = nextState;
        }
    }

    #region Debug

    private void OnDrawGizmos()
    {
        if (currentState) currentState.DrawGizmos(this);
    }

    #endregion
}
