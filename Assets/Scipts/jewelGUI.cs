using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jewelGUI : MonoBehaviour
{
    [SerializeField]
    private GameObject jewelManager;

    [SerializeField]
    private GameObject jewelSlot;

    [SerializeField]
    private int jewelAmount;

    [SerializeField]
    private int slotNumber;

    [SerializeField]
    private List<GameObject> jewelSlots = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        Invoke("countJewels", 0.2f);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void countJewels()
    {
        jewelManager = GameObject.Find("Jewels");
        if (jewelManager != null)
        {
            jewelAmount = jewelManager.GetComponent<JewelManager>().maxJewelCount;
            CreateJewelSlots(jewelAmount);
        }
    }

    void CreateJewelSlots(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject slot = Instantiate(jewelSlot, transform);
            float xPos = i * 100; 
            slot.transform.localPosition = new Vector3(xPos, 0, 0);
            jewelSlots.Add(slot);
        }
    }

    void removeJewelSlots()
    {
        foreach (var slot in jewelSlots)
        {
            Destroy(slot);
        }
        jewelSlots.Clear();
    }


    public void SlotSelect()
    {
        if (slotNumber >= 0 && slotNumber < jewelSlots.Count)
        {
            gemCollected(jewelSlots[slotNumber].transform);
            slotNumber += 1;
        }
    }

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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        slotNumber = 0;
        Invoke("removeJewelSlots", 0.01f);
        if (scene.name != "Main Menu" && scene.name != "The Interstice" && scene.name != "RoTComplete")
        {
            Invoke("countJewels", 0.1f);
        }
    }
}
