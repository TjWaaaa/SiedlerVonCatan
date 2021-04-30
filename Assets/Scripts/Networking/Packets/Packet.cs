using Newtonsoft.Json;

namespace Networking
{
    
    public abstract class Packet
    {
        public string myPlayerName { get; set; }
        
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string myPlayerColor { get; set; }

        public int[] buildPosition { get; set; } // (x,y)
        public int buildType { get; set; } // building i want to build
    }
}