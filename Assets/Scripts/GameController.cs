using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private CinemachineVirtualCamera[] vcam;
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
    [SerializeField] private Image[] enemysImage;
    [SerializeField] private TMP_Text[] number;
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
        currentVcam = vcam[currentVcamIndex];
        
    }

    public void ChangeVCam(int index)
    {
        currentVcam.m_Priority = 0;
        int i = Mathf.Clamp(index + currentVcamIndex, 0, vcam.Length);
        currentVcam = vcam[i];
        Debug.Log(currentVcam.name);
        currentVcam.m_Priority = 10;
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
            if (enemyName == "Bee")
            {
                for (int j = 0; j < numberOfEnemyTypes; j++)
                {
                    if (enemysList[j] == "Bee")
                    {
                        enemysDestroyed[j] = Math.Clamp(enemysDestroyed[j] + 1, 0, enemysAmount[j]);
                        number[j].text = enemysDestroyed[j] + "/" + enemysAmount[j];
                        break;
                    }
                }
                break;
            }
            else if (enemyName == "Bat")
            {
                for (int j = 0; j < numberOfEnemyTypes; j++)
                {
                    if (enemysList[j] == "Bat")
                    {
                        enemysDestroyed[j] = Math.Clamp(enemysDestroyed[j] + 1, 0, enemysAmount[j]);
                        number[j].text = enemysDestroyed[j] + "/" + enemysAmount[j];
                        break;
                    }
                }
                break;
            }
            else if (enemyName == "Plant")
            {
                for (int j = 0; j < numberOfEnemyTypes; j++)
                {
                    if (enemysList[j] == "Plant")
                    {
                        enemysDestroyed[j] = Math.Clamp(enemysDestroyed[j] + 1, 0, enemysAmount[j]);
                        number[j].text = enemysDestroyed[j] + "/" + enemysAmount[j];
                        break;
                    }
                }
                break;
            }
            else if (enemyName == "Snail")
            {
                for (int j = 0; j < numberOfEnemyTypes; j++)
                {
                    if (enemysList[j] == "Snail")
                    {
                        enemysDestroyed[j] = Math.Clamp(enemysDestroyed[j] + 1, 0, enemysAmount[j]);
                        number[j].text = enemysDestroyed[j] + "/" + enemysAmount[j];
                        break;
                    }
                }
                break;
            }
            else if (enemyName == "Chicken")
            {
                for (int j = 0; j < numberOfEnemyTypes; j++)
                {
                    if (enemysList[j] == "Chicken")
                    {
                        enemysDestroyed[j] = Math.Clamp(enemysDestroyed[j] + 1, 0, enemysAmount[j]);
                        number[j].text = enemysDestroyed[j] + "/" + enemysAmount[j];
                        break;
                    }
                }
                break;
            }
        }
    }
}
