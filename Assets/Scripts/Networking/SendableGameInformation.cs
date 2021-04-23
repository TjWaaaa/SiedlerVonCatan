using Newtonsoft.Json;

namespace Networking
{
    public class SendableGameInformation
    {
        private MyJson myJson = new MyJson();


        private class MyJson
        {
            public string playerName { get; set; }
            public string playerColor { get; set; }
            public int playerNumber { get; set; }
        }


        public SendableGameInformation(string playerName)
        {
            myJson.playerName = playerName;
        }

        public SendableGameInformation(string playerName, string playerColor, int playerNumber)
        {
            myJson.playerName = playerName;
            myJson.playerColor = playerColor;
            myJson.playerNumber = playerNumber;
        }
         

        public string objectToJsonString()
        {
            return JsonConvert.SerializeObject(myJson);
        }


        public SendableGameInformation jsonToObject(string jsonString)
        {
            return JsonConvert.DeserializeObject<SendableGameInformation>(jsonString);
        }
    }
}