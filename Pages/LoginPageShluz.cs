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
    public class LoginPageShluz
    {
        protected IWebDriver _driver;
        public int delay1 = 1000;
        public int delay2 = 2000;
        public LoginPageShluz(IWebDriver driver)
        {
            _driver = driver;
        }
        public IWebElement userNameField => _driver.FindElement(By.XPath("//input[@placeholder='Логин']"));
        //public IWebElement userNameField => _driver.FindElement(By.Name("//input[@placeholder='Логин']"));
        public IWebElement passwordField => _driver.FindElement(By.XPath("//input[@placeholder='Пароль']"));
        public IWebElement btnLogin => _driver.FindElement(By.XPath("//button[@type='submit']"));
        //public IWebElement btnLogin => _driver.FindElement(By.LinkText("Войти"));

        public void Login(string userName, string userPassword)
        {
            userNameField.SendKeys(userName);
            passwordField.SendKeys(userPassword);
            btnLogin.Submit();
        }

    }
}
