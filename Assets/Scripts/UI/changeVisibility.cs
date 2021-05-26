using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class changeVisibility : MonoBehaviour
    {
        public GameObject changeVisibilityButton;
        private Boolean isVisible = false;

        void Start()
        {
            gameObject.SetActive(false);
            changeVisibilityButton.GetComponent<Button>().onClick.AddListener(changeVisible);
        }
        
        public void changeVisible()
        {
            if (!isVisible)
            {
                Debug.Log("set visible");
                gameObject.SetActive(true);
                isVisible = true;
            }
            else
            {
                Debug.Log("set hidden");
                gameObject.SetActive(false);
                isVisible = false;
            }
        }


    }
}