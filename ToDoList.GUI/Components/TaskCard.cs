using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ToDoList.GUI.Resources;

namespace ToDoList.GUI.Components
{
    public class TaskCard : Panel
    {
        private Label lblTitle;
        private Label lblTaskCount;
        private Label lblEstimate;
        private CheckBox chkComplete;
        private Panel pnlColor;
        private bool isHovered = false;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int TaskId { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string TaskTitle { get; set; } = string.Empty;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int PendingTasks { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int TotalTasks { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int EstimateMinutes { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Color CardColor { get; set; } = Color.FromArgb(100, 149, 237);
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool IsCompleted { get; set; }
        
        public event EventHandler? CardClicked;
        public event EventHandler? TaskCompleted;
        
        public TaskCard()
        {
            InitializeComponents();
            SetupEvents();
        }
        
        private void InitializeComponents()
        {
            this.Size = new Size(280, 120);
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.Cursor = Cursors.Hand;
            this.Padding = new Padding(15);
            
            // Color indicator panel
            pnlColor = new Panel
            {
                Size = new Size(4, 90),
                Location = new Point(5, 15),
                BackColor = CardColor
            };
            
            // Checkbox
            chkComplete = new CheckBox
            {
                Location = new Point(20, 20),
                Size = new Size(20, 20),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            chkComplete.CheckedChanged += (s, e) => TaskCompleted?.Invoke(this, e);
            
            // Title
            lblTitle = new Label
            {
                Location = new Point(50, 18),
                Size = new Size(210, 25),
                Text = "Task Title",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            
            // Task count
            lblTaskCount = new Label
            {
                Location = new Point(50, 50),
                Size = new Size(210, 20),
                Text = "2 pending tasks",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.Transparent
            };
            
            // Estimate time
            lblEstimate = new Label
            {
                Location = new Point(50, 75),
                Size = new Size(210, 20),
                Text = "Est: 20min",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent
            };
            
            this.Controls.Add(pnlColor);
            this.Controls.Add(chkComplete);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblTaskCount);
            this.Controls.Add(lblEstimate);
        }
        
        private void SetupEvents()
        {
            this.Click += (s, e) => CardClicked?.Invoke(this, e);
            lblTitle.Click += (s, e) => CardClicked?.Invoke(this, e);
            lblTaskCount.Click += (s, e) => CardClicked?.Invoke(this, e);
            lblEstimate.Click += (s, e) => CardClicked?.Invoke(this, e);
            
            this.MouseEnter += TaskCard_MouseEnter;
            this.MouseLeave += TaskCard_MouseLeave;
            lblTitle.MouseEnter += (s, e) => TaskCard_MouseEnter(s, e);
            lblTitle.MouseLeave += (s, e) => TaskCard_MouseLeave(s, e);
        }
        
        private void TaskCard_MouseEnter(object? sender, EventArgs e)
        {
            isHovered = true;
            this.BackColor = Color.FromArgb(55, 55, 58);
            this.Invalidate();
        }
        
        private void TaskCard_MouseLeave(object? sender, EventArgs e)
        {
            isHovered = false;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.Invalidate();
        }
        
        public void UpdateData()
        {
            lblTitle.Text = TruncateText(TaskTitle, 30);
            lblTaskCount.Text = $"{PendingTasks} {Strings.PendingTasks.ToLower()}";
            lblEstimate.Text = $"Est: {EstimateMinutes}min";
            pnlColor.BackColor = CardColor;
            chkComplete.Checked = IsCompleted;
            
            if (IsCompleted)
            {
                lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Strikeout);
                lblTitle.ForeColor = Color.FromArgb(120, 120, 120);
            }
            else
            {
                lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                lblTitle.ForeColor = Color.White;
            }
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
            using (GraphicsPath path = GetRoundedRectangle(this.ClientRectangle, 8))
            {
                this.Region = new Region(path);
                
                if (isHovered)
                {
                    using (Pen pen = new Pen(CardColor, 2))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.DrawPath(pen, path);
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
            
            // Top left arc
            path.AddArc(arc, 180, 90);
            
            // Top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            
            // Bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            
            // Bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            
            path.CloseFigure();
            return path;
        }
    }
}
