using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // The static reference to the GameManager, ensuring it's a singleton
    public static GameManager instance;

    // Game state variables
    public int playerScore = 0; // Example of a game state you want to track
    public bool isGameOver = false; // Tracks if the game is over

    private bool isPaused = false;  // Track the current pause state
    private bool gameHasStarted = false;
    [SerializeField] private GameObject pauseMenuUI;  // Optional: UI to show when the game is paused
    [SerializeField] private GameObject startMenuUI;  // The UI GameObject for the start menu
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject heart1, heart2, heart3;
    public StylizedCountdown countdown;  // Reference to the countdown script
    public bool objecctsCanMove = false;

    // Called before Start, used for initialization
    void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        // Assign UI elements when the game starts
        objecctsCanMove = false;
        AssignUIElements();
        HandleSceneUI();
    }

    private void Update()
    {
     
        if (gameHasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseMenu();
                }
            }
        }
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels!");
        }
    }

    // Subscribe to scene loaded event
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribe from scene loaded event
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-assign UI elements when a new scene is loaded
        AssignUIElements();
        HandleSceneUI();
    }

    // Method to assign UI elements after a scene is loaded
    private void AssignUIElements()
    {
        // Re-find the UI elements in the current scene
        startMenuUI = GameObject.FindGameObjectWithTag("StartMenu");
        gameOverUI = GameObject.FindGameObjectWithTag("GameOverUI");
        heart1 = GameObject.Find("Heart1");
        heart2 = GameObject.Find("Heart2");
        heart3 = GameObject.Find("Heart3");

        
        // Optional: Add null checks to avoid potential null reference errors
        if (startMenuUI == null || gameOverUI == null || heart1 == null || heart2 == null || heart3 == null)
        {
            Debug.LogError("One or more UI elements are missing from the scene.");
        }
    }

    // Handle UI visibility based on the current scene
    private void HandleSceneUI()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        
        if (sceneIndex == 0)
        {
            // Show start menu in the first scene
            StartMenu();
        }
        else
        {
            // Hide start menu and show game elements in other scenes
            startMenuUI?.SetActive(false);
            gameHasStarted = true;
            heart1?.SetActive(true);
            heart2?.SetActive(true);
            heart3?.SetActive(true);
        }
    }

    public void StartGame()
    {
        
        if (!gameHasStarted)
        {
            gameHasStarted = true;
            startMenuUI.SetActive(false);
            gameOverUI.SetActive(false);
            countdown.StartCountdown();  // Start countdown
            Time.timeScale = 1.0f;


        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverUI?.SetActive(false);
    }

    private void PauseMenu()
    {
        Time.timeScale = 0f;  // Freeze the game
        isPaused = true;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);  // Show the pause menu UI if it exists
        }

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;  // Resume the game
        isPaused = false;

        if (pauseMenuUI != null && !startMenuUI.activeSelf)
        {
            pauseMenuUI.SetActive(false);  // Hide the pause menu UI if it exists
        }
        else if (startMenuUI.activeSelf)
        {
            startMenuUI.SetActive(false);
        }

        Debug.Log("Game Resumed");
    }
    private void StartMenu()
    {
        Time.timeScale = 0f;

        if (startMenuUI != null)
        {
            startMenuUI.SetActive(true);  // Show the start menu UI
        }
    }

    public void UpdateHealthUI(int health)
    {
        switch(health) 
        {
            case 3:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(true);
                break;
            case 2:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                break;
            case 1:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
            case 0:
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                CallGameOver();
                break;
        }
    }

    private void CallGameOver()
    {
       gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
}
