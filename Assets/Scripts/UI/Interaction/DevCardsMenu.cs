using Enums;
using Networking.Communication;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DevCardsMenu : MonoBehaviour
    {
        
        private ClientRequest clientRequest = new ClientRequest();
        
        private TextMeshProUGUI leftDevCards;
        private TextMeshProUGUI amountVP;
        private GameObject devCardsVP;
        private GameObject playVPButton;
        private GameObject buyDevCardButton;


        public void Start()
        {
            // Find GameObjects
            leftDevCards = GameObject.Find("LeftDevCards").GetComponent<TextMeshProUGUI>();
            amountVP = GameObject.Find("AmountVP").GetComponent<TextMeshProUGUI>();
            devCardsVP = GameObject.Find("DevCardsVP");
            playVPButton = GameObject.Find("PlayVP");
            buyDevCardButton = GameObject.Find("BuyDevCard");
            playVPButton.GetComponent<Button>().onClick.AddListener(playVP);
            buyDevCardButton.GetComponent<Button>().onClick.AddListener(buyDevCard);
            devCardsVP.SetActive(false);
        }
        
        /// <summary>
        /// If the player has DevCards of type VICTORY_POINT, they are shown
        /// </summary>
        /// <param name="ownClientPlayer"></param>
        public void showDevCards(OwnClientPlayer ownClientPlayer)
        {
            int cacheAmountVP = ownClientPlayer.getDevCardAmount(DEVELOPMENT_TYPE.VICTORY_POINT);
        
            if (cacheAmountVP > 0)
            {
                devCardsVP.SetActive(true);
                amountVP.text = cacheAmountVP.ToString();
            }
            else
            {
                devCardsVP.SetActive(false);
            }
        }

        /// <summary>
        /// Whenever a DevCard is drawn, the deck of cards has to be updated
        /// </summary>
        /// <param name="updateLD">how many DevCards are left</param>
        public void updateLeftDevCards(int updateLD)
        {
            leftDevCards.text = updateLD.ToString();
        }
        
        
        public void playVP()
        {
            clientRequest.requestPlayDevelopement(DEVELOPMENT_TYPE.VICTORY_POINT);
        }
        
        public void buyDevCard()
        {
            Debug.Log($"CLIENT: Player wants to buy a devCard");
            clientRequest.requestBuyDevelopement();
        }
    }
}