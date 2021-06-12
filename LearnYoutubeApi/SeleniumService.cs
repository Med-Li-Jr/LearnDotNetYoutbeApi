using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnYoutubeApi
{
    class SeleniumService
    {

        public static IWebDriver Driver;
        public static ChromeOptions Options = new ChromeOptions() { };
        public static string PathDriverChrome = @"D:\DocStream";

        public static void OpenAnYoutubeVideo(YoutubeVideo Video)
        {
            if (Driver == null)
            {
                Options.DebuggerAddress = "localhost:9014";
                Driver = new ChromeDriver(PathDriverChrome, Options);
            }
            //Driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
            Driver.Navigate().GoToUrl("https://www.youtube.com/watch?v=" + Video.Id);
        }

        public static void CloseWindowSelenium()
        {
            Driver.Close();
            //Driver.Quit();
        }
    }
}
