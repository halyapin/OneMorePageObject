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
    public class SignServicesAddPositiveTest : TestBase
    {
        [Test]
        public void SignServicesAdd()
        {

            SmevSignServicesShluz smevSignServices = new SmevSignServicesShluz(_driver);
            smevSignServices.SignServices();
            SignServiceAddShluz signServiceAdd = new SignServiceAddShluz(_driver);
            signServiceAdd.RemoteSignServiceAdd();

        }
    }
}
