using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using TaskModel = TodoListApp.DAL.Models.Task;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Tests
{
    /// <summary>
    /// Test class để kiểm tra chức năng quản lý project
    /// </summary>
    public class ProjectManagementTest
    {
        private ToDoListContext _context;
        private int _testUserId = 999; // ID test để không ảnh hưởng dữ liệu thật

        public ProjectManagementTest()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                _context = new ToDoListContext();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối database: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Test tạo project mới
        /// </summary>
        public async Task<bool> TestCreateProject()
        {
            try
            {
                var project = new Project
                {
                    UserId = _testUserId,
                    ProjectName = "Test Project " + DateTime.Now.ToString("HHmmss"),
                    Description = "Project test tự động",
                    ColorCode = "#FF6B6B",
                    CreatedAt = DateTime.Now,
                    IsArchived = false
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                MessageBox.Show($"Tạo project thành công! ID: {project.ProjectId}", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo project: {ex.Message}", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Test tạo task mới
        /// </summary>
        public async Task<bool> TestCreateTask(int projectId)
        {
            try
            {
                var task = new TaskModel
                {
                    ProjectId = projectId,
                    UserId = _testUserId,
                    Title = "Test Task " + DateTime.Now.ToString("HHmmss"),
                    Description = "Task test tự động",
                    Priority = "Medium",
                    Status = "Pending",
                    DueDate = DateTime.Now.AddDays(1),
                    EstimatedMinutes = 30,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                MessageBox.Show($"Tạo task thành công! ID: {task.TaskId}", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo task: {ex.Message}", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Test lấy danh sách project
        /// </summary>
        public async Task<bool> TestGetProjects()
        {
            try
            {
                var projects = await _context.Projects
                    .Where(p => p.UserId == _testUserId)
                    .Include(p => p.Tasks)
                    .ToListAsync();

                MessageBox.Show($"Tìm thấy {projects.Count} project(s)", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lấy danh sách project: {ex.Message}", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Test cập nhật task
        /// </summary>
        public async Task<bool> TestUpdateTask(int taskId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task != null)
                {
                    task.Status = "Completed";
                    task.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();

                    MessageBox.Show($"Cập nhật task thành công!", "Test", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy task", "Test", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật task: {ex.Message}", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Test xóa task (soft delete)
        /// </summary>
        public async Task<bool> TestDeleteTask(int taskId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task != null)
                {
                    task.IsDeleted = true;
                    task.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();

                    MessageBox.Show($"Xóa task thành công!", "Test", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy task", "Test", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa task: {ex.Message}", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Chạy tất cả test
        /// </summary>
        public async System.Threading.Tasks.Task RunAllTests()
        {
            MessageBox.Show("Bắt đầu chạy test...", "Test", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Test 1: Tạo project
            var projectCreated = await TestCreateProject();
            if (!projectCreated) return;

            // Test 2: Lấy danh sách project
            await TestGetProjects();

            // Test 3: Tạo task (cần project ID)
            var projects = await _context.Projects
                .Where(p => p.UserId == _testUserId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            if (projects.Any())
            {
                var projectId = projects.First().ProjectId;
                var taskCreated = await TestCreateTask(projectId);
                
                if (taskCreated)
                {
                    // Test 4: Cập nhật task
                    var tasks = await _context.Tasks
                        .Where(t => t.ProjectId == projectId && t.IsDeleted != true)
                        .OrderByDescending(t => t.CreatedAt)
                        .ToListAsync();

                    if (tasks.Any())
                    {
                        var taskId = tasks.First().TaskId;
                        await TestUpdateTask(taskId);
                        await TestDeleteTask(taskId);
                    }
                }
            }

            MessageBox.Show("Hoàn thành tất cả test!", "Test", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Dọn dẹp dữ liệu test
        /// </summary>
        public async System.Threading.Tasks.Task CleanupTestData()
        {
            try
            {
                var testProjects = await _context.Projects
                    .Where(p => p.UserId == _testUserId)
                    .ToListAsync();

                foreach (var project in testProjects)
                {
                    var tasks = await _context.Tasks
                        .Where(t => t.ProjectId == project.ProjectId)
                        .ToListAsync();

                    _context.Tasks.RemoveRange(tasks);
                }

                _context.Projects.RemoveRange(testProjects);
                await _context.SaveChangesAsync();

                MessageBox.Show("Dọn dẹp dữ liệu test thành công!", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi dọn dẹp: {ex.Message}", "Test", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
