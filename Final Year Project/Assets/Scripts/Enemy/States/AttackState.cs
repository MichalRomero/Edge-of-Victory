using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;


    private bool isAttackDelayed = false;
    private float delayAttackTimer = 0f;
    private const float delayAttackTime = 0.4f; // Delay time in seconds


    // Update is called once per frame
    void Update()
    {
        Perform();
    }
    public override void Enter()
    {
        attackTimer = 0; // Reset attack timer on entering the state
    }

    public override void Exit()
    {

    }



    private PlayerHP playerHP;

    private float attackCooldown = 1f; // Cooldown time for attacks
    private float attackTimer = 0;       // Timer to track attack cooldown

    public AttackState()
    {
        // Find the player GameObject and get the PlayerHP component
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Use the tag assigned to your player GameObject
        if (player != null)
        {
            playerHP = player.GetComponent<PlayerHP>();
        }
    }

    public override void Perform()
    {

        attackTimer += Time.deltaTime; // Update attack timer

        // Handle attack delay
        if (isAttackDelayed)
        {
            delayAttackTimer += Time.deltaTime;
            if (delayAttackTimer >= delayAttackTime)
            {
                DelayedAttack();
                isAttackDelayed = false;
            }
        }

        // Checks if the enemy can see the player but is not within attacking range
        if (enemy.PlayerVisable() && !enemy.playerInAttackRange())
        {
            enemy.Agent.SetDestination(enemy.player1.position);
            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        // Checks if the enemy can see the player and is within attacking range
        else if (enemy.PlayerVisable() && enemy.playerInAttackRange())
        {
            enemy.Agent.SetDestination(enemy.transform.position);
            enemy.transform.LookAt(enemy.player1);

            // Check if attack cooldown has elapsed
            if (attackTimer >= attackCooldown)
            {
                Attack();
                Debug.Log("Enemy ATTACKED");
                attackTimer = 0; // Reset attack timer
            }
        }
        else
        {
            // Changes to search state
            stateMachine.ChangeState(new SearchState());
        }
    }


    private void Attack()
    {
        isAttackDelayed = true;
        delayAttackTimer = 0f;
    }

    private void DelayedAttack()
    {
        // Check if the player is still in attack range
        if (enemy.PlayerVisable() && enemy.playerInAttackRange())
        {
            playerHP.TakeDamage(25f); // Apply damage
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    
}
