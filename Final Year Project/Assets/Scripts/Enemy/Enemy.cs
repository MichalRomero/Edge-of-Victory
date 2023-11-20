using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentHealth);
    }

    int currentHealth;
    public int maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        
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
        // TEMPORARY: Destroy Object
        Destroy(gameObject);
    }
}
