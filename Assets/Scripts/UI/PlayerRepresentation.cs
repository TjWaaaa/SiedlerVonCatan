using System;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerRepresentation : MonoBehaviour
    {
        //private PrefabFactory prefabFactory;
        private GameObject allPlayers;
        private GameObject[] playerRepresentations = new GameObject[4];

        public void Start()
        {
            allPlayers = GameObject.Find("AllPlayers");

            // set all Playerboards inactive
            for (int playerRepresentation = 0; playerRepresentation < playerRepresentations.Length; playerRepresentation++)
            {
                playerRepresentations[playerRepresentation] = GameObject.Find("Player"+(playerRepresentation+1));
                playerRepresentations[playerRepresentation].SetActive(false);

            }

            // represent all players
            for (int player = 0; player < GameController.representativePlayers.Count; player++)
            {
                playerRepresentations[player].SetActive(true);
                GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/PlayerName").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getPlayerName();
                GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/PlayerName").GetComponent<TextMeshProUGUI>().color = GameController.representativePlayers[player].getPlayerColor();
                Debug.Log(GameController.representativePlayers[player] + " writed on board number "+ (player+1));
                GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/VictoryPoints").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getVictoryPoints().ToString();
                GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/TotalResourceAmount").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getTotalResourceAmount().ToString();
                GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/DevCards").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getDevCardAmount().ToString();
            }
        }

        
        // playerNumber = index in representativePlayers
        public void updateVictoryPoints(int playerNumber, int victoryPoints)
        {
            GameObject.Find("Player" + playerNumber + "/PlayerRepresentation/VictoryPoints").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[playerNumber].getVictoryPoints().ToString();

        }
        
        public void updateTotalResourceAmount(int playerNumber, int totalResourceAmount)
        {
            GameObject.Find("Player" + playerNumber + "/PlayerRepresentation/TotalResourceAmount").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[playerNumber].getTotalResourceAmount().ToString();

        }
        
        public void updateDevCardAmount(int playerNumber, int devCardsAmount)
        {
            GameObject.Find("Player" + playerNumber + "/PlayerRepresentation/DevCards").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[playerNumber].getDevCardAmount().ToString();

        }
    }
}