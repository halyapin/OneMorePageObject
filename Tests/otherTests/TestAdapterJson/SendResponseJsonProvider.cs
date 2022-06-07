using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
    class SendResponseJsonProvider : TestBase
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
            string info = app.Gate.SendResponseFromInterfaceIsProviderJson(endpoint, recid);
            Assert.AreEqual("Идентификатор запроса", info.Split(": ")[0]);

            //"Не найден присвоенный id в тестировании запроса (формат JSON как ПОСТАВЩИК) 1.3.1.1
            requestIdFromInterface = app.Gate.RequestId();

        }
        [Test, Order(2)]
        public void TestRequestCheckUI()
        {
            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (JSON)) в журнале ОТПРАВЛЕННЫХ (1.3.2)");
        }
    }
}
