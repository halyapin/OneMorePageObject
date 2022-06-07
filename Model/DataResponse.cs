using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OneMorePageObject
{
    public class DataResponse
    {
        [JsonProperty("ns1:FNSVipIPResponse")]
        public FnsvipResponse FnsvipResponse { get; set; }
    }
}
