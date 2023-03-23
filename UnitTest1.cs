using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Xml.Linq;
using System;

namespace TestProject1
{
    public class Tests
    {
        private IWebDriver driver; // объявляем переменную driver
        //private readonly By callcenterButton = By.PartialLinkText(" Колл-центр");

        [SetUp] // то, что происходит до тестов
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://eskso2.sapfir.corp:17155/login");
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);

        }

        [Test] // сам тест
        public void Test1()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
                login.SendKeys("admin");
            Thread.Sleep(2000);
            IWebElement password = driver.FindElement(By.Name("password"));
                password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(2000);
            IWebElement callCenter = driver.FindElement(By.PartialLinkText("Колл-центр"));
                callCenter.SendKeys("asdf1234" + Keys.Enter);
            IWebElement callClick = driver.FindElement(By.ClassName("dx-button-text"));
                callClick.Click();
            IWebElement inputCardNum = driver.FindElement(By.ClassName("dx-texteditor-input"));
                inputCardNum.SendKeys("51231917345" + Keys.Enter);
            Thread.Sleep(2000);
            IWebElement continueButton = driver.FindElement(By.XPath("//div[@test-id=\"select-button\"]"));
                continueButton.Click();
            driver.FindElement(By.XPath("//input[@test-id=\"surname\"]")).SendKeys("Иванов");
            driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]")).SendKeys("Иван");
            driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]")).SendKeys("Иванович");
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
            IWebElement characterDropDownList = driver.FindElement(By.XPath("//div[@test-id='tone-select']"));
            characterDropDownList.Click();
            IWebElement characterClick = driver.FindElement(By.XPath("//div[@class='dx-item-content dx-list-item-content' and text()='Негативный']"));
            characterClick.Click();
            IWebElement inputQuery = driver.FindElement(By.XPath("//textarea[@test-id='request-query']"));
            inputQuery.Click();
            inputQuery.SendKeys("Что у вас тут происходит?!");

            IWebElement sendButton = driver.FindElement(By.XPath("//div[@test-id='send-button']"));
            sendButton.Click();

            Assert.IsTrue(driver.FindElement(By.Id("message")).Text.Equals("LambdaTest rules"), "The expected message was not displayed.");
        }

        [TearDown] // вызывается после тестов 
        public void TearDown()
        {
            Thread.Sleep(3000);

            driver.Quit();
        }

    }
}