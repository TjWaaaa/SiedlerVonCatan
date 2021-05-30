using System;
using Enums;
using Networking.ClientSide;
using Networking.Communication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;
    private ClientRequest clientRequest = new ClientRequest();

    // Build
    private bool buildStreetMode;
    private bool buildVillageMode;
    private bool buildCityMode ;
    private GameObject buildStreetButton;
    private GameObject buildVillageButton;
    private GameObject buildCityButton;
    
    // DevCards
    private GameObject playVPButton;
    private GameObject buyDevCardButton;
    private GameObject leftDevCards;
    private GameObject amountVP;
    private GameObject devCardsVP;

    // Start is called before the first frame update
    private void Start()
    {
        // Camera
        mainCamera = Camera.main;

        // Find Build Buttons and add event listener
        buildStreetButton = GameObject.Find("buildStreet");
        buildVillageButton = GameObject.Find("buildVillage");
        buildCityButton = GameObject.Find("buildCity");
        
        // Adding the EventTrigger and onclick Listener
        buildStreetButton.GetComponent<Button>().onClick.AddListener(startBuildStreetMode);
        buildVillageButton.GetComponent<Button>().onClick.AddListener(startBuildVillageMode);
        buildCityButton.GetComponent<Button>().onClick.AddListener(startBuildCityMode);
        
        // DevCards
        playVPButton = GameObject.Find("PlayVP");
        buyDevCardButton = GameObject.Find("BuyDevCard");
        leftDevCards = GameObject.Find("LeftDevCards");
        amountVP = GameObject.Find("AmountVP");
        devCardsVP = GameObject.Find("DevCardsVP");
        devCardsVP.SetActive(true);
        playVPButton.GetComponent<Button>().onClick.AddListener(playVP);
        buyDevCardButton.GetComponent<Button>().onClick.AddListener(buyDevCard);

    }

    // Update is called once per frame
    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (buildVillageMode)
        {
            if (Input.GetMouseButtonDown(0)
                && Physics.Raycast(ray, out hit, 100f)
                && hit.collider.tag == "VillageSlot")
            {
                Debug.Log("Client: want to build a village");
                int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                clientRequest.requestBuild(BUYABLES.VILLAGE, posInArray);
                stopBuildMode();
            }
        }
        else if (buildCityMode)
        {
            if (Input.GetMouseButtonDown(0)
                && Physics.Raycast(ray, out hit, 100f)
                && hit.collider.tag == "Village")
            {
                Debug.Log("Client: want to build a city");
                int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                clientRequest.requestBuild(BUYABLES.CITY, posInArray);
                stopBuildMode();
            }
        }
        else if (buildStreetMode)
        {
            if (Input.GetMouseButtonDown(0)
                && Physics.Raycast(ray, out hit, 100f)
                && hit.collider.tag == "RoadSlot")
            {
                Debug.Log("Client: want to build a road");
                int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                clientRequest.requestBuild(BUYABLES.ROAD, posInArray);
                stopBuildMode();
            }
        }
    }

    private void stopBuildMode()
    {
        buildStreetMode = false;
        buildVillageMode = false;
        buildCityMode = false;
        Debug.Log("BUILDMODE IS OFF");
    }

    private void startBuildStreetMode()
    {
        buildStreetMode = true;
        buildCityMode = false;
        buildVillageMode = false;
        Debug.Log("BUILDSTREETMODE IS ON");
    }

    private void startBuildVillageMode()
    {
        buildVillageMode = true;
        buildStreetMode = false;
        buildCityMode = false;
        Debug.Log("BUILDVILLAGEMODE IS ON");
    }

    private void startBuildCityMode()
    {
        buildVillageMode = false;
        buildStreetMode = false;
        buildCityMode = true;
        Debug.Log("BUILDCITYMODE IS ON");
    }
    
    public void playVP()
    {
        clientRequest.requestPlayDevelopement(DEVELOPMENT_TYPE.VICTORY_POINT);
    }
    
    public void buyDevCard()
    {
        Debug.Log($"CLIENT: Player wants to buy a devCard");
        clientRequest.requestBuyDevelopement();
        // not implemented yet
        // if devCardNew == devCardVP(bzw if AmountVP > 0) then devCardsVP.setActive(true);
    }
}
