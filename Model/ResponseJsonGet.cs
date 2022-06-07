using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    public class ResponseJsonGet
    {
        public string RequestId { get; set; }
        public string SmevId { get; set; }
        public string ResponseXml { get; set; }
        public DataResponse Data { get; set; }
        public string Error { get; set; }
    }
}
