using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Linq;
using System.IO;
public class SaveSystem : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler fileDataHandler;
    private GameData gameData;
    private List<ISaveSystem> saveSystems;
    public static SaveSystem Instance;
    private void Awake()
    {
        if (Instance != null)
        {
        }
        Instance = this;
    }

    public void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.saveSystems = GetSaveSystems();
        LoadGame();
    }
    
    public void SaveGame()
    {
        foreach (ISaveSystem save in saveSystems)
        {
            save.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        //Debug.Log("Load Game");
        this.gameData = fileDataHandler.Load();
        if (this.gameData == null)
        {
            NewGame();
        }
        foreach (ISaveSystem save in saveSystems)
        {
            save.LoadData(gameData);
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        SaveGame();
    }

    private List<ISaveSystem> GetSaveSystems()
    {
        IEnumerable<ISaveSystem> saveSystems = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<ISaveSystem>();
       return new List<ISaveSystem>(saveSystems);
    }
}
