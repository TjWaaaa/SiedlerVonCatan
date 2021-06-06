using Enums;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class OutputController : MonoBehaviour
    {
        private TextMeshProUGUI leftDevCards;
        private TextMeshProUGUI amountVP;
        private GameObject devCardsVP;


        public void Start()
        {
            // Find GameObjects
            leftDevCards = GameObject.Find("LeftDevCards").GetComponent<TextMeshProUGUI>();
            amountVP = GameObject.Find("AmountVP").GetComponent<TextMeshProUGUI>();
            devCardsVP = GameObject.Find("DevCardsVP");
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
    }
}