using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    public class FnsvipResponse
    {
        [JsonProperty("@xmlns:ns1")]
        public string Xmlns { get; set; }
        [JsonProperty("@ИдДок")]
        public string DocId { get; set; }
        [JsonProperty("ns1:КодОбр")]
        public string CodeTemp { get; set; }
    }
}
