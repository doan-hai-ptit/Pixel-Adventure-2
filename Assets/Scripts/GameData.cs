using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int animator = 0;
    public bool isRelaxed = false;
    public int normalLevels = 0;
    public int hardLevels = 0;
    public GameData()
    {
        this.animator = 0;
        this.isRelaxed = false;
        this.normalLevels = 5;
        this.hardLevels = 1;
    }
}
