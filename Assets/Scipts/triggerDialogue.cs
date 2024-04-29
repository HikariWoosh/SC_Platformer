using UnityEngine;

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
                dialogueObject.SetActive(true);
            }
            else
            {
                Debug.Log("Dialogue already read");
            }
            
        }
    }
}
