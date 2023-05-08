using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

[CustomEditor(typeof(SteeringPath))]
public class EditorScript : Editor
{
    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    bool isPointOnPlane = false;
    Vector3 mousePoint = Vector3.zero;

    private enum SelectionState
    {
        None,
        Waypoint,
        Segment,
        Arrow
    }
    int targetedWaypointIndex = -1;
    int hoveredWaypointIndex = -1;

    Vector3 originalMouseDownLocation = Vector3.zero;
    Vector3 originalWaypointLocation = Vector3.zero;

    MouseCursor mc = MouseCursor.Arrow;

    void OnSceneGUI()
    {
        SteeringPath path = target as SteeringPath;
        RaycastHit hitInfo;


        switch (Event.current.type)
        {
            case EventType.MouseMove:

                // Where am I pointing?
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitInfo, 1000))
                {
                    mousePoint = hitInfo.point;
                    isPointOnPlane = true;
                    SceneView.currentDrawingSceneView.Repaint();
                }
                else
                {
                    isPointOnPlane = false;
                }
                break;
            case EventType.MouseDown:
                // Are we near a waypoint? If so, "select" it.
                targetedWaypointIndex = hoveredWaypointIndex;
                originalMouseDownLocation = mousePoint;
                if (targetedWaypointIndex >= 0)
                {
                    Debug.Log("Gotcha! " + targetedWaypointIndex);
                    mc = MouseCursor.MoveArrow;

                    originalWaypointLocation = path.Waypoints[targetedWaypointIndex];

                    // Overcoming bad design
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);


                    Undo.RegisterCompleteObjectUndo(path.transform.GetChild(targetedWaypointIndex), "Starting to move waypoint");

                    Event.current.Use();
                }
                else
                {
                    float closestDistance = -1;
                    int closestSegmentStartIndex = -1;
                    Vector3 newClosestPosition = Vector3.zero;
                    for (int i = 0; i < path.Waypoints.Count; i++)
                    {
                        Vector3 newPosition = path.ClosestPointOnSegment(path.Waypoints[i], path.Waypoints[i + 1], mousePoint);
                        float magnitude = Vector3.Distance(newPosition, mousePoint);
                        if (magnitude < closestDistance || closestDistance < 0)
                        {
                            closestDistance = magnitude;
                            closestSegmentStartIndex = i;
                            newClosestPosition = newPosition;
                        }
                    }

                    Instantiate(path.transform.GetChild(0), newClosestPosition, Quaternion.Euler(0, 0, 0));
                }
                break;
            case EventType.MouseDrag:

                if (targetedWaypointIndex >= 0)
                {
                    if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitInfo, 1000))
                    {
                        mousePoint = hitInfo.point;
                        Vector3 diff = mousePoint - originalMouseDownLocation;
                        path.SetWaypoint(targetedWaypointIndex, originalWaypointLocation + diff);
                        SceneView.currentDrawingSceneView.Repaint();
                    }

                    Event.current.Use();
                }


                break;
            case EventType.MouseUp:
                targetedWaypointIndex = -1;
                mc = MouseCursor.Arrow;
                SceneView.currentDrawingSceneView.Repaint();
                break;
            case EventType.Repaint:

                if (isPointOnPlane)
                {
                    // What am I close to? Am I close to a waypoint?
                    // proximity = 0.5f
                    float proximity = 0.5f;
                    float proxSq = proximity * proximity;
                    hoveredWaypointIndex = -1;
                    for (int i = 0; i < path.Waypoints.Count; i++)
                    {
                        if ((path.Waypoints[i] - mousePoint).sqrMagnitude <= proxSq)
                        {
                            Handles.color = Color.blue;
                            Handles.DrawSolidDisc(path.Waypoints[i], SceneView.currentDrawingSceneView.camera.transform.forward, 0.4f);
                            hoveredWaypointIndex = i;
                        }
                    }
                }
                break;

        }
    }

}