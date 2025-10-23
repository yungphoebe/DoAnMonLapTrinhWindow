using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using ToDoList.GUI.Helpers;
using ToDoList.GUI.Resources;

namespace ToDoList.GUI.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblEmail;
        private Label lblPassword;
        private CheckBox chkShowPassword;
        private Panel pnlMain;
        private Label lblError;
        private Label lblRegisterLink; // ? New: Register link label

        public LoginForm()
        {
            InitializeComponent();
            SetupCustomUI();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // LoginForm
            // 
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(450, 550);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng Nhập - ToDoList App";
            ResumeLayout(false);
        }

        private void SetupCustomUI()
        {
            // Main panel
            pnlMain = new Panel
            {
                Location = new Point(50, 50),
                Size = new Size(350, 450),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Add shadow effect
            pnlMain.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlMain.Width - 1, pnlMain.Height - 1);
                }
            };

            // Title
            lblTitle = new Label
            {
                Text = "Đăng Nhập",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = false,
                Size = new Size(350, 50),
                Location = new Point(0, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Email label
            lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(25, 110)
            };

            // Email textbox
            txtEmail = new TextBox
            {
                Location = new Point(25, 135),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "Nhập email của bạn",
                BorderStyle = BorderStyle.FixedSingle
            };

            // Password label
            lblPassword = new Label
            {
                Text = "Mật Khẩu:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(25, 185)
            };

            // Password textbox
            txtPassword = new TextBox
            {
                Location = new Point(25, 210),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 11),
                PasswordChar = '?',
                PlaceholderText = "Nhập mật khẩu",
                BorderStyle = BorderStyle.FixedSingle
            };

            // Show password checkbox
            chkShowPassword = new CheckBox
            {
                Text = "Hiện mật khẩu",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(25, 255)
            };
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;

            // Error label
            lblError = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(231, 76, 60),
                AutoSize = false,
                Size = new Size(300, 30),
                Location = new Point(25, 285),
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };

            // Login button
            btnLogin = new Button
            {
                Text = "ĐĂNG NHẬP",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 152, 219),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(145, 40),
                Location = new Point(25, 325),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Hover effect for login button
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(41, 128, 185);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(52, 152, 219);

            // Cancel button
            btnCancel = new Button
            {
                Text = "HAY",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                BackColor = Color.FromArgb(236, 240, 241),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(145, 40),
                Location = new Point(180, 325),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            // Hover effect for cancel button
            btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = Color.FromArgb(189, 195, 199);
            btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = Color.FromArgb(236, 240, 241);

            // ? NEW: Register link label
            lblRegisterLink = new Label
            {
                Text = "chưa có tài khoản đăng ký ngay",
                Font = new Font("Segoe UI", 10, FontStyle.Underline),
                ForeColor = Color.FromArgb(52, 152, 219),
                AutoSize = false,
                Size = new Size(300, 25),
                Location = new Point(25, 385),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };

            // Add click event to open RegisterForm
            lblRegisterLink.Click += LblRegisterLink_Click;

            // Add hover effect
            lblRegisterLink.MouseEnter += (s, e) =>
            {
                lblRegisterLink.ForeColor = Color.FromArgb(41, 128, 185);
                lblRegisterLink.Font = new Font("Segoe UI", 10, FontStyle.Bold | FontStyle.Underline);
            };
            lblRegisterLink.MouseLeave += (s, e) =>
            {
                lblRegisterLink.ForeColor = Color.FromArgb(52, 152, 219);
                lblRegisterLink.Font = new Font("Segoe UI", 10, FontStyle.Underline);
            };

            // Add all controls to main panel
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Controls.Add(lblEmail);
            pnlMain.Controls.Add(txtEmail);
            pnlMain.Controls.Add(lblPassword);
            pnlMain.Controls.Add(txtPassword);
            pnlMain.Controls.Add(chkShowPassword);
            pnlMain.Controls.Add(lblError);
            pnlMain.Controls.Add(btnLogin);
            pnlMain.Controls.Add(btnCancel);
            pnlMain.Controls.Add(lblRegisterLink); // ? Add register link

            // Add main panel to form
            this.Controls.Add(pnlMain);

            // Enter key support
            txtEmail.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) txtPassword.Focus(); };
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(s, e); };
        }

        // ? NEW: Event handler for register link click
        private void LblRegisterLink_Click(object sender, EventArgs e)
        {
            // Open RegisterForm as a dialog
            using (var registerForm = new RegisterForm())
            {
                if (registerForm.ShowDialog() == DialogResult.OK)
                {
                    // If registration is successful, optionally auto-fill the email
                    // or show a success message
                    MessageBox.Show(
                        "Đăng ký thành công! vùi lòng đăng nhập",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '?';
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowError("Vui lòng nh?p email!");
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Vui lòng nhập mật khẩu");
                txtPassword.Focus();
                return;
            }

            // Disable controls
            SetControlsEnabled(false);
            lblError.Visible = false;

            try
            {
                using (var context = new ToDoListContext())
                {
                    // Find user by email
                    var user = context.Users
                        .FirstOrDefault(u => u.Email == txtEmail.Text.Trim());

                    if (user == null)
                    {
                        ShowError("Email không t?n t?i!");
                        SetControlsEnabled(true);
                        return;
                    }

                    // Check password (plain text comparison for seeded data)
                    // TODO: Implement proper password hashing in production
                    if (user.PasswordHash != txtPassword.Text.Trim())
                    {
                        ShowError("Mật khẩu không đúc!");
                        SetControlsEnabled(true);
                        return;
                    }

                    // Check if user is active
                    if (user.IsActive == false)
                    {
                        ShowError("Tài khoản đã bị khóa!");
                        SetControlsEnabled(true);
                        return;
                    }

                    // Update last login
                    user.LastLogin = DateTime.Now;
                    await context.SaveChangesAsync();

                    // Set current user session
                    UserSession.Login(user);

                    // Show success message
                    MessageBox.Show(
                        $"Xin chào, {user.FullName}!\n\nĐăng nhập thành công!",
                        "Chào mừng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Close login form with success
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi kết nối: {ex.Message}");
                SetControlsEnabled(true);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ShowError(string message)
        {
            lblError.Text = "? " + message;
            lblError.Visible = true;
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtEmail.Enabled = enabled;
            txtPassword.Enabled = enabled;
            btnLogin.Enabled = enabled;
            btnCancel.Enabled = enabled;
            chkShowPassword.Enabled = enabled;

            btnLogin.Text = enabled ? "ĐĂNG NHẬP" : "Đang xử lý...";
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
        }

       
    }
}
