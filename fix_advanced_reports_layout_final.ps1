# ? AUTO FIX ADVANCED REPORTS LAYOUT - THEO ?NH
# Run: pwsh .\fix_advanced_reports_layout_final.ps1

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "FIX ADVANCED REPORTS - LAYOUT 2 COLUMNS" -ForegroundColor Cyan  
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$filePath = "ToDoList.GUI\Forms\AdvancedReportsForm.cs"

if (-not (Test-Path $filePath)) {
    Write-Host "[ERROR] File not found: $filePath" -ForegroundColor Red
    Write-Host "Creating new file..." -ForegroundColor Yellow
    
    # T?o file m?i n?u không t?n t?i
    $newContent = @'
// AUTO-GENERATED ADVANCED REPORTS FORM - LAYOUT 2 COLUMNS
using System;
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
        private TabControl tabControl;

        public AdvancedReportsForm(ToDoListContext context)
        {
            _context = context;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Thong ke nang cao - ToDoList Analytics";
            this.Size = new Size(1300, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // Tab Control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point)
            };

            CreateOverviewTab();

            this.Controls.Add(tabControl);
        }

        private void CreateOverviewTab()
        {
            TabPage tabOverview = new TabPage("Tong quan")
            {
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(20)
            };

            // ========================================
            // ROW 1: STATS CARDS (4 cards horizontal)
            // ========================================
            FlowLayoutPanel flowStatsRow = new FlowLayoutPanel
            {
                Location = new Point(20, 20),
                Size = new Size(1240, 200),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            // Get stats from database
            var totalProjects = _context.Projects.Count(p => p.IsArchived != true);
            var totalTasks = _context.Tasks.Count(t => t.IsDeleted != true);
            var completedTasks = _context.Tasks.Count(t => t.IsDeleted != true && t.Status == "Completed");
            var completion = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;
            var totalHours = _context.Tasks.Where(t => t.IsDeleted != true).Sum(t => t.EstimatedMinutes ?? 0) / 60.0;

            flowStatsRow.Controls.Add(CreateStatCard("Tong Projects", totalProjects.ToString(), "projects", Color.FromArgb(65, 105, 225)));
            flowStatsRow.Controls.Add(CreateStatCard("Tong Tasks", totalTasks.ToString(), "tasks", Color.FromArgb(46, 204, 113)));
            flowStatsRow.Controls.Add(CreateStatCard("Hoan thanh", $"{completion:F1}%", "completion", Color.FromArgb(155, 89, 182)));
            flowStatsRow.Controls.Add(CreateStatCard("Thoi gian", $"{totalHours:F1}h", "hours", Color.FromArgb(241, 196, 15)));

            // ========================================
            // ROW 2: 2 COLUMNS (Activity + Progress)
            // ========================================
            Panel pnlTwoColumns = new Panel
            {
                Location = new Point(20, 240),
                Size = new Size(1240, 450),
                BackColor = Color.Transparent
            };

            // LEFT: Activity Section
            Panel pnlActivity = CreateActivitySection();
            pnlActivity.Location = new Point(0, 0);
            pnlActivity.Size = new Size(600, 450);

            // RIGHT: Progress Section
            Panel pnlProgress = CreateProgressSection();
            pnlProgress.Location = new Point(620, 0);
            pnlProgress.Size = new Size(620, 450);

            pnlTwoColumns.Controls.Add(pnlActivity);
            pnlTwoColumns.Controls.Add(pnlProgress);

            tabOverview.Controls.Add(flowStatsRow);
            tabOverview.Controls.Add(pnlTwoColumns);

            tabControl.TabPages.Add(tabOverview);
        }

        private Panel CreateStatCard(string title, string value, string subtitle, Color iconColor)
        {
            Panel card = new Panel
            {
                Size = new Size(295, 180),
                BackColor = Color.FromArgb(35, 35, 35),
                Margin = new Padding(5),
                Padding = new Padding(20)
            };

            // Icon box
            Panel iconBox = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(60, 60),
                BackColor = iconColor
            };

            string emoji = title.Contains("Project") ? "??" :
                          title.Contains("Task") ? "??" :
                          title.Contains("thanh") ? "?" : "??";

            Label lblIcon = new Label
            {
                Text = emoji,
                Font = new Font("Segoe UI Emoji", 24F),
                ForeColor = Color.White,
                Size = new Size(60, 60),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            iconBox.Controls.Add(lblIcon);

            // Title
            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(95, 25),
                Size = new Size(180, 25),
                Font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.Transparent
            };

            // Value
            Label lblValue = new Label
            {
                Text = value,
                Location = new Point(20, 95),
                Size = new Size(200, 45),
                Font = new Font("Arial", 32F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Subtitle
            Label lblSubtitle = new Label
            {
                Text = subtitle,
                Location = new Point(210, 115),
                Size = new Size(75, 20),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            card.Controls.Add(iconBox);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblSubtitle);

            return card;
        }

        private Panel CreateActivitySection()
        {
            Panel section = new Panel
            {
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20)
            };

            Label lblHeader = new Label
            {
                Text = "?? Hoat dong gan day",
                Location = new Point(20, 15),
                Size = new Size(560, 30),
                Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            FlowLayoutPanel flowTasks = new FlowLayoutPanel
            {
                Location = new Point(20, 60),
                Size = new Size(560, 370),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            // Load recent tasks from database
            var recentTasks = _context.Tasks
                .Where(t => t.IsDeleted != true)
                .OrderByDescending(t => t.CreatedAt)
                .Take(8)
                .Select(t => new {
                    Title = t.Title,
                    Project = t.Project.ProjectName,
                    Time = EF.Functions.DateDiffDay(t.CreatedAt, DateTime.Now) + "d"
                })
                .ToList();

            foreach (var task in recentTasks)
            {
                flowTasks.Controls.Add(CreateTaskItem(
                    task.Title.Length > 30 ? task.Title.Substring(0, 27) + "..." : task.Title,
                    task.Project.Length > 10 ? task.Project.Substring(0, 10) + "..." : task.Project,
                    task.Time
                ));
            }

            section.Controls.Add(lblHeader);
            section.Controls.Add(flowTasks);

            return section;
        }

        private Panel CreateTaskItem(string taskName, string project, string time)
        {
            Panel item = new Panel
            {
                Size = new Size(540, 45),
                BackColor = Color.FromArgb(40, 40, 40),
                Margin = new Padding(0, 2, 0, 2),
                Padding = new Padding(10)
            };

            Label lblIcon = new Label
            {
                Text = "?",
                Location = new Point(10, 12),
                Size = new Size(20, 20),
                Font = new Font("Arial", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 100),
                BackColor = Color.Transparent
            };

            Label lblTask = new Label
            {
                Text = taskName,
                Location = new Point(40, 12),
                Size = new Size(300, 20),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };

            Label lblProject = new Label
            {
                Text = project,
                Location = new Point(350, 12),
                Size = new Size(100, 20),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblTime = new Label
            {
                Text = time,
                Location = new Point(460, 12),
                Size = new Size(60, 20),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point),
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

        private Panel CreateProgressSection()
        {
            Panel section = new Panel
            {
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20)
            };

            Label lblHeader = new Label
            {
                Text = "?? Tien do theo du an",
                Location = new Point(20, 15),
                Size = new Size(580, 30),
                Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            FlowLayoutPanel flowProgress = new FlowLayoutPanel
            {
                Location = new Point(20, 60),
                Size = new Size(580, 370),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent
            };

            // Load projects with progress
            var projects = _context.Projects
                .Where(p => p.IsArchived != true)
                .Select(p => new {
                    Name = p.ProjectName,
                    Total = p.Tasks.Count(t => t.IsDeleted != true),
                    Completed = p.Tasks.Count(t => t.IsDeleted != true && t.Status == "Completed")
                })
                .ToList();

            foreach (var proj in projects)
            {
                int progress = proj.Total > 0 ? (int)((double)proj.Completed / proj.Total * 100) : 0;
                flowProgress.Controls.Add(CreateProgressItem(proj.Name, progress, $"{proj.Completed}/{proj.Total}"));
            }

            section.Controls.Add(lblHeader);
            section.Controls.Add(flowProgress);

            return section;
        }

        private Panel CreateProgressItem(string projectName, int progress, string tasks)
        {
            Panel item = new Panel
            {
                Size = new Size(560, 70),
                BackColor = Color.FromArgb(40, 40, 40),
                Margin = new Padding(0, 5, 0, 5),
                Padding = new Padding(15)
            };

            Label lblName = new Label
            {
                Text = projectName,
                Location = new Point(15, 10),
                Size = new Size(400, 22),
                Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label lblPercent = new Label
            {
                Text = $"{progress}%",
                Location = new Point(480, 10),
                Size = new Size(65, 22),
                Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = progress == 0 ? Color.FromArgb(231, 76, 60) :
                           progress >= 66 ? Color.FromArgb(241, 196, 15) :
                           Color.FromArgb(230, 126, 34),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            Panel barBg = new Panel
            {
                Location = new Point(15, 40),
                Size = new Size(530, 8),
                BackColor = Color.FromArgb(60, 60, 60)
            };

            Panel barFill = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size((int)(530 * progress / 100.0), 8),
                BackColor = progress == 0 ? Color.FromArgb(231, 76, 60) :
                           progress >= 66 ? Color.FromArgb(241, 196, 15) :
                           Color.FromArgb(230, 126, 34)
            };
            barBg.Controls.Add(barFill);

            Label lblTasks = new Label
            {
                Text = $"{tasks} tasks",
                Location = new Point(15, 52),
                Size = new Size(100, 16),
                Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent
            };

            item.Controls.Add(lblName);
            item.Controls.Add(lblPercent);
            item.Controls.Add(barBg);
            item.Controls.Add(lblTasks);

            return item;
        }

        private void LoadData()
        {
            // Data is loaded in CreateOverviewTab
        }
    }
}
'@
    
    Set-Content -Path $filePath -Value $newContent -Encoding UTF8
    Write-Host "[OK] Created new file!" -ForegroundColor Green
}
else {
    Write-Host "[1] Backing up..." -ForegroundColor Yellow
    Copy-Item $filePath "$filePath.backup" -Force
    Write-Host "[OK] Backup saved" -ForegroundColor Green
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "COMPLETED!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Build Solution (Ctrl+Shift+B)" -ForegroundColor White
Write-Host "2. Run (F5)" -ForegroundColor White
Write-Host "3. Open 'Thong ke nang cao'" -ForegroundColor White
Write-Host ""
Write-Host "Expected layout:" -ForegroundColor Cyan
Write-Host "- 4 stats cards horizontally" -ForegroundColor White
Write-Host "- 2 columns below: Activity | Progress" -ForegroundColor White
Write-Host "- Black background with dark cards" -ForegroundColor White
Write-Host ""
pause
