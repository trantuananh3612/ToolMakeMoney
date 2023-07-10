using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace ToolUploadShirtsPlus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Khởi tạo ChromeDriver
            IWebDriver driver = new ChromeDriver();

            // Mở trang web Google
            driver.Navigate().GoToUrl("https://www.teepublic.com");

            //Chờ page load
            this.WaitPageReady(driver);

            //login 
            this.Login_Func(driver, "tta.dev.22@gmail.com", "Tta_02122001");

            //Chờ page load
            this.WaitPageReady(driver);

            //Upload áo
            this.UploadShirt(driver);


            // Đóng trình duyệt
            //driver.Quit();
        }

        private bool Login_Func(IWebDriver driver, string email, string password)
        {
            try
            {
                IWebElement showLogin = driver.FindElement(By.LinkText("Log In")); 
                showLogin.Click();

                // Tìm input email login
                IWebElement emailInput = driver.FindElement(By.Name("session[email]"));
                emailInput.SendKeys(email);

                // Tìm input password login
                IWebElement passwordInput = driver.FindElement(By.Name("session[password]"));
                passwordInput.SendKeys(password);

                IWebElement buttonLogin = driver.FindElement(By.Id("login"));
                buttonLogin.Submit();

                SaveLog("Đăng nhập thành công!");
                return true;
            }
            catch (Exception ex)
            {
                this.SaveLog(ex.Message);
                return false;
            }
        }

        private void SaveLog(string mess)
        {
            var index = Directory.GetCurrentDirectory().IndexOf("ToolUploadShirts");
            string server = Directory.GetCurrentDirectory().Substring(0, index) + "ToolUploadShirts";
            string logFilePath = $"{server}\\log.txt";
            try
            {
                // Kiểm tra nếu file log không tồn tại, tạo mới
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath).Dispose();
                }
            }
            finally
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now} - {mess}{Environment.NewLine}");
            }
        }


        private void WaitPageReady(IWebDriver driver)
        {
            try
            {
                // Chờ load trang
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var checkReady = wait.Until(x => ((IJavaScriptExecutor)x).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception ex )
            {
                this.SaveLog(ex.Message);
            }
           
        }


        private void UploadShirt(IWebDriver driver)
        {
            try
            {
                //Click nut Upload Art => mở ra form upload áo
                IWebElement uploadArt = driver.FindElement(By.LinkText("Upload Art"));
                uploadArt.Click();

                //Upload single
                IWebElement uploadSingle = driver.FindElement(By.ClassName("m-uploader-funnel__option jsUploaderFunnelOption"));
                uploadSingle.Click();

                //Upload multi
                //IWebElement uploadMulti = driver.FindElement(By.ClassName("m-uploader-funnel__option jsUploaderFunnelOption jsUploaderFunnelMulti"));
                //uploadArt.Click();
            }
            catch (Exception ex)
            {
                this.SaveLog(ex.Message);
            }
        }
    }
}
