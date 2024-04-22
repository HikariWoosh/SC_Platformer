using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class keypadScript : MonoBehaviour
{
    [SerializeField]
    private string Code;

    [SerializeField]
    private string Number;

    [SerializeField]
    private int NumberIndex;

    [SerializeField]
    private TMP_Text codeText;

    [SerializeField]
    private Animator boxAnimator;

    [SerializeField]
    private keypadOverworld overworldKeypad;

    private void Start()
    {
        overworldKeypad = FindAnyObjectByType<keypadOverworld>();
    }

    public void keypadCode(string Input)
    {
        if (NumberIndex <= 4)
        {
            NumberIndex += 1;
            Number = Number + Input;
            codeText.text = Number;
        }
    }

    public void enterCode()
    {
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
        NumberIndex = 0;
        Number = null;
        codeText.text = Number;
    }

}
