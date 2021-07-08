using System;
using Networking.Communication;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NextPlayer : MonoBehaviour
    {

        private ClientRequest clientRequest = new ClientRequest();

        private GameObject nextPlayerButton;

        private void Start()
        {
            nextPlayerButton = GameObject.Find("nextPlayer");
            nextPlayerButton.GetComponent<Button>().onClick.AddListener(nextPlayer);
        }

        public void nextPlayer()
        {
            Debug.LogWarning("CLIENT: NextPlayer in GameController is called");
            clientRequest.requestEndTurn();
        }
    }
}