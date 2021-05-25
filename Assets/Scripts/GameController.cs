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
using UI;

public class GameController : MonoBehaviour
{
    private GameObject clientGameLogic;
    public GameObject showCurrentPlayer;
    
    
    public static List<RepresentativePlayer> representativePlayers = new List<RepresentativePlayer>();
    public static OwnClientPlayer ownClientPlayer;
    
    // only for testing
    private static ServerPlayer[] players;
    private static int currentPlayer;

    private Builder builder;


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
        //builder = new Builder();

        // All this stuff has to go.
        players = new ServerPlayer[]
        {
            new ServerPlayer("Player1", Color.red),
            new ServerPlayer("Player2", Color.blue)
        };
        
       
        

        currentPlayer = 0;

        // test
        PlayerRepresentation.showNextPlayer(0,1);

        showCurrentPlayer.GetComponent<Image>().color = players[currentPlayer].getPlayerColor();
        showCurrentPlayer.transform.GetChild(0).GetComponent<Text>().text = players[currentPlayer].getPlayerName();
        ChangeRessourcesOutput(players[currentPlayer]);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!TradeMenu.isActive()) //that nobody can build cities by accident (while trading)
        {
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (Input.GetMouseButtonDown(0))
                {

                    if (hit.collider.tag == "VillageSlot")
                    {

                        Debug.Log("Village: " + hit.transform.position);
                        if (players[currentPlayer].canBuyVillage())
                        {

                            //Color color = players[currentPlayer].GetColor();
                            BuildVillage(hit.transform.position + new Vector3(0, 0.065f, 0));
                            Destroy(hit.transform.gameObject);
                            players[currentPlayer].buyVillage();
                            ChangeRessourcesOutput(players[currentPlayer]);
                        }
                        else Debug.Log("Not enough ressources");
                    }

                    else if (hit.collider.tag == "Village")
                    {

                        Debug.Log("City: " + hit.transform.position);
                        if (players[currentPlayer].canBuyCity())
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
                        if (players[currentPlayer].canBuyStreet())
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

    public static void createRepresentativePlayer(int playerID, string playerName, Color playerColor)
    {
        representativePlayers.Add( new RepresentativePlayer(playerID, playerName, playerColor));
        Debug.Log("client: "+ playerName + " created. Player Number " + representativePlayers.Count);
    }

    public static void createOwnClientPlayer(int playerID)
    {
        ownClientPlayer = new OwnClientPlayer(playerID);
        Debug.Log("Created OwnClientPlayer with ID" + playerID);
    }
    
    public void BuildVillage(Vector3 position)
    {

        Color c = players[currentPlayer].getPlayerColor();

        if (c.Equals(Color.blue))
        {
            Instantiate(villageBlue, position, Quaternion.identity);
        }
        else if (c.Equals(Color.red))
        {
            Instantiate(villageRed, position, Quaternion.identity);
        }
        else if (c.Equals(Color.white))
        {
            Instantiate(villageWhite, position, Quaternion.identity);
        }
        else if (c.Equals(Color.yellow))
        {
            Instantiate(villageYellow, position, Quaternion.identity);
        }
    }

    public void BuildCity(Vector3 position)
    {

        Color c = players[currentPlayer].getPlayerColor();

        if (c.Equals(Color.blue))
        {
            Instantiate(cityBlue, position, Quaternion.identity);
        }
        else if (c.Equals(Color.red))
        {
            Instantiate(cityRed, position, Quaternion.identity);
        }
        else if (c.Equals(Color.white))
        {
            Instantiate(cityWhite, position, Quaternion.identity);
        }
        else if (c.Equals(Color.yellow))
        {
            Instantiate(cityYellow, position, Quaternion.identity);
        }
    }

    public void BuildRoad(Vector3 position, Quaternion rotation)
    {

        Color c = players[currentPlayer].getPlayerColor();

        if (c.Equals(Color.blue))
        {
            Instantiate(roadBlue, position, rotation);
        }
        else if (c.Equals(Color.red))
        {
            Instantiate(roadRed, position, rotation);
        }
        else if (c.Equals(Color.white))
        {
            Instantiate(roadWhite, position, rotation);
        }
        else if (c.Equals(Color.yellow))
        {
            Instantiate(roadYellow, position, rotation);
        }
    }


    public void NextPlayer()
    {
        Debug.Log("NextPlayer in GameController is called");
        clientRequest.requestEndTurn();

        if (currentPlayer == players.Length - 1)
        {
            currentPlayer = 0;
        }
        else
        {
            currentPlayer++;
        }

        showCurrentPlayer.GetComponent<Image>().color = players[currentPlayer].getPlayerColor();
        showCurrentPlayer.transform.GetChild(0).GetComponent<Text>().text = players[currentPlayer].getPlayerName();
        ChangeRessourcesOutput(players[currentPlayer]);
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
