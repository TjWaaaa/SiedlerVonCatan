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


        /// <summary>
        /// Represents all player on boards
        /// </summary>
        /// <param name="representativePlayers">array of all players, which are ingame</param>
        public void represent(RepresentativePlayer[] representativePlayers)
        {
            // Find all Playerboards and set them inactive
            for (int playerRepresentation = 0; playerRepresentation < 4; playerRepresentation++)
            {
                playerRepresentations.Add(GameObject.Find("Player" + (playerRepresentation + 1)));
                playerBoardLights[playerRepresentation] = GameObject.Find("Player" + (playerRepresentation + 1) + "/PlayerRepresentation/Board_light");
                playerRepresentations[playerRepresentation].SetActive(false);
            }

            // Represent all players
            for (int player = 0; player < representativePlayers.Length; player++)
            {
                playerRepresentations[player].SetActive(true);
                playerRepresentations[player].transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = representativePlayers[player].getPlayerName();
                playerRepresentations[player].transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = representativePlayers[player].getPlayerColor();
                Debug.Log("CLIENT: " + representativePlayers[player] + " wrote on board number " + (player + 1));
                updateUiPR(player, representativePlayers[player]);
            }

            // Delete PLayerboards which aren't ingame
            for (int playerboards = 4; playerboards > representativePlayers.Length; playerboards--)
            {
                playerRepresentations.RemoveAt(playerboards - 1);
            }

            Debug.Log("CLIENT/SERVER: There are " + playerRepresentations.Count + " Players ingame");
        }


        /// <summary>
        /// Update Victorypoints, resources amount and devcard amount 
        /// </summary>
        /// <param name="player">index in playerRepresentations (playernumber -1)</param>
        /// <param name="representativePlayer">representativePlayer, which should be updated</param>
        public void updateUiPR(int player, RepresentativePlayer representativePlayer)
        {
            Debug.Log("CLIENT: RPUi has been updated via PlayerRepresentation.cs");
            playerRepresentations[player].transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = representativePlayer.getVictoryPoints().ToString();
            playerRepresentations[player].transform.GetChild(0).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = representativePlayer.getTotalResourceAmount().ToString();
            playerRepresentations[player].transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = representativePlayer.getDevCardAmount().ToString();
        }

        /// <summary>
        /// Show current player by setting the light_board in front of the normal.
        /// Change it back for the previous player.
        /// </summary>
        /// <param name="previousPlayer">board, which has to be dark</param>
        /// <param name="nextPlayer">board, which has to be light</param>
        public void showNextPlayer(int previousPlayer, int nextPlayer)
        {
            playerBoardLights[previousPlayer].transform.SetAsFirstSibling();
            playerBoardLights[nextPlayer].transform.SetSiblingIndex(1);
        }
    }
}