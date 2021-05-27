using System;
using Enums;
using UnityEngine;
using Player;
using TMPro;
using Networking.ClientSide;

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


        public void represent(OwnClientPlayer ownClientPlayer)
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
            updaetOwnPlayerUI(ownClientPlayer);
        }

        public void updaetOwnPlayerUI(OwnClientPlayer ownClientPlayer)
        {
            ownPlayerLeftStreets.text = ownClientPlayer.getLeftStreets().ToString();
            ownPlayerLeftVillages.text = ownClientPlayer.getLeftVillages().ToString();
            ownPlayerLeftCitys.text = ownClientPlayer.getLeftCitys().ToString();

            ownPlayerSheep.text = ownClientPlayer.getResourceAmount(RESOURCETYPE.SHEEP).ToString();
            ownPlayerWood.text = ownClientPlayer.getResourceAmount(RESOURCETYPE.WOOD).ToString();
            ownPlayerBrick.text = ownClientPlayer.getResourceAmount(RESOURCETYPE.BRICK).ToString();
            ownPlayerOre.text = ownClientPlayer.getResourceAmount(RESOURCETYPE.ORE).ToString();
            ownPlayerWheat.text = ownClientPlayer.getResourceAmount(RESOURCETYPE.WHEAT).ToString();
        }
    }
}