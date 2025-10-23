using System.Text;
using Microsoft.Extensions.Configuration;
using ToDoList.GUI.Tests;
using ToDoList.GUI.Resources;
using ToDoList.GUI.Forms;
using ToDoList.GUI.Helpers;

namespace ToDoList.GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            // Initialize language system
            LanguageManager.Initialize();
            
            // Doc cau hinh tu appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            
            bool enableTest = configuration.GetValue<bool>("AppSettings:EnableDatabaseTestOnStartup", false); // Changed to false
            bool useMessageBox = configuration.GetValue<bool>("AppSettings:UseMessageBoxForTest", true);
            
            // Test ket noi database neu duoc bat
            if (enableTest)
            {
                if (useMessageBox)
                {
                    // Dung Custom Form voi font tieng Viet dep
                    using (var testForm = new DatabaseTestForm())
                    {
                        testForm.ShowDialog();
                    }
                }
                else
                {
                    // Dung Console - Can thiet lap UTF-8
                    AllocConsole();
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.InputEncoding = Encoding.UTF8;
                    SimpleConnectionTest.TestConnection();
                    Console.WriteLine("\n\nNhan phim bat ky de mo ung dung...");
                    Console.ReadKey();
                    FreeConsole();
                }
            }
            
            // Hien thi man hinh dang nhap truoc
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Neu dang nhap thanh cong, chay Form1
                    Application.Run(new Form1());
                }
                else
                {
                    // Neu huy dang nhap, thoat ung dung
                    return;
                }
            }
        }
        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
    }
}
