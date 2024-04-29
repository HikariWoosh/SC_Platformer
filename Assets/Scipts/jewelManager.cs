using System.Collections.Generic;
using UnityEngine;

public class JewelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> jewels = new List<GameObject>();

    [SerializeField]
    public int maxJewelCount;

    [SerializeField]
    private int jewelCount;

    [SerializeField]
    private AudioSource collectSound;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            GameObject jewelObject = child.gameObject;
            jewels.Add(jewelObject);
     
        }

        maxJewelCount = jewels.Count;
        jewelCount = jewels.Count;
    }

    public void jewelCollected(GameObject Collected)
    {
        if (Collected != null && jewels.Contains(Collected))
        {
            collectSound.Play();

            jewels.Remove(Collected);

            jewelCount = jewels.Count;
        }

        if (jewelCount == 0)
        {
            allJewelsCollected();
        }
    }

    public void allJewelsCollected()
    {
        Debug.Log("All Jewels Collected");
    }
}
