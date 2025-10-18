using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI
{
    public partial class Form1 : Form
    {
        private ToDoListContext _context;
        private int _userId = 1; // Tạm thời hardcode user ID

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadProjectsFromDatabase();
            btnCreateNewList.Click += BtnCreateNewList_Click;
            
            // Add test button (for development only)
            AddTestButton();
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
            
            this.Controls.Add(btnTest);
            this.Controls.Add(btnTestDB);
            this.Controls.Add(btnTestData);
        }

        private void BtnTestDB_Click(object sender, EventArgs e)
        {
            Tests.SimpleDatabaseTest.TestDatabaseConnection();
        }

        private void BtnTestData_Click(object sender, EventArgs e)
        {
            Tests.DatabaseConnectionTest.TestConnectionAndCreateSampleData();
            // Reload projects after creating sample data
            LoadProjectsFromDatabase();
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            using (var testForm = new Tests.ProjectManagementTestForm())
            {
                testForm.ShowDialog();
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

        private void BtnCreateNewList_Click(object sender, EventArgs e)
        {
            using (CreateListForm createListForm = new CreateListForm())
            {
                if (createListForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload projects from database after creating new one
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

                // Get or create default user
                var user = await _context.Users.FirstOrDefaultAsync();
                if (user == null)
                {
                    // Create default user if none exists
                    user = new User
                    {
                        FullName = "Default User",
                        Email = "user@example.com",
                        PasswordHash = "default",
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                _userId = user.UserId;

                // Load projects from database
                var projects = await _context.Projects
                    .Where(p => p.UserId == _userId && p.IsArchived != true)
                    .Include(p => p.Tasks)
                    .OrderByDescending(p => p.CreatedAt)
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
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSampleData()
        {
            // Xóa tất cả controls hiện có
            pnlListsContainer.Controls.Clear();

            // Luôn thêm card tạo danh sách mới ở cuối
            AddCreateListCard();
        }

        private void AddProjectCard(Project project, int pendingTasks, int estimatedMinutes, List<(string name, string time)> tasks)
        {
            Panel listCard = new Panel
            {
                Width = 320,
                Height = 430,
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

                // Limit to 4 tasks to avoid overflow
                if (taskIndex > 4) break;
            }

            // Footer info - improved positioning
            Label lblPendingTasks = new Label
            {
                Text = $"{pendingTasks} công việc đang chờ",
                Location = new Point(20, 390),
                Size = new Size(160, 20),
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.Transparent
            };

            Label lblEstTime = new Label
            {
                Text = $"Dự kiến: {estimatedMinutes}ph",
                Location = new Point(200, 390),
                Size = new Size(100, 20),
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            listCard.Controls.Add(lblPendingTasks);
            listCard.Controls.Add(lblEstTime);

            // Add click event to open project details
            listCard.Click += (s, e) => OpenProjectDetails(project);

            pnlListsContainer.Controls.Add(listCard);
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

            bool isHovering = false;

            createCard.Paint += (s, e) =>
            {
                Panel panel = s as Panel;
                Color borderColor = isHovering ? Color.FromArgb(100, 149, 237) : Color.FromArgb(60, 60, 60);
                
                // Draw dashed border
                using (var pen = new Pen(borderColor, 2))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, 1, 1, panel.Width - 3, panel.Height - 3);
                }
            };

            // Event handlers for hover effect
            Action<object, EventArgs> mouseEnter = (s, e) =>
            {
                isHovering = true;
                createCard.BackColor = Color.FromArgb(30, 30, 30);
                lblPlus.ForeColor = Color.FromArgb(100, 149, 237);
                lblCreate.ForeColor = Color.FromArgb(100, 149, 237);
                createCard.Invalidate(); // Redraw the panel
            };

            Action<object, EventArgs> mouseLeave = (s, e) =>
            {
                isHovering = false;
                createCard.BackColor = Color.FromArgb(25, 25, 25);
                lblPlus.ForeColor = Color.FromArgb(100, 100, 100);
                lblCreate.ForeColor = Color.FromArgb(120, 120, 120);
                createCard.Invalidate(); // Redraw the panel
            };

            createCard.MouseEnter += new EventHandler(mouseEnter);
            lblPlus.MouseEnter += new EventHandler(mouseEnter);
            lblCreate.MouseEnter += new EventHandler(mouseEnter);

            createCard.MouseLeave += new EventHandler(mouseLeave);
            lblPlus.MouseLeave += new EventHandler(mouseLeave);
            lblCreate.MouseLeave += new EventHandler(mouseLeave);

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
