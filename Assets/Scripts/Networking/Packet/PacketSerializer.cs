using Newtonsoft.Json;

namespace Networking
{
    public class PacketSerializer
    {
        public static string objectToJsonString(object packet)
        {
            return JsonConvert.SerializeObject(packet, 
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }); // remove all null values from result string (this is soooo sexy)
        }


        public static object jsonToObject(string jsonString)
        {
            return JsonConvert.DeserializeObject<object>(jsonString);
        }
    }
}