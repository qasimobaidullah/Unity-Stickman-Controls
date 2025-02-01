using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;             // Rigidbody for the player
    public GameObject scoreUI;         // Score UI for the player (shared)
    private Text scoreText;            // Text component of the Score UI
    public GameObject lineTop;         // Reference to the top line sprite (for Player 1)
    public GameObject lineBottom;      // Reference to the bottom line sprite (for Player 2)

    // Position variables
    public float positionX = -6;       // Starting X position
    public float positionY = 2.5f;     // Default Y position for Player 1
    // Speed and gameplay variables
    public float speedUp = 0.01f;      // Speed increment
    private AudioSource changeLinesSound, throwSounds; // Sound effect when switching lines
    public float timer = 0;            // Timer for scoring
    public int score = 0;              // Shared score for both players

    // Control and identifier
    public string identifier;          // To differentiate Player 1 and Player 2
    public float playerHeight = 1f;    // The height of the player (adjust as needed)
    public Transform throwPoint; // Assign the position from where to throw
    public float throwForce = 5f;
    void Start()
    {
        changeLinesSound = GameObject.Find("changeLines").GetComponent<AudioSource>();
        throwSounds = GameObject.Find("throw").GetComponent<AudioSource>();

        scoreText = scoreUI.GetComponent<Text>();

        // Set initial player positions based on screen division
        if (identifier == "Player1")
        {
            positionY = 2.60f; // Player 1 starts at 2.5 Y position (top lane)
            transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (identifier == "Player2")
        {
            positionY = -2.60f; // Player 2 starts at -2.5 Y position (bottom lane)
            transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        speedUp += 0.00001f;
        positionX += (0.05f + speedUp);

        // Align player position with the lanes based on the current player
        if (identifier == "Player1")
        {
            // Ensure Player 1 stays within its lane (1.5 and 2.5 Y positions)
            if (positionY > 2.60f)
            {
                positionY = 2.60f;
            }
            else if (positionY < 1.45f)
            {
                positionY = 1.44f;
            }
        }
        else if (identifier == "Player2")
        {
            // Ensure Player 2 stays within its lane (-2.5 and -1.5 Y positions)
            if (positionY < -2.60f)
            {
                positionY = -2.60f;
            }
            else if (positionY > -1.44f)
            {
                positionY = -1.44f;
            }
        }

        rb.position = new Vector2(positionX, positionY);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            score += 10; // Increment score for every second of gameplay
            scoreText.text = "Score: " + score; // Update shared score UI
        }

        // Input logic for Player 1 and Player 2 to switch lanes
        if (identifier == "Player1")
        {
            if (Input.GetKeyDown(KeyCode.Q))  // Move Player 1 Up
            {
                if (positionY < 2.60f)
                {
                    positionY = 2.60f;
                    changeLinesSound.Play();

                    transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;

                }
            }
            else if (Input.GetKeyDown(KeyCode.A))  // Move Player 1 Down
            {
                if (positionY > 1.44f)
                {
                    positionY = 1.44f;
                    changeLinesSound.Play();
                    transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
                    transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) // Player 1 throws an obstacle
            {
                ThrowObstacle();
                transform.GetChild(0).transform.GetComponent<Animator>().SetTrigger("throw");
                transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("throw");
            }
        }
        else if (identifier == "Player2")
        {
            if (Input.GetKeyDown(KeyCode.P))  // Move Player 2 Up
            {
                if (positionY < -1.44f)
                {
                    positionY = -1.44f;
                    changeLinesSound.Play();
                    transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.L))  // Move Player 2 Down
            {

                if (positionY > -2.60f)
                {
                    positionY = -2.60f;
                    changeLinesSound.Play();
                    transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
                    transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.O)) // Player 2 throws an obstacle
            {
                ThrowObstacle();
                transform.GetChild(0).transform.GetComponent<Animator>().SetTrigger("throw");
                transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("throw");

            }
        }

    }
    void ThrowObstacle()
    {
        // Load the obstacle from Resources
        GameObject obstaclePrefab = Resources.Load<GameObject>("rock");

        if (obstaclePrefab != null)
        {
            // Instantiate the obstacle at the throw point
            GameObject obstacle = Instantiate(obstaclePrefab, throwPoint.position, Quaternion.identity);
            obstacle.tag = "SabotageObstacle";

            // Get Rigidbody2D component
            Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();

            // Find the opponent's position
            Vector2 targetPosition = Vector2.zero;

            if (identifier == "Player1")
            {
                GameObject player2 = GameObject.FindWithTag("Player2");
                if (player2 != null)
                    targetPosition = player2.transform.position;
            }
            else if (identifier == "Player2")
            {
                GameObject player1 = GameObject.FindWithTag("Player1");
                if (player1 != null)
                    targetPosition = player1.transform.position;
            }

            if (rb != null && targetPosition != Vector2.zero)
            {
                // Calculate direction towards the target
                Vector2 direction = (targetPosition - (Vector2)throwPoint.position).normalized;

                // Apply velocity towards the target
                rb.velocity = direction * throwForce;
            }
            // Play the throw sound
            throwSounds.Play();
            // Destroy the obstacle after 3 seconds (adjust if needed)
            Destroy(obstacle, 3f);
        }
        else
        {
            Debug.LogError("Obstacle prefab not found in Resources.");
        }
    }




    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("SabotageObstacle"))
        {
            StopAllCoroutines(); // Stop all running coroutines
            StartCoroutine(SlowDown(2f, 0.5f)); // Slow down for 2 seconds
            Destroy(col.gameObject); // Remove the obstacle after hitting the player
        }
        else
        {
            StopAllCoroutines(); // Ensure speed modifiers don’t persist
            GameObject.Find("Canvas").GetComponent<UIManager>().gameOver();



            if (this.identifier == "Player1")
            {
                transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
                transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;
                Instantiate(Resources.Load("explosion"), this.transform);
                StartCoroutine(DestroyExplosion());
            }
            else
            {
                transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
                transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;
                Instantiate(Resources.Load("explosion"), this.transform);
                StartCoroutine(DestroyExplosion());
            }
        }

    }
    public IEnumerator SlowDown(float duration, float factor)
    {
        float originalSpeed = speedUp;  // Store the original speed

        speedUp *= factor;  // Reduce speed
        yield return new WaitForSeconds(duration);

        speedUp = originalSpeed;  // Restore the exact original speed
    }

    IEnumerator DestroyExplosion()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        GameObject g = GameObject.FindGameObjectWithTag("explosion");
        Destroy(g);
        GameObject.Find("Canvas").GetComponent<UIManager>().restart();
    }

}
