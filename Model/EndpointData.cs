using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMorePageObject
{
    public class EndpointData
    {
        public EndpointData()
        {
        }
        public string Name { get; set; }
        public string FtpUrl { get; set; }
        public string FtpPassword { get; set; }
        public string Description { get; set; }
        public string FtpUser { get; set; }
        public string WaitingTimeoutsSec { get; set; }
        public string Url { get; set; }
        public string IsSendRequest { get; set; }
    }
}