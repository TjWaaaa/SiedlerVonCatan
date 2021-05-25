﻿namespace Enums
{
    public enum COMMUNICATION_METHODS
    {
        // it's going to be GRAAAAAAAAND:D
        
        //Client tell Server what to call:
        HANDLE_REQUEST_JOIN_LOBBY, //0
        HANDLE_PLAYER_READY, //1
        HANDLE_BEGIN_ROUND, //2
        HANDLE_TRADE_BANK, //3
        HANDLE_BUILD, //4
        HANDLE_TRADE_PORT, //5
        HANDLE_BUY_DEVELOPMENT, //6
        HANDLE_PLAY_DEVELOPMENT, //7
        HANDLE_END_TURN, //8
        HANDLE_CLIENT_DISCONNECT_SERVER_CALL, //9
        // Server tell Client what to call:
        HANDLE_CLIENT_JOINED, //10
        HANDLE_GAMESTART_INITIALIZE, //11
        HANDLE_DISTRIBUTE_RESOURCES, //12
        HANDLE_OBJECT_PLACEMENT, //13
        HANDLE_NEXT_PLAYER, //14
        HANDLE_VICTORY, //15
        HANDLE_PLAYER_TO_PLAYER_REQUEST, //16
        HANDLE_CLIENT_DISCONNECT, //17
        
        HANDLE_REJECTION, //18
        HANDLE_PLAYER_READY_NOTIFICATION, //19
        HANDLE_ACCEPT_BEGIN_ROUND, //20
        HANDLE_ACCEPT_TRADE_BANK, //21
        HANDLE_ACCEPT_TRADE_PORT, //22
        HANDLE_ACCEPT_BUILD, //23
        HANDLE_GET_RESOURCES, //24
        HANDLE_ACCEPT_BUY_DEVELOPMENT_CARD, //25
        HANDLE_ACCEPT_PLAY_DEVELOPMENT_CARD, //26
        HANDLE_UPDATE_RP //27
    }
}