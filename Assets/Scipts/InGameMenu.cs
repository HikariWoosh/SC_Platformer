using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject gameCamera;

    [SerializeField]
    private GameObject gameManager;

    public bool isPaused = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        SceneManager.LoadScene("Main Menu");
        Unpause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

}