using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace SelFormFiller
{
    public abstract class WebAutomationBase : IDisposable
    {
        protected static IWebDriver driver = null;

        public WebAutomationBase(string url)
        {
            if (driver != null)
            {
                return;
            }

            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["browserDir"]))
            {
                //add browser exe to search PATH
                string pathvar = System.Environment.GetEnvironmentVariable("PATH");
                var value = pathvar + ConfigurationManager.AppSettings["browserDir"];
                var target = EnvironmentVariableTarget.Process;
                System.Environment.SetEnvironmentVariable("PATH", value, target);
            }

            String browserType = ConfigurationManager.AppSettings["browserType"].ToLower();
            if (browserType == "chrome" || browserType == "chromium")
            {
                SetChromeDriver();
            }
            else if (browserType == "firefox")
            {
                SetFirefoxDriver();
            }
            else
            { //default to IE
                SetIEDriver();
            }
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 6);                        
        }

        private void SetChromeDriver()
        {
            KillProc("chromedriver");
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            driver = new ChromeDriver(service);
        }

        private void SetFirefoxDriver()
        {
            KillProc("geckodriver");
            var service = FirefoxDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            driver = new FirefoxDriver(service);
        }

        private void SetIEDriver()
        {
            KillProc("IEServerDriver");
            var service = InternetExplorerDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            driver = new InternetExplorerDriver(service);
        }

        private void KillProc(String procname)
        {
            foreach (var process in System.Diagnostics.Process.GetProcesses())
            {
                if (process.ProcessName.Contains(procname))
                {
                    process.Kill();
                }
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
