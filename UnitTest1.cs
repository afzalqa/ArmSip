using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
            driver.FindElement(By.PartialLinkText(" ����� ")).Click();
            /*var clickCallcenter = driver.FindElement(callcenterButton);
            clickCallcenter.Click();*/
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