using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Forms
{
    public partial class AdvancedReportsForm : Form
    {
        private ToDoListContext _context;
        
        // UI Controls
        private Panel pnlHeader = null!;
        private TabControl tabControl = null!;
        private Panel pnlOverview = null!;
        private Panel pnlActivity = null!;
        private Panel pnlProgress = null!;
        private Panel pnlTime = null!;

        public AdvancedReportsForm(ToDoListContext context)
        {
            _context = context;
            InitializeComponent();
            LoadStatistics();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1200, 800);
            this.Text = "Thong ke nang cao - ToDoList Analytics";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1000, 600);

            CreateHeader();
            CreateTabs();
        }

        private void CreateHeader()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(25, 25, 25),
                Padding = new Padding(20, 15, 20, 15)
            };

            Label lblTitle = new Label
            {
                Text = "Thong ke nang cao - ToDoList Analytics",
                Location = new Point(20, 20),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Button btnClose = new Button
            {
                Text = "X",
                Location = new Point(1130, 20),
                Size = new Size(40, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnClose);
            this.Controls.Add(pnlHeader);
        }

        private void CreateTabs()
        {
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                Padding = new Point(10, 5)
            };

            // Tab 1: Tong quan
            TabPage tabOverview = new TabPage("Tong quan")
            {
                BackColor = Color.FromArgb(20, 20, 20)
            };
            CreateOverviewTab(tabOverview);

            // Tab 2: Nang suat
            TabPage tabProductivity = new TabPage("Nang suat")
            {
                BackColor = Color.FromArgb(20, 20, 20)
            };
            CreateProductivityTab(tabProductivity);

            // Tab 3: Tien do
            TabPage tabProgress = new TabPage("Tien do")
            {
                BackColor = Color.FromArgb(20, 20, 20)
            };
            CreateProgressTab(tabProgress);

            // Tab 4: Thoi gian
            TabPage tabTime = new TabPage("Thoi gian")
            {
                BackColor = Color.FromArgb(20, 20, 20)
            };
            CreateTimeTab(tabTime);

            tabControl.TabPages.Add(tabOverview);
            tabControl.TabPages.Add(tabProductivity);
            tabControl.TabPages.Add(tabProgress);
            tabControl.TabPages.Add(tabTime);

            this.Controls.Add(tabControl);
        }

        private void CreateOverviewTab(TabPage tab)
        {
            pnlOverview = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(30)
            };

            // Title
            Label lblOverviewTitle = new Label
            {
       
                Location = new Point(30, 20),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Stats cards container (ROW 1)
            FlowLayoutPanel flowStats = new FlowLayoutPanel
            {
                Location = new Point(30, 70),
                Size = new Size(1100, 180),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.Transparent
            };

            // Create 4 stat cards
            flowStats.Controls.Add(CreateStatCard("", "Tổng Projects", "0", "projects", Color.FromArgb(52, 152, 219)));
            flowStats.Controls.Add(CreateStatCard("", "Tổng Tasks", "0", "tasks", Color.FromArgb(46, 204, 113)));
            flowStats.Controls.Add(CreateStatCard("", "Hoàn thành", "0%", "completion", Color.FromArgb(155, 89, 182)));
            flowStats.Controls.Add(CreateStatCard("", "Thời gian", "0h", "time spent", Color.FromArgb(230, 126, 34)));

            // ========================================
            // ROW 2: TWO SECTIONS (Activity + Progress)
            // ========================================
            
            // LEFT: Activity Section
            Panel pnlActivitySection = new Panel
            {
                Location = new Point(30, 270),
                Size = new Size(530, 420),
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20)
            };

            Label lblActivityTitle = new Label
            {
                Text = "📝 Hoạt động gần đây",
                Location = new Point(20, 15),
                Size = new Size(490, 30),
                Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            FlowLayoutPanel flowActivity = new FlowLayoutPanel
            {
                Location = new Point(20, 55),
                Size = new Size(490, 345),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Name = "flowActivity"
            };

            pnlActivitySection.Controls.Add(lblActivityTitle);
            pnlActivitySection.Controls.Add(flowActivity);

            // RIGHT: Progress Section
            Panel pnlProgressSection = new Panel
            {
                Location = new Point(580, 270),
                Size = new Size(550, 420),
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20)
            };

            Label lblProgressTitle = new Label
            {
                Text = "📊 Tiến độ dự án",
                Location = new Point(20, 15),
                Size = new Size(510, 30),
                Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            FlowLayoutPanel flowProgressSection = new FlowLayoutPanel
            {
                Location = new Point(20, 55),
                Size = new Size(510, 345),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Name = "flowProgressOverview"
            };

            pnlProgressSection.Controls.Add(lblProgressTitle);
            pnlProgressSection.Controls.Add(flowProgressSection);

            // Add all to overview panel
            pnlOverview.Controls.Add(lblOverviewTitle);
            pnlOverview.Controls.Add(flowStats);
            pnlOverview.Controls.Add(pnlActivitySection);
            pnlOverview.Controls.Add(pnlProgressSection);
            
            tab.Controls.Add(pnlOverview);
        }

        private Panel CreateStatCard(string icon, string label, string value, string subtitle, Color color)
        {
            Panel card = new Panel
            {
                Size = new Size(250, 150),
                BackColor = Color.FromArgb(30, 30, 30),
                Margin = new Padding(10),
                Padding = new Padding(20)
            };

            // Icon
            Label lblIcon = new Label
            {
                Text = icon,
                Location = new Point(20, 20),
                Size = new Size(50, 50),
                Font = new Font("Segoe UI", 28F),
                ForeColor = color,
                BackColor = Color.Transparent
            };

            // Label
            Label lblLabel = new Label
            {
                Text = label,
                Location = new Point(20, 80),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.Transparent
            };

            // Value
            Label lblValue = new Label
            {
                Text = value,
                Location = new Point(20, 100),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Name = $"lblValue_{subtitle.Replace(" ", "")}"
            };

            // Subtitle
            Label lblSubtitle = new Label
            {
                Text = subtitle,
                Location = new Point(170, 110),
                Size = new Size(70, 15),
                Font = new Font("Segoe UI", 7F),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblLabel);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblSubtitle);

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(40, 40, 40);
            card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(30, 30, 30);

            return card;
        }

        private void CreateProductivityTab(TabPage tab)
        {
            pnlActivity = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(30)
            };

            Label lblTitle = new Label
            {
                Text = "Năng suất làm việc",
                Location = new Point(30, 20),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            pnlActivity.Controls.Add(lblTitle);
            tab.Controls.Add(pnlActivity);
        }

        private void CreateProgressTab(TabPage tab)
        {
            pnlProgress = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(30)
            };

            Label lblTitle = new Label
            {
                Text = "Tien do theo du an",
                Location = new Point(30, 20),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Progress list container
            FlowLayoutPanel flowProgress = new FlowLayoutPanel
            {
                Location = new Point(30, 70),
                Size = new Size(1100, 600),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Name = "flowProgress"
            };

            pnlProgress.Controls.Add(lblTitle);
            pnlProgress.Controls.Add(flowProgress);
            tab.Controls.Add(pnlProgress);
        }

        private void CreateTimeTab(TabPage tab)
        {
            pnlTime = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(30)
            };

            Label lblTitle = new Label
            {
                Text = "Phân tích thời gian",
                Location = new Point(30, 20),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            pnlTime.Controls.Add(lblTitle);
            tab.Controls.Add(pnlTime);
        }

        private async void LoadStatistics()
        {
            try
            {
                // Get total projects
                var totalProjects = await _context.Projects
                    .Where(p => p.IsArchived != true)
                    .CountAsync();

                // Get total tasks
                var totalTasks = await _context.Tasks
                    .Where(t => t.IsDeleted != true)
                    .CountAsync();

                // Get completed tasks
                var completedTasks = await _context.Tasks
                    .Where(t => t.IsDeleted != true && t.Status == "Completed")
                    .CountAsync();

                // Calculate completion rate
                var completionRate = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;

                // Calculate total time
                var totalMinutes = await _context.Tasks
                    .Where(t => t.IsDeleted != true)
                    .SumAsync(t => t.EstimatedMinutes ?? 0);
                var totalHours = totalMinutes / 60.0;

                // Update stat cards
                UpdateStatCard("projects", totalProjects.ToString());
                UpdateStatCard("tasks", totalTasks.ToString());
                UpdateStatCard("completion", $"{completionRate:F0}%");
                UpdateStatCard("timespent", $"{totalHours:F1}h");

                // Load recent activity
                await LoadRecentActivity();
                
                // Load project progress overview
                await LoadProjectProgressOverview();
                
                // Load full project progress (for Progress tab)
                await LoadProjectProgress();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ỗi thống kê: {ex.Message}", "Loi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatCard(string name, string value)
        {
            var lblValue = FindControlByName(pnlOverview, $"lblValue_{name}") as Label;
            if (lblValue != null)
            {
                lblValue.Text = value;
            }
        }

        private async System.Threading.Tasks.Task LoadProjectProgress()
        {
            try
            {
                var progressFlow = FindControlByName(pnlProgress, "flowProgress") as FlowLayoutPanel;
                if (progressFlow == null) return;

                progressFlow.Controls.Clear();

                var projects = await _context.Projects
                    .Where(p => p.IsArchived != true)
                    .Include(p => p.Tasks.Where(t => t.IsDeleted != true))
                    .ToListAsync();

                foreach (var project in projects)
                {
                    var totalTasks = project.Tasks.Count();
                    var completedTasks = project.Tasks.Count(t => t.Status == "Completed");
                    var percentage = totalTasks > 0 ? (int)((double)completedTasks / totalTasks * 100) : 0;

                    var progressItem = CreateProgressItem(
                        project.ProjectName ?? "Unnamed Project",
                        percentage,
                        completedTasks,
                        totalTasks
                    );

                    progressFlow.Controls.Add(progressItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi  tiến độ: {ex.Message}", "Loi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateProgressItem(string projectName, int percentage, int completed, int total)
        {
            Panel container = new Panel
            {
                Size = new Size(1000, 55),
                BackColor = Color.FromArgb(30, 30, 30),
                Margin = new Padding(5),
                Padding = new Padding(15)
            };

            // Project name
            Label lblName = new Label
            {
                Text = TruncateText(projectName, 30),
                Location = new Point(0, 0),
                Size = new Size(320, 22),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };

            // Percentage
            Label lblPercentage = new Label
            {
                Text = $"{percentage}%",
                Location = new Point(420, 0),
                Size = new Size(60, 22),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = GetProgressColor(percentage),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Progress bar background
            Panel progressBg = new Panel
            {
                Location = new Point(0, 26),
                Size = new Size(480, 8),
                BackColor = Color.FromArgb(50, 50, 50)
            };

            // Progress bar fill
            Panel progressFill = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size((int)(480 * percentage / 100.0), 8),
                BackColor = GetProgressColor(percentage)
            };
            progressBg.Controls.Add(progressFill);

            // Task count
            Label lblCount = new Label
            {
                Text = $"{completed}/{total} tasks",
                Location = new Point(0, 36),
                Size = new Size(150, 14),
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.Transparent
            };

            container.Controls.Add(lblName);
            container.Controls.Add(lblPercentage);
            container.Controls.Add(progressBg);
            container.Controls.Add(lblCount);

            return container;
        }

        private Color GetProgressColor(int percentage)
        {
            if (percentage >= 75) return Color.FromArgb(46, 204, 113);
            if (percentage >= 50) return Color.FromArgb(241, 196, 15);
            if (percentage >= 25) return Color.FromArgb(230, 126, 34);
            return Color.FromArgb(231, 76, 60);
        }

        private string TruncateText(string text, int maxLength)
        {
            if (text.Length <= maxLength) return text;
            return text.Substring(0, maxLength - 3) + "...";
        }

        private string GetTimeAgo(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return "N/A";

            var timeSpan = DateTime.Now - dateTime.Value;
            
            if (timeSpan.TotalMinutes < 1) return "bây giờ";
            if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}m";
            if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}h";
            if (timeSpan.TotalDays < 7) return $"{(int)timeSpan.TotalDays}d";
            return dateTime.Value.ToString("dd/MM");
        }

        private Control? FindControlByName(Control parent, string name)
        {
            if (parent.Name == name)
                return parent;

            foreach (Control child in parent.Controls)
            {
                var found = FindControlByName(child, name);
                if (found != null)
                    return found;
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Context is passed from outside, don't dispose it
            }
            base.Dispose(disposing);
        }

        private async System.Threading.Tasks.Task LoadRecentActivity()
        {
            try
            {
                var activityFlow = FindControlByName(pnlOverview, "flowActivity") as FlowLayoutPanel;
                if (activityFlow == null) return;

                activityFlow.Controls.Clear();

                var recentTasks = await _context.Tasks
                    .Where(t => t.IsDeleted != true)
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(8)
                    .Include(t => t.Project)
                    .ToListAsync();

                foreach (var task in recentTasks)
                {
                    var activityItem = CreateActivityItem(
                        task.Title ?? "Unnamed Task",
                        task.Project?.ProjectName ?? "No Project",
                        GetTimeAgo(task.CreatedAt)
                    );
                    activityFlow.Controls.Add(activityItem);
                }
            }
            catch (Exception ex)
            {
                // Silently handle errors
            }
        }

        private Panel CreateActivityItem(string taskName, string projectName, string timeAgo)
        {
            Panel item = new Panel
            {
                Size = new Size(470, 40),
                BackColor = Color.FromArgb(40, 40, 40),
                Margin = new Padding(0, 2, 0, 2),
                Padding = new Padding(10)
            };

            Label lblIcon = new Label
            {
                Text = "✓",
                Location = new Point(10, 10),
                Size = new Size(20, 20),
                Font = new Font("Arial", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 100),
                BackColor = Color.Transparent
            };

            Label lblTask = new Label
            {
                Text = TruncateText(taskName, 25),
                Location = new Point(40, 10),
                Size = new Size(250, 20),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };

            Label lblProject = new Label
            {
                Text = TruncateText(projectName, 12),
                Location = new Point(300, 10),
                Size = new Size(100, 20),
                Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblTime = new Label
            {
                Text = timeAgo,
                Location = new Point(410, 10),
                Size = new Size(50, 20),
                Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            item.Controls.Add(lblIcon);
            item.Controls.Add(lblTask);
            item.Controls.Add(lblProject);
            item.Controls.Add(lblTime);

            return item;
        }

        private async System.Threading.Tasks.Task LoadProjectProgressOverview()
        {
            try
            {
                var progressFlow = FindControlByName(pnlOverview, "flowProgressOverview") as FlowLayoutPanel;
                if (progressFlow == null) return;

                progressFlow.Controls.Clear();

                var projects = await _context.Projects
                    .Where(p => p.IsArchived != true)
                    .Include(p => p.Tasks.Where(t => t.IsDeleted != true))
                    .Take(6)  // Only show top 6 in overview
                    .ToListAsync();

                foreach (var project in projects)
                {
                    var totalTasks = project.Tasks.Count();
                    var completedTasks = project.Tasks.Count(t => t.Status == "Completed");
                    var percentage = totalTasks > 0 ? (int)((double)completedTasks / totalTasks * 100) : 0;

                    var progressItem = CreateProgressItemCompact(
                        project.ProjectName ?? "Unnamed Project",
                        percentage,
                        completedTasks,
                        totalTasks
                    );

                    progressFlow.Controls.Add(progressItem);
                }
            }
            catch (Exception ex)
            {
                // Silently handle errors
            }
        }

        private Panel CreateProgressItemCompact(string projectName, int percentage, int completed, int total)
        {
            Panel container = new Panel
            {
                Size = new Size(490, 50),
                BackColor = Color.FromArgb(40, 40, 40),
                Margin = new Padding(0, 3, 0, 3),
                Padding = new Padding(12)
            };

            // Project name
            Label lblName = new Label
            {
                Text = TruncateText(projectName, 25),
                Location = new Point(12, 8),
                Size = new Size(300, 18),
                Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };

            // Percentage
            Label lblPercentage = new Label
            {
                Text = $"{percentage}%",
                Location = new Point(420, 8),
                Size = new Size(60, 18),
                Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = GetProgressColor(percentage),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Progress bar background
            Panel progressBg = new Panel
            {
                Location = new Point(12, 30),
                Size = new Size(466, 6),
                BackColor = Color.FromArgb(60, 60, 60)
            };

            // Progress bar fill
            Panel progressFill = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size((int)(466 * percentage / 100.0), 6),
                BackColor = GetProgressColor(percentage)
            };
            progressBg.Controls.Add(progressFill);

            // Task count (smaller text below bar)
            Label lblCount = new Label
            {
                Text = $"{completed}/{total} tasks",
                Location = new Point(12, 38),
                Size = new Size(100, 10),
                Font = new Font("Arial", 7F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(140, 140, 140),
                BackColor = Color.Transparent
            };

            container.Controls.Add(lblName);
            container.Controls.Add(lblPercentage);
            container.Controls.Add(progressBg);
            container.Controls.Add(lblCount);

            return container;
        }
    }
}
