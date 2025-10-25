using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using ToDoList.GUI.Helpers;

namespace ToDoList.GUI.Forms
{
    public partial class AddTaskToListForm : Form
    {
        private ToDoListContext _context;
        private int _userId;
        private TextBox txtTitle;
        private ComboBox cmbProjects;
        private Button btnSave;
        private Button btnCancel;

        public AddTaskToListForm()
        {
            InitializeComponent();
            InitializeDatabase();
            SetupUI();
            LoadProjects();
        }

        private void InitializeDatabase()
        {
            try
            {
                _context = new ToDoListContext();
                _userId = UserSession.GetUserId();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối với database: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Add task to list";
            this.Size = new Size(640, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // Add rounded corners effect
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        private void SetupUI()
        {
            // Main container panel
            Panel pnlMain = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(640, 580),
                BackColor = Color.White,
                Dock = DockStyle.Fill
            };

            // Close button (X)
            Button btnClose = new Button
            {
                Text = "?",
                Location = new Point(590, 15),
                Size = new Size(35, 35),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnClose.Click += (s, e) => this.Close();

            // Title label
            Label lblTitle = new Label
            {
                Text = "Add task to list",
                Location = new Point(0, 60),
                Size = new Size(640, 40),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 33, 33),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // "Title" label
            Label lblTitleLabel = new Label
            {
                Text = "Title",
                Location = new Point(80, 140),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(66, 66, 66),
                BackColor = Color.Transparent
            };

            // Title textbox
            txtTitle = new TextBox
            {
                Location = new Point(80, 170),
                Size = new Size(480, 40),
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(66, 66, 66),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Enter task title*"
            };
            
            // Placeholder handling
            txtTitle.ForeColor = Color.Gray;
            txtTitle.Enter += (s, e) =>
            {
                if (txtTitle.Text == "Enter task title*")
                {
                    txtTitle.Text = "";
                    txtTitle.ForeColor = Color.FromArgb(66, 66, 66);
                }
            };
            txtTitle.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    txtTitle.Text = "Enter task title*";
                    txtTitle.ForeColor = Color.Gray;
                }
            };

            // Add border to textbox
            Panel txtTitleBorder = new Panel
            {
                Location = new Point(78, 168),
                Size = new Size(484, 44),
                BackColor = Color.Transparent
            };
            txtTitleBorder.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(200, 200, 200), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, txtTitleBorder.Width - 1, txtTitleBorder.Height - 1);
                }
            };

            // "Select a list to add your task to" label
            Label lblSelectList = new Label
            {
                Text = "Select a list to add your task to",
                Location = new Point(80, 250),
                Size = new Size(480, 25),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(66, 66, 66),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Projects combobox
            cmbProjects = new ComboBox
            {
                Location = new Point(80, 285),
                Size = new Size(480, 40),
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };

            // Custom draw for combobox
            Panel cmbProjectsBorder = new Panel
            {
                Location = new Point(78, 283),
                Size = new Size(484, 44),
                BackColor = Color.Transparent
            };
            cmbProjectsBorder.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(200, 200, 200), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, cmbProjectsBorder.Width - 1, cmbProjectsBorder.Height - 1);
                }
            };

            // Cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(80, 380),
                Size = new Size(220, 55),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(66, 66, 66),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(66, 66, 66);
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.Click += (s, e) => this.Close();

            // Add rounded corners to Cancel button
            btnCancel.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCancel.Width, btnCancel.Height, 30, 30));

            // Save button
            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(340, 380),
                Size = new Size(220, 55),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(80, 200, 120), // Green color
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 220, 140);
            btnSave.Click += BtnSave_Click;

            // Add rounded corners to Save button
            btnSave.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width, btnSave.Height, 30, 30));

            // Add all controls to main panel
            pnlMain.Controls.Add(btnClose);
            pnlMain.Controls.Add(lblTitle);
            pnlMain.Controls.Add(lblTitleLabel);
            pnlMain.Controls.Add(txtTitleBorder);
            pnlMain.Controls.Add(txtTitle);
            pnlMain.Controls.Add(lblSelectList);
            pnlMain.Controls.Add(cmbProjectsBorder);
            pnlMain.Controls.Add(cmbProjects);
            pnlMain.Controls.Add(btnCancel);
            pnlMain.Controls.Add(btnSave);

            // Bring textbox and combobox to front
            txtTitle.BringToFront();
            cmbProjects.BringToFront();

            this.Controls.Add(pnlMain);

            // Add shadow effect
            this.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                }
            };
        }

        private async void LoadProjects()
        {
            try
            {
                if (_context == null || _userId == 0) return;

                var projects = await _context.Projects
                    .Where(p => p.UserId == _userId && p.IsArchived != true)
                    .OrderBy(p => p.ProjectName)
                    .ToListAsync();

                cmbProjects.Items.Clear();

                foreach (var project in projects)
                {
                    cmbProjects.Items.Add(new ComboBoxItem
                    {
                        Text = $"?? {project.ProjectName}",
                        Value = project.ProjectId
                    });
                }

                if (cmbProjects.Items.Count > 0)
                {
                    cmbProjects.SelectedIndex = 0;
                }
                else
                {
                    cmbProjects.Items.Add(new ComboBoxItem
                    {
                        Text = "No lists available",
                        Value = -1
                    });
                    cmbProjects.SelectedIndex = 0;
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i khi t?i danh sách: {ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == "Enter task title*")
                {
                    MessageBox.Show("Vui lòng nh?p tiêu ?? công vi?c!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTitle.Focus();
                    return;
                }

                if (cmbProjects.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng ch?n danh sách!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedItem = cmbProjects.SelectedItem as ComboBoxItem;
                if (selectedItem == null || selectedItem.Value == -1)
                {
                    MessageBox.Show("Vui lòng ch?n danh sách h?p l?!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Disable button to prevent double click
                btnSave.Enabled = false;
                btnSave.Text = "?ang l?u...";

                // Create new task
                var task = new TodoListApp.DAL.Models.Task
                {
                    ProjectId = selectedItem.Value,
                    UserId = _userId,
                    Title = txtTitle.Text.Trim(),
                    Priority = "Medium",
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                // Save to database
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                MessageBox.Show("Thêm công vi?c thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close form with success
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i khi thêm công vi?c: {ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = "Save";
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

        // Helper class for ComboBox items
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
