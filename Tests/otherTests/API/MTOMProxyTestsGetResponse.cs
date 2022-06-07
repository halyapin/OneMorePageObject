using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class MTOMProxyTestsGetResponse : TestBase
    {
        [Test, Category("API")]
        public void TestGetResponsetXmlProxyMtom()
        {
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:GetResponse";
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
            var responseFromGate = app.Gate.SendRequestPostUnique("POST", contentType, headers, $"/smev/send/{recidEndpoints}", "Files\\getrequest-mtom.xml");

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
            }
            else Assert.IsFalse(getResponseNode.HasChildNodes, "При запросе GetResponse потребителем шлюз вернул пустой ответ. Для корректной работы теста необходимо убедиться, что на шлюзе есть неподтвержденные неэкспортированные ответ для потребителя.");
        }
    }
}
