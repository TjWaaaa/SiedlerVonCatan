using System;
using Enums;
using UnityEngine;
using Player;
using TMPro;

namespace UI
{
    public class OwnPlayerRepresentation : MonoBehaviour
    {
        private TextMeshProUGUI ownPlayerSheep;
        private TextMeshProUGUI ownPlayerWood;
        private TextMeshProUGUI ownPlayerBrick;
        private TextMeshProUGUI ownPlayerOre;
        private TextMeshProUGUI ownPlayerWheat;

        private TextMeshProUGUI ownPlayerLeftStreets;
        private TextMeshProUGUI ownPlayerLeftVillages;
        private TextMeshProUGUI ownPlayerLeftCitys;

        public void Start()
        {
            // find labels in UI
            
            ownPlayerSheep = GameObject.Find("OwnPlayerSheep").GetComponent<TextMeshProUGUI>();
            ownPlayerWood = GameObject.Find("OwnPlayerWood").GetComponent<TextMeshProUGUI>();
            ownPlayerBrick = GameObject.Find("OwnPlayerBrick").GetComponent<TextMeshProUGUI>();
            ownPlayerOre = GameObject.Find("OwnPlayerOre").GetComponent<TextMeshProUGUI>();
            ownPlayerWheat = GameObject.Find("OwnPlayerWheat").GetComponent<TextMeshProUGUI>();
            
            ownPlayerLeftStreets = GameObject.Find("OwnPlayerLeftStreets").GetComponent<TextMeshProUGUI>();
            ownPlayerLeftVillages = GameObject.Find("OwnPlayerLeftVillages").GetComponent<TextMeshProUGUI>();
            ownPlayerLeftCitys = GameObject.Find("OwnPlayerLeftCitys").GetComponent<TextMeshProUGUI>();

            
            // connect labels to ownClientPlayer
            
            updateOwnPlayerResources();
            updateOwnPlayerLeftStreets();
            updateOwnPlayerLeftVillages();
            updateOwnPlayerLeftCitys();

        }

        public void updateOwnPlayerResources()
        {
            ownPlayerSheep.text = GameController.ownClientPlayer.getResourceAmount(RESOURCETYPE.SHEEP).ToString();
            ownPlayerWood.text = GameController.ownClientPlayer.getResourceAmount(RESOURCETYPE.WOOD).ToString();
            ownPlayerBrick.text = GameController.ownClientPlayer.getResourceAmount(RESOURCETYPE.BRICK).ToString();
            ownPlayerOre.text = GameController.ownClientPlayer.getResourceAmount(RESOURCETYPE.ORE).ToString();
            ownPlayerWheat.text = GameController.ownClientPlayer.getResourceAmount(RESOURCETYPE.WHEAT).ToString();
        }

        public void updateOwnPlayerLeftStreets()
        {
            ownPlayerLeftStreets.text = GameController.ownClientPlayer.getLeftStreets().ToString();
        }

        public void updateOwnPlayerLeftVillages()
        {
            ownPlayerLeftVillages.text = GameController.ownClientPlayer.getLeftVillages().ToString();
        }

        public void updateOwnPlayerLeftCitys()
        {
            ownPlayerLeftCitys.text = GameController.ownClientPlayer.getLeftCitys().ToString();
        }
        
    }
}