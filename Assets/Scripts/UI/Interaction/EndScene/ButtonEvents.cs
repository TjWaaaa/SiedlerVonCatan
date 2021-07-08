using System.Collections;
using System.Collections.Generic;
using Networking.ClientSide;
using Networking.ServerSide;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        var playAgainButton = GameObject.Find("Canvas/victoryPanel/PlayAgain");
        var quitButton = GameObject.Find("Canvas/victoryPanel/Quit");

        playAgainButton.GetComponent<Button>().onClick.AddListener(playAgain);
        quitButton.GetComponent<Button>().onClick.AddListener(quitGame);
    }


    public void playAgain()
    {
        var factory = GameObject.Find("PrefabFactory");
        Destroy(factory);
        var clientReceive = GameObject.Find("clientReceive(Clone)");
        Destroy(clientReceive);

        Client.shutDownClient();
        Server.shutDownServer();
        SceneManager.LoadScene("0_StartScene");
    }


    public void quitGame()
    {
#if UNITY_EDITOR
        Client.shutDownClient();
        Server.shutDownServer();
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Client.shutDownClient();
        Server.shutDownServer();
        Application.Quit();
#endif
    }
}