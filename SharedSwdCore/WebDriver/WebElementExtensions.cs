using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SharedSwdCore.WebDriver
{
    /// <summary>
    /// WebElementExtensions defines extension methods for IWebElement objects.
    /// Extend and simplify the functionality provided by WebDriver Core/Support libraries 
    /// </summary>
    public static class WebElementExtensions
    {
        /// <summary>
        /// Default timeout for <see cref=" WaitUntilVisible"/> methods
        /// </summary>
        public static int DefaultTimeOutMilliseconds = 1000;

        /// <summary>
        /// Waits until element is visible. Internally, uses element.Displayed with ignored WebDriver exceptions
        /// </summary>
        /// <returns></returns>
        public static void WaitUntilVisible(this IWebElement element, string elementName)
        {
            Wait.UntilVisible(element, elementName, TimeSpan.FromMilliseconds(DefaultTimeOutMilliseconds));
        }

        /// <summary>
        /// Replaces WebDriver’s element.Text property. Gets value from 
        /// *input* and *select* tags rather than returning text inside those elements. 
        /// </summary>
        public static string GetElementText(this IWebElement element)
        {
            string result;
            string tag = element.TagName.ToLower();

            switch (tag)
            {
                case "input":
                    result = element.GetAttribute("value");
                    break;
                case "select":
                    result = new SelectElement(element).SelectedOption.Text;
                    break;
                default:
                    result = element.Text;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not this element is displayed.  
        /// This method  suppresses any WebDriver exceptions  
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsDisplayedSafe(this IWebElement element)
        {
            try
            {
                if (element.Displayed)
                {
                    return true;
                }
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}