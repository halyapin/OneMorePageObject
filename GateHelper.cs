using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace OneMorePageObject
{
    public class GateHelper : HelperBase
    {
        protected string BaseUrl { get; set; }

        public GateHelper(ApplicationManager manager, string baseURL)
            : base(manager)
        {
            BaseUrl = baseURL;
        }

        public string SendRequestPostUnique(string method, string contentType, Dictionary<string, string> headers, string endPoint, string fileName)
        {
            var requestBody = "";

            using (var streamReader = new StreamReader(fileName))
            {
                requestBody = streamReader.ReadToEnd();
            }

            var body = Encoding.UTF8.GetBytes(requestBody);

            var request = WebRequest.Create
(BaseUrl.Replace("/smev", $"/api{endPoint}")) as HttpWebRequest;

            request.Method = method;
            request.ContentType = contentType;
            request.ContentLength = body.Length;
            foreach (var header in headers)
                request.Headers[header.Key] = header.Value;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
                stream.Close();
            }

            var responseString = "";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }
            }
            return responseString;
        }

        public void CheckingSuccessGetResponse(string responseFromGate, string messageId)
        {


            var requestId = new Guid();
            using (var Context = new RdevContext())
            {
                requestId = Context.RsmevRequests.Where(
                    x => x.RequestId == new Guid(messageId)).FirstOrDefault().Recid;
            }

            Thread.Sleep(20000);
            var ResponseGet = SendRequestGet(requestId.ToString());

            //Проверка ответа от СМЭВ (mock-сервис) || рассмотреть вариант вывода ошибки из ответа на запрос (свойство error)       
            Assert.NotNull(ResponseGet.RequestId, "requestId (ответ на GET) равен null (2.5.2.1)");
            Assert.NotNull(ResponseGet.SmevId, "smevId равен null (2.5.2.1)");
            Assert.NotNull(ResponseGet.ResponseXml, "responseXml равен null (2.5.2.1)");
            Assert.NotNull(ResponseGet.Data.FnsvipResponse, "ns1:FNSVipIPResponse равен null (2.5.2.1)");
        }

        /// <summary>
        /// Медот проверки успешной отправки запроса
        /// </summary>
        public void CheckingSuccessSending(string responseFromGate, string messageId)
        {
            var messageData = GetMessageDataFromResponse(responseFromGate);
            var tagSendRequestResponse = GetTagSendRequestResponse(responseFromGate);


            var requestType = GetValueFromTag("MessageType", responseFromGate);
            var status = GetValueFromTag("Status", responseFromGate);

            var xmlns = tagSendRequestResponse.NamespaceURI;

            Assert.IsNotNull(responseFromGate, "Не удалось отправить прокси запрос (2.5.1.1)");

            Assert.AreEqual(xmlns, "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1",
                "Namespace не равен urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1 (2.5.1.1)");

            Assert.IsNotNull(messageId, "messageId равен null (2.5.1.1)");
            Assert.AreEqual(status, "requestIsQueued", "requestType не равен requestIsQueued (2.5.1.1)");
            if (requestType == "REQUEST" || requestType == "RESPONSE")
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail("requestType не равен REQUEST или RESPONSE");
            }
        }

        //получаем идентификатор контура из базы по наименованию
        public string GetRecidEndpointsFromDB(string choiceEndpoints)
        {
            var recidEndpoint = "68f6f420 - 2731 - 4a07 - a809 - 94605855fd53";
            /*using (var Context = new RdevContext())
            {
                recidEndpoint = Context.RsmevEndpoints.Where(
                    x => x.Recname == choiceEndpoints).FirstOrDefault().Recid;
            }*/
            return recidEndpoint.ToString();
        }

        public string SendRequestPostUnique<T>(string method, string contentType, T requestBody, Dictionary<string, string> headers, string endPoint)
        {
            return "";
        }

        //вынести в отдельный класс RequestHelper
        public ResponseJsonGet SendRequestGet(string id)
        {
            Thread.Sleep(3000);
            var request = (HttpWebRequest)WebRequest.Create(BaseUrl.Replace("/smev", "/api/smev/send/" + id));

            request.Method = "GET";
            request.ContentType = "application/json";

            var responseJson = new ResponseJsonGet();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    var responseString = reader.ReadToEnd();
                    responseJson = JsonConvert.DeserializeObject<ResponseJsonGet>(responseString);
                }
                response.Close();
            }

            return responseJson;
        }

        //вынести в отдельный класс RequestHelper
        public string SendRequestPost(RequestJson requestJson)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var json = JsonConvert.SerializeObject(requestJson, settings);

            var body = Encoding.UTF8.GetBytes(json);
            var request = (HttpWebRequest)WebRequest.Create(BaseUrl.Replace("/smev", "/api/smev/sendrequest"));

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = body.Length;

            return GetRequestJson(request, body);
        }

        //вынести в отдельный класс RequestHelper
        public string SendRequestXmlPost()
        {
            //В данный момент читается из файла
            //TODO: исправить
            var xml = "";
            using (var streamReader = new StreamReader("C:\\Users\\v.halyapin\\source\\repos\\OneMorepageObject\\OneMorePageObject\\Files\\xml.xml"))
            {
                xml = streamReader.ReadToEnd();
            }

            var body = Encoding.UTF8.GetBytes(xml);

            var request = (HttpWebRequest)WebRequest.Create(BaseUrl.Replace("/smev", $"/api/smev/send/8a79ab4e-5f84-426d-a855-19d76bd6811c"));

            request.Method = "POST";
            request.ContentType = "text / xml; charset = utf - 8;";
            request.Headers["SOAPAction"] = "urn: SendRequest";
            request.ContentLength = body.Length;

            return GetRequestJson(request, body);
        }

        public string GetRequestJson(HttpWebRequest request, byte[] body)
        {
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
                stream.Close();
            }

            var responseJson = new ResponseJsonPost();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    var responseString = reader.ReadToEnd();
                    responseJson = JsonConvert.DeserializeObject<ResponseJsonPost>(responseString);
                }
                response.Close();
            }
            return responseJson.RequestId;
        }
        //подписание запроса через встроенный СП Рдева
        public string SignXml(string requestForSign)
        {
            var body = Encoding.UTF8.GetBytes(requestForSign);

            var request = (HttpWebRequest)WebRequest.Create
                (BaseUrl.Replace("/smev", "/api/sign/xml"));

            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = body.Length;

            return SendApi(request, body);
        }

        //TODO: рефакторить этот ужас
        public string CreateRequestForSign(string fileName)
        {
            var requestSmev = "";
            var requestForSign = "";
            using (var streamReader = new StreamReader(fileName))
            {
                requestSmev = streamReader.ReadToEnd().Replace("<MessageID></MessageID>",
                    $"<MessageID>{GuidGenerator.GenerateTimeBasedGuid().ToString()}</MessageID>");
            }

            string myEncodedString = HttpUtility.HtmlEncode(requestSmev);

            var soapValue = @"<a:SOAP>&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Envelope xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot; xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns=&quot;http://schemas.xmlsoap.org/soap/envelope/&quot;&gt;&lt;Body&gt;&lt;SendRequestRequest xmlns=&quot;urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1&quot;&gt;&lt;SenderProvidedRequestData Id=&quot;SIGNED_BY_CALLER&quot;&gt;&lt;MessageID&gt;45861b51-7b0a-11ea-b2f5-a805a62acbed&lt;/MessageID&gt;&lt;MessagePrimaryContent xmlns=&quot;urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1&quot;&gt;&lt;ns1:FNSVipIPRequest НомерДела=&quot;БН&quot; ИдДок=&quot;22FE13EF-FDE8-05F2-E050-A8C0D3C820B5&quot; xmlns:ns1=&quot;urn://x-artefacts-fns-vipip-tosmv-ru/311-15/4.0.5&quot;&gt;&lt;ns1:ЗапросИП&gt;&lt;ns1:ИНН&gt;772770224286&lt;/ns1:ИНН&gt;&lt;/ns1:ЗапросИП&gt;&lt;/ns1:FNSVipIPRequest&gt;&lt;/MessagePrimaryContent&gt;&lt;/SenderProvidedRequestData&gt;&lt;CallerInformationSystemSignature&gt;&lt;ds:Signature xmlns:ds=&quot;http://www.w3.org/2000/09/xmldsig#&quot;&gt;&lt;ds:SignedInfo&gt;&lt;ds:CanonicalizationMethod Algorithm=&quot;http://www.w3.org/2001/10/xml-exc-c14n#&quot; /&gt;&lt;ds:SignatureMethod Algorithm=&quot;urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256&quot; /&gt;&lt;ds:Reference URI=&quot;#SIGNED_BY_CALLER&quot;&gt;&lt;ds:Transforms&gt;&lt;ds:Transform Algorithm=&quot;http://www.w3.org/2001/10/xml-exc-c14n#&quot; /&gt;&lt;ds:Transform Algorithm=&quot;urn://smev-gov-ru/xmldsig/transform&quot; /&gt;&lt;/ds:Transforms&gt;&lt;ds:DigestMethod Algorithm=&quot;urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256&quot; /&gt;&lt;ds:DigestValue&gt;9mCmpT2UvDbqIVKNg5MrJ2lVNKqKbzDfJWwWJhIZv8Y=&lt;/ds:DigestValue&gt;&lt;/ds:Reference&gt;&lt;/ds:SignedInfo&gt;&lt;ds:SignatureValue&gt;igUl4NI4KZAVrVx8BL/qyJRHE74HQsbBL8lRwfHuEezbR0gwl7ICCjktCLBZrGkfe0WbH6EnloEztrhbUv83xQ==&lt;/ds:SignatureValue&gt;&lt;ds:KeyInfo&gt;&lt;ds:X509Data&gt;&lt;ds:X509Certificate&gt;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&lt;/ds:X509Certificate&gt;&lt;/ds:X509Data&gt;&lt;/ds:KeyInfo&gt;&lt;/ds:Signature&gt;&lt;/CallerInformationSystemSignature&gt;&lt;/SendRequestRequest&gt;&lt;/Body&gt;&lt;/Envelope&gt;</a:SOAP>";
            using (var streamReader = new StreamReader("Files\\requestsign.xml"))
            {
                requestForSign = streamReader.ReadToEnd().Replace(soapValue, $"<a:SOAP>{myEncodedString}</a:SOAP>");
            }
            return requestForSign;
        }

        //получение идентификатора для поставщика
        public string GetRecidForProvider()
        {
            var Context = new RdevContext();
            string recid = Context.RsmevResponses.Where(x => x.Recstate == 1 && x.ReplyTo != "" && x.IsProvider)
                .OrderByDescending(x => x.Reccreated).Select(x => x.Recid).FirstOrDefault().ToString();
            return recid;
        }

        public string GetValueFromSoap(string responseSigned)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responseSigned);
            return doc.LastChild.LastChild.LastChild.InnerText;
        }

        public XmlNodeList GetMessageDataFromResponse(string response)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(response);
            return document.LastChild.LastChild.FirstChild.FirstChild.ChildNodes;
        }

        public XmlNode GetMessageData(string[] lines)
        {
            var xml = "";
            XmlDocument xmlDocument = new XmlDocument();
            for (int i = 5; i < lines.Length - 1; i++)
                xml = lines[i];
            xmlDocument.LoadXml(xml);
            return xmlDocument.FirstChild.FirstChild.FirstChild;
        }

        public XmlNode GetTagSendRequestResponse(string response)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(response);
            return document.LastChild.LastChild.FirstChild;

        }
        /// <summary>
        /// Метод получения значения заданного элемента
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetValueFromTag(string tagName, string text)
        {
            XDocument xdoc = XDocument.Parse(text);
            //считываем содержимое элемента
            var element = xdoc.Descendants()
                .Where(x => x.Name.LocalName == tagName)
                .Select(x => x.Value).FirstOrDefault().ToString();
            return element;
        }

        public string[] GetLinesFile(string path, string content)
        {
            File.WriteAllText(path, content);
            return File.ReadAllLines(path);
        }

        public List<string> GetHeadersForGetRequest(string[] linesFile)
        {
            var listHeaders = new List<string>();
            for (int i = 1; i < linesFile.Length; i++)
                if (i < 4)
                {
                    var header = linesFile[i].Split(": ");
                    listHeaders.Add(header[1]);
                }
            return listHeaders;
        }

        public List<string> GetUuidsFromGetRequest(string[] linesFile)
        {
            var listUuids = new List<string>();
            listUuids.Add(linesFile[0].Split(":")[0]);
            listUuids.Add(linesFile[1].Split(":")[0]);
            return listUuids;
        }

        public List<string> GetDefsFromGetRequest(string[] lines)
        {
            var list = new List<string>();
            list.Add(lines[0].Substring(0, 2));
            list.Add(lines.Last().Substring(0, 2));
            list.Add(lines.Last().Substring(lines.Last().Length - 2, 2));
            return list;
        }

        public string SendRequestProxy(string requestWithSign, string contentType, string recidEndpoints)
        {
            var body = Encoding.UTF8.GetBytes(requestWithSign);

            var request = (HttpWebRequest)WebRequest.Create
                (BaseUrl.Replace("/smev", $"/api/smev/send/{recidEndpoints}"));

            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = body.Length;
            request.Headers["SOAPAction"] = "urn:SendRequest";
            request.Headers["node_id"] = "Autotest";

            return SendApi(request, body);
        }

        public string SendResponseProxy(string requestWithSign, string contentType, string recidEndpoints)
        {
            var body = Encoding.UTF8.GetBytes(requestWithSign);

            var request = (HttpWebRequest)WebRequest.Create
                (BaseUrl.Replace("/smev", "/api/smev/send/" + recidEndpoints + ""));

            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = body.Length;
            request.Headers["SOAPAction"] = "urn:SendResponse";
            request.Headers["node_id"] = "Autotest";

            return SendApi(request, body);
        }

        //отправка api
        public string SendApi(HttpWebRequest request, byte[] body)
        {
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
                stream.Close();
            }

            var responseString = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }
            }
            return responseString;
        }

        public string RequestId()
        {
            return GetRecidText();
        }

        public string GetAttributeSendRequestFromSignRequest(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return xmlDocument.LastChild.LastChild.FirstChild.Attributes["xmlns"].Value;
        }

        public string GetAttributeMessagePrimaryFromSignRequest(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return xmlDocument.LastChild.LastChild.FirstChild.FirstChild.ChildNodes[1].Attributes["xmlns"].Value;
        }
        public string GetAttributeSendResponseMessagePrimaryFromSignRequest(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return xmlDocument.LastChild.LastChild.LastChild.FirstChild.LastChild.Attributes["xmlns"].Value;
        }

        //проверка отображения запроса в журнале отправленных
        public bool CheckingRequestFromInterface(string requestId)
        {
            //открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            //открыть журнал отправленных запросов
            OpenRequestsPage();

            //поиск по идентификатору
            SearchigId(requestId);

            //проверка наличия отправленного запроса в списке
            return IsRequestExist(requestId);
        }

        //проверка отображения ответа в журнале принятых
        public bool CheckingResponseFromInterface(string smevId)
        {
            //открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            //открыть журнал принятых запросов
            OpenResponsesPage();

            //выбор радиокнопки по поиску связанного идентификатора в СМЭВ
            RadioButtonSmevId();

            //поиск по идентификатору
            SearchigId(smevId);

            //проверка наличия полученного запроса в списке
            return IsResponseExist(smevId);
        }
        public bool CheckingResponseFromInterfaceByMessageId(string messageId)
        {
            //открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            //открыть журнал принятых запросов
            OpenResponsesPage();

            //выбор радиокнопки по поиску идентификатора в СМЭВ
            RadioButtonSmevId();

            //поиск по идентификатору
            SearchigId(messageId);

            //проверка наличия полученного запроса в списке
            return IsResponseExist(messageId);
        }
        public bool CheckingRequestFromInterfaceByMessageId(string messageId)
        {
            //открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            //открыть журнал принятых запросов
            OpenRequestsPage();

            //выбор радиокнопки по поиску идентификатора в СМЭВ
            RadioButtonSmevId();

            //поиск по идентификатору
            SearchigId(messageId);

            //проверка наличия отправленного запроса в списке
            return IsRequestExist(messageId);
        }


        public string SendRequestFromTestRequestJson(EndpointData endpoint)
        {
            //Открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            string id = EndpointId(endpoint.Name);

            //Тесты > Тест запроса (формат JSON)
            OpenTestRequestJson();

            //Выбор контура > Текст запроса
            FillFormRequestJson(id);

            //Отправить запрос
            SendRequest();

            //проверка отображения присвоенного идентификатора
            return IsRecidExiscText();
        }

        public bool SendRequestFromTestRequestXml(EndpointData endpoint)
        {
            //Открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            string id = EndpointId(endpoint.Name);

            //Тесты > Тест запроса (формат XML)
            OpenTestRequestXml();

            //Выбор контура > Текст запроса
            FillFormRequestXml(id);

            //Отправить запрос
            SendRequest();

            //проверка отображения присвоенного идентификатора
            return IsRecidExist();
        }

        public string IsRecidExiscText()
        {
            Thread.Sleep(6000);
            return driver.FindElement(By.CssSelector("label[for='responseText']")).Text;
        }

        //отправка ответа как ПОСТАВЩИК через тестирование запроса (формат JSON)
        public string SendResponseFromInterfaceIsProviderJson(EndpointData endpoint, string recid)
        {
            //Открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            string id = EndpointId(endpoint.Name);

            //Тесты > Тест запроса (формат JSON)
            OpenTestRequestJson();

            //Выбор контура > Текст запроса
            FillFormRequestIsProviderJson(id, recid);

            //Отправить запрос
            SendRequest();

            return IsRecidExiscText();
        }
        //проверка наличия элемента
        public bool IsRecidExist()
        {
            Thread.Sleep(6000);
            return IsElementPresent(By.CssSelector("a[title='Посмотреть детали по запросу']"));
        }

        //отправка ответа как ПОСТАВЩИК через тестирование запроса (формат XML)
        public bool SendResponseFromInterfaceIsProviderXml(EndpointData endpoint, string recid)
        {
            //Открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            string id = EndpointId(endpoint.Name);

            //Тесты > Тест запроса (формат XML)
            OpenTestRequestXml();

            //Выбор контура > Текст запроса
            FillFormRequestIsProviderXml(id, recid);

            ImportXmlResponse();

            //Отправить запрос
            SendRequest();

            return IsRecidExist();
        }

        public void ImportXmlResponse()
        {
            IWebElement fileInput = driver.FindElement(By.Name("dataXml"));
            fileInput.SendKeys(@"C:\Users\Melad\source\repos\RDEV.Gate\rdev.gate.automation\RDEV.Gate.Tests\bin\Debug\netcoreapp2.1\Files\XMLIsProvider.xml");
        }

        public void FillFormRequestIsProviderXml(string id, string recid)
        {
            driver.FindElement(By.XPath("//select[@id='fileType']//option[@value='" + id + "']")).Click();
            driver.FindElement(By.CssSelector("input[name='radioGroupProvider'][value='2']")).Click();
            driver.FindElement(By.Id("isTest")).Click();
            driver.FindElement(By.CssSelector("input[id='replyTo']")).SendKeys(recid);
        }

        //отправить запрос
        public void SendRequest()
        {
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }
        //заполнить форму в тестировании запроса формат JSON
        public void FillFormRequestIsProviderJson(string id, string recid)
        {
            driver.FindElement(By.XPath("//select[@id='fileType']//option[@value='" + id + "']")).Click();
            driver.FindElement(By.CssSelector("input[name='radioGroupProvider'][value='2']")).Click();
            driver.FindElement(By.CssSelector("input[id='replyTo']")).SendKeys(recid);
            IWebElement request = driver.FindElement(By.CssSelector("textarea.ace_text-input"));

            using (var reader = new StreamReader("jsonIsProvider.json"))
            {
                request.SendKeys(reader.ReadToEnd());
            }
        }


        public void FillFormRequestJson(string id)
        {
            driver.FindElement(By.XPath("//select[@id='fileType']//option[@value='" + id + "']")).Click();
            IWebElement request = driver.FindElement(By.CssSelector("textarea.ace_text-input"));

            using (var reader = new StreamReader("json.json"))
            {
                request.SendKeys(reader.ReadToEnd());
            }
        }

        //заполнить форму в тестировании запроса формат XML
        public void FillFormRequestXml(string id)
        {
            driver.FindElement(By.XPath("//select[@id='fileType']//option[@value='" + id + "']")).Click();
            driver.FindElement(By.Id("isTest")).Click();
            IWebElement request = driver.FindElement(By.CssSelector("textarea.ace_text-input"));

            var xml = "";
            using (var reader = new StreamReader("Files\\xml.xml"))
            {
                xml = reader.ReadLine();
            }
            request.SendKeys(xml.Remove(xml.Length - 49, 49));
        }

        //сохранение идентификатора контура
        public string EndpointId(string name)
        {
            OpenSettingEndpoints();
            Thread.Sleep(600);
            return SaveEndpointId(name);
        }

        //открыть страницу со списком контуров
        public void OpenSettingEndpoints()
        {
            driver.FindElements(By.CssSelector("li.dropdown"))[0].Click();
            driver.FindElements(By.TagName("li"))[3].FindElement(By.TagName("a")).Click();
        }
        //получение идентификатора контура
        public string SaveEndpointId(string name)
        {
            Thread.Sleep(2000);
            string id = driver.FindElement(By.XPath("//td[contains(text(), '" + name + "')]/..")).GetAttribute("data-id");
            return id;
        }
        //открыть страницу тестирования запроса формат JSON
        public void OpenTestRequestJson()
        {
            driver.FindElements(By.CssSelector("li.dropdown"))[1].Click();
            driver.FindElements(By.CssSelector("ul.dropdown-menu"))[1]
                .FindElements(By.TagName("li"))[0]
                .FindElement(By.TagName("a")).Click();
        }
        //открыть страницу тестирования запроса формат XML
        public void OpenTestRequestXml()
        {
            driver.FindElements(By.CssSelector("li.dropdown"))[1].Click();
            driver.FindElements(By.CssSelector("ul.dropdown-menu"))[1]
                .FindElements(By.TagName("li"))[1]
                .FindElement(By.TagName("a")).Click();
        }
        //проверка наличия запроса в списке
        public bool IsRequestExist(string requestId)
        {
            Thread.Sleep(3000);
            return IsElementPresent(By.XPath("//a[@href='/smev/requests/" + requestId + "']"));
        }
        //открыть страницу журнала отправленных запросов
        public void OpenRequestsPage()
        {
            driver.FindElements(By.CssSelector("li.dropdown"))[2].Click();
            driver.FindElements(By.CssSelector("ul.dropdown-menu"))[2]
                .FindElements(By.TagName("li"))[0]
                .FindElement(By.TagName("a")).Click();
        }
        //проверка наличия полученного запроса в списке
        public bool IsResponseExist(string smevId)
        {
            Thread.Sleep(2000);
            return IsElementPresent(By.XPath("//td[contains(text(), '" + smevId + "')]"));
        }

        //открыть журнал принятых запросов
        public void OpenResponsesPage()
        {
            driver.FindElements(By.CssSelector("li.dropdown"))[2].Click();
            driver.FindElements(By.CssSelector("ul.dropdown-menu"))[2]
                .FindElements(By.TagName("li"))[1]
                .FindElement(By.TagName("a")).Click();
        }

        //выбор радиокнопки по поиску идентификатора в СМЭВ
        public void RadioButtonSmevId()
        {
            driver.FindElement(By.CssSelector("input[name='radioGroupTest'][value='1']")).Click();
        }
        //проверка отображения присвоенного идентификатора
        public string GetRecidText()
        {
            Thread.Sleep(6000);
            return driver.FindElement(By.CssSelector("a[title='Посмотреть детали по запросу']")).Text;
        }
        //поиск по идентификатору
        public void SearchigId(string Id)
        {
            driver.FindElement(By.CssSelector("input[placeholder='Идентификатор']")).SendKeys(Id);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        public void ChangeSignService(string endpointId, string signServiceId)
        {
            manager.Navigation.OpenHomePage();
            OpenEndpoint(endpointId);
            ChoiceSignService(signServiceId);

        }
        //открыть страницу с контуром
        public void OpenEndpoint(string endpointId)
        {
            driver.Url =
                $"{BaseUrl}/settings/endpoints/{endpointId}";
        }

        //выбор сервиса подписания в контуре
        public void ChoiceSignService(string signServiceId)
        {
            Thread.Sleep(2000);
            driver.FindElement(By.XPath($"//select[@id='fileType']//option[@value='{signServiceId}']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }
        //получаем идентификатор необходимого СП
        public string GetRecidSignServiceForEndpoint(bool signService)
        {
            var Context = new RdevContext();
            //если указан удаленный Редок, то берем идентификатор СП удаленный Редок
            if (signService)
            {
                string recIdSingService = Context.RsmevSignservices.Where(
                x => x.ServiceTypeEnum == 1).Select(x => x.Recid).FirstOrDefault().ToString();

                return recIdSingService;
            }
            //иначе необходимо получить идентификатор встроенного СП
            else
            {
                string recIdSingService = Context.RsmevSignservices.Where(
                x => x.ServiceTypeEnum == 0).Select(x => x.Recid).FirstOrDefault().ToString();

                return recIdSingService;
            }
        }
    }
}