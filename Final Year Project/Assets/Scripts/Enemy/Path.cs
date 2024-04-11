using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> waypoints;

    // Given options for the path.
    public bool alwaysDrawPath = true;
    public bool drawAsLoop;
    public bool drawNumbers;
    public Color debugColour = Color.white; // Draws a visual line.

    // Called when gizmos are drawn in the scene view.
    private void OnDrawGizmos()
    {
        if (alwaysDrawPath)
        {
            DrawPath();
        }
    }

    // Draws the path using Gizmos
    private void DrawPath()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (drawNumbers)
            {
                // Here you could implement an alternative way to display the numbers
                // For now, this feature is removed as it requires the Unity Editor.
            }

            // Draw Lines Between Points.
            if (i >= 1)
            {
                Gizmos.color = debugColour;
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);

                // Draws a line connecting the last waypoint to the first if it's a loop
                if (drawAsLoop && i == waypoints.Count - 1)
                    Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
            }
        }
    }
}
