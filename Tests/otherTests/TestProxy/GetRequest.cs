using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    class GetRequest : TestBase
    {
        public string messageId { get; set; }

        [Test, Order(1)]
        public void SendGetRequest()
        {
            //хедеры для GetRequest запроса
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:GetRequest";

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
            var responseFromGate = app.Gate.SendRequestPostUnique("POST", "text/xml", headers, "/smev/send/"+ recidEndpoints + "", "request-getrequest.xml");
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
                messageId = MtomRegex.GetValueFromTag("MessageID", responseFromGate);
                var messageType = MtomRegex.GetValueFromTag("MessageType", responseFromGate);
                Assert.IsNotNull(messageId, "messageId равен null (2.3.1.1)");
                Assert.AreEqual("REQUEST", messageType, "requestType не равен REQUEST (2.3.1.1)");                
            }
            catch (Exception)
            {
                Assert.Fail("в MTOM нет ответов");
            }           
        }

        [Test, Order(2)]
        public void SendGetRequestCheckUi()
        {
            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            Assert.IsTrue(app.Gate.CheckingResponseFromInterface(messageId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.1.3)");
        }
    }
}