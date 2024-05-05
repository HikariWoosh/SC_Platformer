using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jewelGUI : MonoBehaviour
{
    [Header("Jewel Controls")]
    [SerializeField]
    private GameObject jewelManager; // Reference to where the jewels are kept

    [SerializeField]
    private GameObject jewelSlot; // Reference to jewel slot

    [SerializeField]
    private int jewelAmount; // Amount of jewels

    [SerializeField]
    private int slotNumber; // Slot number

    [SerializeField]
    private List<GameObject> jewelSlots = new List<GameObject>(); // List of jewels


    // Start is called before the first frame update
    void Start()
    {
        Invoke("countJewels", 0.2f);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Function to count how many jewels are in the scene
    void countJewels()
    {
        jewelManager = GameObject.Find("Jewels");
        if (jewelManager != null)
        {
            // Gets total jewels and creates slots for that amount
            jewelAmount = jewelManager.GetComponent<JewelManager>().maxJewelCount;
            CreateJewelSlots(jewelAmount);
        }
    }

    // Creates jewel slots on the GUI
    void CreateJewelSlots(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // For each jewel create a new GUI element
            GameObject slot = Instantiate(jewelSlot, transform);
            float xPos = i * 100; 
            slot.transform.localPosition = new Vector3(xPos, 0, 0);
            jewelSlots.Add(slot);
        }
    }

    // Deletes jewels
    void removeJewelSlots()
    {
        foreach (var slot in jewelSlots)
        {
            Destroy(slot);
        }
        jewelSlots.Clear();
    }


    // Selects a specfic jewel slot
    public void SlotSelect()
    {
        if (slotNumber >= 0 && slotNumber < jewelSlots.Count)
        {
            gemCollected(jewelSlots[slotNumber].transform);
            slotNumber += 1;
        }
    }

    // Function when a jewel is collected
    public void gemCollected(Transform slot)
    {
        // Loop through children of the slot
        foreach (Transform child in slot)
        {
            // Find the jewelNot child
            if (child.name == "JewelNot")
            {
                // Disable the jewelNot child if it's not already disabled
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    // Run when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Resets the jewels 
        slotNumber = 0;
        Invoke("removeJewelSlots", 0.01f);

        // Depending on scene count the jewels
        if (scene.name != "Main Menu" && scene.name != "The Interstice" && scene.name != "RoTComplete")
        {
            Invoke("countJewels", 0.1f);
        }
    }
}
