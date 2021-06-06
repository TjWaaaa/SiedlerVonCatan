using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChangeVisibility : MonoBehaviour
    {
        public GameObject showButton;
        public GameObject hideButton;
        private Boolean isVisible;

        void Start()
        {
            gameObject.SetActive(false);
            showButton.GetComponent<Button>().onClick.AddListener(show);
            hideButton.GetComponent<Button>().onClick.AddListener(hide);
        }

        public void show()
        {
            if (!isVisible)
            {
                Debug.Log("CLIENT: set visible");
                gameObject.SetActive(true);
                isVisible = true;
            }
        }

        public void hide()
        {
            if (isVisible)
            {
                Debug.Log("CLIENT: set hidden");
                gameObject.SetActive(false);
                isVisible = false;
            }
        }
    }
}