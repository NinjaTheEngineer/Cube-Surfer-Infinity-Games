using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public Transform buttonsGrid;
    public GameObject levelButtonPrefab;

    [SerializeField]
    private GameEvent switchLevel, onButtonClick;
    private List<Button> lvlButtons;
    private int currentLevel;
    private int maxLevel;

    public void Initialize(int numberOfLevels) //Initializes the level selector with a number of levels
    {
        lvlButtons = new List<Button>();
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        for (int i = 0; i < numberOfLevels; i++) //Create a button for each level
        {
            int x = i;
            Button button = Instantiate(levelButtonPrefab, buttonsGrid).GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate
                                            {
                                                SwitchLevel(x);
                                                onButtonClick.Raise(); //click listener to each button
                                            });
            lvlButtons.Add(button);
        }
        UpdateAvailableLevels();
    }
    public void UpdateAvailableLevels() //Starts the coroutine
    {
        StartCoroutine(UpdateLevels());
    }
    public void RestartLevel()
    {
        SwitchLevel(currentLevel);
    }
    IEnumerator UpdateLevels() //Updates the available levels and their text
    {
        yield return new WaitForSeconds(0.5f);
        maxLevel = PlayerPrefs.GetInt("maxLevel", 0);
        for (int i = 0; i < lvlButtons.Count; i++)
        {
            lvlButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            lvlButtons[i].interactable = (i <= maxLevel);
        }
    }
    public void SwitchLevel(int level) //Switch to a new level
    {
        maxLevel = PlayerPrefs.GetInt("maxLevel", 0);
        if (maxLevel >= level)
        {
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("currentLevel", level);
            switchLevel.Raise();
            return;
        }
        Debug.LogWarning("Can't play this level yet!");
    }
}
