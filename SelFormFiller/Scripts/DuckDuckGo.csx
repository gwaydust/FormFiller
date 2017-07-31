using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SelFormFiller;

[Display(Name = "DuckDuckGoSearch", Description = "Duck Duck go search engine")]
public void DuckDuckGoSearch(string searchTerm1, string searchTerm2)
{
    driver.Navigate().GoToUrl("https://duckduckgo.com/");
    driver.FindElement(By.Id("search_form_input_homepage")).Clear();
    driver.FindElement(By.Id("search_form_input_homepage")).SendKeys(searchTerm1);
    driver.FindElement(By.Id("search_button_homepage")).Click();
}

public int helper1()
{
    Console.WriteLine("test that it doesn't get helper1");
    return 1;
}

private void helper2()
{
    Console.WriteLine("test that it doesn't get helper1");
   
}
