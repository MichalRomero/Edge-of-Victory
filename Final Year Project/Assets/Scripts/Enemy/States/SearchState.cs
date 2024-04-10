using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;

    private Animator animator; // Reference to the Animator component
    public const string EnemyWalk = "EnemyWalk"; // Define your walking animation state name
    public const string EnemyIdle = "EnemyIdle"; // Define your idle animation state name
    private string currentAnimationState;

    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        ChangeAnimationState(EnemyWalk);

        // Access the Animator component from the enemy object
        animator = enemy.GetComponent<Animator>();
    }

    public override void Perform()
    {
        // If the enemy sees the player it will turn back into its attack state.
        if (enemy.PlayerVisable())
        {
            ResetAnimationState();
            stateMachine.ChangeState(new AttackState());
        }

        // Checks if arrived at players last known position.
        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            ChangeAnimationState(EnemyIdle);
            // After 10 seconds and player not found, enemy will get back to patrol state.
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (searchTimer > 10)
            {
                ResetAnimationState();
                stateMachine.ChangeState(new PatrolState());
            }
            if (moveTimer > Random.Range(2, 4))
            {
                // Move enemy to random location & reset timer
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 7));
                moveTimer = 0;
                ChangeAnimationState(EnemyWalk);
            }
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (animator == null)
        {
            //Debug.LogError("Animator not found on the enemy.");
            return;
        }

        Debug.Log("Changing animation state to: " + newState);
        if (currentAnimationState == newState) return;

        animator.CrossFadeInFixedTime(newState, 0.2f);
        currentAnimationState = newState;
    }


    private void ResetAnimationState()
    {
        currentAnimationState = ""; // Reset to an empty or default state
    }

    public override void Exit()
    {
        // Any cleanup when the state exits
    }
}
