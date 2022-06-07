using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendResponse : TestBase
    {
        public string messageId { get; set; }
        public Guid requestId { get; set; }
        public ResponseJsonGet ResponseGet { get; set; }

        [Test, Order(1)]
        public void SignAndSendProxyResponse(
           [Values(
            "text/xml",
            "text/xml;",
            "text/xml;charset=utf-8;",
            "text/xml;charset=utf-8")] string contentType)
        {
            //запрос для подписания с сгенерированным messageId
            var requestForSign = app.Gate.CreateRequestForSign("requestsmev-sendresponse.xml");

            //подписываем запрос
            var responseSigned = app.Gate.SignXml(requestForSign);

            //вставляем в тег <SOAP></SOAP> подписанный запрос
            var requestProxy = app.Gate.GetValueFromSoap(responseSigned);

            string recidEndpoints = app.EndpointNameForProxyMock;
            //выбираем по какому методу будет отправляться прокси запрос
            //если метод api/smev/send/
            if (recidEndpoints is null) { }

            //если метод api/smev/send/id контура
            else
            {
                recidEndpoints = app.Gate.GetRecidEndpointsFromDB(recidEndpoints);
            }
            //отправляем прокси-запрос (сразу на СМЭВ/мок)
            var responseFromSmev = app.Gate.SendResponseProxy(requestProxy, contentType, recidEndpoints);
            Console.WriteLine(responseFromSmev);

            //получаем содержимое ответа на запрос со смэв
            var messageData = app.Gate.GetMessageDataFromResponse(responseFromSmev);
            var tagSendRequestResponse = app.Gate.GetTagSendRequestResponse(responseFromSmev);

            //получаем messageId, messageType, status из ответа на запрос
            messageId = messageData[0].InnerText;
            var requestType = messageData[1].InnerText;
            var status = messageData[7].InnerText;

            //получаем атрибуты (неймспейсы)
            var xmlns = tagSendRequestResponse.Attributes["xmlns"].Value;
            var xmlns2 = tagSendRequestResponse.Attributes["xmlns:ns2"].Value;
            var xmlns3 = tagSendRequestResponse.Attributes["xmlns:ns3"].Value;

            Assert.IsNotNull(responseSigned, "Не удалось подписать запрос");
            Assert.IsNotNull(responseFromSmev, "Не удалось отправить прокси запрос (2.2.1.1)");

            Assert.AreEqual(xmlns, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1",
                "Атрибут xmlns не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1 (2.1.1.1)");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1",
                "Атрибут xmlns:ns2 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1 (2.1.1.1)");
            Assert.AreEqual(xmlns3, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1",
                "Атрибут xmlns:ns3 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1 (2.1.1.1)");

            Assert.IsNotNull(messageId, "messageId равен null (2.2.1.1)");
            Assert.AreEqual("RESPONSE", requestType, "requestType не равен RESPONSE (2.2.1.1)");
            Assert.AreEqual("requestIsQueued", status, "requestType не равен requestIsQueued (2.2.1.1)");
        }

        [Test, Order(2)]
        public void SendRequestGet()
        {
            //получаем requestId по smevId (messageId)
            requestId = new Guid();
            using (var Context = new RdevContext())
            {
                requestId = Context.RsmevRequests.Where(
                    x => x.RequestId == new Guid(messageId)).FirstOrDefault().Recid;
            }

            //делаем get запрос
            Thread.Sleep(15000);
            ResponseGet = app.Gate.SendRequestGet(requestId.ToString());
            Console.WriteLine(ResponseGet.SmevId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)       
            Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (2.2.2.1)");
            Assert.NotNull(ResponseGet.SmevId, "smevId равен null (2.2.2.1)");
            Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (2.2.2.1)");
            Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (2.2.2.1)");
        }

        [Test, Order(3)]
        public void SendRequestGetCheckIdBySmev()
        {
            //проверка отображения ответа через интерфейс в журнале ОТПРАВЛЕННЫХ
            if (requestId != null)
                Assert.IsTrue(app.Gate.CheckingRequestFromInterfaceByMessageId(requestId.ToString()), "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (2.2.3)");
            else Assert.Fail("requestId равен null");
        }
    }
}