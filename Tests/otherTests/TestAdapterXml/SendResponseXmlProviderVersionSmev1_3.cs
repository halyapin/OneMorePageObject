using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    class SendResponseXmlProviderVersionSmev1_3 : TestBase
    {
        public string requestIdFromInterface { get; set; }

        [Test, Order(1)]
        public void SendResponseSmev1_3()
        {
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур 1.3"
            };
            //получение идентификатора для поставщика
            string recid = app.Gate.GetRecidForProvider();

            //отправка ответа как ПОСТАВЩИК через тестирование запроса (формат JSON)
            bool info = app.Gate.SendResponseFromInterfaceIsProviderXml(endpoint, recid);
            Assert.IsTrue(info, "Не найден присвоенный id в тестировании запроса(формат XML как ПОСТАВЩИК) 1.4.1.1");

            requestIdFromInterface = app.Gate.RequestId();
        }
        [Test, Order(2)]
        public void SendResponseSmev1_3CheckUI()
        {
            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (XML)) в журнале ОТПРАВЛЕННЫХ (1.4.2)");
        }
        [Test, Order(3)]
        public void TestAdapterSendResponseXmlVersionSmev1_3CheckAttribute()
        {
            Thread.Sleep(15000);
            var Context = new RdevContext();
            var xml = Context.RsmevRequests.Where(x => x.Recid == new Guid(requestIdFromInterface)).Select(x => x.RequestSigned).FirstOrDefault();

            string xmlns1 = app.Gate.GetAttributeSendRequestFromSignRequest(xml);
            string xmlns2 = app.Gate.GetAttributeSendResponseMessagePrimaryFromSignRequest(xml);

            Assert.AreEqual(xmlns1, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.3");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.3");
        }
    }
}
