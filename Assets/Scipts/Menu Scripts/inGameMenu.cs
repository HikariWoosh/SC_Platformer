using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private GameObject pauseMenu; // Reference to the pause menu 

    [SerializeField]
    private GameObject Timer; // Reference to the timer

    [SerializeField]
    private GameObject Crystals; // Reference to the crystals UI

    [SerializeField]
    private GameObject gameCamera; // Reference to the Main Camera

    [SerializeField]
    private GameObject gameManager; // Reference to the Game Manager

    [SerializeField] 
    private Animator transition; // Reference to the animator used for transitions

    [SerializeField]
    private float transitionTime; // Value used to control the length of transitions

    [SerializeField]
    public bool isPaused = false; // Bool to control if the game is paused or not

    private void Start()
    {
        transition = GameObject.Find("SceneTransition").GetComponent<Animator>();

        // Makes the cursor not visable to the user
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        // If the user presses escape and is not in a forbidden scene, open the menu
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Main Menu" && SceneManager.GetActiveScene().name != "Beginning Sequence")
        {
            // Ensures the player is not able to open the menu when respawning
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

    // Run when the menu is opened
    public void Pause()
    {
        // Enables the cursor, shows the menu, pauses time, and prevents camera rotation 
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        gameCamera.GetComponent<CameraControl>().SetRotateSpeed(0); 
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    // Run when the game is resumed
    public void Unpause()
    {
        // Enables the cursor and hides the pause menu
        Cursor.visible = false;
        pauseMenu.SetActive(false);

        // Checks if time was slowed when the game was paused and sets to correct value
        if (gameManager.GetComponent<timeSlow>().isTimeSlow) {
            Time.timeScale = 0.5f;
        }
        else
        {
            Time.timeScale = 1;
        }
        
        // Sets cmaera rotation back to original value
        gameCamera.GetComponent<CameraControl>().SetRotateSpeed(1.5f);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    // Runs when main menu button is pressed
    public void mainMenu()
    {
        // Re-enables the cursor and returns user to main menu
        StartCoroutine(LoadLevel("Main Menu"));
        Unpause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Function used to teleport user to different scenes
    private IEnumerator LoadLevel(string Level)
    {
        // Plays the scene transition effect
        transition.SetBool("Fade", true);

        yield return new WaitForSeconds(transitionTime);

        // Loads new scene
        SceneManager.LoadScene(Level);

        transition.SetBool("Fade", false);
    }

    // Runs when the "Return to Interstice" button is pressed
    public void Interstice()
    {
        StartCoroutine(LoadLevel("The Interstice"));
        Unpause();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Function run once all jewels in Realm of Time are collected
    public void RoTC()
    {
        StartCoroutine(LoadLevel("RoTComplete"));
        Unpause();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Retry button functionality
    public void Retry()
    {
        gameManager.GetComponent<HealthControl>().damagePlayer(2);
        Unpause();
    }

    // Button to restart the current scene
    public void Restart()
    {
        // Reloads the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        Unpause();
        SceneManager.LoadScene(currentSceneName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // Run when the scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Enables or disables certian UI components depending on scene
        if (scene.name != "Main Menu") 
        {
            if(scene.name != "The Interstice" && scene.name != "RoTComplete")
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