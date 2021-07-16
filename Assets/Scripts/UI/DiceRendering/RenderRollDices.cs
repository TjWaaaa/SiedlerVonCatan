using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRollDices : MonoBehaviour
{
    private DiceHandler Dice1;
    private DiceHandler Dice2;
    private int[] diceNumbers;

    // Start is called before the first frame update
    /// <summary>
    /// Grabs the DiceHandler object and sorts it into the array
    /// </summary>
    void Start()
    {
        DiceHandler[] diceDiceHandlers = GetComponentsInChildren<DiceHandler>();
        Debug.Log("CLIENT: diceDiceHandlers " + diceDiceHandlers.Length);
        Dice1 = diceDiceHandlers[0];
        Dice2 = diceDiceHandlers[1];
    }

    /// <summary>
    /// Enables the animation.
    /// Calls a coroutine to let the animation go on for atleast a short amount of time.
    /// </summary>
    public void renderRollDices(int[] numbers)
    {
        Dice1.GetComponent<Animator>().enabled = true;
        Dice2.GetComponent<Animator>().enabled = true;
        diceNumbers = numbers;
        Debug.Log("CLIENT: " + diceNumbers[0] + " " + diceNumbers[1]);
        StartCoroutine(TimeYield());
        TimeYield();
    }

    /// <summary>
    /// Waits half a second to set the numbers into the dice.
    /// </summary>
    public IEnumerator TimeYield()
    {
        Debug.Log("CLIENT: TimeYield Function is called");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("CLIENT: Waited 2 Seconds");
        Dice1.updateDiceNumber(diceNumbers[0]);
        yield return new WaitForSeconds(0);
        Dice2.updateDiceNumber(diceNumbers[1]);
    }
}
