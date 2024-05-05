using UnityEngine;
using TMPro;


public class keypadScript : MonoBehaviour
{
    [Header("Keypad Settings")]

    [SerializeField]
    private string Code; // The correct code

    [SerializeField]
    private string Number; // String of numbers the user inputs

    [SerializeField]
    private int NumberIndex; // The amount of numbers inputted by the user

    [SerializeField]
    private TMP_Text codeText; // Text displayed in the keypad screen

    [SerializeField]
    private Animator boxAnimator; // The animator used for the glass box

    [SerializeField]
    private keypadOverworld overworldKeypad; // Reference to the overworldKeypad

    private void Start()
    {
        // Caches the overworldKeypad
        overworldKeypad = FindAnyObjectByType<keypadOverworld>();
    }

    public void keypadCode(string Input)
    {
        // If the amount of digits is 5 or less
        if (NumberIndex <= 4)
        {
            // Increase the number index and add the new number to the code
            NumberIndex += 1;
            Number = Number + Input;
            codeText.text = Number;
        }
    }

    public void enterCode()
    {
        // If the number is equal to the code, release the Jewel and close the keypad, else clear the code
        if (Number == Code)
        {
            boxAnimator.SetTrigger("Up");
            overworldKeypad.closeKeypad();
        }
        else
        {
            clearCode();
        }
    }

    public void clearCode()
    {
        // Resets the index, text, and inputted code
        NumberIndex = 0;
        Number = null;
        codeText.text = Number;
    }

}
