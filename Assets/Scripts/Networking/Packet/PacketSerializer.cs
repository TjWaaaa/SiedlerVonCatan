using Newtonsoft.Json;

namespace Enums
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
            return JsonConvert.DeserializeObject<Packet>(jsonString);
        }
    }
}