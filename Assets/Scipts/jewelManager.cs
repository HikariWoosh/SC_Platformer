using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private InGameMenu gameMenu;

    [SerializeField]
    private timeSlow timeSlow;

    // Start is called before the first frame update
    void Start()
    {
        gameMenu = GameObject.Find("UI").GetComponent<InGameMenu>();
        timeSlow = GameObject.Find("gameManager").GetComponent<timeSlow>();

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
        if (SceneManager.GetActiveScene().name == "Realm Of Time")
        {
            if(timeSlow.slowTimeUnlocked != true)
            {
                gameMenu.RoTC();
            }
            else
            {
                gameMenu.Interstice();
            }
        }
    }
}
