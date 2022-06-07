using System;
using System.Collections.Generic;

namespace OneMorePageObject
{
    public partial class RsmevEndpoints
    {
        public RsmevEndpoints()
        {
            RsmevRequests = new HashSet<RsmevRequests>();
            RsmevResponses = new HashSet<RsmevResponses>();
        }

        public Guid Recid { get; set; }
        public string Recname { get; set; }
        public string Recdescription { get; set; }
        public DateTime Reccreated { get; set; }
        public DateTime Recupdated { get; set; }
        public string Reccreatedby { get; set; }
        public string Recupdatedby { get; set; }
        public int Recstate { get; set; }
        public string Reccode { get; set; }
        public string Url { get; set; }
        public string SchemaVersion { get; set; }
        public bool IsSendRequest { get; set; }
        public bool IsGetResponseRequest { get; set; }
        public bool IsGetRequestRequest { get; set; }
        public bool IsDictEnabled { get; set; }
        public string WaitingTimeouts { get; set; }
        public Guid SignServiceId { get; set; }
        public int MaxRequestLimit { get; set; }
        public bool AckOnlySelfRequest { get; set; }
        public bool IgnoreFilesInResponse { get; set; }
        public bool IsThrowAckNotfound { get; set; }
        public bool NeverAck { get; set; }
        public string NodeId { get;  set; }
        public bool UseAsDefault { get; set; }
        public string FtpUrl { get; set; }
        public string FtpUser { get; set; }
        public string FtpPass { get; set; }
        public bool GetOnlyMyData { get; set; }
        public bool GetRequestAsRealProxy { get; set; }

        public virtual RsmevSignservices SignService { get; set; }
        public virtual ICollection<RsmevRequests> RsmevRequests { get; set; }
        public virtual ICollection<RsmevResponses> RsmevResponses { get; set; }
        
    }
}
