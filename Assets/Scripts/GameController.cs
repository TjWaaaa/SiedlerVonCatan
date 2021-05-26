using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Enums;
using Networking.ClientSide;
using Trade;
using UnityEngine;
using UnityEngine.UI;
using Networking.Communication;
using Player;
using PlayerColor;
using UI;

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

        if (!TradeMenu.isActive()) //that nobody can build cities by accident (while trading)
        {
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (Input.GetMouseButtonDown(0))
                {

                    if (hit.collider.tag == "VillageSlot")
                    {
                        int posInArray = Int32.Parse(hit.transform.name);
                        
                        clientRequest.requestBuild(BUYABLES.VILLAGE, posInArray);
                            
                            
                    //     Debug.Log("Village: " + hit.transform.position);
                    //     if (players[currentPlayer].canBuyBuyable(BUYABLES.VILLAGE)) // clientGameLogic.requestBuild
                    //     {
                    //     
                    //         //Color color = players[currentPlayer].GetColor();
                    //         BuildVillage(hit.transform.position + new Vector3(0, 0.065f, 0));
                    //         Destroy(hit.transform.gameObject);
                    //         players[currentPlayer].buyVillage();
                    //         ChangeRessourcesOutput(players[currentPlayer]);
                    //     }
                    //     else Debug.Log("Not enough ressources");
                    }

                    else if (hit.collider.tag == "Village")
                    {

                        Debug.Log("City: " + hit.transform.position);
                        if (players[currentPlayer].canBuyBuyable(BUYABLES.CITY))
                        {

                            //Color color = players[currentPlayer].GetColor();
                            BuildCity(hit.transform.position);
                            Destroy(hit.transform.gameObject);
                            players[currentPlayer].buyCity();
                            ChangeRessourcesOutput(players[currentPlayer]);
                        }
                        else Debug.Log("Not enough ressources");
                    }

                    else if (hit.collider.tag == "RoadSlot")
                    {

                        Debug.Log("Road: " + hit.transform.position);
                        if (players[currentPlayer].canBuyBuyable(BUYABLES.ROAD))
                        {

                            //Color color = players[currentPlayer].GetColor();
                            BuildRoad(hit.transform.position + new Vector3(0, 0.065f, 0), hit.transform.rotation);
                            Destroy(hit.transform.gameObject);
                            players[currentPlayer].buyStreet();
                            ChangeRessourcesOutput(players[currentPlayer]);
                        }
                        else Debug.Log("Not enough resources");
                    }
                }
            }
        }
        else ChangeRessourcesOutput(players[currentPlayer]);
    }
    
    
    public void BuildVillage(Vector3 position)
    {
    
        PLAYERCOLOR c = players[currentPlayer].getPlayerColor();
    
        if (c.Equals(PLAYERCOLOR.BLUE))
        {
            Instantiate(villageBlue, position, Quaternion.identity);
        }
        else if (c.Equals(PLAYERCOLOR.RED))
        {
            Instantiate(villageRed, position, Quaternion.identity);
        }
        else if (c.Equals(PLAYERCOLOR.WHITE))
        {
            Instantiate(villageWhite, position, Quaternion.identity);
        }
        else if (c.Equals(PLAYERCOLOR.YELLOW))
        {
            Instantiate(villageYellow, position, Quaternion.identity);
        }
    }
    
    public void BuildCity(Vector3 position)
    {
    
        PLAYERCOLOR c = players[currentPlayer].getPlayerColor();
    
        if (c.Equals(PLAYERCOLOR.BLUE))
        {
            Instantiate(cityBlue, position, Quaternion.identity);
        }
        else if (c.Equals(PLAYERCOLOR.RED))
        {
            Instantiate(cityRed, position, Quaternion.identity);
        }
        else if (c.Equals(PLAYERCOLOR.WHITE))
        {
            Instantiate(cityWhite, position, Quaternion.identity);
        }
        else if (c.Equals(PLAYERCOLOR.YELLOW))
        {
            Instantiate(cityYellow, position, Quaternion.identity);
        }
    }
    
    public void BuildRoad(Vector3 position, Quaternion rotation)
    {
    
        PLAYERCOLOR c = players[currentPlayer].getPlayerColor();
    
        if (c.Equals(PLAYERCOLOR.BLUE))
        {
            Instantiate(roadBlue, position, rotation);
        }
        else if (c.Equals(PLAYERCOLOR.RED))
        {
            Instantiate(roadRed, position, rotation);
        }
        else if (c.Equals(PLAYERCOLOR.WHITE))
        {
            Instantiate(roadWhite, position, rotation);
        }
        else if (c.Equals(PLAYERCOLOR.YELLOW))
        {
            Instantiate(roadYellow, position, rotation);
        }
    }


    public void NextPlayer()
    {
        Debug.Log("NextPlayer in GameController is called");
        clientRequest.requestEndTurn();
    }

    private void ChangeRessourcesOutput(ServerPlayer player)
    {
        bricksText.text = player.getResourceAmount(RESOURCETYPE.BRICK).ToString();
        oreText.text = player.getResourceAmount(RESOURCETYPE.ORE).ToString();
        sheepText.text = player.getResourceAmount(RESOURCETYPE.SHEEP).ToString();
        wheatText.text = player.getResourceAmount(RESOURCETYPE.WHEAT).ToString();
        woodText.text = player.getResourceAmount(RESOURCETYPE.WOOD).ToString();
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
