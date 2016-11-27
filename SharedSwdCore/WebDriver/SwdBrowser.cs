using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SharedSwdCore.Configuration;

namespace SharedSwdCore.WebDriver
{
    public static class SwdBrowser
    {
        private static IWebDriver _driver;
        private static WebDriverWait _wait;



        /// <summary>
        /// Returns current WebDriver instance.    
        /// 
        /// * When the Driver was already created and the browser was opened – the 
        ///   property returns a reference to current browser.  
        /// * If the Driver was not initialized yet – it will create a new browser 
        ///   (WebDriver) instance automatically, according to the configuration file.  
        /// </summary>
        public static IWebDriver Driver
        {
            get
            {
                string swdBrowserType = Config.SwdBrowserType;
                if (swdBrowserType == "WebdriverSystem")
                {
                    swdBrowserType = Environment.GetEnvironmentVariable("WebdriverSystem");
                }

                if (_driver == null)
                {
                    _driver = WebDriverRunner.Run(swdBrowserType,
                        Config.WdIsRemote,
                        Config.WdRemoteUrl);
                    //_driver = eyes.Open(_driver, "Applitools", "Test Web Page", new Size(1024, 768));
                }
                return _driver;
            }
        }

        public static WebDriverWait Wait
        {
            get
            {
                if (_wait == null)
                {
                    _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                }
                return _wait;
            }
            set { _wait = value; }
        }

        /// <summary>
        /// Closes the current WebDriver instance (and a web-browser window)
        /// </summary>
        public static void CloseDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
            //foreach (var process in Process.GetProcessesByName("chromedriver.exe"))
            //{
            //    process.Kill();
            //}
            //foreach (var process in Process.GetProcessesByName(Config.SwdBrowserType))
            //{
            //    process.Kill();
            //}
        }

        /// <summary>
        /// Executes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="jsCode">JavaScript code block</param>
        /// <returns>The value returned by the script</returns>
        public static object ExecuteScript(string jsCode)
        {
            return ((IJavaScriptExecutor) Driver).ExecuteScript(jsCode);
        }

        /// <summary>
        /// *Executes JavaScript code block inside opened Web-Browser*   
        ///  
        /// Collects JavaScript errors on the page and throws JavaScriptErrorOnThePageException 
        /// in case unhandled JavaScript errors had occurred on the WebPage   
        /// During the first call on the specific web page, this method injects a script 
        /// for error collection into the web page. The next calls will check if there 
        /// are errors collected.   
        /// If any JavaScript errors are captured – the method will throw JavaScriptErrorOnThePageException
        /// </summary>
        public static void HandleJavaScriptErrors()
        {
            try
            {
                string jsCode =
                #region JavaScript Error Handler code
 @"
                    if (typeof window.jsErrors === 'undefined') 
                    {
                        window.jsErrors = '';
                        window.onerror = function (errorMessage, url, lineNumber) 
                                         {
                                              var message = 'Error: [' + errorMessage + '], url: [' + url + '], line: [' + lineNumber + ']';
                                              message = message + ""\n"";
                                              window.jsErrors += message;
                                              return false;
                                         };
                    }

                    var errors = window.jsErrors;
                    window.jsErrors = '';
                    return errors;";
                #endregion

                var errors = (string)ExecuteScript(jsCode);

                if (!string.IsNullOrEmpty(errors))
                {
                    throw new JavaScriptErrorOnThePageException(errors);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new JavaScriptErrorOnThePageException(e.Message);
            }
        }

        public static Screenshot TakeScreenshot()
        {
            return Driver.TakeScreenshot();
        }

        public static string GetApplicationMainUrl()
        {
            string appUrl = Config.ApplicationMainUrl;
            if (appUrl == "urlSystem")
            {
                appUrl = Environment.GetEnvironmentVariable("urlSystem");
            }
            return appUrl;
        }
    }
}
