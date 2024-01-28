using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }



    private PlayerHP playerHP;

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
        // Checks if the enemy can see the player
        if (enemy.PlayerVisable() && !enemy.playerInAttackRange())
        {
            enemy.Agent.SetDestination(enemy.player1.position);
            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else if (enemy.PlayerVisable() && enemy.playerInAttackRange())
        {
            enemy.Agent.SetDestination(enemy.transform.position);
            enemy.transform.LookAt(enemy.player1);

            playerHP.TakeDamage(1f);
        }
        else
        {
            // Changes to search state
            stateMachine.ChangeState(new SearchState());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    
}
