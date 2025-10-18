using System;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Tests
{
    /// <summary>
    /// Test đơn giản để kiểm tra kết nối database
    /// </summary>
    public class SimpleDatabaseTest
    {
        public static void TestDatabaseConnection()
        {
            try
            {
                using (var context = new ToDoListContext())
                {
                    // Test connection by trying to access database
                    var canConnect = context.Database.CanConnect();
                    
                    if (canConnect)
                    {
                        MessageBox.Show("✅ Kết nối database thành công!", "Test Database", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("❌ Không thể kết nối database!", "Test Database", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi kết nối database:\n\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", 
                    "Test Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void TestCreateProject()
        {
            try
            {
                using (var context = new ToDoListContext())
                {
                    var project = new Project
                    {
                        UserId = 1,
                        ProjectName = "Test Project " + DateTime.Now.ToString("HHmmss"),
                        Description = "Test project",
                        ColorCode = "#FF6B6B",
                        CreatedAt = DateTime.Now,
                        IsArchived = false
                    };

                    context.Projects.Add(project);
                    context.SaveChanges();

                    MessageBox.Show($"✅ Tạo project thành công!\nID: {project.ProjectId}\nTên: {project.ProjectName}", 
                        "Test Create Project", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi tạo project:\n\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", 
                    "Test Create Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
