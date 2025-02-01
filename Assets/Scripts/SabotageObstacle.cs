using UnityEngine;

public class SabotageObstacle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy obstacle when it hits a player
        }
    }
}
