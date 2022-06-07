using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    public class Form
    {        
        [JsonProperty("num")]
        public string Num { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("vibor")]
        public string Vibor { get; set; }

        [JsonProperty("inn")]        
        public string Inn { get; set; }

        [JsonProperty("baseRequestId")]
        public string BaseRequestId { get; set; }

        [JsonProperty("switchDeclarant")]
        public string SwitchDeclarant { get; set; }        
    }
}
