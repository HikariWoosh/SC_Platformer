using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthControl : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField]
    private int maxHealth; // Used to set the users max health

    [SerializeField]
    public int Health; // Used to control the users current health

    [SerializeField]
    private PlayerController playerCharacter; // Refers to the players PlayerController


    [Header("Respawn Details")]
    [SerializeField]
    public bool isRespawning; // Variable to control the respawn process

    [SerializeField]
    public Vector3 respawnPoint; // Position of respawn point

    [SerializeField]
    private float respawnLength; // Amount of time it takes until the respawn process begins

    [SerializeField]
    private GameObject deathParticles; // Particles that are emitted upon death

    [SerializeField]
    private CharacterController charControl;


    [Header("Fading")]
    [SerializeField]
    private Image blackScreen; // An image of a black screen used to emulate a fading effect

    [SerializeField]
    public bool isFading; // isFading variable to control when the fading effect beings

    [SerializeField]
    public bool unFading; // unFading variable to control when the fading effect ends

    [SerializeField]
    private float fadeSpeed; // How fast the black screen fades 

    [SerializeField]
    private float waitFade; // How long it should take to fade in and out

    [SerializeField]
    public bool Transition;



    [Header("Sound Effects")]

    [SerializeField]
    private AudioSource deathSoundEffect; // Death Sound Effect AS slot

    [SerializeField]
    private AudioSource checkpointSoundEffect; // Checkpoint Sound Effect AS slot


    [Header("Deaths")]

    [SerializeField]
    private TMP_Text deathText;

    [SerializeField]
    private int deaths = 0;

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth; // Sets the players health to max upon spawning

        deathText.text = string.Format("x {0}", deaths); // Sets death text to 0

        respawnPoint = playerCharacter.transform.position; // Sets the players respawn point to were they begin

        charControl = FindAnyObjectByType<CharacterController>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading)
        {
            Transition = true;
            // Alters the alpha value of the black screen to simulate a fade out effect, the closer to 1f the closer the alpha value is to 255
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(blackScreen.color.a == 1f)
            {
                isFading = false;
                Transition = false;
            }
        }

        // Alters the alpha value of the black screen to simulate a fade in effect
        if (unFading)
        {
            Transition = true;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 0f)
            {
                unFading = false;
                Transition = false;
            }
        }
    }

    // Method to damage the player, takes in a int parameter for the damage
    public void damagePlayer(int damage)
    {
        Health -= damage; // Takes the taken damage off the players health

        // If the players health reaches 0 (or less), respawn
        if (Health <= 0)
        {
            deathSoundEffect.Play();
            respawn();
        }
    }

    // Method used to heal the player, takes in a int parameter for the amount of healing
    public void healPlayer(int heal)
    {
        Health += heal;

        // Prevents the players health from exceeding the max health value
        if(Health >  maxHealth)
        {
            Health = maxHealth;
        }
    }

    // Respawn function that is called when the players health is below 0
    public void respawn()
    {
        // Prevents the player from dying twice if two instances of damage are taken by using Co-Routines
        if (!isRespawning)
        {
            StartCoroutine("reCo");
        }
    }

    // Co-Routine to handle player respawning
    public IEnumerator reCo()
    {
        isRespawning = true;
        // Disables the playerCharacter, preventing it from being accessed
        playerCharacter.gameObject.SetActive(false);

        // Increments the players death counter
        deaths += 1;
        deathText.text = string.Format("x {0}", deaths);

        // Plays the particle effect ontop of the player when they die, deleting the particle clone after 1 second
        GameObject Particles = Instantiate(deathParticles, playerCharacter.transform.position, playerCharacter.transform.rotation);
        Destroy(Particles, 1f);

        yield return new WaitForSeconds(respawnLength); // Waits for the designated time before performing a respawn

        isFading = true;

        yield return new WaitForSeconds(waitFade); // Waits until the fade begins

        // Enables the player character, allowing it to be accessed 
        playerCharacter.gameObject.SetActive(true);

        // Disables the CharacterController to allow the respawn to reposition the player

        charControl.enabled = false;

        playerCharacter.transform.position = respawnPoint; // Set player position to respawn point 
        Health = maxHealth;

        yield return new WaitForSeconds(waitFade); // Waits until the fade finishes

        yield return new WaitForSeconds(0.5f);

        charControl.enabled = true; // Re-enables the players character controller to allow them to move again

        // Resets the players dash if said co-routine was interuptted
        dashRest();

        isRespawning = false;
        unFading = true;
    }

    public IEnumerator Fade()
    {
        Debug.Log("Check");

        isFading = true;

        yield return new WaitForSeconds(waitFade); // Waits until the fade begins

        unFading = true;
    }

    // Function to control respawn position in relation to checkpoints, takes in a position value depending on the checkpoints position
    public void setCheckpoint(Vector3 newCheckpoint)
    {
        if (respawnPoint != newCheckpoint)
        {
            checkpointSoundEffect.Play(); // Plays the checkpoint sound effect if its a new checkpoint
        }
        respawnPoint = newCheckpoint;
    }

    private void dashRest()
    {
        playerCharacter.GetComponent<PlayerController>().canDash = true;
        playerCharacter.GetComponent<PlayerController>().elapsedTime = 0;
        playerCharacter.GetComponent<PlayerController>().moveSpeed = playerCharacter.GetComponent<PlayerController>().originalMoveSpeed;
        playerCharacter.GetComponent<PlayerController>().StelCrystal.fillAmount = 1;
    }

    private void Teleport()
    {

        // Enables the player character, allowing it to be accessed 
        playerCharacter.gameObject.SetActive(true);

        // Disables the CharacterController to allow the respawn to reposition the player
        charControl.enabled = false;
        playerCharacter.transform.position = respawnPoint; // Set player position to respawn point 
        Health = maxHealth;
        charControl.enabled = true; // Re-enables the players character controller to allow them to move again
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        respawnPoint = new Vector3(0, 5, 0);
        Teleport();

    }
}
