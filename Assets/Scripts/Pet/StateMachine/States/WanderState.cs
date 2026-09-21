using UnityEngine;

[CreateAssetMenu(fileName = "WanderState", menuName = "ScriptableObjects/States/Wander")]
public class WanderState : State
{
    [Header("Idle")]
    [SerializeField, Range(0f, 10f)] private float idleMinDuration = 3f;
    [SerializeField, Range(0f, 10f)] private float idleMaxDuration = 8f;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField, Range(0f, 10f)] private float moveMinDuration = 2f;
    [SerializeField, Range(0f, 10f)] private float moveMaxDuration = 5f;
    private float moveXTarget;
    private const float ARRIVAL_THRESHOLD = 0.05f;

    // Animation
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsIdleHash = Animator.StringToHash("IsIdle");
    private int currentAnimParamHash;
    private int pausedAnimHash;
    private float pausedAnimTime;

    // State management
    private delegate void CurrentState(StateMachine owner);
    private CurrentState currentState;

    private float stateDuration;
    private float stateTimer;

    public override void OnStateEnter(StateMachine owner)
    {
        ResetState();
    }

    public override State Run(StateMachine owner)
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= stateDuration)
        {
            stateTimer = 0;
            DecideNextState(owner);
        }
        else currentState?.Invoke(owner);

        return base.Run(owner);
    }

    public override void OnStateExit(StateMachine owner)
    {
        ResetState();
    }

    public override void OnStatePause(StateMachine owner)
    {
        AnimatorStateInfo currentAnim = owner.Animator.GetCurrentAnimatorStateInfo(0);
        pausedAnimHash = currentAnim.shortNameHash;
        pausedAnimTime = currentAnim.normalizedTime % 1.0f;
    }

    public override void OnStateResume(StateMachine owner)
    {
        if (pausedAnimHash != 0) 
            owner.Animator.Play(pausedAnimHash, 0, pausedAnimTime);
    }

    private void DecideNextState(StateMachine owner)
    {
        if (owner.Animator && currentAnimParamHash != 0)
            owner.Animator.SetBool(currentAnimParamHash, false);

        switch (Random.Range(0, 2))
        {
            // IDLE
            case 0:
                {
                    stateDuration = Random.Range(idleMinDuration, idleMaxDuration);
                    owner.Animator.SetBool(IsIdleHash, true);
                    currentAnimParamHash = IsIdleHash;
                    currentState = null;
                    break;
                }
            // MOVE
            case 1:
                {
                    stateDuration = Random.Range(moveMinDuration, moveMaxDuration);
                    moveXTarget = Random.Range(-20f, 20f);
                    owner.Animator.SetBool(IsMovingHash, true);
                    currentAnimParamHash = IsMovingHash;
                    currentState = Move;
                    break;
                }
        }
    }

    private void Move(StateMachine owner)
    {
        float currentX = owner.transform.position.x;
        float newX = Mathf.MoveTowards(currentX, moveXTarget, movementSpeed * Time.deltaTime);
        owner.transform.position = new Vector3(newX, owner.transform.position.y, owner.transform.position.z);

        // Has arrived
        if (Mathf.Abs(owner.transform.position.x - moveXTarget) <= ARRIVAL_THRESHOLD)
            stateTimer = stateDuration;
    }

    private void ResetState()
    {
        stateDuration = 0;
        stateTimer = 1;
        currentAnimParamHash = 0;
        pausedAnimHash = 0;
        pausedAnimTime = 0;
        currentState = null;
    }
}
