using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public Text inputFieldName;
    public Text nameDisplay;

    public void Text_Changed(string newText)
    {
        Debug.Log(inputFieldName.text);
        Debug.Log(nameDisplay.text);
        nameDisplay.text = 
        "Name: " + newText;
    }
}
