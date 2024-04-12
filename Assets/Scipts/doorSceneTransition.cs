using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doorSceneTransition : MonoBehaviour
{
    [SerializeField]
    private Animator transition;

    [SerializeField]
    private float transitionTime;

    void Start()
    {
        transition = GameObject.Find("SceneTransition").GetComponent<Animator>();

    }

    // When a collider triggers collision with this object
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            StartCoroutine(LoadLevel("Realm Of Time"));
        }
    }

    private IEnumerator LoadLevel(string Level)
    {
        transition.SetBool("Fade", true);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(Level);

        transition.SetBool("Fade", false);
    }
}
