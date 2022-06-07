using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace OneMorePageObject
{
    class MtomConverter
    {
        public static string GetResponse(WebResponse webResponse, string addHeaders = null)
		{
			using (var webResponseStream = webResponse.GetResponseStream())
			{
				try
				{
					if (webResponse.ContentType.ToLower().Contains("multipart"))
					{
						// непонятный параметр parentTagResponse заменили на addHeaders, от которого зависит нужно ли добавлять заголовки
						string response = string.Empty;
						if (addHeaders == null)
						{
							response = webResponse.Headers.AllKeys.Where(item => item == "Content-Type")
								.Aggregate("MIME-Version: 1.0\r\n",
									(current, item) => current + (item + ": " + webResponse.Headers[item] + "\r\n"));
							response += "\r\n";
						}
						
						using (var mstream = new MemoryStream())
						{
							var responseBytes = Encoding.UTF8.GetBytes(response);
							mstream.Write(responseBytes, 0, responseBytes.Length);
							webResponseStream.CopyTo(mstream);
							mstream.Position = 0;
							var origResponse = MtomToCanonicalXml(mstream);
							return origResponse;
						}
					}
					
					using (TextReader tRes = new StreamReader(webResponseStream, new UTF8Encoding(false)))
					{
						var origResponse = tRes.ReadToEnd();
						return origResponse;
					}
				}
				catch (Exception ex)
				{
					return "TATATA";
				}
			}
		}

		private static string MtomToCanonicalXml(Stream ms)
		{
			var reader = XmlDictionaryReader.CreateMtomReader(ms, Encoding.UTF8, XmlDictionaryReaderQuotas.Max);
			var doc = new XmlDocument();
			doc.Load(reader);
			return doc.OuterXml;
		}
    }
}
