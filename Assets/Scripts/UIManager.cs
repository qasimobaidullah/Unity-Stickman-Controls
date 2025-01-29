using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject mainUI;
    public GameObject gameplayUI;
    public GameObject pauseUI;
    public GameObject pauseButton;
    public GameObject gameOverUI;
    private AudioSource buttonClick;

    private PlayerMovement playerMovement1;
    private PlayerMovement playerMovement2;
    private SpawnObstacles spawnObstacles;

    void Start()
    {
        playerMovement1 = GameObject.Find("Player1").GetComponent<PlayerMovement>();  // Player 1
        playerMovement2 = GameObject.Find("Player2").GetComponent<PlayerMovement>();  // Player 2
        spawnObstacles = GameObject.Find("Canvas").GetComponent<SpawnObstacles>();
        buttonClick = GameObject.Find("buttonClick").GetComponent<AudioSource>();
        mainUI.transform.Find("Score").GetComponent<Text>().text = "Score: " + PlayerPrefs.GetInt("bestScore", 0);
    }


private void Update() {
    if(Input.GetKeyDown(KeyCode.Space)){
        play();
    }
}

    public void play()
    {
        playerMovement1.enabled = true;
        playerMovement2.enabled = true;
        spawnObstacles.enabled = true;

        playerMovement1.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("isRun", true);
        playerMovement1.gameObject.transform.GetChild(1).transform.GetComponent<Animator>().SetBool("isRun", true);

        playerMovement2.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("isRun", true);
        playerMovement2.gameObject.transform.GetChild(1).transform.GetComponent<Animator>().SetBool("isRun", true);


        mainUI.SetActive(false);
        gameplayUI.SetActive(true);
        pauseButton.SetActive(true);
        buttonClick.Play();
    }



    public void pause()
    {
        playerMovement1.enabled = false;
        playerMovement2.enabled = false;
        playerMovement1.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("isRun", false);
        playerMovement1.gameObject.transform.GetChild(1).transform.GetComponent<Animator>().SetBool("isRun", false);

        playerMovement2.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("isRun", false);
        playerMovement2.gameObject.transform.GetChild(1).transform.GetComponent<Animator>().SetBool("isRun", false);

        Time.timeScale = 0;
        pauseUI.SetActive(true);
        pauseButton.SetActive(false);
        buttonClick.Play();
    }

    public void resume()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        pauseButton.SetActive(true);
        playerMovement1.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("isRun", true);
        playerMovement1.gameObject.transform.GetChild(1).transform.GetComponent<Animator>().SetBool("isRun", true);

        playerMovement2.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("isRun", true);
        playerMovement2.gameObject.transform.GetChild(1).transform.GetComponent<Animator>().SetBool("isRun", true);


        playerMovement1.enabled = true;
        playerMovement2.enabled = true;
        buttonClick.Play();
    }

    public void gameOver()
    {

        playerMovement1.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("isRun", false);
        playerMovement1.gameObject.transform.GetChild(1).transform.GetComponent<Animator>().SetBool("isRun", false);

        playerMovement2.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("isRun", false);
        playerMovement2.gameObject.transform.GetChild(1).transform.GetComponent<Animator>().SetBool("isRun", false);

        GameObject.Find("gameOver").GetComponent<AudioSource>().Play();
        if (playerMovement1.score > PlayerPrefs.GetInt("bestScore", 0))
        {
            PlayerPrefs.SetInt("Score", playerMovement1.score);
        }
        if (playerMovement2.score > PlayerPrefs.GetInt("bestScore", 0))
        {
            PlayerPrefs.SetInt("Score", playerMovement2.score);
        }
        gameOverUI.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void restart()
    {
        Time.timeScale = 1; // Resume time flow

        // Destroy all obstacles in the scene
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        for (int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i]);
        }

        // Reset the position of Player 1 and Player 2 based on their respective lines
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        // Set Player 1 at the top line
        player1.transform.position = new Vector2(-4, playerMovement1.lineTop.transform.position.y + (playerMovement1.playerHeight / 2));

        // Set Player 2 at the bottom line
        player2.transform.position = new Vector2(-4, playerMovement1.lineBottom.transform.position.y - (playerMovement2.playerHeight / 2));

        // Reset player movement and scores
        playerMovement1.transform.GetChild(0).transform.GetComponent<SpriteRenderer>().enabled = false;
        playerMovement1.transform.GetChild(1).transform.GetComponent<SpriteRenderer>().enabled = false;

        playerMovement2.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        playerMovement2.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;

        playerMovement1.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        playerMovement2.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;

        playerMovement1.score = 0;
        playerMovement2.score = 0;
        playerMovement1.positionX = -4; // Reset X position of Player 1
        playerMovement2.positionX = -4; // Reset X position of Player 2
        playerMovement1.positionY = playerMovement1.lineTop.transform.position.y + (playerMovement1.playerHeight / 2); // Reset Y position of Player 1
        playerMovement2.positionY = playerMovement1.lineBottom.transform.position.y - (playerMovement2.playerHeight / 2); // Reset Y position of Player 2
        //playerMovement1.speedUp = 0.01f; // Reset speed of Player 1
        //playerMovement2.speedUp = 0.01f; // Reset speed of Player 2
        playerMovement1.timer = 0; // Reset Player 1's timer
        playerMovement2.timer = 0; // Reset Player 2's timer

        // Reset the score UI
        GameObject.Find("PScore").GetComponent<Text>().text = "score: 0";  // Reset Player 1 score UI

        // Reset obstacles
        GameObject.Find("Canvas").GetComponent<SpawnObstacles>().deletedObstacleLevel = 0;
        GameObject.Find("Canvas").GetComponent<SpawnObstacles>().obstacleLevel = 1;
        GameObject.Find("Canvas").GetComponent<SpawnObstacles>().lastObstaclePosition = 20.48f;

        // Disable gameplay and reset menus
        playerMovement1.enabled = false;  // Disable Player 1 movement
        playerMovement2.enabled = false;  // Disable Player 2 movement
        spawnObstacles.enabled = false;  // Disable obstacle spawning
        mainUI.SetActive(true);  // Show main UI
        gameplayUI.SetActive(false);  // Hide gameplay UI
        mainUI.transform.Find("Score").GetComponent<Text>().text = "Score: " + PlayerPrefs.GetInt("bestScore", 0); // Update best score text
        gameOverUI.SetActive(false);  // Hide game over UI
        buttonClick.Play();  // Play button click sound
    }
}
