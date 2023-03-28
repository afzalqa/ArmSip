using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Xml.Linq;
using System; // это директива using, которая сообщаем компилятору, что мы хотим использовать пространство имен System, которое содержит основные классы и функции в С#
using System.Text; // через using добавляем пространсво System.Text, который содержит класс StringBuilder. Этот класс нужен для генерации логина
using OpenQA.Selenium.DevTools.V108.ServiceWorker;
using System.Net.Mail;

namespace TestProject1
{
    class LoginGenerator // определяем класс LoginGenerator
    {
        private static readonly Random random = new Random(); // создаем статическое поле random, которое используется для генерации случайных значений
        // pravte означает, что поле не может быть использовано за пределами класса LoginGenerator
        // readonly - означает, что его значение может быть использовано один раз и не может быть изменено после этого

        public static string GenerateLogin(int length) // метод GenerateLogin для генерации логина, принимает параметр length, который определяет длину логина
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";// создаем константу chars, который содержит все латинские буквы, используем эту строку для генерации
            var builder = new StringBuilder(length); // создаем экземпляр класса StringBuilder, который используем для построения строки логина
            // передаем параметр length в конструктор, который указывает начальную емкость StringBuilder. Это позволяет заранее зарезервировать достаточное кол-во памяти для построения строки
            for (int i = 0; i < length; i++) // создаем цикл for, который будет выполняться length раз
            {
                builder.Append(chars[random.Next(chars.Length)]); // генерируем случайнную букву логина. random.Next(chars.Length) возвращает случайное число от 0 до длины строки chars
                // это число затем используется для получения буквы их строки chars
            }
            return builder.ToString(); // после того, как цикл завершен, возвращаем строку логина, которую мы построили с помощью StringBuilder
        }
    }
        
    class Program
    {
        public static string GenerateRandomEmail()
        {
            var random = new Random();

            // генерируем случайное имя по шаблону "user_{случайное число}@mail.ru"
            string username = "user_" + random.Next(1000, 9999);
            string domain = "mail.ru";

            // собираем email из имени пользователя и домена
            return username + "@" + domain;
        }
    }

    public class SnilsGenerator // определение класса SnilsGenerator, который будет генерировать СНИЛС
    {
        private Random random; //объявляем приватную переменную "random" типа Random, которая будет использоваться для генерации случайных чисел

        public SnilsGenerator() // конструктор класса SnilsGenerator, который инициализирует переменную "random" при создании экземпляра класса
        {
            random = new Random();
        }

        public string GenerateSnils()
        {
            int[] snilsArray = new int[9]; // объявляем массив snilsArray из 9 целых чисел, который будет использоваться для хранения первых 9 цифр СНИЛС
            for (int i = 0; i < 9; i++) // цикл for, который проходит по каждому элементу массива snilsArray
            {
                snilsArray[i] = random.Next(0, 9); // генерация случайного числа от 0 до 9 и его присвоение элементу i массива snilsArray
            }

            int sum = snilsArray[8] * 1 +  // расчитываем контрольное число СНИЛС. Умножаем каждую цифру в массиве snilsArray на соответсвующий вес
                      snilsArray[7] * 2 +  // (1,2,3 и т.д.) затем суммируем все эти произведения
                      snilsArray[6] * 3 +
                      snilsArray[5] * 4 +
                      snilsArray[4] * 5 +
                      snilsArray[3] * 6 +
                      snilsArray[2] * 7 +
                      snilsArray[1] * 8 +
                      snilsArray[0] * 9;
            int checkDigit = sum % 101; // вычисляем остаток от деления суммы на 101. Остаток будет контрольным числом СНИЛС
            if (checkDigit == 100) // если контрольное число равно 100, то контрольное число будет равно 0
            {
                checkDigit = 0;
            }
            // форматированное значение СНИЛС в формате XXX-XXX-XXX YY. Контрольное число представлено лидирующим 0, если оно меньше 10
            return $"{snilsArray[0]}{snilsArray[1]}{snilsArray[2]}-{snilsArray[3]}{snilsArray[4]}{snilsArray[5]}-{snilsArray[6]}{snilsArray[7]}{snilsArray[8]} {checkDigit:D2}";
        }
    }

    public class Tests
    {
        private IWebDriver driver; // объявляем переменную driver

        [SetUp] // то, что происходит до тестов
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
            Assert.IsTrue(errorMessage.Text.Contains("Некорректный логин или пароль"));
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
            Assert.IsTrue(errorMessage.Text.Contains("Некорректный логин или пароль"));
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
            string generateLogin = LoginGenerator.GenerateLogin(8); // генерируем логин длиной 8 символов
            inputLogin.SendKeys(generateLogin);// вызываем метод SendKeys и передаем ему сгенерированный логин, исплоьзуя метод GenerateLogin класса LoginGenerator

            Program program = new Program();
            string email = Program.GenerateRandomEmail();
            IWebElement inputEmail = driver.FindElement(By.XPath("//input[@test-id=\"e-mail\"]"));
            inputEmail.SendKeys(email);

            IWebElement inputSurname = driver.FindElement(By.XPath("//input[@test-id=\"surname\"]"));
            inputSurname.SendKeys("Хусаин");
            Thread.Sleep(2000);
            IWebElement inputFirstName = driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]"));
            inputFirstName.SendKeys("Джоррдж");
            Thread.Sleep(2000);
            IWebElement inputMiddleName = driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]"));
            inputMiddleName.SendKeys("Ирек");
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

        [Test] // сам тест
        public void CreateCallCentrAppeal()
        {
            SnilsGenerator snilsGenerator = new SnilsGenerator(); // создаем новый экземпляр класса 'SnilsGenerator' и генерируем новый СНИЛС
            string snils = snilsGenerator.GenerateSnils(); // сохраняем сгенерированный СНИЛС в переменную snils

            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("admin");
            Thread.Sleep(2000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(2000);
            IWebElement callCenter = driver.FindElement(By.PartialLinkText("Колл-центр"));
                callCenter.Click();
            IWebElement callClick = driver.FindElement(By.ClassName("dx-button-text"));
                callClick.Click();
            IWebElement inputCardNum = driver.FindElement(By.ClassName("dx-texteditor-input"));
                inputCardNum.SendKeys(snils + Keys.Enter); // используем snils в качестве аргумента метода SendKeys и вызываем метод GenerateSnils
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


        [TearDown] // вызывается после тестов 
        public void TearDown()
        {
            Thread.Sleep(3000);

            driver.Quit();
        }

    }
}