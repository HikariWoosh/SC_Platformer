using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Invoke("countJewels", 0.1f);

    }

    void countJewels()
    {
        jewelAmount = jewelManager.GetComponent<JewelManager>().maxJewelCount;
        CreateJewelSlots(jewelAmount);
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
}
