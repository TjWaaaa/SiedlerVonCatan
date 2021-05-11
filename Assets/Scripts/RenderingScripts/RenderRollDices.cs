using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRollDices : MonoBehaviour
{
    public Animation Dice1;
    public Animation Dice2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void renderRollDices(int firstNumber, int secondNumber)
    {
        Dice1.enabled = true;
        Dice2.enabled = true;
    }
}
