using System.Collections;
using System.Collections.Generic;
using Resource;
using Trade;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject showCurrentPlayer;

    private static Player[] players;

    private static int currentPlayer;

    private Builder builder;

    public GameObject tradeMenu;

    public GameObject bricksText;
    public GameObject oreText;
    public GameObject sheepText;
    public GameObject wheatText;
    public GameObject woodText;

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

    // Start is called before the first frame update
    void Start()
    {
        //builder = new Builder();

        players = new Player[]
            {
                new Player("Player1", Color.blue),
                new Player("Player2", Color.red),
                new Player("Player3", Color.white),
                new Player("Player4", Color.yellow)
            };

        players[0].setResourceAmount(RESOURCE.ORE, 5);
        players[0].setResourceAmount(RESOURCE.WOOD, 5);

        players[1].setResourceAmount(RESOURCE.ORE, 5);
        players[1].setResourceAmount(RESOURCE.WOOD, 5);
        players[1].setResourceAmount(RESOURCE.SHEEP, 1);
        players[1].setResourceAmount(RESOURCE.BRICK, 2);

        currentPlayer = 0;

        showCurrentPlayer.GetComponent<Image>().color = players[currentPlayer].GetColor();
        showCurrentPlayer.transform.GetChild(0).GetComponent<Text>().text = players[currentPlayer].GetName();
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
                        if (players[currentPlayer].canBuildVillage())
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
                        if (players[currentPlayer].canBuildCity())
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
                        if (players[currentPlayer].canBuildStreet())
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

        Color c = players[currentPlayer].GetColor();

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

        Color c = players[currentPlayer].GetColor();

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

        Color c = players[currentPlayer].GetColor();

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
        if (currentPlayer == players.Length - 1)
        {
            currentPlayer = 0;
        }
        else
        {
            currentPlayer++;
        }

        showCurrentPlayer.GetComponent<Image>().color = players[currentPlayer].GetColor();
        showCurrentPlayer.transform.GetChild(0).GetComponent<Text>().text = players[currentPlayer].GetName();
        ChangeRessourcesOutput(players[currentPlayer]);
    }

    private void ChangeRessourcesOutput(Player player)
    {
        bricksText.GetComponent<Text>().text = player.getResourceAmount(RESOURCE.BRICK).ToString();
        oreText.GetComponent<Text>().text = player.getResourceAmount(RESOURCE.ORE).ToString();
        sheepText.GetComponent<Text>().text = player.getResourceAmount(RESOURCE.SHEEP).ToString();
        wheatText.GetComponent<Text>().text = player.getResourceAmount(RESOURCE.WHEAT).ToString();
        woodText.GetComponent<Text>().text = player.getResourceAmount(RESOURCE.WOOD).ToString();
    }

    public static int getCurrentPlayer()
    {
        return currentPlayer;
    }

    public static Player[] getPlayers()
    {
        return players;
    }
}
