using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Edge;

namespace SharedSwdCore.WebDriver
{
    public class WebDriverRunner
    {
        public const string BrowserFirefox = "Firefox";
        public const string BrowserChrome = "Chrome";
        public const string BrowserInternetExplorer = "InternetExplorer";
        public const string BrowserPhantomJs = "PhantomJS";
        public const string BrowserHtmlUnit = "HtmlUnit";
        public const string BrowserHtmlUnitWithJavaScript = "HtmlUnitWithJavaScript";
        public const string BrowserOpera = "Opera";
        public const string BrowserSafari = "Safari";
        public const string BrowserIPhone = "IPhone";
        public const string BrowserIPad = "IPad";
        public const string BrowserAndroid = "Android";
        public const string BrowserNgWebDriver = "NgWebDriver";
        public const string BrowserEdge = "Edge";


        public static IWebDriver Run(string browserName, bool isRemote, string remoteUrl)
        {
            IWebDriver driver;
            if (isRemote)
            {
                driver = ConnetctToRemoteWebDriver(browserName, remoteUrl);
            }
            else
            {
                driver = StartEmbededWebDriver(browserName);

                // Добавила строку, чтобы увеличить окно
                driver.Manage().Window.Maximize();
                //driver.Manage().Window.Size = new Size(1024, 768);
            }
            return driver;
        }

        private static IWebDriver ConnetctToRemoteWebDriver(string browserName, string remoteUrl)
        {
            DesiredCapabilities caps = null;
            Uri hubUri = new Uri(remoteUrl);

            // устанавливаю браузеру начинать действовать если часть элементов на странице уже загрузилась
            caps.SetCapability(CapabilityType.PageLoadStrategy, "normal");

            switch (browserName)
            {

                case BrowserFirefox:
                    caps = DesiredCapabilities.Firefox();
                    break;
                case BrowserChrome:
                    caps = DesiredCapabilities.Chrome();
                    break;
                case BrowserEdge:
                    caps = DesiredCapabilities.Edge();
                    break;
                case BrowserInternetExplorer:
                    caps = DesiredCapabilities.InternetExplorer();
                    //caps.SetCapability("InternetExplorer", "ignoreZoomSetting");
                    //InternetExplorerOptions options = new InternetExplorerOptions {IgnoreZoomLevel = true};
                    //InternetExplorerDriver driver = new InternetExplorerDriver(options);
                    break;
                case BrowserPhantomJs:
                    caps = DesiredCapabilities.PhantomJS();
                    break;
                case BrowserHtmlUnit:
                    caps = DesiredCapabilities.HtmlUnit();
                    break;
                case BrowserHtmlUnitWithJavaScript:
                    caps = DesiredCapabilities.HtmlUnitWithJavaScript();
                    break;
                case BrowserOpera:
                    caps = DesiredCapabilities.Opera();
                    break;
                case BrowserSafari:
                    caps = DesiredCapabilities.Safari();
                    break;
                case BrowserIPhone:
                    caps = DesiredCapabilities.IPhone();
                    break;
                case BrowserIPad:
                    caps = DesiredCapabilities.IPad();
                    break;
                case BrowserAndroid:
                    caps = DesiredCapabilities.Android();
                    break;
                default:
                    throw new ArgumentException(String.Format(@"<{0}> was not recognized as supported browser. This parameter is case sensitive", browserName) + 
                                                "WebDriverOptions.BrowserName");
            }
            RemoteWebDriver newDriver = new RemoteWebDriver(hubUri, caps);
            return newDriver;
        }

        private static IWebDriver StartEmbededWebDriver(string browserName)
        {
            switch (browserName)
            {
                case BrowserFirefox:
                    return new FirefoxDriver();
                case BrowserChrome:
                    return new ChromeDriver();
                case BrowserEdge:
                    return new EdgeDriver();
                case BrowserInternetExplorer:
                    //InternetExplorerOptions options = new InternetExplorerOptions { IgnoreZoomLevel = true };
                    return new InternetExplorerDriver();
                case BrowserPhantomJs:
                    return new PhantomJSDriver();
                case BrowserSafari:
                    return new SafariDriver();
                default:
                    throw new ArgumentException(String.Format(@"<{0}> was not recognized as supported browser. This parameter is case sensitive", browserName) + "WebDriverOptions.BrowserName");
            }
        }
    }
}
