using System;
using Enums;
using Networking.Communication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trade
{
    public class TradeMenu : MonoBehaviour
    {
        private ClientRequest clientRequest = new ClientRequest();

        // UI Interaction
        private GameObject tradeMenu;
        private GameObject startTradeButton;
        private GameObject closeTradeButton;
        private GameObject tradeButton;
        private GameObject[] offerResources = new GameObject[5];
        private GameObject[] expectResources = new GameObject[5];
        private TextMeshProUGUI resourceOffer;
        private TextMeshProUGUI resourceExpect;
        private TextMeshProUGUI amountOffer;


        void Start()
        {
            tradeMenu = GameObject.Find("TradeMenu");

            // Find all buttons 
            startTradeButton = GameObject.Find("startTrade");
            closeTradeButton = GameObject.Find("closeTrade");
            tradeButton = GameObject.Find("trade");
            resourceOffer = GameObject.Find("resourceOffer").GetComponent<TextMeshProUGUI>();
            resourceExpect = GameObject.Find("resourceExpect").GetComponent<TextMeshProUGUI>();
            amountOffer = GameObject.Find("amountOffer").GetComponent<TextMeshProUGUI>();
            offerResources = GameObject.FindGameObjectsWithTag("giveResource");
            expectResources = GameObject.FindGameObjectsWithTag("getResource");

            // Add EventListener
            startTradeButton.GetComponent<Button>().onClick.AddListener(startTrade);
            closeTradeButton.GetComponent<Button>().onClick.AddListener(closeTrade);
            tradeButton.GetComponent<Button>().onClick.AddListener(trade);
            foreach (GameObject button in offerResources) { button.GetComponent<Button>().onClick.AddListener(delegate { checkOfferResource(button); }); }
            foreach (GameObject button in expectResources) { button.GetComponent<Button>().onClick.AddListener(delegate { markExpectResource(button); }); }

            // Inactive by default
            tradeMenu.SetActive(false);

        }


        /// <summary>
        /// OfferResource buttons can only be clicked if the player has enough resources.
        /// To check if this is the case, sending a clientRequest
        /// </summary>
        /// <param name="button">resourceOfferButton which is clicked</param>
        void checkOfferResource(GameObject button)
        {
            clientRequest.requestTradeOffer(button.GetComponent<TradeButton>().getResourcetype(), Array.IndexOf(offerResources, button));

        }

        /// <summary>
        /// Try to click the offerResourceButton and write the according resource to the UI.
        /// Called by ClientReceive
        /// </summary>
        /// <param name="buttonIndex">index in offerResources</param>
        public void markOfferResource(int buttonIndex)
        {
            resourceOffer.text = offerResources[buttonIndex].GetComponent<TradeButton>().clickButton();
        }

        /// <summary>
        /// Try to click the expectResourceButton and write the according resource to the UI.
        /// </summary>
        /// <param name="button">resourceExpectButton which is clicked</param>
        void markExpectResource(GameObject button)
        {
            resourceExpect.text = button.GetComponent<TradeButton>().clickButton();
        }

        /// <summary>
        /// Sends tradeRequest, if the input is valid
        /// </summary>
        void trade()
        {
            if (TradeButton.isValidTradeRequest())
            {

                Debug.Log("Client want's to trade " + TradeButton.getOfferResourcetype() + " against " + TradeButton.getExpectResourcetype());
                int[] offer = convertOfferResourcesToArray();
                int[] expect = convertExpectResourcesToArray();

                clientRequest.requestTradeBank(offer, expect);

            }
            else Debug.Log("CLIENT: You have to choose a resource on each side");
            setInactive();
        }

        void startTrade() { tradeMenu.SetActive(true); }

        void closeTrade() { setInactive(); }

        /// <summary>
        /// Before closing the tradeMenu, everything needs to be reseted
        /// </summary>
        void setInactive()
        {
            foreach (GameObject button in offerResources) { button.GetComponent<TradeButton>().reset(); }
            foreach (GameObject button in expectResources) { button.GetComponent<TradeButton>().reset(); }

            resourceExpect.text = "";
            resourceOffer.text = "";
            tradeMenu.SetActive(false);

        }

        /// <summary>
        /// Convert offerResource to a sendable Array
        /// </summary>
        /// <returns>offer</returns>
        private int[] convertOfferResourcesToArray()
        {
            switch (TradeButton.getOfferResourcetype())
            {
                case RESOURCETYPE.SHEEP:
                    return new[] { 4, 0, 0, 0, 0 };
                case RESOURCETYPE.ORE:
                    return new[] { 0, 4, 0, 0, 0 };
                case RESOURCETYPE.BRICK:
                    return new[] { 0, 0, 4, 0, 0 };
                case RESOURCETYPE.WOOD:
                    return new[] { 0, 0, 0, 4, 0 };
                case RESOURCETYPE.WHEAT:
                    return new[] { 0, 0, 0, 0, 4 };
                default:
                    return new[] { 0, 0, 0, 0, 0 };

            }
        }

        /// <summary>
        /// Convert expectResource to a sendable Array
        /// </summary>
        /// <returns>expect</returns>
        private int[] convertExpectResourcesToArray()
        {
            switch (TradeButton.getExpectResourcetype())
            {
                case RESOURCETYPE.SHEEP:
                    return new[] { 1, 0, 0, 0, 0 };
                case RESOURCETYPE.ORE:
                    return new[] { 0, 1, 0, 0, 0 };
                case RESOURCETYPE.BRICK:
                    return new[] { 0, 0, 1, 0, 0 };
                case RESOURCETYPE.WOOD:
                    return new[] { 0, 0, 0, 1, 0 };
                case RESOURCETYPE.WHEAT:
                    return new[] { 0, 0, 0, 0, 1 };
                default:
                    return new[] { 0, 0, 0, 0, 0 };

            }
        }
    }
}
