using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace ToDoList.GUI.Forms
{
    public partial class ReportsForm : Form
    {
        private ToDoListContext _context;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private Panel pnlStats;
        private Panel pnlChart;
        private ComboBox cmbProjects;
        private Label lblTimeZone;

        public ReportsForm(ToDoListContext context)
        {
            _context = context;
            InitializeComponent();
            LoadInitialData();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1200, 800);
            this.Text = "?? Bao cao thong ke - ToDoList";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            
            // Set font to support Vietnamese
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

            CreateHeader();
            CreateDateRangePicker();
            CreateStatsCards();
            CreateChart();
        }

        private void CreateHeader()
        {
            // Header panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(25, 25, 25)
            };

            // Back button
            Button btnBack = new Button
            {
                Text = "? BACK",
                Location = new Point(20, 25),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40);
            btnBack.Click += (s, e) => this.Close();

            // Title
            Label lblTitle = new Label
            {
                Text = "Reports",
                Location = new Point(120, 22),
                Size = new Size(200, 35),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Upgrade button (decorative)
            Button btnUpgrade = new Button
            {
                Text = "? Upgrade Now",
                Location = new Point(950, 20),
                Size = new Size(120, 35),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 220, 180),
                Cursor = Cursors.Hand
            };
            btnUpgrade.FlatAppearance.BorderSize = 0;

            // Settings button
            Button btnSettings = new Button
            {
                Text = "??",
                Location = new Point(1080, 20),
                Size = new Size(35, 35),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 14F),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 40);

            // User button
            Button btnUser = new Button
            {
                Text = "D",
                Location = new Point(1125, 20),
                Size = new Size(35, 35),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 149, 237),
                Cursor = Cursors.Hand
            };
            btnUser.FlatAppearance.BorderSize = 0;

            headerPanel.Controls.Add(btnBack);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnUpgrade);
            headerPanel.Controls.Add(btnSettings);
            headerPanel.Controls.Add(btnUser);

            this.Controls.Add(headerPanel);
        }

        private void CreateDateRangePicker()
        {
            Panel datePanel = new Panel
            {
                Location = new Point(20, 100),
                Size = new Size(1160, 60),
                BackColor = Color.Transparent
            };

            // Project filter
            Label lblProject = new Label
            {
                Text = "?? Tat ca du an",
                Location = new Point(0, 5),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            cmbProjects = new ComboBox
            {
                Location = new Point(0, 30),
                Size = new Size(150, 25),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Date range
            Label lblDateRange = new Label
            {
                Text = "??",
                Location = new Point(200, 5),
                Size = new Size(25, 25),
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            dtpStartDate = new DateTimePicker
            {
                Location = new Point(230, 30),
                Size = new Size(120, 25),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(40, 40, 40),
                CalendarForeColor = Color.White,
                Value = DateTime.Now.AddDays(-30) // Show last 30 days by default
            };

            Label lblTo = new Label
            {
                Text = "-",
                Location = new Point(360, 32),
                Size = new Size(15, 20),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };

            dtpEndDate = new DateTimePicker
            {
                Location = new Point(380, 30),
                Size = new Size(120, 25),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(40, 40, 40),
                CalendarForeColor = Color.White,
                Value = DateTime.Now
            };

            // Timezone info
            lblTimeZone = new Label
            {
                Text = $"?? Mui gio: {DateTime.Now:ddd, dd MMM HH:mm tt}",
                Location = new Point(900, 32),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Update button
            Button btnUpdate = new Button
            {
                Text = "?? Cap nhat",
                Location = new Point(520, 28),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 149, 237),
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 169, 255);
            btnUpdate.Click += BtnUpdate_Click;

            datePanel.Controls.Add(lblProject);
            datePanel.Controls.Add(cmbProjects);
            datePanel.Controls.Add(lblDateRange);
            datePanel.Controls.Add(dtpStartDate);
            datePanel.Controls.Add(lblTo);
            datePanel.Controls.Add(dtpEndDate);
            datePanel.Controls.Add(btnUpdate);
            datePanel.Controls.Add(lblTimeZone);

            this.Controls.Add(datePanel);

            // Event handlers
            dtpStartDate.ValueChanged += (s, e) => LoadReportData();
            dtpEndDate.ValueChanged += (s, e) => LoadReportData();
            cmbProjects.SelectedIndexChanged += (s, e) => LoadReportData();
        }

        private void CreateStatsCards()
        {
            pnlStats = new Panel
            {
                Location = new Point(20, 180),
                Size = new Size(1160, 120),
                BackColor = Color.Transparent
            };

            this.Controls.Add(pnlStats);
        }

        private void CreateChart()
        {
            pnlChart = new Panel
            {
                Location = new Point(20, 320),
                Size = new Size(1160, 400),
                BackColor = Color.FromArgb(25, 25, 25),
            };

            // Chart title
            Label lblChartTitle = new Label
            {
                Text = "?? Tien do theo ngay",
                Location = new Point(20, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            pnlChart.Controls.Add(lblChartTitle);
            this.Controls.Add(pnlChart);
        }

        private async void LoadInitialData()
        {
            try
            {
                // Load projects for filter
                var projects = await _context.Projects
                    .Where(p => p.IsArchived != true)
                    .Select(p => new { p.ProjectId, p.ProjectName })
                    .ToListAsync();

                cmbProjects.Items.Clear();
                cmbProjects.Items.Add(new { ProjectId = 0, ProjectName = "Tat ca du an" });
                foreach (var project in projects)
                {
                    cmbProjects.Items.Add(project);
                }
                cmbProjects.DisplayMember = "ProjectName";
                cmbProjects.ValueMember = "ProjectId";
                cmbProjects.SelectedIndex = 0;

                // Show a simple message first
                MessageBox.Show($"Da tai {projects.Count()} du an. Dang tai bao cao...", "Thong tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Load initial report data
                await LoadReportData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi tai du lieu: {ex.Message}\n\nStack: {ex.StackTrace}", "Loi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            LoadReportData();
        }

        private async System.Threading.Tasks.Task LoadReportData()
        {
            try
            {
                var startDate = dtpStartDate.Value.Date;
                var endDate = dtpEndDate.Value.Date.AddDays(1);
                
                dynamic selectedProject = cmbProjects.SelectedItem;
                int? projectId = selectedProject?.ProjectId == 0 ? null : selectedProject?.ProjectId;

                // Debug information
                var totalTasksInDb = await _context.Tasks.CountAsync(t => t.IsDeleted != true);
                var totalCompletedInDb = await _context.Tasks.CountAsync(t => t.IsDeleted != true && t.Status == "Completed");
                
                MessageBox.Show($"Debug:\nTotal tasks in DB: {totalTasksInDb}\nCompleted tasks: {totalCompletedInDb}\nDate range: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}", 
                    "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Query data with correct date filtering
                var tasksQuery = _context.Tasks
                    .Where(t => t.IsDeleted != true);

                // Only filter by date if we're not looking at all data
                if (startDate != DateTime.MinValue.Date)
                {
                    tasksQuery = tasksQuery.Where(t => t.CreatedAt >= startDate && t.CreatedAt < endDate);
                }

                if (projectId.HasValue)
                {
                    tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId);
                }

                var tasks = await tasksQuery.ToListAsync();

                // Debug filtered results
                MessageBox.Show($"Filtered tasks: {tasks.Count}\nCompleted in range: {tasks.Count(t => t.Status == "Completed")}", 
                    "Filtered Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Calculate statistics
                var stats = CalculateStatistics(tasks, startDate, endDate);
                var dailyData = CalculateDailyData(tasks, startDate, endDate);

                // Update UI
                UpdateStatsCards(stats);
                UpdateChart(dailyData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi tai bao cao: {ex.Message}", "Loi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private object CalculateStatistics(List<TodoListApp.DAL.Models.Task> tasks, DateTime startDate, DateTime endDate)
        {
            var totalTasks = tasks.Count;
            var completedTasks = tasks.Count(t => t.Status == "Completed");
            var inProgressTasks = tasks.Count(t => t.Status == "In Progress");
            var pendingTasks = tasks.Count(t => t.Status == "Pending");
            
            var workDays = CalculateWorkDays(startDate, endDate.AddDays(-1));
            
            // Calculate total estimated hours (from all tasks, not just completed)
            var totalMinutes = tasks.Where(t => t.EstimatedMinutes.HasValue)
                                   .Sum(t => t.EstimatedMinutes.Value);
            var totalHours = totalMinutes / 60.0;
            
            // Average time per task (from all tasks that have time estimates)
            var tasksWithTime = tasks.Where(t => t.EstimatedMinutes.HasValue).ToList();
            var avgTimePerTask = tasksWithTime.Count > 0 ? 
                tasksWithTime.Average(t => t.EstimatedMinutes.Value) : 0;

            return new
            {
                WorkDays = Math.Max(workDays, 1),
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                InProgressTasks = inProgressTasks,
                PendingTasks = pendingTasks,
                TasksPerDay = workDays > 0 ? Math.Round((double)totalTasks / workDays, 1) : totalTasks,
                CompletedPerDay = workDays > 0 ? Math.Round((double)completedTasks / workDays, 1) : completedTasks,
                TotalHours = Math.Round(totalHours, 1),
                HoursPerDay = workDays > 0 ? Math.Round(totalHours / workDays, 1) : totalHours,
                AvgTimePerTask = Math.Round(avgTimePerTask, 1),
                CompletionRate = totalTasks > 0 ? Math.Round((double)completedTasks / totalTasks * 100, 1) : 0
            };
        }

        private int CalculateWorkDays(DateTime startDate, DateTime endDate)
        {
            int workDays = 0;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workDays++;
                }
            }
            return Math.Max(workDays, 1);
        }

        private List<object> CalculateDailyData(List<TodoListApp.DAL.Models.Task> tasks, DateTime startDate, DateTime endDate)
        {
            var dailyData = new List<object>();
            
            // If we have a small date range, show daily data
            // If we have a large date range, show weekly or monthly data
            var totalDays = (endDate.Date - startDate.Date).TotalDays;
            
            if (totalDays <= 14) // Show daily for 2 weeks or less
            {
                for (var date = startDate.Date; date < endDate.Date; date = date.AddDays(1))
                {
                    // Include tasks created on this date OR tasks that were active on this date
                    var dayTasks = tasks.Where(t => 
                        t.CreatedAt?.Date == date || 
                        (t.CreatedAt?.Date <= date && t.UpdatedAt?.Date >= date)
                    ).ToList();
                    
                    var completedTasks = dayTasks.Count(t => t.Status == "Completed");
                    var totalTasks = dayTasks.Count;

                    dailyData.Add(new
                    {
                        Date = date,
                        DayName = GetVietnameseDayName(date.DayOfWeek),
                        DateString = date.ToString("dd/MM"),
                        CompletedTasks = completedTasks,
                        TotalTasks = totalTasks,
                        CompletionRate = totalTasks > 0 ? (double)completedTasks / totalTasks : 0
                    });
                }
            }
            else // Show weekly data for longer periods
            {
                var weekStart = startDate.Date;
                while (weekStart < endDate.Date)
                {
                    var weekEnd = weekStart.AddDays(7);
                    if (weekEnd > endDate.Date) weekEnd = endDate.Date;
                    
                    var weekTasks = tasks.Where(t => 
                        t.CreatedAt >= weekStart && t.CreatedAt < weekEnd
                    ).ToList();
                    
                    var completedTasks = weekTasks.Count(t => t.Status == "Completed");
                    var totalTasks = weekTasks.Count;

                    dailyData.Add(new
                    {
                        Date = weekStart,
                        DayName = $"Tuan {weekStart:dd/MM}",
                        DateString = $"Tuan {weekStart:dd/MM}",
                        CompletedTasks = completedTasks,
                        TotalTasks = totalTasks,
                        CompletionRate = totalTasks > 0 ? (double)completedTasks / totalTasks : 0
                    });
                    
                    weekStart = weekEnd;
                }
            }

            return dailyData;
        }

        private string GetVietnameseDayName(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Thu 2",
                DayOfWeek.Tuesday => "Thu 3", 
                DayOfWeek.Wednesday => "Thu 4",
                DayOfWeek.Thursday => "Thu 5",
                DayOfWeek.Friday => "Thu 6",
                DayOfWeek.Saturday => "Thu 7",
                DayOfWeek.Sunday => "Chu nhat",
                _ => dayOfWeek.ToString()
            };
        }

        private void UpdateStatsCards(dynamic stats)
        {
            pnlStats.Controls.Clear();

            var cards = new[]
            {
                new { Title = "TONG SO NGAY LAM", Value = $"{stats.WorkDays} ngay", SubValue = "", Color = Color.FromArgb(50, 50, 50) },
                new { Title = "TASK HOAN THANH", Value = $"{stats.CompletedTasks:D2}/{stats.TotalTasks}", SubValue = $"{stats.CompletedPerDay} / ngay", Color = Color.FromArgb(50, 50, 50) },
                new { Title = "TONG GIO LAM VIEC", Value = $"{stats.TotalHours}h", SubValue = $"{stats.HoursPerDay}h / ngay", Color = Color.FromArgb(50, 50, 50) },
                new { Title = "THOI GIAN TB/TASK", Value = $"{stats.AvgTimePerTask} phut", SubValue = $"{stats.CompletionRate}% hoan thanh", Color = Color.FromArgb(50, 50, 50) }
            };

            for (int i = 0; i < cards.Length; i++)
            {
                var card = CreateStatCard(cards[i].Title, cards[i].Value, cards[i].SubValue, cards[i].Color);
                card.Location = new Point(i * 290, 0);
                pnlStats.Controls.Add(card);
            }
        }

        private Panel CreateStatCard(string title, string value, string subValue, Color bgColor)
        {
            var card = new Panel
            {
                Size = new Size(280, 100),
                BackColor = bgColor,
                Margin = new Padding(5)
            };

            var lblTitle = new Label
            {
                Text = title,
                Location = new Point(15, 15),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent
            };

            var lblValue = new Label
            {
                Text = value,
                Location = new Point(15, 35),
                Size = new Size(200, 35),
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            if (!string.IsNullOrEmpty(subValue))
            {
                var lblSubValue = new Label
                {
                    Text = subValue,
                    Location = new Point(15, 70),
                    Size = new Size(200, 20),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(150, 150, 150),
                    BackColor = Color.Transparent
                };
                card.Controls.Add(lblSubValue);
            }

            return card;
        }

        private void UpdateChart(List<object> dailyData)
        {
            // Clear existing chart
            var chartArea = pnlChart.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "ChartArea");
            if (chartArea != null)
            {
                pnlChart.Controls.Remove(chartArea);
            }

            // Create new chart area
            chartArea = new Panel
            {
                Name = "ChartArea",
                Location = new Point(20, 50),
                Size = new Size(1120, 300),
                BackColor = Color.Transparent
            };

            chartArea.Paint += (sender, e) => DrawChart(e.Graphics, dailyData, chartArea.Size);
            
            // Add day labels
            for (int i = 0; i < dailyData.Count; i++)
            {
                dynamic dayData = dailyData[i];
                var lblDay = new Label
                {
                    Text = $"{dayData.DayName}\n{dayData.DateString}",
                    Location = new Point(i * (1120 / Math.Max(dailyData.Count, 1)) + 40, 310),
                    Size = new Size(80, 40),
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(150, 150, 150),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.TopCenter
                };
                chartArea.Controls.Add(lblDay);
            }

            pnlChart.Controls.Add(chartArea);
        }

        private void DrawChart(Graphics g, List<object> dailyData, Size chartSize)
        {
            if (dailyData.Count == 0) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            var maxTasks = dailyData.Max(d => ((dynamic)d).TotalTasks);
            if (maxTasks == 0) maxTasks = 1;

            var barWidth = chartSize.Width / Math.Max(dailyData.Count, 1) - 20;
            var chartHeight = chartSize.Height - 60;

            // Draw gradient bars for each day
            for (int i = 0; i < dailyData.Count; i++)
            {
                dynamic dayData = dailyData[i];
                var completedTasks = (int)dayData.CompletedTasks;
                var totalTasks = (int)dayData.TotalTasks;
                var completionRate = (double)dayData.CompletionRate;

                var x = i * (chartSize.Width / dailyData.Count) + 10;
                var completedHeight = (int)(chartHeight * completionRate);
                var totalHeight = (int)(chartHeight * totalTasks / (double)maxTasks);

                // Draw total tasks bar (background)
                var totalRect = new Rectangle(x, chartSize.Height - totalHeight - 40, barWidth, totalHeight);
                using (var brush = new SolidBrush(Color.FromArgb(60, 60, 60)))
                {
                    g.FillRectangle(brush, totalRect);
                }

                // Draw completed tasks bar (foreground)
                if (completedHeight > 0)
                {
                    var completedRect = new Rectangle(x, chartSize.Height - completedHeight - 40, barWidth, completedHeight);
                    
                    // Create gradient brush
                    var colorIntensity = Math.Min(255, (int)(completionRate * 255 + 100));
                    using (var brush = new LinearGradientBrush(
                        completedRect, 
                        Color.FromArgb(colorIntensity, 100, 200, 150), 
                        Color.FromArgb(colorIntensity, 50, 150, 100), 
                        LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(brush, completedRect);
                    }
                }

                // Draw task count on top of bar
                if (totalTasks > 0)
                {
                    var text = $"{completedTasks}/{totalTasks}";
                    var textSize = g.MeasureString(text, new Font("Segoe UI", 8F));
                    var textX = x + (barWidth - textSize.Width) / 2;
                    var textY = chartSize.Height - Math.Max(totalHeight, completedHeight) - 60;
                    
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.DrawString(text, new Font("Segoe UI", 8F), brush, textX, textY);
                    }
                }
            }

            // Draw progress line
            if (dailyData.Count > 1)
            {
                var points = new List<PointF>();
                for (int i = 0; i < dailyData.Count; i++)
                {
                    dynamic dayData = dailyData[i];
                    var completionRate = (double)dayData.CompletionRate;
                    var x = i * (chartSize.Width / dailyData.Count) + barWidth / 2 + 10;
                    var y = chartSize.Height - (int)(chartHeight * completionRate) - 40;
                    points.Add(new PointF(x, y));
                }

                if (points.Count > 1)
                {
                    using (var pen = new Pen(Color.FromArgb(100, 149, 237), 3))
                    {
                        g.DrawLines(pen, points.ToArray());
                    }

                    // Draw points
                    foreach (var point in points)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(100, 149, 237)))
                        {
                            g.FillEllipse(brush, point.X - 4, point.Y - 4, 8, 8);
                        }
                    }
                }
            }
        }
    }
}