using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using TaskModel = TodoListApp.DAL.Models.Task;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Forms
{
    public partial class AddTaskForm : Form
    {
        private ToDoListContext _context;
        private Project _project;
        private int _userId = 1; // Tạm thời hardcode user ID

        public AddTaskForm(Project project)
        {
            InitializeComponent();
            _project = project;
            InitializeDatabase();
            InitializeControls();
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

        private void InitializeControls()
        {
            // Set default values
            cmbPriority.SelectedIndex = 1; // Medium
            cmbStatus.SelectedIndex = 0; // Pending
            dtpDueDate.Value = DateTime.Now.AddDays(1);
            dtpDueDate.Checked = false;
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Vui lòng nhập tiêu đề công việc!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get or create default user
                var user = await _context.Users.FirstOrDefaultAsync();
                if (user == null)
                {
                    user = new User
                    {
                        FullName = "Default User",
                        Email = "user@example.com",
                        PasswordHash = "default",
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                // Create new task
                var task = new TaskModel
                {
                    ProjectId = _project.ProjectId,
                    UserId = user.UserId,
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Priority = cmbPriority.SelectedItem?.ToString(),
                    Status = cmbStatus.SelectedItem?.ToString(),
                    DueDate = dtpDueDate.Checked ? dtpDueDate.Value : null,
                    EstimatedMinutes = (int?)nudEstimatedMinutes.Value,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                // Save to database
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                MessageBox.Show("Thêm công việc thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close form and return success
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm công việc: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
