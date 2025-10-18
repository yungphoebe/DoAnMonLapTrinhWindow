using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using ToDoList.GUI.Components;

namespace ToDoList.GUI.Forms
{
    public partial class AdvancedReportsForm : Form
    {
        private ToDoListContext _context;
        private TabControl tabControl;
        
        // Cache data to avoid concurrent database access
        private List<ProjectProgressData> _cachedProjectData;
        private List<PriorityData> _cachedPriorityData;
        private List<WeeklyData> _cachedWeeklyData;

        public AdvancedReportsForm(ToDoListContext context)
        {
            _context = context;
            InitializeComponent();
            LoadData();
        }
        
        // Helper classes for cached data
        private class ProjectProgressData
        {
            public string Name { get; set; }
            public double Progress { get; set; }
            public Color Color { get; set; }
        }
        
        private class PriorityData
        {
            public string Priority { get; set; }
            public int TotalMinutes { get; set; }
        }
        
        private class WeeklyData
        {
            public string WeekLabel { get; set; }
            public int Tasks { get; set; }
            public double Hours { get; set; }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1400, 900);
            this.Text = "?? Th?ng kê nâng cao - ToDoList Analytics";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 20);

            CreateTabControl();
        }

        private void CreateTabControl()
        {
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            // Tab 1: Overview
            var tabOverview = new TabPage("?? T?ng quan")
            {
                BackColor = Color.FromArgb(25, 25, 25)
            };
            CreateOverviewTab(tabOverview);

            // Tab 2: Productivity
            var tabProductivity = new TabPage("? N?ng su?t")
            {
                BackColor = Color.FromArgb(25, 25, 25)
            };
            CreateProductivityTab(tabProductivity);

            // Tab 3: Projects Analysis
            var tabProjects = new TabPage("?? Phân tích d? án")
            {
                BackColor = Color.FromArgb(25, 25, 25)
            };
            CreateProjectsTab(tabProjects);

            // Tab 4: Time Analysis
            var tabTime = new TabPage("? Phân tích th?i gian")
            {
                BackColor = Color.FromArgb(25, 25, 25)
            };
            CreateTimeAnalysisTab(tabTime);

            tabControl.TabPages.Add(tabOverview);
            tabControl.TabPages.Add(tabProductivity);
            tabControl.TabPages.Add(tabProjects);
            tabControl.TabPages.Add(tabTime);

            this.Controls.Add(tabControl);
        }

        private void CreateOverviewTab(TabPage tab)
        {
            // Summary cards
            var pnlSummary = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1320, 150),
                BackColor = Color.Transparent
            };

            var summaryCards = new[]
            {
                new { Title = "T?ng Projects", Icon = "??", Value = "0", Color = Color.FromArgb(100, 149, 237) },
                new { Title = "T?ng Tasks", Icon = "?", Value = "0", Color = Color.FromArgb(76, 175, 80) },
                new { Title = "Hoàn thành", Icon = "??", Value = "0%", Color = Color.FromArgb(156, 39, 176) },
                new { Title = "Th?i gian", Icon = "??", Value = "0h", Color = Color.FromArgb(255, 152, 0) }
            };

            for (int i = 0; i < summaryCards.Length; i++)
            {
                var card = CreateSummaryCard(summaryCards[i].Title, summaryCards[i].Icon, 
                    summaryCards[i].Value, summaryCards[i].Color);
                card.Location = new Point(i * 320, 0);
                card.Name = $"card{i}";
                pnlSummary.Controls.Add(card);
            }

            // Recent activity
            var pnlActivity = new Panel
            {
                Location = new Point(20, 190),
                Size = new Size(640, 400),
                BackColor = Color.FromArgb(35, 35, 35)
            };

            var lblActivityTitle = new Label
            {
                Text = "?? Ho?t ??ng g?n ?ây",
                Location = new Point(20, 15),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            var listActivity = new ListBox
            {
                Location = new Point(20, 50),
                Size = new Size(600, 330),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F),
                Name = "listActivity"
            };

            pnlActivity.Controls.Add(lblActivityTitle);
            pnlActivity.Controls.Add(listActivity);

            // Progress chart
            var pnlProgress = new Panel
            {
                Location = new Point(680, 190),
                Size = new Size(660, 400),
                BackColor = Color.FromArgb(35, 35, 35),
                Name = "pnlProgress"
            };

            var lblProgressTitle = new Label
            {
                Text = "?? Ti?n ?? theo d? án",
                Location = new Point(20, 15),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            pnlProgress.Controls.Add(lblProgressTitle);
            pnlProgress.Paint += (s, e) => DrawProjectProgressChart(e.Graphics, pnlProgress.Size);

            tab.Controls.Add(pnlSummary);
            tab.Controls.Add(pnlActivity);
            tab.Controls.Add(pnlProgress);
        }

        private void CreateProductivityTab(TabPage tab)
        {
            var lblTitle = new Label
            {
                Text = "? Phân tích n?ng su?t làm vi?c",
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Add Interactive Charts button
            var btnInteractiveCharts = new Button
            {
                Text = "?? Interactive Charts",
                Location = new Point(880, 20),
                Size = new Size(180, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(156, 39, 176),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnInteractiveCharts.FlatAppearance.BorderSize = 0;
            btnInteractiveCharts.Click += BtnInteractiveCharts_Click;

            // Add Python Charts button
            var btnPythonCharts = new Button
            {
                Text = "?? Python Charts",
                Location = new Point(1070, 20),
                Size = new Size(150, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(100, 149, 237),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPythonCharts.FlatAppearance.BorderSize = 0;
            btnPythonCharts.Click += BtnPythonCharts_Click;
            
            tab.Controls.Add(btnInteractiveCharts);

            // Productivity metrics
            var pnlMetrics = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(1320, 200),
                BackColor = Color.Transparent,
                Name = "pnlProductivityMetrics"
            };

            // Add productivity cards
            CreateProductivityCards(pnlMetrics);

            // Daily productivity chart
            var pnlDailyChart = new Panel
            {
                Location = new Point(20, 290),
                Size = new Size(1320, 400),
                BackColor = Color.FromArgb(35, 35, 35),
                Name = "pnlDailyProductivity"
            };

            var lblDailyTitle = new Label
            {
                Text = "?? N?ng su?t hàng ngày (7 ngày g?n nh?t)",
                Location = new Point(20, 15),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            pnlDailyChart.Controls.Add(lblDailyTitle);
            pnlDailyChart.Paint += (s, e) => DrawDailyProductivityChart(e.Graphics, pnlDailyChart.Size);

            tab.Controls.Add(lblTitle);
            tab.Controls.Add(btnPythonCharts);
            tab.Controls.Add(pnlMetrics);
            tab.Controls.Add(pnlDailyChart);
        }

        private void CreateProjectsTab(TabPage tab)
        {
            var lblTitle = new Label
            {
                Text = "?? Phân tích chi ti?t theo d? án",
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Projects table
            var dgvProjects = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(1320, 500),
                BackgroundColor = Color.FromArgb(35, 35, 35),
                GridColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Name = "dgvProjects"
            };

            // Configure DataGridView style
            dgvProjects.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dgvProjects.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProjects.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvProjects.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvProjects.DefaultCellStyle.ForeColor = Color.White;
            dgvProjects.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 149, 237);
            dgvProjects.BorderStyle = BorderStyle.None;

            tab.Controls.Add(lblTitle);
            tab.Controls.Add(dgvProjects);
        }

        private void CreateTimeAnalysisTab(TabPage tab)
        {
            var lblTitle = new Label
            {
                Text = "? Phân tích th?i gian làm vi?c",
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Time distribution pie chart
            var pnlTimeDistribution = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(640, 400),
                BackColor = Color.FromArgb(35, 35, 35),
                Name = "pnlTimeDistribution"
            };

            var lblPieTitle = new Label
            {
                Text = "?? Phân b? th?i gian theo ?? ?u tiên",
                Location = new Point(20, 15),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            pnlTimeDistribution.Controls.Add(lblPieTitle);
            pnlTimeDistribution.Paint += (s, e) => DrawTimeDistributionPieChart(e.Graphics, pnlTimeDistribution.Size);

            // Weekly time trends
            var pnlWeeklyTrends = new Panel
            {
                Location = new Point(680, 70),
                Size = new Size(660, 400),
                BackColor = Color.FromArgb(35, 35, 35),
                Name = "pnlWeeklyTrends"
            };

            var lblWeeklyTitle = new Label
            {
                Text = "?? Xu h??ng th?i gian theo tu?n",
                Location = new Point(20, 15),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            pnlWeeklyTrends.Controls.Add(lblWeeklyTitle);
            pnlWeeklyTrends.Paint += (s, e) => DrawWeeklyTrendsChart(e.Graphics, pnlWeeklyTrends.Size);

            tab.Controls.Add(lblTitle);
            tab.Controls.Add(pnlTimeDistribution);
            tab.Controls.Add(pnlWeeklyTrends);
        }

        private Panel CreateSummaryCard(string title, string icon, string value, Color color)
        {
            var card = new Panel
            {
                Size = new Size(300, 120),
                BackColor = Color.FromArgb(35, 35, 35),
                Margin = new Padding(10)
            };

            var lblIcon = new Label
            {
                Text = icon,
                Location = new Point(20, 20),
                Size = new Size(30, 30),
                Font = new Font("Segoe UI", 20F),
                ForeColor = color,
                BackColor = Color.Transparent
            };

            var lblTitle = new Label
            {
                Text = title,
                Location = new Point(60, 20),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent
            };

            var lblValue = new Label
            {
                Text = value,
                Location = new Point(60, 45),
                Size = new Size(200, 35),
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        private async void LoadData()
        {
            try
            {
                await LoadOverviewData();
                await LoadProductivityData();
                await LoadProjectsData();
                await LoadChartData(); // Load and cache chart data
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i khi t?i d? li?u: {ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private async System.Threading.Tasks.Task LoadChartData()
        {
            // Load project progress data
            var projectsRaw = await _context.Projects
                .Where(p => p.IsArchived != true)
                .Select(p => new
                {
                    Name = p.ProjectName,
                    ColorCode = p.ColorCode,
                    TotalTasks = p.Tasks.Count(t => t.IsDeleted != true),
                    CompletedTasks = p.Tasks.Count(t => t.IsDeleted != true && t.Status == "Completed")
                })
                .Take(6)
                .ToListAsync();

            _cachedProjectData = projectsRaw.Select(p => new ProjectProgressData
            {
                Name = p.Name,
                Progress = p.TotalTasks > 0 ? (double)p.CompletedTasks / p.TotalTasks : 0,
                Color = !string.IsNullOrEmpty(p.ColorCode) ? 
                    ColorTranslator.FromHtml(p.ColorCode) : 
                    Color.FromArgb(100, 149, 237)
            }).ToList();

            // Load priority data
            var priorityRaw = await _context.Tasks
                .Where(t => t.IsDeleted != true && t.EstimatedMinutes.HasValue)
                .GroupBy(t => t.Priority)
                .Select(g => new
                {
                    Priority = g.Key ?? "Medium",
                    TotalMinutes = g.Sum(t => t.EstimatedMinutes.Value)
                })
                .ToListAsync();

            _cachedPriorityData = priorityRaw.Select(p => new PriorityData
            {
                Priority = p.Priority,
                TotalMinutes = p.TotalMinutes
            }).ToList();

            // Load weekly data
            var fourWeeksAgo = DateTime.Now.AddDays(-28);
            var weeklyRaw = await _context.Tasks
                .Where(t => t.IsDeleted != true && t.CreatedAt >= fourWeeksAgo)
                .GroupBy(t => new
                {
                    Year = t.CreatedAt.Value.Year,
                    Week = (t.CreatedAt.Value.DayOfYear - 1) / 7 + 1
                })
                .Select(g => new
                {
                    WeekNumber = g.Key.Week,
                    Tasks = g.Count(),
                    TotalMinutes = g.Where(t => t.EstimatedMinutes.HasValue).Sum(t => t.EstimatedMinutes.Value)
                })
                .OrderBy(w => w.WeekNumber)
                .Take(4)
                .ToListAsync();

            _cachedWeeklyData = weeklyRaw.Select(w => new WeeklyData
            {
                WeekLabel = $"Tuan {w.WeekNumber}",
                Tasks = w.Tasks,
                Hours = w.TotalMinutes / 60.0
            }).ToList();
            
            // Refresh panels after data loaded
            this.Invoke((MethodInvoker)delegate
            {
                var pnlProgress = tabControl.TabPages[0].Controls.OfType<Panel>()
                    .FirstOrDefault(p => p.Name == "pnlProgress");
                pnlProgress?.Invalidate();
                
                var pnlTimeDistribution = tabControl.TabPages[3].Controls.OfType<Panel>()
                    .FirstOrDefault(p => p.Name == "pnlTimeDistribution");
                pnlTimeDistribution?.Invalidate();
                
                var pnlWeeklyTrends = tabControl.TabPages[3].Controls.OfType<Panel>()
                    .FirstOrDefault(p => p.Name == "pnlWeeklyTrends");
                pnlWeeklyTrends?.Invalidate();
            });
        }

        private async System.Threading.Tasks.Task LoadOverviewData()
        {
            var totalProjects = await _context.Projects.CountAsync(p => p.IsArchived != true);
            var totalTasks = await _context.Tasks.CountAsync(t => t.IsDeleted != true);
            var completedTasks = await _context.Tasks.CountAsync(t => t.IsDeleted != true && t.Status == "Completed");
            var totalMinutes = await _context.Tasks
                .Where(t => t.IsDeleted != true && t.EstimatedMinutes.HasValue)
                .SumAsync(t => t.EstimatedMinutes.Value);

            var completionRate = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;
            var totalHours = totalMinutes / 60.0;

            // Update summary cards
            var summaryPanel = tabControl.TabPages[0].Controls.OfType<Panel>().FirstOrDefault();
            if (summaryPanel != null)
            {
                var cards = summaryPanel.Controls.OfType<Panel>().ToArray();
                if (cards.Length >= 4)
                {
                    UpdateCardValue(cards[0], totalProjects.ToString());
                    UpdateCardValue(cards[1], totalTasks.ToString());
                    UpdateCardValue(cards[2], $"{completionRate:F1}%");
                    UpdateCardValue(cards[3], $"{totalHours:F1}h");
                }
            }

            // Load recent activity
            var recentTasks = await _context.Tasks
                .Where(t => t.IsDeleted != true)
                .OrderByDescending(t => t.CreatedAt)
                .Take(10)
                .Include(t => t.Project)
                .ToListAsync();

            var listActivity = tabControl.TabPages[0].Controls
                .OfType<Panel>()
                .SelectMany(p => p.Controls.OfType<ListBox>())
                .FirstOrDefault(l => l.Name == "listActivity");

            if (listActivity != null)
            {
                listActivity.Items.Clear();
                foreach (var task in recentTasks)
                {
                    var status = task.Status == "Completed" ? "?" : task.Status == "In Progress" ? "??" : "?";
                    var projectName = task.Project?.ProjectName ?? "No Project";
                    listActivity.Items.Add($"{status} {task.Title} ({projectName}) - {task.CreatedAt:dd/MM HH:mm}");
                }
            }
        }

        private async System.Threading.Tasks.Task LoadProductivityData()
        {
            // Load daily productivity for last 7 days
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            var dailyStats = await _context.Tasks
                .Where(t => t.IsDeleted != true && t.CreatedAt >= sevenDaysAgo)
                .GroupBy(t => t.CreatedAt.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalTasks = g.Count(),
                    CompletedTasks = g.Count(t => t.Status == "Completed"),
                    TotalMinutes = g.Where(t => t.EstimatedMinutes.HasValue).Sum(t => t.EstimatedMinutes.Value)
                })
                .ToListAsync();

            // Store data for chart drawing
            this.Tag = dailyStats;
        }

        private async System.Threading.Tasks.Task LoadProjectsData()
        {
            var projectsData = await _context.Projects
                .Where(p => p.IsArchived != true)
                .Select(p => new
                {
                    ProjectName = p.ProjectName,
                    TotalTasks = p.Tasks.Count(t => t.IsDeleted != true),
                    CompletedTasks = p.Tasks.Count(t => t.IsDeleted != true && t.Status == "Completed"),
                    InProgressTasks = p.Tasks.Count(t => t.IsDeleted != true && t.Status == "In Progress"),
                    PendingTasks = p.Tasks.Count(t => t.IsDeleted != true && t.Status == "Pending"),
                    TotalMinutes = p.Tasks.Where(t => t.IsDeleted != true && t.EstimatedMinutes.HasValue)
                                         .Sum(t => t.EstimatedMinutes.Value),
                    CompletionRate = p.Tasks.Count(t => t.IsDeleted != true) > 0 ? 
                        (double)p.Tasks.Count(t => t.IsDeleted != true && t.Status == "Completed") / 
                        p.Tasks.Count(t => t.IsDeleted != true) * 100 : 0
                })
                .ToListAsync();

            var dgvProjects = tabControl.TabPages[2].Controls.OfType<DataGridView>()
                .FirstOrDefault(d => d.Name == "dgvProjects");

            if (dgvProjects != null)
            {
                dgvProjects.DataSource = projectsData.Select(p => new
                {
                    Du_an = p.ProjectName,
                    Tong_tasks = p.TotalTasks,
                    Hoan_thanh = p.CompletedTasks,
                    Dang_thuc_hien = p.InProgressTasks,
                    Cho_xu_ly = p.PendingTasks,
                    Thoi_gian_phut = p.TotalMinutes,
                    Ty_le_hoan_thanh = $"{p.CompletionRate:F1}%"
                }).ToList();
            }
        }

        private void UpdateCardValue(Panel card, string newValue)
        {
            var valueLabel = card.Controls.OfType<Label>()
                .FirstOrDefault(l => l.Font.Size > 20);
            if (valueLabel != null)
            {
                valueLabel.Text = newValue;
            }
        }

        private void DrawProjectProgressChart(Graphics g, Size size)
        {
            // Real project progress visualization
            g.Clear(Color.FromArgb(35, 35, 35));
            
            // Use cached data to avoid database access during Paint event
            if (_cachedProjectData == null || !_cachedProjectData.Any())
            {
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString("Dang tai du lieu...", 
                        new Font("Segoe UI", 12F), brush, 20, size.Height / 2);
                }
                return;
            }

            var realProjects = _cachedProjectData;

            var barHeight = 40;
            var startY = 60;
            var barWidth = size.Width - 100;

            for (int i = 0; i < realProjects.Count; i++)
            {
                var data = realProjects[i];
                var y = startY + i * (barHeight + 20);
                
                // Project name
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString(data.Name, new Font("Segoe UI", 10F), brush, 20, y - 20);
                }

                // Background bar
                using (var brush = new SolidBrush(Color.FromArgb(60, 60, 60)))
                {
                    g.FillRectangle(brush, 20, y, barWidth, barHeight);
                }

                // Progress bar
                var progressWidth = (int)(barWidth * data.Progress);
                using (var brush = new SolidBrush(data.Color))
                {
                    g.FillRectangle(brush, 20, y, progressWidth, barHeight);
                }

                // Progress text
                using (var brush = new SolidBrush(Color.White))
                {
                    var progressText = $"{data.Progress * 100:F0}%";
                    var textSize = g.MeasureString(progressText, new Font("Segoe UI", 9F, FontStyle.Bold));
                    g.DrawString(progressText, new Font("Segoe UI", 9F, FontStyle.Bold), brush, 
                        20 + progressWidth - textSize.Width - 10, y + (barHeight - textSize.Height) / 2);
                }
            }
        }

        private void DrawDailyProductivityChart(Graphics g, Size size)
        {
            g.Clear(Color.FromArgb(35, 35, 35));
            
            var dailyStats = this.Tag as List<dynamic>;
            if (dailyStats == null || !dailyStats.Any()) return;

            var chartArea = new Rectangle(60, 60, size.Width - 120, size.Height - 120);
            var maxTasks = dailyStats.Max(d => d.TotalTasks);
            if (maxTasks == 0) maxTasks = 1;

            var barWidth = chartArea.Width / dailyStats.Count - 10;

            for (int i = 0; i < dailyStats.Count; i++)
            {
                var data = dailyStats[i];
                var x = chartArea.X + i * (chartArea.Width / dailyStats.Count);
                var height = (int)(chartArea.Height * data.TotalTasks / (double)maxTasks);
                var y = chartArea.Bottom - height;

                // Draw bar
                using (var brush = new SolidBrush(Color.FromArgb(100, 149, 237)))
                {
                    g.FillRectangle(brush, x, y, barWidth, height);
                }

                // Draw date label
                using (var brush = new SolidBrush(Color.White))
                {
                    var dateText = ((DateTime)data.Date).ToString("dd/MM");
                    var textSize = g.MeasureString(dateText, new Font("Segoe UI", 8F));
                    g.DrawString(dateText, new Font("Segoe UI", 8F), brush, 
                        x + (barWidth - textSize.Width) / 2, chartArea.Bottom + 10);
                }

                // Draw value on top
                using (var brush = new SolidBrush(Color.White))
                {
                    var valueText = data.TotalTasks.ToString();
                    var textSize = g.MeasureString(valueText, new Font("Segoe UI", 9F, FontStyle.Bold));
                    g.DrawString(valueText, new Font("Segoe UI", 9F, FontStyle.Bold), brush,
                        x + (barWidth - textSize.Width) / 2, y - 25);
                }
            }
        }

        private void DrawTimeDistributionPieChart(Graphics g, Size size)
        {
            g.Clear(Color.FromArgb(35, 35, 35));

            // Use cached data
            if (_cachedPriorityData == null || !_cachedPriorityData.Any())
            {
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString("Dang tai du lieu...", 
                        new Font("Segoe UI", 12F), brush, 20, size.Height / 2);
                }
                return;
            }

            var realPriorityData = _cachedPriorityData;

            // Convert to chart data with colors
            var priorityColors = new Dictionary<string, Color>
            {
                { "High", Color.FromArgb(244, 67, 54) },
                { "Medium", Color.FromArgb(255, 152, 0) },
                { "Low", Color.FromArgb(76, 175, 80) }
            };

            var chartData = realPriorityData.Select(p => new
            {
                Label = $"{p.Priority} Priority",
                Value = p.TotalMinutes,
                Percentage = 0, // Will calculate below
                Color = priorityColors.ContainsKey(p.Priority) ? priorityColors[p.Priority] : Color.Gray
            }).ToArray();

            var totalMinutes = chartData.Sum(d => d.Value);
            for (int i = 0; i < chartData.Length; i++)
            {
                chartData[i] = new
                {
                    chartData[i].Label,
                    chartData[i].Value,
                    Percentage = (int)(chartData[i].Value * 100.0 / totalMinutes),
                    chartData[i].Color
                };
            }

            var centerX = size.Width / 2;
            var centerY = size.Height / 2;
            var radius = Math.Min(centerX, centerY) - 80;
            var rect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);

            float startAngle = 0;
            var total = chartData.Sum(d => d.Value);

            foreach (var data in chartData)
            {
                var sweepAngle = (float)(360.0 * data.Value / total);
                
                using (var brush = new SolidBrush(data.Color))
                {
                    g.FillPie(brush, rect, startAngle, sweepAngle);
                }

                // Draw label with real data
                var labelAngle = startAngle + sweepAngle / 2;
                var labelRadius = radius + 30;
                var labelX = centerX + (int)(labelRadius * Math.Cos(labelAngle * Math.PI / 180));
                var labelY = centerY + (int)(labelRadius * Math.Sin(labelAngle * Math.PI / 180));

                using (var brush = new SolidBrush(Color.White))
                {
                    var text = $"{data.Label}\n{data.Percentage}%\n({data.Value}ph)";
                    g.DrawString(text, new Font("Segoe UI", 8F), brush, labelX - 40, labelY - 20);
                }

                startAngle += sweepAngle;
            }
        }

        private void DrawWeeklyTrendsChart(Graphics g, Size size)
        {
            g.Clear(Color.FromArgb(35, 35, 35));

            // Use cached data
            if (_cachedWeeklyData == null || !_cachedWeeklyData.Any())
            {
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString("Dang tai du lieu...", 
                        new Font("Segoe UI", 12F), brush, 20, size.Height / 2);
                }
                return;
            }

            var weeklyData = _cachedWeeklyData.ToArray();

            var chartArea = new Rectangle(60, 60, size.Width - 120, size.Height - 120);
            var maxTasks = weeklyData.Length > 0 ? weeklyData.Max(w => w.Tasks) : 1;
            var maxHours = weeklyData.Length > 0 ? weeklyData.Max(w => w.Hours) : 1;

            var barWidth = chartArea.Width / Math.Max(weeklyData.Length, 1) - 20;

            for (int i = 0; i < weeklyData.Length; i++)
            {
                var data = weeklyData[i];
                var x = chartArea.X + i * (chartArea.Width / weeklyData.Length) + 10;
                
                // Tasks bar
                var tasksHeight = (int)(chartArea.Height * data.Tasks / maxTasks * 0.8);
                var tasksY = chartArea.Bottom - tasksHeight;
                using (var brush = new SolidBrush(Color.FromArgb(100, 149, 237)))
                {
                    g.FillRectangle(brush, x, tasksY, barWidth / 2 - 2, tasksHeight);
                }

                // Hours bar  
                var hoursHeight = (int)(chartArea.Height * data.Hours / maxHours * 0.8);
                var hoursY = chartArea.Bottom - hoursHeight;
                using (var brush = new SolidBrush(Color.FromArgb(255, 152, 0)))
                {
                    g.FillRectangle(brush, x + barWidth / 2 + 2, hoursY, barWidth / 2 - 2, hoursHeight);
                }

                // Week label
                using (var brush = new SolidBrush(Color.White))
                {
                    var textSize = g.MeasureString(data.WeekLabel, new Font("Segoe UI", 8F));
                    g.DrawString(data.WeekLabel, new Font("Segoe UI", 8F), brush,
                        x + (barWidth - textSize.Width) / 2, chartArea.Bottom + 10);
                }
                
                // Add values on bars
                if (data.Tasks > 0)
                {
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(data.Tasks.ToString(), new Font("Segoe UI", 8F, FontStyle.Bold), brush,
                            x + 2, tasksY - 15);
                    }
                }
                
                if (data.Hours > 0)
                {
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.DrawString($"{data.Hours:F1}h", new Font("Segoe UI", 8F, FontStyle.Bold), brush,
                            x + barWidth / 2 + 4, hoursY - 15);
                    }
                }
            }

            // Legend
            using (var brush = new SolidBrush(Color.FromArgb(100, 149, 237)))
            {
                g.FillRectangle(brush, 20, 20, 15, 15);
            }
            using (var brush = new SolidBrush(Color.White))
            {
                g.DrawString("Tasks", new Font("Segoe UI", 9F), brush, 40, 18);
            }

            using (var brush = new SolidBrush(Color.FromArgb(255, 152, 0)))
            {
                g.FillRectangle(brush, 100, 20, 15, 15);
            }
            using (var brush = new SolidBrush(Color.White))
            {
                g.DrawString("Hours", new Font("Segoe UI", 9F), brush, 120, 18);
            }
        }

        private void CreateProductivityCards(Panel pnlMetrics)
        {
            try
            {
                // Calculate productivity metrics
                var today = DateTime.Now.Date;
                var weekAgo = today.AddDays(-7);
                var monthAgo = today.AddDays(-30);

                var todayTasks = _context.Tasks.Count(t => t.IsDeleted != true && t.CreatedAt.HasValue && t.CreatedAt.Value.Date == today);
                var weekTasks = _context.Tasks.Count(t => t.IsDeleted != true && t.CreatedAt.HasValue && t.CreatedAt >= weekAgo);
                var monthTasks = _context.Tasks.Count(t => t.IsDeleted != true && t.CreatedAt.HasValue && t.CreatedAt >= monthAgo);
                var avgDailyTasks = weekTasks / 7.0;

                var metrics = new[]
                {
                    new { Title = "Hôm nay", Value = todayTasks.ToString(), Subtitle = "tasks", Color = Color.FromArgb(76, 175, 80) },
                    new { Title = "Tu?n này", Value = weekTasks.ToString(), Subtitle = "tasks", Color = Color.FromArgb(100, 149, 237) },
                    new { Title = "Tháng này", Value = monthTasks.ToString(), Subtitle = "tasks", Color = Color.FromArgb(156, 39, 176) },
                    new { Title = "TB/ngày", Value = avgDailyTasks.ToString("F1"), Subtitle = "tasks", Color = Color.FromArgb(255, 152, 0) }
                };

                for (int i = 0; i < metrics.Length; i++)
                {
                    var metric = metrics[i];
                    var card = new Panel
                    {
                        Size = new Size(300, 120),
                        Location = new Point(i * 320, 20),
                        BackColor = Color.FromArgb(40, 40, 40),
                        Margin = new Padding(10)
                    };

                    var lblIcon = new Label
                    {
                        Text = "??",
                        Location = new Point(20, 20),
                        Size = new Size(30, 30),
                        Font = new Font("Segoe UI", 18F),
                        ForeColor = metric.Color,
                        BackColor = Color.Transparent
                    };

                    var lblTitle = new Label
                    {
                        Text = metric.Title,
                        Location = new Point(60, 20),
                        Size = new Size(200, 20),
                        Font = new Font("Segoe UI", 10F),
                        ForeColor = Color.FromArgb(180, 180, 180),
                        BackColor = Color.Transparent
                    };

                    var lblValue = new Label
                    {
                        Text = metric.Value,
                        Location = new Point(60, 45),
                        Size = new Size(150, 35),
                        Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = Color.Transparent
                    };

                    var lblSubtitle = new Label
                    {
                        Text = metric.Subtitle,
                        Location = new Point(60, 85),
                        Size = new Size(100, 20),
                        Font = new Font("Segoe UI", 9F),
                        ForeColor = Color.FromArgb(150, 150, 150),
                        BackColor = Color.Transparent
                    };

                    card.Controls.Add(lblIcon);
                    card.Controls.Add(lblTitle);
                    card.Controls.Add(lblValue);
                    card.Controls.Add(lblSubtitle);

                    pnlMetrics.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading productivity metrics: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnInteractiveCharts_Click(object sender, EventArgs e)
        {
            ShowInteractiveCharts();
        }

        private void BtnPythonCharts_Click(object sender, EventArgs e)
        {
            GeneratePythonCharts();
        }

        private void ShowInteractiveCharts()
        {
            // Check Python availability
            if (!IsPythonAvailable())
            {
                var result = MessageBox.Show(
                    "?? Python charts require Python to be installed.\n\n" +
                    "Would you like to:\n" +
                    "• YES: Use C# interactive charts (built-in)\n" +
                    "• NO: Install Python and use beautiful Python charts",
                    "Choose Chart Type", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ShowBuiltInInteractiveCharts();
                }
                else if (result == DialogResult.No)
                {
                    ShowPythonInstallDialog();
                }
                return;
            }

            // Show Python-based charts
            ShowPythonInteractiveCharts();
        }

        private void ShowBuiltInInteractiveCharts()
        {
            var chartsForm = new Form
            {
                Text = "?? Interactive Charts (C# Version) - Zoom & Pan Enabled",
                Size = new Size(1400, 900),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(20, 20, 20)
            };

            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            // Tab 1: Project Progress Bar Chart
            var tabProgress = new TabPage("?? Project Progress")
            {
                BackColor = Color.FromArgb(25, 25, 25)
            };
            CreateInteractiveProjectChart(tabProgress);

            // Tab 2: Daily Productivity Line Chart
            var tabDaily = new TabPage("?? Daily Productivity")
            {
                BackColor = Color.FromArgb(25, 25, 25)
            };
            CreateInteractiveDailyChart(tabDaily);

            // Tab 3: Priority Distribution Pie Chart
            var tabPriority = new TabPage("?? Priority Distribution")
            {
                BackColor = Color.FromArgb(25, 25, 25)
            };
            CreateInteractivePriorityChart(tabPriority);

            // Tab 4: Weekly Trends Area Chart
            var tabWeekly = new TabPage("?? Weekly Trends")
            {
                BackColor = Color.FromArgb(25, 25, 25)
            };
            CreateInteractiveWeeklyChart(tabWeekly);

            tabControl.TabPages.Add(tabProgress);
            tabControl.TabPages.Add(tabDaily);
            tabControl.TabPages.Add(tabPriority);
            tabControl.TabPages.Add(tabWeekly);

            chartsForm.Controls.Add(tabControl);
            chartsForm.ShowDialog();
        }

        private void ShowPythonInteractiveCharts()
        {
            var chartsForm = new Form
            {
                Text = "?? Python Charts - Professional Visualization",
                Size = new Size(1400, 900),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(20, 20, 20)
            };

            var loadingLabel = new Label
            {
                Text = "?? Generating beautiful Python charts...\nPlease wait...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White
            };
            chartsForm.Controls.Add(loadingLabel);
            chartsForm.Show();
            Application.DoEvents();

            try
            {
                // Generate Python charts in real-time
                var chartsGenerated = GeneratePythonChartsRealtime();

                if (!chartsGenerated)
                {
                    chartsForm.Close();
                    MessageBox.Show("Failed to generate Python charts. Using C# charts instead.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ShowBuiltInInteractiveCharts();
                    return;
                }

                // Remove loading label
                chartsForm.Controls.Clear();

                // Create tab control for charts
                var tabControl = new TabControl
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.FromArgb(30, 30, 30),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold)
                };

                // Load Python-generated charts
                LoadPythonChartsIntoTabs(tabControl);

                chartsForm.Controls.Add(tabControl);
            }
            catch (Exception ex)
            {
                chartsForm.Close();
                MessageBox.Show($"Error loading Python charts: {ex.Message}\n\nUsing C# charts instead.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowBuiltInInteractiveCharts();
            }
        }

        private bool GeneratePythonChartsRealtime()
        {
            try
            {
                var pythonScript = Path.Combine(Application.StartupPath, "python_charts", "realtime_charts.py");

                // Create realtime Python script if not exists
                if (!File.Exists(pythonScript))
                {
                    CreateRealtimePythonScript(pythonScript);
                }

                // Export data to JSON for Python
                ExportDataForPython();

                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = GetPythonCommand(),
                    Arguments = $"\"{pythonScript}\"",
                    WorkingDirectory = Path.GetDirectoryName(pythonScript),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    process.WaitForExit(10000); // 10 second timeout
                    return process.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private void CreateRealtimePythonScript(string scriptPath)
        {
            var scriptContent = @"
import json
import matplotlib
matplotlib.use('Agg')
import matplotlib.pyplot as plt
import seaborn as sns
from pathlib import Path

# Set style
sns.set_style('darkgrid')
plt.rcParams['figure.facecolor'] = '#1a1a1a'
plt.rcParams['axes.facecolor'] = '#252525'
plt.rcParams['text.color'] = 'white'
plt.rcParams['axes.labelcolor'] = 'white'
plt.rcParams['xtick.color'] = 'white'
plt.rcParams['ytick.color'] = 'white'
plt.rcParams['grid.color'] = '#404040'

# Load data
script_dir = Path(__file__).parent
data_file = script_dir / 'chart_data.json'

with open(data_file, 'r', encoding='utf-8') as f:
    data = json.load(f)

output_dir = script_dir / 'realtime_charts'
output_dir.mkdir(exist_ok=True)

# Chart 1: Project Progress
if 'projects' in data:
    fig, ax = plt.subplots(figsize=(12, 6))
    projects = data['projects']
    names = [p['name'] for p in projects]
    values = [p['completion'] for p in projects]
    colors = [p['color'] for p in projects]
    
    bars = ax.barh(names, values, color=colors, edgecolor='white', linewidth=1.5)
    ax.set_xlabel('Completion Rate (%)', fontsize=12, fontweight='bold')
    ax.set_title('?? Project Progress', fontsize=14, fontweight='bold', pad=20)
    ax.set_xlim(0, 100)
    
    # Add value labels
    for i, (bar, value) in enumerate(zip(bars, values)):
        ax.text(value + 2, bar.get_y() + bar.get_height()/2, 
                f'{value:.0f}%', va='center', fontweight='bold')
    
    plt.tight_layout()
    plt.savefig(output_dir / 'project_progress.png', dpi=150, bbox_inches='tight')
    plt.close()

# Chart 2: Daily Productivity
if 'daily' in data:
    fig, ax = plt.subplots(figsize=(12, 6))
    daily = data['daily']
    dates = [d['date'] for d in daily]
    values = [d['count'] for d in daily]
    
    ax.plot(dates, values, marker='o', linewidth=3, markersize=10, 
            color='#6495ED', markerfacecolor='white', markeredgewidth=2)
    ax.fill_between(range(len(dates)), values, alpha=0.3, color='#6495ED')
    ax.set_xlabel('Date', fontsize=12, fontweight='bold')
    ax.set_ylabel('Tasks', fontsize=12, fontweight='bold')
    ax.set_title('?? Daily Productivity', fontsize=14, fontweight='bold', pad=20)
    ax.grid(True, alpha=0.3)
    
    plt.xticks(range(len(dates)), dates, rotation=45)
    plt.tight_layout()
    plt.savefig(output_dir / 'daily_productivity.png', dpi=150, bbox_inches='tight')
    plt.close()

# Chart 3: Priority Distribution
if 'priority' in data:
    fig, ax = plt.subplots(figsize=(10, 10))
    priority = data['priority']
    labels = [p['label'] for p in priority]
    sizes = [p['count'] for p in priority]
    colors_map = {'High': '#F44336', 'Medium': '#FF9800', 'Low': '#4CAF50'}
    colors = [colors_map.get(p['label'], '#999') for p in priority]
    
    wedges, texts, autotexts = ax.pie(sizes, labels=labels, colors=colors,
                                       autopct='%1.1f%%', startangle=90,
                                       textprops={'fontsize': 12, 'fontweight': 'bold'})
    
    for text in texts:
        text.set_color('white')
    for autotext in autotexts:
        autotext.set_color('white')
        autotext.set_fontsize(14)
    
    ax.set_title('?? Priority Distribution', fontsize=14, fontweight='bold', pad=20)
    plt.tight_layout()
    plt.savefig(output_dir / 'priority_distribution.png', dpi=150, bbox_inches='tight')
    plt.close()

# Chart 4: Weekly Trends
if 'weekly' in data:
    fig, ax = plt.subplots(figsize=(12, 6))
    weekly = data['weekly']
    weeks = [w['label'] for w in weekly]
    values = [w['count'] for w in weekly]
    
    x = range(len(weeks))
    bars = ax.bar(x, values, color='#9C27B0', alpha=0.8, edgecolor='white', linewidth=1.5)
    
    # Add gradient effect
    for bar in bars:
        bar.set_facecolor('#9C27B0')
        bar.set_alpha(0.8)
    
    ax.set_xlabel('Week', fontsize=12, fontweight='bold')
    ax.set_ylabel('Tasks', fontsize=12, fontweight='bold')
    ax.set_title('?? Weekly Trends', fontsize=14, fontweight='bold', pad=20)
    ax.set_xticks(x)
    ax.set_xticklabels(weeks)
    ax.grid(True, alpha=0.3, axis='y')
    
    # Add value labels
    for i, (bar, value) in enumerate(zip(bars, values)):
        ax.text(bar.get_x() + bar.get_width()/2, value + max(values)*0.02,
                f'{value}', ha='center', fontweight='bold')
    
    plt.tight_layout()
    plt.savefig(output_dir / 'weekly_trends.png', dpi=150, bbox_inches='tight')
    plt.close()

print('Charts generated successfully!')
";

            Directory.CreateDirectory(Path.GetDirectoryName(scriptPath));
            File.WriteAllText(scriptPath, scriptContent);
        }

        private void ExportDataForPython()
        {
            var dataPath = Path.Combine(Application.StartupPath, "python_charts", "chart_data.json");
            
            // Get data from database
            var projectData = _context.Projects
                .Where(p => p.IsArchived != true)
                .Select(p => new
                {
                    name = p.ProjectName,
                    completion = p.Tasks.Count(t => t.IsDeleted != true) > 0 ?
                        (double)p.Tasks.Count(t => t.IsDeleted != true && t.Status == "Completed") /
                        p.Tasks.Count(t => t.IsDeleted != true) * 100 : 0,
                    color = !string.IsNullOrEmpty(p.ColorCode) ? p.ColorCode : "#6495ED"
                })
                .Take(8)
                .ToList();

            var dailyData = _context.Tasks
                .Where(t => t.IsDeleted != true && t.CreatedAt >= DateTime.Now.AddDays(-7))
                .GroupBy(t => t.CreatedAt.Value.Date)
                .Select(g => new
                {
                    date = g.Key.ToString("dd/MM"),
                    count = g.Count()
                })
                .OrderBy(d => d.date)
                .ToList();

            var priorityData = _context.Tasks
                .Where(t => t.IsDeleted != true)
                .GroupBy(t => t.Priority ?? "Medium")
                .Select(g => new
                {
                    label = g.Key,
                    count = g.Count()
                })
                .ToList();

            var weeklyData = _context.Tasks
                .Where(t => t.IsDeleted != true && t.CreatedAt >= DateTime.Now.AddDays(-28))
                .GroupBy(t => new
                {
                    Year = t.CreatedAt.Value.Year,
                    Week = (t.CreatedAt.Value.DayOfYear - 1) / 7 + 1
                })
                .Select(g => new
                {
                    label = $"Week {g.Key.Week}",
                    count = g.Count()
                })
                .OrderBy(w => w.label)
                .Take(4)
                .ToList();

            var jsonData = new
            {
                projects = projectData,
                daily = dailyData,
                priority = priorityData,
                weekly = weeklyData
            };

            var json = System.Text.Json.JsonSerializer.Serialize(jsonData, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            File.WriteAllText(dataPath, json);
        }

        private void LoadPythonChartsIntoTabs(TabControl tabControl)
        {
            var chartsDir = Path.Combine(Application.StartupPath, "python_charts", "realtime_charts");
            
            var charts = new[]
            {
                new { Title = "?? Project Progress", FileName = "project_progress.png" },
                new { Title = "?? Daily Productivity", FileName = "daily_productivity.png" },
                new { Title = "?? Priority Distribution", FileName = "priority_distribution.png" },
                new { Title = "?? Weekly Trends", FileName = "weekly_trends.png" }
            };

            foreach (var chart in charts)
            {
                var chartPath = Path.Combine(chartsDir, chart.FileName);
                
                if (File.Exists(chartPath))
                {
                    var tab = new TabPage(chart.Title)
                    {
                        BackColor = Color.FromArgb(25, 25, 25)
                    };

                    var panel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        AutoScroll = true,
                        BackColor = Color.FromArgb(25, 25, 25)
                    };

                    var pictureBox = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Fill,
                        BackColor = Color.FromArgb(25, 25, 25)
                    };

                    try
                    {
                        // Load image with proper disposal
                        using (var stream = new FileStream(chartPath, FileMode.Open, FileAccess.Read))
                        {
                            pictureBox.Image = Image.FromStream(stream);
                        }

                        // Add zoom controls
                        var btnZoomIn = new Button
                        {
                            Text = "?? +",
                            Location = new Point(10, 10),
                            Size = new Size(50, 30),
                            BackColor = Color.FromArgb(100, 149, 237),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        btnZoomIn.FlatAppearance.BorderSize = 0;
                        btnZoomIn.Click += (s, e) =>
                        {
                            if (pictureBox.SizeMode == PictureBoxSizeMode.Zoom)
                            {
                                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                            }
                        };

                        var btnZoomOut = new Button
                        {
                            Text = "?? -",
                            Location = new Point(70, 10),
                            Size = new Size(50, 30),
                            BackColor = Color.FromArgb(100, 149, 237),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        btnZoomOut.FlatAppearance.BorderSize = 0;
                        btnZoomOut.Click += (s, e) =>
                        {
                            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        };

                        var btnRefresh = new Button
                        {
                            Text = "??",
                            Location = new Point(130, 10),
                            Size = new Size(50, 30),
                            BackColor = Color.FromArgb(76, 175, 80),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        btnRefresh.FlatAppearance.BorderSize = 0;
                        btnRefresh.Click += (s, e) =>
                        {
                            if (GeneratePythonChartsRealtime())
                            {
                                using (var stream = new FileStream(chartPath, FileMode.Open, FileAccess.Read))
                                {
                                    var oldImage = pictureBox.Image;
                                    pictureBox.Image = Image.FromStream(stream);
                                    oldImage?.Dispose();
                                }
                                MessageBox.Show("Chart refreshed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        };

                        panel.Controls.Add(pictureBox);
                        panel.Controls.Add(btnZoomIn);
                        panel.Controls.Add(btnZoomOut);
                        panel.Controls.Add(btnRefresh);

                        tab.Controls.Add(panel);
                        tabControl.TabPages.Add(tab);
                    }
                    catch (Exception ex)
                    {
                        var lblError = new Label
                        {
                            Text = $"Error loading chart: {ex.Message}",
                            Dock = DockStyle.Fill,
                            ForeColor = Color.White,
                            TextAlign = ContentAlignment.MiddleCenter
                        };
                        tab.Controls.Add(lblError);
                    }
                }
            }

            if (tabControl.TabPages.Count == 0)
            {
                MessageBox.Show("No charts were generated. Please check Python installation.",
                    "No Charts", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CreateInteractiveProjectChart(TabPage tab)
        {
            var chart = new InteractiveChartControl
            {
                Dock = DockStyle.Fill
            };

            // Step 1: Get data from database WITHOUT Color conversion
            var projectDataRaw = _context.Projects
                .Where(p => p.IsArchived != true)
                .Select(p => new
                {
                    Name = p.ProjectName,
                    ColorCode = p.ColorCode,
                    TotalTasks = p.Tasks.Count(t => t.IsDeleted != true),
                    CompletedTasks = p.Tasks.Count(t => t.IsDeleted != true && t.Status == "Completed")
                })
                .Take(10)
                .ToList(); // Execute query first

            // Step 2: Process colors and calculate completion rate in memory
            var chartData = projectDataRaw.Select(p => new InteractiveChartControl.ChartDataPoint
            {
                Label = p.Name,
                Value = p.TotalTasks > 0 ? (double)p.CompletedTasks / p.TotalTasks * 100 : 0,
                Color = !string.IsNullOrEmpty(p.ColorCode) ?
                    ColorTranslator.FromHtml(p.ColorCode) :
                    Color.FromArgb(100, 149, 237)
            }).ToList();

            chart.SetData(chartData, InteractiveChartControl.ChartType.Bar);
            tab.Controls.Add(chart);
        }

        private void CreateInteractiveDailyChart(TabPage tab)
        {
            var chart = new InteractiveChartControl
            {
                Dock = DockStyle.Fill
            };

            // Get daily data for last 7 days
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            var dailyData = _context.Tasks
                .Where(t => t.IsDeleted != true && t.CreatedAt >= sevenDaysAgo)
                .GroupBy(t => t.CreatedAt.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToList();

            var chartData = dailyData.Select((d, i) => new InteractiveChartControl.ChartDataPoint
            {
                Label = d.Date.ToString("dd/MM"),
                Value = d.Count,
                Date = d.Date,
                Color = Color.FromArgb(100, 149, 237)
            }).ToList();

            chart.SetData(chartData, InteractiveChartControl.ChartType.Line);
            tab.Controls.Add(chart);
        }

        private void CreateInteractivePriorityChart(TabPage tab)
        {
            var chart = new InteractiveChartControl
            {
                Dock = DockStyle.Fill
            };

            // Get priority distribution
            var priorityData = _context.Tasks
                .Where(t => t.IsDeleted != true)
                .GroupBy(t => t.Priority ?? "Medium")
                .Select(g => new
                {
                    Priority = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var priorityColors = new Dictionary<string, Color>
            {
                { "High", Color.FromArgb(244, 67, 54) },
                { "Medium", Color.FromArgb(255, 152, 0) },
                { "Low", Color.FromArgb(76, 175, 80) }
            };

            var chartData = priorityData.Select(p => new InteractiveChartControl.ChartDataPoint
            {
                Label = p.Priority,
                Value = p.Count,
                Color = priorityColors.ContainsKey(p.Priority) ? priorityColors[p.Priority] : Color.Gray
            }).ToList();

            chart.SetData(chartData, InteractiveChartControl.ChartType.Pie);
            tab.Controls.Add(chart);
        }

        private void CreateInteractiveWeeklyChart(TabPage tab)
        {
            var chart = new InteractiveChartControl
            {
                Dock = DockStyle.Fill
            };

            // Get weekly data for last 4 weeks
            var fourWeeksAgo = DateTime.Now.AddDays(-28);
            var weeklyData = _context.Tasks
                .Where(t => t.IsDeleted != true && t.CreatedAt >= fourWeeksAgo)
                .GroupBy(t => new
                {
                    Year = t.CreatedAt.Value.Year,
                    Week = (t.CreatedAt.Value.DayOfYear - 1) / 7 + 1
                })
                .Select(g => new
                {
                    WeekNumber = g.Key.Week,
                    Count = g.Count()
                })
                .OrderBy(w => w.WeekNumber)
                .ToList();

            var chartData = weeklyData.Select(w => new InteractiveChartControl.ChartDataPoint
            {
                Label = $"Week {w.WeekNumber}",
                Value = w.Count,
                Color = Color.FromArgb(156, 39, 176)
            }).ToList();

            chart.SetData(chartData, InteractiveChartControl.ChartType.Area);
            tab.Controls.Add(chart);
        }

        private void GeneratePythonCharts()
        {
            try
            {
                var pythonScript = Path.Combine(Application.StartupPath, "python_charts", "todolist_charts.py");

                // Check if Python is available
                if (!IsPythonAvailable())
                {
                    ShowPythonInstallDialog();
                    return;
                }

                // Check if script exists
                if (!File.Exists(pythonScript))
                {
                    MessageBox.Show("Python script not found. Please run setup first.", "Missing Files", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("?? Generating Python charts...\nThis may take a few seconds.", 
                    "Python Charts", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = GetPythonCommand(),
                    Arguments = $"\"{pythonScript}\"",
                    WorkingDirectory = Path.GetDirectoryName(pythonScript),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    process.WaitForExit();
                    
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    
                    if (process.ExitCode == 0)
                    {
                        var chartsFolder = Path.Combine(Path.GetDirectoryName(pythonScript), "ToDoList_Charts");
                        
                        var result = MessageBox.Show($"? Charts generated successfully!\n\n" +
                            $"?? Location: {chartsFolder}\n\n" +
                            "Would you like to open the charts folder?", 
                            "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        
                        if (result == DialogResult.Yes && Directory.Exists(chartsFolder))
                        {
                            System.Diagnostics.Process.Start("explorer.exe", chartsFolder);
                        }
                    }
                    else
                    {
                        HandlePythonError(error, output);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating charts: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsPythonAvailable()
        {
            // Try py first (Python Launcher on Windows)
            string[] commands = { "py", "python", "python3" };
            
            foreach (string cmd in commands)
            {
                try
                {
                    var testInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = cmd,
                        Arguments = "--version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };
                    
                    using (var process = System.Diagnostics.Process.Start(testInfo))
                    {
                        process.WaitForExit();
                        if (process.ExitCode == 0)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            return false;
        }

        private string GetPythonCommand()
        {
            // Try py first (Python Launcher on Windows)
            string[] commands = { "py", "python", "python3" };
            
            foreach (string cmd in commands)
            {
                try
                {
                    var testInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = cmd,
                        Arguments = "--version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };
                    
                    using (var process = System.Diagnostics.Process.Start(testInfo))
                    {
                        process.WaitForExit();
                        if (process.ExitCode == 0)
                        {
                            return cmd;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            return "py"; // Default to py on Windows
        }

        private void ShowPythonInstallDialog()
        {
            var result = MessageBox.Show(
                "? Python not found!\n\n" +
                "To use Python Charts:\n" +
                "1?? Install Python from python.org\n" +
                "2?? ? Check 'Add Python to PATH'\n" +
                "3?? Run QUICK_FIX_PYTHON_CHARTS.bat\n\n" +
                "Would you like to:\n" +
                "• YES: Run auto-installer\n" +
                "• NO: Open Python download page",
                "Python Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    var installerPath = Path.Combine(Application.StartupPath, "..", "..", "..", "QUICK_FIX_PYTHON_CHARTS.bat");
                    if (File.Exists(installerPath))
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = installerPath,
                            UseShellExecute = true,
                            Verb = "runas"
                        });
                    }
                    else
                    {
                        MessageBox.Show("Installer not found. Please run QUICK_FIX_PYTHON_CHARTS.bat manually.", 
                            "Installer Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot run installer: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://www.python.org/downloads/",
                        UseShellExecute = true
                    });
                }
                catch { }
            }
        }

        private void HandlePythonError(string error, string output)
        {
            if (error.Contains("ModuleNotFoundError") || error.Contains("No module named"))
            {
                var result = MessageBox.Show(
                    "? Missing Python packages!\n\n" +
                    "The following packages are required:\n" +
                    "• matplotlib (for charts)\n" +
                    "• seaborn (for styling)\n" +
                    "• pandas (for data processing)\n" +
                    "• pyodbc (database connection)\n" +
                    "• plotly (interactive charts)\n\n" +
                    "Would you like to install them automatically?",
                    "Missing Packages", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = "/k echo Installing Python packages... && python -m pip install matplotlib seaborn pandas pyodbc plotly kaleido && echo. && echo Installation completed! && pause",
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Cannot run installer: {ex.Message}\n\n" +
                            "Please run manually:\npython -m pip install matplotlib seaborn pandas pyodbc plotly kaleido",
                            "Manual Installation Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show($"? Python Error:\n\n{error}\n\nOutput:\n{output}", 
                    "Python Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}