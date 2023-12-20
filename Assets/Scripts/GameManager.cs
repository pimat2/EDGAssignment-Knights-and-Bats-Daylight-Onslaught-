using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //Define the different states of the game
    public enum GameState
    {
        Gameplay,
        Pause,
        GameOver,
        LevelUp
    }

    //Stores the current active game state
    public GameState currentState;
    //Store the prveious state of the game
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;
    [Header("TMPro objects for live display of stats")]
    //Current stats display
    public TextMeshProUGUI currentHealthDisplay;
    public TextMeshProUGUI currentRecoveryDisplay;
    public TextMeshProUGUI currentMoveSpeedDisplay;
    public TextMeshProUGUI currentMightDisplay;
    public TextMeshProUGUI currentProjectileSpeedDisplay;
    public TextMeshProUGUI currentMagnetDisplay;
    [Header("Results Screen Display")]
    public Image chosenCharachterImage;
    public TextMeshProUGUI chosenCharacterName;
    public TextMeshProUGUI levelReachedDisplay;
    public TextMeshProUGUI timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);
    [Header("Stopwatch")]
    public float timeLimit; //time limit in seconds
    float stopwatchTime; //current time elapsed
    public TextMeshProUGUI stopwatchDisplay;

    //Check if the game is over
    public bool isGameOver = false;
    public bool choosingUpgrade = false;

    //Reference to the player's game object
    public GameObject playerObject;
    void Awake() 
    {
        
        if(instance ==null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Extra" + this + "DELETED");
            Destroy(gameObject);
        }
        DisableScreens();    

    }
    void Update() 
    {

        switch(currentState)
        {
            case GameState.Gameplay:
                // Code for gameplay state
                CheckForPauseAndResume();
                UpdateStopWatch();
                break;
            case GameState.Pause:
                // Code for Pause state 
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                //Code for game over state
                if(!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GAME IS OVER");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if(!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    Debug.Log("Upgrades should appear");
                    levelUpScreen.SetActive(true);
                }
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
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);

    }
    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }
    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharachterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.Name;
    }
    public void AssignLevelReached(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }
    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if(chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.LogWarning("Chosen weapons and passive items data list have different lengths");
            return;
        }
        //Assign choseen weapons data to chosen weapon UI
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            //Checks to make sure that the sprite of the corresponding weapon in weaponData is not null
            if(chosenWeaponsData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            //Checks to make sure that the sprite of the corresponding passive item in passiveItemData is not null
            if(chosenPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }

    }
    void UpdateStopWatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopWatchDisplay();
        if(stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopWatchDisplay()
    {
        //calculate elapsed time and convert it into minutes and seconds
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);
        //update stopwatch text
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }
    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
