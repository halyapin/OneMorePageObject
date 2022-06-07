using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendRequestXml : TestBase
    {
        public string RequestId { get; set; }
        public ResponseJsonGet ResponseGet { get; set; }
        public string RequestIdFromInterface { get; set; }
        public ResponseJsonGet ResponseGetFromTestRequest { get; set; }


        [Test, Order(1), Category("API")]
        public void SendRequestPost()
        {
            RequestId = app.Gate.SendRequestXmlPost();

            Assert.NotNull(RequestId, "requestId (ответ на POST) равен null (1.2.1.1)");
        }

        [Test, Order(2)]
        public void SendRequestGet()
        {
            Thread.Sleep(20000);
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
    }
}
