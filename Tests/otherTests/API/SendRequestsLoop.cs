using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendRequestsLoop : TestBase
    {
        public string messageId { get; set; }
        public Guid requestId { get; set; }
        public ResponseJsonGet ResponseGet { get; set; }

        /// <summary>
        /// Подписание и отправка прокси запросав СМЭВ
        /// </summary>
        /// <param name="contentType"></param>
        [Test, Order(1), Category("Proxy")]
        public void SignAndSendProxyRequest(
            [Values(
            "text/xml",
            "text/xml;",
            "text/xml;charset=utf-8;",
            "text/xml;charset=utf-8")] string contentType)
        {
            for(int i=0; i<1000; i++)
            {
                //запрос для подписания с сгенерированным messageId
                var requestForSign = app.Gate.CreateRequestForSign("Files\\requestsmev.xml");

                //подписываем запрос
                var requestSigned = app.Gate.SignXml(requestForSign);

                //вставляем в тег <SOAP></SOAP> подписанный запрос
                var requestProxy = app.Gate.GetValueFromSoap(requestSigned);

                string recidEndpoints = app.EndpointNameForProxySmev;
                //выбираем по какому методу будет отправляться прокси запрос
                //если метод api/smev/send/
                if (recidEndpoints is null) { }

                //если метод api/smev/send/id контура
                else
                {
                    recidEndpoints = app.Gate.GetRecidEndpointsFromDB(recidEndpoints);
                }
                //отправляем прокси-запрос (сразу на СМЭВ/мок)
                var responseFromSmev = app.Gate.SendRequestProxy(requestProxy, contentType, recidEndpoints);
                Console.WriteLine(responseFromSmev);
            }
            

            ////получаем содержимое ответа на запрос со смэв            
            //var tagSendRequestResponse = app.Gate.GetTagSendRequestResponse(responseFromSmev);

            ////получаем messageId, messageType, status из ответа на запрос
            //messageId = app.Gate.GetValueFromTag("MessageId", responseFromSmev);
            //var messageType = app.Gate.GetValueFromTag("MessageType", responseFromSmev);
            //var status = app.Gate.GetValueFromTag("Status", responseFromSmev);

            ////получаем атрибуты (неймспейсы)
            //var xmlns = tagSendRequestResponse.NamespaceURI;

            //Assert.IsNotNull(requestSigned, "Не удалось подписать запрос");
            //Assert.IsNotNull(responseFromSmev, "Не удалось отправить прокси запрос (2.1.1.1)");

            //Assert.AreEqual("urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1", xmlns,
            //    "Атрибут xmlns не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1 (2.1.1.1)");

            //Assert.IsNotNull(messageId, "messageId равен null (2.1.1.1)");
            //Assert.AreEqual("REQUEST", messageType, "requestType не равен REQUEST (2.1.1.1)");
            //Assert.AreEqual("requestIsQueued", status, "requestType не равен requestIsQueued (2.1.1.1)");
        }        
    }
}
