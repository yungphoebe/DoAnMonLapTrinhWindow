using System.Text;
using Microsoft.Extensions.Configuration;
using ToDoList.GUI.Tests;
using ToDoList.GUI.Resources;

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
            
            // Đọc cấu hình từ appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            
            bool enableTest = configuration.GetValue<bool>("AppSettings:EnableDatabaseTestOnStartup", true);
            bool useMessageBox = configuration.GetValue<bool>("AppSettings:UseMessageBoxForTest", true);
            
            // Test kết nối database nếu được bật
            if (enableTest)
            {
                if (useMessageBox)
                {
                    // Dùng Custom Form với font tiếng Việt đẹp
                    using (var testForm = new DatabaseTestForm())
                    {
                        testForm.ShowDialog();
                    }
                }
                else
                {
                    // Dùng Console - Cần thiết lập UTF-8
                    AllocConsole();
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.InputEncoding = Encoding.UTF8;
                    SimpleConnectionTest.TestConnection();
                    Console.WriteLine("\n\nNhấn phím bất kỳ để mở ứng dụng...");
                    Console.ReadKey();
                    FreeConsole();
                }
            }
            
            // Chạy Form1 như ban đầu
            Application.Run(new Form1());
        }
        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
    }
}
