using Resource;

namespace Networking
{
    public class ClientRequest : ClientToServerCommunication
    {
        public void requestBeginRound(ClientToServerCommunication.acceptCallback acceptCallback, ClientToServerCommunication.rejectCallback rejectCallback)
        {
            // alle Daten sammeln
            
            // daten verpacken
            
            // daten senden
            
            // accept oder reject verarbeiten
            
            throw new System.NotImplementedException();
        }

        public void requestTradeBank(RESOURCE offer, RESOURCE expectation, ClientToServerCommunication.acceptCallback acceptCallback,
            ClientToServerCommunication.rejectCallback rejectCallback)
        {
            throw new System.NotImplementedException();
        }

        public void requestBuild(BUYABLES type, int x, int y, ClientToServerCommunication.acceptCallback acceptCallback, ClientToServerCommunication.rejectCallback rejectCallback)
        {
            throw new System.NotImplementedException();
        }

        public void requestBuyDevelopement(ClientToServerCommunication.acceptCallback acceptCallback, ClientToServerCommunication.rejectCallback rejectCallback)
        {
            throw new System.NotImplementedException();
        }

        public void requestPlayDevelopement(ClientToServerCommunication.acceptCallback acceptCallback, ClientToServerCommunication.rejectCallback rejectCallback)
        {
            throw new System.NotImplementedException();
        }

        public void requestEndTurn(ClientToServerCommunication.acceptCallback acceptCallback, ClientToServerCommunication.rejectCallback rejectCallback)
        {
            throw new System.NotImplementedException();
        }
    }
}