using System;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using Networking.ClientSide;
using Player;

namespace UI
{
    public class PlayerRepresentation
    {
        private GameObject[] playerBoardLights = new GameObject[4]; 

        
        private List<GameObject> playerRepresentations = new List<GameObject>();
        

        public void represent(RepresentativePlayer[] representativePlayers)
        {
            
            // find all Playerboards and set them inactive
                     
            for (int playerRepresentation = 0; playerRepresentation < 4; playerRepresentation++)
            {
                playerRepresentations.Add(GameObject.Find("Player"+(playerRepresentation+1)));
                playerBoardLights[playerRepresentation] = GameObject.Find("Player" + (playerRepresentation+1)+ "/PlayerRepresentation/Board_light");
                playerRepresentations[playerRepresentation].SetActive(false);

            }
            

            // represent all players
                     
            for (int player = 0; player < representativePlayers.Length; player++)
            {
                playerRepresentations[player].SetActive(true);
                playerRepresentations[player].transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = representativePlayers[player].getPlayerName();
                playerRepresentations[player].transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = representativePlayers[player].getPlayerColor();
                Debug.Log("CLIENT: " + representativePlayers[player] + " wrote on board number "+ (player+1));
                updateUiPR(player, representativePlayers[player]);
            }
            
            // delete PLayerboards which aren't ingame

            for (int playerboards = 4; playerboards > representativePlayers.Length; playerboards--)
            {
                playerRepresentations.RemoveAt(playerboards - 1);
            }

            Debug.Log("CLIENT/SERVER: There are "+ playerRepresentations.Count + " Players ingame");
        }
        
        // player = index in representativePlayers

        public void updateUiPR(int player, RepresentativePlayer representativePlayer)
        {   
            Debug.Log("CLIENT: RPUi has been updated via PlayerRepresentation.cs");
            playerRepresentations[player].transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = representativePlayer.getVictoryPoints().ToString();
            playerRepresentations[player].transform.GetChild(0).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = representativePlayer.getTotalResourceAmount().ToString();
            playerRepresentations[player].transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = representativePlayer.getDevCardAmount().ToString();
        }

        public void showNextPlayer(int nextPlayer)
        {
            playerBoardLights[0].transform.SetAsFirstSibling();
            playerBoardLights[1].transform.SetAsFirstSibling();
            playerBoardLights[2].transform.SetAsFirstSibling();
            playerBoardLights[3].transform.SetAsFirstSibling();

            playerBoardLights[nextPlayer].transform.SetSiblingIndex(1);
            //GameObject.Find("Player" + (nextPlayer +1)+ "/PlayerRepresentation/Board_light").
        }
    }
}