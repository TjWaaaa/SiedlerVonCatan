using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TradeButton : MonoBehaviour
{
    
    [FormerlySerializedAs("resource")] public RESOURCETYPE resourcetype;
    private Boolean isClicked = false;

    //which resources are selected -> only one on each side!
    private static RESOURCETYPE offerResourcetype = RESOURCETYPE.NONE;
    private static RESOURCETYPE expectResourcetype = RESOURCETYPE.NONE;

    void Start()
    {
        
        // get the resource of each button individually
        resourcetype = (RESOURCETYPE)Enum.Parse(typeof(RESOURCETYPE), gameObject.name, true);
        
    }

    public String clickButton()
    {
        if (!isClicked)
        {
            if (gameObject.CompareTag("giveResource"))
            {
                if (offerResourcetype == RESOURCETYPE.NONE)
                {
                    offerResourcetype = resourcetype;
                    gameObject.transform.GetChild(1).SetAsFirstSibling();
                    isClicked = true;
                }
                return offerResourcetype.ToString().ToLower();
            }
            else //if (gameObject.CompareTag("getResource"))
            {
                if (expectResourcetype == RESOURCETYPE.NONE)
                {
                    expectResourcetype = resourcetype;
                                    gameObject.transform.GetChild(1).SetAsFirstSibling();
                                    isClicked = true;
                }
                return expectResourcetype.ToString().ToLower();
            }
            
        }
        else
        {
            gameObject.transform.GetChild(1).SetAsFirstSibling();
            isClicked = false;
            if (gameObject.CompareTag("giveResource")) offerResourcetype = RESOURCETYPE.NONE;
            else expectResourcetype = RESOURCETYPE.NONE;
            return "";
        }
    }

    public void reset()
    {
        if (isClicked)
        {
            gameObject.transform.GetChild(1).SetAsFirstSibling();
        }
        isClicked = false;
        offerResourcetype = RESOURCETYPE.NONE;
        expectResourcetype = RESOURCETYPE.NONE;
    }

    public static RESOURCETYPE getGetResource()
    {
        return expectResourcetype;
    }
    public static RESOURCETYPE getGiveResource()
    {
        return offerResourcetype;
    }

    public static Boolean isValidTradeRequest()
    {
        if (expectResourcetype != RESOURCETYPE.NONE && offerResourcetype != RESOURCETYPE.NONE) return true;
        else return false;
    }

}

