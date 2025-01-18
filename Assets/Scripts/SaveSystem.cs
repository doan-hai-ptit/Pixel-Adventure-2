using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Linq;

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
            Debug.LogWarning("More than one instance of SaveSystem found!");
        }
        Instance = this;
    }

    public void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.saveSystems = GetSaveSystems();
        Debug.Log(saveSystems.Count);
        LoadGame();
    }
    
    public void SaveGame()
    {
        foreach (ISaveSystem save in saveSystems)
        {
            save.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
        Debug.Log("Saved Game");
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();
        if (this.gameData == null)
        {
            Debug.LogWarning("No game data loaded!");
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
    }

    private List<ISaveSystem> GetSaveSystems()
    {
        IEnumerable<ISaveSystem> saveSystems = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<ISaveSystem>();
       return new List<ISaveSystem>(saveSystems);
    }
}
