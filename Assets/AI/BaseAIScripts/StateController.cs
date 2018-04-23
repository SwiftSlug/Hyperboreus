using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
//using Complete;

public class StateController : NetworkBehaviour {

    public State currentState;
    public State remainState;

    [Tooltip("The default range in which the AI can detect other objects")]
    public float detectionRange = 500.0f;

    [Tooltip("The default walking speed of the AI unit")]
    public float walkSpeed = 3.5f;

    [Tooltip("The default running speed of the AI unit")]
    public float runSpeed = 6.0f;

    [Tooltip("The amount of time an AI will spend in the searching state (Seconds)")]
    public float searchWaitTime = 5.0f;

    [Tooltip("The default cooldown inbetween attacks (Seconds)")]
    public float attackCooldown = 5.0f;

    [Tooltip("The default cooldown inbetween jumps (Seconds)")]
    public float jumpCooldown = 5.0f;

    [Tooltip("The distance at which AI stop moving towards their target")]
    public float stopDistance = 1.0f;

    [Tooltip("The distance at which the AI can attack their target")]
    public float attackDistance = 1.5f;

    [Tooltip("The distance at which the AI can attack buildings")]
    public float buildingAttackDistance = 5.0f;

    [Tooltip("The amount of damage the AI unit does per hit")]
    public int attackDamage = 5;

    [Tooltip("The area size around the player the AI will search in")]
    public float wanderRange = 5.0f;

    [HideInInspector] public float lastJumped = 0.0f;   //  Used for timing cooldown between jumps
    [HideInInspector] public float lastAttack = 0.0f;   //  Used for timing cooldown between jumps

    public GameObject previousPlayerTarget;    //  The player the AI will target if able
    public GameObject target;         //  Generic gameobject target for AI
    //[HideInInspector] public Vector3 targetLocation;    //  Generic vector location used for AI

    [HideInInspector] public Vector3 moveCommandLocation;    //  Movement command vector used for AI

    [HideInInspector] public Animator animator;         //  Generic animator reference used for AI

    //[HideInInspector] public float searchTimeStart;     //  Reference to when AI starts searching

	[HideInInspector] public NavMeshAgent navMeshAgent;
	[HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;

    public bool aiActive;
    public bool underAttack = false;


	void Awake () 
	{
		navMeshAgent = GetComponent<NavMeshAgent> ();

        animator = GetComponentInChildren<Animator>();

        moveCommandLocation = Vector3.zero;

        aiActive = true;
	}

	public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
	{
		wayPointList = wayPointsFromTankManager;
		aiActive = aiActivationFromTankManager;
		if (aiActive) 
		{
			navMeshAgent.enabled = true;
		} else 
		{
			navMeshAgent.enabled = false;
		}
	}

    private void Update()
    {
        if (!aiActive)
        {
            return;
        }
        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    public void setTarget(GameObject newTarget)
    {

        //  Remember the previous target if it was a player

        if (target != null)
        {
            //  Target has been previously set

            if (target.GetComponent<PlayerStats>())
            {
                //  The current target is a player

                previousPlayerTarget = target;  //  Remember previous player target
            }
        }

        target = newTarget;


    }


}