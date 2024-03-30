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

    private bool isPaused = false;

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
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        gameCamera.GetComponent<CameraControl>().SetRotateSpeed(0); 
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        gameCamera.GetComponent<CameraControl>().SetRotateSpeed(1.5f);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Unpause();
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry()
    {
        gameManager.GetComponent<HealthControl>().damagePlayer(2);
        Cursor.lockState = CursorLockMode.Locked;
        Unpause();
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Cursor.lockState = CursorLockMode.Locked;
        Unpause();
    }

}