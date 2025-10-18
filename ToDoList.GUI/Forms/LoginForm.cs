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

        public LoginForm()
        {
            InitializeComponent();
            SetupCustomUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.ClientSize = new Size(450, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "??ng Nh?p - ToDoList App";
            this.BackColor = Color.FromArgb(245, 247, 250);
            
            this.ResumeLayout(false);
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
                Text = "??NG NH?P",
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
                PlaceholderText = "Nh?p email c?a b?n",
                BorderStyle = BorderStyle.FixedSingle
            };

            // Password label
            lblPassword = new Label
            {
                Text = "M?t kh?u:",
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
                PlaceholderText = "Nh?p m?t kh?u",
                BorderStyle = BorderStyle.FixedSingle
            };

            // Show password checkbox
            chkShowPassword = new CheckBox
            {
                Text = "Hi?n th? m?t kh?u",
                Location = new Point(25, 255),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;

            // Error label
            lblError = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(231, 76, 60),
                AutoSize = false,
                Size = new Size(300, 40),
                Location = new Point(25, 290),
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };

            // Login button
            btnLogin = new Button
            {
                Text = "??NG NH?P",
                Location = new Point(25, 345),
                Size = new Size(300, 45),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 152, 219),
                FlatStyle = FlatStyle.Flat,
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
                Text = "H?Y",
                Location = new Point(25, 400),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(127, 140, 141),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnCancel.Click += BtnCancel_Click;

            // Add controls to panel
            pnlMain.Controls.AddRange(new Control[]
            {
                lblTitle,
                lblEmail,
                txtEmail,
                lblPassword,
                txtPassword,
                chkShowPassword,
                lblError,
                btnLogin,
                btnCancel
            });

            // Add panel to form
            this.Controls.Add(pnlMain);

            // Enter key support
            txtEmail.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) txtPassword.Focus(); };
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(s, e); };
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
                ShowError("Vui lòng nh?p m?t kh?u!");
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
                        ShowError("M?t kh?u không ?úng!");
                        SetControlsEnabled(true);
                        return;
                    }

                    // Check if user is active
                    if (user.IsActive == false)
                    {
                        ShowError("Tài kho?n ?ã b? khóa!");
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
                        $"Xin chào, {user.FullName}!\n\n??ng nh?p thành công!",
                        "Chào m?ng",
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
                ShowError($"L?i k?t n?i: {ex.Message}");
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

            btnLogin.Text = enabled ? "??NG NH?P" : "?ang x? lý...";
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
        }
    }
}
