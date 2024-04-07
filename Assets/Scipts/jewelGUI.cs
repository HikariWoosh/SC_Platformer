using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jewelGUI : MonoBehaviour
{
    [SerializeField]
    private GameObject jewelManager;

    [SerializeField]
    private int jewelAmount;

    // Start is called before the first frame update
    void Start()
    {
        jewelAmount = jewelManager.GetComponent<JewelManager>().maxJewelCount;

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
