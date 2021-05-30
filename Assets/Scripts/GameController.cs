
using TMPro;
using UnityEngine;
using Networking.Communication;
using Player;
using Enums;

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
    }
}
