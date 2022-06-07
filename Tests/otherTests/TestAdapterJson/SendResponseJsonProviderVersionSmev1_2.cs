using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendResponseJsonProviderVersionSmev1_2 : TestBase
    {
        public string requestIdFromInterface { get; set; }

        [Test, Order(1)]
        public void TestRequestSendResponseSmev1_2()
        {
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур 1.2"
            };
            //получение идентификатора для поставщика
            string recid = app.Gate.GetRecidForProvider();

            //отправка ответа как ПОСТАВЩИК через тестирование запроса (формат JSON)
            string info = app.Gate.SendResponseFromInterfaceIsProviderJson(endpoint, recid);
            Assert.AreEqual("Идентификатор запроса", info.Split(": ")[0]);

            //"Не найден присвоенный id в тестировании запроса (формат JSON как ПОСТАВЩИК) 1.3.1.1
            requestIdFromInterface = app.Gate.RequestId();

        }

        [Test, Order(2)]
        public void TestRequestSendResponseSmev1_2CheckUI()
        {
            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (JSON)) в журнале ОТПРАВЛЕННЫХ (1.3.2)");
        }

        [Test, Order(3)]
        public void SingleJsonTestRequestSendResponseVersionSmev1_2CheckAttribute()
        {
            Thread.Sleep(15000);
            var Context = new RdevContext();
            var xml = Context.RsmevRequests.Where(x => x.Recid == new Guid(requestIdFromInterface)).Select(x => x.RequestSigned).FirstOrDefault();

            string xmlns1 = app.Gate.GetAttributeSendRequestFromSignRequest(xml);
            string xmlns2 = app.Gate.GetAttributeSendResponseMessagePrimaryFromSignRequest(xml);

            Assert.AreEqual(xmlns1, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.2");
        }
    }
}
