using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;


namespace OneMorePageObject
{
    public partial class RdevContext : DbContext
    {
        public RdevContext()
        {
        }

        public RdevContext(DbContextOptions<RdevContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RsmevEndpoints> RsmevEndpoints { get; set; }
        public virtual DbSet<RsmevRequests> RsmevRequests { get; set; }
        public virtual DbSet<RsmevResponses> RsmevResponses { get; set; }
        public virtual DbSet<RsmevSignservices> RsmevSignservices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var json = File.ReadAllText("settings/settings.json");
            var settings = JsonConvert.DeserializeObject<SettingsJson>(json);

            var host = settings.Database.Host;
            var port = settings.Database.Port;
            var database = settings.Database.DatabaseName;
            var userName = settings.Database.UserName;
            var password = settings.Database.Password;

            var connectionString = $"Host={host};Port={port};Database={database};Username={userName};Password={password};";

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<RsmevEndpoints>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("rsmev___endpoints");

                entity.HasIndex(e => e.SignServiceId);

                entity.Property(e => e.Recid)
                    .HasColumnName("recid")
                    .ValueGeneratedNever();

                entity.Property(e => e.AckOnlySelfRequest).HasColumnName("ack_only_self_request");

                entity.Property(e => e.FtpPass).HasColumnName("ftp_pass");

                entity.Property(e => e.FtpUrl)
                    .IsRequired()
                    .HasColumnName("ftp_url");

                entity.Property(e => e.FtpUser).HasColumnName("ftp_user");

                entity.Property(e => e.GetOnlyMyData).HasColumnName("get_only_my_data");
                entity.Property(e => e.GetRequestAsRealProxy).HasColumnName("get_request_as_real_proxy");

                entity.Property(e => e.IgnoreFilesInResponse).HasColumnName("ignore_files_in_response");

                entity.Property(e => e.IsDictEnabled).HasColumnName("is_dict_enabled");

                entity.Property(e => e.IsGetRequestRequest).HasColumnName("is_get_request_request");

                entity.Property(e => e.IsGetResponseRequest).HasColumnName("is_get_response_request");

                entity.Property(e => e.IsSendRequest).HasColumnName("is_send_request");

                entity.Property(e => e.IsThrowAckNotfound).HasColumnName("is_throw_ack_notfound");

                entity.Property(e => e.MaxRequestLimit).HasColumnName("max_request_limit");

                entity.Property(e => e.NeverAck).HasColumnName("never_ack");
                entity.Property(e => e.NodeId).HasColumnName("node_id");

                entity.Property(e => e.Reccode).HasColumnName("reccode");

                entity.Property(e => e.Reccreated).HasColumnName("reccreated");

                entity.Property(e => e.Reccreatedby).HasColumnName("reccreatedby");

                entity.Property(e => e.Recdescription).HasColumnName("recdescription");

                entity.Property(e => e.Recname)
                    .IsRequired()
                    .HasColumnName("recname");

                entity.Property(e => e.Recstate).HasColumnName("recstate");

                entity.Property(e => e.Recupdated).HasColumnName("recupdated");

                entity.Property(e => e.Recupdatedby).HasColumnName("recupdatedby");

                entity.Property(e => e.SchemaVersion)
                    .IsRequired()
                    .HasColumnName("schema_version");

                entity.Property(e => e.SignServiceId).HasColumnName("sign_service_id");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url");

                entity.Property(e => e.UseAsDefault).HasColumnName("use_as_default");

                entity.Property(e => e.WaitingTimeouts)
                    .IsRequired()
                    .HasColumnName("waiting_timeouts");

                entity.HasOne(d => d.SignService)
                    .WithMany(p => p.RsmevEndpoints)
                    .HasForeignKey(d => d.SignServiceId);
            });

            modelBuilder.Entity<RsmevRequests>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("rsmev___requests");

                //изменил .HasName на .HasDatabaseName

                entity.HasIndex(e => e.CertThumbprint)
                    .HasDatabaseName("ix_requests_certthumbprint");

                entity.HasIndex(e => e.EndpointId)
                    .HasDatabaseName("ix_requests_endpointid");

                entity.HasIndex(e => e.IsRequestSended)
                    .HasDatabaseName("ix_requests_isrequestsended");

                entity.HasIndex(e => e.NodeId)
                    .HasDatabaseName("ix_requests_nodeid");

                entity.HasIndex(e => e.Recupdated)
                    .HasDatabaseName("ix_requests_recupdated");

                entity.HasIndex(e => e.RequestId)
                    .HasDatabaseName("ix_requests_requestid");

                entity.HasIndex(e => new { e.IsRequestSended, e.ResponseStatus })
                    .HasDatabaseName("ix_requests_searchnotanswered");

                entity.HasIndex(e => new { e.EndpointId, e.IsFinalRequest, e.IsRequestSended, e.IsSending })
                    .HasDatabaseName("ix_requests_search1");

                entity.Property(e => e.Recid)
                    .HasColumnName("recid")
                    .ValueGeneratedNever();

                entity.Property(e => e.CertSubject).HasColumnName("cert_subject");

                entity.Property(e => e.CertThumbprint).HasColumnName("cert_thumbprint");

                entity.Property(e => e.EndpointId).HasColumnName("endpoint_id");

                entity.Property(e => e.Error).HasColumnName("error");

                entity.Property(e => e.IsFinalRequest).HasColumnName("is_final_request");

                entity.Property(e => e.IsInternalRequest).HasColumnName("is_internal_request");

                entity.Property(e => e.IsProviderResponse).HasColumnName("is_provider_response");

                entity.Property(e => e.IsRequestSended).HasColumnName("is_request_sended");

                entity.Property(e => e.IsResponseExists).HasColumnName("is_response_exists");

                entity.Property(e => e.IsSending).HasColumnName("is_sending");

                entity.Property(e => e.LastResponseId).HasColumnName("last_response_id");

                entity.Property(e => e.Namespace)
                    .HasColumnName("namespace")
                    .HasMaxLength(512);

                entity.Property(e => e.NodeId).HasColumnName("node_id");

                entity.Property(e => e.Reccode).HasColumnName("reccode");

                entity.Property(e => e.Reccreated).HasColumnName("reccreated");

                entity.Property(e => e.Reccreatedby).HasColumnName("reccreatedby");

                entity.Property(e => e.Recdescription).HasColumnName("recdescription");

                entity.Property(e => e.Recname)
                    .IsRequired()
                    .HasColumnName("recname");

                entity.Property(e => e.Recstate).HasColumnName("recstate");

                entity.Property(e => e.Recupdated).HasColumnName("recupdated");

                entity.Property(e => e.Recupdatedby).HasColumnName("recupdatedby");

                entity.Property(e => e.ReplyTo).HasColumnName("reply_to");

                entity.Property(e => e.ReplyToId).HasColumnName("reply_to_id");

                entity.Property(e => e.RequestBeforeSign).HasColumnName("request_before_sign");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.RequestIncoming)
                    .IsRequired()
                    .HasColumnName("request_incoming");

                entity.Property(e => e.RequestResponseRaw).HasColumnName("request_response_raw");

                entity.Property(e => e.RequestSigned).HasColumnName("request_signed");

                entity.Property(e => e.Response).HasColumnName("response");

                entity.Property(e => e.ResponseDelivery).HasColumnName("response_delivery");

                entity.Property(e => e.ResponseRaw).HasColumnName("response_raw");

                entity.Property(e => e.ResponseSending).HasColumnName("response_sending");

                entity.Property(e => e.ResponseStatus).HasColumnName("response_status");

                entity.Property(e => e.ResponseXml).HasColumnName("response_xml");

                entity.Property(e => e.RootElement)
                    .HasColumnName("root_element")
                    .HasMaxLength(512);

                entity.Property(e => e.SendTryCount).HasColumnName("send_try_count");

                entity.HasOne(d => d.Endpoint)
                    .WithMany(p => p.RsmevRequests)
                    .HasForeignKey(d => d.EndpointId);
            });

            modelBuilder.Entity<RsmevResponses>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("rsmev___responses");

                entity.HasIndex(e => e.EndpointId);

                entity.HasIndex(e => e.MessageId)
                    .HasDatabaseName("ix_responses_message_id");

                entity.HasIndex(e => e.NamespaceId);

                entity.HasIndex(e => e.OriginalMessageId)
                    .HasDatabaseName("ix_responses_original_message_id");

                entity.HasIndex(e => e.Reccreated)
                    .HasDatabaseName("ix_responses_reccreated");

                entity.HasIndex(e => e.ResponseHash)
                      .HasDatabaseName("ix_responses_hash");

                entity.HasIndex(e => new { e.IsExternalConfirmed, e.IsProvider }).HasDatabaseName("ix_responses_isexternalconfirmed");

                entity.HasIndex(e => new { e.IsProvider, e.IsParseCompleted, e.IsExported, e.IsExportConfirmed })
                .HasDatabaseName("ix_responses_getincomings");

                entity.HasIndex(e => new { e.IsProvider, e.IsParseCompleted, e.IsExported, e.IsExportConfirmed, e.NamespaceUri })
                      .HasDatabaseName("ix_responses_getincomings2");

                entity.Property(e => e.Recid)
                    .HasColumnName("recid")
                    .ValueGeneratedNever();

                entity.Property(e => e.AckSended).HasColumnName("ack_sended");

                entity.Property(e => e.AskCertSubject).HasColumnName("ask_cert_subject");

                entity.Property(e => e.AskCertThumbprint).HasColumnName("ask_cert_thumbprint");

                entity.Property(e => e.CertSubject).HasColumnName("cert_subject");

                entity.Property(e => e.CertThumbprint).HasColumnName("cert_thumbprint");

                entity.Property(e => e.EndpointId).HasColumnName("endpoint_id");

                entity.Property(e => e.ExportConfirmed).HasColumnName("export_confirmed");

                entity.Property(e => e.ExportDate).HasColumnName("export_date");

                entity.Property(e => e.ExternalConfirmedDate).HasColumnName("external_confirmed_date");

                entity.Property(e => e.ExternalExportCertSubject).HasColumnName("external_export_cert_subject");

                entity.Property(e => e.ExternalExportCertThumb).HasColumnName("external_export_cert_thumb");

                entity.Property(e => e.ExternalExportDate).HasColumnName("external_export_date");

                entity.Property(e => e.IsExportConfirmed).HasColumnName("is_export_confirmed");

                entity.Property(e => e.IsExported).HasColumnName("is_exported");

                entity.Property(e => e.IsExternalConfirmed).HasColumnName("is_external_confirmed");

                entity.Property(e => e.IsExternalExport).HasColumnName("is_external_export");

                entity.Property(e => e.IsForMyRequest).HasColumnName("is_for_my_request");

                entity.Property(e => e.IsInternalRequest).HasColumnName("is_internal_request");

                entity.Property(e => e.IsParseCompleted).HasColumnName("is_parse_completed");

                entity.Property(e => e.IsProvider).HasColumnName("is_provider");

                entity.Property(e => e.MessageId).HasColumnName("message_id");

                entity.Property(e => e.MnemonicRecipient).HasColumnName("mnemonic_recipient");

                entity.Property(e => e.MnemonicSender).HasColumnName("mnemonic_sender");

                entity.Property(e => e.NamespaceId).HasColumnName("namespace_id");

                entity.Property(e => e.NamespaceUri).HasColumnName("namespace_uri");

                entity.Property(e => e.NeedToAck).HasColumnName("need_to_ack");

                entity.Property(e => e.OriginalMessageId).HasColumnName("original_message_id");

                entity.Property(e => e.Reccode).HasColumnName("reccode");

                entity.Property(e => e.Reccreated).HasColumnName("reccreated");

                entity.Property(e => e.Reccreatedby).HasColumnName("reccreatedby");

                entity.Property(e => e.Recdescription).HasColumnName("recdescription");

                entity.Property(e => e.RecipientOktmo).HasColumnName("recipient_oktmo");

                entity.Property(e => e.Recname)
                    .IsRequired()
                    .HasColumnName("recname");

                entity.Property(e => e.Recstate).HasColumnName("recstate");

                entity.Property(e => e.Recupdated).HasColumnName("recupdated");

                entity.Property(e => e.Recupdatedby).HasColumnName("recupdatedby");

                entity.Property(e => e.ReplyTo).HasColumnName("reply_to");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.ResponseContentType).HasColumnName("response_content_type");

                entity.Property(e => e.ResponseDelivery).HasColumnName("response_delivery");

                entity.Property(e => e.ResponseHash).HasColumnName("response_hash");

                entity.Property(e => e.ResponsePrimaryContent).HasColumnName("response_primary_content");

                entity.Property(e => e.ResponseRaw)
                    .IsRequired()
                    .HasColumnName("response_raw");

                entity.Property(e => e.ResponseSending).HasColumnName("response_sending");

                entity.Property(e => e.ResponseStatus).HasColumnName("response_status");

                entity.Property(e => e.ResponseText).HasColumnName("response_text");

                entity.Property(e => e.RootElement)
                    .HasColumnName("root_element")
                    .HasMaxLength(512);

                entity.Property(e => e.TransactionCode).HasColumnName("transaction_code");

                entity.HasOne(d => d.Endpoint)
                    .WithMany(p => p.RsmevResponses)
                    .HasForeignKey(d => d.EndpointId);
            });

            modelBuilder.Entity<RsmevSignservices>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("rsmev___signservices");

                entity.Property(e => e.Recid)
                    .HasColumnName("recid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Mr).HasColumnName("mr");

                entity.Property(e => e.Reccode).HasColumnName("reccode");

                entity.Property(e => e.Reccreated).HasColumnName("reccreated");

                entity.Property(e => e.Reccreatedby).HasColumnName("reccreatedby");

                entity.Property(e => e.Recdescription).HasColumnName("recdescription");

                entity.Property(e => e.Recname)
                    .IsRequired()
                    .HasColumnName("recname");

                entity.Property(e => e.Recstate).HasColumnName("recstate");

                entity.Property(e => e.Recupdated).HasColumnName("recupdated");

                entity.Property(e => e.Recupdatedby).HasColumnName("recupdatedby");

                entity.Property(e => e.ServiceTypeEnum).HasColumnName("service_type_enum");

                entity.Property(e => e.Thumbprint).HasColumnName("thumbprint");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.Property(e => e.UseAsDefault).HasColumnName("use_as_default");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
