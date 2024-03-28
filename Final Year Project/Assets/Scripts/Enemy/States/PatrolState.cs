using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    //Used to track which waypoint currently targeting.
    public int waypointIndex;
    public float waitTimer;

    private Animator animator; // Reference to the Animator component
    public const string EnemyWalk = "EnemyWalk"; // Define your walking animation state name
    public const string EnemyIdle = "EnemyIdle"; // Define your idle animation state name
    private string currentAnimationState;
    public override void Enter()
    {
    }

    public PatrolState()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("AiBody"); 
        if (enemy != null)
        {
            animator = enemy.GetComponent<Animator>();
        }
    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.PlayerVisable())
        {
            ResetAnimationState();
            stateMachine.ChangeState(new AttackState());
        }
    }
    public override void Exit()
    {
    }

    public void PatrolCycle()
    {
        // Check if the enemy has reached its current waypoint
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            // Increment the timer to track waiting time
            waitTimer += Time.deltaTime;

            // If the wait time exceeds 1 second, move to the next waypoint and reset the timer
            if (waitTimer > 1)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;

                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0; // Reset the wait timer
                ChangeAnimationState(EnemyWalk); // Change to walking animation
            }
            else
            {
                ChangeAnimationState(EnemyIdle); // Change to idle animation when waiting
            }
        }
        else
        {
            ChangeAnimationState(EnemyWalk); // Change to walking animation when moving
        }
    }

    public void ChangeAnimationState(string newState)
    {
        Debug.Log("Changing animation state to: " + newState);
        if (currentAnimationState == newState) return;

        animator.CrossFadeInFixedTime(newState, 0.2f);
        currentAnimationState = newState;
    }

    private void ResetAnimationState()
    {
        currentAnimationState = ""; // Reset to an empty or default state
    }

}
