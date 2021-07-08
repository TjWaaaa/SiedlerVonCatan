
using UnityEngine;
using System;
using Enums;

namespace Trade
{
    public class TradeButton : MonoBehaviour
    {
        private RESOURCETYPE resourcetype;
        private Boolean isClicked;

        // Selected resources
        private static RESOURCETYPE offerResourcetype = RESOURCETYPE.NONE;
        private static RESOURCETYPE expectResourcetype = RESOURCETYPE.NONE;


        void Start()
        {
            // Get the resource of each button individually
            resourcetype = (RESOURCETYPE)Enum.Parse(typeof(RESOURCETYPE), gameObject.name, true);

        }

        /// <summary>
        /// Select/Deselect tradeButtons. Only one button of each type can be clicked.
        /// </summary>
        /// <returns>resource of button which is clicked</returns>
        public String clickButton()
        {
            if (!isClicked)
            {
                if (gameObject.CompareTag("giveResource"))
                {
                    if (offerResourcetype == RESOURCETYPE.NONE)
                    {
                        offerResourcetype = resourcetype;
                        gameObject.transform.GetChild(1).SetAsFirstSibling();
                        isClicked = true;
                    }

                    return offerResourcetype.ToString().ToLower();
                }
                else
                {
                    if (expectResourcetype == RESOURCETYPE.NONE)
                    {
                        expectResourcetype = resourcetype;
                        gameObject.transform.GetChild(1).SetAsFirstSibling();
                        isClicked = true;
                    }

                    return expectResourcetype.ToString().ToLower();
                }
            }
            else
            {
                gameObject.transform.GetChild(1).SetAsFirstSibling();
                isClicked = false;
                if (gameObject.CompareTag("giveResource")) offerResourcetype = RESOURCETYPE.NONE;
                else expectResourcetype = RESOURCETYPE.NONE;
                return "";
            }
        }

        /// <summary>
        /// Reset if the tradeMenu is closed
        /// </summary>
        public void reset()
        {
            if (isClicked)
            {
                gameObject.transform.GetChild(1).SetAsFirstSibling();
            }

            isClicked = false;
            offerResourcetype = RESOURCETYPE.NONE;
            expectResourcetype = RESOURCETYPE.NONE;
        }

        /// <summary>
        /// Check if selected buttons are valid. There has to be a offerResource and a expectResource
        /// </summary>
        /// <returns>isValidTradeRequest</returns>
        public static Boolean isValidTradeRequest()
        {
            if (expectResourcetype != RESOURCETYPE.NONE && offerResourcetype != RESOURCETYPE.NONE) return true;
            else return false;
        }


        // Getter

        public RESOURCETYPE getResourcetype()
        {
            return resourcetype;
        }

        public static RESOURCETYPE getExpectResourcetype()
        {
            return expectResourcetype;
        }

        public static RESOURCETYPE getOfferResourcetype()
        {
            return offerResourcetype;
        }

    }
}

