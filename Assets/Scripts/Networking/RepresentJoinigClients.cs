using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepresentJoinigClients : MonoBehaviour
{
    public static GameObject PlayerListItem;
    private static GameObject scrollViewContent;
    private static int playerNumber = 1;
    
    public void Start()
    {
        scrollViewContent = GameObject.Find("Canvas/Scroll View/Viewport/Content");
    }

    public static void representNewPlayer(string playerName, Color playerColor)
    {
        GameObject listItem = Instantiate(PlayerListItem, scrollViewContent.transform);
        listItem.transform.Find("No.").GetComponent<Text>().text = playerNumber.ToString();
        playerNumber++;
        listItem.transform.Find("Player").GetComponent<Text>().text = playerName;
        listItem.transform.Find("Color").GetComponent<Image>().color = playerColor;
    }
}
