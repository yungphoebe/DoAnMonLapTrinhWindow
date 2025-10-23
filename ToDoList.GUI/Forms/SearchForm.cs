using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using ToDoList.GUI.Helpers;

namespace ToDoList.GUI.Forms
{
    public partial class SearchForm : Form
    {
        private ToDoListContext _context;
        private TextBox txtSearch;
        private Panel pnlQuickActions;
        private FlowLayoutPanel flowSearchResults;
        private Label lblNoResults;
        private int _userId;

        public SearchForm()
        {
            InitializeComponent();
            InitializeDatabase();
            SetupUI();
            
            // Set focus to search box
            this.Shown += (s, e) => txtSearch.Focus();
        }

        private void InitializeDatabase()
        {
            try
            {
                _context = new ToDoListContext();
                _userId = UserSession.GetUserId();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối database: {ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Tìm ki?m - CuLuList";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(24, 24, 24);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.KeyPreview = true;
            
            // ESC to close
            this.KeyDown += (s, e) => 
            {
                if (e.KeyCode == Keys.Escape)
                    this.Close();
            };
        }

        private void SetupUI()
        {
            // Main container panel
            Panel pnlMain = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(800, 600),
                BackColor = Color.FromArgb(24, 24, 24),
                Dock = DockStyle.Fill
            };

            // Search box panel
            Panel pnlSearchBox = new Panel
            {
                Location = new Point(30, 30),
                Size = new Size(740, 60),
                BackColor = Color.FromArgb(40, 40, 40),
                Padding = new Padding(15)
            };

            // Search icon
            Label lblSearchIcon = new Label
            {
                Text = "??",
                Location = new Point(15, 15),
                Size = new Size(30, 30),
                Font = new Font("Segoe UI", 16F),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Search textbox
            txtSearch = new TextBox
            {
                Location = new Point(55, 15),
                Size = new Size(610, 30),
                Font = new Font("Segoe UI", 14F),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 40, 40),
                BorderStyle = BorderStyle.None,
                PlaceholderText = "Search for tasks, lists"
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Ctrl+F hint
            Label lblShortcut = new Label
            {
                Text = "Ctrl+F",
                Location = new Point(680, 18),
                Size = new Size(50, 24),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlSearchBox.Controls.Add(lblSearchIcon);
            pnlSearchBox.Controls.Add(txtSearch);
            pnlSearchBox.Controls.Add(lblShortcut);

            // Quick actions section
            Label lblQuickActions = new Label
            {
                Text = "Quick actions",
                Location = new Point(30, 110),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White
            };

            pnlQuickActions = new Panel
            {
                Location = new Point(30, 145),
                Size = new Size(740, 80),
                BackColor = Color.Transparent
            };

            // Create quick action buttons
            CreateQuickActionButton(" Add new task", 0, AddNewTask_Click);
            CreateQuickActionButton("  Add new list", 220, AddNewList_Click);
            CreateQuickActionButton(" Go to Reports", 440, GoToReports_Click);

            // Search results container
            flowSearchResults = new FlowLayoutPanel
            {
                Location = new Point(30, 245),
                Size = new Size(740, 315),
                BackColor = Color.Transparent,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // No results label
            lblNoResults = new Label
            {
                Text = "Nhập từ khóa tìm kiếm tasks và lists...",
                Location = new Point(200, 100),
                Size = new Size(340, 100),
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = true
            };
            flowSearchResults.Controls.Add(lblNoResults);

            // Add all controls to main panel
            pnlMain.Controls.Add(pnlSearchBox);
            pnlMain.Controls.Add(lblQuickActions);
            pnlMain.Controls.Add(pnlQuickActions);
            pnlMain.Controls.Add(flowSearchResults);

            this.Controls.Add(pnlMain);
        }

        private void CreateQuickActionButton(string text, int offsetX, EventHandler clickHandler)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(offsetX, 0),
                Size = new Size(200, 60),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 62);
            btn.Click += clickHandler;

            pnlQuickActions.Controls.Add(btn);
        }

        private async void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Clear results
                flowSearchResults.Controls.Clear();
                lblNoResults.Text = "Nhập tự khóa tìm kiếm tasks và lists...";
                lblNoResults.Visible = true;
                flowSearchResults.Controls.Add(lblNoResults);
                return;
            }

            await PerformSearch(searchText);
        }

        private async System.Threading.Tasks.Task PerformSearch(string searchText)
        {
            try
            {
                flowSearchResults.Controls.Clear();
                lblNoResults.Visible = false;

                // Search in Projects (Lists)
                var projects = await _context.Projects
                    .Where(p => p.UserId == _userId && 
                                p.IsArchived != true &&
                                p.ProjectName.Contains(searchText))
                    .ToListAsync();

                // Search in Tasks
                var tasks = await _context.Tasks
                    .Include(t => t.Project)
                    .Where(t => t.UserId == _userId && 
                                t.IsDeleted != true &&
                                (t.Title.Contains(searchText) || 
                                 t.Description.Contains(searchText)))
                    .ToListAsync();

                // Display results
                if (projects.Any())
                {
                    AddSectionHeader($"Danh sách ({projects.Count})");
                    foreach (var project in projects)
                    {
                        AddProjectResult(project);
                    }
                }

                if (tasks.Any())
                {
                    AddSectionHeader($"Công việc ({tasks.Count})");
                    foreach (var task in tasks)
                    {
                        AddTaskResult(task);
                    }
                }

                if (!projects.Any() && !tasks.Any())
                {
                    lblNoResults.Text = $"Không tìm thấy kết quả cho \"{searchText}\"";
                    lblNoResults.Visible = true;
                    flowSearchResults.Controls.Add(lblNoResults);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSectionHeader(string text)
        {
            Label lbl = new Label
            {
                Text = text,
                Size = new Size(720, 30),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 150),
                Margin = new Padding(0, 10, 0, 5)
            };
            flowSearchResults.Controls.Add(lbl);
        }

        private void AddProjectResult(Project project)
        {
            Panel pnl = new Panel
            {
                Size = new Size(720, 60),
                BackColor = Color.FromArgb(35, 35, 35),
                Margin = new Padding(0, 5, 0, 5),
                Cursor = Cursors.Hand
            };

            // Project icon
            Label lblIcon = new Label
            {
                Text = "??",
                Location = new Point(15, 15),
                Size = new Size(30, 30),
                Font = new Font("Segoe UI", 16F),
                ForeColor = Color.White
            };

            // Project name
            Label lblName = new Label
            {
                Text = project.ProjectName,
                Location = new Point(55, 12),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White
            };

            // Task count
            int taskCount = _context.Tasks.Count(t => t.ProjectId == project.ProjectId && t.IsDeleted != true);
            Label lblCount = new Label
            {
                Text = $"{taskCount} tasks",
                Location = new Point(55, 35),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray
            };

            pnl.Controls.Add(lblIcon);
            pnl.Controls.Add(lblName);
            pnl.Controls.Add(lblCount);

            // Click to open project details
            EventHandler clickHandler = (s, e) =>
            {
                using (var detailsForm = new ProjectDetailsForm(project))
                {
                    detailsForm.ShowDialog();
                }
            };
            pnl.Click += clickHandler;
            lblIcon.Click += clickHandler;
            lblName.Click += clickHandler;
            lblCount.Click += clickHandler;

            // Hover effect
            pnl.MouseEnter += (s, e) => pnl.BackColor = Color.FromArgb(45, 45, 45);
            pnl.MouseLeave += (s, e) => pnl.BackColor = Color.FromArgb(35, 35, 35);

            flowSearchResults.Controls.Add(pnl);
        }

        private void AddTaskResult(TodoListApp.DAL.Models.Task task)
        {
            Panel pnl = new Panel
            {
                Size = new Size(720, 70),
                BackColor = Color.FromArgb(35, 35, 35),
                Margin = new Padding(0, 5, 0, 5),
                Cursor = Cursors.Hand
            };

            // Checkbox
            CheckBox chk = new CheckBox
            {
                Location = new Point(15, 15),
                Size = new Size(20, 20),
                Checked = task.Status == "Completed"
            };
            chk.CheckedChanged += async (s, e) =>
            {
                task.Status = chk.Checked ? "Completed" : "Pending";
                await _context.SaveChangesAsync();
            };

            // Task title
            Label lblTitle = new Label
            {
                Text = task.Title,
                Location = new Point(45, 12),
                Size = new Size(500, 25),
                Font = new Font("Segoe UI", 11F, task.Status == "Completed" ? FontStyle.Strikeout : FontStyle.Bold),
                ForeColor = task.Status == "Completed" ? Color.Gray : Color.White
            };

            // Task details
            string projectName = task.Project?.ProjectName ?? "No Project";
            string priority = task.Priority ?? "Medium";
            Label lblDetails = new Label
            {
                Text = $"{projectName} • {priority} • {task.EstimatedMinutes ?? 0}ph",
                Location = new Point(45, 37),
                Size = new Size(500, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray
            };

            pnl.Controls.Add(chk);
            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblDetails);

            // Click to open task details (placeholder)
            EventHandler clickHandler = (s, e) =>
            {
                MessageBox.Show($"Task: {task.Title}\n\nChức năng xem chi tiếttask đang được phát triển.", 
                    "Task Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            pnl.Click += clickHandler;
            lblTitle.Click += clickHandler;
            lblDetails.Click += clickHandler;

            // Hover effect
            pnl.MouseEnter += (s, e) => pnl.BackColor = Color.FromArgb(45, 45, 45);
            pnl.MouseLeave += (s, e) => pnl.BackColor = Color.FromArgb(35, 35, 35);

            flowSearchResults.Controls.Add(pnl);
        }

        private void AddNewTask_Click(object sender, EventArgs e)
        {
            using (var addTaskForm = new AddTaskToListForm())
            {
                if (addTaskForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Task đã đượcthêm thành công!", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Refresh search results if there's a search query
                    if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        TxtSearch_TextChanged(txtSearch, EventArgs.Empty);
                    }
                }
            }
        }

        private void AddNewList_Click(object sender, EventArgs e)
        {
            using (var createListForm = new CreateListForm())
            {
                if (createListForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Danh sách mời đã được tạo thành công!", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void GoToReports_Click(object sender, EventArgs e)
        {
            try
            {
                using (var reportsForm = new ReportsForm(_context))
                {
                    reportsForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở báo cáo: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
