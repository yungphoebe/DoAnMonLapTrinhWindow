using System;
using System.Drawing;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Forms
{
    // KHÔNG dùng partial ?? tránh Designer can thi?p
    public class CuculistMiniForm : Form
    {
        private Project _project;
        private TodoListApp.DAL.Models.Task? _currentTask;
        private System.Windows.Forms.Timer _timer;
        private TimeSpan _elapsedTime;
        private bool _isTimerRunning;
        
        // ? Break functionality variables
        private bool _isBreak = false;
        private TimeSpan _pausedTime = TimeSpan.Zero;
        private DateTime _breakStartTime;
        private TimeSpan _totalBreakTime = TimeSpan.Zero;
        private int _breakCount = 0;
        
        private Label lblTimer;
        private Label lblMarkDone;
        private Label lblBreakInfo;
        private Button btnBreak;
        private ToDoListContext _context;

        public CuculistMiniForm(Project project, TodoListApp.DAL.Models.Task? currentTask, TimeSpan elapsedTime)
        {
            _project = project;
            _currentTask = currentTask;
            _elapsedTime = elapsedTime;
            _isTimerRunning = true;

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000; // 1 second
            _timer.Tick += Timer_Tick;

            _context = new ToDoListContext();

            InitializeComponent();
            UpdateDisplay();

            if (_isTimerRunning)
            {
                _timer.Start();
            }
        }

        private void InitializeComponent()
        {
            // ===== FORM SETTINGS =====
            this.Size = new Size(490, 60); // ? Gi?m width t? 540 v? 490
            this.Text = "Cuculist - Focus Timer";
            this.StartPosition = FormStartPosition.Manual;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.TopMost = true;
            
            // Position at top center of screen
            this.Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                0
            );

            CreateControls();
        }

        private void CreateControls()
        {
            // ===== "Mark me as done" LABEL =====
            lblMarkDone = new Label
            {
                Text = "Mark me as done",
                Location = new Point(15, 18),
                Size = new Size(180, 35),
                Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                AutoSize = false
            };
            lblMarkDone.Click += LblMarkDone_Click;
            lblMarkDone.MouseEnter += (s, e) => lblMarkDone.ForeColor = Color.Gray;
            lblMarkDone.MouseLeave += (s, e) => lblMarkDone.ForeColor = Color.Black;

            // ? BREAK BUTTON
            btnBreak = new Button
            {
                Text = "?",
                Location = new Point(200, 15),
                Size = new Size(40, 35),
                Font = new Font("Segoe UI", 16F, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(255, 152, 0), // Màu cam
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnBreak.FlatAppearance.BorderSize = 0;
            btnBreak.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 172, 50);
            btnBreak.Click += BtnBreak_Click;
            
            // Tooltip
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnBreak, "Tạm dừng để nghỉ giải lao");

            // ? BREAK INFO LABEL (?n m?c ??nh)
            lblBreakInfo = new Label
            {
                Text = "",
                Location = new Point(245, 5), // ? ?i?u ch?nh l?i v? trí
                Size = new Size(90, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Visible = false
            };

            // ===== TIMER LABEL =====
            lblTimer = new Label
            {
                Text = "00:15:30",
                Location = new Point(245, 23), // ? ?i?u ch?nh l?i v? trí
                Size = new Size(230, 30),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false
            };

            this.Controls.Add(lblMarkDone);
            this.Controls.Add(btnBreak);
            this.Controls.Add(lblBreakInfo);
            this.Controls.Add(lblTimer);

            // ===== DRAGGABLE =====
            this.MouseDown += Form_MouseDown;
            lblMarkDone.MouseDown += Form_MouseDown;
            lblTimer.MouseDown += Form_MouseDown;
            btnBreak.MouseDown += Form_MouseDown;

            // ===== BORDER =====
            this.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                }
            };
        }

        // ? BREAK BUTTON CLICK EVENT
        private void BtnBreak_Click(object? sender, EventArgs e)
        {
            if (!_isBreak)
            {
                // ===== B?T ??U BREAK (T?M D?NG) =====
                _isBreak = true;
                _isTimerRunning = false;
                _breakCount++;
                
                // L?u l?i th?i gian ?ã ch?y
                _pausedTime = _elapsedTime;
                _breakStartTime = DateTime.Now;
                
                // ===== ??I GIAO DI?N =====
                btnBreak.Text = "?";
                btnBreak.BackColor = Color.FromArgb(76, 175, 80); // Màu xanh lá
                lblTimer.ForeColor = Color.FromArgb(255, 152, 0); // Màu cam
                lblBreakInfo.Visible = true;
                
                // Tooltip
                var toolTip = new ToolTip();
                toolTip.SetToolTip(btnBreak, "Tiếp tục làm việc");
                
                // ===== THÔNG BÁO =====
                this.Text = $"Cuculist - Break #{_breakCount}";
            }
            else
            {
                // ===== RESUME L?I TIMER =====
                _isBreak = false;
                _isTimerRunning = true;
                
                // Th?ng kê th?i gian break
                var breakDuration = DateTime.Now - _breakStartTime;
                _totalBreakTime += breakDuration;
                
                // ===== ??I GIAO DI?N TR? L?I =====
                btnBreak.Text = "?";
                btnBreak.BackColor = Color.FromArgb(255, 152, 0); // Màu cam
                lblTimer.ForeColor = Color.Black;
                lblBreakInfo.Visible = false;
                
                // Tooltip
                var toolTip = new ToolTip();
                toolTip.SetToolTip(btnBreak, "Tạm dừng để nghỉ giải lao");
                
                this.Text = "Cuculist - Focus Timer";
            }
        }

        private async void LblMarkDone_Click(object? sender, EventArgs e)
        {
            if (_currentTask != null)
            {
                // ? Hi?n th? th?ng kê break trong thông báo
                string breakStats = _breakCount > 0 
                    ? $"\n\n?? Thống kê:\n• Số lần nghỉ: {_breakCount}\n• Tăng thời gian nghỉ: {_totalBreakTime:mm\\:ss}\n• Thời gian làm việc thực: {(_elapsedTime - _totalBreakTime):hh\\:mm\\:ss}"
                    : "";

                var result = MessageBox.Show(
                    $"Đánh dấu task '{_currentTask.Title}' là hoàn thành?{breakStats}",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _currentTask.Status = "Completed";
                        _currentTask.UpdatedAt = DateTime.Now;
                        _currentTask.ActualMinutes = (int)_elapsedTime.TotalMinutes;

                        await _context.SaveChangesAsync();

                        MessageBox.Show(
                            $" Task '{_currentTask.Title}' đã hoàn thành!\n\n?? Thời gian: {_elapsedTime:hh\\:mm\\:ss}",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        this.Tag = _elapsedTime;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Lỗi khi cập nhậttask: {ex.Message}",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Không có task nào đang được chọn",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private Point _lastPoint;
        private bool _isDragging;

        private void Form_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _lastPoint = e.Location;
                this.Cursor = Cursors.SizeAll;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isDragging)
            {
                this.Location = new Point(
                    this.Location.X + e.X - _lastPoint.X,
                    this.Location.Y + e.Y - _lastPoint.Y
                );
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isDragging = false;
            this.Cursor = Cursors.Default;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // ? CH? c?p nh?t th?i gian khi KHÔNG ?ang Break
            if (_isTimerRunning && !_isBreak)
            {
                _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
                UpdateDisplay();
            }
            // ? Hi?n th? th?i gian break
            else if (_isBreak)
            {
                var breakDuration = DateTime.Now - _breakStartTime;
                lblBreakInfo.Text = $"Break: {breakDuration:mm\\:ss}";
            }
        }

        private void UpdateDisplay()
        {
            lblTimer.Text = _elapsedTime.ToString(@"hh\:mm\:ss");
        }

        public TimeSpan GetElapsedTime()
        {
            return _elapsedTime;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Stop();
                _timer?.Dispose();
                _context?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Tag = _elapsedTime;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return true;
            }
            // ? Phím Space ?? Break/Resume
            else if (keyData == Keys.Space)
            {
                BtnBreak_Click(null, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
