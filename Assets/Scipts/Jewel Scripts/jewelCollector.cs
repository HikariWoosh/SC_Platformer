using UnityEngine;

public class jewelCollector : MonoBehaviour
{
    [Header("Jewel Controls")]

    [SerializeField]
    private bool isCollected; // Checks if the Jewel has been obtained


    [Header("Game Objects")]

    [SerializeField]
    private JewelManager jewelManager; // Reference to the jewel manager

    [SerializeField]
    private jewelGUI jewelGUI;// Reference to the jewel GUI

    [SerializeField]
    private SphereCollider SphereCollider; // Hitbox of the jewel

    [SerializeField]
    private GameObject Jewel; // Jewel object

    [SerializeField]
    private GameObject collectParticles; // Particles when obtained

    void Start()
    {
        Invoke("findObjects", 0.01f);
    }

    void findObjects()
    {
        // Caches game objects
        jewelManager = FindAnyObjectByType<JewelManager>();
        jewelGUI = FindAnyObjectByType<jewelGUI>();
    }

    // Whent the player is the jewels hitbox
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && !isCollected)
        {
            // Collects the jewel disabling it and adding it to the jewel counter
            Jewel.SetActive(false);
            jewelManager.jewelCollected(Jewel);
            jewelGUI.SlotSelect();

            // Plays the particles 
            GameObject Particles = Instantiate(collectParticles, Jewel.transform.position, Jewel.transform.rotation);
            Destroy(Particles, 1f);
        }
    }
}
