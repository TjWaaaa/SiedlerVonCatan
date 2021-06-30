
using UnityEngine;
using System;
using Enums;

namespace Trade
{
    public class TradeButton : MonoBehaviour
    {
        private RESOURCE_TYPE resourceType;
        private Boolean isClicked;

        // Selected resources
        private static RESOURCE_TYPE offerResourcetype = RESOURCE_TYPE.NONE;
        private static RESOURCE_TYPE expectResourcetype = RESOURCE_TYPE.NONE;


        void Start()
        {
            // Get the resource of each button individually
            resourceType = (RESOURCE_TYPE) Enum.Parse(typeof(RESOURCE_TYPE), gameObject.name, true);

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
                    if (offerResourcetype == RESOURCE_TYPE.NONE)
                    {
                        offerResourcetype = resourceType;
                        gameObject.transform.GetChild(1).SetAsFirstSibling();
                        isClicked = true;
                    }

                    return offerResourcetype.ToString().ToLower();
                }
                else
                {
                    if (expectResourcetype == RESOURCE_TYPE.NONE)
                    {
                        expectResourcetype = resourceType;
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
                if (gameObject.CompareTag("giveResource")) offerResourcetype = RESOURCE_TYPE.NONE;
                else expectResourcetype = RESOURCE_TYPE.NONE;
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
            offerResourcetype = RESOURCE_TYPE.NONE;
            expectResourcetype = RESOURCE_TYPE.NONE;
        }
        
        /// <summary>
        /// Check if selected buttons are valid. There has to be a offerResource and a expectResource
        /// </summary>
        /// <returns>isValidTradeRequest</returns>
        public static Boolean isValidTradeRequest()
                {
                    if (expectResourcetype != RESOURCE_TYPE.NONE && offerResourcetype != RESOURCE_TYPE.NONE) return true;
                    else return false;
                }
        
        
        // Getter

        public RESOURCE_TYPE getResourcetype()
        {
            return resourceType;
        }

        public static RESOURCE_TYPE getExpectResourcetype()
        {
            return expectResourcetype;
        }

        public static RESOURCE_TYPE getOfferResourcetype()
        {
            return offerResourcetype;
        }
        
    }
}

