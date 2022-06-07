using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json;
using System.IO;

namespace OneMorePageObject
{
    public class TestBase
    {
        protected ApplicationManager app;
        //public readonly string urlLogin = "http://10.10.10.55:5000/login";
        public readonly string urlAuth = "http://10.10.10.55:5000/login?return=%2Fsmev";
        protected IWebDriver _driver;
        
        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            var json = File.ReadAllText("C:\\Users\\v.halyapin\\source\\repos\\OneMorepageObject\\OneMorePageObject\\Settings\\settings.json");
            var settings = JsonConvert.DeserializeObject<SettingsJson>(json);

            app = ApplicationManager.GetInstance(settings);

            //option.AddArguments("--window-size=500,500");
            //driver.Manage().Window.Size = new System.Drawing.Size(200, 100);
            //var json = File.ReadAllText("C:\\RDEV.Gate.Settings\\settings.json");
            //var settings = JsonConvert.DeserializeObject<SettingsJson>(json);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(urlAuth);
            LoginPageShluz loginPageShluz = new LoginPageShluz(_driver);
            loginPageShluz.Login("superadmin", "2128506");
        }
    }
}
