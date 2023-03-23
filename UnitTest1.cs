using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Xml.Linq;
using System;

namespace TestProject1
{
    public class Tests
    {
        private IWebDriver driver; // ��������� ���������� driver
        //private readonly By callcenterButton = By.PartialLinkText(" ����-�����");

        [SetUp] // ��, ��� ���������� �� ������
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://eskso2.sapfir.corp:17155/login");
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);

        }

        [Test] // ��� ����
        public void Test1()
        {
            driver.FindElement(By.Name("login")).SendKeys("admin");
            Thread.Sleep(2000);
            driver.FindElement(By.Name("password")).SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(2000);
            driver.FindElement(By.PartialLinkText("����-�����")).SendKeys("asdf1234" + Keys.Enter);
            driver.FindElement(By.ClassName("dx-button-text")).Click();
            driver.FindElement(By.ClassName("dx-texteditor-input")).SendKeys("51231917345" + Keys.Enter);
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[@test-id=\"select-button\"]")).Click();
            driver.FindElement(By.XPath("//input[@test-id=\"surname\"]")).SendKeys("������");
            driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]")).SendKeys("����");
            driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]")).SendKeys("��������");
            /*WebElement cardNum = (WebElement)driver.FindElement(By.XPath("//input[@test-id=\"card-number\"]"));
            cardNum.Clear();
            var myString = "9620812001307270";
            for (int i = 0; i < myString.Length; i++)
            {
                cardNum.SendKeys(myString[i].ToString());
            };
            cardNum.SendKeys("9620812001307270");*/
            driver.FindElement(By.XPath("//div[@class=\"dx-dropdowneditor-icon\"]")).Click();
            driver.FindElement(By.XPath("//div[@class=\"dx-item-content dx-list-item-content\"]")).Click();
            WebElement character = (WebElement)driver.FindElement(By.XPath("//div[@test-id=\"tone-select\"]"));
            character.Click();
            driver.FindElement(By.XPath("//div[@class=\"dx-item-content dx-list-item-content\"]")).Click();
            IWebElement element = driver.FindElement(By.CssSelector("[style*='line-height: 20px;'][style*='font-size: 15.4px;']"));
            Thread.Sleep(2000);


            Assert.IsTrue(driver.FindElement(By.Id("message")).Text.Equals("LambdaTest rules"), "The expected message was not displayed.");
        }

        [TearDown] // ���������� ����� ������ 
        public void TearDown()
        {
            Thread.Sleep(3000);

            driver.Quit();
        }

    }
}