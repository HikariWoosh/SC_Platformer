using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject gameCamera;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

}