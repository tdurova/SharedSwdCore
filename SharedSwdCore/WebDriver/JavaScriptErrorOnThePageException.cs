using System;

namespace SharedSwdCore.WebDriver
{
    public class JavaScriptErrorOnThePageException : Exception
    {
        public JavaScriptErrorOnThePageException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
