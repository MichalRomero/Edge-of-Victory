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
            // Is the player close enough to be soon?
            if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fov && angleToPlayer <= fov)
                {
                    Ray ray = new Ray(transform.position, targetDirection);
                    Debug.DrawRay(ray.origin, ray.direction * viewDistance);
                }
            }
        }
        return true;
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
