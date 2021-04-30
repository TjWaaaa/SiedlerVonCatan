using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ResourceType;

using UnityEngine.UI;

public class TradeButton : MonoBehaviour
{

    public RESOURCETYPE resource;
    private Boolean isClicked = false;

    //which resources are selected -> only one on each side!
    private static RESOURCETYPE giveResource = RESOURCETYPE.NONE;
    private static RESOURCETYPE getResource = RESOURCETYPE.NONE;

    void Start()
    {
        //get the resource of each button individually
        resource = (RESOURCETYPE)Enum.Parse(typeof(ResourceType.RESOURCETYPE), gameObject.name, true);
    }

    public void clickButton()
    {
        if (!isClicked)
        {
            if (gameObject.CompareTag("giveResource") && giveResource == RESOURCETYPE.NONE)
            {
                giveResource = resource;
                gameObject.GetComponent<Image>().color = Color.red;
                isClicked = true;

            }
            else if (gameObject.CompareTag("getResource") && getResource == RESOURCETYPE.NONE)
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
            if (gameObject.CompareTag("giveResource")) giveResource = RESOURCETYPE.NONE;
            else getResource = RESOURCETYPE.NONE;
        }
    }

    public void reset()
    {
        gameObject.GetComponent<Image>().color = Color.white;
        isClicked = false;
        giveResource = RESOURCETYPE.NONE;
        getResource = RESOURCETYPE.NONE;
    }

    public static RESOURCETYPE getGetResource()
    {
        return getResource;
    }
    public static RESOURCETYPE getGiveResource()
    {
        return giveResource;
    }

    public static Boolean isValidTradeRequest()
    {
        if (getResource != RESOURCETYPE.NONE && giveResource != RESOURCETYPE.NONE) return true;
        else return false;
    }

}

