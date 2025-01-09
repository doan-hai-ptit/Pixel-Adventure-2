using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField]private CinemachineVirtualCamera[] vcam;
    private CinemachineVirtualCamera currentVcam;
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
    [SerializeField] private Image[] enemysImage;
    [SerializeField] private TMP_Text[] number;
    // Variables related to collect objects
    [SerializeField] private List<GameObject> objs = new List<GameObject>();
    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnsceneLoaded;
        SetUpEnemysList();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin perlin = currentVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0f;
            }
        }
    }

    void Start()
    {
        
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnsceneLoaded;
    }
    void OnsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        vcam = FindObjectsOfType<CinemachineVirtualCamera>();
        int len = vcam.Length;
        for (int i = 0; i < len; i++)
        {
            for (int j = i + 1; j < len; j++)
            {
                if (string.Compare(vcam[i].gameObject.name, vcam[j].gameObject.name) > 0)
                {
                    // Hoán đổi vcam[i] và vcam[j]
                    (vcam[i], vcam[j]) = (vcam[j], vcam[i]);
                }
            }
        }
        currentVcam = vcam[currentVcamIndex];
        
    }

    public void ChangeVCam(int index)
    {
        currentVcam.m_Priority = 0;
        int i = Mathf.Clamp(index + currentVcamIndex, 0, vcam.Length);
        currentVcam = vcam[i];
//        Debug.Log(currentVcam.name);
        currentVcam.m_Priority = 10;
        for (int j = i; j < vcam.Length; j++)
        {
            CinemachineBasicMultiChannelPerlin perlin = vcam[j].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0f;
        }
    }
    public void NextScene()
    {
        StartCoroutine(LoadLevel());
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = currentVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

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
        }

        for (int i = 0; i < numberOfEnemyTypes; i++)
        {
            number[i].text = enemysDestroyed[i] + "/" + enemysAmount[i];
        }
    }

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
}