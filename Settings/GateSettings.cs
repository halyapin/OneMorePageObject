using Newtonsoft.Json;
using System.Collections.Generic;

namespace OneMorePageObject
{
    public class GateSettings
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("endPoints")]
        public List<string> EndPoints { get; set; }

        [JsonProperty("isRemoteSign")]
        public bool IsRemoteServiceSign { get; set; }

        [JsonProperty("chooseEndpointsForProxySmev")]
        public string ChooseEndpointsForProxySmev { get; set; }

        [JsonProperty("chooseEndpointsForProxyMock")]
        public string ChooseEndpointsForProxyMock { get; set; }

        [JsonProperty("choiceEndpointsForSignService")]
        public string ChoiceEndpointsForSignService { get; set; }
    }
}
