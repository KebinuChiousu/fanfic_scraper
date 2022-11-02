using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using HtmlScraper.Utility.Browser.HTML;
using Microsoft.VisualBasic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using NHibernate.Driver;

namespace HtmlScraper.Utility.Browser
{

    public class clsWeb : IDisposable
    {

        private IWebDriver _driver;
        private FirefoxOptions _options;
        private FirefoxDriverService _service;
        private bool _cookie = false;
        private IWebElement _submit;

        // Private UserAgent As String = "Chrome/34.0.1847.131"

        public clsWeb()
        {

            _service = FirefoxDriverService.CreateDefaultService(Environment.CurrentDirectory);

            _service.HideCommandPromptWindow = true;

            _options = new FirefoxOptions();
            // _options.AddAdditionalCapability("phantomjs.page.settings.userAgent", UserAgent)

            _driver = new FirefoxDriver(_service, _options);
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

        }

        public void Dispose()
        {

            _driver.Quit();
            _driver = null;
            _options = null;
            _service = null;

        }

        private bool InnerElements(IWebElement ctrl)
        {

            bool ret = false;
            string html;
            HtmlAgilityPack.HtmlDocument doc = null;

            html = ctrl.GetAttribute("outerHTML");

            doc = modHTML.CleanHTML(ref html);

            if (doc.DocumentNode.FirstChild.HasChildNodes)
            {

                if (doc.DocumentNode.FirstChild.ChildNodes.Count > 1)
                {
                    ret = true;
                }
                else if (doc.DocumentNode.FirstChild.FirstChild.Name == "#text")
                {
                    ret = false;
                }
                else
                {
                    ret = true;
                }
            }

            doc = null;

            return ret;

        }

        private bool CheckTag(string name)
        {

            switch (name ?? "")
            {
                case "input":
                case "select":
                    {
                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }


        }

        private void ProcessFields(IWebElement item, ref List<KeyValuePair<string, string>> Fields)


        {

            int idx;
            KeyValuePair<string, string> kvp;
            bool ret;

            if (CheckTag(item.TagName))
            {

                var loopTo = Fields.Count - 1;
                for (idx = 0; idx <= loopTo; idx++)
                {

                    kvp = Fields[idx];
                    ret = ProcessElement(ref item, ref kvp);

                    if (ret == true)
                    {
                        break;
                    }

                }

            }

        }

        private bool ProcessElement(ref IWebElement item, ref KeyValuePair<string, string> kvp)


        {

            SelectElement sel;
            bool ret = false;
            string name;
            string type;

            name = item.GetAttribute("name");
            type = item.GetAttribute("type");

            if ((name ?? "") == (kvp.Key ?? ""))
            {
                switch (type ?? "")
                {
                    case "text":
                    case "password":
                        {
                            item.SendKeys(kvp.Value);
                            break;
                        }
                    case "checkbox":
                        {
                            item.Click();
                            break;
                        }
                    case "select-one":
                        {
                            sel = new SelectElement(item);
                            sel.SelectByValue(kvp.Value);
                            break;
                        }
                }
                ret = true;
            }

            if (!ret)
            {
                if ((type ?? "") == (kvp.Key ?? ""))
                {
                    if ((item.Text ?? "") == (kvp.Value ?? "") | (item.GetAttribute("value") ?? "") == (kvp.Value ?? ""))
                    {
                        _submit = item;
                    }
                }
            }


            return ret;

        }

        private IWebElement? GetInnerElements(IWebElement? ctrl, ref List<KeyValuePair<string, string>> Fields)
        {

            if (ctrl == null)
                return null;

            IWebElement? ret = null;

            ReadOnlyCollection<IWebElement> ChildElements;

            ChildElements = ctrl.FindElements(By.XPath("*"));

            foreach (IWebElement item in ChildElements)
            {

                if (InnerElements(item))
                {
                    ret = GetInnerElements(item, ref Fields);
                }

                ProcessFields(ret, ref Fields);

            }

            return null;

        }

        public void FollowLink(string URL, string LinkText, string cookieName)
        {

            ReadOnlyCollection<OpenQA.Selenium.Cookie> cookies;
            var cookies2 = new List<OpenQA.Selenium.Cookie>();
            IWebElement link;
            string ret;

            _driver.Navigate().GoToUrl(URL);

            link = _driver.FindElement(By.LinkText(LinkText));

            link.Click();

            ret = _driver.PageSource;

            cookies = _driver.Manage().Cookies.AllCookies;

            for (int idx = 0, loopTo = cookies.Count - 1; idx <= loopTo; idx++)
                cookies2.Add(cookies[idx]);

            WriteCookiesToDisk(cookieName, cookies2);

        }

        public void LogIn(string URL, string formName, List<KeyValuePair<string, string>> Fields, string cookieName)
        {

            ReadOnlyCollection<OpenQA.Selenium.Cookie> cookies;
            var cookies2 = new List<OpenQA.Selenium.Cookie>();

            int idx;
            IWebElement formElement;
            ReadOnlyCollection<IWebElement> forms;
            ReadOnlyCollection<IWebElement> allFormChildElements;

            IWebElement btn = null;

            _driver.Navigate().GoToUrl(URL);

            forms = _driver.FindElements(By.TagName("form"));

            formElement = forms[0];

            allFormChildElements = formElement.FindElements(By.XPath("*"));

            IWebElement? ret = null;

            foreach (IWebElement item in allFormChildElements)
            {

                if (InnerElements(item))
                {
                    ret = GetInnerElements(item, ref Fields);
                }

                ProcessFields(ret, ref Fields);

            }

            _submit.Submit();

            cookies = _driver.Manage().Cookies.AllCookies;

            var loopTo = cookies.Count - 1;
            for (idx = 0; idx <= loopTo; idx++)
                cookies2.Add(cookies[idx]);

            WriteCookiesToDisk(cookieName, cookies2);

        }

        public string DownloadPage(string URL, string Cookie = "")
        {

            List<OpenQA.Selenium.Cookie> cookies;
            int idx;

            string ret = "";

            _driver.Navigate().GoToUrl(URL);

            if (File.Exists(Environment.CurrentDirectory + @"\" + Cookie))
            {

                cookies = ReadCookiesFromDisk(Cookie, URL);
                _driver.Manage().Cookies.DeleteAllCookies();

                var loopTo = cookies.Count - 1;
                for (idx = 0; idx <= loopTo; idx++)
                    _driver.Manage().Cookies.AddCookie(cookies[idx]);

                _driver.Navigate().GoToUrl(URL);

                _cookie = true;

            }

            ret = _driver.PageSource;

            return ret;

        }

        private void WriteCookiesToDisk(string fileName, List<OpenQA.Selenium.Cookie> cookieJar)
        {

            var cookies = new CookieContainer();
            System.Net.Cookie cookie;

            int idx;

            var loopTo = cookieJar.Count - 1;
            for (idx = 0; idx <= loopTo; idx++)
            {
                cookie = new System.Net.Cookie(cookieJar[idx].Name, cookieJar[idx].Value, cookieJar[idx].Path, cookieJar[idx].Domain) { Expires = (DateTime)cookieJar[idx].Expiry };

                cookies.Add(cookie);

            }

            using (Stream stream = File.Create(fileName))
            {
                try
                {
                    // MsgBox("Writing cookies to disk... ")
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cookies);
                }
                // MsgBox("Done.")
                catch (Exception e)
                {
                    Interaction.MsgBox("Problem writing cookies to disk: " + e.Message);
                }
            }
        }

        private List<OpenQA.Selenium.Cookie> ReadCookiesFromDisk(string fileName, string URL)
        {

            CookieContainer cookies;
            CookieCollection cookiec;

            var ts = new TimeSpan();

            var dte = DateTime.Now;

            dte = dte.AddDays(365d);

            var u = new Uri(URL);

            // URL = u.Scheme & Uri.SchemeDelimiter & GetDomain(u)

            // u = Nothing
            // u = New System.Uri(URL)

            int idx;

            var cookiejar = new List<OpenQA.Selenium.Cookie>();
            OpenQA.Selenium.Cookie cookie;

            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Open))
                {
                    // MsgBox("Reading cookies from disk... ")
                    var formatter = new BinaryFormatter();
                    // MsgBox("Done.")
                    cookies = (CookieContainer)formatter.Deserialize(stream);
                    cookiec = cookies.GetCookies(u);

                    var loopTo = cookiec.Count - 1;
                    for (idx = 0; idx <= loopTo; idx++)
                    {
                        cookie = new OpenQA.Selenium.Cookie(cookiec[idx].Name, cookiec[idx].Value, cookiec[idx].Domain, cookiec[idx].Path, dte);






                        cookiejar.Add(cookie);

                    }


                }
            }
            catch (Exception e)
            {
                Interaction.MsgBox("Problem reading cookies from disk: " + e.Message);
                cookiejar = null;
            }

            return cookiejar;

        }

        private string GetDomain(Uri u)
        {
            if (u.HostNameType != UriHostNameType.Dns)
                return string.Empty;
            var parts = u.Host.Split('.');
            if (parts.Length > 1)
            {
                return string.Join(".", parts, parts.Length - 2, 2);
            }
            else
            {
                return parts[0];
            }
        }

    }
}