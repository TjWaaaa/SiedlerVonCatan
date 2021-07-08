using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Networking.Communication;

public class PlayerReady : MonoBehaviour, IPointerClickHandler
{
    private readonly ClientRequest clientRequest = new ClientRequest();
    private Toggle toggleComponent;

    private void Start()
    {
        toggleComponent = GetComponent<Toggle>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        clientRequest.requestPlayerReady(toggleComponent.isOn);
    }
}
