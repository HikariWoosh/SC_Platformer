using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doorSceneTransition : MonoBehaviour
{
    [Header("Door Controls")]

    [SerializeField]
    private Animator transition; // Reference to the transition animator

    [SerializeField]
    private float transitionTime; // Time used to control transition time

    void Start()
    {
        // Cache the transition animator
        transition = GameObject.Find("SceneTransition").GetComponent<Animator>();

    }

    // When a collider triggers collision with this object
    private void OnTriggerEnter(Collider other)
    {
        // If the collider is the player
        if (other.gameObject.tag == "Player") 
        {
            StartCoroutine(LoadLevel("Realm Of Time"));
        }
    }

    // Coroutine to load new scene
    private IEnumerator LoadLevel(string Level)
    {
        // Begins the transition and loads a new scene
        transition.SetBool("Fade", true);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(Level);

        transition.SetBool("Fade", false);
    }
}
