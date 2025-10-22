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
        private Label lblTimer;
        private Label lblMarkDone;
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
            this.Size = new Size(370, 60);
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
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                AutoSize = false
            };
            lblMarkDone.Click += LblMarkDone_Click;
            lblMarkDone.MouseEnter += (s, e) => lblMarkDone.ForeColor = Color.Gray;
            lblMarkDone.MouseLeave += (s, e) => lblMarkDone.ForeColor = Color.Black;

            // ===== TIMER LABEL =====
            lblTimer = new Label
            {
                Text = "00:15:30",
                Location = new Point(245, 18),
                Size = new Size(110, 25),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false
            };

            this.Controls.Add(lblMarkDone);
            this.Controls.Add(lblTimer);

            // ===== DRAGGABLE =====
            this.MouseDown += Form_MouseDown;
            lblMarkDone.MouseDown += Form_MouseDown;
            lblTimer.MouseDown += Form_MouseDown;

            // ===== BORDER =====
            this.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                }
            };
        }

        private async void LblMarkDone_Click(object? sender, EventArgs e)
        {
            if (_currentTask != null)
            {
                var result = MessageBox.Show(
                    $"?ánh d?u task '{_currentTask.Title}' là hoàn thành?",
                    "Xác nh?n",
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
                            $"? Task '{_currentTask.Title}' ?ã hoàn thành!",
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
                            $"L?i khi c?p nh?t task: {ex.Message}",
                            "L?i",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Không có task nào ?ang ???c ch?n.",
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
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
            UpdateDisplay();
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
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
