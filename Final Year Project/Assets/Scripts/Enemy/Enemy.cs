using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }
    [SerializeField]
    private string currentState;
    public Path path;
    private GameObject player;
    public float viewDistance = 20f;
    public float fov = 85f;
    public float eyeHeight;

    int currentHealth;
    public int maxHealth;
    public GameObject Player { get => player; }
    private Vector3 lastKnownPos;
    // Having the getter allows access the private var of lastKnownPos, having the setter allows asigning new value to it.
    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    //Used in Attack state
    public Transform player1;

    //Used for attack state
    public float sightRange, attackRange;
    public bool pAttackInRange;
    public LayerMask whatIsGround, whatIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");

       

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Enemy hp: " + currentHealth);
        PlayerVisable();
        currentState = stateMachine.activeState.ToString(); // Allows to see what state the enemy is currently in
        
        //EXPERIMENTAL
        player1 = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public bool playerInAttackRange()
    {
        pAttackInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        return pAttackInRange;
    }

    // Check if player is within range and within FOV of enemy
    public bool PlayerVisable()
    {
        if (player != null)
        {
            // checking distance if player can be seen
            if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
            {
                // Calculating angle to player
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                
                // Check if angle is within FOV of enemy
                if (angleToPlayer >= -fov && angleToPlayer <= fov)
                {
                    // Using raycast to check if line of sight is blocked by object
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray,out hitInfo, viewDistance))
                    {
                        if(hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * viewDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Taking damage");

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(directionToPlayer, transform.forward);

        if (dotProduct < 0) // Player is behind
        {
            // Start coroutine to smoothly rotate towards the player
            StartCoroutine(RotateTowardsPlayer(directionToPlayer));
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    IEnumerator RotateTowardsPlayer(Vector3 directionToPlayer)
    {
        float duration = 0.2f; // Duration of the rotation, adjust as needed
        float time = 0;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(directionToPlayer);

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
            yield return null;
        }

        // Change to attack state after rotation is completed
        stateMachine.ChangeState(new AttackState());
    }

    void Death()
    {
        // Death function
        Destroy(gameObject);
    }





    
}
