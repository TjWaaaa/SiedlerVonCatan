using Networking;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerReady : MonoBehaviour, IPointerClickHandler
{
    private readonly ClientRequest clientRequest = new ClientRequest();
    private bool oldReadyStatus;
    private Toggle toggleComponent;

    private void Start()
    {
        oldReadyStatus = false;
        toggleComponent = this.GetComponent<Toggle>();
    }

    // private void Update()
    // {
    //     if (oldReadyStatus != toggleComponent.isOn) // if ready status was updated --> send to server
    //     {
    //         oldReadyStatus = toggleComponent.isOn;
    //         clientRequest.requestPlayerReady(toggleComponent.isOn);
    //     }
    // }

    public void OnPointerClick(PointerEventData eventData)
    {
        clientRequest.requestPlayerReady(toggleComponent.isOn);
    }
}
