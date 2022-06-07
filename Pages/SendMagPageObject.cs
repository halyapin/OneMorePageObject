using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OneMorePageObject
{
    public class SendMagPageObject
    {
        private IWebDriver _driver;
        public int delay1 = 1000;
        public int delay2 = 2000;
        public SendMagPageObject(IWebDriver driver)
        {
            _driver = driver;
        }


        //magazines

        public IWebElement smevMagField => _driver.FindElement(By.XPath("//html/body/div[1]/div/div[1]/nav/div/div[2]/ul[1]/li[3]/a"));
        public IWebElement smevRequestsField => _driver.FindElement(By.XPath("//a[@href ='/smev/requests']"));


        /*
        //после public писать именно имя КЛАССА!
        public void VerifyMagjson()
        {
            smevMagField.Click();
            Thread.Sleep(delay1);
            smevRequestsField.Click();
            Thread.Sleep(delay1);
        }
        public void OpenRequestsPage()
        {
            driver.FindElements(By.CssSelector("li.dropdown"))[2].Click();
            driver.FindElements(By.CssSelector("ul.dropdown-menu"))[2]
                .FindElements(By.TagName("li"))[0]
                .FindElement(By.TagName("a")).Click();
        }

            //открыть журнал отправленных запросов
            OpenRequestsPage();

        //поиск по идентификатору
        public bool CheckingRequestFromInterface(string requestId)
        {
            //открыть домашнюю страницу
            manager.Navigation.OpenHomePage();

            //открыть журнал отправленных запросов
            OpenRequestsPage();

            //поиск по идентификатору
            SearchigId(requestId);
            public void SearchigId(string Id)
            {
                driver.FindElement(By.CssSelector("input[placeholder='Идентификатор']")).SendKeys(Id);
                driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            }


            //проверка наличия отправленного запроса в списке
            return IsRequestExist(requestId);
        }


            //проверка наличия отправленного запроса в списке
            return IsRequestExist(requestId);
            //проверка наличия запроса в списке
            /*public bool IsRequestExist(string requestId)
            {
                Thread.Sleep(3000);
                return IsElementPresent(By.XPath("//a[@href='/smev/requests/" + requestId + "']"));
            }*/

    
    }
}
