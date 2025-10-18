using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ToDoList.GUI.Resources;

namespace ToDoList.GUI.Components
{
    public class ProjectCard : Panel
    {
        private Panel pnlHeader;
        private Label lblIcon;
        private Label lblTitle;
        private Label lblTaskCount;
        private Label lblEstimate;
        private Label lblAllClear;
        private PictureBox picAllClear;
        private FlowLayoutPanel flowTasks;
        private bool isHovered = false;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int ProjectId { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string ProjectTitle { get; set; } = string.Empty;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Color ProjectColor { get; set; } = Color.FromArgb(100, 149, 237);
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int PendingTasksCount { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int TotalEstimateMinutes { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public List<TaskInfo> Tasks { get; set; } = new List<TaskInfo>();
        
        public event EventHandler? CardClicked;
        public event EventHandler<int>? TaskClicked;
        
        public ProjectCard()
        {
            InitializeComponents();
            SetupEvents();
        }
        
        private void InitializeComponents()
        {
            this.Size = new Size(320, 400);
            this.BackColor = Color.FromArgb(37, 37, 38);
            this.Cursor = Cursors.Hand;
            this.Margin = new Padding(10);
            
            // Header panel
            pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(320, 60),
                BackColor = Color.FromArgb(45, 45, 48),
                Dock = DockStyle.Top
            };
            
            // Icon
            lblIcon = new Label
            {
                Location = new Point(15, 15),
                Size = new Size(35, 35),
                Text = "T",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = ProjectColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Title
            lblTitle = new Label
            {
                Location = new Point(60, 15),
                Size = new Size(200, 30),
                Text = "Project Title",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            
            // More button
            Button btnMore = new Button
            {
                Location = new Point(270, 15),
                Size = new Size(35, 35),
                Text = "⋮",
                Font = new Font("Segoe UI", 18F),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMore.FlatAppearance.BorderSize = 0;
            btnMore.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 72);
            
            pnlHeader.Controls.Add(lblIcon);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnMore);
            
            // Tasks flow panel
            flowTasks = new FlowLayoutPanel
            {
                Location = new Point(0, 60),
                Size = new Size(320, 250),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(15, 10, 15, 10)
            };
            
            // All Clear section
            Panel pnlAllClear = new Panel
            {
                Location = new Point(0, 310),
                Size = new Size(320, 70),
                BackColor = Color.Transparent,
                Dock = DockStyle.Bottom
            };
            
            picAllClear = new PictureBox
            {
                Location = new Point(135, 10),
                Size = new Size(50, 50),
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent
            };
            // Draw circle with checkmark
            DrawAllClearIcon();
            
            lblAllClear = new Label
            {
                Location = new Point(100, 65),
                Size = new Size(120, 20),
                Text = "ALL CLEAR",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 200, 100),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            
            pnlAllClear.Controls.Add(picAllClear);
            pnlAllClear.Controls.Add(lblAllClear);
            
            // Footer
            Panel pnlFooter = new Panel
            {
                Location = new Point(0, 380),
                Size = new Size(320, 40),
                BackColor = Color.FromArgb(30, 30, 30),
                Dock = DockStyle.Bottom
            };
            
            lblTaskCount = new Label
            {
                Location = new Point(15, 10),
                Size = new Size(150, 20),
                Text = "0 pending tasks",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.Transparent
            };
            
            lblEstimate = new Label
            {
                Location = new Point(180, 10),
                Size = new Size(120, 20),
                Text = "Est: 0min",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            pnlFooter.Controls.Add(lblTaskCount);
            pnlFooter.Controls.Add(lblEstimate);
            
            this.Controls.Add(flowTasks);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlAllClear);
            this.Controls.Add(pnlFooter);
        }
        
        private void DrawAllClearIcon()
        {
            Bitmap bmp = new Bitmap(50, 50);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Draw circle
                using (Pen pen = new Pen(Color.FromArgb(100, 200, 100), 3))
                {
                    g.DrawEllipse(pen, 5, 5, 40, 40);
                }
                
                // Draw checkmark
                using (Pen pen = new Pen(Color.FromArgb(100, 200, 100), 3))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    g.DrawLine(pen, 15, 25, 22, 32);
                    g.DrawLine(pen, 22, 32, 35, 18);
                }
            }
            picAllClear.Image = bmp;
        }
        
        private void SetupEvents()
        {
            this.Click += (s, e) => CardClicked?.Invoke(this, e);
            pnlHeader.Click += (s, e) => CardClicked?.Invoke(this, e);
            lblTitle.Click += (s, e) => CardClicked?.Invoke(this, e);
            
            this.MouseEnter += ProjectCard_MouseEnter;
            this.MouseLeave += ProjectCard_MouseLeave;
        }
        
        private void ProjectCard_MouseEnter(object? sender, EventArgs e)
        {
            isHovered = true;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Invalidate();
        }
        
        private void ProjectCard_MouseLeave(object? sender, EventArgs e)
        {
            isHovered = false;
            this.BorderStyle = BorderStyle.None;
            this.Invalidate();
        }
        
        public void UpdateData()
        {
            lblTitle.Text = TruncateText(ProjectTitle, 20);
            lblIcon.Text = ProjectTitle.Length > 0 ? ProjectTitle[0].ToString().ToUpper() : "P";
            lblIcon.BackColor = ProjectColor;
            lblTaskCount.Text = $"{PendingTasksCount} {Strings.PendingTasks.ToLower()}";
            lblEstimate.Text = $"Est: {TotalEstimateMinutes}min";
            
            // Show/Hide All Clear
            bool allDone = PendingTasksCount == 0 && Tasks.Count > 0;
            lblAllClear.Visible = allDone;
            picAllClear.Visible = allDone;
            
            // Load tasks
            flowTasks.Controls.Clear();
            foreach (var task in Tasks.Take(2)) // Show only first 2 tasks
            {
                var taskItem = CreateTaskItem(task);
                flowTasks.Controls.Add(taskItem);
            }
        }
        
        private Panel CreateTaskItem(TaskInfo task)
        {
            Panel pnl = new Panel
            {
                Size = new Size(280, 50),
                BackColor = Color.FromArgb(50, 50, 52),
                Margin = new Padding(0, 5, 0, 5),
                Cursor = Cursors.Hand
            };
            
            CheckBox chk = new CheckBox
            {
                Location = new Point(10, 15),
                Size = new Size(20, 20),
                Checked = task.IsCompleted,
                BackColor = Color.Transparent
            };
            
            Label lblTaskTitle = new Label
            {
                Location = new Point(40, 12),
                Size = new Size(180, 20),
                Text = TruncateText(task.Title, 25),
                Font = new Font("Segoe UI", 10F, task.IsCompleted ? FontStyle.Strikeout : FontStyle.Regular),
                ForeColor = task.IsCompleted ? Color.Gray : Color.White,
                BackColor = Color.Transparent
            };
            
            Label lblTime = new Label
            {
                Location = new Point(220, 15),
                Size = new Size(50, 20),
                Text = $"{task.EstimateMinutes}min",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            pnl.Controls.Add(chk);
            pnl.Controls.Add(lblTaskTitle);
            pnl.Controls.Add(lblTime);
            
            pnl.Click += (s, e) => TaskClicked?.Invoke(this, task.TaskId);
            lblTaskTitle.Click += (s, e) => TaskClicked?.Invoke(this, task.TaskId);
            
            return pnl;
        }
        
        private string TruncateText(string text, int maxLength)
        {
            if (text.Length <= maxLength) return text;
            return text.Substring(0, maxLength - 3) + "...";
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Draw rounded border
            using (GraphicsPath path = GetRoundedRectangle(this.ClientRectangle, 12))
            {
                this.Region = new Region(path);
                
                if (isHovered)
                {
                    using (Pen pen = new Pen(ProjectColor, 2))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        Rectangle bounds = this.ClientRectangle;
                        bounds.Inflate(-1, -1);
                        using (GraphicsPath borderPath = GetRoundedRectangle(bounds, 12))
                        {
                            e.Graphics.DrawPath(pen, borderPath);
                        }
                    }
                }
            }
        }
        
        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();
            
            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }
            
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            
            path.CloseFigure();
            return path;
        }
    }
    
    public class TaskInfo
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int EstimateMinutes { get; set; }
    }
}
