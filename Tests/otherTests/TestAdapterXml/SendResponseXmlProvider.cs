using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    class SendResponseXmlProvider : TestBase
    {
        public string requestIdFromInterface { get; set; }

        [Test, Order(1)]
        public void TestRequestSendResponse()
        {
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур для автотестов"
            };
            //получение идентификатора для поставщика
            string recid = app.Gate.GetRecidForProvider();

            //отправка ответа как ПОСТАВЩИК через тестирование запроса (формат JSON)
            bool info = app.Gate.SendResponseFromInterfaceIsProviderXml(endpoint, recid);
            Assert.IsTrue(info, "Не найден присвоенный id в тестировании запроса(формат XML как ПОСТАВЩИК) 1.4.1.1");
        }
        [Test, Order(2)]
        public void TestRequestCheckUI()
        {
            requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (XML)) в журнале ОТПРАВЛЕННЫХ (1.4.2)");
        }
    }
}
