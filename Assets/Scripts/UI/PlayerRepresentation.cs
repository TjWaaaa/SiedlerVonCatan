using System;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerRepresentation : MonoBehaviour
    {
        
        private List<GameObject> playerRepresentations = new List<GameObject>();

        public void Start()
        {

            // find all Playerboards and set them inactive
            
            for (int playerRepresentation = 0; playerRepresentation < 4; playerRepresentation++)
            {
                playerRepresentations.Add(GameObject.Find("Player"+(playerRepresentation+1)));
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
            
            // delete PLayerboards which aren't ingame

            for (int playerboards = 4; playerboards > GameController.representativePlayers.Count; playerboards--)
            {
                playerRepresentations.RemoveAt(playerboards - 1);
            }
            Debug.Log("There are "+ playerRepresentations.Count + " Players ingame");
        }

        
        // player = index in representativePlayers
        public static void updateVictoryPoints(int player)
        {
            GameObject.Find("Player" + (player +1) + "/PlayerRepresentation/VictoryPoints").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getVictoryPoints().ToString();

        }
        
        public static void updateTotalResourceAmount(int player)
        {
            GameObject.Find("Player" + (player +1) + "/PlayerRepresentation/TotalResourceAmount").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getTotalResourceAmount().ToString();

        }
        
        public static void updateDevCardAmount(int player)
        {
            GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/DevCards").GetComponent<TextMeshProUGUI>().text = GameController.representativePlayers[player].getDevCardAmount().ToString();

        }

        public static void showNextPlayer(int player, int nextPlayer)
        {
            GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/Board_light").transform.SetAsFirstSibling();
            GameObject.Find("Player" + (nextPlayer +1)+ "/PlayerRepresentation/Board_light").transform.SetSiblingIndex(1);
        }
    }
}