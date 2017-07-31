using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Configuration;

namespace SelFormFiller
{
    class CustomCalendar
    {
        private string calID = "";
        DateTime targetDateTime = DateTime.Now;
        string prevMthPath, nextMthPath, yrMthPath, hourPath, minPath, dayPath, dtPickerPath;

        public CustomCalendar(string calendarID)
        {
            this.calID = calendarID;
            dtPickerPath = $"//DIV[@id='{calID}']//I[@class='icon-date']";
            prevMthPath = $"//div[@id='{calID}']//div[@class='rdtDays']//span[@class='icon-arrow-left']";
            nextMthPath = $"//div[@id='{calID}']//div[@class='rdtDays']//span[@class='icon-arrow-right']";
            yrMthPath = $"//div[@id='{calID}']//th[@class='rdtSwitch']";
                        
            hourPath = $".//*[@id='sdaExpiryDate']//input[contains(@class,'input-hour')]";
            minPath = $".//*[@id='sdaExpiryDate']//input[contains(@class,'input-minutes')]";
        }

        public void SelectDate(IWebDriver driver, string dateStr)
        {
            if (!DateTime.TryParse(dateStr,out targetDateTime)) {
                return;
            }
            driver.FindElement(By.XPath(dtPickerPath)).Click();
            dayPath = $"//div[@id='{calID}']//td[@data-value='{targetDateTime.Day.ToString()}']";

            var we = driver.FindElement(By.XPath(yrMthPath));
            var curYrMonth = int.Parse(DateTime.Parse(we.Text.Trim()).ToString("yyyyMM"));
            var targetYrMonth = (targetDateTime.Year * 100) + targetDateTime.Month;

            //click prev or next until date and time match
            while (targetYrMonth > curYrMonth)
            {
                driver.FindElement(By.XPath(nextMthPath)).Click();
                curYrMonth = int.Parse(DateTime.Parse(we.Text.Trim()).ToString("yyyyMM"));
            }
            while (targetYrMonth < curYrMonth)
            {
                driver.FindElement(By.XPath(prevMthPath)).Click();
                curYrMonth = int.Parse(DateTime.Parse(we.Text.Trim()).ToString("yyyyMM"));
            }
            
            driver.FindElement(By.XPath(hourPath)).Clear();
            driver.FindElement(By.XPath(hourPath)).SendKeys(targetDateTime.Hour.ToString());
            driver.FindElement(By.XPath(minPath)).Clear();
            driver.FindElement(By.XPath(minPath)).SendKeys(targetDateTime.Minute.ToString());
            driver.FindElement(By.XPath(dayPath)).Click();
        }
    }
}
