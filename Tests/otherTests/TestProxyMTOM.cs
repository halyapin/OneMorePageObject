using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class TestProxyMTOM : TestBase
    {
        [Test]
        public void TestSendRequestXmlProxyMtom()
        {
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:SendRequest";
            headers["Accept"] = "text/xml, multipart/related";
            headers["User-Agent"] = "JAX-WS RI 2.2.1-b01-";
            headers["Pragma"] = "no-cache";
            var contentType = "multipart/related;start=\"<rootpart*5a9601fa-2f8a-429b-991a-e750c5b9ebc2@example.jaxws.sun.com>\";type=\"application/xop+xml\";boundary=\"uuid:5a9601fa-2f8a-429b-991a-e750c5b9ebc2\";start-info=\"text/xml\"";
            string recidEndpoints = app.EndpointNameForProxyMock;
            //если метод api/smev/send/
            if (recidEndpoints is null) { }

            //если метод api/smev/send/id контура
            else
            {
                recidEndpoints = app.Gate.GetRecidEndpointsFromDB(recidEndpoints);
            }

            var responseFromGate = app.Gate.SendRequestPostUnique("POST", contentType, headers, $"/smev/send/{recidEndpoints}", "sendrequest-mtom.xml");

            var messageData = app.Gate.GetMessageDataFromResponse(responseFromGate);
            var tagSendRequestResponse = app.Gate.GetTagSendRequestResponse(responseFromGate);

            var messageId = app.Gate.GetValueFromTag("MessageId", responseFromGate);
            var requestType = app.Gate.GetValueFromTag("MessageType", responseFromGate);
            var status = app.Gate.GetValueFromTag("Status", responseFromGate);

            var xmlns = tagSendRequestResponse.NamespaceURI;

            Assert.IsNotNull(responseFromGate, "Не удалось отправить прокси запрос (2.5.1.1)");

            Assert.AreEqual(xmlns, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1",
                "Namespace не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1 (2.5.1.1)");

            Assert.IsNotNull(messageId, "messageId равен null (2.5.1.1)");
            Assert.AreEqual(requestType, "REQUEST", "requestType не равен REQUEST (2.5.1.1)");
            Assert.AreEqual(status, "requestIsQueued", "requestType не равен requestIsQueued (2.5.1.1)");

            var requestId = new Guid();
            using (var Context = new RdevContext())
            {
                requestId = Context.RsmevRequests.Where(
                    x => x.RequestId == new Guid(messageId)).FirstOrDefault().Recid;
            }

            Thread.Sleep(12000);
            var ResponseGet = app.Gate.SendRequestGet(requestId.ToString());

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)       
            Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (2.5.2.1)");
            Assert.NotNull(ResponseGet.SmevId, "smevId равен null (2.5.2.1)");
            Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (2.5.2.1)");
            Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (2.5.2.1)");

            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            if (ResponseGet.SmevId != null)
                Assert.IsTrue(app.Gate.CheckingResponseFromInterface(ResponseGet.SmevId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.5.3)");
            else Assert.Fail("smevId равен null");
        }

        [Test]
        public void TestSendResponseXmlProxyMtom()
        {
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:SendResponse";
            headers["Accept"] = "text/xml, multipart/related";
            headers["User-Agent"] = "JAX-WS RI 2.2.1-b01-";
            headers["Pragma"] = "no-cache";
            var contentType = "multipart/related;start=\"<rootpart*5a9601fa-2f8a-429b-991a-e750c5b9ebc2@example.jaxws.sun.com>\";type=\"application/xop+xml\";boundary=\"uuid:5a9601fa-2f8a-429b-991a-e750c5b9ebc2\";start-info=\"text/xml\"";

            var responseFromGate = app.Gate.SendRequestPostUnique("POST", contentType, headers, "/smev/send", "sendresponse-mtom.xml");

            var messageData = app.Gate.GetMessageDataFromResponse(responseFromGate);
            var tagSendRequestResponse = app.Gate.GetTagSendRequestResponse(responseFromGate);

            var messageId = messageData[0].InnerText;
            var requestType = messageData[1].InnerText;
            var status = messageData[7].InnerText;

            var xmlns = tagSendRequestResponse.Attributes["xmlns"].Value;
            var xmlns2 = tagSendRequestResponse.Attributes["xmlns:ns2"].Value;
            var xmlns3 = tagSendRequestResponse.Attributes["xmlns:ns3"].Value;

            Assert.IsNotNull(responseFromGate, "Не удалось отправить прокси запрос (2.6.1.1)");

            Assert.AreEqual(xmlns, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1",
                "Атрибут xmlns не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1 (2.6.1.1)");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1",
                "Атрибут xmlns:ns2 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1 (2.6.1.1)");
            Assert.AreEqual(xmlns3, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1",
                "Атрибут xmlns:ns3 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1 (2.6.1.1)");

            Assert.IsNotNull(messageId, "messageId равен null (2.6.1.1)");
            Assert.AreEqual(requestType, "RESPONSE", "requestType не равен RESPONSE (2.6.1.1)");
            Assert.AreEqual(status, "requestIsQueued", "requestType не равен requestIsQueued (2.6.1.1)");

            var requestId = new Guid();
            using (var Context = new RdevContext())
            {
                requestId = Context.RsmevRequests.Where(
                    x => x.RequestId == new Guid(messageId)).FirstOrDefault().Recid;
            }

            Thread.Sleep(12000);
            var ResponseGet = app.Gate.SendRequestGet(requestId.ToString());

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)       
            Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (2.6.2.1)");
            Assert.NotNull(ResponseGet.SmevId, "smevId равен null (2.6.2.1)");
            Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (2.6.2.1)");
            Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (2.6.2.1)");

            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            if (ResponseGet.SmevId != null)
                Assert.IsTrue(app.Gate.CheckingResponseFromInterface(ResponseGet.SmevId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.6.3)");
            else Assert.Fail("smevId равен null");
        }

        [Test]
        public void TestGetRequestXmlProxyMtom()
        {
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:GetRequest";
            headers["Accept"] = "text/xml, multipart/related";
            headers["User-Agent"] = "JAX-WS RI 2.2.1-b01-";
            headers["Pragma"] = "no-cache";
            var contentType = "multipart/related;start=\"<rootpart*5a9601fa-2f8a-429b-991a-e750c5b9ebc2@example.jaxws.sun.com>\";type=\"application/xop+xml\";boundary=\"uuid:5a9601fa-2f8a-429b-991a-e750c5b9ebc2\";start-info=\"text/xml\"";

            var responseFromGate = app.Gate.SendRequestPostUnique("POST", contentType, headers, "/smev/send", "getrequest-mtom.xml");

            var lines = app.Gate.GetLinesFile("response/getresponse.xml", responseFromGate);

            foreach (var def in app.Gate.GetDefsFromGetRequest(lines))
                Assert.AreEqual("--", def, "Отсуствуют дефисы uuid (2.7.1.1)");

            foreach (var uuid in app.Gate.GetUuidsFromGetRequest(lines))
                Assert.NotNull(uuid, "Отсуствуют uuid (2.7.1.1)");

            foreach (var item in app.Gate.GetHeadersForGetRequest(lines))
                Assert.NotNull(item, "Отсуствуют заголовки запроса (2.7.1.1)");

            var getResponseNode = app.Gate.GetMessageData(lines);

            if (getResponseNode.HasChildNodes)
            {
                var metadata = getResponseNode.FirstChild.FirstChild.ChildNodes[2].ChildNodes;
                var messageId = metadata[0].InnerText;
                var requestType = metadata[1].InnerText;
                Assert.IsNotNull(messageId, "messageId равен null (2.7.1.1)");
                Assert.AreEqual(requestType, "RESPONSE", "requestType не равен RESPONSE (2.7.1.1)");
                Assert.IsTrue(app.Gate.CheckingResponseFromInterface(messageId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.7.1.1)");
            }
            else Assert.IsFalse(getResponseNode.HasChildNodes, "В МТОМ нет ответов");
        }

        [Test]
        public void TestGetResponsetXmlProxyMtom()
        {
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:GetResponse";
            headers["Accept"] = "text/xml, multipart/related";
            headers["User-Agent"] = "JAX-WS RI 2.2.1-b01-";
            headers["Pragma"] = "no-cache";
            var contentType = "multipart/related;start=\"<rootpart*5a9601fa-2f8a-429b-991a-e750c5b9ebc2@example.jaxws.sun.com>\";type=\"application/xop+xml\";boundary=\"uuid:5a9601fa-2f8a-429b-991a-e750c5b9ebc2\";start-info=\"text/xml\"";

            var responseFromGate = app.Gate.SendRequestPostUnique("POST", contentType, headers, "/smev/send", "getrequest-mtom.xml");

            var lines = app.Gate.GetLinesFile("response/getresponse.xml", responseFromGate);

            foreach (var def in app.Gate.GetDefsFromGetRequest(lines))
                Assert.AreEqual("--", def, "Отсуствуют дефисы uuid (2.8.1.1)");

            foreach (var uuid in app.Gate.GetUuidsFromGetRequest(lines))
                Assert.NotNull(uuid, "Отсуствуют uuid (2.8.1.1)");

            foreach (var item in app.Gate.GetHeadersForGetRequest(lines))
                Assert.NotNull(item, "Отсуствуют заголовки запроса (2.8.1.1)");

            var getResponseNode = app.Gate.GetMessageData(lines);

            if (getResponseNode.HasChildNodes)
            {
                var metadata = getResponseNode.FirstChild.FirstChild.ChildNodes[2].ChildNodes;
                var messageId = metadata[0].InnerText;
                var requestType = metadata[1].InnerText;
                Assert.IsNotNull(messageId, "messageId равен null (2.8.1.1)");
                Assert.AreEqual(requestType, "RESPONSE", "requestType не равен RESPONSE (2.8.1.1)");
                Assert.IsTrue(app.Gate.CheckingResponseFromInterfaceByMessageId(messageId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.8.1.1)");
            }
            else Assert.IsFalse(getResponseNode.HasChildNodes, "В МТОМ нет ответов");
        }
    }
}
