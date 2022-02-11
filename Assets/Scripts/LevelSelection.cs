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

    public void Initialize(int numberOfLevels)
    {
        lvlButtons = new List<Button>();
        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        for (int i = 0; i < numberOfLevels; i++)
        {
            int x = i;
            Button button = Instantiate(levelButtonPrefab, buttonsGrid).GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate
                                            {
                                                SwitchLevel(x);
                                                onButtonClick.Raise();
                                            });
            lvlButtons.Add(button);
        }
        UpdateAvailableLevels();
    }
    public void UpdateAvailableLevels()
    {
        StartCoroutine(UpdateLevels());
    }
    public void RestartLevel()
    {
        SwitchLevel(currentLevel);
    }
    IEnumerator UpdateLevels()
    {
        yield return new WaitForSeconds(0.5f);
        maxLevel = PlayerPrefs.GetInt("maxLevel", 0);
        Debug.Log(">Max level -" + maxLevel);
        for (int i = 0; i < lvlButtons.Count; i++)
        {
            lvlButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            lvlButtons[i].interactable = (i <= maxLevel);
        }
    }
    public void SwitchLevel(int level)
    {
        Debug.Log(">> level -> " + level);
        maxLevel = PlayerPrefs.GetInt("maxLevel", 0);
        Debug.Log(">> maxLevel -> " + maxLevel);
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
