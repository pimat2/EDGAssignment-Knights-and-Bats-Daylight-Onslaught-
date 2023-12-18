using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Define the different states of the game
    public enum GameState
    {
        Gameplay,
        Pause,
        GameOver
    }

    //Stores the current active game state
    public GameState currentState;
    //Store the prveious state of the game
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;
    void Awake() {
        DisableScreens();    
    }
    void Update() 
    {

        switch(currentState)
        {
            case GameState.Gameplay:
                // Code for gameplay state
                CheckForPauseAndResume();
                break;
            case GameState.Pause:
                // Code for Pause state 
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                //Code for game over state
                break;

            default:
                Debug.LogWarning("State does not exist");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public void PauseGame()
    {
        if(currentState != GameState.Pause)
        {
            previousState = currentState;
            ChangeState(GameState.Pause);
            Time.timeScale = 0f; //Pauses the game time
            Debug.Log("Game has been paused");
            pauseScreen.SetActive(true);
        }
        
    }
    public void ResumeGame()
    {
        if(currentState == GameState.Pause)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // Resumes the game time
            Debug.Log("Game has been resumed");
            pauseScreen.SetActive(false);
        }
    }

    public void CheckForPauseAndResume()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentState == GameState.Pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void DisableScreens()
    {
        pauseScreen.SetActive(false);
    }
}
