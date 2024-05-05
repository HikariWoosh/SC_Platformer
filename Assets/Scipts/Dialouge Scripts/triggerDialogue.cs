using UnityEngine;
using UnityEngine.SceneManagement;

public class triggerDialogue : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private GameObject dialougeHolder; // Reference to the object which holds the dialouge objects

    [SerializeField]
    private GameObject dialogueObject; // Reference to the dialouge object to be triggered


    [Header("Controls")]
    [SerializeField]
    private bool readCheck; // Bool to check if the dialouge has been read


    private void Start()
    {
        // Caches game objects
        dialougeHolder = GameObject.Find("Dialogue Boxes");
        readCheck = dialogueObject.GetComponent<dialogueScript>().dialogueRead;
    }

    // Function that controls dialouge output when collision zone is entered
    private void OnTriggerEnter(Collider other)
    {
        // If the collider is the player check if the dialouge has been read
        if (other.gameObject.tag == "Player")
        {
            if (!readCheck)
            {
                foreach(Transform child in dialougeHolder.transform)
                {
                    child.gameObject.SetActive(false);
                }

                readCheck = true;
                
                // Checks if the dialouge for the interstice has been read
                if(SceneManager.GetActiveScene().name == "The Interstice")
                {

                    if (PlayerPrefs.HasKey("intersticeRead") && PlayerPrefs.GetInt("intersticeRead") == 1)
                    {
                        dialogueObject.SetActive(false);
                    }
                    else
                    {
                        dialogueObject.SetActive(true);
                    }
                }

                // Checks if the dialouge for the realm of time has been read
                else if (SceneManager.GetActiveScene().name == "Realm Of Time")
                {

                    if (PlayerPrefs.HasKey("RoTRead") && PlayerPrefs.GetInt("RoTRead") == 1)
                    {
                        dialogueObject.SetActive(false);
                    }
                    else
                    {
                        dialogueObject.SetActive(true);
                    }
                }

                else
                {
                    dialogueObject.SetActive(true);
                }

                // Updates the players save based on what dialouge has been interacted with
                if (SceneManager.GetActiveScene().name == "The Interstice" && PlayerPrefs.GetInt("intersticeRead") == 0)
                {
                    PlayerPrefs.SetInt("intersticeRead", 1);
                }

                if (SceneManager.GetActiveScene().name == "Realm Of Time" && PlayerPrefs.GetInt("RoTRead") == 0)
                {
                    PlayerPrefs.SetInt("RoTRead", 1);
                }
            }
        }
    }
}
