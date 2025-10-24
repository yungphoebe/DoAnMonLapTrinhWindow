using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using ToDoList.GUI.Helpers;

namespace ToDoList.GUI
{
    public partial class Form1 : Form
    {
        private ToDoListContext? _context;
        private int _userId = 1; // Tạm thời hardcode user ID
        private bool _justCreatedList = false; // Add this field at class level

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            UpdateGreetingLabels(); // Update greeting with user name
            LoadProjectsFromDatabase();
            btnCreateNewList.Click += BtnCreateNewList_Click;
            
            // ✨ Add Search button click handler
            btnSearch.Click += BtnSearch_Click;
            
            // Add test button (for development only)
            AddTestButton();
        }

        // ✨ NEW: Search button click handler
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            using (var searchForm = new Forms.SearchForm())
            {
                searchForm.ShowDialog();
                // Reload data after search form closes (in case user made changes)
                LoadProjectsFromDatabase();
            }
        }

        private void UpdateGreetingLabels()
        {
            // Update greeting label with user name
            string timeOfDay = GetTimeOfDay();
            lblGreeting.Text = $"Chào {timeOfDay}, {UserSession.GetDisplayName()}!";
            
            // Update subtitle
            lblUserName.Text = "Tuyệt vời! Bạn đang làm việc rất chăm chỉ.";
        }

        private string GetTimeOfDay()
        {
            int hour = DateTime.Now.Hour;
            if (hour < 12)
                return "buổi sáng";
            else if (hour < 18)
                return "buổi chiều";
            else
                return "buổi tối";
        }

        private void AddTestButton()
        {
            Button btnTest = new Button
            {
                Text = "Test",
                Location = new Point(10, 10),
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(100, 200, 150),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Black
            };
            btnTest.FlatAppearance.BorderSize = 0;
            btnTest.Click += BtnTest_Click;
            
            Button btnTestDB = new Button
            {
                Text = "Test DB",
                Location = new Point(80, 10),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(200, 100, 100),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            btnTestDB.FlatAppearance.BorderSize = 0;
            btnTestDB.Click += BtnTestDB_Click;
            
            Button btnTestData = new Button
            {
                Text = "Test Data",
                Location = new Point(160, 10),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(100, 100, 200),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            btnTestData.FlatAppearance.BorderSize = 0;
            btnTestData.Click += BtnTestData_Click;
            
            Button btnReports = new Button
            {
                Text = "📊 Báo cáo",
                Location = new Point(250, 10),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(100, 149, 237),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 169, 255);
            btnReports.Click += BtnReports_Click;
            
            // ✨ NEW: Add Task button
            Button btnAddTask = new Button
            {
                Text = "➕ Add Task",
                Location = new Point(350, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(80, 200, 120),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnAddTask.FlatAppearance.BorderSize = 0;
            btnAddTask.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 220, 140);
            btnAddTask.Click += BtnAddTask_Click;
            
            // ⚙️ NEW: Settings button
            Button btnSettings = new Button
            {
                Text = "⚙️ Settings",
                Location = new Point(460, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(127, 140, 141),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(147, 160, 161);
            btnSettings.Click += BtnSettings_Click;
            
            this.Controls.Add(btnTest);
            this.Controls.Add(btnTestDB);
            this.Controls.Add(btnTestData);
            this.Controls.Add(btnReports);
            this.Controls.Add(btnAddTask);
            this.Controls.Add(btnSettings);
            
            // ✨ NEW: Add Bottom Navigation Panel
            CreateBottomNavigationPanel();
        }

        // ✨ NEW: Create Bottom Navigation Panel
        private void CreateBottomNavigationPanel()
        {
            Panel pnlBottomNav = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(40, 40, 40)
            };

            // Home button
            Button btnHome = new Button
            {
                Text = "🏠 Trang chủ",
                Location = new Point(30, 15),
                Size = new Size(110, 35),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

            // ✨ REPORTS BUTTON - MAIN FEATURE
            Button btnBottomReports = new Button
            {
                Text = "📊 Báo cáo",
                Location = new Point(150, 15),
                Size = new Size(110, 35),
                BackColor = Color.FromArgb(100, 149, 237),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBottomReports.FlatAppearance.BorderSize = 0;
            btnBottomReports.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 169, 255);
            btnBottomReports.Click += BtnReports_Click; // ✨ USE EXISTING EVENT HANDLER

            // ⚙️ Settings button in bottom nav
            Button btnBottomSettings = new Button
            {
                Text = "⚙️ Cài đặt",
                Location = new Point(270, 15),
                Size = new Size(110, 35),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnBottomSettings.FlatAppearance.BorderSize = 0;
            btnBottomSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);
            btnBottomSettings.Click += BtnSettings_Click;

            pnlBottomNav.Controls.Add(btnHome);
            pnlBottomNav.Controls.Add(btnBottomReports);
            pnlBottomNav.Controls.Add(btnBottomSettings);

            this.Controls.Add(pnlBottomNav);
            pnlBottomNav.BringToFront();
        }

        // ✨ NEW: Add Task button click handler
        private void BtnAddTask_Click(object? sender, EventArgs e)
        {
            using (var addTaskForm = new Forms.AddTaskToListForm())
            {
                if (addTaskForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProjectsFromDatabase();
                }
            }
        }

        // ⚙️ NEW: Settings button click handler
        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            try
            {
                using (var settingsForm = new Forms.SettingsForm())
                {
                    if (settingsForm.ShowDialog() == DialogResult.OK)
                    {
                        // Reload data if settings changed
                        LoadProjectsFromDatabase();
                        UpdateGreetingLabels();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cài đặt: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTest_Click(object? sender, EventArgs e)
        {
            using (var testForm = new Tests.ProjectManagementTestForm())
            {
                testForm.ShowDialog();
            }
        }

        private void BtnTestDB_Click(object? sender, EventArgs e)
        {
            Tests.SimpleDatabaseTest.TestDatabaseConnection();
        }

        private void BtnTestData_Click(object? sender, EventArgs e)
        {
            Tests.DatabaseConnectionTest.TestConnectionAndCreateSampleData();
            LoadProjectsFromDatabase();

            if (_context == null)
            {
                MessageBox.Show("Không thể truy cập dữ liệu vì database chưa được khởi tạo.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var totalProjects = _context.Projects.Count(p => p.IsArchived != true);
                var totalTasks = _context.Tasks.Count(t => t.IsDeleted != true);
                var completedTasks = _context.Tasks.Count(t => t.IsDeleted != true && t.Status == "Completed");

                MessageBox.Show($"Dữ liệu hiện tại:\n" +
                    $"- Projects: {totalProjects}\n" +
                    $"- Tasks: {totalTasks}\n" +
                    $"- Completed: {completedTasks}\n" +
                    $"- In Progress: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "In Progress")}\n" +
                    $"- Pending: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "Pending")}", 
                    "Thông tin dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kiểm tra dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReports_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("Database context chưa được khởi tạo. Vui lòng khởi động lại ứng dụng.", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ MỞ REPORTSFORM (màu đen - original design)
                var reportsForm = new Forms.ReportsForm(_context);
                reportsForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở báo cáo:\n{ex.Message}\n\nStack trace:\n{ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAdvancedStats()
        {
            if (_context == null)
            {
                MessageBox.Show("Không thể mở thống kê nâng cao vì database chưa được khởi tạo.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // ✅ Now using the new AdvancedReportsForm
            try
            {
                using (var advancedReportsForm = new Forms.AdvancedReportsForm(_context))
                {
                    advancedReportsForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở thống kê nâng cao: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowPythonSetupGuide()
        {
            var message = "🐍 Python Charts Setup Guide\n\n" +
                "Step 1: Install Python\n" +
                "• Download from: https://www.python.org/downloads/\n" +
                "• ✅ IMPORTANT: Check 'Add Python to PATH' during installation\n\n" +
                "Step 2: Setup Charts\n" +
                "• Run: setup_python_integration.bat\n" +
                "• Or manually copy python_charts folder to build directory\n\n" +
                "Step 3: Install packages\n" +
                "• pip install matplotlib seaborn pandas pyodbc plotly\n\n" +
                "Would you like to open the Python download page?";
            
            var result = MessageBox.Show(message, "🐍 Python Setup Guide", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://www.python.org/downloads/",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot open browser: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        
        private void GeneratePythonCharts()
        {
            try
            {
                var pythonScript = Path.Combine(Application.StartupPath, "python_charts", "todolist_charts.py");
                var pythonFolder = Path.Combine(Application.StartupPath, "python_charts");
                
                if (!Directory.Exists(pythonFolder))
                {
                    ShowPythonSetupGuide();
                    return;
                }
                
                if (!File.Exists(pythonScript))
                {
                    MessageBox.Show($"Python script not found at:\n{pythonScript}\n\n" +
                        "Please run setup_python_integration.bat to copy the required files.", 
                        "Script Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Try different Python commands
                string[] pythonCommands = { "python", "py", "python3" };
                string? workingPython = null;
                
                // Test which Python command works
                foreach (string cmd in pythonCommands)
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
                        
                        using (var testProcess = System.Diagnostics.Process.Start(testInfo))
                        {
                            if (testProcess != null)
                            {
                                testProcess.WaitForExit();
                                if (testProcess.ExitCode == 0)
                                {
                                    workingPython = cmd;
                                    break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                
                if (workingPython == null)
                {
                    var result = MessageBox.Show(
                        "❌ Python not found on your system!\n\n" +
                        "To use Python Charts, you need to:\n" +
                        "1️⃣ Install Python from python.org\n" +
                        "2️⃣ ✅ CHECK 'Add Python to PATH' during installation\n" +
                        "3️⃣ Restart this application\n\n" +
                        "Would you like to:\n" +
                        "• YES: Open Python download page\n" +
                        "• NO: Continue without Python charts",
                        "Python Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.Yes)
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
                    return;
                }

                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = workingPython,
                    Arguments = $"\"{pythonScript}\"",
                    WorkingDirectory = Path.Combine(Application.StartupPath, "python_charts"),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                MessageBox.Show("🐍 Generating beautiful Python charts...\nThis may take a few seconds.", 
                    "Python Charts", MessageBoxButtons.OK, MessageBoxIcon.Information);

                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        
                        var output = process.StandardOutput.ReadToEnd();
                        var error = process.StandardError.ReadToEnd();
                        
                        if (process.ExitCode == 0)
                        {
                            var chartsFolder = Path.Combine(Application.StartupPath, "python_charts", "ToDoList_Charts");
                            
                            var result = MessageBox.Show($"✅ Charts generated successfully!\n\n" +
                                $"📁 Location: {chartsFolder}\n\n" +
                                "Would you like to open the charts folder?", 
                                "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            
                            if (result == DialogResult.Yes && Directory.Exists(chartsFolder))
                            {
                                System.Diagnostics.Process.Start("explorer.exe", chartsFolder);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"❌ Error generating charts:\n\n{error}\n\nOutput:\n{output}", 
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error running Python script:\n\n{ex.Message}\n\n" +
                    "Make sure Python is installed and accessible from PATH.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                _context = new ToDoListContext();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối database: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCreateNewList_Click(object? sender, EventArgs e)
        {
            using (CreateListForm createListForm = new CreateListForm())
            {
                if (createListForm.ShowDialog() == DialogResult.OK)
                {
                    _justCreatedList = true; // Set flag when list is created
                    LoadProjectsFromDatabase();
                }
            }
        }

        private async void LoadProjectsFromDatabase()
        {
            try
            {
                if (_context == null) return;

                // Xóa tất cả controls hiện có
                pnlListsContainer.Controls.Clear();

                // ✅ FIX: Lấy UserId từ UserSession thay vì FirstOrDefaultAsync()
                _userId = UserSession.GetUserId();
                
                if (_userId == 0)
                {
                    MessageBox.Show("Lỗi: Không tìm thấy thông tin người dùng. Vui lòng đăng nhập lại.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ FIX: Set command timeout to prevent timeout errors
                _context.Database.SetCommandTimeout(60); // 60 seconds timeout

                // Load projects from database - CHỈ CỦA USER HIỆN TẠI
                var projects = await _context.Projects
                    .Where(p => p.UserId == _userId && p.IsArchived != true)
                    .Include(p => p.Tasks.Where(t => t.UserId == _userId && t.IsDeleted != true))  // ✅ Filter tasks
                    .OrderByDescending(p => p.CreatedAt)
                    .AsNoTracking() // ✅ Improve performance by not tracking entities
                    .ToListAsync();

                // Add project cards
                foreach (var project in projects)
                {
                    var pendingTasks = project.Tasks?.Count(t => t.Status != "Completed" && t.IsDeleted != true) ?? 0;
                    var estimatedMinutes = project.Tasks?.Where(t => t.IsDeleted != true).Sum(t => t.EstimatedMinutes) ?? 0;
                    
                    var taskList = project.Tasks?
                        .Where(t => t.IsDeleted != true)
                        .Take(5)
                        .Select(t => (t.Title, t.EstimatedMinutes?.ToString() + "ph" ?? "N/A"))
                        .ToList() ?? new List<(string, string)>();

                    AddProjectCard(project, pendingTasks, estimatedMinutes, taskList);
                }

                // Luôn thêm card tạo danh sách mới ở cuối
                AddCreateListCard();
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == -2) // Timeout error
            {
                MessageBox.Show("Kết nối cơ sở dữ liệu bị timeout. Vui lòng thử lại.\n\n" +
                    "Nếu lỗi vẫn tiếp tục, hãy kiểm tra:\n" +
                    "- Kết nối mạng\n" +
                    "- SQL Server có đang chạy không\n" +
                    "- Connection string trong appsettings.json", 
                    "Lỗi Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách: {ex.Message}\n\n" +
                    $"Chi tiết: {ex.InnerException?.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddProjectCard(Project project, int pendingTasks, int estimatedMinutes, List<(string name, string time)> tasks)
        {
            Panel listCard = new Panel
            {
                Width = 320,
                Height = 450, // ✅ FIXED: Changed from 480 to 450 to avoid validation error
                BackColor = Color.FromArgb(35, 35, 35),
                BorderStyle = BorderStyle.None,
                Margin = new Padding(12),
                Cursor = Cursors.Hand
            };

            // Add subtle shadow effect and rounded appearance
            listCard.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, listCard.Width - 1, listCard.Height - 1);
                using (var pen = new Pen(Color.FromArgb(50, 50, 50), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            // Add hover effect
            listCard.MouseEnter += (s, e) => listCard.BackColor = Color.FromArgb(40, 40, 40);
            listCard.MouseLeave += (s, e) => listCard.BackColor = Color.FromArgb(35, 35, 35);

            // Icon với background màu từ project - improved design
            Panel iconBg = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(36, 36),
                BackColor = !string.IsNullOrEmpty(project.ColorCode) ? 
                    ColorTranslator.FromHtml(project.ColorCode) : 
                    Color.FromArgb(100, 149, 237)
            };

            // Round corners for icon background
            iconBg.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, iconBg.Width, iconBg.Height);
                using (var brush = new SolidBrush(iconBg.BackColor))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            };

            Label lblIcon = new Label
            {
                Text = "📋",
                Location = new Point(0, 0),
                Size = new Size(36, 36),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            iconBg.Controls.Add(lblIcon);

            // Tên list - improved typography
            Label lblName = new Label
            {
                Text = project.ProjectName,
                Location = new Point(65, 22),
                Size = new Size(200, 30),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };

            // Button menu - improved design and positioning
            Button btnMenu = new Button
            {
                Text = "⋮",
                Location = new Point(280, 15),
                Size = new Size(30, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);
            btnMenu.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 80, 80);
            btnMenu.Click += (s, e) => ShowProjectMenu(project, btnMenu);
            
            // Enhanced hover effects
            btnMenu.MouseEnter += (s, e) => {
                btnMenu.ForeColor = Color.White;
                btnMenu.BackColor = Color.FromArgb(55, 55, 55);
            };
            btnMenu.MouseLeave += (s, e) => {
                btnMenu.ForeColor = Color.FromArgb(150, 150, 150);
                btnMenu.BackColor = Color.Transparent;
            };

            listCard.Controls.Add(iconBg);
            listCard.Controls.Add(lblName);
            listCard.Controls.Add(btnMenu);
            
            // Ensure menu button is on top
            btnMenu.BringToFront();

            // Các task items - improved spacing and design
            int taskY = 70;
            int taskIndex = 1;
            foreach (var task in tasks)
            {
                Panel taskPanel = new Panel
                {
                    Location = new Point(20, taskY),
                    Size = new Size(280, 55),
                    BackColor = Color.FromArgb(45, 45, 45),
                    Margin = new Padding(0, 2, 0, 2)
                };

                // Add subtle border
                taskPanel.Paint += (s, e) =>
                {
                    var rect = new Rectangle(0, 0, taskPanel.Width - 1, taskPanel.Height - 1);
                    using (var pen = new Pen(Color.FromArgb(60, 60, 60), 1))
                    {
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                };

                Label lblTaskNumber = new Label
                {
                    Text = taskIndex.ToString(),
                    Location = new Point(12, 18),
                    Size = new Size(25, 20),
                    ForeColor = Color.FromArgb(150, 150, 150),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    BackColor = Color.Transparent
                };

                Label lblTaskName = new Label
                {
                    Text = task.name,
                    Location = new Point(45, 18),
                    Size = new Size(160, 20),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F),
                    BackColor = Color.Transparent,
                    AutoEllipsis = true
                };

                Label lblTaskTime = new Label
                {
                    Text = task.time,
                    Location = new Point(210, 18),
                    Size = new Size(60, 20),
                    ForeColor = Color.FromArgb(150, 150, 150),
                    Font = new Font("Segoe UI", 9F),
                    TextAlign = ContentAlignment.MiddleRight,
                    BackColor = Color.Transparent
                };

                taskPanel.Controls.Add(lblTaskNumber);
                taskPanel.Controls.Add(lblTaskName);
                taskPanel.Controls.Add(lblTaskTime);
                listCard.Controls.Add(taskPanel);

                taskY += 65;
                taskIndex++;

                // Limit to 3 tasks to make room for button
                if (taskIndex > 3) break;
            }

            // Footer info - adjusted positioning for new height
            Label lblPendingTasks = new Label
            {
                Text = $"{pendingTasks} công việc đang chờ",
                Location = new Point(20, 310), // ✅ ADJUSTED: From 340 to 310
                Size = new Size(160, 20),
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.Transparent
            };

            Label lblEstTime = new Label
            {
                Text = $"Dự kiến: {estimatedMinutes}ph",
                Location = new Point(200, 310), // ✅ ADJUSTED: From 340 to 310
                Size = new Size(100, 20),
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            // ✨ Add "Cuculist Now" button - adjusted position
            Button btnCuculist = new Button
            {
                Text = "Cuculist Now",
                Location = new Point(60, 350), // ✅ ADJUSTED: From 380 to 350
                Size = new Size(200, 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(100, 149, 237),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCuculist.FlatAppearance.BorderSize = 0;
            btnCuculist.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 169, 255);
            btnCuculist.Click += (s, e) => {
                // Open Cuculist Detail Form (full view with tasks list)
                using (var cuculistForm = new Forms.CuculistDetailForm(project))
                {
                    cuculistForm.ShowDialog();
                    // Reload after closing to update any completed tasks
                    LoadProjectsFromDatabase();
                }
            };

            listCard.Controls.Add(lblPendingTasks);
            listCard.Controls.Add(lblEstTime);
            listCard.Controls.Add(btnCuculist);

            // Don't add click event to card itself to avoid conflict with button
            // listCard.Click += (s, e) => OpenProjectDetails(project);

            pnlListsContainer.Controls.Add(listCard);
        }

        private void LoadSampleData()
        {
            // Xóa tất cả controls hiện có
            pnlListsContainer.Controls.Clear();

            // Luôn thêm card tạo danh sách mới ở cuối
            AddCreateListCard();
        }

        private void AddListCard(string listName, int pendingTasks, int estimatedMinutes, List<(string name, string time)> tasks)
        {
            Panel listCard = new Panel
            {
                Width = 310,
                Height = 420,
                BackColor = Color.FromArgb(30, 30, 30),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            // Icon với background màu vàng
            Panel iconBg = new Panel
            {
                Location = new Point(15, 15),
                Size = new Size(30, 30),
                BackColor = Color.FromArgb(200, 180, 50)
            };

            Label lblIcon = new Label
            {
                Text = "T",
                Location = new Point(0, 0),
                Size = new Size(30, 30),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            iconBg.Controls.Add(lblIcon);

            // Tên list
            Label lblName = new Label
            {
                Text = listName,
                Location = new Point(55, 18),
                Size = new Size(200, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            // Button menu
            Button btnMenu = new Button
            {
                Text = "⋮",
                Location = new Point(270, 15),
                Size = new Size(25, 25),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 16F)
            };
            btnMenu.FlatAppearance.BorderSize = 0;

            listCard.Controls.Add(iconBg);
            listCard.Controls.Add(lblName);
            listCard.Controls.Add(btnMenu);

            // Các task items
            int taskY = 60;
            int taskIndex = 1;
            foreach (var task in tasks)
            {
                Panel taskPanel = new Panel
                {
                    Location = new Point(15, taskY),
                    Size = new Size(280, 50),
                    BackColor = Color.FromArgb(40, 40, 40)
                };

                Label lblTaskNumber = new Label
                {
                    Text = taskIndex.ToString(),
                    Location = new Point(10, 15),
                    Size = new Size(20, 20),
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 9F)
                };

                Label lblTaskName = new Label
                {
                    Text = task.name,
                    Location = new Point(40, 15),
                    Size = new Size(150, 20),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F)
                };

                Label lblTaskTime = new Label
                {
                    Text = task.time,
                    Location = new Point(220, 15),
                    Size = new Size(50, 20),
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 9F),
                    TextAlign = ContentAlignment.MiddleRight
                };

                taskPanel.Controls.Add(lblTaskNumber);
                taskPanel.Controls.Add(lblTaskName);
                taskPanel.Controls.Add(lblTaskTime);
                listCard.Controls.Add(taskPanel);

                taskY += 60;
                taskIndex++;
            }

            // Footer info
            Label lblPendingTasks = new Label
            {
                Text = $"{pendingTasks} công việc đang chờ",
                Location = new Point(15, 380),
                Size = new Size(150, 20),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8F)
            };

            Label lblEstTime = new Label
            {
                Text = $"Dự kiến: {estimatedMinutes}phút",
                Location = new Point(200, 380),
                Size = new Size(95, 20),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8F),
                TextAlign = ContentAlignment.MiddleRight
            };

            listCard.Controls.Add(lblPendingTasks);
            listCard.Controls.Add(lblEstTime);

            pnlListsContainer.Controls.Add(listCard);
        }

        private void AddCreateListCard()
        {
            Panel createCard = new Panel
            {
                Width = 320,
                Height = 430,
                BackColor = Color.FromArgb(25, 25, 25),
                BorderStyle = BorderStyle.None,
                Margin = new Padding(12),
                Cursor = Cursors.Hand
            };

            Label lblPlus = new Label
            {
                Text = "+",
                Location = new Point(0, 120),
                Size = new Size(320, 100),
                ForeColor = Color.FromArgb(100, 100, 100),
                Font = new Font("Segoe UI", 60F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblCreate = new Label
            {
                Text = "TẠO DANH SÁCH MỚI",
                Location = new Point(0, 240),
                Size = new Size(320, 30),
                ForeColor = Color.FromArgb(120, 120, 120),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Only add Cuculist Now button if list was just created
            if (_justCreatedList)
            {
                Button btnCuculist = new Button
                {
                    Text = "Cuculist Now",
                    Location = new Point(60, 300),
                    Size = new Size(200, 40),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(100, 149, 237),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnCuculist.FlatAppearance.BorderSize = 0;
                btnCuculist.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 169, 255);
                btnCuculist.Click += BtnCreateNewList_Click;
                createCard.Controls.Add(btnCuculist);
            }

            bool isHovering = false;

            createCard.Paint += (s, e) =>
            {
                if (s is Panel panel)
                {
                    Color borderColor = isHovering ? Color.FromArgb(100, 149, 237) : Color.FromArgb(60, 60, 60);
                    
                    // Draw dashed border
                    using (var pen = new Pen(borderColor, 2))
                    {
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        e.Graphics.DrawRectangle(pen, 1, 1, panel.Width - 3, panel.Height - 3);
                    }
                }
            };

            // Event handlers for hover effect
            EventHandler mouseEnter = (s, e) =>
            {
                isHovering = true;
                createCard.BackColor = Color.FromArgb(30, 30, 30);
                lblPlus.ForeColor = Color.FromArgb(100, 149, 237);
                lblCreate.ForeColor = Color.FromArgb(100, 149, 237);
                createCard.Invalidate(); // Redraw the panel
            };

            EventHandler mouseLeave = (s, e) =>
            {
                isHovering = false;
                createCard.BackColor = Color.FromArgb(25, 25, 25);
                lblPlus.ForeColor = Color.FromArgb(100, 100, 100);
                lblCreate.ForeColor = Color.FromArgb(120, 120, 120);
                createCard.Invalidate(); // Redraw the panel
            };

            createCard.MouseEnter += mouseEnter;
            lblPlus.MouseEnter += mouseEnter;
            lblCreate.MouseEnter += mouseEnter;

            createCard.MouseLeave += mouseLeave;
            lblPlus.MouseLeave += mouseLeave;
            lblCreate.MouseLeave += mouseLeave;

            createCard.Click += BtnCreateNewList_Click;
            lblPlus.Click += BtnCreateNewList_Click;
            lblCreate.Click += BtnCreateNewList_Click;

            createCard.Controls.Add(lblPlus);
            createCard.Controls.Add(lblCreate);

            pnlListsContainer.Controls.Add(createCard);
        }

        private void ShowProjectMenu(Project project, Button btnMenu)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.BackColor = Color.FromArgb(40, 40, 40);
            menu.ForeColor = Color.White;
            menu.RenderMode = ToolStripRenderMode.Professional;
            menu.Renderer = new DarkMenuRenderer();
            
            // Thêm các menu items
            var editItem = menu.Items.Add("✏️ Chỉnh sửa", null, (s, e) => EditProject(project));
            var viewItem = menu.Items.Add("👁️ Xem chi tiết", null, (s, e) => OpenProjectDetails(project));
            var statsItem = menu.Items.Add("📊 Thống kê nâng cao", null, (s, e) => ShowAdvancedStats());
            
            // Check if Python is available
            var pythonFolder = Path.Combine(Application.StartupPath, "python_charts");
            if (Directory.Exists(pythonFolder))
            {
                var pythonChartsItem = menu.Items.Add("🐍 Python Charts", null, (s, e) => GeneratePythonCharts());
            }
            else
            {
                var setupPythonItem = menu.Items.Add("🐍 Setup Python Charts", null, (s, e) => ShowPythonSetupGuide());
            }
            
            var archiveItem = menu.Items.Add("📁 Lưu trữ", null, (s, e) => ArchiveProject(project));
            
            // Add separator properly
            var separator = new ToolStripSeparator();
            separator.BackColor = Color.FromArgb(60, 60, 60);
            separator.ForeColor = Color.FromArgb(60, 60, 60);
            menu.Items.Add(separator);
            
            var deleteItem = menu.Items.Add("🗑️ Xóa", null, (s, e) => DeleteProject(project));
            
            // Style cho menu items - Fix casting error
            foreach (ToolStripItem item in menu.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.BackColor = Color.FromArgb(40, 40, 40);
                    menuItem.ForeColor = Color.White;
                    menuItem.Font = new Font("Segoe UI", 9F);
                    
                    // Add hover effects
                    menuItem.MouseEnter += (s, e) => menuItem.BackColor = Color.FromArgb(60, 60, 60);
                    menuItem.MouseLeave += (s, e) => menuItem.BackColor = Color.FromArgb(40, 40, 40);
                }
            }
            
            // Đặt màu đỏ cho item xóa
            if (deleteItem is ToolStripMenuItem deleteMenuItem)
            {
                deleteMenuItem.ForeColor = Color.FromArgb(255, 100, 100);
                deleteMenuItem.MouseEnter += (s, e) => {
                    deleteMenuItem.BackColor = Color.FromArgb(80, 30, 30);
                    deleteMenuItem.ForeColor = Color.FromArgb(255, 120, 120);
                };
                deleteMenuItem.MouseLeave += (s, e) => {
                    deleteMenuItem.BackColor = Color.FromArgb(40, 40, 40);
                    deleteMenuItem.ForeColor = Color.FromArgb(255, 100, 100);
                };
            }
            
            menu.Show(btnMenu, new Point(0, btnMenu.Height));
        }

        private void EditProject(Project project)
        {
            // TODO: Implement edit project form
            MessageBox.Show($"Chỉnh sửa project: {project.ProjectName}", "Thông báo", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void ArchiveProject(Project project)
        {
            try
            {
                var result = MessageBox.Show($"Bạn có muốn lưu trữ project '{project.ProjectName}'?\n\nProject sẽ được ẩn khỏi danh sách chính.", 
                    "Xác nhận lưu trữ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    project.IsArchived = true;
                    await _context.SaveChangesAsync();
                    LoadProjectsFromDatabase(); // Reload to hide archived project
                    
                    MessageBox.Show("Project đã được lưu trữ thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu trữ project: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DeleteProject(Project project)
        {
            try
            {
                var result = MessageBox.Show($"⚠️ BẠN CÓ CHẮC CHẮN MUỐN XÓA PROJECT '{project.ProjectName}'?\n\n" +
                    "Hành động này sẽ xóa vĩnh viễn project và TẤT CẢ các task bên trong!\n" +
                    "Không thể hoàn tác sau khi xóa!", 
                    "⚠️ XÁC NHẬN XÓA VĨNH VIỄN", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    // Xóa tất cả tasks trong project trước
                    var tasks = await _context.Tasks
                        .Where(t => t.ProjectId == project.ProjectId)
                        .ToListAsync();
                    
                    _context.Tasks.RemoveRange(tasks);
                    
                    // Xóa project
                    _context.Projects.Remove(project);
                    await _context.SaveChangesAsync();
                    
                    LoadProjectsFromDatabase(); // Reload to remove deleted project
                    
                    MessageBox.Show($"Project '{project.ProjectName}' và {tasks.Count} task(s) đã được xóa vĩnh viễn!", "Đã xóa", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa project: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenProjectDetails(Project project)
        {
            using (var projectDetailsForm = new Forms.ProjectDetailsForm(project))
            {
                projectDetailsForm.ShowDialog();
                // Reload projects after closing details form
                LoadProjectsFromDatabase();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }

    // Custom renderer for dark theme context menu
    public class DarkMenuRenderer : ToolStripProfessionalRenderer
    {
        public DarkMenuRenderer() : base(new DarkColorTable()) { }
    }

    public class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(60, 60, 60);
        public override Color MenuItemBorder => Color.FromArgb(70, 70, 70);
        public override Color MenuBorder => Color.FromArgb(70, 70, 70);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(60, 60, 60);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(60, 60, 60);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(80, 80, 80);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(80, 80, 80);
        public override Color SeparatorDark => Color.FromArgb(60, 60, 60);
        public override Color SeparatorLight => Color.FromArgb(70, 70, 70);
    }
}
