using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Xml.Linq;
using System; // ��� ��������� using, ������� �������� �����������, ��� �� ����� ������������ ������������ ���� System, ������� �������� �������� ������ � ������� � �#
using System.Text; // ����� using ��������� ����������� System.Text, ������� �������� ����� StringBuilder. ���� ����� ����� ��� ��������� ������
using OpenQA.Selenium.DevTools.V108.ServiceWorker;
using System.Net.Mail;

namespace TestProject1
{
    class LoginGenerator // ���������� ����� LoginGenerator
    {
        private static readonly Random random = new Random(); // ������� ����������� ���� random, ������� ������������ ��� ��������� ��������� ��������
        // pravte ��������, ��� ���� �� ����� ���� ������������ �� ��������� ������ LoginGenerator
        // readonly - ��������, ��� ��� �������� ����� ���� ������������ ���� ��� � �� ����� ���� �������� ����� �����

        public static string GenerateLogin(int length) // ����� GenerateLogin ��� ��������� ������, ��������� �������� length, ������� ���������� ����� ������
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";// ������� ��������� chars, ������� �������� ��� ��������� �����, ���������� ��� ������ ��� ���������
            var builder = new StringBuilder(length); // ������� ��������� ������ StringBuilder, ������� ���������� ��� ���������� ������ ������
            // �������� �������� length � �����������, ������� ��������� ��������� ������� StringBuilder. ��� ��������� ������� ��������������� ����������� ���-�� ������ ��� ���������� ������
            for (int i = 0; i < length; i++) // ������� ���� for, ������� ����� ����������� length ���
            {
                builder.Append(chars[random.Next(chars.Length)]); // ���������� ���������� ����� ������. random.Next(chars.Length) ���������� ��������� ����� �� 0 �� ����� ������ chars
                // ��� ����� ����� ������������ ��� ��������� ����� �� ������ chars
            }
            return builder.ToString(); // ����� ����, ��� ���� ��������, ���������� ������ ������, ������� �� ��������� � ������� StringBuilder
        }
    }
        
    class Program
    {
        public static string GenerateRandomEmail()
        {
            var random = new Random();

            // ���������� ��������� ��� �� ������� "user_{��������� �����}@mail.ru"
            string username = "user_" + random.Next(1000, 9999);
            string domain = "mail.ru";

            // �������� email �� ����� ������������ � ������
            return username + "@" + domain;
        }
    }

    public class SnilsGenerator // ����������� ������ SnilsGenerator, ������� ����� ������������ �����
    {
        private Random random; //��������� ��������� ���������� "random" ���� Random, ������� ����� �������������� ��� ��������� ��������� �����

        public SnilsGenerator() // ����������� ������ SnilsGenerator, ������� �������������� ���������� "random" ��� �������� ���������� ������
        {
            random = new Random();
        }

        public string GenerateSnils()
        {
            int[] snilsArray = new int[9]; // ��������� ������ snilsArray �� 9 ����� �����, ������� ����� �������������� ��� �������� ������ 9 ���� �����
            for (int i = 0; i < 9; i++) // ���� for, ������� �������� �� ������� �������� ������� snilsArray
            {
                snilsArray[i] = random.Next(0, 9); // ��������� ���������� ����� �� 0 �� 9 � ��� ���������� �������� i ������� snilsArray
            }

            int sum = snilsArray[8] * 1 +  // ����������� ����������� ����� �����. �������� ������ ����� � ������� snilsArray �� �������������� ���
                      snilsArray[7] * 2 +  // (1,2,3 � �.�.) ����� ��������� ��� ��� ������������
                      snilsArray[6] * 3 +
                      snilsArray[5] * 4 +
                      snilsArray[4] * 5 +
                      snilsArray[3] * 6 +
                      snilsArray[2] * 7 +
                      snilsArray[1] * 8 +
                      snilsArray[0] * 9;
            int checkDigit = sum % 101; // ��������� ������� �� ������� ����� �� 101. ������� ����� ����������� ������ �����
            if (checkDigit == 100) // ���� ����������� ����� ����� 100, �� ����������� ����� ����� ����� 0
            {
                checkDigit = 0;
            }
            // ��������������� �������� ����� � ������� XXX-XXX-XXX YY. ����������� ����� ������������ ���������� 0, ���� ��� ������ 10
            return $"{snilsArray[0]}{snilsArray[1]}{snilsArray[2]}-{snilsArray[3]}{snilsArray[4]}{snilsArray[5]}-{snilsArray[6]}{snilsArray[7]}{snilsArray[8]} {checkDigit:D2}";
        }
    }

    public class Tests
    {
        private IWebDriver driver; // ��������� ���������� driver

        [SetUp] // ��, ��� ���������� �� ������
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://eskso2.sapfir.corp:17155/login");
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);

        }

        [Test]
        public void ValidLogPass() 
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("admin");
            Thread.Sleep(2000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(2000);
            Assert.AreEqual("http://eskso2.sapfir.corp:17155/administration", driver.Url);

        }

        [Test]
        public void InvalidPassword() 
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("qwerty");
            Thread.Sleep(2000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("password" + Keys.Enter);
            Thread.Sleep(2000);
            IWebElement errorMessage = driver.FindElement(By.ClassName("dx-toast-message"));
            Assert.IsTrue(errorMessage.Text.Contains("������������ ����� ��� ������"));
        }

        [Test]
        public void BlockedUser() 
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("adminBLOCKED");
            Thread.Sleep(2000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(2000);
            IWebElement errorMessage = driver.FindElement(By.ClassName("dx-toast-message"));
            Assert.IsTrue(errorMessage.Text.Contains("������������ ����� ��� ������"));
        }
        [Test]
        public void CreateUser() 
        {

            IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("admin");
            Thread.Sleep(2000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(2000);
            IWebElement createUser = driver.FindElement(By.XPath("//div[@test-id=\"add-user-button\"]"));
                createUser.Click();
            IWebElement inputLogin = driver.FindElement(By.XPath("//input[@test-id=\"login\"]"));
            string generateLogin = LoginGenerator.GenerateLogin(8); // ���������� ����� ������ 8 ��������
            inputLogin.SendKeys(generateLogin);// �������� ����� SendKeys � �������� ��� ��������������� �����, ��������� ����� GenerateLogin ������ LoginGenerator

            Program program = new Program();
            string email = Program.GenerateRandomEmail();
            IWebElement inputEmail = driver.FindElement(By.XPath("//input[@test-id=\"e-mail\"]"));
            inputEmail.SendKeys(email);

            IWebElement inputSurname = driver.FindElement(By.XPath("//input[@test-id=\"surname\"]"));
            inputSurname.SendKeys("������");
            Thread.Sleep(2000);
            IWebElement inputFirstName = driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]"));
            inputFirstName.SendKeys("�������");
            Thread.Sleep(2000);
            IWebElement inputMiddleName = driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]"));
            inputMiddleName.SendKeys("����");
            Thread.Sleep(2000);
            IWebElement inputPassword = driver.FindElement(By.XPath("//input[@test-id=\"password\"]"));
            inputPassword.SendKeys("asdf1234");
            IWebElement confirmPassword = driver.FindElement(By.XPath("//input[@test-id=\"confirmPassword\"]"));
            confirmPassword.SendKeys("asdf1234");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[@test-id=\"operatorPortal\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[@test-id=\"confirm-button\"]")).Click();





        }

        private string GenerateRandomEmail()
        {
            throw new NotImplementedException();
        }

        [Test] // ��� ����
        public void CreateCallCentrAppeal()
        {
            SnilsGenerator snilsGenerator = new SnilsGenerator(); // ������� ����� ��������� ������ 'SnilsGenerator' � ���������� ����� �����
            string snils = snilsGenerator.GenerateSnils(); // ��������� ��������������� ����� � ���������� snils

            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("admin");
            Thread.Sleep(2000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(2000);
            IWebElement callCenter = driver.FindElement(By.PartialLinkText("����-�����"));
                callCenter.Click();
            IWebElement callClick = driver.FindElement(By.ClassName("dx-button-text"));
                callClick.Click();
            IWebElement inputCardNum = driver.FindElement(By.ClassName("dx-texteditor-input"));
                inputCardNum.SendKeys(snils + Keys.Enter); // ���������� snils � �������� ��������� ������ SendKeys � �������� ����� GenerateSnils
            Thread.Sleep(2000);
            IWebElement continueButton = driver.FindElement(By.XPath("//div[@test-id=\"select-button\"]"));
                continueButton.Click();
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
            IWebElement characterDropDownList = driver.FindElement(By.XPath("//div[@test-id='tone-select']"));
            characterDropDownList.Click();
            IWebElement characterClick = driver.FindElement(By.XPath("//div[@class='dx-item-content dx-list-item-content' and text()='����������']"));
            characterClick.Click();
            IWebElement inputQuery = driver.FindElement(By.XPath("//textarea[@test-id='request-query']"));
            inputQuery.Click();
            inputQuery.SendKeys("��� � ��� ��� ����������?!");

            IWebElement sendButton = driver.FindElement(By.XPath("//div[@test-id='send-button']"));
            sendButton.Click();

            
        }

        [Test]
        public void Cabinet()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("admin");
            Thread.Sleep(2000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(2000);
            IWebElement cabinetClick = driver.FindElement(By.XPath("//a[@test-id='menuitem-requests-cabinet']"));
            cabinetClick.Click();
            Assert.AreEqual("http://eskso2.sapfir.corp:17155/requests/cabinet", driver.Url);
        }


        [TearDown] // ���������� ����� ������ 
        public void TearDown()
        {
            Thread.Sleep(3000);

            driver.Quit();
        }

    }
}