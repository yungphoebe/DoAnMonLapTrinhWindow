using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using ToDoList.GUI.Helpers;

namespace ToDoList.GUI.Forms
{
    public partial class RegisterForm : Form
    {
        private TextBox txtFullName;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblFullName;
        private Label lblEmail;
        private Label lblPassword;
        private Label lblConfirmPassword;
        private CheckBox chkShowPassword;
        private Panel pnlMain;
        private Label lblError;

        public string RegisteredEmail { get; private set; } = string.Empty;

        public RegisterForm()
        {
            InitializeComponent();
            SetupCustomUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.ClientSize = new Size(450, 650);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "??ng Ký - ToDoList App";
            this.BackColor = Color.FromArgb(245, 247, 250);
            
            this.ResumeLayout(false);
        }

        private void SetupCustomUI()
        {
            // Main panel
            pnlMain = new Panel
            {
                Location = new Point(50, 30),
                Size = new Size(350, 590),
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
                Text = "??NG KÝ TÀI KHO?N",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = false,
                Size = new Size(350, 50),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Full Name label
            lblFullName = new Label
            {
                Text = "H? và tên:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(25, 90)
            };

            // Full Name textbox
            txtFullName = new TextBox
            {
                Location = new Point(25, 115),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "Nh?p h? và tên ??y ??",
                BorderStyle = BorderStyle.FixedSingle
            };

            // Email label
            lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(25, 165)
            };

            // Email textbox
            txtEmail = new TextBox
            {
                Location = new Point(25, 190),
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
                Location = new Point(25, 240)
            };

            // Password textbox
            txtPassword = new TextBox
            {
                Location = new Point(25, 265),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 11),
                PasswordChar = '?',
                PlaceholderText = "Nh?p m?t kh?u (t?i thi?u 6 ký t?)",
                BorderStyle = BorderStyle.FixedSingle
            };

            // Confirm Password label
            lblConfirmPassword = new Label
            {
                Text = "Xác nh?n m?t kh?u:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(25, 315)
            };

            // Confirm Password textbox
            txtConfirmPassword = new TextBox
            {
                Location = new Point(25, 340),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 11),
                PasswordChar = '?',
                PlaceholderText = "Nh?p l?i m?t kh?u",
                BorderStyle = BorderStyle.FixedSingle
            };

            // Show password checkbox
            chkShowPassword = new CheckBox
            {
                Text = "Hi?n th? m?t kh?u",
                Location = new Point(25, 385),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(127, 140, 141)
            };
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;

            // Error label
            lblError = new Label
            {
                Location = new Point(25, 420),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(231, 76, 60),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };

            // Register button
            btnRegister = new Button
            {
                Text = "??NG KÝ",
                Location = new Point(25, 460),
                Size = new Size(300, 45),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;

            // Hover effect for register button
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = Color.FromArgb(39, 174, 96);
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = Color.FromArgb(46, 204, 113);

            // Cancel button
            btnCancel = new Button
            {
                Text = "H?Y",
                Location = new Point(25, 515),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(127, 140, 141),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(189, 195, 199);
            btnCancel.Click += BtnCancel_Click;

            // Back to login link
            LinkLabel lnkBackToLogin = new LinkLabel
            {
                Text = "?ã có tài kho?n? ??ng nh?p",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(90, 560),
                Size = new Size(180, 20),
                LinkColor = Color.FromArgb(52, 152, 219),
                ActiveLinkColor = Color.FromArgb(41, 128, 185),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lnkBackToLogin.Click += (s, e) => this.Close();

            // Add controls to panel
            pnlMain.Controls.AddRange(new Control[]
            {
                lblTitle,
                lblFullName,
                txtFullName,
                lblEmail,
                txtEmail,
                lblPassword,
                txtPassword,
                lblConfirmPassword,
                txtConfirmPassword,
                chkShowPassword,
                lblError,
                btnRegister,
                btnCancel,
                lnkBackToLogin
            });

            // Add panel to form
            this.Controls.Add(pnlMain);

            // Enter key support
            txtFullName.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) txtEmail.Focus(); };
            txtEmail.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) txtPassword.Focus(); };
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) txtConfirmPassword.Focus(); };
            txtConfirmPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnRegister_Click(s, e); };
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            char displayChar = chkShowPassword.Checked ? '\0' : '?';
            txtPassword.PasswordChar = displayChar;
            txtConfirmPassword.PasswordChar = displayChar;
        }

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowError("Vui lòng nh?p h? và tên!");
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowError("Vui lòng nh?p email!");
                txtEmail.Focus();
                return;
            }

            // Basic email validation
            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                ShowError("Email không h?p l?!");
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Vui lòng nh?p m?t kh?u!");
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                ShowError("M?t kh?u ph?i có ít nh?t 6 ký t?!");
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ShowError("M?t kh?u xác nh?n không kh?p!");
                txtConfirmPassword.Focus();
                return;
            }

            // Disable controls
            SetControlsEnabled(false);
            lblError.Visible = false;

            try
            {
                using (var context = new ToDoListContext())
                {
                    // Check if email already exists
                    var existingUser = context.Users
                        .FirstOrDefault(u => u.Email == txtEmail.Text.Trim());

                    if (existingUser != null)
                    {
                        ShowError("Email ?ã ???c s? d?ng!");
                        SetControlsEnabled(true);
                        txtEmail.Focus();
                        return;
                    }

                    // Create new user
                    var newUser = new User
                    {
                        FullName = txtFullName.Text.Trim(),
                        Email = txtEmail.Text.Trim().ToLower(),
                        PasswordHash = txtPassword.Text.Trim(), // TODO: Hash password in production
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    context.Users.Add(newUser);
                    await context.SaveChangesAsync();

                    // Store registered email for auto-fill
                    RegisteredEmail = txtEmail.Text.Trim();

                    // Show success message
                    MessageBox.Show(
                        $"Chào m?ng, {newUser.FullName}!\n\n??ng ký thành công!\nB?n có th? ??ng nh?p ngay bây gi?.",
                        "??ng ký thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Close form with success
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
            txtFullName.Enabled = enabled;
            txtEmail.Enabled = enabled;
            txtPassword.Enabled = enabled;
            txtConfirmPassword.Enabled = enabled;
            btnRegister.Enabled = enabled;
            btnCancel.Enabled = enabled;
            chkShowPassword.Enabled = enabled;

            btnRegister.Text = enabled ? "??NG KÝ" : "?ang x? lý...";
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
        }
    }
}
