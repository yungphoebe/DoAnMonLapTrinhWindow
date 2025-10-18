using System;
using System.Drawing;
using System.Windows.Forms;
using ToDoList.GUI.Resources;
using ToDoList.GUI.Components;
using ToDoList.GUI.Forms;

namespace ToDoList.GUI.Forms
{
    public partial class DashboardForm : Form
    {
        private Panel pnlHeader;
        private FlowLayoutPanel flowProjects;
        private Panel btnAddProject;
        
        public DashboardForm()
        {
            InitializeComponent();
            LoadProjects();
        }
        
        private void InitializeComponent()
        {
            this.Text = Strings.AppName;
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            
            // Header
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(45, 45, 48)
            };
            
            // App title
            Label lblTitle = new Label
            {
                Location = new Point(30, 25),
                Size = new Size(300, 35),
                Text = Strings.AppName,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            
            // Search box
            TextBox searchBox = new TextBox
            {
                Location = new Point(400, 28),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 12F),
                BackColor = Color.FromArgb(60, 60, 62),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "🔍 Search projects..."
            };
            searchBox.Enter += (s, e) =>
            {
                if (searchBox.Text == "🔍 Search projects...")
                    searchBox.Text = "";
            };
            searchBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                    searchBox.Text = "🔍 Search projects...";
            };
            
            // Settings button
            Button btnSettings = new Button
            {
                Location = new Point(1200, 25),
                Size = new Size(50, 35),
                Text = "⚙️",
                Font = new Font("Segoe UI", 16F),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 72);
            btnSettings.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 92);
            btnSettings.Click += BtnSettings_Click;
            
            // Language button
            Button btnLanguage = new Button
            {
                Location = new Point(1260, 25),
                Size = new Size(50, 35),
                Text = "🌐",
                Font = new Font("Segoe UI", 16F),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLanguage.FlatAppearance.BorderSize = 0;
            btnLanguage.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 72);
            btnLanguage.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 92);
            btnLanguage.Click += BtnLanguage_Click;
            
            // Profile button
            Button btnProfile = new Button
            {
                Location = new Point(1320, 25),
                Size = new Size(40, 40),
                Text = "👤",
                Font = new Font("Segoe UI", 16F),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 149, 237),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 169, 255);
            btnProfile.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 129, 217);
            
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(searchBox);
            pnlHeader.Controls.Add(btnSettings);
            pnlHeader.Controls.Add(btnLanguage);
            pnlHeader.Controls.Add(btnProfile);
            
            // Projects flow panel
            flowProjects = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20, 30, 20, 20)
            };
            
            // Add new project button
            btnAddProject = CreateAddProjectCard();
            flowProjects.Controls.Add(btnAddProject);
            
            this.Controls.Add(flowProjects);
            this.Controls.Add(pnlHeader);
            
            // Bottom navigation
            CreateBottomNav();
        }
        
        private Panel CreateAddProjectCard()
        {
            Panel pnl = new Panel
            {
                Size = new Size(320, 400),
                BackColor = Color.FromArgb(45, 45, 48),
                Cursor = Cursors.Hand,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            Label lblPlus = new Label
            {
                Location = new Point(120, 160),
                Size = new Size(80, 80),
                Text = "+",
                Font = new Font("Segoe UI", 48F),
                ForeColor = Color.FromArgb(100, 149, 237),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            
            Label lblText = new Label
            {
                Location = new Point(60, 250),
                Size = new Size(200, 30),
                Text = "Add New Project",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            
            pnl.Controls.Add(lblPlus);
            pnl.Controls.Add(lblText);
            
            // Improved click handling
            EventHandler clickHandler = (s, e) => AddNewProject();
            pnl.Click += clickHandler;
            lblPlus.Click += clickHandler;
            lblText.Click += clickHandler;
            
            // Enhanced hover effects
            EventHandler mouseEnter = (s, e) => {
                pnl.BackColor = Color.FromArgb(55, 55, 58);
                lblPlus.ForeColor = Color.FromArgb(120, 169, 255);
            };
            EventHandler mouseLeave = (s, e) => {
                pnl.BackColor = Color.FromArgb(45, 45, 48);
                lblPlus.ForeColor = Color.FromArgb(100, 149, 237);
            };
            
            pnl.MouseEnter += mouseEnter;
            pnl.MouseLeave += mouseLeave;
            lblPlus.MouseEnter += mouseEnter;
            lblPlus.MouseLeave += mouseLeave;
            lblText.MouseEnter += mouseEnter;
            lblText.MouseLeave += mouseLeave;
            
            return pnl;
        }
        
        private void CreateBottomNav()
        {
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(45, 45, 48)
            };
            
            Button btnHome = new Button
            {
                Location = new Point(40, 15),
                Size = new Size(100, 30),
                Text = "🏠 Home",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 72);
            
            Button btnReports = new Button
            {
                Location = new Point(160, 15),
                Size = new Size(100, 30),
                Text = "📊 Reports",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 72);
            
            Button btnAddTask = new Button
            {
                Location = new Point(1100, 10),
                Size = new Size(150, 40),
                Text = "Add new task",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(100, 220, 180),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddTask.FlatAppearance.BorderSize = 0;
            
            Button btnHelp = new Button
            {
                Location = new Point(1270, 15),
                Size = new Size(100, 30),
                Text = "❓ Help center",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHelp.FlatAppearance.BorderSize = 0;
            btnHelp.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 72);
            
            pnlBottom.Controls.Add(btnHome);
            pnlBottom.Controls.Add(btnReports);
            pnlBottom.Controls.Add(btnAddTask);
            pnlBottom.Controls.Add(btnHelp);
            
            this.Controls.Add(pnlBottom);
        }
        
        private void LoadProjects()
        {
            // Clear existing (except Add button)
            while (flowProjects.Controls.Count > 1)
            {
                flowProjects.Controls.RemoveAt(0);
            }
            
            // Sample data - TODO: Load from database
            var projects = new[]
            {
                new { Id = 1, Title = "Test", Color = Color.FromArgb(255, 215, 0), Tasks = 2, Minutes = 20 },
                new { Id = 2, Title = "All Lists", Color = Color.FromArgb(100, 149, 237), Tasks = 2, Minutes = 20 },
                new { Id = 3, Title = "Work", Color = Color.FromArgb(255, 99, 71), Tasks = 5, Minutes = 60 }
            };
            
            foreach (var proj in projects)
            {
                var card = new ProjectCard
                {
                    ProjectId = proj.Id,
                    ProjectTitle = proj.Title,
                    ProjectColor = proj.Color,
                    PendingTasksCount = proj.Tasks,
                    TotalEstimateMinutes = proj.Minutes,
                    Tasks = new List<TaskInfo>
                    {
                        new TaskInfo { TaskId = 1, Title = "Click Blitzit Now", IsCompleted = false, EstimateMinutes = 10 },
                        new TaskInfo { TaskId = 2, Title = "Mark me as done", IsCompleted = false, EstimateMinutes = 10 }
                    }
                };
                
                card.UpdateData();
                card.CardClicked += Card_CardClicked;
                
                flowProjects.Controls.Add(card);
                flowProjects.Controls.SetChildIndex(card, flowProjects.Controls.Count - 2);
            }
        }
        
        private void Card_CardClicked(object? sender, EventArgs e)
        {
            if (sender is ProjectCard card)
            {
                var kanbanForm = new KanbanBoardForm(card.ProjectId, card.ProjectTitle, card.ProjectColor);
                kanbanForm.ShowDialog();
                
                // Refresh after closing
                LoadProjects();
            }
        }
        
        private void AddNewProject()
        {
            // Open CreateListForm
            using (var form = new CreateListForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProjects();
                }
            }
        }
        
        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Settings coming soon!",
                Strings.Settings,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        
        private void BtnLanguage_Click(object? sender, EventArgs e)
        {
            using (var form = new LanguageSettingsForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Refresh UI
                    this.Text = Strings.AppName;
                    LoadProjects();
                }
            }
        }
    }
}
