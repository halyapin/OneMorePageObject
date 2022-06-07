using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    public class RequestXml
    {
        public FnsvipRequest FnsvipRequest { get; set; }
    }

    public class FnsvipRequest
    {
        public string Num { get; set; }
        public string Guid { get; set; }
        public RequestForm Form { get; set; }
    }

    public class RequestForm
    {
        public string Inn { get; set; }
    }
}
