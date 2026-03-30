using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class PatrolPath : MonoBehaviour
{
    public List<Transform> waypoints;
    [SerializeField] private bool alwaysDrawPath; // Display path
    [SerializeField] private bool drawAsLoop;     // Display relationship of firstPointWay and Last
    [SerializeField] private bool drawNumbers;    // Display numbers of wayPoint
    public Color debugColour = Color.white;       // Set up color of path
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (alwaysDrawPath)
        {
            DrawPath();
        }
    }
    public void DrawPath()
    {
        // Broswe point by point
        for (int i = 0; i < waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColour;

            if (drawNumbers)
                Handles.Label(waypoints[i].position, i.ToString(), labelStyle); // Display point
            // Draw Lines Between Point
            if (i >= 1)
            {
                Gizmos.color = debugColour;
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);

                if (drawAsLoop)
                    Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
            }
        }
    }
    public void OwGizmosSelected()
    {
        if (alwaysDrawPath)
            return;
        else
            DrawPath();      
    }
#endif
}
