using System;
using Enums;
using UnityEngine;
using Player;
using TMPro;

namespace UI
{
    public class OwnPlayerRepresentation
    {
        private TextMeshProUGUI ownPlayerSheep;
        private TextMeshProUGUI ownPlayerWood;
        private TextMeshProUGUI ownPlayerBrick;
        private TextMeshProUGUI ownPlayerOre;
        private TextMeshProUGUI ownPlayerWheat;

        private TextMeshProUGUI ownPlayerLeftStreets;
        private TextMeshProUGUI ownPlayerLeftVillages;
        private TextMeshProUGUI ownPlayerLeftCitys;


        /// <summary>
        /// Represents all resources and left buildObjects of the player
        /// </summary>
        /// <param name="ownClientPlayer"></param>
        public void represent(OwnClientPlayer ownClientPlayer)
        {
            // Find all GameObjects

            ownPlayerSheep = GameObject.Find("OwnPlayerSheep").GetComponent<TextMeshProUGUI>();
            ownPlayerWood = GameObject.Find("OwnPlayerWood").GetComponent<TextMeshProUGUI>();
            ownPlayerBrick = GameObject.Find("OwnPlayerBrick").GetComponent<TextMeshProUGUI>();
            ownPlayerOre = GameObject.Find("OwnPlayerOre").GetComponent<TextMeshProUGUI>();
            ownPlayerWheat = GameObject.Find("OwnPlayerWheat").GetComponent<TextMeshProUGUI>();

            ownPlayerLeftStreets = GameObject.Find("OwnPlayerLeftStreets").GetComponent<TextMeshProUGUI>();
            ownPlayerLeftVillages = GameObject.Find("OwnPlayerLeftVillages").GetComponent<TextMeshProUGUI>();
            ownPlayerLeftCitys = GameObject.Find("OwnPlayerLeftCitys").GetComponent<TextMeshProUGUI>();
            
            // Connect labels to ownClientPlayer
            updaetOwnPlayerUI(ownClientPlayer);
        }

        public void updaetOwnPlayerUI(OwnClientPlayer ownClientPlayer)
        {
            ownPlayerSheep.text = ownClientPlayer.getResourceAmount(RESOURCE_TYPE.SHEEP).ToString();
            ownPlayerWood.text = ownClientPlayer.getResourceAmount(RESOURCE_TYPE.WOOD).ToString();
            ownPlayerBrick.text = ownClientPlayer.getResourceAmount(RESOURCE_TYPE.BRICK).ToString();
            ownPlayerOre.text = ownClientPlayer.getResourceAmount(RESOURCE_TYPE.ORE).ToString();
            ownPlayerWheat.text = ownClientPlayer.getResourceAmount(RESOURCE_TYPE.WHEAT).ToString(); 
            
            ownPlayerLeftStreets.text = ownClientPlayer.getLeftStreets().ToString();
            ownPlayerLeftVillages.text = ownClientPlayer.getLeftVillages().ToString();
            ownPlayerLeftCitys.text = ownClientPlayer.getLeftCitys().ToString();
        }
    }
}