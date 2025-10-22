using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Forms
{
    public class CuculistDetailForm : Form
    {
        private Project _project;
        private ToDoListContext _context;
        private System.Windows.Forms.Timer _timer;
        private TimeSpan _elapsedTime;
        private TodoListApp.DAL.Models.Task? _activeTask;
        private bool _isTimerRunning;

        // UI Controls
        private Label lblProjectIcon = null!;
        private Label lblProjectTitle = null!;
        private Label lblEstimate = null!;
        private Label lblProgress = null!;
        private FlowLayoutPanel flowTasks = null!;
        private Button btnFocusMode = null!;
        private Button btnDoneForDay = null!;
        private ProgressBar progressBar = null!;
        private Button btnMenu = null!;
        private Button btnExpand = null!;

        public CuculistDetailForm(Project project)
        {
            _project = project;
            _context = new ToDoListContext();
            _elapsedTime = TimeSpan.Zero;
            _isTimerRunning = false;

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;

            InitializeComponent();
            LoadTasks();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(390, 950);
            this.Text = "Cuculist - Today";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.FormBorderStyle = FormBorderStyle.None;

            CreateControls();
        }

        private void CreateControls()
        {
            // ===== HEADER =====
            Panel pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(390, 80),
                BackColor = Color.White
            };

            lblProjectIcon = new Label
            {
                Text = _project.ProjectName?.Substring(0, 1).ToUpper() ?? "T",
                Location = new Point(20, 20),
                Size = new Size(40, 40),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = !string.IsNullOrEmpty(_project.ColorCode) ? 
                    ColorTranslator.FromHtml(_project.ColorCode) : 
                    Color.FromArgb(200, 180, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblProjectTitle = new Label
            {
                Text = _project.ProjectName ?? "Today",
                Location = new Point(70, 15),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent
            };

            Button btnSettings1 = new Button
            {
                Text = "\u2699", // Settings gear icon
                Location = new Point(280, 15),
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Symbol", 14F),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnSettings1.FlatAppearance.BorderSize = 0;

            btnMenu = new Button
            {
                Text = "\u2715", // X/Close icon
                Location = new Point(320, 15),
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.Click += (s, e) => this.Close();

            btnExpand = new Button
            {
                Text = "\u25A2", // Expand/window icon
                Location = new Point(360, 15),
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Symbol", 14F),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnExpand.FlatAppearance.BorderSize = 0;
            btnExpand.Click += BtnExpand_Click;

            pnlHeader.Controls.Add(lblProjectIcon);
            pnlHeader.Controls.Add(lblProjectTitle);
            pnlHeader.Controls.Add(btnSettings1);
            pnlHeader.Controls.Add(btnMenu);
            pnlHeader.Controls.Add(btnExpand);

            // ===== ESTIMATE & PROGRESS =====
            lblEstimate = new Label
            {
                Text = "Est: 20min",
                Location = new Point(20, 95),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent
            };

            lblProgress = new Label
            {
                Text = "0/2 Done",
                Location = new Point(320, 95),
                Size = new Size(70, 20),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };

            progressBar = new ProgressBar
            {
                Location = new Point(20, 125),
                Size = new Size(350, 6),
                Style = ProgressBarStyle.Continuous,
                Value = 0,
                Maximum = 100
            };

            // ===== TASKS CONTAINER =====
            flowTasks = new FlowLayoutPanel
            {
                Location = new Point(20, 150),
                Size = new Size(350, 600),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            // ===== BOTTOM BUTTONS =====
            btnFocusMode = new Button
            {
                Text = "\uD83C\uDFAF Focus mode", // Target emoji + text
                Location = new Point(20, 780),
                Size = new Size(350, 55),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(100, 220, 180),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFocusMode.FlatAppearance.BorderSize = 0;
            btnFocusMode.Click += BtnFocusMode_Click;

            btnDoneForDay = new Button
            {
                Text = "\u2713 Done for the day", // Checkmark + text
                Location = new Point(20, 850),
                Size = new Size(350, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 12F),
                Cursor = Cursors.Hand
            };
            btnDoneForDay.FlatAppearance.BorderSize = 0;
            btnDoneForDay.Click += BtnDoneForDay_Click;

            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblEstimate);
            this.Controls.Add(lblProgress);
            this.Controls.Add(progressBar);
            this.Controls.Add(flowTasks);
            this.Controls.Add(btnFocusMode);
            this.Controls.Add(btnDoneForDay);
        }

        private async void LoadTasks()
        {
            try
            {
                var tasks = await _context.Tasks
                    .Where(t => t.ProjectId == _project.ProjectId && t.IsDeleted != true)
                    .OrderBy(t => t.Status == "Completed" ? 1 : 0)
                    .ThenBy(t => t.CreatedAt)
                    .ToListAsync();

                flowTasks.Controls.Clear();

                var totalTasks = tasks.Count;
                var completedTasks = tasks.Count(t => t.Status == "Completed");
                var totalMinutes = tasks.Where(t => t.EstimatedMinutes.HasValue).Sum(t => t.EstimatedMinutes.Value);

                // Update header info
                lblEstimate.Text = $"Est: {totalMinutes}min";
                lblProgress.Text = $"{completedTasks}/{totalTasks} Done";
                progressBar.Value = totalTasks > 0 ? (completedTasks * 100 / totalTasks) : 0;

                foreach (var task in tasks)
                {
                    AddTaskCard(task);
                }

                AddTaskButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i khi t?i tasks: {ex.Message}", "L?i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddTaskCard(TodoListApp.DAL.Models.Task task)
        {
            Panel taskCard = new Panel
            {
                Width = 340,
                Height = task.Status == "Completed" ? 60 : 115, // ? Increased from 100 to 115
                BackColor = Color.White,
                Margin = new Padding(0, 5, 0, 5)
            };

            taskCard.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, taskCard.Width - 1, taskCard.Height - 1);
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            if (task.Status != "Completed")
            {
                Label lblTaskTitle = new Label
                {
                    Text = task.Title,
                    Location = new Point(15, 10), // ? Adjusted Y from 15 to 10
                    Size = new Size(180, 30), // ? Increased height to 30
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold), // ? Reduced from 11F
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent,
                    AutoEllipsis = true
                };

                Label lblTimer = new Label
                {
                    Name = "lblTimer", // ? Add Name property
                    Text = task == _activeTask && _isTimerRunning ? 
                        string.Format("{0:00}:{1:00}:{2:00}", 
                            (int)_elapsedTime.TotalHours, 
                            _elapsedTime.Minutes, 
                            _elapsedTime.Seconds) : 
                        "00:00:00",
                    Location = new Point(195, 10), // ? Moved left from 205 to 195
                    Size = new Size(130, 30), // ? Increased width from 120 to 130
                    Font = new Font("Consolas", 11F, FontStyle.Bold), // ? Reduced from 13F to 11F
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleRight
                };

                Button btnMarkDone = new Button
                {
                    Text = "Mark me as done",
                    Location = new Point(15, 45), // ? Moved down
                    Size = new Size(310, 30), // ? Increased height
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Green,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 9.5F), // ? Adjusted font size
                    Cursor = Cursors.Hand,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                btnMarkDone.FlatAppearance.BorderSize = 0;
                btnMarkDone.Click += async (s, e) => await MarkTaskCompleted(task);

                Label lblEstTime = new Label
                {
                    Text = $"{task.EstimatedMinutes ?? 0}min",
                    Location = new Point(15, 80), // ? Moved down
                    Size = new Size(100, 25), // ? Increased height
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent
                };

                Label lblActualTime = new Label
                {
                    Text = $"{task.ActualMinutes ?? 0}min",
                    Location = new Point(225, 80), // ? Moved down
                    Size = new Size(100, 25), // ? Increased height
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleRight
                };

                taskCard.Controls.Add(lblTaskTitle);
                taskCard.Controls.Add(lblTimer);
                taskCard.Controls.Add(btnMarkDone);
                taskCard.Controls.Add(lblEstTime);
                taskCard.Controls.Add(lblActualTime);
            }
            else
            {
                Label lblCheckmark = new Label
                {
                    Text = "\u2713",
                    Location = new Point(15, 15),
                    Size = new Size(30, 30),
                    Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 200, 100),
                    BackColor = Color.Transparent
                };

                Label lblTitle = new Label
                {
                    Text = task.Title,
                    Location = new Point(50, 20),
                    Size = new Size(200, 20),
                    Font = new Font("Segoe UI", 10F, FontStyle.Strikeout),
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    AutoEllipsis = true
                };

                string priorityText = task.Priority?.Substring(0, 1) ?? "M";
                Label lblPriority = new Label
                {
                    Text = priorityText,
                    Location = new Point(300, 15),
                    Size = new Size(25, 25),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = GetPriorityColor(task.Priority),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                taskCard.Controls.Add(lblCheckmark);
                taskCard.Controls.Add(lblTitle);
                taskCard.Controls.Add(lblPriority);
            }

            flowTasks.Controls.Add(taskCard);
        }

        private void AddTaskButton()
        {
            Button btnAddTask = new Button
            {
                Text = "+ ADD TASK",
                Width = 340,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 10, 0, 0)
            };
            btnAddTask.FlatAppearance.BorderSize = 0;
            btnAddTask.Click += BtnAddTask_Click;

            flowTasks.Controls.Add(btnAddTask);
        }

        private Color GetPriorityColor(string? priority)
        {
            return priority?.ToLower() switch
            {
                "high" => Color.FromArgb(255, 80, 80),
                "medium" => Color.FromArgb(255, 165, 0),
                "low" => Color.FromArgb(100, 200, 100),
                _ => Color.Gray
            };
        }

        private async System.Threading.Tasks.Task MarkTaskCompleted(TodoListApp.DAL.Models.Task task)
        {
            try
            {
                task.Status = "Completed";
                task.UpdatedAt = DateTime.Now;
                task.ActualMinutes = (int)_elapsedTime.TotalMinutes;

                await _context.SaveChangesAsync();
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFocusMode_Click(object? sender, EventArgs e)
        {
            TodoListApp.DAL.Models.Task? firstTask = null;
            
            using (var localContext = new ToDoListContext())
            {
                firstTask = localContext.Tasks
                    .FirstOrDefault(t => t.ProjectId == _project.ProjectId && 
                                       t.Status != "Completed" && 
                                       t.IsDeleted != true);
            }

            using (var miniForm = new CuculistMiniForm(_project, firstTask, _elapsedTime))
            {
                miniForm.ShowDialog();
                    
                if (miniForm.Tag is TimeSpan elapsed)
                {
                    _elapsedTime = elapsed;
                }
            }

            LoadTasks();
        }

        private void BtnAddTask_Click(object? sender, EventArgs e)
        {
            using (var addTaskForm = new AddTaskForm(_project))
            {
                if (addTaskForm.ShowDialog() == DialogResult.OK)
                {
                    LoadTasks();
                }
            }
        }

        private void BtnDoneForDay_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Hoàn thành công vi?c trong ngày?", "Xác nh?n",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnExpand_Click(object? sender, EventArgs e)
        {
            TodoListApp.DAL.Models.Task? firstTask = null;
            
            using (var localContext = new ToDoListContext())
            {
                firstTask = localContext.Tasks
                    .FirstOrDefault(t => t.ProjectId == _project.ProjectId && 
                                       t.Status != "Completed" && 
                                       t.IsDeleted != true);
            }

            using (var miniForm = new CuculistMiniForm(_project, firstTask, _elapsedTime))
            {
                this.Hide();
                
                miniForm.ShowDialog();
                
                if (miniForm.Tag is TimeSpan elapsed)
                {
                    _elapsedTime = elapsed;
                }
                
                this.Show();
            }

            LoadTasks();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_activeTask != null && _isTimerRunning)
            {
                _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
                
                // ? FIX: Update timer label with proper format
                foreach (Control control in flowTasks.Controls)
                {
                    if (control is Panel taskCard)
                    {
                        foreach (Control taskControl in taskCard.Controls)
                        {
                            if (taskControl is Label lblTimer && lblTimer.Name == "lblTimer")
                            {
                                // ? FORMAT ?ÚNG: Hi?n th? giây v?i 2 ch? s?
                                lblTimer.Text = string.Format("{0:00}:{1:00}:{2:00}", 
                                    (int)_elapsedTime.TotalHours,
                                    _elapsedTime.Minutes,
                                    _elapsedTime.Seconds);
                            }
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Stop();
                _timer?.Dispose();
                _context?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
