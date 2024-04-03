using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class abilityCooldowns : MonoBehaviour
{
    [Header("Stel Gem")]
    [SerializeField]
    private Image StelGem;
    private float dashTime;
    private float duration;


    [Header("Game Objects")]
    [SerializeField]
    private PlayerController playerCharacter; // Refers to the players PlayerController



    // Start is called before the first frame update
    void Start()
    {
        StelGem.fillAmount = 0;
        duration = playerCharacter.GetComponent<PlayerController>().dashDuration;
    }

    // Update is called once per frame
    void Update()
    {
        dashTime = playerCharacter.GetComponent<PlayerController>().elapsedTime;
    }

    public void DashEffect()
    {
        while (StelGem.fillAmount != 1)
        {
            StelGem.fillAmount = dashTime / duration;
        }
    }
}
