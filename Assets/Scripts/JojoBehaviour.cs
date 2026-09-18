using UnityEngine;
using UnityEngine.EventSystems;

public enum Action
{
    IDLE,
    MOVE
}

public class JojoBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float edges = 20f;

    [Header("Sounds")]
    [SerializeField] private AudioClip move;
    [SerializeField] private AudioClip idle;
    [SerializeField] private AudioClip[] barkClips;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsIdleHash = Animator.StringToHash("IsIdle");
    private static readonly int IsBarkingHash = Animator.StringToHash("IsBarking");
    private int currentHash;


    // Decision Making
    private Action lastAction;
    private Action currentAction;
    private float lastDecisionDuration;
    private float decisionTimer;

    private float targetX;
    private const float ARRIVAL_THRESHOLD = 0.05f;

    private void Start()
    {
        ChooseNewAction();
    }

    private void Update()
    {
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= lastDecisionDuration)
        {
            ChooseNewAction();
            return;
        }

        if (currentAction == Action.MOVE)
        {
            float currentX = transform.position.x;
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            if (Mathf.Abs(transform.position.x - targetX) <= ARRIVAL_THRESHOLD)
            {
                ChooseNewAction();
            }
        }
    }

    private void ChooseNewAction()
    {
        lastAction = currentAction;

        decisionTimer = 0f;
        lastDecisionDuration = Random.Range(1f, 5f);

        System.Array actions = System.Enum.GetValues(typeof(Action));
        currentAction = (Action)actions.GetValue(Random.Range(0, actions.Length));

        if (currentAction == Action.MOVE)
        {
            targetX = Random.Range(-edges, edges);
            audioSource.PlayOneShot(move);
            SetAnimationState(IsMovingHash, IsIdleHash);
        }
        else
        {
            audioSource.PlayOneShot(idle);
            SetAnimationState(IsIdleHash, IsMovingHash);
        }
    }

    private void SetAnimationState(int activeTrigger, int inactiveTrigger)
    {
        animator.ResetTrigger(inactiveTrigger);
        animator.SetTrigger(activeTrigger);
        currentHash = activeTrigger;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (barkClips != null && barkClips.Length > 0)
        {
            audioSource.PlayOneShot(barkClips[Random.Range(0, barkClips.Length)]);
        }

        lastAction = currentAction;
        currentAction = Action.IDLE;

        animator.ResetTrigger(IsMovingHash);
        animator.ResetTrigger(IsIdleHash);
        animator.SetTrigger(IsBarkingHash);
    }

    public void RestoreAnim()
    {
        currentAction = lastAction;
        animator.ResetTrigger(IsBarkingHash);
        animator.SetTrigger(currentHash);
    }
}