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
    
    
    //todo: put this class in Client Game Logic...
    
    /// <summary>
    /// Look for the game object that holds the player list entries.
    /// Assign prefab of player list entry to static variable.
    /// </summary>
    public void Start()
    {
        scrollViewContent = GameObject.Find("Canvas/Scroll View/Viewport/Content");
        _playerListItemPrefab = playerListItemPrefab; // weird workaround to get a static prefab reference
    }

    
    /// <summary>
    /// Add a player list entry to the lobby.
    /// If a player already exists, its values are updated.
    /// </summary>
    /// <param name="playerName">Name of the player</param>
    /// <param name="playerColor">Color of the player</param>
    public static void representNewPlayer(string playerName, Color playerColor)
    {
        GameObject listItem = GameObject.Find(playerName); // search for already existing list entries.
        
        if (listItem == null) // If the list entry for a player doesn't exist --> instantiate new.
        {
            listItem = Instantiate(_playerListItemPrefab, scrollViewContent.transform);
            listItem.transform.Find("No.").GetComponent<Text>().text = playerNumber.ToString();
            playerNumber++;
            listItem.transform.Find("Player").GetComponent<Text>().text = playerName;
            listItem.transform.Find("Color").GetComponent<Image>().color = playerColor;
            listItem.name = playerName;
        }
        else // List entry does already exist --> update name and color 
        {
            listItem.transform.Find("Player").GetComponent<Text>().text = playerName;
            listItem.transform.Find("Color").GetComponent<Image>().color = playerColor;
        }
    }
}
