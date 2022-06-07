using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OneMorePageObject.API
{
    class SendRequestXml : TestBase
    {
        public string RequestId { get; set; }
        public ResponseJsonGet ResponseGet { get; set; }
        public string RequestIdFromInterface { get; set; }
        public ResponseJsonGet ResponseGetFromTestRequest { get; set; }


        [Test, Order(1)]
        public void SendRequestPost()
        {
            RequestId = app.Gate.SendRequestXmlPost();

            Assert.NotNull(RequestId, "requestId (ответ на POST) равен null (1.2.1.1)");
        }

        [Test, Order(2)]
        public void SigleXmlSendRequestPostCheckUI()
        {
            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(RequestId), "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.2.3)");
        }

        [Test, Order(3)]
        public void SendRequestGet()
        {
            Thread.Sleep(12000);
            try
            {
                ResponseGet = app.Gate.SendRequestGet(RequestId);
            }
            catch (Exception)
            {
                Assert.Fail("RequestId отсутствует или произошла другая ошибка с отправкой запроса. Для получения RequestId необходимо отправить POST SendRequest");
            }

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)  
            Assert.IsNull(ResponseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.2.2.1)");
            Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (1.2.2.1)");
            Assert.NotNull(ResponseGet.SmevId, "smevId равен null (1.2.2.1)");
            Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (1.2.2.1)");
            Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.2.2.1)");
        }

        [Test, Order(4)]
        public void SendRequestGetCheckUI()
        {
            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            if (ResponseGet.SmevId != null)
                Assert.IsTrue(app.Gate.CheckingResponseFromInterface(ResponseGet.SmevId), "Не найден запрос по id в журнале ПРИНЯТЫХ (1.2.3)");
            else Assert.Fail("smevId равен null");
        }

        [Test, Order(5)]
        public void SendTestRequestsPost()
        {
            EndpointData endpoint = new EndpointData()
            {
                Name = "Мок"
            };                    
            //отправка запроса через тестирование запроса 
            bool info = app.Gate.SendRequestFromTestRequestXml(endpoint);
            Assert.IsTrue(info, "Не найден присвоенный id в тестировании запроса(формат XML как ПОСТАВЩИК) 1.4.1.1");

            RequestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(RequestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (XML)) в журнале ОТПРАВЛЕННЫХ (1.2.4.3)");
        }

        [Test, Order(6)]
        public void SendTestRequestsGet()
        {
            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(12000);
            ResponseGetFromTestRequest = app.Gate.SendRequestGet(RequestIdFromInterface);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)             
            Assert.NotNull(ResponseGetFromTestRequest.RequestId, "requestId (ответ на GET) равен null (1.2.4.2)");
            Assert.NotNull(ResponseGetFromTestRequest.SmevId, "smevId равен null (1.2.4.2)");
            Assert.NotNull(ResponseGetFromTestRequest.ResponseXml, "responseXml равен null (1.2.4.2)");
            Assert.NotNull(ResponseGetFromTestRequest.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.2.4.2)");
        }

        [Test, Order(7)]
        public void SendTestRequestsGetCheckUI()
        {
            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            Assert.IsTrue(app.Gate.CheckingResponseFromInterface(ResponseGetFromTestRequest.SmevId),
                "Не найден запрос по id в журнале ПРИНЯТЫХ (1.2.4.3)");

            Assert.Pass();
        }
    }
}
