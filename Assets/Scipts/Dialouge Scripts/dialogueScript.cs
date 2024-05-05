using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dialogueScript : MonoBehaviour
{

    [Header("Dialouge Arrays")]

    [SerializeField]
    private string[] dialogueLines; // Array to store the dialouge lines of each convosation

    [SerializeField]
    private Sprite[] characterImages; // Array to store the character images of each convosation

    [SerializeField]
    private AudioSource[] audioSources; // Array to store the audio of each convosation


    [Header("Text Controls")]

    [SerializeField]
    private TMP_Text text; // Text used to display the dialogue

    [SerializeField]
    private Image characterImage; // Image displayed next to the dialogue

    [SerializeField]
    public bool dialogueRead; // Check to see if the dialouge has been triggered

    [SerializeField]
    private float textSpeed; // Speed at which each character is outputted

    [SerializeField]
    private float timeToText; // Time to control the output of dialogue to ensure it does not run when paused

    [SerializeField]
    private float timeTillText;  // Time limit referenced by timeToText

    [SerializeField]
    private int index; // The index of the current dialouge convosation

    [SerializeField]
    private bool TPPlayer; // Bool to check if the current convosation requires specific output


    [Header("Game Objects")]

    [SerializeField]
    private InGameMenu gameMenu; // Reference to the in-game menu script

    [SerializeField]
    private timeSlow timeSlow; // Reference to the time slow script

    [SerializeField]
    private GameObject Eos; // The game object used to display the character of Eos

    [SerializeField]
    private Sprite EosIcon; // Sprite used to represent Eos

    [SerializeField]
    private Material eosIdle; // Material property (dark blue) - Not talking

    [SerializeField]
    private Material eosTalk; // Material property (light blue) - Talking


    void Start()
    {
        // Caches important components
        gameMenu = GameObject.Find("UI").GetComponent<InGameMenu>();
        timeSlow = GameObject.Find("gameManager").GetComponent<timeSlow>();

        // Initalises dialouge
        text.text = string.Empty;
        dialogueRead = false;
        initaliseDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        // Allows the player to request to skip the current dialogue output
        if(Input.GetMouseButtonDown(0) && !gameMenu.isPaused)
        {
            dialogueLogic();
        }
        else
        {
            timeToText += Time.deltaTime;
            if (timeToText >= timeTillText)
            {
                dialogueLogic();
            }
        }
        
    }

    // Function to skip the slow output of the dialouge 
    void dialogueLogic()
    {
        timeToText = 0;
        audioSources[index].Stop();

        // If the dialogue has finished, go to next line, else finish current line
        if (text.text == dialogueLines[index])
        {
            text.text = string.Empty;
            nextLine();
        }
        else
        {
            StopAllCoroutines();
            text.text = dialogueLines[index];
        }
    }

    // Initalises the dialouge sequence by resetting index and displaying correct image
    public void initaliseDialogue()
    {
        index = 0;
        changeCharacterImage();

        // Prevents tutorial audio from being audible when progressing from the main menu
        if (SceneManager.GetActiveScene().name == "Beginning Sequence" && PlayerPrefs.HasKey("tutorialCompleted") && PlayerPrefs.GetInt("tutorialCompleted") == 1)
        {
            return;
        }
        else
        {
            audioSourcePlay();
        }

        StartCoroutine(typeDialogue());
    }

    // Coroutine used to output dialouge
    IEnumerator typeDialogue()
    {
        // Displays each character in the current piece of dialouge
        foreach (char c in dialogueLines[index].ToCharArray())
        {
            text.text += c;

            // Ensures the dialouge is not run when paused
            if (!gameMenu.isPaused)
            {
                audioSources[index].UnPause();
                yield return new WaitForSeconds(textSpeed * Time.deltaTime);
            }
            else
            {
                audioSources[index].Pause();
                yield return new WaitWhile(() => gameMenu.isPaused);
            }
        }
        
        // During a certain conovsation, ensures material property is reset
        if (TPPlayer)
        {
            Eos.GetComponent<Renderer>().material = eosIdle;
        }

        // Prevents the audio of each character from continung when they have finished talking
        audioSources[index].Stop();
    }

    // Function run to display the next dialouge line
    public void nextLine()
    {
        // If there is another line of dialouge, display it
        if (index < dialogueLines.Length - 1)
        {
            index++;
            text.text += string.Empty;
            StartCoroutine(typeDialogue());
            audioSourcePlay();
            changeCharacterImage();
        }
        else
        {
            // If the last line of dialouge, set it as read and deactive the GameObject
            dialogueRead = true;
            gameObject.SetActive(false);

            // If completing a certain convosation, updates the players save
            if (TPPlayer && timeSlow.slowTimeUnlocked != true)
            {
                timeSlow.slowTimeUnlocked = true;
                PlayerPrefs.SetInt("timeSlowing", 1);
                gameMenu.Interstice();   
            }
        }
    }


    // Displays the correct character image depending on the current index
    private void changeCharacterImage()
    {
        if (index < characterImages.Length && characterImages[index] != null)
        {
            characterImage.sprite = characterImages[index];
        }

        // During certain convosations, alters the material of the GameObject "Eos"
        if (TPPlayer)
        {
            Renderer eosRenderer = Eos.GetComponent<Renderer>();
            if (characterImage.sprite == EosIcon)
            {
                eosRenderer.material = eosTalk;
            }
            else
            {
                eosRenderer.material = eosIdle;
            }
        }
    }

    // Plays the respective audio source depending on the current index
    private void audioSourcePlay()
    {
        if (index < audioSources.Length && audioSources[index] != null && !gameMenu.isPaused)
        {
            audioSources[index].Play();
        }
    }
}
