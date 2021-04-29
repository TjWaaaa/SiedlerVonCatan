using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using Resource;

namespace Trade
{
    public class TradeMenu : MonoBehaviour
    {
        public GameObject startTradeButton;
        public GameObject closeTradeButton;
        public GameObject trade;

        private GameObject[] giveResources = new GameObject[6];
        private GameObject[] getResources = new GameObject[6];
        
        void Start()
        {
            startTradeButton = GameObject.Find("startTrade");
            closeTradeButton = GameObject.Find("closeTrade");
            trade = GameObject.Find("trade");
            startTradeButton.GetComponent<Button>().onClick.AddListener(startTrade);
            closeTradeButton.GetComponent<Button>().onClick.AddListener(closeTrade);
            trade.GetComponent<Button>().onClick.AddListener(tryTrade);

            
            giveResources = GameObject.FindGameObjectsWithTag("giveResource");
            getResources = GameObject.FindGameObjectsWithTag("getResource");
            
            foreach (GameObject button in giveResources) { button.GetComponent<Button>().onClick.AddListener(delegate { giveResource(button); }); }
            foreach (GameObject button in getResources) { button.GetComponent<Button>().onClick.AddListener(delegate { getResource(button); }); }

            gameObject.SetActive(false);
            

        }


        void giveResource(GameObject button)
        {
            
            if (GameController.getPlayers()[GameController.getCurrentPlayer()].canTrade(button.GetComponent<TradeButton>().resource))
            {
                button.GetComponent<TradeButton>().clickButton();
            }
            
        }

        void getResource(GameObject button)
        {
            button.GetComponent<TradeButton>().clickButton();
        }

        void startTrade()
        {
            gameObject.SetActive(true);
        }

        void closeTrade()
        {
            setInactive();
        }

        void tryTrade()
        {

            setInactive();
        }
        
        void setInactive()
        {
            foreach (GameObject button in giveResources) { button.GetComponent<TradeButton>().reset(); }
            foreach (GameObject button in getResources) { button.GetComponent<TradeButton>().reset(); }
            gameObject.SetActive(false);
            
        }

        void requestTradeBank()
        {
            
        }
        
    }
}