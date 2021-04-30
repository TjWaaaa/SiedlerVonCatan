using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameLoop : MonoBehaviour
{
    private Player[] players;
    private Stack<Player> playerStack;
    private int currentplayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<GameController>();
    }

    public void AddPlayer(Player newPlayer)
    {
        playerStack.Push(newPlayer);
    }

    void Pre_GameLoop()
    {
        // PlayerOrder may be implemented later 
        int playerAmount = playerStack.Count();

        // Making an array for the players for this game
        playerStack.ToArray();

        // PlayerLoop up placing village + road
        for (int i=0 ; i< players.Length ; i++)
            {
                placeFirstPieces(players[i]);
            }

        // PlayerLoop down placing village + road

        for (int i=players.Length ; i >= 0; i--)
            {
                placeFirstPieces(players[i]);
            }
    }

    void actualGameLoop()
    {
        if(currentplayer > players.Length - 1)
        {
            currentplayer = 0;
        }
        Debug.Log("Current player: " + currentplayer);

        //throwDices
        //supplyRessources
        //waitForUserAction
        //waitForEndMoveOrTimerRunsOut

        currentplayer ++;
    }

    void placeFirstPieces(Player Player)
    {
        bool placing = true;
        while(placing)
        {

    
            //highLightPossibleCityPositions()
            // Await the placement click, if occupied restart waiting process
            //if(ButtonDown){getComponent.ID}
            if(canPlaceVillage(13))
            {
                //place prefab
                placing = false;
            }
            else
            {
                Debug.Log("you can't place a village here.");
            }
        }
    }
    private bool canPlaceVillage(int cornerPosition)
    {   
        if(true) //has to be implemented later
        {
            return true;
        }
        return false;
    }
}
