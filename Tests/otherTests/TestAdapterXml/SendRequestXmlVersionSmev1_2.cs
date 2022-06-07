using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendRequestXmlVersionSmev1_2 : TestBase
    {
        public ResponseJsonGet response;

        [Test, Order (1)]
        public void SendRequestSmev1_2()
        {
            //TODO: тест не должен прерывается на ошибке
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур 1.2"
            };

            //POST REQUEST > GET ID FROM JSON 
            //string requestId = app.Gate.SendRequestXmlPost();
            var headers = new Dictionary<string, string>();
            headers["Request-Type"] = "VS00050v003-FNS001";
            string requestIdObj = app.Gate.SendRequestPostUnique("POST", "text/xml", headers, "/smev/sendxml", "xmlV1.2.xml");
            response = JsonConvert.DeserializeObject<ResponseJsonGet>(requestIdObj);

            //проверка полученного requestId в ответе на POST-запрос
            Assert.NotNull(response.RequestId,
                "requestId (ответ на POST) равен null (1.2.1.1)");   
        }

        [Test, Order(2)]
        public void SendRequestSmev1_2CheckUI()
        {
            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(response.RequestId),
                "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.2.3)");
        }

        [Test, Order(3)] /// ?????? SendRequestJsonVersionSmev1_2CheckAttribute, json???
        public void SendRequestJsonVersionSmev1_2CheckAttribute()
        {
            Thread.Sleep(15000);
            var Context = new RdevContext();
            var xml = Context.RsmevRequests.Where(x => x.Recid == new Guid(response.RequestId)).Select(x => x.RequestSigned).FirstOrDefault();

            string xmlns1 = app.Gate.GetAttributeSendRequestFromSignRequest(xml);
            string xmlns2 = app.Gate.GetAttributeMessagePrimaryFromSignRequest(xml);

            Assert.AreEqual(xmlns1, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.2");
        }
        [Test, Order(4)]
        public void SendRequestGetSmev1_2()
        {
            //GET REQUEST > GET XML FROM RESPONSE 
            Thread.Sleep(25000);
            var responseGet = app.Gate.SendRequestGet(response.RequestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)  
            Assert.IsNull(responseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.2.4.2)");


            Console.WriteLine("End");
            Assert.Pass();
        }
    }
}
