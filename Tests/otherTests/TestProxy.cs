using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using NUnit.Framework;


namespace OneMorePageObject
{
    class TestProxyRequests : TestBase
    {
        //[Test]
        //public void TestProxySendRequest(
        //    [Values(
        //    "text/xml", 
        //    "text/xml;", 
        //    "text/xml;charset=utf-8;", 
        //    "text/xml;charset=utf-8")] string contentType)
        //{
        //    //запрос для подписания с сгенерированным messageId
        //    var requestForSign = app.Gate.CreateRequestForSign("requestsmev.xml");

        //    //подписываем запрос
        //    var responseSigned = app.Gate.SignXml(requestForSign);

        //    //вставляем в тег <SOAP></SOAP> подписанный запрос
        //    var requestProxy = app.Gate.GetValueFromSoap(responseSigned);

        //    //отправляем прокси-запрос (сразу на СМЭВ/мок)
        //    var responseFromSmev = app.Gate.SendRequestProxy(requestProxy, contentType);
        //    Console.WriteLine(responseFromSmev);
        //    //получаем содержимое ответа на запрос со смэв
        //    var messageData = app.Gate.GetMessageDataFromResponse(responseFromSmev);
        //    var tagSendRequestResponse = app.Gate.GetTagSendRequestResponse(responseFromSmev);

        //    var messageId = messageData[0].InnerText;
        //    var requestType = messageData[1].InnerText;
        //    var status = messageData[7].InnerText;

        //    var xmlns = tagSendRequestResponse.Attributes["xmlns"].Value;
        //    var xmlns2 = tagSendRequestResponse.Attributes["xmlns:ns2"].Value;
        //    var xmlns3 = tagSendRequestResponse.Attributes["xmlns:ns3"].Value;

        //    Assert.IsNotNull(responseSigned, "Не удалось подписать запрос");
        //    Assert.IsNotNull(responseFromSmev, "Не удалось отправить прокси запрос (2.1.1.1)");

        //    Assert.AreEqual(xmlns, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1",
        //        "Атрибут xmlns не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1 (2.1.1.1)");
        //    Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1",
        //        "Атрибут xmlns:ns2 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1 (2.1.1.1)");
        //    Assert.AreEqual(xmlns3, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1",
        //        "Атрибут xmlns:ns3 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1 (2.1.1.1)");

        //    Assert.IsNotNull(messageId, "messageId равен null (2.1.1.1)");
        //    Assert.AreEqual(requestType, "REQUEST", "requestType не равен REQUEST (2.1.1.1)");
        //    Assert.AreEqual(status, "requestIsQueued", "requestType не равен requestIsQueued (2.1.1.1)");

        //    var requestId = new Guid();
        //    using (var Context = new RdevContext())
        //    {
        //        requestId = Context.RsmevRequests.Where(
        //            x => x.RequestId == new Guid(messageId)).FirstOrDefault().Recid;
        //    }

        //    Thread.Sleep(12000);
        //    var ResponseGet = app.Gate.SendRequestGet(requestId.ToString());

        //    //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)       
        //    Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (2.1.2.1)");
        //    Assert.NotNull(ResponseGet.SmevId, "smevId равен null (2.1.2.1)");
        //    Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (2.1.2.1)");
        //    Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (2.1.2.1)");

        //    //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
        //    if (ResponseGet.SmevId != null)
        //        Assert.IsTrue(app.Gate.CheckingResponseFromInterface(ResponseGet.SmevId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.1.3)");
        //    else Assert.Fail("smevId равен null");
        //}

        //[Test]
        //public void TestProxySendResponse(
        //    [Values(
        //    "text/xml",
        //    "text/xml;",
        //    "text/xml;charset=utf-8;",
        //    "text/xml;charset=utf-8")] string contentType)
        //{
        //    //запрос для подписания с сгенерированным messageId
        //    var requestForSign = app.Gate.CreateRequestForSign("requestsmev-sendresponse.xml");

        //    //подписываем запрос
        //    var responseSigned = app.Gate.SignXml(requestForSign);

        //    //вставляем в тег <SOAP></SOAP> подписанный запрос
        //    var requestProxy = app.Gate.GetValueFromSoap(responseSigned);

        //    //отправляем прокси-запрос (сразу на СМЭВ/мок)
        //    var responseFromSmev = app.Gate.SendResponseProxy(requestProxy, contentType);

        //    //получаем содержимое ответа на запрос со смэв
        //    var messageData = app.Gate.GetMessageDataFromResponse(responseFromSmev);
        //    var tagSendRequestResponse = app.Gate.GetTagSendRequestResponse(responseFromSmev);

        //    var messageId = messageData[0].InnerText;
        //    var requestType = messageData[1].InnerText;
        //    var status = messageData[7].InnerText;

        //    var xmlns = tagSendRequestResponse.Attributes["xmlns"].Value;
        //    var xmlns2 = tagSendRequestResponse.Attributes["xmlns:ns2"].Value;
        //    var xmlns3 = tagSendRequestResponse.Attributes["xmlns:ns3"].Value;

        //    Assert.IsNotNull(responseSigned, "Не удалось подписать запрос");
        //    Assert.IsNotNull(responseFromSmev, "Не удалось отправить прокси запрос (2.2.1.1)");

        //    Assert.AreEqual(xmlns, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1",
        //        "Атрибут xmlns не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1 (2.1.1.1)");
        //    Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1",
        //        "Атрибут xmlns:ns2 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1 (2.1.1.1)");
        //    Assert.AreEqual(xmlns3, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1",
        //        "Атрибут xmlns:ns3 не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1 (2.1.1.1)");

        //    Assert.IsNotNull(messageId, "messageId равен null (2.2.1.1)");
        //    Assert.AreEqual(requestType, "RESPONSE", "requestType не равен RESPONSE (2.2.1.1)");
        //    Assert.AreEqual(status, "requestIsQueued", "requestType не равен requestIsQueued (2.2.1.1)");

        //    var requestId = new Guid();
        //    using (var Context = new RdevContext())
        //    {
        //        requestId = Context.RsmevRequests.Where(
        //            x => x.RequestId == new Guid(messageId)).FirstOrDefault().Recid;
        //    }

        //    Thread.Sleep(15000);
        //    var ResponseGet = app.Gate.SendRequestGet(requestId.ToString());

        //    //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)       
        //    Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (2.2.2.1)");
        //    Assert.NotNull(ResponseGet.SmevId, "smevId равен null (2.2.2.1)");
        //    Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (2.2.2.1)");
        //    Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (2.2.2.1)");

        //    //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
        //    if (ResponseGet.SmevId != null)
        //        Assert.IsTrue(app.Gate.CheckingResponseFromInterface(ResponseGet.SmevId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.2.3)");
        //    else Assert.Fail("smevId равен null");
        //}

        public void TestProxyGetRequest()
        {
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:GetRequest";
            var responseFromGate = app.Gate.SendRequestPostUnique("POST", "text/xml", headers, "/smev/send", "request-getrequest.xml");

            var lines = app.Gate.GetLinesFile("response/getresponse.xml", responseFromGate);

            foreach (var def in app.Gate.GetDefsFromGetRequest(lines))
                Assert.AreEqual("--", def, "Отсуствуют дефисы uuid (2.3.1.1)");

            foreach (var uuid in app.Gate.GetUuidsFromGetRequest(lines))
                Assert.NotNull(uuid, "Отсуствуют uuid (2.3.1.1)");

            foreach (var item in app.Gate.GetHeadersForGetRequest(lines))
                Assert.NotNull(item, "Отсуствуют заголовки запроса (2.3.1.1)");

            var getResponseNode = app.Gate.GetMessageData(lines);

            if (getResponseNode.HasChildNodes)
            {
                var metadata = getResponseNode.FirstChild.FirstChild.ChildNodes[2].ChildNodes;
                var messageId = metadata[0].InnerText;
                var requestType = metadata[1].InnerText;
                Assert.IsNotNull(messageId, "messageId равен null (2.3.1.1)");
                Assert.AreEqual(requestType, "RESPONSE", "requestType не равен RESPONSE (2.3.1.1)");
                Assert.IsTrue(app.Gate.CheckingResponseFromInterface(messageId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.1.3)");
            }
            else Assert.IsFalse(getResponseNode.HasChildNodes, "В МТОМ нет ответов");
        }

        public void TestProxyGetResponse()
        {
            var headers = new Dictionary<string, string>();
            headers["SOAPAction"] = "urn:GetResponse";
            var responseFromGate = app.Gate.SendRequestPostUnique("POST", "text/xml", headers, "/smev/send", "request-getrequest.xml");

            var lines = app.Gate.GetLinesFile("response/getresponse.xml", responseFromGate);

            foreach (var def in app.Gate.GetDefsFromGetRequest(lines))
                Assert.AreEqual("--", def, "Отсуствуют дефисы uuid (2.4.1.1)");

            foreach (var uuid in app.Gate.GetUuidsFromGetRequest(lines))
                Assert.NotNull(uuid, "Отсуствуют uuid (2.4.1.1)");

            foreach (var item in app.Gate.GetHeadersForGetRequest(lines))
                Assert.NotNull(item, "Отсуствуют заголовки запроса (2.4.1.1)");

            var getResponseNode = app.Gate.GetMessageData(lines);

            if (getResponseNode.HasChildNodes)
            {
                var metadata = getResponseNode.FirstChild.FirstChild.ChildNodes[2].ChildNodes;
                var messageId = metadata[0].InnerText;
                var requestType = metadata[1].InnerText;
                Assert.IsNotNull(messageId, "messageId равен null (2.4.1.1)");
                Assert.AreEqual(requestType, "RESPONSE", "requestType не равен RESPONSE (2.4.1.1)");
                Assert.IsTrue(app.Gate.CheckingResponseFromInterfaceByMessageId(messageId), "Не найден запрос по id в журнале ПРИНЯТЫХ (2.1.3)");
            }
            else Assert.IsFalse(getResponseNode.HasChildNodes, "В МТОМ нет ответов");
        }
    }
}
