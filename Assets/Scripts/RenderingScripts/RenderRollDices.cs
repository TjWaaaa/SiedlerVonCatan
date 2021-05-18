using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderRollDices : MonoBehaviour
{
    private DiceHandler Dice1;
    private DiceHandler Dice2;
    private int[] diceNumbers = {-1,-1};

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeYield());
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
            renderRollDices(new int[] {6,6});
        }
    }

    public void renderRollDices(int[] numbers)
    {
        Dice1.GetComponent<Animator>().enabled = true;
        Dice2.GetComponent<Animator>().enabled = true;
        diceNumbers = numbers;
        TimeYield();
    }

    public IEnumerator TimeYield()
    {
        yield return new WaitForSeconds(2);
        Dice1.updateDiceNumber(diceNumbers[0]);
        Dice2.updateDiceNumber(diceNumbers[1]);
    }

}
