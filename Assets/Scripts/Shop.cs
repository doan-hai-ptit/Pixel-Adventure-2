using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour, ISaveSystem
{
    private int coins = 10;
    [SerializeField] private TMP_Text coinText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();
    }
    public void LoadData(GameData data)
    {
        this.coins = data.coins;
    }

    public void SaveData(ref GameData data)
    {
        data.coins = this.coins;
    }
    private void UpdateCoinsText()
    {
        coinText.text = coins.ToString();
    }

    public void BuyCoin(int amount)
    {
        coins  = Math.Clamp(coins + amount, 0, Int32.MaxValue);
        UpdateCoinsText();
    }
}
