using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendResponseProxy : TestBase
    {
        public string messageId { get; set; }
        public Guid requestId { get; set; }
        public ResponseJsonGet ResponseGet { get; set; }

        [Test, Order(1), Category("API")]
        public void SignAndSendProxyResponse(
           [Values(
            "text/xml",
            "text/xml;",
            "text/xml;charset=utf-8;",
            "text/xml;charset=utf-8")] string contentType)
        {
            //запрос для подписания с сгенерированным messageId
            var requestForSign = app.Gate.CreateRequestForSign("Files\\requestsmev-sendresponse.xml");

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
            messageId = app.Gate.GetValueFromTag("MessageId", responseFromSmev);
            var requestType = app.Gate.GetValueFromTag("MessageType", responseFromSmev);
            var status = app.Gate.GetValueFromTag("Status", responseFromSmev);

            //получаем атрибуты (неймспейсы)
            var xmlns = tagSendRequestResponse.NamespaceURI;

            Assert.IsNotNull(responseSigned, "Не удалось подписать запрос");
            Assert.IsNotNull(responseFromSmev, "Не удалось отправить прокси запрос (2.2.1.1)");

            Assert.AreEqual(xmlns, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1",
                "Namespace не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1");
            //Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1",
            //    "Атрибут xmlns:ns2 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1 (2.1.1.1)");
            //Assert.AreEqual(xmlns3, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1",
            //    "Атрибут xmlns:ns3 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1 (2.1.1.1)");

            Assert.IsNotNull(messageId, "messageId равен null (2.2.1.1)");
            Assert.AreEqual("RESPONSE", requestType, "requestType не равен RESPONSE (2.2.1.1)");
            Assert.AreEqual("requestIsQueued", status, "requestType не равен requestIsQueued (2.2.1.1)");
        }
    }
}
