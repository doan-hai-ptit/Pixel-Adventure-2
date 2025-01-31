using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class GameController : MonoBehaviour, ISaveSystem
{
    public InputAction pauseAction;
    private bool paused = false;
    public static GameController instance;
    [SerializeField] private CinemachineVirtualCamera[] vcam;
    [SerializeField] private CinemachineVirtualCamera currentVcam;
    [SerializeField] private GameObject PauseMenu;
    public int currentVcamIndex = 0;
    private float shakeTimer;
    [SerializeField] Animator animator;
    // Variables related to collect enemys
    [SerializeField] private int numberOfEnemyTypes = 4;
    [SerializeField] private string[] enemysList;
    [SerializeField] private int[] enemysAmount;
    [SerializeField] private int[] enemysDestroyed;
    [SerializeField] private Sprite bee;
    [SerializeField] private Sprite bat;
    [SerializeField] private Sprite plant;
    [SerializeField] private Sprite snail;
    [SerializeField] private Sprite chicken;
    [SerializeField] private Sprite mushroom;
    [SerializeField] private Sprite radish;
    [SerializeField] private Sprite blueBird;
    [SerializeField] private Sprite fatBird;
    [SerializeField] private Sprite turtle;
    [SerializeField] private Sprite duck;
    [SerializeField] private Image[] enemysImage;
    [SerializeField] private TMP_Text[] number;
    // Variables related to collect objects
    [SerializeField] private List<GameObject> objs = new List<GameObject>();
    // Variables related to change animator
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] RuntimeAnimatorController[] listControllers;
    private int currentAnimIndex = 0;
    // Variables related to change mode
    private bool isRelaxed = false;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private PlayerController player;
    // Variables related to up level
    [SerializeField] private GameObject completeUI;
    private int levelMax;
    [SerializeField] private int currentLevel;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
        //DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnsceneLoaded;
        SetUpEnemysList();
        
    }
    void Start()
    {
        pauseAction.Enable();
        animatorPlayer.runtimeAnimatorController = listControllers[currentAnimIndex];
        if (isRelaxed)
        {
            player.isRelaxed = true;
            healthBar.SetActive(false);
        }
    }
    private void Update()
    {
        paused = pauseAction.WasPressedThisFrame();
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin perlin = currentVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0f;
            }
        }

        if (paused)
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
        }
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnsceneLoaded;
    }
    public void NextScene()
    {
        if(currentLevel < 5) StartCoroutine(LoadLevel());
        else OpenMainMenu();
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void OnsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        vcam = FindObjectsOfType<CinemachineVirtualCamera>();
        int len = vcam.Length;
        for (int i = 0; i < len; i++)
        {
            for (int j = i + 1; j < len; j++)
            {
                if (String.CompareOrdinal(vcam[i].gameObject.name, vcam[j].gameObject.name) > 0) // loi cam o day neu co
                {
                    // Hoán đổi vcam[i] và vcam[j]
                    (vcam[i], vcam[j]) = (vcam[j], vcam[i]);
                }
            }
        }
        currentVcam = vcam[currentVcamIndex];
        
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator LoadLevel()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetTrigger("Start");
    }
    public bool IsEligible()
    {
        for (int i = 0; i < numberOfEnemyTypes; i++)
        {
            if (enemysDestroyed[i] != enemysAmount[i])
            {
                return false;
            }
        }
        return true;
    }
    // Vcam
    public void ChangeVCam(int index)
    {
        currentVcam.m_Priority = 0;
        int i = Mathf.Clamp(index + currentVcamIndex, 0, vcam.Length-1);
        currentVcam = vcam[i];
        currentVcam.m_Priority = 10;
        for (int j = 0; j < i; j++)
        {
            CinemachineBasicMultiChannelPerlin perlin = vcam[j].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0f;
        }
    }
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = currentVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
    //Enemys
    private void SetUpEnemysList()
    {
        for (int i = 0; i < numberOfEnemyTypes; i++)
        {
            if(enemysList[i] == "Bee") enemysImage[i].sprite = bee;
            else if (enemysList[i] == "Bat") enemysImage[i].sprite = bat;
            else if (enemysList[i] == "Plant") enemysImage[i].sprite = plant;
            else if (enemysList[i] == "Snail") enemysImage[i].sprite = snail;
            else if (enemysList[i] == "Chicken") enemysImage[i].sprite = chicken;
            else if (enemysList[i] == "Mushroom") enemysImage[i].sprite = mushroom;
            else if (enemysList[i] == "Radish") enemysImage[i].sprite = radish;
            else if (enemysList[i] == "BlueBird") enemysImage[i].sprite = blueBird;
            else if (enemysList[i] == "FatBird") enemysImage[i].sprite = fatBird;
            else if (enemysList[i] == "Turtle") enemysImage[i].sprite = turtle;
            else if (enemysList[i] == "Duck") enemysImage[i].sprite = duck;
            else
            {
                enemysImage[i].enabled = false;
                number[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < numberOfEnemyTypes; i++)
        {
            number[i].text = enemysDestroyed[i] + "/" + enemysAmount[i];
        }
    }
    public void UpdateEnemysList(string enemyName)
    {
        for (int i = 0; i < numberOfEnemyTypes; i++)
        {
            if (enemyName == enemysList[i])
            {
                enemysDestroyed[i] = Math.Clamp(enemysDestroyed[i] + 1, 0, enemysAmount[i]);
                number[i].text = enemysDestroyed[i] + "/" + enemysAmount[i];
                break;
            }
        }
    }
    //ResetObjs
    public void CollectObj(GameObject obj)
    {
        this.objs.Add(obj);
    }
    public void ResetObj()
    {
        int len = objs.Count;
        for (int i = 0; i < len; i++)
        {
            ResettableObject resettableObject = objs[i].GetComponent<ResettableObject>();
            //Resettable
            resettableObject.ResetObject();
        }
        objs.Clear();
    }
    //Menu
    public void Resume()
    {
        Time.timeScale = 1f;
        animatorPlayer.runtimeAnimatorController = listControllers[currentAnimIndex];
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    //Animator
    public void ChangeAnim(int index)
    {
        currentAnimIndex = index;
    }
    //Data
    public void LoadData(GameData data)
    {
        this.currentAnimIndex = data.animator;
        this.isRelaxed = data.isRelaxed;
        this.levelMax = data.hardLevels;
    }

    public void SaveData(ref GameData data)
    {
        if (!this.isRelaxed && levelMax > data.hardLevels)
        {
            data.hardLevels = levelMax;
        }
    }
    //Complete LV
    public void CompleteLV()
    {
        if (!isRelaxed && levelMax == currentLevel)
        {
            levelMax = Mathf.Clamp(levelMax + 1, 1, 5);
        }
        completeUI.SetActive(true);
    }
}