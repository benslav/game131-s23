using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringPathFollowing : MonoBehaviour, ISteering
{

    public SteeringCore agent;

    public SteeringPath path;

    private SteeringSeek mySeek;
    private SteeringArrive myArrive;

    public enum MovementDriver {
        Seek,
        Arrive
    }
    public MovementDriver movementDriver = MovementDriver.Arrive;


    public bool showDebugGizmos = false;

    public float distanceOffset = 2.0f;

    // Start is called before the first frame update
    void Awake()
    {
        if (agent == null) agent = GetComponent<SteeringCore>();
        if (agent == null)
        {
            this.enabled = false;
            Debug.LogError("Object " + transform.name + " has a SteeringPursue behavior, but no SteeringCore assigned.");
        }

        mySeek = GetComponent<SteeringSeek>();
        myArrive = GetComponent<SteeringArrive>();
        if (mySeek == null && myArrive == null)
        {
            this.enabled = false;
            Debug.LogError("Object " + transform.name + " has a SteeringPursue behavior, but no SteeringSeek or SteeringArrive behavior. SteeringPursue depends on SteeringSeek or SteeringArrive.");
        }

        if (mySeek == null && myArrive != null) movementDriver = MovementDriver.Arrive;
        if (myArrive == null && mySeek != null) movementDriver = MovementDriver.Seek;
    }


    void Update()
    {
        // Get me the closest point on the path
        Vector3 closestPoint = path.GetClosestPathLocation(agent.transform.position, distanceOffset);

        switch (movementDriver) {
            case MovementDriver.Arrive:
                myArrive.targetTransform = null;
                myArrive.targetPosition = closestPoint;
                agent.SetLinearAcceleration(myArrive.GetSteering().linearAcceleration);
                break;
            case MovementDriver.Seek:
                agent.SetLinearAcceleration(mySeek.GetSteering(closestPoint).linearAcceleration);
                break;
        }

    }

    public SteeringOutput GetSteering()
    {
        // Get me the closest point on the path
        Vector3 closestPoint = path.GetClosestPathLocation(agent.transform.position, distanceOffset);

        switch (movementDriver) {
            case MovementDriver.Arrive:
                myArrive.targetTransform = null;
                myArrive.targetPosition = closestPoint;
                return myArrive.GetSteering();
            case MovementDriver.Seek:
                mySeek.targetTransform = null;
                mySeek.targetPosition = closestPoint;
                return mySeek.GetSteering();
        }
        
        return null;
    }

}
