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
    private AudioSource changeLinesSound; // Sound effect when switching lines
    public float timer = 0;            // Timer for scoring
    public int score = 0;              // Shared score for both players

    // Control and identifier
    public string identifier;          // To differentiate Player 1 and Player 2
    public float playerHeight = 1f;    // The height of the player (adjust as needed)

    void Start()
    {
        changeLinesSound = GameObject.Find("changeLines").GetComponent<AudioSource>();
        scoreText = scoreUI.GetComponent<Text>();

        // Set initial player positions based on screen division
        if (identifier == "Player1")
        {
            positionY = 2.55f; // Player 1 starts at 2.5 Y position (top lane)
            transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (identifier == "Player2")
        {
            positionY = -2.55f; // Player 2 starts at -2.5 Y position (bottom lane)
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
            if (positionY > 2.55f)
            {
                positionY = 2.55f;
            }
            else if (positionY < 1.45f)
            {
                positionY = 1.45f;
            }
        }
        else if (identifier == "Player2")
        {
            // Ensure Player 2 stays within its lane (-2.5 and -1.5 Y positions)
            if (positionY < -2.55f)
            {
                positionY = -2.55f;
            }
            else if (positionY > -1.45f)
            {
                positionY = -1.45f;
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
                if (positionY < 2.55f)
                {
                    positionY = 2.55f;
                    changeLinesSound.Play();

                    transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;

                }
            }
            else if (Input.GetKeyDown(KeyCode.A))  // Move Player 1 Down
            {
                if (positionY > 1.45f)
                {
                    positionY = 1.45f;
                    changeLinesSound.Play();
                    transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
                    transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
        else if (identifier == "Player2")
        {
            if (Input.GetKeyDown(KeyCode.P))  // Move Player 2 Up
            {
                if (positionY < -1.45f)
                {
                    positionY = -1.45f;
                    changeLinesSound.Play();
                    transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.L))  // Move Player 2 Down
            {

                if (positionY > -2.55f)
                {
                    positionY = -2.55f;
                    changeLinesSound.Play();
                    transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
                    transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject.Find("Canvas").GetComponent<UIManager>().gameOver();
    }
}
