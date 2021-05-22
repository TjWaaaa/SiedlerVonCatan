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
    private static RESOURCETYPE _giveResourcetype = RESOURCETYPE.NONE;
    private static RESOURCETYPE _getResourcetype = RESOURCETYPE.NONE;

    void Start()
    {
        
        //get the resource of each button individually
        resourcetype = (RESOURCETYPE)Enum.Parse(typeof(RESOURCETYPE), gameObject.name, true);
        
    }

    public String clickButton()
    {
        if (!isClicked)
        {
            if (gameObject.CompareTag("giveResource"))
            {
                if (_giveResourcetype == RESOURCETYPE.NONE)
                {
                    _giveResourcetype = resourcetype;
                    gameObject.transform.GetChild(1).SetAsFirstSibling();
                    isClicked = true;
                }
                return _giveResourcetype.ToString().ToLower();
            }
            else //if (gameObject.CompareTag("getResource"))
            {
                if (_getResourcetype == RESOURCETYPE.NONE)
                {
                    _getResourcetype = resourcetype;
                                    gameObject.transform.GetChild(1).SetAsFirstSibling();
                                    isClicked = true;
                }
                return _getResourcetype.ToString().ToLower();
            }
            
        }
        else
        {
            gameObject.transform.GetChild(1).SetAsFirstSibling();
            isClicked = false;
            if (gameObject.CompareTag("giveResource")) _giveResourcetype = RESOURCETYPE.NONE;
            else _getResourcetype = RESOURCETYPE.NONE;
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
        _giveResourcetype = RESOURCETYPE.NONE;
        _getResourcetype = RESOURCETYPE.NONE;
    }

    public static RESOURCETYPE getGetResource()
    {
        return _getResourcetype;
    }
    public static RESOURCETYPE getGiveResource()
    {
        return _giveResourcetype;
    }

    public static Boolean isValidTradeRequest()
    {
        if (_getResourcetype != RESOURCETYPE.NONE && _giveResourcetype != RESOURCETYPE.NONE) return true;
        else return false;
    }

}

