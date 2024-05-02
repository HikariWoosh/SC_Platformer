using UnityEngine;
using UnityEngine.SceneManagement;

public class triggerDialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject dialougeHolder;

    [SerializeField]
    private GameObject dialogueObject;

    [SerializeField]
    private bool readCheck;


    private void Start()
    {
        dialougeHolder = GameObject.Find("Dialogue Boxes");
        readCheck = dialogueObject.GetComponent<dialogueScript>().dialogueRead;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // If the collider is the player
        {
            if (!readCheck)
            {
                foreach(Transform child in dialougeHolder.transform)
                {
                    child.gameObject.SetActive(false);
                }

                readCheck = true;
                
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
