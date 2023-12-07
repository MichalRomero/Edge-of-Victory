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

    // Called when gizmos are drawn in the scence view.
    private void OnDrawGizmos()
    {
        // Checks if path should be drawn always or when the gameobject is selected.
        if (alwaysDrawPath || UnityEditor.Selection.activeGameObject == gameObject)
        {
            DrawPath();
        }
    }

    // Draws the path using Gizmos
    private void DrawPath()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColour;

            if (drawNumbers)
                UnityEditor.Handles.Label(waypoints[i].position, i.ToString(), labelStyle);

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

