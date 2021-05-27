//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using Enums;
//using Networking.ClientSide;
//using Trade;
//using UnityEngine;
//using UnityEngine.UI;
//using Networking.Communication;

//public class GameController : MonoBehaviour
//{
//    private GameObject clientGameLogic;
//    public GameObject showCurrentPlayer;

//    private static Player[] players;

//    private static int currentPlayer;

//    private Builder builder;


//    public TextMeshProUGUI bricksText;
//    public TextMeshProUGUI oreText;
//    public TextMeshProUGUI sheepText;
//    public TextMeshProUGUI wheatText;
//    public TextMeshProUGUI woodText;

//    public GameObject villageBlue;
//    public GameObject villageRed;
//    public GameObject villageWhite;
//    public GameObject villageYellow;

//    public GameObject cityBlue;
//    public GameObject cityRed;
//    public GameObject cityWhite;
//    public GameObject cityYellow;

//    public GameObject roadBlue;
//    public GameObject roadRed;
//    public GameObject roadWhite;
//    public GameObject roadYellow;

//    private ClientRequest clientRequest = new ClientRequest();

//    // Start is called before the first frame update
//    void Start()
//    {
//        //builder = new Builder();

//        players = new Player[]
//            {
//                new Player("Player1", Color.blue),
//                new Player("Player2", Color.red),
//                new Player("Player3", Color.white),
//                new Player("Player4", Color.yellow)
//            };

//        players[0].setResourceAmount(RESOURCETYPE.WHEAT, 10);
//        players[0].setResourceAmount(RESOURCETYPE.WOOD, 10);
//        players[0].setResourceAmount(RESOURCETYPE.SHEEP, 10);
//        players[0].setResourceAmount(RESOURCETYPE.BRICK, 10);
//        players[0].setResourceAmount(RESOURCETYPE.ORE, 10);

//        players[1].setResourceAmount(RESOURCETYPE.WHEAT, 10);
//        players[1].setResourceAmount(RESOURCETYPE.WOOD, 10);
//        players[1].setResourceAmount(RESOURCETYPE.SHEEP, 10);
//        players[1].setResourceAmount(RESOURCETYPE.BRICK, 10);
//        players[1].setResourceAmount(RESOURCETYPE.ORE, 10);

//        players[2].setResourceAmount(RESOURCETYPE.WHEAT, 10);
//        players[2].setResourceAmount(RESOURCETYPE.WOOD, 10);
//        players[2].setResourceAmount(RESOURCETYPE.SHEEP, 10);
//        players[2].setResourceAmount(RESOURCETYPE.BRICK, 10);
//        players[2].setResourceAmount(RESOURCETYPE.ORE, 10);

//        players[3].setResourceAmount(RESOURCETYPE.WHEAT, 10);
//        players[3].setResourceAmount(RESOURCETYPE.WOOD, 10);
//        players[3].setResourceAmount(RESOURCETYPE.SHEEP, 10);
//        players[3].setResourceAmount(RESOURCETYPE.BRICK, 10);
//        players[3].setResourceAmount(RESOURCETYPE.ORE, 10);

//        currentPlayer = 0;

//        showCurrentPlayer.GetComponent<Image>().color = players[currentPlayer].GetColor();
//        showCurrentPlayer.transform.GetChild(0).GetComponent<Text>().text = players[currentPlayer].GetName();
//        ChangeRessourcesOutput(players[currentPlayer]);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hit;

//        if (!TradeMenu.isActive()) //that nobody can build cities by accident (while trading)
//        {
//            if (Physics.Raycast(ray, out hit, 100f))
//            {
//                if (Input.GetMouseButtonDown(0))
//                {

//                    if (hit.collider.tag == "VillageSlot")
//                    {

//                        Debug.Log("Village: " + hit.transform.position);
//                        if (players[currentPlayer].canBuildVillage())
//                        {

//                            //Color color = players[currentPlayer].GetColor();
//                            BuildVillage(hit.transform.position + new Vector3(0, 0.065f, 0));
//                            Destroy(hit.transform.gameObject);
//                            players[currentPlayer].buyVillage();
//                            ChangeRessourcesOutput(players[currentPlayer]);
//                        }
//                        else Debug.Log("Not enough ressources");
//                    }

//                    else if (hit.collider.tag == "Village")
//                    {

//                        Debug.Log("City: " + hit.transform.position);
//                        if (players[currentPlayer].canBuildCity())
//                        {

//                            //Color color = players[currentPlayer].GetColor();
//                            BuildCity(hit.transform.position);
//                            Destroy(hit.transform.gameObject);
//                            players[currentPlayer].buyCity();
//                            ChangeRessourcesOutput(players[currentPlayer]);
//                        }
//                        else Debug.Log("Not enough ressources");
//                    }

//                    else if (hit.collider.tag == "RoadSlot")
//                    {

//                        Debug.Log("Road: " + hit.transform.position);
//                        if (players[currentPlayer].canBuildStreet())
//                        {

//                            //Color color = players[currentPlayer].GetColor();
//                            BuildRoad(hit.transform.position + new Vector3(0, 0.065f, 0), hit.transform.rotation);
//                            Destroy(hit.transform.gameObject);
//                            players[currentPlayer].buyStreet();
//                            ChangeRessourcesOutput(players[currentPlayer]);
//                        }
//                        else Debug.Log("Not enough resources");
//                    }
//                }
//            }
//        }
//        else ChangeRessourcesOutput(players[currentPlayer]);
//    }

//    public void BuildVillage(Vector3 position)
//    {

//        Color c = players[currentPlayer].GetColor();

//        if (c.Equals(Color.blue))
//        {
//            Instantiate(villageBlue, position, Quaternion.identity);
//        }
//        else if (c.Equals(Color.red))
//        {
//            Instantiate(villageRed, position, Quaternion.identity);
//        }
//        else if (c.Equals(Color.white))
//        {
//            Instantiate(villageWhite, position, Quaternion.identity);
//        }
//        else if (c.Equals(Color.yellow))
//        {
//            Instantiate(villageYellow, position, Quaternion.identity);
//        }
//    }

//    public void BuildCity(Vector3 position)
//    {

//        Color c = players[currentPlayer].GetColor();

//        if (c.Equals(Color.blue))
//        {
//            Instantiate(cityBlue, position, Quaternion.identity);
//        }
//        else if (c.Equals(Color.red))
//        {
//            Instantiate(cityRed, position, Quaternion.identity);
//        }
//        else if (c.Equals(Color.white))
//        {
//            Instantiate(cityWhite, position, Quaternion.identity);
//        }
//        else if (c.Equals(Color.yellow))
//        {
//            Instantiate(cityYellow, position, Quaternion.identity);
//        }
//    }

//    public void BuildRoad(Vector3 position, Quaternion rotation)
//    {

//        Color c = players[currentPlayer].GetColor();

//        if (c.Equals(Color.blue))
//        {
//            Instantiate(roadBlue, position, rotation);
//        }
//        else if (c.Equals(Color.red))
//        {
//            Instantiate(roadRed, position, rotation);
//        }
//        else if (c.Equals(Color.white))
//        {
//            Instantiate(roadWhite, position, rotation);
//        }
//        else if (c.Equals(Color.yellow))
//        {
//            Instantiate(roadYellow, position, rotation);
//        }
//    }


//    public void NextPlayer()
//    {
//        Debug.Log("NextPlayer in GameController is called");
//        clientRequest.requestEndTurn();

//        if (currentPlayer == players.Length - 1)
//        {
//            currentPlayer = 0;
//        }
//        else
//        {
//            currentPlayer++;
//        }

//        showCurrentPlayer.GetComponent<Image>().color = players[currentPlayer].GetColor();
//        showCurrentPlayer.transform.GetChild(0).GetComponent<Text>().text = players[currentPlayer].GetName();
//        ChangeRessourcesOutput(players[currentPlayer]);
//    }

//    private void ChangeRessourcesOutput(Player player)
//    {
//        bricksText.text = player.getResourceAmount(RESOURCETYPE.BRICK).ToString();
//        oreText.text = player.getResourceAmount(RESOURCETYPE.ORE).ToString();
//        sheepText.text = player.getResourceAmount(RESOURCETYPE.SHEEP).ToString();
//        wheatText.text = player.getResourceAmount(RESOURCETYPE.WHEAT).ToString();
//        woodText.text = player.getResourceAmount(RESOURCETYPE.WOOD).ToString();
//    }

//    public static int getCurrentPlayer()
//    {
//        return currentPlayer;
//    }

//    public static Player[] getPlayers()
//    {
//        return players;
//    }
//}
