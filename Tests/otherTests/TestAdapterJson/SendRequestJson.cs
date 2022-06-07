using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendRequestJson : TestBase
    {
        public string RequestId { get; set; }
        public ResponseJsonGet ResponseGet { get; set; }
        public string RequestIdFromInterface { get; set; }
        public ResponseJsonGet ResponseGetFromTestRequest { get; set; }
        /// <summary>
        /// Отправка запроса формата JSON
        /// </summary>
        [Test, Order(1)]
        public void SendRequestPost()
        {
            RequestJson requestJson = new RequestJson()
            {
                RequestName = "VS00050v003-FNS001",
                IsTest = true,
                Form = new Form
                {
                    Num = "БН",
                    Guid = "22ED0D32-F3F5-0693-E050-A8C0D3C81091",
                    Vibor = "inn",
                    Inn = "5257045651"
                }
            };

            //POST REQUEST > GET ID FROM JSON 
            RequestId = app.Gate.SendRequestPost(requestJson);

            Assert.NotNull(RequestId, "requestId (ответ на POST) равен null (1.1.1.1)");
        }

        [Test, Order(2)]
        public void SendRequestPostCheckUI()
        {
            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(RequestId), "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.1.3)");
        }
        /// <summary>
        /// Проверка ответа от СМЭВ
        /// </summary>
        [Test, Order(3)]
        public void SingleJsonSendRequestGet()
        {
            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(12000);
            ResponseGet = app.Gate.SendRequestGet(RequestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)   
            Assert.IsNull(ResponseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.1.2.1)");
            Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (1.1.2.1)");
            Assert.NotNull(ResponseGet.SmevId, "smevId равен null (1.1.2.1)");
            Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (1.1.2.1)");
            Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.1.2.1)");
            
        }

        [Test, Order(4)]
        public void SendRequestGetCheckUI()
        {
            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            if (ResponseGet.SmevId != null)
            {
                Assert.IsTrue(app.Gate.CheckingResponseFromInterface(ResponseGet.SmevId), "Не найден запрос по id в журнале ПРИНЯТЫХ (1.1.3)");
            }
            else
            {
                Assert.Fail("smevId равен null");
            }
        }

        [Test, Order(5)]
        public void SendTestRequestsPost()
        {
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур для автотестов"
            };
            //отправка запроса через тестирование запроса (формат JSON)
            string info = app.Gate.SendRequestFromTestRequestJson(endpoint);
            Assert.AreEqual("Идентификатор запроса", info.Split(": ")[0]);

            //"Не найден присвоенный id в тестировании запроса (формат JSON как ПОСТАВЩИК) 1.3.1.1
            RequestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(RequestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (JSON)) в журнале ОТПРАВЛЕННЫХ (1.1.4.4)");
        }
        /// <summary>
        /// сбор ответа через теста запроса (интерфейс)
        /// </summary>
        [Test, Order(6)]
        public void SendTestRequestsGet()
        {
            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(12000);
            ResponseGetFromTestRequest = app.Gate.SendRequestGet(RequestIdFromInterface);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)              
            Assert.NotNull(ResponseGetFromTestRequest.RequestId, "requestId (ответ на GET) равен null (1.1.4.3)");
            Assert.NotNull(ResponseGetFromTestRequest.SmevId, "smevId равен null (1.1.4.3)");
            Assert.NotNull(ResponseGetFromTestRequest.ResponseXml, "responseXml равен null (1.1.4.3)");
            Assert.NotNull(ResponseGetFromTestRequest.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.1.4.3)");
        }

        [Test, Order(7)]
        public void SendTestRequestsGetCheckUI()
        {
            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            Assert.IsTrue(app.Gate.CheckingResponseFromInterface(ResponseGetFromTestRequest.SmevId),
                "Не найден запрос по id в журнале ПРИНЯТЫХ (1.2.4.3)");
        }
    }
}
