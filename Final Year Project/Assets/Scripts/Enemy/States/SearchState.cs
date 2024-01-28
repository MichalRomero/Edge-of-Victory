using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
   public override void Enter()
   {
        enemy.Agent.SetDestination(enemy.LastKnownPos);
   }

    public override void Perform()
    {
        // If the enemy sees the player it will turn back into its attack state.
        if (enemy.PlayerVisable())
        {
            stateMachine.ChangeState(new AttackState());
        }

        // Checks if arraived at players last known postition.
        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            // After 6 seconds and player not found, enemy will get back to patrol state.
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (searchTimer > 10)
            {
                stateMachine.ChangeState(new PatrolState());
            }
            if (moveTimer > Random.Range(2, 4))
            {
                // Move enemy to random location & reset timer
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 7));
                moveTimer = 0;
            }
        }
    }

    public override void Exit()
    {

    }
}
