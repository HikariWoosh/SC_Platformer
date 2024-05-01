using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dialogueScript : MonoBehaviour
{

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private Image characterImage;

    [SerializeField]
    public bool dialogueRead;

    [SerializeField]
    private string[] dialogueLines;

    [SerializeField]
    private Sprite[] characterImages;

    [SerializeField]
    private AudioSource[] audioSources;

    [SerializeField]
    private float textSpeed;

    [SerializeField]
    private float timeToText;

    [SerializeField]
    private float timeTillText;

    [SerializeField]
    private int index;

    [SerializeField]
    private bool TPPlayer;

    [SerializeField]
    private InGameMenu gameMenu;

    [SerializeField]
    private timeSlow timeSlow;

    [SerializeField]
    private GameObject Eos;

    [SerializeField]
    private Sprite EosIcon;

    [SerializeField]
    private Material eosIdle;

    [SerializeField]
    private Material eosTalk;



    void Start()
    {
        gameMenu = GameObject.Find("UI").GetComponent<InGameMenu>();
        timeSlow = GameObject.Find("gameManager").GetComponent<timeSlow>();

        text.text = string.Empty;
        dialogueRead = false;
        initaliseDialogue();
    }

    // Update is called once per frame
    void Update()
    {
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

    void dialogueLogic()
    {
        timeToText = 0;
        audioSources[index].Stop();
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

    public void initaliseDialogue()
    {
        index = 0;
        ChangeCharacterImage();
        if (SceneManager.GetActiveScene().name == "Beginning Sequence" && PlayerPrefs.HasKey("tutorialCompleted") && PlayerPrefs.GetInt("tutorialCompleted") == 1)
        {
            Debug.Log("Audio Silenced");
        }
        else
        {
            audioSourcePlay();
        }
        StartCoroutine(typeDialogue());
    }

    IEnumerator typeDialogue()
    {
        foreach (char c in dialogueLines[index].ToCharArray())
        {
            text.text += c;

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

        if (TPPlayer)
        {
            Eos.GetComponent<Renderer>().material = eosIdle;
        }

        audioSources[index].Stop();
    }

    public void nextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            text.text += string.Empty;
            StartCoroutine(typeDialogue());
            audioSourcePlay();
            ChangeCharacterImage();
        }
        else
        {
            dialogueRead = true;
            gameObject.SetActive(false);
            if (TPPlayer && timeSlow.slowTimeUnlocked != true)
            {
                timeSlow.slowTimeUnlocked = true;
                PlayerPrefs.SetInt("timeSlowing", 1);
                gameMenu.Interstice();   
            }
        }
    }

    private void ChangeCharacterImage()
    {
        if (index < characterImages.Length && characterImages[index] != null)
        {
            characterImage.sprite = characterImages[index];
        }

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

    private void audioSourcePlay()
    {
        if (index < audioSources.Length && audioSources[index] != null && !gameMenu.isPaused)
        {
            audioSources[index].Play();
        }
    }
}
