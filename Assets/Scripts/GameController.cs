using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject showCurrentPlayer;

    private Player[] players;

    private int currentPlayer;

    private Builder builder;

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

        players[0].SetBricks(5);
        players[0].SetWood(5);

        players[1].SetBricks(2);
        players[1].SetOre(3);
        players[1].SetSheep(2);
        players[1].SetWheat(4);
        players[1].SetWood(2);

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

        if (Physics.Raycast(ray, out hit, 100f)) {
            if (Input.GetMouseButtonDown(0)) {

                if (hit.collider.tag == "VillageSlot") {

                    Debug.Log("Village: " + hit.transform.position);
                    if (PayRessources(players[currentPlayer], 1, 0, 1, 1, 1)) {

                        //Color color = players[currentPlayer].GetColor();
                        BuildVillage(hit.transform.position + new Vector3(0, 0.065f, 0));
                        Destroy(hit.transform.gameObject);
                        ChangeRessourcesOutput(players[currentPlayer]);
                    }
                    else Debug.Log("Not enough ressources");
                }

                else if (hit.collider.tag == "Village") {

                    Debug.Log("City: " + hit.transform.position);
                    if (PayRessources(players[currentPlayer], 0, 3, 0, 2, 0)) {

                        //Color color = players[currentPlayer].GetColor();
                        BuildCity(hit.transform.position);
                        Destroy(hit.transform.gameObject);
                        ChangeRessourcesOutput(players[currentPlayer]);
                    }
                    else Debug.Log("Not enough ressources");
                }
                
                else if (hit.collider.tag == "RoadSlot") {

                    Debug.Log("Road: " + hit.transform.position);
                    if (PayRessources(players[currentPlayer], 1, 0, 0, 0, 1)) {

                        //Color color = players[currentPlayer].GetColor();
                        BuildRoad(hit.transform.position + new Vector3(0, 0.065f, 0), hit.transform.rotation);
                        Destroy(hit.transform.gameObject);
                        ChangeRessourcesOutput(players[currentPlayer]);
                    }
                    else Debug.Log("Not enough ressources");
                }
            }
        }
    }

    public void BuildVillage(Vector3 position) {

        Color c = players[currentPlayer].GetColor();
        
        if (c.Equals(Color.blue)) {
            Instantiate(villageBlue, position, Quaternion.identity);
        }
        else if (c.Equals(Color.red)) {
            Instantiate(villageRed, position, Quaternion.identity);
        }
        else if (c.Equals(Color.white)) {
            Instantiate(villageWhite, position, Quaternion.identity);
        }
        else if (c.Equals(Color.yellow)) {
            Instantiate(villageYellow, position, Quaternion.identity);
        }
    }

    public void BuildCity(Vector3 position) {

        Color c = players[currentPlayer].GetColor();
        
        if (c.Equals(Color.blue)) {
            Instantiate(cityBlue, position, Quaternion.identity);
        }
        else if (c.Equals(Color.red)) {
            Instantiate(cityRed, position, Quaternion.identity);
        }
        else if (c.Equals(Color.white)) {
            Instantiate(cityWhite, position, Quaternion.identity);
        }
        else if (c.Equals(Color.yellow)) {
            Instantiate(cityYellow, position, Quaternion.identity);
        }
    }

    public void BuildRoad(Vector3 position, Quaternion rotation) {

        Color c = players[currentPlayer].GetColor();
        
        if (c.Equals(Color.blue)) {
            Instantiate(roadBlue, position, rotation);
        }
        else if (c.Equals(Color.red)) {
            Instantiate(roadRed, position, rotation);
        }
        else if (c.Equals(Color.white)) {
            Instantiate(roadWhite, position, rotation);
        }
        else if (c.Equals(Color.yellow)) {
            Instantiate(roadYellow, position, rotation);
        }
    }

    private bool PayRessources(Player player, int bricks, int ore, int sheep, int wheat, int wood) {

        if (player.GetBricks() >= bricks
            && player.GetOre() >= ore
            && player.GetSheep() >= sheep
            && player.GetWheat() >= wheat
            && player.GetWood() >= wood) {

            player.SetBricks(player.GetBricks() - bricks);
            player.SetOre(player.GetOre() - ore);
            player.SetSheep(player.GetSheep() - sheep);
            player.SetWheat(player.GetWheat() - wheat);
            player.SetWood(player.GetWood() - wood);
            ChangeRessourcesOutput(player);
            return true;
        }
        else return false;
    }

    public void NextPlayer() {
        if (currentPlayer == players.Length - 1) {
            currentPlayer = 0;
        }
        else {
            currentPlayer++;
        }

        showCurrentPlayer.GetComponent<Image>().color = players[currentPlayer].GetColor();
        showCurrentPlayer.transform.GetChild(0).GetComponent<Text>().text = players[currentPlayer].GetName();
        ChangeRessourcesOutput(players[currentPlayer]);
    }

    private void ChangeRessourcesOutput(Player player) {
        bricksText.GetComponent<Text>().text = player.GetBricks().ToString();
        oreText.GetComponent<Text>().text = player.GetOre().ToString();
        sheepText.GetComponent<Text>().text = player.GetSheep().ToString();
        wheatText.GetComponent<Text>().text = player.GetWheat().ToString();
        woodText.GetComponent<Text>().text = player.GetWood().ToString();
    }
}
