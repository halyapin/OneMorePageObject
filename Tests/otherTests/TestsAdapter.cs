using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OneMorePageObject
{
    class TestsAdapter : TestBase
    {
        public void TestAdapterSendRequestJson()
        {
            RequestJson requestJson = new RequestJson()
            {
                RequestName = "VS00051v003-FNS001",
                IsTest = true,
                Form = new Form
                {
                    Num = "БН",
                    Guid = "22ED0D32-F3F5-0693-E050-A8C0D3C81091",
                    Vibor = "inn",
                    Inn = "5257045651"
                }
            };
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур для автотестов"
            };

            //POST REQUEST > GET ID FROM JSON 
            string requestId = app.Gate.SendRequestPost(requestJson);

            Assert.NotNull(requestId, "requestId (ответ на POST) равен null (1.1.1.1)");

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestId), "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.1.3)");

            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(12000);
            var responseGet = app.Gate.SendRequestGet(requestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)       
            Assert.NotNull(responseGet.RequestId, "requestId (ответ на GET) равен null (1.1.2.1)");
            Assert.NotNull(responseGet.SmevId, "smevId равен null (1.1.2.1)");
            Assert.NotNull(responseGet.ResponseXml, "responseXml равен null (1.1.2.1)");
            Assert.NotNull(responseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.1.2.1)");

            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            Assert.IsTrue(app.Gate.CheckingResponseFromInterface(responseGet.SmevId), "Не найден запрос по id в журнале ПРИНЯТЫХ (1.1.3)");

            //отправка запроса через тестирование запроса (формат JSON)
            string info = app.Gate.SendRequestFromTestRequestJson(endpoint);
            Assert.AreEqual("Идентификатор запроса", info.Split(": ")[0]);

            //"Не найден присвоенный id в тестировании запроса (формат JSON как ПОСТАВЩИК) 1.3.1.1
            string requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (JSON)) в журнале ОТПРАВЛЕННЫХ (1.1.4.4)");

            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(12000);
            var responseGetFromTestRequest = app.Gate.SendRequestGet(requestIdFromInterface);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)             
            Assert.NotNull(responseGetFromTestRequest.RequestId, "requestId (ответ на GET) равен null (1.1.4.3)");
            Assert.NotNull(responseGetFromTestRequest.SmevId, "smevId равен null (1.1.4.3)");
            Assert.NotNull(responseGetFromTestRequest.ResponseXml, "responseXml равен null (1.1.4.3)");
            Assert.NotNull(responseGetFromTestRequest.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.1.4.3)");

            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            Assert.IsTrue(app.Gate.CheckingResponseFromInterface(responseGetFromTestRequest.SmevId),
                "Не найден запрос по id в журнале ПРИНЯТЫХ (1.1.4.4)");

            Console.WriteLine("End");
            Assert.Pass();
        }

        public void TestAdapterSendRequestXml()
        {
            //TODO: тест не должен прерывается на ошибке
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур для автотестов"
            };

            //POST REQUEST > GET ID FROM JSON 
            string requestId = app.Gate.SendRequestXmlPost();

            //проверка полученного requestId в ответе на POST-запрос
            Assert.NotNull(requestId,
                "requestId (ответ на POST) равен null (1.2.1.1)");

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestId),
                "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.2.3)");

            //GET REQUEST > GET XML FROM RESPONSE 
            Thread.Sleep(12000);
            var responseGet = app.Gate.SendRequestGet(requestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)            
            Assert.NotNull(responseGet.RequestId, "requestId (ответ на GET) равен null (1.2.2.1)");
            Assert.NotNull(responseGet.SmevId, "smevId равен null (1.2.2.1)");
            Assert.NotNull(responseGet.ResponseXml, "responseXml равен null (1.2.2.1)");
            Assert.NotNull(responseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.2.2.1)");

            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            Assert.IsTrue(app.Gate.CheckingResponseFromInterface(responseGet.SmevId), "Не найден запрос по id в журнале ПРИНЯТЫХ (1.2.3)");

            //отправка ответа как ПОСТАВЩИК через тестирование запроса (формат JSON)
            bool info = app.Gate.SendRequestFromTestRequestXml(endpoint);
            Assert.IsTrue(info, "Не найден присвоенный id в тестировании запроса(формат XML как ПОСТАВЩИК) 1.4.1.1");

            string requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (XML)) в журнале ОТПРАВЛЕННЫХ (1.2.4.3)");

            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(12000);
            var responseGetFromTestRequest = app.Gate.SendRequestGet(requestIdFromInterface);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)              
            Assert.NotNull(responseGetFromTestRequest.RequestId, "requestId (ответ на GET) равен null (1.2.4.2)");
            Assert.NotNull(responseGetFromTestRequest.SmevId, "smevId равен null (1.2.4.2)");
            Assert.NotNull(responseGetFromTestRequest.ResponseXml, "responseXml равен null (1.2.4.2)");
            Assert.NotNull(responseGetFromTestRequest.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (1.2.4.2)");

            //проверка отображения ответа через интерфейс в журнале ПРИНЯТЫХ
            Assert.IsTrue(app.Gate.CheckingResponseFromInterface(responseGetFromTestRequest.SmevId),
                "Не найден запрос по id в журнале ПРИНЯТЫХ (1.2.4.3)");

            Console.WriteLine("End");
            Assert.Pass();
        }

        public void TestAdapterSendResponseJson()
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
            string requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (JSON)) в журнале ОТПРАВЛЕННЫХ (1.3.2)");
        }
        
        public void TestAdapterSendResponseXml()
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

            string requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (XML)) в журнале ОТПРАВЛЕННЫХ (1.4.2)");
        }
        
        public void TestAdapterSendRequestJsonVersionSmev1_2()
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

            //POST REQUEST > GET ID FROM JSON 
            string requestId = app.Gate.SendRequestPost(requestJson);

            Assert.NotNull(requestId, "requestId (ответ на POST) равен null (1.1.1.1)");

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestId), "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.1.3)");

            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(40000);
            var responseGet = app.Gate.SendRequestGet(requestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)  
            Assert.IsNull(responseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.1.2.1)");

            Console.WriteLine("End");
            Assert.Pass();
        }
        
        public void TestAdapterSendRequestXmlVersionSmev1_2()
        {
            //POST REQUEST > GET ID FROM JSON 
            var headers = new Dictionary<string, string>();
            headers["Request-Type"] = "VS00050v003-FNS001";
            string requestIdObj = app.Gate.SendRequestPostUnique("POST", "text/xml", headers, "/smev/sendxml", "xmlV1.2.xml");
            var response = JsonConvert.DeserializeObject<ResponseJsonGet>(requestIdObj);

            //проверка полученного requestId в ответе на POST-запрос
            Assert.NotNull(response.RequestId,
                "requestId (ответ на POST) равен null (1.2.1.1)");

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(response.RequestId),
                "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.2.3)");

            //GET REQUEST > GET XML FROM RESPONSE 
            Thread.Sleep(40000);
            var responseGet = app.Gate.SendRequestGet(response.RequestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)  
            Assert.IsNull(responseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.2.4.2)");


            Console.WriteLine("End");
            Assert.Pass();
        }
        
        public void TestAdapterSendRequestJsonVersionSmev1_3()
        {
            RequestJson requestJson = new RequestJson()
            {
                RequestName = "VS00238v001-FNS001",
                IsTest = true,
                Form = new Form
                {
                    BaseRequestId = "00000000-0000-0000-0000-000000000002",
                    SwitchDeclarant = "fl",
                    Inn = "772770224286"
                }
            };

            //POST REQUEST > GET ID FROM JSON 
            string requestId = app.Gate.SendRequestPost(requestJson);

            Assert.NotNull(requestId, "requestId (ответ на POST) равен null (1.1.1.1)");
            
            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestId), "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.1.3)");

            var Context = new RdevContext();
            var xml = Context.RsmevRequests.Where(x => x.Recid == new Guid(requestId)).Select(x => x.RequestSigned).FirstOrDefault();

            string xmlns1 = app.Gate.GetAttributeSendRequestFromSignRequest(xml);
            string xmlns2 = app.Gate.GetAttributeMessagePrimaryFromSignRequest(xml);

            Assert.AreEqual(xmlns1, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.3");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.3");

            //GET REQUEST > GET XML FROM RESPONSE
            Thread.Sleep(40000);
            var responseGet = app.Gate.SendRequestGet(requestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)  
            Assert.IsNull(responseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.1.2.1)");

            Console.WriteLine("End");
            Assert.Pass();
        }
        
        public void TestAdapterSendRequestXmlVersionSmev1_3()
        {
            //POST REQUEST > GET ID FROM JSON 
            var headers = new Dictionary<string, string>();
            headers["Request-Type"] = "VS00238v001-FNS001";
            string requestIdObj = app.Gate.SendRequestPostUnique("POST", "text/xml", headers, "/smev/sendxml", "xmlV1.3.xml");
            var response = JsonConvert.DeserializeObject<ResponseJsonGet>(requestIdObj);

            //проверка полученного requestId в ответе на POST-запрос
            Assert.NotNull(response.RequestId,
                "requestId (ответ на POST) равен null (1.2.1.1)");

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(response.RequestId),
                "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.2.3)");
           
            var Context = new RdevContext();
            var xml = Context.RsmevRequests.Where(x => x.Recid == new Guid(response.RequestId)).Select(x => x.RequestSigned).FirstOrDefault();

            string xmlns1 = app.Gate.GetAttributeSendRequestFromSignRequest(xml);
            string xmlns2 = app.Gate.GetAttributeMessagePrimaryFromSignRequest(xml);

            Assert.AreEqual(xmlns1, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.3");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.3");

            //GET REQUEST > GET XML FROM RESPONSE 
            Thread.Sleep(40000);
            var responseGet = app.Gate.SendRequestGet(response.RequestId);

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)  
            Assert.IsNull(responseGet.Error, "Ошибка при выполнении запроса на получение ответа (1.2.4.2)");

            Console.WriteLine("End");
            Assert.Pass();
        }
                
        public void TestAdapterSendResponseJsonVersionSmev1_2()
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
            string requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (JSON)) в журнале ОТПРАВЛЕННЫХ (1.3.2)");
        }
        
        public void TestAdapterSendResponseXmlVersionSmev1_2()
        {
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур 1.2"
            };
            //получение идентификатора для поставщика
            string recid = app.Gate.GetRecidForProvider();

            //отправка ответа как ПОСТАВЩИК через тестирование запроса (формат JSON)
            bool info = app.Gate.SendResponseFromInterfaceIsProviderXml(endpoint, recid);
            Assert.IsTrue(info, "Не найден присвоенный id в тестировании запроса(формат XML как ПОСТАВЩИК) 1.4.1.1");

            string requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (XML)) в журнале ОТПРАВЛЕННЫХ (1.4.2)");
        }
        
        public void TestAdapterSendResponseXmlVersionSmev1_3()
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

            string requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (XML)) в журнале ОТПРАВЛЕННЫХ (1.4.2)");

            var Context = new RdevContext();
            var xml = Context.RsmevRequests.Where(x => x.Recid == new Guid(requestIdFromInterface)).Select(x => x.RequestSigned).FirstOrDefault();

            string xmlns1 = app.Gate.GetAttributeSendRequestFromSignRequest(xml);
            string xmlns2 = app.Gate.GetAttributeSendResponseMessagePrimaryFromSignRequest(xml);

            Assert.AreEqual(xmlns1, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.3");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.3");
        }
        
        public void TestAdapterSendResponseJsonVersionSmev1_3()
        {
            EndpointData endpoint = new EndpointData()
            {
                Name = "Контур 1.3"
            };
            //получение идентификатора для поставщика
            string recid = app.Gate.GetRecidForProvider();

            //отправка ответа как ПОСТАВЩИК через тестирование запроса (формат JSON)
            string info = app.Gate.SendResponseFromInterfaceIsProviderJson(endpoint, recid);
            Assert.AreEqual("Идентификатор запроса", info.Split(": ")[0]);

            //"Не найден присвоенный id в тестировании запроса (формат JSON как ПОСТАВЩИК) 1.3.1.1
            string requestIdFromInterface = app.Gate.RequestId();

            //проверка отображения запроса через интерфейс в журнале ОТПРАВЛЕННЫХ
            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(requestIdFromInterface),
                "Не найден запрос по id (отправленный через Тест запроса (JSON)) в журнале ОТПРАВЛЕННЫХ (1.3.2)");

            var Context = new RdevContext();
            var xml = Context.RsmevRequests.Where(x => x.Recid == new Guid(requestIdFromInterface)).Select(x => x.RequestSigned).FirstOrDefault();

            string xmlns1 = app.Gate.GetAttributeSendRequestFromSignRequest(xml);
            string xmlns2 = app.Gate.GetAttributeSendResponseMessagePrimaryFromSignRequest(xml);

            Assert.AreEqual(xmlns1, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.3");
            Assert.AreEqual(xmlns2, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.3");
        }
    }
}