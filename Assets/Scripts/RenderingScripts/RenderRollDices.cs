using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRollDices : MonoBehaviour
{
    private DiceHandler Dice1;
    private DiceHandler Dice2;
    private int[] diceNumbers;

    // Start is called before the first frame update
    void Start()
    {
        DiceHandler[] diceDiceHandlers = GetComponentsInChildren<DiceHandler>();
        Debug.Log(diceDiceHandlers.Length);
        Dice1 = diceDiceHandlers[0];
        Dice2 = diceDiceHandlers[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space pressed");
            StartCoroutine(TimeYield());
            renderRollDices(new int[] {6,6});
        }
    }

    public void renderRollDices(int[] numbers)
    {
        Dice1.GetComponent<Animator>().enabled = true;
        Dice2.GetComponent<Animator>().enabled = true;
        diceNumbers = numbers;
        Debug.Log(diceNumbers.ToString());
        TimeYield();
        Debug.Log("Behind the TimeYieldCall");
    }

    public IEnumerator TimeYield()
    {
        Debug.Log("TimeYield Function is called");
        yield return new WaitForSeconds(2);
        Debug.Log("Waited 2 Seconds");
        Dice1.updateDiceNumber(diceNumbers[0]);
        Dice2.updateDiceNumber(diceNumbers[1]);
    }

}
