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
        
        public void TheCreateNameProfileTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/#/login?backUrl=%2F");
            driver.FindElement(By.Id("login-username")).Clear();
            driver.FindElement(By.Id("login-username")).SendKeys("tradingsupportanalyst1");
            driver.FindElement(By.Id("login-password")).Clear();
            driver.FindElement(By.Id("login-password")).SendKeys("password");
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            driver.FindElement(By.CssSelector("i.arrow.icon-chevron_right")).Click();
            driver.FindElement(By.LinkText("Name Management")).Click();
            driver.FindElement(By.Name("btnEdit")).Click();
            driver.FindElement(By.Name("btnAddNew")).Click();
            driver.FindElement(By.Name("enFullName")).Clear();
            driver.FindElement(By.Name("enFullName")).SendKeys("seltest1");
            driver.FindElement(By.Name("twFullName")).Clear();
            driver.FindElement(By.Name("twFullName")).SendKeys("seltest1");
            driver.FindElement(By.Name("cnFullName")).Clear();
            driver.FindElement(By.Name("cnFullName")).SendKeys("seltest1");
            driver.FindElement(By.Name("enShortName")).Clear();
            driver.FindElement(By.Name("enShortName")).SendKeys("seltest1");
            driver.FindElement(By.Name("twShortName")).Clear();
            driver.FindElement(By.Name("twShortName")).SendKeys("seltest1");
            driver.FindElement(By.Name("cnShortName")).Clear();
            driver.FindElement(By.Name("cnShortName")).SendKeys("seltest1");
            new SelectElement(driver.FindElement(By.Name("continentId"))).SelectByText("Africa");
            new SelectElement(driver.FindElement(By.Name("countryId"))).SelectByText("France");
            driver.FindElement(By.Name("eventLevel1Code")).Clear();
            driver.FindElement(By.Name("eventLevel1Code")).SendKeys("1234");
            driver.FindElement(By.XPath("//div[@id='sdaExpiryDate']/div/div[2]/div/table/tbody/tr[6]/td[2]")).Click();
            driver.FindElement(By.CssSelector("div.rdt.form-control > div.rdtPicker > div.rdtDays > table > tfoot > tr > td.rdtTimeToggle > input.form-control.input-hour")).Clear();
            driver.FindElement(By.CssSelector("div.rdt.form-control > div.rdtPicker > div.rdtDays > table > tfoot > tr > td.rdtTimeToggle > input.form-control.input-hour")).SendKeys("09");
            driver.FindElement(By.Name("btnCreate")).Click();
            driver.FindElement(By.Name("btnConfirm")).Click();
            driver.FindElement(By.Name("btnConfirm")).Click();
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
