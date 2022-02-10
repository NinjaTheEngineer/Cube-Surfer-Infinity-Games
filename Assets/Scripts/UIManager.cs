using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject initialPanel, replayMenu, swipeToStart, pauseButton;

    private LevelSelection levelSelection;

    private void Awake()
    {
        levelSelection = GetComponent<LevelSelection>();
    }
    public void HideInitialPanel()
    {
        initialPanel.SetActive(false);
    }
    public void Initialize(int numberOfLevels)
    {
        levelSelection.Initialize(numberOfLevels);
    }
    public void OnLevelStart()
    {
        swipeToStart.SetActive(false);
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
}
