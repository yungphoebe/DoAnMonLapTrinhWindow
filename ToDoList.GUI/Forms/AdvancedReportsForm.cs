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
            this.Text = "Th?ng kê nâng cao - ToDoList Analytics";
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
                Text = "?? Th?ng kê nâng cao - ToDoList Analytics",
                Location = new Point(20, 20),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Button btnClose = new Button
            {
                Text = "?",
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

            // Tab 1: T?ng quan
            TabPage tabOverview = new TabPage("T?ng quan")
            {
                BackColor = Color.FromArgb(20, 20, 20)
            };
            CreateOverviewTab(tabOverview);

            // Tab 2: N?ng su?t
            TabPage tabProductivity = new TabPage("N?ng su?t")
            {
                BackColor = Color.FromArgb(20, 20, 20)
            };
            CreateProductivityTab(tabProductivity);

            // Tab 3: Phân tích d? án
            TabPage tabProjectAnalysis = new TabPage("Phân tích d? án")
            {
                BackColor = Color.FromArgb(20, 20, 20)
            };
            CreateProjectAnalysisTab(tabProjectAnalysis);

            // Tab 4: Phân tích th?i gian
            TabPage tabTimeAnalysis = new TabPage("Phân tích th?i gian")
            {
                BackColor = Color.FromArgb(20, 20, 20)
            };
            CreateTimeAnalysisTab(tabTimeAnalysis);

            tabControl.TabPages.Add(tabOverview);
            tabControl.TabPages.Add(tabProductivity);
            tabControl.TabPages.Add(tabProjectAnalysis);
            tabControl.TabPages.Add(tabTimeAnalysis);

            this.Controls.Add(tabControl);
        }

        private void CreateOverviewTab(TabPage tab)
        {
            Panel container = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            // ===== TOP 4 STATISTICS BOXES =====
            Panel pnlTopStats = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1100, 130),  // ? Gi?m width t? 1120 xu?ng 1100, height t? 140 xu?ng 130
                BackColor = Color.Transparent
            };

            // Box 1: T?ng Projects
            var box1 = CreateStatBox(
                "?? T?ng Projects",
                "0",
                "projects",
                Color.FromArgb(100, 149, 237),
                new Point(0, 0)
            );

            // Box 2: T?ng Tasks  // ? Gi?m kho?ng cách t? 290 xu?ng 275
            var box2 = CreateStatBox(
                "?? T?ng Tasks",
                "0",
                "tasks",
                Color.FromArgb(46, 204, 113),
                new Point(275, 0)
            );

            // Box 3: Hoàn thành  // ? ?i?u ch?nh v? trí
            var box3 = CreateStatBox(
                "? Hoàn thành",
                "0%",
                "completion",
                Color.FromArgb(155, 89, 182),
                new Point(550, 0)
            );

            // Box 4: Th?i gian  // ? ?i?u ch?nh v? trí
            var box4 = CreateStatBox(
                "?? Th?i gian",
                "0h",
                "hours",
                Color.FromArgb(241, 196, 15),
                new Point(825, 0)
            );

            pnlTopStats.Controls.Add(box1);
            pnlTopStats.Controls.Add(box2);
            pnlTopStats.Controls.Add(box3);
            pnlTopStats.Controls.Add(box4);

            // Store references for updating
            box1.Tag = "totalProjects";
            box2.Tag = "totalTasks";
            box3.Tag = "completionRate";
            box4.Tag = "totalTime";

            // ===== ACTIVITY SECTION =====
            Panel pnlActivity = new Panel
            {
                Location = new Point(20, 180),
                Size = new Size(540, 400),
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20)
            };

            Label lblActivityTitle = new Label
            {
                Text = "?? Ho?t ??ng g?n ?ây",
                Location = new Point(20, 15),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            FlowLayoutPanel flowActivity = new FlowLayoutPanel
            {
                Location = new Point(20, 55),
                Size = new Size(500, 325),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent
            };
            flowActivity.Name = "flowActivity";

            pnlActivity.Controls.Add(lblActivityTitle);
            pnlActivity.Controls.Add(flowActivity);

            // ===== PROGRESS SECTION =====
            Panel pnlProgress = new Panel
            {
                Location = new Point(580, 180),
                Size = new Size(540, 400),
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20)
            };

            Label lblProgressTitle = new Label
            {
                Text = "?? Ti?n ?? theo d? án",
                Location = new Point(20, 15),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Panel pnlProgressBars = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(500, 325),
                AutoScroll = true,
                BackColor = Color.Transparent
            };
            pnlProgressBars.Name = "pnlProgressBars";

            pnlProgress.Controls.Add(lblProgressTitle);
            pnlProgress.Controls.Add(pnlProgressBars);

            container.Controls.Add(pnlTopStats);
            container.Controls.Add(pnlActivity);
            container.Controls.Add(pnlProgress);
            tab.Controls.Add(container);
        }

        private Panel CreateStatBox(string title, string value, string subtitle, Color color, Point location)
        {
            Panel box = new Panel
            {
                Location = location,
                Size = new Size(260, 120),  // ? Gi?m t? 270x130 xu?ng 260x120
                BackColor = Color.FromArgb(35, 35, 35),
                Cursor = Cursors.Hand
            };

            // Icon background with color
            Panel iconBg = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(45, 45),  // ? T?ng t? 50x50 lên 45x45
                BackColor = color
            };

            Label lblIcon = new Label
            {
                Text = title.Split(' ')[0], // Get emoji
                Location = new Point(0, 0),
                Size = new Size(45, 45),  // ? Match with iconBg
                Font = new Font("Segoe UI", 18F),  // ? Gi?m t? 20F xu?ng 18F
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };
            iconBg.Controls.Add(lblIcon);

            // Title
            Label lblTitle = new Label
            {
                Text = title.Substring(title.IndexOf(' ') + 1), // Remove emoji
                Location = new Point(70, 18),  // ? ?i?u ch?nh X t? 85 xu?ng 70
                Size = new Size(175, 22),  // ? Gi?m width và height
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),  // ? Gi?m t? 10F xu?ng 9F
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.Transparent
            };

            // Value
            Label lblValue = new Label
            {
                Text = value,
                Location = new Point(15, 70),  // ? ?i?u ch?nh Y t? 80 xu?ng 70
                Size = new Size(160, 30),  // ? Gi?m height t? 35 xu?ng 30
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),  // ? Gi?m t? 22F xu?ng 18F
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false  // ? Prevent auto-sizing
            };
            lblValue.Name = "value";

            // Subtitle
            Label lblSubtitle = new Label
            {
                Text = subtitle,
                Location = new Point(180, 80),  // ? ?i?u ch?nh v? trí
                Size = new Size(70, 18),  // ? Gi?m height
                Font = new Font("Segoe UI", 7F),  // ? Gi?m t? 8F xu?ng 7F
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            box.Controls.Add(iconBg);
            box.Controls.Add(lblTitle);
            box.Controls.Add(lblValue);
            box.Controls.Add(lblSubtitle);

            // Hover effect
            box.MouseEnter += (s, e) => box.BackColor = Color.FromArgb(45, 45, 45);
            box.MouseLeave += (s, e) => box.BackColor = Color.FromArgb(35, 35, 35);

            // Border
            box.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, box.Width - 1, box.Height - 1);
                using (var pen = new Pen(Color.FromArgb(50, 50, 50), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            return box;
        }

        private void CreateProductivityTab(TabPage tab)
        {
            Label lbl = new Label
            {
                Text = "?? Bi?u ?? n?ng su?t ?ang ???c phát tri?n...",
                Location = new Point(50, 50),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            tab.Controls.Add(lbl);
        }

        private void CreateProjectAnalysisTab(TabPage tab)
        {
            Label lbl = new Label
            {
                Text = "?? Phân tích d? án ?ang ???c phát tri?n...",
                Location = new Point(50, 50),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            tab.Controls.Add(lbl);
        }

        private void CreateTimeAnalysisTab(TabPage tab)
        {
            Label lbl = new Label
            {
                Text = "? Phân tích th?i gian ?ang ???c phát tri?n...",
                Location = new Point(50, 50),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            tab.Controls.Add(lbl);
        }

        private async void LoadStatistics()
        {
            try
            {
                // Load data from database
                var totalProjects = await _context.Projects.CountAsync(p => p.IsArchived != true);
                var totalTasks = await _context.Tasks.CountAsync(t => t.IsDeleted != true);
                var completedTasks = await _context.Tasks.CountAsync(t => t.IsDeleted != true && t.Status == "Completed");
                var totalMinutes = await _context.Tasks
                    .Where(t => t.IsDeleted != true && t.ActualMinutes.HasValue)
                    .SumAsync(t => t.ActualMinutes.Value);

                var completionRate = totalTasks > 0 ? (completedTasks * 100.0 / totalTasks) : 0;
                var totalHours = totalMinutes / 60.0;

                // Update stat boxes
                UpdateStatBox("totalProjects", totalProjects.ToString(), "projects");
                UpdateStatBox("totalTasks", totalTasks.ToString(), "tasks");
                UpdateStatBox("completionRate", $"{completionRate:F1}%", "completion rate");
                UpdateStatBox("totalTime", $"{totalHours:F1}h", "total hours");

                // Load recent activity
                await LoadRecentActivity();

                // Load project progress
                await LoadProjectProgress();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i khi t?i th?ng kê: {ex.Message}", "L?i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatBox(string tag, string value, string subtitle)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TabControl tc)
                {
                    foreach (TabPage tp in tc.TabPages)
                    {
                        var box = FindControlByTag(tp, tag);
                        if (box != null)
                        {
                            var lblValue = box.Controls.Find("value", false).FirstOrDefault() as Label;
                            if (lblValue != null)
                            {
                                // ? Truncate long values to prevent overflow
                                lblValue.Text = TruncateValue(value, 12);
                            }
                        }
                    }
                }
            }
        }

        private string TruncateValue(string value, int maxLength)
        {
            if (value.Length <= maxLength)
                return value;
            return value.Substring(0, maxLength - 2) + "..";
        }

        private Control? FindControlByTag(Control parent, string tag)
        {
            if (parent.Tag?.ToString() == tag)
                return parent;

            foreach (Control child in parent.Controls)
            {
                var found = FindControlByTag(child, tag);
                if (found != null)
                    return found;
            }
            return null;
        }

        private async System.Threading.Tasks.Task LoadRecentActivity()
        {
            try
            {
                var recentTasks = await _context.Tasks
                    .Where(t => t.IsDeleted != true)
                    .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt)
                    .Take(8)
                    .Include(t => t.Project)
                    .ToListAsync();

                // Find the flow panel
                var flowActivity = FindControlByName(this, "flowActivity") as FlowLayoutPanel;
                if (flowActivity != null)
                {
                    flowActivity.Controls.Clear();

                    foreach (var task in recentTasks)
                    {
                        var activityItem = CreateActivityItem(task);
                        flowActivity.Controls.Add(activityItem);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading activity: {ex.Message}");
            }
        }

        private Panel CreateActivityItem(TodoListApp.DAL.Models.Task task)
        {
            Panel item = new Panel
            {
                Width = 480,
                Height = 32,  // ? Gi?m t? 35 xu?ng 32
                BackColor = Color.FromArgb(40, 40, 40),
                Margin = new Padding(0, 2, 0, 2)  // ? Gi?m margin
            };

            // Status icon
            string icon = task.Status == "Completed" ? "?" : "?";
            Color iconColor = task.Status == "Completed" ? Color.FromArgb(46, 204, 113) : Color.FromArgb(241, 196, 15);

            Label lblIcon = new Label
            {
                Text = icon,
                Location = new Point(8, 6),  // ? ?i?u ch?nh v? trí
                Size = new Size(18, 18),  // ? Gi?m size
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),  // ? Gi?m font
                ForeColor = iconColor,
                BackColor = Color.Transparent
            };

            // Task title
            Label lblTitle = new Label
            {
                Text = TruncateText(task.Title, 35),  // ? Thêm truncate
                Location = new Point(35, 7),  // ? ?i?u ch?nh v? trí
                Size = new Size(260, 18),  // ? Gi?m height
                Font = new Font("Segoe UI", 8F),  // ? Gi?m font t? 9F xu?ng 8F
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };

            // Project name
            Label lblProject = new Label
            {
                Text = TruncateText(task.Project?.ProjectName ?? "N/A", 12),  // ? Thêm truncate
                Location = new Point(305, 7),  // ? ?i?u ch?nh v? trí
                Size = new Size(95, 18),  // ? Gi?m width
                Font = new Font("Segoe UI", 7F),  // ? Gi?m font t? 8F xu?ng 7F
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Time
            var timeAgo = GetTimeAgo(task.UpdatedAt ?? task.CreatedAt);
            Label lblTime = new Label
            {
                Text = timeAgo,
                Location = new Point(410, 7),  // ? ?i?u ch?nh v? trí
                Size = new Size(60, 18),  // ? T?ng width
                Font = new Font("Segoe UI", 7F),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            item.Controls.Add(lblIcon);
            item.Controls.Add(lblTitle);
            item.Controls.Add(lblProject);
            item.Controls.Add(lblTime);

            return item;
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;
            return text.Substring(0, maxLength - 2) + "..";
        }

        private async System.Threading.Tasks.Task LoadProjectProgress()
        {
            try
            {
                var projects = await _context.Projects
                    .Where(p => p.IsArchived != true)
                    .Include(p => p.Tasks)
                    .ToListAsync();

                var pnlProgressBars = FindControlByName(this, "pnlProgressBars") as Panel;
                if (pnlProgressBars != null)
                {
                    pnlProgressBars.Controls.Clear();

                    int y = 10;
                    foreach (var project in projects.Take(6))
                    {
                        var totalTasks = project.Tasks?.Count(t => t.IsDeleted != true) ?? 0;
                        var completedTasks = project.Tasks?.Count(t => t.IsDeleted != true && t.Status == "Completed") ?? 0;
                        var percentage = totalTasks > 0 ? (completedTasks * 100 / totalTasks) : 0;

                        var progressItem = CreateProgressBar(project.ProjectName ?? "Unknown", percentage, totalTasks, completedTasks, y);
                        pnlProgressBars.Controls.Add(progressItem);
                        y += 48;  // ? Gi?m t? 55 xu?ng 48
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading progress: {ex.Message}");
            }
        }

        private Panel CreateProgressBar(string projectName, int percentage, int total, int completed, int yPosition)
        {
            Panel container = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(480, 42),  // ? Gi?m height t? 45 xu?ng 42
                BackColor = Color.Transparent
            };

            // Project name
            Label lblName = new Label
            {
                Text = TruncateText(projectName, 30),  // ? Thêm truncate
                Location = new Point(0, 0),
                Size = new Size(320, 18),  // ? Gi?m height
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),  // ? Gi?m t? 9F xu?ng 8F
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };

            // Percentage
            Label lblPercentage = new Label
            {
                Text = $"{percentage}%",
                Location = new Point(420, 0),
                Size = new Size(60, 18),  // ? Gi?m height
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),  // ? Gi?m t? 9F xu?ng 8F
                ForeColor = GetProgressColor(percentage),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Progress bar background
            Panel progressBg = new Panel
            {
                Location = new Point(0, 22),  // ? ?i?u ch?nh Y t? 25 xu?ng 22
                Size = new Size(480, 6),  // ? Gi?m height t? 8 xu?ng 6
                BackColor = Color.FromArgb(50, 50, 50)
            };

            // Progress bar fill
            Panel progressFill = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size((int)(480 * percentage / 100.0), 6),  // ? Gi?m height t? 8 xu?ng 6
                BackColor = GetProgressColor(percentage)
            };
            progressBg.Controls.Add(progressFill);

            // Task count
            Label lblCount = new Label
            {
                Text = $"{completed}/{total} tasks",
                Location = new Point(0, 30),  // ? ?i?u ch?nh Y t? 35 xu?ng 30
                Size = new Size(150, 12),  // ? Gi?m height t? 15 xu?ng 12
                Font = new Font("Segoe UI", 7F),  // ? Font size không ??i
                ForeColor = Color.FromArgb(150, 150, 150),
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

        private string GetTimeAgo(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return "N/A";

            var timeSpan = DateTime.Now - dateTime.Value;
            
            if (timeSpan.TotalMinutes < 1) return "now";
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
    }
}
