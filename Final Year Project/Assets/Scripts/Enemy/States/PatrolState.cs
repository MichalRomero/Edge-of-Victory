using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    //Used to track which waypoint currently targeting.
    public int waypointIndex;
    public float waitTimer; 
    public override void Enter()
    {
    }
    public override void Perform()
    {
        PatrolCycle();
        if (enemy.PlayerVisable())
        {
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
            }
        }
    }

}
