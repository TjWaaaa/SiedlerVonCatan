using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextPlaytime : MonoBehaviour
{
    public Text timerText;
    private DateTime dt;

    // Start is called before the first frame update
    void Start()
    {
        dt = System.DateTime.Now;
        Debug.Log("CLIENT: Current DateTime.Now: " + dt);
    }

    

    // Update is called on every frame
    void Update()
    {
        timerText.text = "Playtime: " + string.Format("{0:hh\\:mm\\:ss}", DateTime.Now - dt);
    }
}