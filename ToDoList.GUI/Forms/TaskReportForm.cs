using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using TaskModel = TodoListApp.DAL.Models.Task;
using ToDoList.GUI.Components;

namespace ToDoList.GUI.Forms
{
    /// <summary>
    /// Form báo cáo chi ti?t cho t?ng task
    /// </summary>
    public partial class TaskReportForm : Form
    {
        private readonly ToDoListContext _context;
        private readonly TaskModel _task;
        private readonly int _userId;
        
        // UI Components
        private Panel pnlHeader;
        private Panel pnlStats;
        private Panel pnlCharts;
        private Panel pnlTimeline;
        private Panel pnlActivity;
        private InteractiveChartControl chartProgress;
        private InteractiveChartControl chartTimeSpent;

        public TaskReportForm(ToDoListContext context, TaskModel task, int userId)
        {
            _context = context;
            _task = task;
            _userId = userId;
            
            InitializeComponent();
            LoadTaskReport();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1400, 900);
            this.Text = $"?? Báo Cáo Task: {_task?.Title ?? "N/A"}";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            
            // Create main layout
            CreateHeader();
            CreateStatsPanel();
            CreateChartsPanel();
            CreateTimelinePanel();
            CreateActivityPanel();
        }

        #region Header
        private void CreateHeader()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = Color.FromArgb(25, 25, 25)
            };

            // Back button
            Button btnBack = new Button
            {
                Text = "?",
                Location = new Point(20, 20),
                Size = new Size(50, 40),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 40, 40),
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);
            btnBack.Click += (s, e) => this.Close();

            // Task Title
            Label lblTitle = new Label
            {
                Text = _task?.Title ?? "N/A",
                Location = new Point(90, 20),
                Size = new Size(1000, 40),
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // Task Status Badge
            Panel statusBadge = new Panel
            {
                Location = new Point(90, 70),
                Size = new Size(120, 30),
                BackColor = GetStatusColor(_task?.Status)
            };

            Label lblStatus = new Label
            {
                Text = _task?.Status ?? "N/A",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };
            statusBadge.Controls.Add(lblStatus);

            // Task Priority Badge
            Panel priorityBadge = new Panel
            {
                Location = new Point(220, 70),
                Size = new Size(100, 30),
                BackColor = GetPriorityColor(_task?.Priority)
            };

            Label lblPriority = new Label
            {
                Text = _task?.Priority ?? "N/A",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };
            priorityBadge.Controls.Add(lblPriority);

            // Due Date
            Label lblDueDate = new Label
            {
                Text = _task?.DueDate.HasValue == true 
                    ? $"?? H?n: {_task.DueDate.Value:dd/MM/yyyy}" 
                    : "?? Không có h?n",
                Location = new Point(340, 75),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent
            };

            // Created Date
            Label lblCreated = new Label
            {
                Text = _task?.CreatedAt.HasValue == true 
                    ? $"?? T?o: {_task.CreatedAt.Value:dd/MM/yyyy HH:mm}" 
                    : "?? Không rõ",
                Location = new Point(90, 110),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent
            };

            // Updated Date
            Label lblUpdated = new Label
            {
                Text = _task?.UpdatedAt.HasValue == true 
                    ? $"?? C?p nh?t: {_task.UpdatedAt.Value:dd/MM/yyyy HH:mm}" 
                    : "?? Ch?a c?p nh?t",
                Location = new Point(360, 110),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent
            };

            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(statusBadge);
            pnlHeader.Controls.Add(priorityBadge);
            pnlHeader.Controls.Add(lblDueDate);
            pnlHeader.Controls.Add(lblCreated);
            pnlHeader.Controls.Add(lblUpdated);

            this.Controls.Add(pnlHeader);
        }
        #endregion

        #region Stats Panel
        private void CreateStatsPanel()
        {
            pnlStats = new Panel
            {
                Location = new Point(20, 170),
                Size = new Size(1360, 120),
                BackColor = Color.Transparent
            };

            this.Controls.Add(pnlStats);
        }

        private void LoadStatsCards()
        {
            pnlStats.Controls.Clear();

            var cards = new[]
            {
                new { Title = "TH?I GIAN D? KI?N", Value = $"{_task?.EstimatedMinutes ?? 0} phút", Icon = "??", Color = Color.FromArgb(40, 40, 40) },
                new { Title = "TH?I GIAN TH?C T?", Value = $"{_task?.ActualMinutes ?? 0} phút", Icon = "?", Color = Color.FromArgb(40, 40, 40) },
                new { Title = "TI?N ??", Value = CalculateProgress() + "%", Icon = "??", Color = Color.FromArgb(40, 40, 40) },
                new { Title = "HI?U SU?T", Value = CalculateEfficiency() + "%", Icon = "?", Color = Color.FromArgb(40, 40, 40) }
            };

            for (int i = 0; i < cards.Length; i++)
            {
                var card = CreateStatCard(cards[i].Title, cards[i].Value, cards[i].Icon, cards[i].Color);
                card.Location = new Point(i * 340, 0);
                pnlStats.Controls.Add(card);
            }
        }

        private Panel CreateStatCard(string title, string value, string icon, Color bgColor)
        {
            var card = new Panel
            {
                Size = new Size(330, 100),
                BackColor = bgColor,
                Margin = new Padding(5)
            };

            // Icon
            Label lblIcon = new Label
            {
                Text = icon,
                Location = new Point(15, 15),
                Size = new Size(40, 40),
                Font = new Font("Segoe UI Emoji", 24F),
                ForeColor = Color.FromArgb(100, 149, 237),
                BackColor = Color.Transparent
            };

            // Title
            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(70, 15),
                Size = new Size(240, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent
            };

            // Value
            Label lblValue = new Label
            {
                Text = value,
                Location = new Point(70, 40),
                Size = new Size(240, 40),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        private string CalculateProgress()
        {
            if (_task == null) return "0";
            
            if (_task.Status == "Completed") return "100";
            if (_task.Status == "In Progress") return "50";
            return "0";
        }

        private string CalculateEfficiency()
        {
            if (_task == null || !_task.EstimatedMinutes.HasValue || _task.EstimatedMinutes == 0)
                return "N/A";

            if (!_task.ActualMinutes.HasValue || _task.ActualMinutes == 0)
                return "100";

            var efficiency = Math.Round((double)_task.EstimatedMinutes.Value / _task.ActualMinutes.Value * 100, 1);
            return efficiency.ToString("F0");
        }
        #endregion

        #region Charts Panel
        private void CreateChartsPanel()
        {
            pnlCharts = new Panel
            {
                Location = new Point(20, 310),
                Size = new Size(1360, 250),
                BackColor = Color.Transparent
            };

            this.Controls.Add(pnlCharts);
        }

        private void LoadCharts()
        {
            pnlCharts.Controls.Clear();

            // Progress Pie Chart
            chartProgress = CreateProgressChart();
            chartProgress.Location = new Point(0, 0);
            chartProgress.Size = new Size(670, 250);
            pnlCharts.Controls.Add(chartProgress);

            // Time Comparison Bar Chart
            chartTimeSpent = CreateTimeComparisonChart();
            chartTimeSpent.Location = new Point(690, 0);
            chartTimeSpent.Size = new Size(670, 250);
            pnlCharts.Controls.Add(chartTimeSpent);
        }

        private InteractiveChartControl CreateProgressChart()
        {
            var chart = new InteractiveChartControl();
            chart.BackColor = Color.FromArgb(30, 30, 30);
            
            var progress = int.Parse(CalculateProgress());
            var data = new List<InteractiveChartControl.ChartDataPoint>
            {
                new InteractiveChartControl.ChartDataPoint 
                { 
                    Label = "Hoàn thành", 
                    Value = progress,
                    Color = Color.FromArgb(100, 200, 150)
                },
                new InteractiveChartControl.ChartDataPoint 
                { 
                    Label = "Còn l?i", 
                    Value = 100 - progress,
                    Color = Color.FromArgb(60, 60, 60)
                }
            };

            chart.SetData(data, InteractiveChartControl.ChartType.Pie);
            return chart;
        }

        private InteractiveChartControl CreateTimeComparisonChart()
        {
            var chart = new InteractiveChartControl();
            chart.BackColor = Color.FromArgb(30, 30, 30);
            
            var data = new List<InteractiveChartControl.ChartDataPoint>
            {
                new InteractiveChartControl.ChartDataPoint 
                { 
                    Label = "D? ki?n", 
                    Value = _task?.EstimatedMinutes ?? 0,
                    Color = Color.FromArgb(100, 149, 237)
                },
                new InteractiveChartControl.ChartDataPoint 
                { 
                    Label = "Th?c t?", 
                    Value = _task?.ActualMinutes ?? 0,
                    Color = Color.FromArgb(255, 165, 0)
                }
            };

            chart.SetData(data, InteractiveChartControl.ChartType.Bar);
            return chart;
        }
        #endregion

        #region Timeline Panel
        private void CreateTimelinePanel()
        {
            pnlTimeline = new Panel
            {
                Location = new Point(20, 580),
                Size = new Size(670, 250),
                BackColor = Color.FromArgb(30, 30, 30)
            };

            Label lblTimelineTitle = new Label
            {
                Text = "?? Timeline",
                Location = new Point(20, 15),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            pnlTimeline.Controls.Add(lblTimelineTitle);

            this.Controls.Add(pnlTimeline);
        }

        private void LoadTimeline()
        {
            int yPos = 60;

            // Created event
            if (_task?.CreatedAt.HasValue == true)
            {
                var item = CreateTimelineItem("??", "Task ???c t?o", _task.CreatedAt.Value);
                item.Location = new Point(20, yPos);
                pnlTimeline.Controls.Add(item);
                yPos += 50;
            }

            // Status changes (if we had a history table, we'd show it here)
            // For now, show current status
            if (_task?.Status == "In Progress" && _task.UpdatedAt.HasValue)
            {
                var item = CreateTimelineItem("??", "B?t ??u làm vi?c", _task.UpdatedAt.Value);
                item.Location = new Point(20, yPos);
                pnlTimeline.Controls.Add(item);
                yPos += 50;
            }

            if (_task?.Status == "Completed" && _task.UpdatedAt.HasValue)
            {
                var item = CreateTimelineItem("?", "Hoàn thành", _task.UpdatedAt.Value);
                item.Location = new Point(20, yPos);
                pnlTimeline.Controls.Add(item);
                yPos += 50;
            }

            // Due date warning
            if (_task?.DueDate.HasValue == true)
            {
                var daysLeft = (_task.DueDate.Value - DateTime.Now).Days;
                if (daysLeft < 0)
                {
                    var item = CreateTimelineItem("??", $"Quá h?n {Math.Abs(daysLeft)} ngày", _task.DueDate.Value);
                    item.BackColor = Color.FromArgb(80, 30, 30);
                    item.Location = new Point(20, yPos);
                    pnlTimeline.Controls.Add(item);
                }
                else if (daysLeft <= 3)
                {
                    var item = CreateTimelineItem("?", $"Còn {daysLeft} ngày", _task.DueDate.Value);
                    item.BackColor = Color.FromArgb(80, 60, 30);
                    item.Location = new Point(20, yPos);
                    pnlTimeline.Controls.Add(item);
                }
            }
        }

        private Panel CreateTimelineItem(string icon, string text, DateTime date)
        {
            var panel = new Panel
            {
                Size = new Size(620, 40),
                BackColor = Color.FromArgb(40, 40, 40),
                Margin = new Padding(5)
            };

            Label lblIcon = new Label
            {
                Text = icon,
                Location = new Point(10, 10),
                Size = new Size(30, 20),
                Font = new Font("Segoe UI Emoji", 12F),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label lblText = new Label
            {
                Text = text,
                Location = new Point(50, 12),
                Size = new Size(350, 20),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label lblDate = new Label
            {
                Text = date.ToString("dd/MM/yyyy HH:mm"),
                Location = new Point(450, 12),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            panel.Controls.Add(lblIcon);
            panel.Controls.Add(lblText);
            panel.Controls.Add(lblDate);

            return panel;
        }
        #endregion

        #region Activity Panel
        private void CreateActivityPanel()
        {
            pnlActivity = new Panel
            {
                Location = new Point(710, 580),
                Size = new Size(670, 250),
                BackColor = Color.FromArgb(30, 30, 30)
            };

            Label lblActivityTitle = new Label
            {
                Text = "?? Mô T? Task",
                Location = new Point(20, 15),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            pnlActivity.Controls.Add(lblActivityTitle);

            // Description
            TextBox txtDescription = new TextBox
            {
                Location = new Point(20, 60),
                Size = new Size(630, 170),
                Multiline = true,
                ReadOnly = true,
                Text = _task?.Description ?? "Không có mô t?.",
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            pnlActivity.Controls.Add(txtDescription);

            this.Controls.Add(pnlActivity);
        }
        #endregion

        #region Helper Methods
        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "Completed" => Color.FromArgb(100, 200, 150),
                "In Progress" => Color.FromArgb(100, 149, 237),
                "Pending" => Color.FromArgb(255, 165, 0),
                _ => Color.FromArgb(100, 100, 100)
            };
        }

        private Color GetPriorityColor(string priority)
        {
            return priority switch
            {
                "High" => Color.FromArgb(255, 100, 100),
                "Medium" => Color.FromArgb(255, 165, 0),
                "Low" => Color.FromArgb(100, 200, 150),
                _ => Color.FromArgb(100, 100, 100)
            };
        }

        private async void LoadTaskReport()
        {
            try
            {
                LoadStatsCards();
                LoadCharts();
                LoadTimeline();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo: {ex.Message}", "L?i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
