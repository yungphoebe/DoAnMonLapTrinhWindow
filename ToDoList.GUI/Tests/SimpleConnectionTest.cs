using System;
using System.Data.SqlClient;
using System.Text;

namespace ToDoList.GUI.Tests
{
    public static class SimpleConnectionTest
    {
        public static void TestConnection()
        {
            // Thi?t l?p UTF-8 encoding cho console
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            
            string connectionString = "Server=DESKTOP-LN5QDF6\\SQLEXPRESS;Database=ToDoListApp;Integrated Security=True;TrustServerCertificate=True";
            
            Console.WriteLine("================================================");
            Console.WriteLine("      KI?M TRA K?T N?I DATABASE");
            Console.WriteLine("================================================\n");
            Console.WriteLine($"Server: DESKTOP-LN5QDF6\\SQLEXPRESS");
            Console.WriteLine($"Database: ToDoListApp");
            Console.WriteLine($"Authentication: Windows Authentication\n");
            Console.WriteLine("?ang k?t n?i...\n");
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    Console.WriteLine("? K?T N?I THÀNH CÔNG!\n");
                    Console.WriteLine($"Server Version: {connection.ServerVersion}");
                    Console.WriteLine($"Database: {connection.Database}");
                    Console.WriteLine($"State: {connection.State}\n");
                    
                    // Test query
                    Console.WriteLine("?ang ki?m tra các b?ng...\n");
                    
                    string[] tables = { "Users", "Tasks", "Projects", "Tags", "FocusSessions", 
                                       "Reminders", "ActivityLog", "UserSettings", "ProjectMembers" };
                    
                    foreach (string table in tables)
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM {table}", connection))
                            {
                                int count = (int)command.ExecuteScalar();
                                Console.WriteLine($"? {table,-20} : {count,5} records");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"? {table,-20} : {ex.Message}");
                        }
                    }
                    
                    Console.WriteLine("\n================================================");
                    Console.WriteLine("       ? T?T C? TEST HOÀN T?T!");
                    Console.WriteLine("================================================");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("\n? L?I K?T N?I SQL SERVER!\n");
                Console.WriteLine($"Error Code: {ex.Number}");
                Console.WriteLine($"Message: {ex.Message}\n");
                
                Console.WriteLine("?? H??NG D?N KH?C PH?C:\n");
                
                switch (ex.Number)
                {
                    case 2:
                    case 53:
                        Console.WriteLine("- Không tìm th?y server ho?c server không th? truy c?p");
                        Console.WriteLine("- Ki?m tra tên server: DESKTOP-LN5QDF6\\SQLEXPRESS");
                        Console.WriteLine("- ??m b?o SQL Server ?ã ???c kh?i ??ng");
                        Console.WriteLine("- Ki?m tra SQL Server Configuration Manager");
                        break;
                    case 4060:
                        Console.WriteLine("- Database 'ToDoListApp' không t?n t?i");
                        Console.WriteLine("- T?o database m?i ho?c ki?m tra tên database");
                        break;
                    case 18456:
                        Console.WriteLine("- L?i xác th?c");
                        Console.WriteLine("- Ki?m tra Windows Authentication có ???c b?t không");
                        break;
                    default:
                        Console.WriteLine($"- L?i không xác ??nh: {ex.Message}");
                        break;
                }
                
                Console.WriteLine("\n================================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n? L?I: {ex.Message}");
            }
        }
    }
}
