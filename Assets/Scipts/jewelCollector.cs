using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jewelCollector : MonoBehaviour
{
    [SerializeField]
    private bool isCollected;

    [SerializeField]
    private JewelManager jewelManager;

    [SerializeField]
    private jewelGUI jewelGUI;

    [SerializeField]
    private SphereCollider SphereCollider;

    [SerializeField]
    private GameObject Jewel;

    [SerializeField]
    private GameObject collectParticles;

    void Start()
    {
        Invoke("findObjects", 0.01f);
    }

    void findObjects()
    {
        jewelManager = FindAnyObjectByType<JewelManager>(); // Caches the jewel manager
        jewelGUI = FindAnyObjectByType<jewelGUI>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && !isCollected)
        {
            Jewel.SetActive(false);
            jewelManager.jewelCollected(Jewel);
            jewelGUI.SlotSelect();

            GameObject Particles = Instantiate(collectParticles, Jewel.transform.position, Jewel.transform.rotation);
            Destroy(Particles, 1f);
        }
    }
}
