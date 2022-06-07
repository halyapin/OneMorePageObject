using System;
using System.Collections.Generic;

namespace OneMorePageObject
{
    public partial class RsmevResponses
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
        public bool IsProvider { get; set; }
        public Guid? MessageId { get; set; }
        public Guid? OriginalMessageId { get; set; }
        public DateTime? ResponseDelivery { get; set; }
        public DateTime? ResponseSending { get; set; }
        public string ResponsePrimaryContent { get; set; }
        public int ResponseStatus { get; set; }
        public byte[] ResponseRaw { get; set; }
        public string ResponseText { get; set; }
        public string ReplyTo { get; set; }
        public Guid? TransactionCode { get; set; }
        public string NamespaceUri { get; set; }
        public string MnemonicSender { get; set; }
        public string MnemonicRecipient { get; set; }
        public string RecipientOktmo { get; set; }
        public bool IsParseCompleted { get; set; }
        public string ResponseHash { get; set; }
        public bool IsExported { get; set; }
        public DateTime? ExportDate { get; set; }
        public bool IsExportConfirmed { get; set; }
        public DateTime? ExportConfirmed { get; set; }
        public bool IsExternalConfirmed { get; set; }
        public DateTime? ExternalConfirmedDate { get; set; }
        public bool IsExternalExport { get; set; }
        public DateTime? ExternalExportDate { get; set; }
        public string ExternalExportCertSubject { get; set; }
        public string ExternalExportCertThumb { get; set; }
        public string ResponseContentType { get; set; }
        public bool? NeedToAck { get; set; }
        public bool? AckSended { get; set; }
        public bool? IsForMyRequest { get; set; }
        public string CertSubject { get; set; }
        public string CertThumbprint { get; set; }
        public string AskCertSubject { get; set; }
        public string AskCertThumbprint { get; set; }
        public Guid? NamespaceId { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? EndpointId { get; set; }
        public string RootElement { get; set; }
        public Guid? SmevNamespaceRecId { get; set; }
        public bool? IsInternalRequest { get; set; }

        public virtual RsmevEndpoints Endpoint { get; set; }
    }
}
