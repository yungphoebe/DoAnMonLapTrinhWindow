using System;
using TodoListApp.DAL.Data;
using TodoListApp.DAL.Models;

namespace TodoListApp.DAL
{
    /// <summary>
    /// Console app ?? seed d? li?u m?u v�o database
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
                        Console.WriteLine("? K?t n?i database th�nh c�ng!\n");
                    }
                    else
                    {
                        Console.WriteLine("? Kh�ng th? k?t n?i database!");
                        Console.WriteLine("Vui l�ng ki?m tra connection string trong appsettings.json");
                        return;
                    }

                    var seeder = new DatabaseSeeder(context);

                    Console.WriteLine("Ch?n h�nh ??ng:");
                    Console.WriteLine("1. Seed d? li?u m?u");
                    Console.WriteLine("2. X�a t?t c? d? li?u");
                    Console.WriteLine("3. X�a v� seed l?i (Reset)");
                    Console.WriteLine("0. Tho�t");
                    Console.Write("\nL?a ch?n c?a b?n: ");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("\n?? B?t ??u seed d? li?u...\n");
                            seeder.SeedAll();
                            Console.WriteLine("\n? Ho�n t?t! D? li?u ?� ???c seed v�o database.");
                            break;

                        case "2":
                            Console.Write("\n??  B?n c� ch?c mu?n x�a T?T C? d? li?u? (yes/no): ");
                            var confirm = Console.ReadLine();
                            if (confirm?.ToLower() == "yes")
                            {
                                Console.WriteLine("\n???  ?ang x�a d? li?u...\n");
                                seeder.ClearAll();
                                Console.WriteLine("\n? ?� x�a t?t c? d? li?u.");
                            }
                            else
                            {
                                Console.WriteLine("H?y thao t�c x�a.");
                            }
                            break;

                        case "3":
                            Console.Write("\n??  B?n c� ch?c mu?n X�A v� SEED L?I t?t c? d? li?u? (yes/no): ");
                            var confirmReset = Console.ReadLine();
                            if (confirmReset?.ToLower() == "yes")
                            {
                                Console.WriteLine("\n???  ?ang x�a d? li?u c?...\n");
                                seeder.ClearAll();
                                Console.WriteLine("\n?? ?ang seed d? li?u m?i...\n");
                                seeder.SeedAll();
                                Console.WriteLine("\n? Ho�n t?t! Database ?� ???c reset v� seed l?i.");
                            }
                            else
                            {
                                Console.WriteLine("H?y thao t�c reset.");
                            }
                            break;

                        case "0":
                            Console.WriteLine("Tho�t ch??ng tr�nh.");
                            return;

                        default:
                            Console.WriteLine("L?a ch?n kh�ng h?p l?!");
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

            Console.WriteLine("\nNh?n ph�m b?t k? ?? tho�t...");
            Console.ReadKey();
        }
    }
}
