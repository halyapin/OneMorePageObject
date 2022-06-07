using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OneMorePageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;



namespace OneMorePageObject
{
    public class JsonReqMagIdTest : TestBase
    {
        /*public string RequestId { get; set; }

        [Test]
        public void RequestMagIdTestJson()
        {

            //зайти в журнал отправленного
            SendMagPageObject verifyMagjson = new SendMagPageObject(_driver);
            verifyMagjson.VerifyMagjson();
            //отправить запрос request json и спрарсить id в response
            SignServiceAddShluz signServiceAdd = new SignServiceAddShluz(_driver);
            signServiceAdd.RemoteSignServiceAdd();
            //проверить что id совпал и что запрос отправлен и ответ получен без ошибок
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
            RequestId = app.Gate.SendRequestPost(requestJson);

            Assert.NotNull(RequestId, "requestId (ответ на POST) равен null (1.1.1.1)");

            Assert.IsTrue(app.Gate.CheckingRequestFromInterface(RequestId), "Не найден запрос по id в журнале ОТПРАВЛЕННЫХ (1.1.3)");


        }*/
    }
}
