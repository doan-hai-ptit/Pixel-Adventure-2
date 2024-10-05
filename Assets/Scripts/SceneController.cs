using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public void NextScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(LoadLevel());
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadLevel()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetTrigger("Start");
    }
}
