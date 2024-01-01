using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    // This is slightly different to the approach you took for the GameEnding Trigger.
        // This script will check for the player character’s Transform instead of its GameObject.
        // It will make it easier to access JohnLemon’s position and determine whether there is a clear line of sight to him. 
    // Transform >> Position, rotation and scale of an object.
    public Transform player; // This will be the Observer class to detect the player's character.
    // Use OnTriggerEnter special method to detect the player's character & store whether the character was within the Trigger using a bool variable.
    bool m_IsPlayerInRange;
    public GameEnding gameEnding; // To end the game, we need a reference to the GameEnding class.

    private void OnTriggerEnter(Collider other)
    {
        // Check JohnLemon is in range whenever OnTriggerEnter is called.
        if(other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }

    // A player character enting this Trigger may not automatically mean the end of the game -
    // for example, there may be a wall in the way. So it's important to detect when JohnLemon leaves the Trigger.
    private void OnTriggerExit(Collider other)
    {
        if(other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    // Check the enemy's line of sight to JohnLemon is clear. Otherwise, end of the game could be triggered when there is a wall in the way.
    // Since the player's character's position could change at any moment, this needs to be checked in every frame.
    private void Update()
    {
        // It only makes sense to check the line of sight when the player character is actually in range.
        if (m_IsPlayerInRange)
        {
            // To check whether there are any Colliders along the path of a line starting from a point.
                // This line starting from a specific point is called a Ray.  Checking for Colliders along this Ray is called a Raycast. 
            // From vector math, the direction from the PointOfView GameObject to JohnLemon is JohnLemon's position minus the PointOfView GameObject's position.
                // To make sure the Observer can see JohnLemon's center of mass, point the direction up 1 unit using Vector3.up.
                // Vector3.up is a shortcut for (0, 1, 0).
            Vector3 direction = player.position - transform.position + Vector3.up;

            // Ray Construction >> Ray(Vector3 origin, Vector3 direction);
            // Ray creates a ray starting at origin along direction.
            Ray ray = new Ray(transform.position, direction);

            RaycastHit raycastHit; // This line defines the RaycastHit variable. 

            // With the Ray created, we can perform the Raycast -
            // purpose is to define the Ray along which the Raycast happens & restrictions on what sort of Colliders they want to detect.
            if (Physics.Raycast(ray, out raycastHit))   // Will return a bool, which is true when it hits something & false when it hasn't hit anything.
                                                        // The out parameter is Raycast method that sets its data to info about whatever the Raycast hit.
            {
                // The script can now identify the player's char is in range, perform a Raycast & know whether anything has been hit.
                if(raycastHit.collider.transform == player) // Check whether it was the Player that was hit.
                {
                    gameEnding.CaughtPlayer();
                }
            }


        }
    }

}
