using System;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using TaskModel = TodoListApp.DAL.Models.Task;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Tests
{
    /// <summary>
    /// Test kết nối database và tạo dữ liệu mẫu
    /// </summary>
    public class DatabaseConnectionTest
    {
        public static void TestConnectionAndCreateSampleData()
        {
            try
            {
                using (var context = new ToDoListContext())
                {
                    // Test connection
                    var canConnect = context.Database.CanConnect();
                    if (!canConnect)
                    {
                        MessageBox.Show("❌ Không thể kết nối database!", "Test Database", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Create sample user
                    var user = new User
                    {
                        FullName = "Test User",
                        Email = "test@example.com",
                        PasswordHash = "hashedpassword",
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    context.Users.Add(user);
                    context.SaveChanges();

                    // Create sample project
                    var project = new Project
                    {
                        UserId = user.UserId,
                        ProjectName = "Dự án mẫu",
                        Description = "Đây là dự án mẫu để test",
                        ColorCode = "#FF6B6B",
                        CreatedAt = DateTime.Now,
                        IsArchived = false
                    };

                    context.Projects.Add(project);
                    context.SaveChanges();

                    // Create sample task
                    var task = new TaskModel
                    {
                        ProjectId = project.ProjectId,
                        UserId = user.UserId,
                        Title = "Công việc mẫu",
                        Description = "Đây là công việc mẫu",
                        Priority = "Medium",
                        Status = "Pending",
                        DueDate = DateTime.Now.AddDays(1),
                        EstimatedMinutes = 30,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false
                    };

                    context.Tasks.Add(task);
                    context.SaveChanges();

                    MessageBox.Show($"✅ Kết nối database thành công!\n\n" +
                        $"✅ Đã tạo dữ liệu mẫu:\n" +
                        $"• User: {user.FullName} (ID: {user.UserId})\n" +
                        $"• Project: {project.ProjectName} (ID: {project.ProjectId})\n" +
                        $"• Task: {task.Title} (ID: {task.TaskId})", 
                        "Test Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi test database:\n\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", 
                    "Test Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}