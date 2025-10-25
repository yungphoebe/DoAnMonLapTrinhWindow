using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using TaskModel = TodoListApp.DAL.Models.Task;
using Microsoft.EntityFrameworkCore;
using ToDoList.GUI.Helpers;

namespace ToDoList.GUI.Forms
{
    public partial class EditTaskForm : Form
    {
        private ToDoListContext _context;
        private TaskModel _task;
        private int _userId;

        public EditTaskForm(TaskModel task)
        {
            InitializeComponent();
            _task = task;
            _userId = UserSession.GetUserId();
            InitializeDatabase();
            LoadTaskData();
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

        private void LoadTaskData()
        {
            // Load existing task data
            txtTitle.Text = _task.Title;
            txtDescription.Text = _task.Description ?? "";
            
            // Set priority
            if (!string.IsNullOrEmpty(_task.Priority))
            {
                int priorityIndex = cmbPriority.Items.IndexOf(_task.Priority);
                if (priorityIndex >= 0)
                    cmbPriority.SelectedIndex = priorityIndex;
            }
            else
            {
                cmbPriority.SelectedIndex = 1; // Medium default
            }

            // Set status
            if (!string.IsNullOrEmpty(_task.Status))
            {
                int statusIndex = cmbStatus.Items.IndexOf(_task.Status);
                if (statusIndex >= 0)
                    cmbStatus.SelectedIndex = statusIndex;
            }
            else
            {
                cmbStatus.SelectedIndex = 0; // Pending default
            }

            // Set due date
            if (_task.DueDate.HasValue)
            {
                dtpDueDate.Value = _task.DueDate.Value;
                dtpDueDate.Checked = true;
            }
            else
            {
                dtpDueDate.Checked = false;
            }

            // Set estimated minutes
            if (_task.EstimatedMinutes.HasValue)
            {
                nudEstimatedMinutes.Value = _task.EstimatedMinutes.Value;
            }

            // Set actual minutes if exists
            if (_task.ActualMinutes.HasValue && _task.ActualMinutes.Value > 0)
            {
                nudActualMinutes.Value = _task.ActualMinutes.Value;
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable button to prevent double click
                btnSave.Enabled = false;
                btnSave.Text = "Đang lưu...";

                // Validate input
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Vui lòng nhập tiêu đề công việc!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSave.Enabled = true;
                    btnSave.Text = "Lưu thay đổi";
                    return;
                }

                // Check database connection
                if (_context == null)
                {
                    MessageBox.Show("Lỗi kết nối database. Vui lòng thử lại!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = true;
                    btnSave.Text = "Lưu thay đổi";
                    return;
                }

                // Find the task in context and update
                var taskToUpdate = await _context.Tasks.FindAsync(_task.TaskId);
                if (taskToUpdate == null)
                {
                    MessageBox.Show("không tìm thấy task!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Update task properties
                taskToUpdate.Title = txtTitle.Text.Trim();
                taskToUpdate.Description = string.IsNullOrWhiteSpace(txtDescription.Text) 
                    ? null 
                    : txtDescription.Text.Trim();
                taskToUpdate.Priority = cmbPriority.SelectedItem?.ToString();
                taskToUpdate.Status = cmbStatus.SelectedItem?.ToString();
                taskToUpdate.DueDate = dtpDueDate.Checked ? dtpDueDate.Value : null;
                taskToUpdate.EstimatedMinutes = (int?)nudEstimatedMinutes.Value;
                taskToUpdate.ActualMinutes = (int?)nudActualMinutes.Value;
                taskToUpdate.UpdatedAt = DateTime.Now;

                // Save to database
                await _context.SaveChangesAsync();

                MessageBox.Show("cập nhật task thành công", "thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close form and return success
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"lỗi khi cập nhât task: {ex.Message}\n\nchi tiết: {ex.InnerException?.Message}", "lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Re-enable button
                btnSave.Enabled = true;
                btnSave.Text = "Lưu thay đổi";
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
