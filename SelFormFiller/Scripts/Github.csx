using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SelFormFiller;

[Display(Name = "GithubLogin", Description = "login to Github")]
public void GithubLogin(string email, string password)
{
    driver.Navigate().GoToUrl("https://github.com");
    driver.FindElement(By.LinkText("Sign in")).Click();
    driver.FindElement(By.Id("login_field")).Clear();
    driver.FindElement(By.Id("login_field")).SendKeys(email);
    driver.FindElement(By.Id("password")).Clear();
    driver.FindElement(By.Id("password")).SendKeys(password);
    driver.FindElement(By.Name("commit")).Click();
}