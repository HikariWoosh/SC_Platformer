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

    void Start()
    {
        text.text = string.Empty;
        dialogueRead = false;
        initaliseDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
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
            yield return new WaitForSeconds(textSpeed * Time.deltaTime);
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
        }
    }

    private void ChangeCharacterImage()
    {
        if (index < characterImages.Length && characterImages[index] != null)
        {
            characterImage.sprite = characterImages[index];
        }
    }

    private void audioSourcePlay()
    {
        if (index < audioSources.Length && audioSources[index] != null)
        {
            audioSources[index].Play();
        }
    }
}
