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

            for (int playerRepresentation = 0; playerRepresentation < playerRepresentations.Length; playerRepresentation++)
            {
                playerRepresentations[playerRepresentation] = GameObject.Find("Player"+(playerRepresentation+1));
                playerRepresentations[playerRepresentation].SetActive(false);
                Debug.Log("Set Playerrepresentation " + playerRepresentation + " invisible" );
                
            }

            for (int player = 0; player < GameController.playerRep.Count; player++)
            {
                playerRepresentations[player].SetActive(true);
                Debug.Log("Set Playerrepresentation "+ (player +1)+ " visible");
                GameObject.Find("Player" + (player +1)+ "/PlayerRepresentation/PlayerName").GetComponent<TextMeshProUGUI>().text = GameController.playerRep[player];
                Debug.Log(GameController.playerRep[player] + " writed on board number "+ (player+1));
            }






        }
    }
}