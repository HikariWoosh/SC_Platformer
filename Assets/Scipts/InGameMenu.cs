using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField]
    private CameraControl cameraControl;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject Timer;

    [SerializeField]
    private GameObject Crystals;

    [SerializeField]
    private GameObject gameCamera;

    [SerializeField]
    private GameObject gameManager;

    [SerializeField]
    private Animator transition;

    [SerializeField]
    private float transitionTime;

    public bool isPaused = false;

    private void Start()
    {
        cameraControl = FindAnyObjectByType<CameraControl>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Main Menu")
        {
            bool isRespawning = gameManager.GetComponent<HealthControl>().isRespawning;
            if (!isRespawning)
            {
                if (isPaused)
                {
                    Unpause();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Pause()
    {
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        gameCamera.GetComponent<CameraControl>().SetRotateSpeed(0); 
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void Unpause()
    {
        Cursor.visible = false;
        pauseMenu.SetActive(false);

        if (gameManager.GetComponent<timeSlow>().isTimeSlow) {
            Time.timeScale = 0.5f;
        }
        else
        {
            Time.timeScale = 1;
        }
        
        gameCamera.GetComponent<CameraControl>().SetRotateSpeed(1.5f);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void mainMenu()
    {
        StartCoroutine(LoadLevel("Main Menu"));
        Unpause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator LoadLevel(string Level)
    {
        transition.SetBool("Fade", true);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(Level);

        transition.SetBool("Fade", false);
    }

    public void Interstice()
    {
        StartCoroutine(LoadLevel("The Interstice"));
        Unpause();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Retry()
    {
        gameManager.GetComponent<HealthControl>().damagePlayer(2);
        Unpause();
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Unpause();
        SceneManager.LoadScene(currentSceneName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Main Menu") 
        {
            if(scene.name != "The Interstice")
            {
                Timer.SetActive(true);
            }
            else
            {
                Timer.SetActive(false);
            }

            Crystals.SetActive(true);
        }
        else
        {
            Timer.SetActive(false);
            Crystals.SetActive(false);
        }
    }

}