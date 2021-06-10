using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Networking.Package
{
    public class PacketSerializer
    {
        public static string objectToJsonString(object packet)
        {
            string json = null;
            try
            {
                json =  JsonConvert.SerializeObject(packet, 
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }); // remove all null values from result string (this is soooo sexy)
            }
            catch (JsonReaderException e)
            {
                Debug.LogError("Error while serializing object\n" + e.Message);
            }

            return json;
        }


        public static Packet jsonToObject(string jsonString)
        {
            Packet packet = null;
            try
            {
                packet = JsonConvert.DeserializeObject<Packet>(jsonString);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while deserializing json\n" + e.Message); 
            }

            return packet;
        }
    }
}