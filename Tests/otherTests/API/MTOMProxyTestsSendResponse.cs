using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class MTOMProxySendResponseTest : TestBase
    {
        [Test, Category("API")]
        public void TestSendResponseXmlProxyMtom()
        {
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:SendResponse";
            headers["Accept"] = "text/xml, multipart/related";
            headers["User-Agent"] = "JAX-WS RI 2.2.1-b01-";
            headers["Pragma"] = "no-cache";
            headers["node_id"] = "Autotest";
            var contentType = "multipart/related;start=\"<rootpart*5a9601fa-2f8a-429b-991a-e750c5b9ebc2@example.jaxws.sun.com>\";type=\"application/xop+xml\";boundary=\"uuid:5a9601fa-2f8a-429b-991a-e750c5b9ebc2\";start-info=\"text/xml\"";
            string recidEndpoints = app.EndpointNameForProxyMock;
            //если метод api/smev/send/
            if (recidEndpoints is null) { }

            //если метод api/smev/send/id контура
            else
            {
                recidEndpoints = app.Gate.GetRecidEndpointsFromDB(recidEndpoints);
            }
            var responseFromGate = app.Gate.SendRequestPostUnique("POST", contentType, headers, $"/smev/send/{recidEndpoints}", "Files\\sendresponse-mtom.xml");

            var messageId = app.Gate.GetValueFromTag("MessageId", responseFromGate);

            //проверяем что при отправке запроса был успешно получен синхронный ответ (признак успешной отправки)
            app.Gate.CheckingSuccessSending(responseFromGate, messageId);

            //проверяем, что в ответ на запрос был получен ответ
            app.Gate.CheckingSuccessGetResponse(responseFromGate, messageId);
        }
    }
}
