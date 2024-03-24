using UnityEngine;

public class WayPointSpeedUp : MonoBehaviour
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

            if (currentWayPointIndex == 1)
            {
                this.speed = this.speed + 7;
            }
            else if (currentWayPointIndex == waypoints.Length)
            {
                this.speed = this.speed - 7;
            }


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
