using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Networking.ClientSide;
using Networking.Communication;
using Player;
using PlayerColor;
using TMPro;
using Trade;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject clientGameLogic;
    public GameObject showCurrentPlayer;


    // only for testing
    private static ServerPlayer[] players;
    private static int currentPlayer;

    //only for testing
    public TextMeshProUGUI bricksText;
    public TextMeshProUGUI oreText;
    public TextMeshProUGUI sheepText;
    public TextMeshProUGUI wheatText;
    public TextMeshProUGUI woodText;

    public GameObject villageBlue;
    public GameObject villageRed;
    public GameObject villageWhite;
    public GameObject villageYellow;

    public GameObject cityBlue;
    public GameObject cityRed;
    public GameObject cityWhite;
    public GameObject cityYellow;

    public GameObject roadBlue;
    public GameObject roadRed;
    public GameObject roadWhite;
    public GameObject roadYellow;

    private ClientRequest clientRequest = new ClientRequest();

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        // All this stuff has to go.
        players = new ServerPlayer[]
        {
            new ServerPlayer("Player1", PLAYERCOLOR.RED),
            new ServerPlayer("Player2", PLAYERCOLOR.BLUE)
        };

        currentPlayer = 0;

        // test
        // PlayerRepresentation.showNextPlayer(0,1);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (InputController.buildVillageMode)
                {
                    if (hit.collider.tag == "VillageSlot")
                    {
                        Debug.Log("want to build a village");
                        int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                        clientRequest.requestBuild(BUYABLES.VILLAGE, posInArray);
                    }
                }

                else if (InputController.buildCityMode)
                {
                    if (hit.collider.tag == "Village")
                    {
                        int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                        Debug.Log("CLIENT: " + posInArray);
                        clientRequest.requestBuild(BUYABLES.CITY, posInArray);
                    }
                }

                else if (InputController.buildStreetMode)
                {
                    if (hit.collider.tag == "RoadSlot")
                    {
                        int posInArray = Int32.Parse(hit.transform.name.Substring(1));
                        clientRequest.requestBuild(BUYABLES.ROAD, posInArray);
                    }
                }

            }
        }

    }



    public void NextPlayer()
    {
        Debug.Log("CLIENT: NextPlayer in GameController is called");
        clientRequest.requestEndTurn();
    }



    public static int getCurrentPlayer()
    {
        return currentPlayer;
    }

    public static ServerPlayer[] getPlayers()
    {
        return players;
    }
}
