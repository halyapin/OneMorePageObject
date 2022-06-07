using System;
using System.Collections.Generic;

namespace OneMorePageObject
{
    public partial class RsmevRequests
    {
        public Guid Recid { get; set; }
        public string Recname { get; set; }
        public string Recdescription { get; set; }
        public DateTime Reccreated { get; set; }
        public string Reccreatedby { get; set; }
        public string Recupdatedby { get; set; }
        public int Recstate { get; set; }
        public string Reccode { get; set; }
        public DateTime Recupdated { get; set; }
        public string RequestIncoming { get; set; }
        public Guid? RequestId { get; set; }
        public string RequestBeforeSign { get; set; }
        public string RequestSigned { get; set; }
        public bool IsRequestSended { get; set; }
        public string Error { get; set; }
        public bool IsResponseExists { get; set; }
        public byte[] ResponseRaw { get; set; }
        public string Response { get; set; }
        public int ResponseStatus { get; set; }
        public DateTime? ResponseDelivery { get; set; }
        public DateTime? ResponseSending { get; set; }
        public int? SendTryCount { get; set; }
        public byte[] RequestResponseRaw { get; set; }
        public bool IsSending { get; set; }
        public bool IsProviderResponse { get; set; }
        public Guid? ReplyToId { get; set; }
        public string ReplyTo { get; set; }
        public bool IsFinalRequest { get; set; }
        public string NodeId { get; set; }
        public string CertSubject { get; set; }
        public string CertThumbprint { get; set; }
        public string ResponseXml { get; set; }
        public Guid? EndpointId { get; set; }
        public string Namespace { get; set; }
        public string RootElement { get; set; }
        public Guid? LastResponseId { get; set; }
        public bool? IsInternalRequest { get; set; }

        public virtual RsmevEndpoints Endpoint { get; set; }
    }
}
