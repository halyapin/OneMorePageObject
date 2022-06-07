using Newtonsoft.Json;

namespace OneMorePageObject
{
    public class SettingsJson
    {
        [JsonProperty("database")]
        public DatabaseSettings Database { get; set; }

        [JsonProperty("gate")]
        public GateSettings Gate { get; set; }
    }
}
