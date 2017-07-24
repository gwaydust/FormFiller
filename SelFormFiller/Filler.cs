using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SelFormFiller
{
    public class Filler : IDisposable
    {
        private StringBuilder verificationErrors;
        private string baseURL;
        private static IWebDriver driver = null;
        private bool acceptNextAlert = true;

        public Filler(string url, IWebDriver drv = null)
        {
            if (driver == null)
            {
                var service = FirefoxDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                service.SuppressInitialDiagnosticInformation = true;
                //service.ConnectToRunningBrowser = true;
                driver = new FirefoxDriver(service);                
            } else
            {
                driver = drv;
            }
            baseURL = url;
            verificationErrors = new StringBuilder();
        }       

        public void TheFormTest(String firstname, String lastName, String gender)
        {
            driver.Navigate().GoToUrl("file:///c:/temp/test.html");
            driver.FindElement(By.Name("firstname")).Clear();
            driver.FindElement(By.Name("firstname")).SendKeys(firstname);
            driver.FindElement(By.Name("lastname")).Clear();
            driver.FindElement(By.Name("lastname")).SendKeys(lastName);
            //driver.FindElement(By.XPath($"//input[@value='{gender}']")).Click();
            driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();
            String str = CloseAlertAndGetItsText();
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void DuckDuckGoSearch(string searchTerm)
        {
            driver.Navigate().GoToUrl("https://duckduckgo.com/");
            driver.FindElement(By.Id("search_form_input_homepage")).Clear();
            driver.FindElement(By.Id("search_form_input_homepage")).SendKeys(searchTerm);
            driver.FindElement(By.Id("search_button_homepage")).Click();
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }

        public void Dispose()
        {
            try
            {
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                    driver.Dispose();
                }
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
    }
}
