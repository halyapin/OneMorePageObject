using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendRequestJsonVersionSmev1_2 : TestBase
    {
        public string requestId { get; set; }

        [Test, Order(1)]
        public void SendRequestPostSmev1_2()
        {
            RequestJson requestJson = new RequestJson()
            {
                RequestName = "VS00050v003-FNS001",
                IsTest = true,
                Form = new Form
                {
                    Num = "БН",
                    Guid = "22FE13EF-FDE8-05F2-E050-A8C0D3C820B5",
                    Vibor = "inn",
                    Inn = "772770224286"
                }
            };
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур 1.2"
            };

            //POST REQUEST > GET ID FROM JSON 
            requestId = app.Gate.SendRequestPost(requestJson);

            Assert.NotNull(requestId, "requestId (ответ на POST) равен null (1.1.1.1)");            
        }

        [Test, Order(2)]
        public void SendRequestPostSmev1_2CheckUI()
        {
            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestId), "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.1.3)");
        }
        [Test, Order(3)]
        public void SendRequestPostSmev1_2CheckAttribute()
        {
            Thread.Sleep(20000);
            var Context = new RdevContext();
            var xml = Context.RsmevRequests.Where(x => x.Recid == new Guid(requestId)).Select(x => x.RequestSigned).FirstOrDefault();

            string xmlns1 = app.Gate.GetAttributeSendRequestFromSignRequest(xml);
            string xmlns2 = app.Gate.GetAttributeMessagePrimaryFromSignRequest(xml);

            Assert.AreEqual(xmlns1, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.2");
        }

        [Test, Order(4)]
        public void SendRequestGetSmev1_2()
        {
            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(20000);
            var responseGet = app.Gate.SendRequestGet(requestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)  
            Assert.IsNull(responseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.1.2.1)");

            Console.WriteLine("End");
            Assert.Pass();
        }
    }
}
