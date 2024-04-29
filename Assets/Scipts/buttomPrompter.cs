using UnityEngine;

public class buttomPrompter : MonoBehaviour
{

    [SerializeField]
    private GameObject gPrompt;

    [SerializeField]
    private GameObject ePrompt;

    public void showG()
    {
        gPrompt.SetActive(true);
    }
    public void showE()
    {
        ePrompt.SetActive(true);
    }

    public void hidePrompts()
    {
        gPrompt.SetActive(false);
        ePrompt.SetActive(false);
    }
}
