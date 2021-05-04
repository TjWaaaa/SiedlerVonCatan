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

    //which resources are selected -> only one on each side!
    private static RESOURCE giveResource = RESOURCE.NONE;
    private static RESOURCE getResource = RESOURCE.NONE;

    void Start()
    {
        //get the resource of each button individually
        resource = (RESOURCE)Enum.Parse(typeof(Resource.RESOURCE), gameObject.name, true);
    }

    public void clickButton()
    {
        if (!isClicked)
        {
            if (gameObject.CompareTag("giveResource") && giveResource == RESOURCE.NONE)
            {
                giveResource = resource;
                gameObject.GetComponent<Image>().color = Color.red;
                isClicked = true;

            }
            else if (gameObject.CompareTag("getResource") && getResource == RESOURCE.NONE)
            {
                getResource = resource;
                gameObject.GetComponent<Image>().color = Color.green;
                isClicked = true;
            }

        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
            isClicked = false;
            if (gameObject.CompareTag("giveResource")) giveResource = RESOURCE.NONE;
            else getResource = RESOURCE.NONE;
        }
    }

    public void reset()
    {
        gameObject.GetComponent<Image>().color = Color.white;
        isClicked = false;
        giveResource = RESOURCE.NONE;
        getResource = RESOURCE.NONE;
    }

    public static RESOURCE getGetResource()
    {
        return getResource;
    }
    public static RESOURCE getGiveResource()
    {
        return giveResource;
    }

    public static Boolean isValidTradeRequest()
    {
        if (getResource != RESOURCE.NONE && giveResource != RESOURCE.NONE) return true;
        else return false;
    }

}

