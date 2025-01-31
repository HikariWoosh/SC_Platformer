using System.Collections;
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
    public bool Transition; // Checks if the game is transitioning



    [Header("Sound Effects")]

    [SerializeField]
    private AudioSource deathSoundEffect; // Death Sound Effect AS slot

    [SerializeField]
    private AudioSource checkpointSoundEffect; // Checkpoint Sound Effect AS slot


    [Header("Deaths")]

    [SerializeField]
    private TMP_Text deathText; // Text used to display the amount of deaths

    [SerializeField]
    private int deaths = 0; // Death counter 

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth; 
        deathText.text = string.Format("x {0}", deaths);

        // Sets the players respawn point to were they begin
        respawnPoint = playerCharacter.transform.position;

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
        // Takes the taken damage off the players health
        Health -= damage;

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

        // Waits for the designated time before performing a respawn
        yield return new WaitForSeconds(respawnLength);

        isFading = true;

        // Waits until the fade begins
        yield return new WaitForSeconds(waitFade);

        // Enables the player character, allowing it to be accessed 
        playerCharacter.gameObject.SetActive(true);

        // Disables the CharacterController to allow the respawn to reposition the player
        charControl.enabled = false;

        // Set player position to respawn point 
        playerCharacter.transform.position = respawnPoint;
        Health = maxHealth;

        // Waits until the fade finishes
        yield return new WaitForSeconds(waitFade);

        yield return new WaitForSeconds(0.5f);

        // Re-enables the players character controller to allow them to move again
        charControl.enabled = true;

        // Resets the players dash if said co-routine was interuptted
        dashRest();

        isRespawning = false;
        unFading = true;
    }

    public IEnumerator Fade()
    {

        isFading = true;

        // Waits until the fade begins
        yield return new WaitForSeconds(waitFade); 

        unFading = true;
    }

    // Function to control respawn position in relation to checkpoints, takes in a position value depending on the checkpoints position
    public void setCheckpoint(Vector3 newCheckpoint)
    {
        // Updates respawn location and plays sound
        if (respawnPoint != newCheckpoint)
        {
            checkpointSoundEffect.Play();
        }
        respawnPoint = newCheckpoint;
    }

    // Function used to reset the players dash 
    private void dashRest()
    {
        playerCharacter.GetComponent<PlayerController>().canDash = true;
        playerCharacter.GetComponent<PlayerController>().elapsedTime = 0;
        playerCharacter.GetComponent<PlayerController>().moveSpeed = playerCharacter.GetComponent<PlayerController>().originalMoveSpeed;
        playerCharacter.GetComponent<PlayerController>().StelCrystal.fillAmount = 1;
    }

    // Function used to teleport the player
    public void Teleport()
    {

        // Enables the player character, allowing it to be accessed 
        playerCharacter.gameObject.SetActive(true);

        // Disables the CharacterController to allow the respawn to reposition the player
        charControl.enabled = false;

        // Set player position to respawn point 
        playerCharacter.transform.position = respawnPoint;
        Health = maxHealth;

        // Re-enables the players character controller to allow them to move again
        charControl.enabled = true;
    }

    // When a scene is loaded, change the players spawn position and reset deaths
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Realm Of Time")
        {
            RoT();
        }

        else if (scene.name == "RoTComplete")
        {
            RoTC();
        }

        else if (scene.name == "The Interstice")
        {
            Interstice();
        }

        else if (scene.name == "Beginning Sequence")
        {
            respawnPoint = new Vector3(-131, 3, -146.5f);
            Teleport();
        }

        deaths = 0;
        deathText.text = string.Format("x {0}", deaths);
    }

    public void RoT()
    {
        respawnPoint = new Vector3(0, 3, 0);
        Teleport();
    }

    public void RoTC()
    {
        respawnPoint = new Vector3(0, 3, -150);
        Teleport();
    }
    public void Interstice()
    {
        respawnPoint = new Vector3(30, 3, 10);
        Teleport();
    }
}
