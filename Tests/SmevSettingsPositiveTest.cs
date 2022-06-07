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
    public class SmevSettingsPositiveTest : TestBase
    {
        [Test]
        public void SettingsPositiveTest()
        {
            SmevPageShluz smevPageShluz = new SmevPageShluz(_driver);
            smevPageShluz.SmevSettingTest();
            smevPageShluz.SmevTestRequestTest();
            smevPageShluz.SmevMagTest();
            smevPageShluz.SmevJobsTest();
        }
    }
}
