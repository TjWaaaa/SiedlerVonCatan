using Newtonsoft.Json;

namespace Networking.Package
{
    public class PacketSerializer
    {
        public static string objectToJsonString(object packet)
        {
            return JsonConvert.SerializeObject(packet, 
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }); // remove all null values from result string (this is soooo sexy)
        }


        public static Packet jsonToObject(string jsonString)
        {
<<<<<<< Updated upstream
            return JsonConvert.DeserializeObject<Packet>(jsonString);
=======
            Packet packet = null;
            try
            {
                packet = JsonConvert.DeserializeObject<Packet>(jsonString);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while deserializing json\n" + e.Message); 
                Debug.LogError(jsonString);
            }

            return packet;
>>>>>>> Stashed changes
        }
    }
}