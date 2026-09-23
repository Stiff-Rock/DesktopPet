using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [Header("State Machine Settings")]
    [SerializeField] private State entryPoint;
    [SerializeField] private State currentState;
    public State PausedState { get; private set; }


    [field: Header("References")]
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
        if (!nextState || nextState == currentState) return;

        // Determine wether if the current state is getting interrumpted (paused) 
        // by the incoming state or if its normally transitioning into it
        if (nextState.IsInterrupting)
        {
            PausedState = currentState;
            if (currentState)
                currentState.OnStatePause(this);
        }
        else
        {
            if (currentState)
                currentState.OnStateExit(this);
        }

        // Determine wether if it's resuming a previously paused state or starting a new one
        if (nextState == PausedState)
        {
            nextState.OnStateResume(this);
            PausedState = null;
        }
        else
        {
            nextState.OnStateEnter(this);
        }

        currentState = nextState;
    }

    #region Debug

    private void OnDrawGizmos()
    {
        if (currentState) currentState.DrawGizmos(this);
    }

    #endregion
}
