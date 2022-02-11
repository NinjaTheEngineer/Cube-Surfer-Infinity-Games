using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject initialPanel, replayMenu, swipeToStart, pauseButton, levelsButton, nextLevelMenu, endGameMenu;

    [SerializeField]
    private TextMeshProUGUI cheeseAmountText;
    [SerializeField]
    private GameEvent onButtonClick;

    private LevelSelection levelSelection;

    private void Awake() //Get the level selection component
    {
        levelSelection = GetComponent<LevelSelection>();
    }
    public void HideInitialPanel() //Hide the initial panel when level is loaded
    {
        initialPanel.SetActive(false);
        levelsButton.SetActive(true);
    }
    public void Initialize(int numberOfLevels) //Initialize the level selection menu
    {
        levelSelection.Initialize(numberOfLevels);
    }
    public void OnLevelStart() //Trigged by onLevelStart event, handles the UI
    {
        swipeToStart.SetActive(false);
        levelsButton.SetActive(false);
    }
    public void UpdateAvailableLevels() //Updates the levels availability in the level selection menu
    {
        levelSelection.UpdateAvailableLevels();
    }
    public void RestartLevel() //Restarts the game
    {
        replayMenu.SetActive(false);
        levelSelection.RestartLevel();
    }
    public void ShowFailedLevelScreen() //Show failed level screen
    {
        replayMenu.SetActive(true);
        pauseButton.SetActive(false);
    }
    public void OpenPauseMenu() //Opens pause menu
    {
        Time.timeScale = 0f;
    }
    public void ClosePauseMenu() //Closes pause menu
    {
        Time.timeScale = 1f;
    }
    public void OnButtonClick() //On click event for the SoundManager to play the sound
    {
        onButtonClick.Raise();
    }

    public void UpdateCheeseAmount(int cheeseCollectedAmount) //Updates the cheese score amount in the UI
    {
        cheeseAmountText.text = cheeseCollectedAmount.ToString();
    }

    public void ShowNextLevelScreen() //Shows the next level menu after level completed
    {
        nextLevelMenu.SetActive(true);
    }
    public void OnNextLevelClick() //Handles the click on the next level menu button
    {
        nextLevelMenu.SetActive(false);
        levelSelection.SwitchLevel(PlayerPrefs.GetInt("currentLevel", 0));
    }

    public void ShowEndGameScreen() //Show end game screen when all levels are completed
    {
        endGameMenu.SetActive(true);
        PlayerPrefs.SetInt("currentLevel", 0);
    }
    public void OnEndGameRestart() //Handles the click on the restart game at the end game screen
    {
        endGameMenu.SetActive(false);
        levelSelection.SwitchLevel(PlayerPrefs.GetInt("currentLevel", 0));
    }
}
