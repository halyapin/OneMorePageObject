using OpenQA.Selenium;
//using RDEV.Gate.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OneMorePageObject
{
    public class NavigationHelper : HelperBase
    {
        public string baseURL;
        public NavigationHelper(ApplicationManager manager, string baseURL)
            : base(manager)
        {
            this.baseURL = baseURL;
        }
        public void OpenHomePage()
        {
            LoginData login = new LoginData("superadmin", "2128506");

            if (driver.Url == baseURL)
            {
                return;
            }
            driver.Navigate().GoToUrl(baseURL);
            Thread.Sleep(1000);
            if (IsLoginPage())
            {
                Login(login);
            }
        }
        public void Login(LoginData login)
        {
            driver.FindElement(By.XPath("//a[@href='" + baseURL.Remove(baseURL.LastIndexOf(@"/"))
                + "/login?return=%2Fsmev']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("input[placeholder='Логин']")).SendKeys(login.Login);
            driver.FindElement(By.CssSelector("input[placeholder='Пароль']")).SendKeys(login.Password);
            driver.FindElement(By.CssSelector("button[type='Submit']")).Click();
            Thread.Sleep(1500);
        }

        public bool IsLoginPage()
        {
            Thread.Sleep(1000);
            return IsElementPresent(By.XPath("//a[@href='" + baseURL.Remove(baseURL.LastIndexOf(@"/"))
                + "/login?return=%2Fsmev']"));
        }
    }
    }
