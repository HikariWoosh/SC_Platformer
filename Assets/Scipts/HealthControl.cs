using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private int Health;

    [SerializeField]
    private bool isRespawning;

    [SerializeField]
    private Vector3 respawnPoint;

    [SerializeField]
    private PlayerController playerCharacter;

    [SerializeField]
    private float respawnLength;

    [SerializeField]
    private GameObject deathParticles;

    [Header("Fading")]
    [SerializeField]
    private Image blackScreen;

    [SerializeField]
    private bool isFading;

    [SerializeField]
    private bool unFading;

    [SerializeField]
    private float fadeSpeed;

    [SerializeField]
    private float waitFade;

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;

        respawnPoint = playerCharacter.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(blackScreen.color.a == 1f)
            {
                isFading = false;
            }
        }

        if (unFading)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 0f)
            {
                unFading = false;
            }
        }
    }

    public void damagePlayer(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            respawn();
        }
    }

    public void healPlayer(int heal)
    {
        Health += heal;

        if(Health >  maxHealth)
        {
            Health = maxHealth;
        }
    }

    public void respawn()
    {
        // Prevents the player from dying twice if two instances of damage are taken
        if (!isRespawning)
        {
            StartCoroutine("reCo");
        }
    }

    public IEnumerator reCo()
    {
        isRespawning = true;
        playerCharacter.gameObject.SetActive(false);
        Instantiate(deathParticles, playerCharacter.transform.position, playerCharacter.transform.rotation);
        yield return new WaitForSeconds(respawnLength);

        isFading = true;

        yield return new WaitForSeconds(waitFade);

        isRespawning = false;

        playerCharacter.gameObject.SetActive(true);
        CharacterController charControl = playerCharacter.GetComponent<CharacterController>();
        charControl.enabled = false;

        // Set player position to respawn point
        playerCharacter.transform.position = respawnPoint;
        Health = maxHealth;

        charControl.enabled = true;

        yield return new WaitForSeconds(waitFade);

        unFading = true;
    }

}
