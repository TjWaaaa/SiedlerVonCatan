using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepresentJoinigClients : MonoBehaviour
{
    private static GameObject _playerListItemPrefab;
    public GameObject playerListItemPrefab;
    private static GameObject scrollViewContent;
    private static int playerNumber = 1;
    
    public void Start()
    {
        scrollViewContent = GameObject.Find("Canvas/Scroll View/Viewport/Content");
        _playerListItemPrefab = playerListItemPrefab; // weird workaround to get a static prefab reference
    }

    public static void representNewPlayer(string playerName, Color playerColor)
    {
        GameObject listItem = Instantiate(_playerListItemPrefab, scrollViewContent.transform);
        listItem.transform.Find("No.").GetComponent<Text>().text = playerNumber.ToString();
        playerNumber++;
        listItem.transform.Find("Player").GetComponent<Text>().text = playerName;
        listItem.transform.Find("Color").GetComponent<Image>().color = playerColor;
    }
}
