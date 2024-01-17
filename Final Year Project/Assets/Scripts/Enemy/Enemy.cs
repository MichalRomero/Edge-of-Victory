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
    }

    int currentHealth;
    public int maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
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

        if (currentHealth <= 0)
        { Death(); }
    }

    void Death()
    {
        // Death function
        Destroy(gameObject);
    }
}
