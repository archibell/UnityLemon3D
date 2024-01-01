using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // This will give me access to the NavMeshAgent class.

public class WaypointPatrol : MonoBehaviour
{
    // This public ref to the component will allow assigning the Nav Mesh Agent ref in the Inspector window.
    // NavMeshAgent >> This component is attached to a mobile character in the game to allow it to navigate the Scene using the NavMesh.
    public NavMeshAgent navMeshAgent; 
    // Declare a public variable `waypoints`, which is an array of Transforms.
    public Transform[] waypoints;
    int m_CurrentWaypointIndex; // In order to track which waypoint is the next, we start by storing the current index of the waypoint array.

    // Start is called before the first frame update
    void Start()
    {
        // NavMeshAgent.SetDestination >> Sets or updates the destination thus triggering the calculation for a new path.
        // Declaration >> public bool SetDestination(Vector3 target).
        // Transform.position >> is a Vector3 world space position of the Transform.
        navMeshAgent.SetDestination(waypoints[0].position);

    }

    // Update is called once per frame
    void Update()
    {
        // Perform a check - if the Nav Mesh Agent has arrived at its destination.
        // See whether the remaining distance to the destination is less than the stopping distance we set in the Inspector window earlier.
        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            // NavMeshAgent.remainingDistance >> The distance between the agent's position and the destination on the current path.
            // NavMeshAgent.stoppingDistance >> The radius within which the agent should stop from the target position.
        {
            // Update the current index using the remainder operator %.
            // The code is saying, add one to the current index,
            // but if that increment puts the index equal to the number of elements in the waypoints array then instead set it to zero.
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            // Update the Nav Mesh Agent's destination w/ the updated CurrentWaypointIndex.
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
}
