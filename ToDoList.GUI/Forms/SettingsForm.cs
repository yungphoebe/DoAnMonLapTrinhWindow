using System;
using System.Drawing;
using System.Windows.Forms;
using ToDoList.GUI.Helpers;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Forms
{
    public partial class SettingsForm : Form
    {
        private ToDoListContext _context;
        private int _userId;
        
        // UI Controls
        private TabControl tabControl;
        private Panel pnlHeader;
        
        // Notifications Tab
        private CheckBox chkTaskReminders;
        private CheckBox chkSoundEnabled;
        private ComboBox cmbSoundType;
        
        // Account Tab
        private Label lblCurrentUser;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private Button btnChangePassword;
        private Button btnLogout;

        public SettingsForm()
        {
            _context = new ToDoListContext();
            _userId = UserSession.GetUserId();
            
            InitializeComponent();
            LoadSettings();
            LoadUserInfo(); // ? NEW: Load user information
        }

        private void InitializeComponent()
        {
            this.Text = "Cai dat";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            CreateHeader();
            CreateTabs();
        }

        private void CreateHeader()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(52, 73, 94)
            };

            Label lblTitle = new Label
            {
                Text = "Cai dat",
                Location = new Point(30, 20),
                Size = new Size(400, 40),
                Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point), // ? FIXED: Segoe UI ? Arial
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Button btnClose = new Button
            {
                Text = "X",
                Location = new Point(720, 20),
                Size = new Size(50, 40),
                Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point), // ? FIXED
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(231, 76, 60);
            btnClose.Click += (s, e) => this.Close();

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnClose);
            this.Controls.Add(pnlHeader);
        }

        private void CreateTabs()
        {
            tabControl = new TabControl
            {
                Location = new Point(20, 100),
                Size = new Size(760, 450),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point) // ? FIXED
            };

            // Tab 1: Thông báo
            TabPage tabNotifications = new TabPage("Thong bao"); // ? Không d?u ?? tránh l?i
            CreateNotificationsTab(tabNotifications);

            // Tab 2: Tài kho?n
            TabPage tabAccount = new TabPage("Tai khoan"); // ? Không d?u
            CreateAccountTab(tabAccount);

            tabControl.TabPages.Add(tabNotifications);
            tabControl.TabPages.Add(tabAccount);

            this.Controls.Add(tabControl);

            // Save button
            Button btnSave = new Button
            {
                Text = "Luu Cai dat", // ? Không d?u
                Location = new Point(550, 560),
                Size = new Size(230, 45),
                Font = new Font("Arial", 11F, FontStyle.Bold, GraphicsUnit.Point), // ? FIXED
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);
        }

        private void CreateNotificationsTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            GroupBox grpNotifications = new GroupBox
            {
                Text = "Thong bao", // ? Không d?u
                Location = new Point(30, 20),
                Size = new Size(680, 250),
                Font = new Font("Arial", 11F, FontStyle.Bold, GraphicsUnit.Point) // ? FIXED
            };

            chkTaskReminders = new CheckBox
            {
                Text = "Nhac nho deadline cong viec", // ? Không d?u
                Location = new Point(20, 40),
                Size = new Size(300, 30),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point), // ? FIXED
                Checked = true
            };

            chkSoundEnabled = new CheckBox
            {
                Text = "Bat am thanh thong bao", // ? Không d?u
                Location = new Point(20, 90),
                Size = new Size(300, 30),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point), // ? FIXED
                Checked = true
            };

            Label lblSound = new Label
            {
                Text = "Loai am thanh:", // ? Không d?u
                Location = new Point(20, 140),
                Size = new Size(150, 25),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point) // ? FIXED
            };

            cmbSoundType = new ComboBox
            {
                Location = new Point(180, 138),
                Size = new Size(250, 30),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point), // ? FIXED
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = chkSoundEnabled.Checked
            };
            cmbSoundType.Items.AddRange(new[] { "Mac dinh", "Chuong", "Tieng vo tay", "Em diu" }); // ? Không d?u
            cmbSoundType.SelectedIndex = 0;

            chkSoundEnabled.CheckedChanged += (s, e) => 
                cmbSoundType.Enabled = chkSoundEnabled.Checked;

            grpNotifications.Controls.Add(chkTaskReminders);
            grpNotifications.Controls.Add(chkSoundEnabled);
            grpNotifications.Controls.Add(lblSound);
            grpNotifications.Controls.Add(cmbSoundType);

            tab.Controls.Add(grpNotifications);
        }

        private void CreateAccountTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            // Current User Info
            GroupBox grpUserInfo = new GroupBox
            {
                Text = "Thong tin tai khoan", // ? Không d?u
                Location = new Point(30, 20),
                Size = new Size(680, 200),
                Font = new Font("Arial", 11F, FontStyle.Bold, GraphicsUnit.Point) // ? FIXED
            };

            lblCurrentUser = new Label
            {
                Text = "Dang tai...", // ? Không d?u
                Location = new Point(20, 35),
                Size = new Size(640, 30),
                Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point), // ? FIXED
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            Label lblName = new Label
            {
                Text = "Ho va ten:", // ? Không d?u
                Location = new Point(20, 80),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point) // ? FIXED
            };

            txtFullName = new TextBox
            {
                Location = new Point(150, 78),
                Size = new Size(500, 30),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point), // ? FIXED
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblEmailLabel = new Label
            {
                Text = "Email:",
                Location = new Point(20, 130),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point) // ? FIXED
            };

            txtEmail = new TextBox
            {
                Location = new Point(150, 128),
                Size = new Size(500, 30),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point), // ? FIXED
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            grpUserInfo.Controls.Add(lblCurrentUser);
            grpUserInfo.Controls.Add(lblName);
            grpUserInfo.Controls.Add(txtFullName);
            grpUserInfo.Controls.Add(lblEmailLabel);
            grpUserInfo.Controls.Add(txtEmail);

            // Actions
            GroupBox grpActions = new GroupBox
            {
                Text = "Hanh dong", // ? Không d?u
                Location = new Point(30, 240),
                Size = new Size(680, 120),
                Font = new Font("Arial", 11F, FontStyle.Bold, GraphicsUnit.Point) // ? FIXED
            };

            btnChangePassword = new Button
            {
                Text = "Doi mat khau", // ? Không d?u
                Location = new Point(20, 40),
                Size = new Size(200, 45),
                Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point), // ? FIXED
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnChangePassword.FlatAppearance.BorderSize = 0;
            btnChangePassword.Click += BtnChangePassword_Click;

            btnLogout = new Button
            {
                Text = "Dang xuat", // ? Không d?u
                Location = new Point(240, 40),
                Size = new Size(200, 45),
                Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point), // ? FIXED
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;

            grpActions.Controls.Add(btnChangePassword);
            grpActions.Controls.Add(btnLogout);

            tab.Controls.Add(grpUserInfo);
            tab.Controls.Add(grpActions);
        }

        // ? NEW: Load user information
        private void LoadUserInfo()
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == _userId);
                if (user != null)
                {
                    lblCurrentUser.Text = $"Xin chao, {user.FullName}!";
                    txtFullName.Text = user.FullName ?? "";
                    txtEmail.Text = user.Email ?? "";
                }
            }
            catch (Exception ex)
            {
                lblCurrentUser.Text = "Khong the tai thong tin nguoi dung";
            }
        }

        private void LoadSettings()
        {
            // No settings to load after removing Appearance and Pomodoro tabs
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Cai dat da duoc luu thanh cong!", "Thanh cong", // ? Không d?u
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi luu cai dat: {ex.Message}", "Loi", // ? Không d?u
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChangePassword_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Chuc nang doi mat khau se duoc phat trien trong phien ban tiep theo.", // ? Không d?u
                "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Ban co chac chan muon dang xuat?", // ? Không d?u
                "Xac nhan dang xuat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UserSession.Clear();
                
                MessageBox.Show("Da dang xuat thanh cong!", "Thong bao", // ? Không d?u
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Application.Restart();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
