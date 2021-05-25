using System;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using Networking.ClientSide;

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
            
            for (int player = 0; player < ClientGameLogic.representativePlayers.Count; player++)
            {
                playerRepresentations[player].SetActive(true);
                GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/PlayerName").GetComponent<TextMeshProUGUI>().text = ClientGameLogic.representativePlayers[player].getPlayerName();
                GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/PlayerName").GetComponent<TextMeshProUGUI>().color = ClientGameLogic.representativePlayers[player].getPlayerColor();
                Debug.Log(ClientGameLogic.representativePlayers[player] + " writed on board number "+ (player+1));
                updateUiPR(player);
            }
            
            // delete PLayerboards which aren't ingame

            for (int playerboards = 4; playerboards > ClientGameLogic.representativePlayers.Count; playerboards--)
            {
                playerRepresentations.RemoveAt(playerboards - 1);
            }
            Debug.Log("There are "+ playerRepresentations.Count + " Players ingame");
        }

        
        // player = index in representativePlayers

        public static void updateUiPR(int player)
        {   
            Debug.Log("RPUi has been updated via PlayerRepresentation.cs");
            GameObject.Find("Player" + (player +1) + "/PlayerRepresentation/VictoryPoints").GetComponent<TextMeshProUGUI>().text = ClientGameLogic.representativePlayers[player].getVictoryPoints().ToString();
            GameObject.Find("Player" + (player +1) + "/PlayerRepresentation/TotalResourceAmount").GetComponent<TextMeshProUGUI>().text = ClientGameLogic.representativePlayers[player].getTotalResourceAmount().ToString();
            GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/DevCards").GetComponent<TextMeshProUGUI>().text = ClientGameLogic.representativePlayers[player].getDevCardAmount().ToString();
        }

        public static void showNextPlayer(int player, int nextPlayer)
        {
            GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/Board_light").transform.SetAsFirstSibling();
            GameObject.Find("Player" + (nextPlayer +1)+ "/PlayerRepresentation/Board_light").transform.SetSiblingIndex(1);
        }
    }
}