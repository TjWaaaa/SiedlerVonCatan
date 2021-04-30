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
        //is needed right now, there should be a better solution later
        private Player currentPlayer;
        
        private GameObject startTradeButton;
        private GameObject closeTradeButton;
        private GameObject trade;

        private GameObject[] giveResources = new GameObject[5];
        private GameObject[] getResources = new GameObject[5];

        private static Boolean active;
        
        void Start()
        {
            
            // Find all buttons and add EventListener
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

            // Inactive by default
            gameObject.SetActive(false);
            active = false;

        }

        private void Update()
        {
            // Only for now. Later there should be a better way to get the currentPlayer
            currentPlayer = GameController.getPlayers()[GameController.getCurrentPlayer()];
        }

        // When the buttons on the left side are clicked -> the resource the player wants to give away
        void giveResource(GameObject button)
        {
            
            if (currentPlayer.canTrade(button.GetComponent<TradeButton>().resource))
            {
                button.GetComponent<TradeButton>().clickButton();
            }
            
        }
        
        // When the buttons on the right side are clicked -> th resource the player wants to get
        void getResource(GameObject button)
        {
            button.GetComponent<TradeButton>().clickButton();
        }

        void startTrade()
        {
            gameObject.SetActive(true);
            active = true;
        }

        void closeTrade()
        {
            setInactive();
        }

        // This method works, but it isn't instantly visible in the UI
        void tryTrade()
        {
            if (TradeButton.isValidTradeRequest())
            {
                if (requestTradeBank(TradeButton.getGiveResource(), TradeButton.getGetResource()))
                {
                    currentPlayer.trade(TradeButton.getGiveResource(), TradeButton.getGetResource());
                    Debug.Log(currentPlayer+" traded 4 " + TradeButton.getGiveResource() + " against 1 " +TradeButton.getGetResource() );
                }
                else Debug.Log("For any reason, you can't trade.");
            }
            else Debug.Log("You have to chose a resource on each side.");
            setInactive();
        }
        
        void setInactive()
        {
            foreach (GameObject button in giveResources) { button.GetComponent<TradeButton>().reset(); }
            foreach (GameObject button in getResources) { button.GetComponent<TradeButton>().reset(); }
            gameObject.SetActive(false);
            active = false;

        }
        
        // Todo: sending this request to server
        Boolean requestTradeBank(RESOURCE giveResource, RESOURCE getResource)
        {
            Debug.Log(currentPlayer+" wants to trade 4 " + giveResource + " against 1 " + getResource);
            return true;
        }

        public static Boolean isActive()
        {
            return active;
        }
    }
}