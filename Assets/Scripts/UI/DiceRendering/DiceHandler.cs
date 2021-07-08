using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceHandler : MonoBehaviour
{
    private String diceNumber = "";

    private Animator diceAnimator;
    private Image diceImage;

    private String[] diceNumberStrings = { "0001 (UnityEngine.Sprite)", "0020 (UnityEngine.Sprite)", "0040 (UnityEngine.Sprite)", "0060 (UnityEngine.Sprite)", "0080 (UnityEngine.Sprite)", "0100 (UnityEngine.Sprite)" };

    void Start()
    {
        diceAnimator = gameObject.GetComponent<Animator>();
        diceAnimator.enabled = false;
        diceImage = gameObject.GetComponent<Image>();
    }
    public void updateDiceNumber(int number)
    {
        diceNumber = diceNumberStrings[number - 1];
    }

    void FixedUpdate()
    {
        if (diceAnimator.enabled)
        {
            if (diceNumber == diceImage.sprite.ToString())
            {
                diceAnimator.enabled = false;
                diceNumber = "";
                Debug.Log("SERVER: Animating: " + diceAnimator.enabled);
            }
        }
    }
}
