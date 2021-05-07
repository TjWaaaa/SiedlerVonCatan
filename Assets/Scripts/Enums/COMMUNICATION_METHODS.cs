namespace Enums
{
    public enum COMMUNICATION_METHODS
    {
        // it's going to be GRAAAAAAAAND:D
        
        //Client tell Server what to call:
        handleBeginRound,
        handleTradeBank,
        handleBuild,
        requestTradePort,
        handleBuyDevelopement,
        handlePlayDevelopement,
        handleEndTurn,
        handleClientDisconnectServerCall,
        // Server tell Client what to call:
        handleClientJoined,
        handleGameStartInitialize,
        distributeResources,
        handleObjectPlacement,
        handleNextPlayer,
        handleVictory,
        hanldePlayerToPlayerTradeRequest,
        handleClientDisconnect,
        
        handleRejection,
        handleAccpetBeginRound,
        handleAcceptTradeBank,
        handleAcceptTradePort,
        handleAcceptBuild,
        handleGetResources,
        handleAcceptBuyDevelopement,
        handleAcceptPlayDevelopement,
        handleAcceptEndTurn
    }
}