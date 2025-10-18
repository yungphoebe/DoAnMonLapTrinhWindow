using System;
using TodoListApp.DAL.Data;
using TodoListApp.DAL.Models;

namespace TodoListApp.DAL
{
    /// <summary>
    /// Console app ?? seed d? li?u m?u vào database
    /// </summary>
    class SeedProgram
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("========================================");
            Console.WriteLine("  ToDoList App - Database Seeder");
            Console.WriteLine("========================================\n");

            try
            {
                using (var context = new ToDoListContext())
                {
                    Console.WriteLine("?ang k?t n?i database...");
                    
                    // Test connection
                    if (context.Database.CanConnect())
                    {
                        Console.WriteLine("? K?t n?i database thành công!\n");
                    }
                    else
                    {
                        Console.WriteLine("? Không th? k?t n?i database!");
                        Console.WriteLine("Vui lòng ki?m tra connection string trong appsettings.json");
                        return;
                    }

                    var seeder = new DatabaseSeeder(context);

                    Console.WriteLine("Ch?n hành ??ng:");
                    Console.WriteLine("1. Seed d? li?u m?u");
                    Console.WriteLine("2. Xóa t?t c? d? li?u");
                    Console.WriteLine("3. Xóa và seed l?i (Reset)");
                    Console.WriteLine("0. Thoát");
                    Console.Write("\nL?a ch?n c?a b?n: ");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("\n?? B?t ??u seed d? li?u...\n");
                            seeder.SeedAll();
                            Console.WriteLine("\n? Hoàn t?t! D? li?u ?ã ???c seed vào database.");
                            break;

                        case "2":
                            Console.Write("\n??  B?n có ch?c mu?n xóa T?T C? d? li?u? (yes/no): ");
                            var confirm = Console.ReadLine();
                            if (confirm?.ToLower() == "yes")
                            {
                                Console.WriteLine("\n???  ?ang xóa d? li?u...\n");
                                seeder.ClearAll();
                                Console.WriteLine("\n? ?ã xóa t?t c? d? li?u.");
                            }
                            else
                            {
                                Console.WriteLine("H?y thao tác xóa.");
                            }
                            break;

                        case "3":
                            Console.Write("\n??  B?n có ch?c mu?n XÓA và SEED L?I t?t c? d? li?u? (yes/no): ");
                            var confirmReset = Console.ReadLine();
                            if (confirmReset?.ToLower() == "yes")
                            {
                                Console.WriteLine("\n???  ?ang xóa d? li?u c?...\n");
                                seeder.ClearAll();
                                Console.WriteLine("\n?? ?ang seed d? li?u m?i...\n");
                                seeder.SeedAll();
                                Console.WriteLine("\n? Hoàn t?t! Database ?ã ???c reset và seed l?i.");
                            }
                            else
                            {
                                Console.WriteLine("H?y thao tác reset.");
                            }
                            break;

                        case "0":
                            Console.WriteLine("Thoát ch??ng trình.");
                            return;

                        default:
                            Console.WriteLine("L?a ch?n không h?p l?!");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n? L?i: {ex.Message}");
                Console.WriteLine($"\nChi ti?t: {ex.InnerException?.Message}");
                Console.WriteLine($"\nStack trace: {ex.StackTrace}");
            }

            Console.WriteLine("\nNh?n phím b?t k? ?? thoát...");
            Console.ReadKey();
        }
    }
}
