using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace ToDoList.GUI.Tests
{
    public static class MessageBoxConnectionTest
    {
        public static void TestConnection()
        {
            string connectionString = "Server=DESKTOP-LN5QDF6\\SQLEXPRESS;Database=ToDoListApp;Integrated Security=True;TrustServerCertificate=True";
            
            StringBuilder result = new StringBuilder();
            result.AppendLine("================================================");
            result.AppendLine("      KI?M TRA K?T N?I DATABASE");
            result.AppendLine("================================================");
            result.AppendLine();
            result.AppendLine($"Server: DESKTOP-LN5QDF6\\SQLEXPRESS");
            result.AppendLine($"Database: ToDoListApp");
            result.AppendLine($"Authentication: Windows Authentication");
            result.AppendLine();
            result.AppendLine("?ang k?t n?i...");
            result.AppendLine();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    result.AppendLine("? K?T N?I TH�NH C�NG!");
                    result.AppendLine();
                    result.AppendLine($"Server Version: {connection.ServerVersion}");
                    result.AppendLine($"Database: {connection.Database}");
                    result.AppendLine($"State: {connection.State}");
                    result.AppendLine();
                    result.AppendLine("?ang ki?m tra c�c b?ng...");
                    result.AppendLine();
                    
                    string[] tables = { "Users", "Tasks", "Projects", "Tags", "FocusSessions", 
                                       "Reminders", "ActivityLog", "UserSettings", "ProjectMembers" };
                    
                    int totalRecords = 0;
                    foreach (string table in tables)
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM {table}", connection))
                            {
                                int count = (int)command.ExecuteScalar();
                                result.AppendLine($"? {table,-20} : {count,5} b?n ghi");
                                totalRecords += count;
                            }
                        }
                        catch (Exception ex)
                        {
                            result.AppendLine($"? {table,-20} : L?i - {ex.Message}");
                        }
                    }
                    
                    result.AppendLine();
                    result.AppendLine("================================================");
                    result.AppendLine($"   ? T?NG C?NG: {totalRecords} B?N GHI");
                    result.AppendLine("================================================");
                    
                    MessageBox.Show(result.ToString(), "Ki?m Tra K?t N?i Database", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                result.Clear();
                result.AppendLine("? L?I K?T N?I SQL SERVER!");
                result.AppendLine();
                result.AppendLine($"M� l?i: {ex.Number}");
                result.AppendLine($"Th�ng b�o: {ex.Message}");
                result.AppendLine();
                result.AppendLine("?? H??NG D?N KH?C PH?C:");
                result.AppendLine();
                
                switch (ex.Number)
                {
                    case 2:
                    case 53:
                        result.AppendLine("? Kh�ng t�m th?y server ho?c kh�ng th? truy c?p");
                        result.AppendLine();
                        result.AppendLine("Vui l�ng ki?m tra:");
                        result.AppendLine("? T�n server: DESKTOP-LN5QDF6\\SQLEXPRESS");
                        result.AppendLine("? SQL Server ?� ???c kh?i ??ng ch?a?");
                        result.AppendLine("? SQL Server Configuration Manager");
                        result.AppendLine("? SQL Server Browser service ?ang ch?y");
                        break;
                    case 4060:
                        result.AppendLine("? Database 'ToDoListApp' kh�ng t?n t?i");
                        result.AppendLine();
                        result.AppendLine("Gi?i ph�p:");
                        result.AppendLine("? T?o database m?i t�n 'ToDoListApp'");
                        result.AppendLine("? Ho?c ki?m tra l?i t�n database");
                        result.AppendLine("? Ch?y migration ?? t?o database");
                        break;
                    case 18456:
                        result.AppendLine("? L?i x�c th?c");
                        result.AppendLine();
                        result.AppendLine("Gi?i ph�p:");
                        result.AppendLine("? Ki?m tra Windows Authentication c� ???c b?t");
                        result.AppendLine("? T�i kho?n Windows c� quy?n truy c?p SQL Server");
                        break;
                    default:
                        result.AppendLine($"? L?i kh�ng x�c ??nh: {ex.Message}");
                        break;
                }
                
                MessageBox.Show(result.ToString(), "L?i K?t N?i Database", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"? L?I:\n\n{ex.Message}", "L?i", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
