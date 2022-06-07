using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    class GetRequestProxy : TestBase
    {
        public string messageId { get; set; }
        /// <summary>
        /// получение запроса методом GetRequest 
        /// </summary>
        [Test, Order(1), Category("API")]
        public void SendGetRequestProxy()
        {
            //хедеры для GetRequest запроса
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:GetRequest";
            headers["node_id"] = "Autotest";

            string recidEndpoints = app.EndpointNameForProxyMock;
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
                Assert.AreEqual("--", def, "Отсуствуют дефисы uuid (2.3.1.1)");

            foreach (var uuid in app.Gate.GetUuidsFromGetRequest(lines))
                Assert.NotNull(uuid, "Отсуствуют uuid (2.3.1.1)");

            foreach (var item in app.Gate.GetHeadersForGetRequest(lines))
                Assert.NotNull(item, "Отсуствуют заголовки запроса (2.3.1.1)");

            try
            {
                //получаем messageId, messageType из ответа
                messageId = app.Gate.GetValueFromTag("MessageId", responseFromGate);
                var messageType = app.Gate.GetValueFromTag("MessageType", responseFromGate);
                Assert.IsNotNull(messageId, "messageId равен null (2.3.1.1)");
                Assert.AreEqual("REQUEST", messageType, "requestType не равен REQUEST (2.3.1.1)");
            }
            catch (Exception)
            {
                Assert.Fail("При запросе GetRequest поставщиком шлюз вернул пустой ответ. Для корректной работы теста необходимо убедиться, что на шлюзе есть неподтвержденные неэкспортированные запросы для поставщика.");
            }
        }
    }
}
