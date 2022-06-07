using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OneMorePageObject
{
    //создаем класс логина
    public class SmevSignServicesShluz
    {
        private IWebDriver _driver;
        //почему не видит переменные в публичном классе browser?
        public int delay1 = 1000;
        public int delay2 = 2000;

        public SmevSignServicesShluz(IWebDriver driver)
        {
            _driver = driver;
        }

        //указываем как драйверу найти элементы куда вносить данные
        //settings
        public IWebElement smevSettingsField => _driver.FindElement(By.XPath("/html/body/div[1]/div/div[1]/nav/div/div[2]/ul[1]/li[1]/a"));
        public IWebElement smevSignservicesField => _driver.FindElement(By.XPath("//a[@href ='/smev/settings/signservices']"));

        public IWebElement smevSignservicesAddField => _driver.FindElement(By.XPath("//a[@href ='/smev/settings/addsignservice']"));
        //поиск по наименованию сервисов подписания
        public IWebElement nameSignservicesField => _driver.FindElement(By.XPath("//td"));

        //создаем метод который будет производить действия
        public void SignServices()
        {
            Thread.Sleep(delay1);
            smevSettingsField.Click();
            Thread.Sleep(delay1);
            smevSignservicesField.Click();
            Thread.Sleep(delay1);
        }
    }
}
