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
    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 20;
    public TextMeshProUGUI textFont;
    public Camera referenceCamera;
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

    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        //If canvas for floating text is not set, return
        if(!instance.damageTextCanvas) return;
        //Find a camera that can be used to convert world position to a screen position
        if(!instance.referenceCamera) instance.referenceCamera = Camera.main;

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }

    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 50f)
    {
        //Start generating the floating text
        GameObject textObj = new GameObject("Damage Floating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = textFontSize;
        rect.position = referenceCamera.WorldToScreenPoint(target.position);

        //Makes sure floating text object is destroyed after the duration finishes
        Destroy(textObj, duration);
        //Parent the generated text object to a canvas
        textObj.transform.SetParent(instance.damageTextCanvas.transform);
        //Pan the text upwards and fade it away over ime
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        while(t < duration)
        {
            yield return w;
            t += Time.deltaTime;
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1-t / duration);
            yOffset += speed * Time.deltaTime;
            rect.position = referenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
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
            playerObject.SendMessage("Kill");
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
