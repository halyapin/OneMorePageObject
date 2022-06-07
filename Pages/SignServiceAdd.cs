using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json;
using System.IO;
using OpenQA.Selenium.Remote;
using System.Threading;

namespace OneMorePageObject
{
    //создаем класс логина
    public class SignServiceAddShluz
    {
        private IWebDriver _driver;
        public int delay1 = 1000;
        public int delay2 = 2000;
        public SignServiceAddShluz(IWebDriver driver)
        {
            _driver = driver;
        }

        //addSignServices
        protected bool serviceSign;
        static Random rnd = new Random();
        static int numSignService = rnd.Next(0, 10);
        string signServiceId = "0";


        public readonly string recName = "АвтотестСервисПодписания" + numSignService;

        
        public readonly string urlSignService = "http://10.10.12.34:8081/api/sign/xml";
        public readonly By _trumbprintSignServiceField = By.XPath("//input[@id ='thumbprint']");
        public readonly string trumbprintSignService = "40ce3fe910279606aa8b3166c586c6cfa92a0448";
        public readonly By _btnSaveSignServiceField = By.XPath("//button[@type='submit']");
        public readonly By _labelSaveField = By.XPath("//label[@for='responseTemplate']");
        public readonly By _btnBackSignServiceField = By.XPath("//a[@title='Назад']");
        public readonly By _nameSignservicesField = By.XPath("//td");

        public IWebElement smevSignservicesAddField => _driver.FindElement(By.XPath("//a[@href ='/smev/settings/addsignservice']"));
        public IWebElement recNameSignServiceField => _driver.FindElement(By.XPath("//input[@placeholder ='Наименование']"));
        public IWebElement serviceTypeEnumSignServiceField => _driver.FindElement(By.XPath("//select[@placeholder ='Выбор типа сервиса']"));
        public IWebElement typeEnumSignServiceField => _driver.FindElement(By.XPath("//option[@value ='1']")); //здесь указываешь какой тип сервиса подписания
        public IWebElement urlSignServiceField => _driver.FindElement(By.XPath("//input[@id ='url']"));
        public IWebElement trumbprintSignServiceField => _driver.FindElement(By.XPath("//input[@id ='thumbprint']"));
        public IWebElement btnSaveSignServiceField => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement labelSaveField => _driver.FindElement(By.XPath("//label[@for='responseTemplate']"));
        public IWebElement btnBackSignServiceField => _driver.FindElement(By.XPath("//a[@title='Назад']"));
        public IWebElement nameSignservicesField => _driver.FindElement(By.XPath("//td"));



        //после public писать именно имя КЛАССА!
        public void RemoteSignServiceAdd()
        {
            smevSignservicesAddField.Click();
            Thread.Sleep(delay1);
            recNameSignServiceField.SendKeys(recName);
            Thread.Sleep(delay1);
            serviceTypeEnumSignServiceField.Click();
            Thread.Sleep(delay1);
            typeEnumSignServiceField.Click();
            Thread.Sleep(delay1);
            if (typeEnumSignServiceField.Text == "Удалённый Redoc")
            {
                urlSignServiceField.SendKeys(urlSignService);
            }
            else return;
            trumbprintSignServiceField.SendKeys(trumbprintSignService);
            Thread.Sleep(delay1);
            btnSaveSignServiceField.Submit();
            //вставить проверку на появление надписи ниже кнопки "Запись успешно сохранена".
            //if (driver.FindElement(_labelSaveField).Text == "Запись успешно сохранена.")
            //Assert.AreEqual(_labelSaveField.Text.ToLower(), "Запись успешно сохранена.".ToLower());
            Thread.Sleep(delay2);
            btnBackSignServiceField.Click();
            Thread.Sleep(delay1);
            //вставить проверку на нахождение на странице сервиса подписания с recName каким мы только что создали.
            //путь до таблицы сервисов подписания tbody/tr/td text=АвтотестСервисПодписания
            //Assert.AreEqual(_labelSaveField.Text.ToLower(), "Запись успешно сохранена.".ToLower());
            if (nameSignservicesField.Text != recName)
            {
                Thread.Sleep(delay1);
                smevSignservicesAddField.Click();
            }

        }
    }
}
