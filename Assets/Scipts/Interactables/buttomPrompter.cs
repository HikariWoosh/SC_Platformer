using UnityEngine;

public class buttomPrompter : MonoBehaviour
{
    [Header("Game Objects")]

    [SerializeField]
    private GameObject gPrompt;

    [SerializeField]
    private GameObject ePrompt;

    // Functions to show different letters
    public void showG()
    {
        gPrompt.SetActive(true);
    }
    public void showE()
    {
        ePrompt.SetActive(true);
    }

    // Function to hide all prompts
    public void hidePrompts()
    {
        gPrompt.SetActive(false);
        ePrompt.SetActive(false);
    }
}
