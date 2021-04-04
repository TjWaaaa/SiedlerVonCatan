using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
    public Camera cam;
    public GameObject Dice1;
    public GameObject Dice2;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.Log(Physics.Raycast(ray, out hit, 100.0f));

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("hit collider name: " + hit.collider.name);
                Debug.Log("hit collider is from DiceScript: " + hit.collider.GetComponent<DiceScript>());
                if (hit.collider.GetComponent<DiceScript>())
                {                        
                    ThrowDices();
                }

            }
        }
    }


    public void ThrowDices()
    {
        Debug.Log("Entered the ThowDice function");
        var firstNumber = Random.Range(1,7);
        Debug.Log("firstNumber " + firstNumber);
        SetDiceRotation(Dice1,firstNumber);

        var secondNumber = Random.Range(1,7);
        Debug.Log("secondNumber " + secondNumber);
        SetDiceRotation(Dice2,secondNumber);
    }

    void SetDiceRotation(GameObject Dice,int number)
    {
        switch(number)
        {
            case 1: Dice.transform.rotation = Quaternion.Euler(0,0,0);
                break; 
            case 2: Dice.transform.rotation = Quaternion.Euler(0,0,90);
                break;
            case 3: Dice.transform.rotation = Quaternion.Euler(270,0,0);
                break;
            case 4: Dice.transform.rotation = Quaternion.Euler(90,0,0);
                break;
            case 5: Dice.transform.rotation = Quaternion.Euler(0,0,-90);
                break;
            case 6: Dice.transform.rotation = Quaternion.Euler(180,0,0);
                break;
        }
    }
}
