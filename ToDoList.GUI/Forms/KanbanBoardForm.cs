using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ToDoList.GUI.Resources;
using ToDoList.GUI.Components;

namespace ToDoList.GUI.Forms
{
    public partial class KanbanBoardForm : Form
    {
        private Panel pnlHeader;
        private Label lblProjectTitle;
        private Label lblProjectInfo;
        private Button btnBack;
        private Button btnSettings;
        private FlowLayoutPanel flowColumns;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int ProjectId { get; set; }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string ProjectTitle { get; set; } = string.Empty;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Color ProjectColor { get; set; }
        
        public KanbanBoardForm()
        {
            InitializeComponent();
        }
        
        public KanbanBoardForm(int projectId, string projectTitle, Color projectColor)
        {
            ProjectId = projectId;
            ProjectTitle = projectTitle;
            ProjectColor = projectColor;
            
            InitializeComponent();
            LoadTasks();
        }
        
        private void InitializeComponent()
        {
            this.Text = ProjectTitle;
            this.Size = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            
            // Header
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(45, 45, 48)
            };
            
            // Back button
            btnBack = new Button
            {
                Location = new Point(20, 20),
                Size = new Size(100, 40),
                Text = "? BACK",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += BtnBack_Click;
            
            // Project icon
            Label lblIcon = new Label
            {
                Location = new Point(140, 20),
                Size = new Size(40, 40),
                Text = ProjectTitle.Length > 0 ? ProjectTitle[0].ToString() : "P",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = ProjectColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Project title
            lblProjectTitle = new Label
            {
                Location = new Point(190, 20),
                Size = new Size(300, 30),
                Text = ProjectTitle,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            
            // Project info
            lblProjectInfo = new Label
            {
                Location = new Point(190, 48),
                Size = new Size(500, 20),
                Text = "This list has 2 pending tasks, Est: 20min",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.Transparent
            };
            
            // Settings button
            btnSettings = new Button
            {
                Location = new Point(1250, 20),
                Size = new Size(120, 40),
                Text = "Settings",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 62),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSettings.FlatAppearance.BorderSize = 0;
            
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblIcon);
            pnlHeader.Controls.Add(lblProjectTitle);
            pnlHeader.Controls.Add(lblProjectInfo);
            pnlHeader.Controls.Add(btnSettings);
            
            // Columns flow panel
            flowColumns = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20)
            };
            
            // Create columns
            CreateColumn("Backlog", 0);
            CreateColumn("This Week", 1);
            CreateColumn("Today", 2);
            CreateColumn("Done", 3);
            
            this.Controls.Add(flowColumns);
            this.Controls.Add(pnlHeader);
        }
        
        private void CreateColumn(string title, int columnIndex)
        {
            Panel pnlColumn = new Panel
            {
                Size = new Size(350, 650),
                BackColor = Color.FromArgb(37, 37, 38),
                Margin = new Padding(10),
                Padding = new Padding(15)
            };
            
            // Column header
            Panel pnlColumnHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.Transparent
            };
            
            Label lblColumnTitle = new Label
            {
                Location = new Point(0, 10),
                Size = new Size(250, 30),
                Text = title,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            
            // Progress info
            Label lblColumnInfo = new Label
            {
                Location = new Point(260, 10),
                Size = new Size(60, 30),
                Text = "20min",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            Button btnAdd = new Button
            {
                Location = new Point(320, 10),
                Size = new Size(30, 30),
                Text = "+",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            
            pnlColumnHeader.Controls.Add(lblColumnTitle);
            pnlColumnHeader.Controls.Add(lblColumnInfo);
            pnlColumnHeader.Controls.Add(btnAdd);
            
            // Progress bar
            Panel pnlProgress = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(320, 4),
                BackColor = Color.FromArgb(60, 60, 62)
            };
            
            Panel pnlProgressFill = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(160, 4),
                BackColor = ProjectColor
            };
            pnlProgress.Controls.Add(pnlProgressFill);
            
            Label lblProgressText = new Label
            {
                Location = new Point(0, 58),
                Size = new Size(320, 20),
                Text = "1/3 Done",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            // Tasks container
            FlowLayoutPanel flowTasks = new FlowLayoutPanel
            {
                Location = new Point(0, 85),
                Size = new Size(320, 480),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 10)
            };
            
            // Add button at bottom
            Button btnAddTask = new Button
            {
                Location = new Point(0, 570),
                Size = new Size(320, 40),
                Text = "+ ADD TASK",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft
            };
            btnAddTask.FlatAppearance.BorderSize = 0;
            
            // All Clear section (only for Done column)
            if (columnIndex == 3)
            {
                Panel pnlAllClear = new Panel
                {
                    Location = new Point(100, 300),
                    Size = new Size(120, 100),
                    BackColor = Color.Transparent
                };
                
                // Draw checkmark circle
                PictureBox picCheck = new PictureBox
                {
                    Location = new Point(35, 0),
                    Size = new Size(50, 50),
                    SizeMode = PictureBoxSizeMode.CenterImage
                };
                DrawCheckIcon(picCheck);
                
                Label lblAllClear = new Label
                {
                    Location = new Point(0, 60),
                    Size = new Size(120, 20),
                    Text = "All Clear",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(100, 200, 100),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                
                pnlAllClear.Controls.Add(picCheck);
                pnlAllClear.Controls.Add(lblAllClear);
                flowTasks.Controls.Add(pnlAllClear);
            }
            
            pnlColumn.Controls.Add(pnlColumnHeader);
            pnlColumn.Controls.Add(pnlProgress);
            pnlColumn.Controls.Add(lblProgressText);
            pnlColumn.Controls.Add(flowTasks);
            pnlColumn.Controls.Add(btnAddTask);
            
            // Add sample tasks
            if (columnIndex <= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    var taskCard = CreateTaskCard($"Sample Task {i + 1}", 10, false);
                    flowTasks.Controls.Add(taskCard);
                }
            }
            
            flowColumns.Controls.Add(pnlColumn);
        }
        
        private Panel CreateTaskCard(string title, int estimateMinutes, bool isCompleted)
        {
            Panel pnl = new Panel
            {
                Size = new Size(320, 100),
                BackColor = Color.FromArgb(50, 50, 52),
                Margin = new Padding(0, 5, 0, 5),
                Cursor = Cursors.Hand,
                Padding = new Padding(15)
            };
            
            CheckBox chk = new CheckBox
            {
                Location = new Point(15, 15),
                Size = new Size(20, 20),
                Checked = isCompleted,
                BackColor = Color.Transparent
            };
            
            Label lblTitle = new Label
            {
                Location = new Point(45, 12),
                Size = new Size(240, 25),
                Text = title,
                Font = new Font("Segoe UI", 11F, isCompleted ? FontStyle.Strikeout : FontStyle.Bold),
                ForeColor = isCompleted ? Color.Gray : Color.White,
                BackColor = Color.Transparent
            };
            
            Label lblTime = new Label
            {
                Location = new Point(15, 45),
                Size = new Size(100, 20),
                Text = $"{estimateMinutes}min",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent
            };
            
            Label lblEstimateRight = new Label
            {
                Location = new Point(250, 45),
                Size = new Size(50, 20),
                Text = $"0min",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight
            };
            
            // Action buttons
            Button btnMenu = new Button
            {
                Location = new Point(15, 70),
                Size = new Size(25, 25),
                Text = "?",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            
            Button btnCopy = new Button
            {
                Location = new Point(45, 70),
                Size = new Size(25, 25),
                Text = "??",
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCopy.FlatAppearance.BorderSize = 0;
            
            pnl.Controls.Add(chk);
            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblTime);
            pnl.Controls.Add(lblEstimateRight);
            pnl.Controls.Add(btnMenu);
            pnl.Controls.Add(btnCopy);
            
            // Hover effect
            pnl.MouseEnter += (s, e) => pnl.BackColor = Color.FromArgb(60, 60, 62);
            pnl.MouseLeave += (s, e) => pnl.BackColor = Color.FromArgb(50, 50, 52);
            
            return pnl;
        }
        
        private void DrawCheckIcon(PictureBox pic)
        {
            Bitmap bmp = new Bitmap(50, 50);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                using (Pen pen = new Pen(Color.FromArgb(100, 200, 100), 3))
                {
                    g.DrawEllipse(pen, 5, 5, 40, 40);
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    g.DrawLine(pen, 15, 25, 22, 32);
                    g.DrawLine(pen, 22, 32, 35, 18);
                }
            }
            pic.Image = bmp;
        }
        
        private void LoadTasks()
        {
            // TODO: Load tasks from database
            lblProjectInfo.Text = $"This list has 2 pending tasks, Est: 20min";
        }
        
        private void BtnBack_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
