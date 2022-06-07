using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OneMorePageObject.API
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
        [Test, Order(1), Category("API")]
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

        /// <summary>
        /// Проверка ответа от СМЭВ
        /// </summary>
        [Test, Order(2)]
        public void SingleJsonSendRequestGet()
        {
            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(20000);
            ResponseGet = app.Gate.SendRequestGet(RequestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)   
            Assert.IsNull(ResponseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.1.2.1)");
            Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (1.1.2.1)");
            Assert.NotNull(ResponseGet.SmevId, "smevId равен null (1.1.2.1)");
            Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (1.1.2.1)");
            Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.1.2.1)");

        }
    }
}