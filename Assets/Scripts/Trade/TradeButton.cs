using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Resource;

using UnityEngine.UI;

public class TradeButton : MonoBehaviour
{
    
    public RESOURCE resource;
    private Boolean isClicked = false;

    void Start()
    {
        resource = (RESOURCE) Enum.Parse(typeof(Resource.RESOURCE), gameObject.name, true);
    }
    
    public void clickButton()
    {
        if (!isClicked)
        {
            if (gameObject.CompareTag("giveResource"))
            {
                gameObject.GetComponent<Image>().color = Color.red;

            }
            else 
                gameObject.GetComponent<Image>().color = Color.green;
            isClicked = true;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
            isClicked = false;
        }
    }

    public void reset()
    {
        gameObject.GetComponent<Image>().color = Color.white;
        isClicked = false;
    }
    
    
}

