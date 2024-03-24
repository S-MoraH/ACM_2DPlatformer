using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWayPointIndex = 0;
    [SerializeField] private float speed = 2f;

    private void Update()

    {

                            // current waypoint position            	         //platform position
        if (Vector2.Distance(waypoints[currentWayPointIndex].transform.position, transform.position) < .1f)
        {
            
            currentWayPointIndex++;

            //edge case 
            if (currentWayPointIndex >= waypoints.Length)
            {
                currentWayPointIndex = 0;
            }

        }
                                                                                                                        // slow down the speed of the game
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWayPointIndex].transform.position, Time.deltaTime * speed);
    }
}
