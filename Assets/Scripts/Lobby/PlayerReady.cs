using Networking;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReady : MonoBehaviour
{
    private readonly ClientRequest clientRequest = new ClientRequest();
    private bool oldReadyStatus;
    private Toggle toggleComponent;

    private void Start()
    {
        oldReadyStatus = false;
        toggleComponent = this.GetComponent<Toggle>();
    }

    private void Update()
    {
        if (oldReadyStatus != toggleComponent.isOn)
        {
            oldReadyStatus = toggleComponent.isOn;
            clientRequest.requestPlayerReady(toggleComponent.isOn);
        }
    }
}
