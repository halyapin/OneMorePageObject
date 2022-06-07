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

namespace OneMorePageObject
{
    public class ApplicationManager
    {
        protected IWebDriver driver;
        protected string baseURL;
        protected NavigationHelper navigationHelper;
        protected GateHelper gateHelper;
        protected string endpointNameForProxySmev;
        protected string endpointNameForProxyMock;
        protected string endpointNameForSignService;
        protected bool serviceSign;
        private static ThreadLocal<ApplicationManager> app = new ThreadLocal<ApplicationManager>();

        private ApplicationManager(SettingsJson settings)
        {
            //driver = new ChromeDriver(System.IO.Directory.GetCurrentDirectory());
            baseURL = settings.Gate.Url;
            //driver.Manage().Window.Maximize();

            navigationHelper = new NavigationHelper(this, baseURL);
            gateHelper = new GateHelper(this, baseURL);
            endpointNameForProxySmev = settings.Gate.ChooseEndpointsForProxySmev;
            endpointNameForProxyMock = settings.Gate.ChooseEndpointsForProxyMock;
            endpointNameForSignService = settings.Gate.ChoiceEndpointsForSignService;
            serviceSign = settings.Gate.IsRemoteServiceSign;
        }

        ~ApplicationManager()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        public static ApplicationManager GetInstance(SettingsJson settings)
        {
            if (!app.IsValueCreated)
            {
                ApplicationManager NewInstance = new ApplicationManager(settings);
                app.Value = NewInstance;
            }
            return app.Value;
        }


        public IWebDriver Driver
        {
            get
            {
                return driver;
            }
        }
        public NavigationHelper Navigation
        {
            get
            {
                return navigationHelper;
            }
        }
        public GateHelper Gate
        {
            get
            {
                return gateHelper;
            }
        }

        public string EndpointNameForProxySmev
        {
            get
            {
                return endpointNameForProxySmev;
            }
        }

        public string EndpointNameForProxyMock
        {
            get
            {
                return endpointNameForProxyMock;
            }
        }

        public string EndpointNameForSignService
        {
            get
            {
                return endpointNameForSignService;
            }
        }
        public bool ServiceSign
        {
            get
            {
                return serviceSign;
            }
        }
    }
}
