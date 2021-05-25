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
       
        private GameObject[] playerRepresentations = new GameObject[4];

        public void Start()
        {

            // find all Playerboards and set them inactive
            
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
                updateVictoryPoints(player);
                updateTotalResourceAmount(player);
                updateDevCardAmount(player);
            }
        }

        
        // player = index in representativePlayers
        public void updateVictoryPoints(int player)
        {
            GameObject.Find("Player" + (player +1) + "/PlayerRepresentation/VictoryPoints").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getVictoryPoints().ToString();

        }
        
        public void updateTotalResourceAmount(int player)
        {
            GameObject.Find("Player" + (player +1) + "/PlayerRepresentation/TotalResourceAmount").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getTotalResourceAmount().ToString();

        }
        
        public void updateDevCardAmount(int player)
        {
            GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/DevCards").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getDevCardAmount().ToString();

        }
    }
}