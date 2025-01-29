using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour, ISaveSystem
{
    private int currentAnimator = 0;
    [SerializeField] private Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        Disable(currentAnimator);
    }
    public void LoadData(GameData data)
    {
        this.currentAnimator = data.animator;
    }
    public void SaveData(ref GameData data)
    {
        data.animator = this.currentAnimator;
    }

    public void Disable(int index)
    {
        for (int i = 0; i < 4; i++)
        {
            buttons[i].interactable = true;
        }
        buttons[index].interactable = false;
    }
    public void ChangeAnimation(int amount)
    {
        currentAnimator = amount;
        if(GameController.instance != null) GameController.instance.ChangeAnim(amount);
        SaveSystem.Instance.SaveGame();
        Disable(amount);
    }
}
