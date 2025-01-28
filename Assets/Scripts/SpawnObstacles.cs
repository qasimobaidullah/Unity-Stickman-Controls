using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public GameObject player1; // Reference to Player 1
    public GameObject player2; // Reference to Player 2
    private List<GameObject> obstacles = new List<GameObject>(); // List to track active obstacles
    public float obstacleLevel = 1; // Obstacle level, increases as the game progresses
    public float lastObstaclePosition = 20.48f; // Position to spawn the next obstacle
    public int deletedObstacleLevel = 0; // Used to delete old obstacles to avoid memory overload

    // Line positions for Player 1 and Player 2 (adjust the Y values as per their lines)
    public Transform lineTop;   // Top line (Player 1)
    public Transform lineBottom; // Bottom line (Player 2)

    // Private selected obstacle indexes for each lane (Player 1 and Player 2)
    private int selectedObstacleIndex1 = 1; // Default obstacle index for Player 1
    private int selectedObstacleIndex2 = 1; // Default obstacle index for Player 2

    void Update()
    {
        // Check if either Player 1 or Player 2 has moved far enough to spawn a new obstacle
        if (player1.transform.position.x + 20.48f >= lastObstaclePosition || player2.transform.position.x + 20.48f >= lastObstaclePosition)
        {
            // Randomly select an obstacle for Player 1 and Player 2
            selectedObstacleIndex1 = UnityEngine.Random.Range(1, 11); // Randomly select an obstacle (from 1 to 10)
            selectedObstacleIndex2 = UnityEngine.Random.Range(1, 11); // Randomly select an obstacle (from 1 to 10)

            // Spawn obstacle for Player 1 (top line)
            GameObject obstacle1 = Instantiate(Resources.Load("obstacle" + selectedObstacleIndex1), 
                new Vector2(lastObstaclePosition, lineTop.position.y), Quaternion.identity) as GameObject;

            // Adjust the obstacle's components (if needed)
            AlignObstacle(obstacle1, lineTop.position.y);

            // Spawn obstacle for Player 2 (bottom line)
            GameObject obstacle2 = Instantiate(Resources.Load("obstacle" + selectedObstacleIndex2), 
                new Vector2(lastObstaclePosition, lineBottom.position.y), Quaternion.identity) as GameObject;

            // Adjust the obstacle's components (if needed)
            AlignObstacle(obstacle2, lineBottom.position.y);

            // Add the obstacles to the list
            obstacles.Add(obstacle1);
            obstacles.Add(obstacle2);

            // Increase difficulty level and adjust spawn position
            obstacleLevel++;
            lastObstaclePosition = obstacleLevel * 20.48f;

            // Clean up old obstacles to avoid memory overload
            if (obstacles.Count > 4) // Keeping only the latest obstacles for each line
            {
                Destroy(obstacles[deletedObstacleLevel]);
                deletedObstacleLevel++;
            }
        }
    }

    // Method to align the obstacle to the correct Y position (adjust position if necessary)
    private void AlignObstacle(GameObject obstacle, float lineYPosition)
    {
        // Adjust the Y position based on the line, to make sure obstacles align properly
        // Here you can add any additional adjustments based on obstacle type or desired behavior
        obstacle.transform.position = new Vector2(obstacle.transform.position.x, lineYPosition);
    }
}
