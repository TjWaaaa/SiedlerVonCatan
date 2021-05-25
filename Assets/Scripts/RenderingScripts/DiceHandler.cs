using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceHandler : MonoBehaviour
{
    private int diceNumber;

    private Animator dice;

    void Awake()
    {
        
    }
    void Start()
    {
        dice = gameObject.GetComponent<Animator>();
        dice.enabled = false;
    }
    public void updateDiceNumber(int number)
    {
        diceNumber = number;
    }

    public void checkForOne()
    {
        if (diceNumber == 1)
        {
            checkedCorrect();
        }
    }
    public void checkForTwo()
    {
        if (diceNumber == 2)
        {
            checkedCorrect();
        }
    }
    public void checkForThree()
    {
        if (diceNumber == 3)
        {
            checkedCorrect();
        }
    }
    public void checkForFour()
    {
        if (diceNumber == 4)
        {
            checkedCorrect();
        }
    }
    public void checkForFive()
    {
        if (diceNumber == 5)
        {
            checkedCorrect();
        }
    }
    public void checkForSix()
    {
        if (diceNumber == 6)
        {
            checkedCorrect();
        }
    }

    private void checkedCorrect()
    {
        dice.enabled = false;
        diceNumber = -1;
        Debug.Log("Animating: " + dice.enabled);
        Debug.Log(gameObject.GetComponent<Image>().sprite);
    }
}
