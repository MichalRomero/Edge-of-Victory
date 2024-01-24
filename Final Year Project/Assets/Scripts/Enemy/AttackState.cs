using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;

   

    public void Awake()
    {
        

    }

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

    public override void Perform()
    {
        // Checks if the enmy can see the player
        if (enemy.PlayerVisable() && !enemy.playerInAttackRange())
        {
            enemy.Agent.SetDestination(enemy.player1.position);
        }
        else if (enemy.PlayerVisable() && enemy.playerInAttackRange())
        {
            enemy.Agent.SetDestination(enemy.transform.position);
            enemy.transform.LookAt(enemy.player1);
        }
        else
        {
            stateMachine.ChangeState(new PatrolState());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
