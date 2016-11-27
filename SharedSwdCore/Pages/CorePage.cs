using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedSwdCore.WebDriver;

namespace SharedSwdCore.Pages
{
    public abstract class CorePage
    {
        protected IWebDriver Driver
        {
            get { return SwdBrowser.Driver; }
        }

        //test
        /*public NgWebDriver Driver
        {
            get
            {
                return new NgWebDriver(SwdBrowser.Driver);
            }
        }*/

        private void VerifyElementVisible(IWebElement webElement)
        {
            try
            {
                ScrollTo(webElement);
            }
            finally
            {
                if (!webElement.Displayed)
                {
                    string message = "<" + webElement + "> \n"
                                     + "was expected to be visible, "
                                     + "but the element was not found on the page.";
                    throw new Exception(message);
                }
            }
        }

        protected CorePage()
        {
            PageFactory.InitElements(Driver, this);
        }

        protected void Click(IWebElement element)
        {
            Wait.WaitForAjax();
            VerifyElementVisible(element);
            element.Click();
        }

        protected void ScrollTo(IWebElement element)
        {
            IJavaScriptExecutor js = SwdBrowser.Driver as IJavaScriptExecutor;
            SwdBrowser.HandleJavaScriptErrors();
            js.ExecuteScript("arguments[0].scrollIntoView()", element);
            SwdBrowser.HandleJavaScriptErrors();
            Wait.WaitForAjax();
            SwdBrowser.HandleJavaScriptErrors();
        }

        protected bool IsElementPresent(IWebElement element)
        {
            try
            {
                return !element.Size.IsEmpty;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("NoSuchElementException expection: " + e);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("IsElementPresent expection: " + e);
                return false;
            }
        }
    }
}