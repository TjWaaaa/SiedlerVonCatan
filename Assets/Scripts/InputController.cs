using System;
using Enums;
using Networking.ClientSide;
using Networking.Communication;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;
    private ClientRequest clientRequest = new ClientRequest();
    private GameObject nextPlayerButton;

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
    private static TextMeshProUGUI leftDevCards;
    private static TextMeshProUGUI amountVP;
    private static GameObject devCardsVP;

    // Start is called before the first frame update
    private void Start()
    {
        // Camera
        mainCamera = Camera.main;
        nextPlayerButton = GameObject.Find("nextPlayer");
        nextPlayerButton.GetComponent<Button>().onClick.AddListener(nextPlayer);

        // Find Build Buttons and add event listener
        buildStreetButton = GameObject.Find("buildStreet");
        buildVillageButton = GameObject.Find("buildVillage");
        buildCityButton = GameObject.Find("buildCity");
        buildStreetButton.GetComponent<Button>().onClick.AddListener(startBuildStreetMode);
        buildVillageButton.GetComponent<Button>().onClick.AddListener(startBuildVillageMode);
        buildCityButton.GetComponent<Button>().onClick.AddListener(startBuildCityMode);
        
        // DevCards
        playVPButton = GameObject.Find("PlayVP");
        buyDevCardButton = GameObject.Find("BuyDevCard");
        leftDevCards = GameObject.Find("LeftDevCards").GetComponent<TextMeshProUGUI>();
        amountVP = GameObject.Find("AmountVP").GetComponent<TextMeshProUGUI>();
        devCardsVP = GameObject.Find("DevCardsVP");
        devCardsVP.SetActive(false);
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
    
    public void nextPlayer()
    {
        Debug.LogWarning("CLIENT: NextPlayer in GameController is called");
        clientRequest.requestEndTurn();
    }
    
    public void playVP()
    {
        clientRequest.requestPlayDevelopement(DEVELOPMENT_TYPE.VICTORY_POINT);
    }
    
    public void buyDevCard()
    {
        Debug.Log($"CLIENT: Player wants to buy a devCard");
        clientRequest.requestBuyDevelopement();
    }

    public static void showDevCards(OwnClientPlayer ownClientPlayer)
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

    public static void updateLeftDevCards(int updateLD)
    {
        leftDevCards.text = updateLD.ToString();
    }
}
