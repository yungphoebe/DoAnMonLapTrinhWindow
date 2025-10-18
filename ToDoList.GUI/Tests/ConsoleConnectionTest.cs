using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.GUI.Data;

namespace ToDoList.GUI.Tests
{
    public class ConsoleConnectionTest
    {
        public static async Task RunTestAsync()
        {
            // Thi?t l?p UTF-8 encoding cho console
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            
            Console.WriteLine("===========================================");
            Console.WriteLine("KI?M TRA K?T N?I DATABASE");
            Console.WriteLine("===========================================\n");
            
            try
            {
                Console.WriteLine("?? ?ang k?t n?i ??n database...");
                Console.WriteLine($"Server: DESKTOP-LN5QDF6\\SQLEXPRESS");
                Console.WriteLine($"Database: ToDoListApp\n");
                
                using var context = DbContextFactory.CreateDbContext();
                
                // Test 1: Ki?m tra k?t n?i
                Console.Write("Test 1: Ki?m tra k?t n?i... ");
                var canConnect = await context.Database.CanConnectAsync();
                
                if (canConnect)
                {
                    Console.WriteLine("? THÀNH CÔNG");
                }
                else
                {
                    Console.WriteLine("? TH?T B?I");
                    return;
                }
                
                // Test 2: ??m s? b?ng
                Console.Write("Test 2: ??c d? li?u Users... ");
                var userCount = await context.Users.CountAsync();
                Console.WriteLine($"? Tìm th?y {userCount} users");
                
                Console.Write("Test 3: ??c d? li?u Tasks... ");
                var taskCount = await context.Tasks.CountAsync();
                Console.WriteLine($"? Tìm th?y {taskCount} tasks");
                
                Console.Write("Test 4: ??c d? li?u Projects... ");
                var projectCount = await context.Projects.CountAsync();
                Console.WriteLine($"? Tìm th?y {projectCount} projects");
                
                Console.Write("Test 5: ??c d? li?u Tags... ");
                var tagCount = await context.Tags.CountAsync();
                Console.WriteLine($"? Tìm th?y {tagCount} tags");
                
                Console.WriteLine("\n===========================================");
                Console.WriteLine("? T?T C? TEST THÀNH CÔNG!");
                Console.WriteLine("===========================================");
                
                // Hi?n th? m?t s? d? li?u m?u n?u có
                if (userCount > 0)
                {
                    Console.WriteLine("\n?? Danh sách Users:");
                    var users = await context.Users.Take(5).ToListAsync();
                    foreach (var user in users)
                    {
                        Console.WriteLine($"  - {user.FullName} ({user.Email})");
                    }
                }
                
                if (projectCount > 0)
                {
                    Console.WriteLine("\n?? Danh sách Projects:");
                    var projects = await context.Projects.Take(5).ToListAsync();
                    foreach (var project in projects)
                    {
                        Console.WriteLine($"  - {project.ProjectName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n? L?I K?T N?I DATABASE!");
                Console.WriteLine("===========================================");
                Console.WriteLine($"L?i: {ex.Message}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"\nChi ti?t: {ex.InnerException.Message}");
                }
                
                Console.WriteLine("\n?? Ki?m tra:");
                Console.WriteLine("1. SQL Server ?ã ???c kh?i ??ng ch?a?");
                Console.WriteLine("2. Tên server có ?úng không? (DESKTOP-LN5QDF6\\SQLEXPRESS)");
                Console.WriteLine("3. Database 'ToDoListApp' ?ã t?n t?i ch?a?");
                Console.WriteLine("4. Windows Authentication có ho?t ??ng không?");
            }
            
            Console.WriteLine("\n\nNh?n phím b?t k? ?? ti?p t?c...");
            Console.ReadKey();
        }
    }
}
