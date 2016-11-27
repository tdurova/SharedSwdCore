using System;
using OpenQA.Selenium;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;

namespace SharedSwdCore.WebDriver
{
    public static class Wait
    {
        public static void UntilVisible(IWebElement element, string elementName, TimeSpan timeOut)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (true)
            {
                Exception lastException = null;
                try
                {
                    if (element.Displayed)
                    {
                        return;
                    }
                    System.Threading.Thread.Sleep(10);
                }
                catch (NoSuchElementException e)
                {
                    lastException = e;
                }
                catch (Exception e)
                {
                    lastException = e;
                }

                if (sw.Elapsed > timeOut)
                {
                    string exceptionMessage = lastException == null ? "" : lastException.Message;
                    string errorMessage = string.Format(elementName + " was not displayed after {0} Milliseconds" +
                            "\r\nBecause: {1}", timeOut.TotalMilliseconds, exceptionMessage);
                    throw new TimeoutException(errorMessage);
                }
            }
        }

        public static void UntilVisible(IWebElement element, string elementName, int timeOutMilliseconds=5000)
        {
            UntilVisible(element, elementName, TimeSpan.FromMilliseconds(timeOutMilliseconds));
        }


        //private static IWebElement UntilVisible(By by, IWebDriver driver, TimeSpan timeOut)
        //{
        //    WebDriverWait wdWait = new WebDriverWait(driver, timeOut);
        //    wdWait.IgnoreExceptionTypes
        //    (
        //        typeof(ElementNotVisibleException),
        //        typeof(NoSuchElementException),
        //        typeof(StaleElementReferenceException)
        //    );

        //    return wdWait.Until(ExpectedConditions.ElementIsVisible(by));
        //}


        public static void WaitForAjax()
        {
            try
            {
                int i = 1000;
                while (i-- > 0)
                {
                    bool ajaxIsComplete;
                    try
                    {
                        ajaxIsComplete = (bool)((IJavaScriptExecutor) SwdBrowser.Driver).
                            ExecuteScript("var result = true; try { result = (typeof jQuery != 'undefined') ? jQuery.active == 0 : true } catch (e) {}; return result;");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return;
                    }
                    if (ajaxIsComplete)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(100);
                }

                if (i == 0) throw new TimeoutException();
            }
            catch (UnhandledAlertException e)
            {
                Console.WriteLine("\n Unexpected alert: " + e);
                SwdBrowser.Driver.Navigate().Refresh();
            }
        }

    }
}