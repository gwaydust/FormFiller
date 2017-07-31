using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SelFormFiller;

private bool acceptNextAlert = true;
public void TheFormTest(String firstname, String lastName, String gender)
{    
    driver.Navigate().GoToUrl("file:///c:/temp/test.html");
    driver.FindElement(By.Name("firstname")).Clear();
    driver.FindElement(By.Name("firstname")).SendKeys(firstname);
    driver.FindElement(By.Name("lastname")).Clear();
    driver.FindElement(By.Name("lastname")).SendKeys(lastName);
    driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();
    String str = CloseAlertAndGetItsText();
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