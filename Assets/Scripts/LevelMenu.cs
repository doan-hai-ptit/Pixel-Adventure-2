using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelMenu : MonoBehaviour, ISaveSystem
{
    private bool isRelaxed = false;
    private int normalLevel = 1;
    private int hardLevel = 1;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Toggle toggle;
    public void OpenLevel(int level)
    {
        string levelName = "Level" + level;
        SceneManager.LoadScene(levelName);
    }

    void Start()
    {
        //ShowButtonMode();
        //ShowButtonLevel();
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadData(GameData data)
    {
        this.normalLevel = data.normalLevels;
        this.hardLevel = data.hardLevels;
        this.isRelaxed = data.isRelaxed;
    }

    public void SaveData(ref GameData data)
    {
        data.isRelaxed = this.isRelaxed;
    }

    public void ShowButtonLevel()
    {
        if (isRelaxed)
        {
            for (int i = 1; i <= normalLevel; i++)
            {
                buttons[i-1].interactable = true;
            }

            for (int i = normalLevel+1; i <= 5; i++)
            {
                buttons[i-1].interactable = false;
            }
        }
        else
        {
            for (int i = 1; i <= hardLevel; i++)
            {
                buttons[i-1].interactable = true;
            }

            for (int i = hardLevel+1; i <= 5; i++)
            {
                buttons[i-1].interactable = false;
            }
        }
    }
    public void ChangeMode()
    {
        if (!isRelaxed)
        {
            isRelaxed = true;
        }
        else
        {
            isRelaxed = false;
        }
        ShowButtonLevel();
        SaveSystem.Instance.SaveGame();
    }

    public void ShowButtonMode()
    {
        if (!isRelaxed)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.SetIsOnWithoutNotify(false);
        }
    }
}
