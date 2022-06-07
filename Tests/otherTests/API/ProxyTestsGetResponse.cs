using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    class GetResponseProxy : TestBase
    {
        public string messageId { get; set; }

        [Test, Order(1), Category("API")]
        public void SendGetResponse()
        {
            //хедеры для GetRequest запроса
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:GetResponse";
            headers["node_id"] = "Autotest";

            string recidEndpoints = app.EndpointNameForProxySmev;
            //выбираем по какому методу будет отправляться прокси запрос
            //если метод api/smev/send/
            if (recidEndpoints is null) { }

            //если метод api/smev/send/id контура
            else
            {
                recidEndpoints = app.Gate.GetRecidEndpointsFromDB(recidEndpoints);
            }

            //делаем POST запрос GetRequest
            var responseFromGate = app.Gate.SendRequestPostUnique("POST", "text/xml", headers, "/smev/send/" + recidEndpoints + "", "Files\\request-getrequest.xml");
            Console.WriteLine(responseFromGate);

            var lines = app.Gate.GetLinesFile("response/getresponse.xml", responseFromGate);

            foreach (var def in app.Gate.GetDefsFromGetRequest(lines))
                Assert.AreEqual("--", def, "Отсуствуют дефисы uuid (2.4.1.1)");

            foreach (var uuid in app.Gate.GetUuidsFromGetRequest(lines))
                Assert.NotNull(uuid, "Отсуствуют uuid (2.4.1.1)");

            foreach (var item in app.Gate.GetHeadersForGetRequest(lines))
                Assert.NotNull(item, "Отсуствуют заголовки запроса (2.4.1.1)");

            try
            {
                //получаем messageId, messageType из ответа
                messageId = MtomRegex.GetValueFromTag("MessageID", responseFromGate);
                var messageType = MtomRegex.GetValueFromTag("MessageType", responseFromGate);
                Assert.IsNotNull(messageId, "messageId равен null (2.4.1.1)");
                Assert.AreEqual("RESPONSE", messageType, "requestType не равен RESPONSE (2.4.1.1)");
            }
            catch (Exception)
            {
                Assert.Fail("При запросе GetResponse потребителем шлюз вернул пустой ответ. Для корректной работы теста необходимо убедиться, что на шлюзе есть неподтвержденные неэкспортированные ответ для потребителя.");
            }
        }
    }
}
