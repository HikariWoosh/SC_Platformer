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
    private SphereCollider SphereCollider;

    [SerializeField]
    private GameObject Jewel;

    [SerializeField]
    private GameObject collectParticles;


    // Start is called before the first frame update
    void Start()
    {
        jewelManager = FindAnyObjectByType<JewelManager>(); // Finds the health controller
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && !isCollected)
        {
            GameObject Particles = Instantiate(collectParticles, Jewel.transform.position, Jewel.transform.rotation);
            Jewel.SetActive(false);
            jewelManager.jewelCollected(Jewel);
        }
    }
}
