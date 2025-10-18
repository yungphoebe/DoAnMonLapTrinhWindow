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
using ToDoList.GUI.Helpers;

namespace ToDoList.GUI.Forms
{
    public partial class ProjectDetailsForm : Form
    {
        private ToDoListContext _context;
        private Project _project;
        private int _userId = 1; // Tạm thời hardcode user ID

        public ProjectDetailsForm(Project project)
        {
            InitializeComponent();
            _project = project;
            InitializeDatabase();
            LoadProjectDetails();
            LoadTasks();
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

        private void LoadProjectDetails()
        {
            lblProjectName.Text = _project.ProjectName;
            lblProjectDescription.Text = _project.Description ?? "Không có mô tả";

            if (!string.IsNullOrEmpty(_project.ColorCode))
            {
                try
                {
                    var color = ColorTranslator.FromHtml(_project.ColorCode);
                    pnlProjectColor.BackColor = color;
                }
                catch
                {
                    pnlProjectColor.BackColor = Color.FromArgb(200, 180, 50);
                }
            }
        }

        private async void LoadTasks()
        {
            try
            {
                if (_context == null) return;

                // ✅ FIX: Lấy UserId từ UserSession thay vì FirstOrDefaultAsync()
                _userId = UserSession.GetUserId();
                
                if (_userId == 0)
                {
                    MessageBox.Show("Lỗi: Không tìm thấy thông tin người dùng. Vui lòng đăng nhập lại.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Load tasks CHỈ của project này
                var tasks = await _context.Tasks
                    .Where(t => t.ProjectId == _project.ProjectId && t.UserId == _userId && t.IsDeleted != true)
                    .OrderBy(t => t.CreatedAt)
                    .ToListAsync();

                pnlTasksContainer.Controls.Clear();

                foreach (var task in tasks)
                {
                    AddTaskCard(task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách công việc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddTaskCard(TaskModel task)
        {
            Panel taskCard = new Panel
            {
                Width = 750,  // Tăng width để chứa nút mới
                Height = 60,
                BackColor = Color.FromArgb(40, 40, 40),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };

            // Thêm hover effect
            taskCard.MouseEnter += (s, e) => taskCard.BackColor = Color.FromArgb(50, 50, 50);
            taskCard.MouseLeave += (s, e) => taskCard.BackColor = Color.FromArgb(40, 40, 40);

            // Checkbox
            CheckBox chkTask = new CheckBox
            {
                Location = new Point(15, 20),
                Size = new Size(20, 20),
                Checked = task.Status == "Completed",
                Cursor = Cursors.Default
            };
            chkTask.CheckedChanged += (s, e) => ToggleTaskStatus(task, chkTask.Checked);

            // Task title
            Label lblTitle = new Label
            {
                Text = task.Title,
                Location = new Point(50, 15),
                Size = new Size(250, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            // Priority
            Label lblPriority = new Label
            {
                Text = task.Priority ?? "Medium",
                Location = new Point(320, 15),
                Size = new Size(70, 25),
                ForeColor = GetPriorityColor(task.Priority),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Status
            Label lblStatus = new Label
            {
                Text = task.Status ?? "Pending",
                Location = new Point(400, 15),
                Size = new Size(80, 25),
                ForeColor = GetStatusColor(task.Status),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Due date
            Label lblDueDate = new Label
            {
                Text = task.DueDate?.ToString("dd/MM/yyyy") ?? "Không có",
                Location = new Point(490, 15),
                Size = new Size(90, 25),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleRight
            };

            // ✨ NÚT XEM BÁO CÁO - MỚI
            Button btnReport = new Button
            {
                Text = "📊",
                Location = new Point(590, 15),
                Size = new Size(35, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 149, 237),
                Font = new Font("Segoe UI", 14F),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnReport.FlatAppearance.BorderSize = 0;
            btnReport.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 169, 255);
            btnReport.Click += (s, e) => ShowTaskReport(task);
            
            // Tooltip cho nút báo cáo
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnReport, "Xem báo cáo chi tiết task");

            // ✏️ NÚT CHỈNH SỬA - MỚI
            Button btnEdit = new Button
            {
                Text = "✏️",
                Location = new Point(635, 15),
                Size = new Size(35, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(255, 165, 0),
                Font = new Font("Segoe UI", 12F),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 185, 50);
            btnEdit.Click += (s, e) => EditTask(task);
            toolTip.SetToolTip(btnEdit, "Chỉnh sửa task");

            // Menu button (giữ lại cho các tùy chọn khác)
            Button btnMenu = new Button
            {
                Text = "⋮",
                Location = new Point(680, 15),
                Size = new Size(25, 25),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 16F),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);
            btnMenu.Click += (s, e) => ShowTaskMenu(task, btnMenu);
            toolTip.SetToolTip(btnMenu, "Thêm tùy chọn");

            taskCard.Controls.Add(chkTask);
            taskCard.Controls.Add(lblTitle);
            taskCard.Controls.Add(lblPriority);
            taskCard.Controls.Add(lblStatus);
            taskCard.Controls.Add(lblDueDate);
            taskCard.Controls.Add(btnReport);  // ✨ THÊM
            taskCard.Controls.Add(btnEdit);    // ✨ THÊM
            taskCard.Controls.Add(btnMenu);

            pnlTasksContainer.Controls.Add(taskCard);
        }

        private Color GetPriorityColor(string priority)
        {
            switch (priority?.ToLower())
            {
                case "high": return Color.Red;
                case "medium": return Color.Orange;
                case "low": return Color.Green;
                default: return Color.Gray;
            }
        }

        private Color GetStatusColor(string status)
        {
            switch (status?.ToLower())
            {
                case "completed": return Color.Green;
                case "in progress": return Color.Blue;
                case "pending": return Color.Orange;
                default: return Color.Gray;
            }
        }

        private async void ToggleTaskStatus(TaskModel task, bool isCompleted)
        {
            try
            {
                task.Status = isCompleted ? "Completed" : "Pending";
                task.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                // Reload tasks to update UI
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật trạng thái: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowTaskMenu(TaskModel task, Button btnMenu)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.BackColor = Color.FromArgb(40, 40, 40);
            menu.ForeColor = Color.White;

            // Đánh dấu hoàn thành/chưa hoàn thành
            var toggleText = task.Status == "Completed" ? "↩️ Đánh dấu chưa hoàn thành" : "✅ Đánh dấu hoàn thành";
            var toggleItem = menu.Items.Add(toggleText, null, (s, e) =>
            {
                var newStatus = task.Status == "Completed" ? "Pending" : "Completed";
                ToggleTaskStatus(task, newStatus == "Completed");
            });
            toggleItem.BackColor = Color.FromArgb(40, 40, 40);
            toggleItem.ForeColor = Color.White;

            // Separator
            menu.Items.Add(new ToolStripSeparator { BackColor = Color.FromArgb(60, 60, 60) });

            // Xóa
            var deleteItem = menu.Items.Add("🗑️ Xóa task", null, (s, e) => DeleteTask(task));
            deleteItem.BackColor = Color.FromArgb(40, 40, 40);
            deleteItem.ForeColor = Color.FromArgb(255, 100, 100);

            menu.Show(btnMenu, new Point(0, btnMenu.Height));
        }

        private void ShowTaskReport(TaskModel task)
        {
            try
            {
                using (var reportForm = new TaskReportForm(_context, task, _userId))
                {
                    reportForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở báo cáo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditTask(TaskModel task)
        {
            // TODO: Implement edit task form
            MessageBox.Show($"Chỉnh sửa task: {task.Title}", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void DeleteTask(TaskModel task)
        {
            try
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa task '{task.Title}'?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    task.IsDeleted = true;
                    task.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa task: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            using (var addTaskForm = new AddTaskForm(_project))
            {
                if (addTaskForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload tasks after adding new one
                    LoadTasks();
                }
            }
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

        private void ProjectDetailsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
