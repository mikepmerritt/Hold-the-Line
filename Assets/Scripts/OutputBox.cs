using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OutputBox : MonoBehaviour
{

    private TMP_Text Textbox;
    private string Message;

    private void Start()
    {
        Textbox = GetComponent<TMP_Text>();
    }

    public void ShowText(string message)
    {
        Textbox.color = Color.white;
        Message = message;
        Textbox.text = Message;
    }

    public void ShowWarning(string message)
    {
        Textbox.color = Color.yellow;
        Message = message;
        Textbox.text = Message;
    }

    public void ShowError(string message)
    {
        Textbox.color = Color.red;
        Message = message;
        Textbox.text = Message;
    }

    public void ShowText()
    {
        Textbox.text = Message;
    }

    public void HideText()
    {
        Textbox.text = "";
    }

}