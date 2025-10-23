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
        
        // Appearance Tab
        private ComboBox cmbTheme;
        private ComboBox cmbLanguage;
        private CheckBox chkAnimations;
        
        // Pomodoro Tab
        private NumericUpDown nudWorkDuration;
        private NumericUpDown nudShortBreak;
        private NumericUpDown nudLongBreak;
        private CheckBox chkAutoStartBreak;
        
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
        }

        private void InitializeComponent()
        {
            this.Text = "?? Cài ??t - ToDoList";
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
                Text = "?? Cài ??t",
                Location = new Point(30, 20),
                Size = new Size(400, 40),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Button btnClose = new Button
            {
                Text = "?",
                Location = new Point(720, 20),
                Size = new Size(50, 40),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
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
                Font = new Font("Segoe UI", 10F)
            };

            // Tab 1: Giao di?n
            TabPage tabAppearance = new TabPage("?? Giao di?n");
            CreateAppearanceTab(tabAppearance);

            // Tab 2: Pomodoro
            TabPage tabPomodoro = new TabPage("? Pomodoro");
            CreatePomodoroTab(tabPomodoro);

            // Tab 3: Thông báo
            TabPage tabNotifications = new TabPage("?? Thông báo");
            CreateNotificationsTab(tabNotifications);

            // Tab 4: Tài kho?n
            TabPage tabAccount = new TabPage("?? Tài kho?n");
            CreateAccountTab(tabAccount);

            tabControl.TabPages.Add(tabAppearance);
            tabControl.TabPages.Add(tabPomodoro);
            tabControl.TabPages.Add(tabNotifications);
            tabControl.TabPages.Add(tabAccount);

            this.Controls.Add(tabControl);

            // Save button
            Button btnSave = new Button
            {
                Text = "?? L?u Cài ??t",
                Location = new Point(550, 560),
                Size = new Size(230, 45),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);
        }

        private void CreateAppearanceTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            // Theme Section
            GroupBox grpTheme = new GroupBox
            {
                Text = "?? Ch? ??",
                Location = new Point(30, 20),
                Size = new Size(680, 100),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            Label lblTheme = new Label
            {
                Text = "Ch?n ch? ??:",
                Location = new Point(20, 35),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };

            cmbTheme = new ComboBox
            {
                Location = new Point(180, 33),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTheme.Items.AddRange(new[] { "Light (Sáng)", "Dark (T?i)", "Auto (T? ??ng)" });
            cmbTheme.SelectedIndex = 0;

            grpTheme.Controls.Add(lblTheme);
            grpTheme.Controls.Add(cmbTheme);

            // Language Section
            GroupBox grpLanguage = new GroupBox
            {
                Text = "?? Ngôn ng?",
                Location = new Point(30, 140),
                Size = new Size(680, 100),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            Label lblLanguage = new Label
            {
                Text = "Ch?n ngôn ng?:",
                Location = new Point(20, 35),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };

            cmbLanguage = new ComboBox
            {
                Location = new Point(180, 33),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLanguage.Items.AddRange(new[] { "Ti?ng Vi?t", "English" });
            cmbLanguage.SelectedIndex = 0;

            grpLanguage.Controls.Add(lblLanguage);
            grpLanguage.Controls.Add(cmbLanguage);

            // Animations Section
            GroupBox grpAnimations = new GroupBox
            {
                Text = "? Hi?u ?ng",
                Location = new Point(30, 260),
                Size = new Size(680, 100),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            chkAnimations = new CheckBox
            {
                Text = "B?t hi?u ?ng chuy?n ??ng",
                Location = new Point(20, 35),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10F),
                Checked = true
            };

            grpAnimations.Controls.Add(chkAnimations);

            tab.Controls.Add(grpTheme);
            tab.Controls.Add(grpLanguage);
            tab.Controls.Add(grpAnimations);
        }

        private void CreatePomodoroTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            GroupBox grpPomodoro = new GroupBox
            {
                Text = "? Cài ??t Pomodoro Timer",
                Location = new Point(30, 20),
                Size = new Size(680, 350),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            // Work Duration
            Label lblWork = new Label
            {
                Text = "Th?i gian làm vi?c (phút):",
                Location = new Point(20, 40),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10F)
            };

            nudWorkDuration = new NumericUpDown
            {
                Location = new Point(280, 38),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 10F),
                Minimum = 5,
                Maximum = 90,
                Value = 25
            };

            // Short Break
            Label lblShortBreak = new Label
            {
                Text = "Ngh? ng?n (phút):",
                Location = new Point(20, 90),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10F)
            };

            nudShortBreak = new NumericUpDown
            {
                Location = new Point(280, 88),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1,
                Maximum = 30,
                Value = 5
            };

            // Long Break
            Label lblLongBreak = new Label
            {
                Text = "Ngh? dài (phút):",
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10F)
            };

            nudLongBreak = new NumericUpDown
            {
                Location = new Point(280, 138),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 10F),
                Minimum = 5,
                Maximum = 60,
                Value = 15
            };

            // Auto Start Break
            chkAutoStartBreak = new CheckBox
            {
                Text = "T? ??ng b?t ??u ngh?",
                Location = new Point(20, 200),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10F),
                Checked = true
            };

            // Info
            Label lblInfo = new Label
            {
                Text = "?? K? thu?t Pomodoro giúp b?n t?p trung làm vi?c hi?u qu? h?n.\n" +
                       "Làm vi?c 25 phút ? Ngh? 5 phút ? L?p l?i.\n" +
                       "Sau 4 Pomodoro ? Ngh? dài 15-30 phút.",
                Location = new Point(20, 250),
                Size = new Size(640, 80),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(127, 140, 141)
            };

            grpPomodoro.Controls.Add(lblWork);
            grpPomodoro.Controls.Add(nudWorkDuration);
            grpPomodoro.Controls.Add(lblShortBreak);
            grpPomodoro.Controls.Add(nudShortBreak);
            grpPomodoro.Controls.Add(lblLongBreak);
            grpPomodoro.Controls.Add(nudLongBreak);
            grpPomodoro.Controls.Add(chkAutoStartBreak);
            grpPomodoro.Controls.Add(lblInfo);

            tab.Controls.Add(grpPomodoro);
        }

        private void CreateNotificationsTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            GroupBox grpNotifications = new GroupBox
            {
                Text = "?? Thông báo",
                Location = new Point(30, 20),
                Size = new Size(680, 250),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            chkTaskReminders = new CheckBox
            {
                Text = "Nh?c nh? deadline công vi?c",
                Location = new Point(20, 40),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10F),
                Checked = true
            };

            chkSoundEnabled = new CheckBox
            {
                Text = "B?t âm thanh thông báo",
                Location = new Point(20, 90),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10F),
                Checked = true
            };

            Label lblSound = new Label
            {
                Text = "Lo?i âm thanh:",
                Location = new Point(20, 140),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };

            cmbSoundType = new ComboBox
            {
                Location = new Point(180, 138),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = chkSoundEnabled.Checked
            };
            cmbSoundType.Items.AddRange(new[] { "M?c ??nh", "Chuông", "Ti?ng v? tay", "Êm d?u" });
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
                Text = "?? Thông tin tài kho?n",
                Location = new Point(30, 20),
                Size = new Size(680, 200),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            lblCurrentUser = new Label
            {
                Text = "?ang t?i...",
                Location = new Point(20, 35),
                Size = new Size(640, 30),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            Label lblName = new Label
            {
                Text = "H? và tên:",
                Location = new Point(20, 80),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };

            txtFullName = new TextBox
            {
                Location = new Point(150, 78),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 10F),
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblEmailLabel = new Label
            {
                Text = "Email:",
                Location = new Point(20, 130),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F)
            };

            txtEmail = new TextBox
            {
                Location = new Point(150, 128),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 10F),
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
                Text = "?? Hành ??ng",
                Location = new Point(30, 240),
                Size = new Size(680, 120),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            btnChangePassword = new Button
            {
                Text = "?? ??i m?t kh?u",
                Location = new Point(20, 40),
                Size = new Size(200, 45),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnChangePassword.FlatAppearance.BorderSize = 0;
            btnChangePassword.Click += BtnChangePassword_Click;

            btnLogout = new Button
            {
                Text = "?? ??ng xu?t",
                Location = new Point(240, 40),
                Size = new Size(200, 45),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
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

        private void LoadSettings()
        {
            try
            {
                var user = _context.Users.Find(_userId);
                if (user != null)
                {
                    lblCurrentUser.Text = $"Xin chào, {user.FullName}!";
                    txtFullName.Text = user.FullName;
                    txtEmail.Text = user.Email;
                }

                // Load user settings if exists
                var settings = _context.UserSettings.FirstOrDefault(s => s.UserId == _userId);
                if (settings != null)
                {
                    // Apply loaded settings
                    cmbTheme.SelectedIndex = settings.Theme == "Dark" ? 1 : 0;
                    cmbLanguage.SelectedIndex = settings.Language == "en" ? 1 : 0;
                    
                    nudWorkDuration.Value = settings.DailyGoalMinutes ?? 25;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i khi t?i cài ??t: {ex.Message}", "L?i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var settings = _context.UserSettings.FirstOrDefault(s => s.UserId == _userId);
                
                if (settings == null)
                {
                    settings = new UserSetting
                    {
                        UserId = _userId
                    };
                    _context.UserSettings.Add(settings);
                }

                // Save settings
                settings.Theme = cmbTheme.SelectedIndex == 1 ? "Dark" : "Light";
                settings.Language = cmbLanguage.SelectedIndex == 1 ? "en" : "vi";
                settings.DailyGoalMinutes = (int)nudWorkDuration.Value;

                _context.SaveChanges();

                MessageBox.Show("? ?ã l?u cài ??t thành công!\n\nM?t s? thay ??i s? có hi?u l?c sau khi kh?i ??ng l?i ?ng d?ng.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i khi l?u cài ??t: {ex.Message}", "L?i",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChangePassword_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Ch?c n?ng ??i m?t kh?u s? ???c phát tri?n trong phiên b?n ti?p theo.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("B?n có ch?c ch?n mu?n ??ng xu?t?",
                "Xác nh?n ??ng xu?t", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UserSession.Clear();
                
                MessageBox.Show("?ã ??ng xu?t thành công!", "Thông báo",
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
