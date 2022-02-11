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

    private void Awake()
    {
        levelSelection = GetComponent<LevelSelection>();
    }
    public void HideInitialPanel()
    {
        initialPanel.SetActive(false);
        levelsButton.SetActive(true);
    }
    public void Initialize(int numberOfLevels)
    {
        levelSelection.Initialize(numberOfLevels);
    }
    public void OnLevelStart()
    {
        swipeToStart.SetActive(false);
        levelsButton.SetActive(false);
    }
    public void UpdateAvailableLevels()
    {
        levelSelection.UpdateAvailableLevels();
    }
    public void RestartLevel()
    {
        replayMenu.SetActive(false);
        levelSelection.RestartLevel();
    }
    public void ShowFailedLevelScreen()
    {
        replayMenu.SetActive(true);
        pauseButton.SetActive(false);
    }
    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
    }
    public void ClosePauseMenu()
    {
        Time.timeScale = 1f;
    }
    public void OnButtonClick()
    {
        onButtonClick.Raise();
    }

    public void UpdateCheeseAmount(int cheeseCollectedAmount)
    {
        cheeseAmountText.text = cheeseCollectedAmount.ToString();
    }

    public void ShowNextLevelScreen()
    {
        nextLevelMenu.SetActive(true);
    }
    public void OnNextLevelClick()
    {
        nextLevelMenu.SetActive(false);
        levelSelection.SwitchLevel(PlayerPrefs.GetInt("currentLevel", 0));
    }

    public void ShowEndGameScreen()
    {
        endGameMenu.SetActive(true);
        PlayerPrefs.SetInt("currentLevel", 0);
    }
    public void OnEndGameRestart()
    {
        endGameMenu.SetActive(false);
        levelSelection.SwitchLevel(PlayerPrefs.GetInt("currentLevel", 0));
    }
}
