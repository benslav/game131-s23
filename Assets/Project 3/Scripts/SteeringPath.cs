using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SteeringPath : MonoBehaviour
{
    private List<Vector3> waypointLocations = new List<Vector3>();

    private List<float> stretchDistances = new List<float>();

    public bool connectEndToStart = true;

    public bool isForwardAligned = true;


    public List<Vector3> Waypoints { get { return waypointLocations; } }

    public void SetWaypoint(int index, Vector3 location)
    {
        transform.GetChild(index).position = location;
        RefreshPath();
    }

    
    // Start is called before the first frame update
    void Start()
    {
        RefreshPath();
    }

    void RefreshPath()
    {
        // Find my children in order and make a path of lines
        waypointLocations.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            waypointLocations.Add(transform.GetChild(i).position);
        }

        float runningDistance = 0;
        for (int i = 0; i < waypointLocations.Count - 1; i++)
        {
            float stretchDistance = (waypointLocations[i + 1] - waypointLocations[i]).magnitude;
            stretchDistances.Add(runningDistance + stretchDistance);
            runningDistance += stretchDistance;
        }

        if (connectEndToStart)
        {
            float stretchDistance = (waypointLocations[0] - waypointLocations[waypointLocations.Count - 1]).magnitude;
            stretchDistances.Add(runningDistance + stretchDistance);
        }

    }

    // Update is called once per frame
    void Update()
    {
        RefreshPath();
    }

    private void OnDrawGizmos()
    {
        Color c = Gizmos.color;
        Gizmos.color = Color.cyan;

        List<Vector3> locations = new List<Vector3>(waypointLocations);
        if (connectEndToStart) locations.Add(waypointLocations[0]);

        for (int i = 0; i < locations.Count - 1; i++)
        {
            Gizmos.DrawLine(locations[i], locations[i + 1]);

            // Draw an "arrow"
            Vector3 centerPoint = (locations[i] + locations[i + 1]) / 2;

            // Aligned vector
            Vector3 onSeg = (locations[i + 1] - locations[i]);
            // Find a coplanar normal
            Vector3 cpn = Quaternion.AngleAxis(90, Vector3.up) * onSeg;
            cpn.Normalize();
            cpn *= 0.6f;

            // onSeg points forward
            if (!isForwardAligned) onSeg *= -1;
            onSeg.Normalize();

            Gizmos.DrawLine(centerPoint, centerPoint - onSeg + cpn);
            Gizmos.DrawLine(centerPoint, centerPoint - onSeg - cpn);

        }

        // Draw spheres at each waypoint
        for (int i = 0; i < waypointLocations.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(waypointLocations[i], 0.5f);
        }
    }

    public Vector3 GetClosestPathLocation(Vector3 outsider, float distanceOffset)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (waypointLocations.Count == 0) return outsider;

        Vector3 candidate = waypointLocations[0];
        float totalDistanceFromStart = 0;

        for (int i = 0; i < waypointLocations.Count - 1; i++)
        {
            Vector3 start = waypointLocations[i];
            Vector3 end = waypointLocations[(i + 1) % waypointLocations.Count];

            Vector3 nc = ClosestPointOnSegment(start, end, outsider);
            if ((candidate - outsider).sqrMagnitude > (nc - outsider).sqrMagnitude)
            {
                // nc is closer
                candidate = nc;

                if (i > 0)
                {
                    totalDistanceFromStart = stretchDistances[i - 1] + (candidate - start).magnitude;
                }
                else
                {
                    totalDistanceFromStart = (candidate - start).magnitude;
                }
            }
        }

        float targetDistance = totalDistanceFromStart + distanceOffset;
        float totalLength = stretchDistances[stretchDistances.Count - 1];
        targetDistance = Mathf.Clamp(targetDistance, 0, totalLength);

        if (targetDistance == 0) return waypointLocations[0];
        if (targetDistance == totalLength) return waypointLocations[waypointLocations.Count - 1];

        // Start from stretchDistances[0] and find the first time stretchDistances[i] is larger than targetDistance
        int stretchIndex = 0;
        while (stretchIndex < stretchDistances.Count && stretchDistances[stretchIndex] < targetDistance)
            stretchIndex++;

        float previousLength = 0;
        if (stretchIndex > 0) previousLength = stretchDistances[stretchIndex - 1];

        float remainingLength = targetDistance - previousLength;

        return waypointLocations[stretchIndex] + (waypointLocations[stretchIndex + 1] - waypointLocations[stretchIndex]).normalized * remainingLength;

    }

    public Vector3 ClosestPointOnSegment(Vector3 a, Vector3 b, Vector3 o)
    {
        // parameterized dot product = 0

        Vector3 p = b - a, c = o;
        float d = (p.x * p.x + p.y * p.y + p.z * p.z);
        if (d == 0) return a;   // length 0
        float t = (p.z * c.z + p.y * c.y + p.x * c.x - p.x * a.x - p.y * a.y - p.z * a.z) / d;

        if (t < 0) return a;
        if (t > 1) return b;
        return a + (b - a) * t;
    }
}
