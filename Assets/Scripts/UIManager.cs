using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject initialPanel;

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

    public void UpdateAvailableLevels()
    {
        levelSelection.UpdateAvailableLevels();
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
