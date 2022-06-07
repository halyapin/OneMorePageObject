using System;
using System.Collections.Generic;

namespace OneMorePageObject
{
    public partial class RsmevSignservices
    {
        public RsmevSignservices()
        {
            RsmevEndpoints = new HashSet<RsmevEndpoints>();
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
        public int ServiceTypeEnum { get; set; }
        public string Url { get; set; }
        public string Thumbprint { get; set; }
        public bool UseAsDefault { get; set; }
        public string Mr { get; set; }

        public virtual ICollection<RsmevEndpoints> RsmevEndpoints { get; set; }
    }
}
