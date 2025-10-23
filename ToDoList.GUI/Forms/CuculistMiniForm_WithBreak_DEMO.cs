// ============================================
// DEMO CODE: Cuculist Mini Form v?i n�t Break
// ============================================

using System;
using System.Drawing;
using System.Windows.Forms;
using TodoListApp.DAL.Models;

namespace ToDoList.GUI.Forms
{
    /// <summary>
    /// Form timer mini v?i ch?c n?ng Break/Resume
    /// S? d?ng ?? ??m th?i gian l�m vi?c v?i kh? n?ng t?m d?ng
    /// </summary>
    public partial class CuculistMiniFormWithBreak : Form
    {
        // ============================================
        // BI?N TIMER
        // ============================================
        private System.Windows.Forms.Timer timer;
        private DateTime startTime;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private TimeSpan pausedTime = TimeSpan.Zero;
        
        // ============================================
        // BI?N TR?NG TH�I
        // ============================================
        private bool isPaused = false;
        private bool isBreak = false;  // ? NEW: Bi?n ki?m tra ?ang break
        
        // ============================================
        // BI?N TH?NG K� BREAK (Optional)
        // ============================================
        private DateTime breakStartTime;
        private TimeSpan totalBreakTime = TimeSpan.Zero;
        private int breakCount = 0;
        
        // ============================================
        // CONTROLS
        // ============================================
        private Label lblTimer;
        private Label lblTaskTitle;
        private Label lblBreakInfo; // ? NEW: Hi?n th? th�ng tin break
        private Button btnMarkDone;
        private Button btnBreak;    // ? NEW: N�t Break
        private Button btnStop;
        
        // ============================================
        // DATA
        // ============================================
        private Project _project;
        private TodoListApp.DAL.Models.Task _currentTask;

        // ============================================
        // CONSTRUCTOR
        // ============================================
        public CuculistMiniFormWithBreak(Project project)
        {
            _project = project;
            InitializeComponent();
            InitializeTimer();
        }

        // ============================================
        // INITIALIZE COMPONENTS
        // ============================================
        private void InitializeComponent()
        {
            // Form properties
            this.Size = new Size(420, 200);  // T?ng k�ch th??c ?? ch?a n�t Break
            this.Text = "Cuculist Timer - Focus Session";
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - this.Width - 20, 20);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.TopMost = true;

            // ============================================
            // TASK TITLE
            // ============================================
            lblTaskTitle = new Label
            {
                Text = _project?.ProjectName ?? "Focus Session",
                Location = new Point(20, 15),
                Size = new Size(380, 30),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            // ============================================
            // TIMER DISPLAY
            // ============================================
            lblTimer = new Label
            {
                Text = "00:00:00",
                Location = new Point(20, 50),
                Size = new Size(200, 40),
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 149, 237),  // M�u xanh d??ng
                BackColor = Color.Transparent
            };

            // ============================================
            // ? NEW: BREAK INFO LABEL
            // ============================================
            lblBreakInfo = new Label
            {
                Text = "",
                Location = new Point(230, 55),
                Size = new Size(170, 30),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent,
                Visible = false
            };

            // ============================================
            // BUTTON: MARK AS DONE
            // ============================================
            btnMarkDone = new Button
            {
                Text = "? Mark me as done",
                Location = new Point(20, 110),
                Size = new Size(150, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(76, 175, 80),  // M�u xanh l�
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnMarkDone.FlatAppearance.BorderSize = 0;
            btnMarkDone.FlatAppearance.MouseOverBackColor = Color.FromArgb(96, 195, 100);
            btnMarkDone.Click += BtnMarkDone_Click;

            // ============================================
            // ? NEW: BUTTON: BREAK/RESUME
            // ============================================
            btnBreak = new Button
            {
                Text = "? Break",
                Location = new Point(180, 110),
                Size = new Size(110, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 152, 0),  // M�u cam
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBreak.FlatAppearance.BorderSize = 0;
            btnBreak.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 172, 50);
            btnBreak.Click += BtnBreak_Click;

            // ============================================
            // BUTTON: STOP
            // ============================================
            btnStop = new Button
            {
                Text = "? Stop",
                Location = new Point(300, 110),
                Size = new Size(100, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(244, 67, 54),  // M�u ??
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 87, 74);
            btnStop.Click += BtnStop_Click;

            // ============================================
            // ADD CONTROLS TO FORM
            // ============================================
            this.Controls.Add(lblTaskTitle);
            this.Controls.Add(lblTimer);
            this.Controls.Add(lblBreakInfo);
            this.Controls.Add(btnMarkDone);
            this.Controls.Add(btnBreak);
            this.Controls.Add(btnStop);
        }

        // ============================================
        // INITIALIZE TIMER
        // ============================================
        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 gi�y
            timer.Tick += Timer_Tick;
            
            startTime = DateTime.Now;
            timer.Start();
        }

        // ============================================
        // TIMER TICK EVENT
        // ============================================
        private void Timer_Tick(object sender, EventArgs e)
        {
            // CH? c?p nh?t th?i gian khi KH�NG ?ang Break
            if (!isPaused && !isBreak)
            {
                elapsedTime = DateTime.Now - startTime + pausedTime;
                lblTimer.Text = elapsedTime.ToString(@"hh\:mm\:ss");
            }
            // ? NEW: Hi?n th? th?i gian break
            else if (isBreak)
            {
                var breakDuration = DateTime.Now - breakStartTime;
                lblBreakInfo.Text = $"Break: {breakDuration:mm\\:ss}";
                
                // L�m nh�y timer khi break (optional)
                lblTimer.Visible = (DateTime.Now.Second % 2 == 0);
            }
        }

        // ============================================
        // ? NEW: BREAK BUTTON CLICK EVENT
        // ============================================
        private void BtnBreak_Click(object sender, EventArgs e)
        {
            if (!isBreak)
            {
                // ===================================
                // B?T ??U BREAK (T?M D?NG)
                // ===================================
                isPaused = true;
                isBreak = true;
                breakCount++;
                
                // L?u l?i th?i gian ?� ch?y
                pausedTime = elapsedTime;
                breakStartTime = DateTime.Now;
                
                // ===================================
                // ??I GIAO DI?N
                // ===================================
                btnBreak.Text = "? Resume";
                btnBreak.BackColor = Color.FromArgb(76, 175, 80);  // M�u xanh l�
                lblTimer.ForeColor = Color.FromArgb(255, 152, 0);  // M�u cam
                lblBreakInfo.Visible = true;
                
                // ===================================
                // TH�NG B�O
                // ===================================
                this.Text = $"Cuculist Timer - Break #{breakCount}";
                
                MessageBox.Show(
                    $"? B?n ?ang ngh? gi?i lao!\n\n" +
                    $"Break l?n {breakCount}\n" +
                    $"Th?i gian l�m vi?c: {elapsedTime:hh\\:mm\\:ss}\n\n" +
                    $"Nh?n 'Resume' khi s?n s�ng ti?p t?c!", 
                    "Break Time ?", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );
            }
            else
            {
                // ===================================
                // RESUME L?I TIMER
                // ===================================
                isBreak = false;
                isPaused = false;
                
                // C?p nh?t l?i startTime ?? t�nh to�n ?�ng
                startTime = DateTime.Now;
                
                // Th?ng k� th?i gian break
                var breakDuration = DateTime.Now - breakStartTime;
                totalBreakTime += breakDuration;
                
                // ===================================
                // ??I GIAO DI?N TR? L?I
                // ===================================
                btnBreak.Text = "? Break";
                btnBreak.BackColor = Color.FromArgb(255, 152, 0);  // M�u cam
                lblTimer.ForeColor = Color.FromArgb(100, 149, 237);  // M�u xanh d??ng
                lblTimer.Visible = true;  // ??m b?o hi?n th? l?i
                lblBreakInfo.Visible = false;
                
                this.Text = "Cuculist Timer - Focus Session";
                
                // ===================================
                // TH�NG B�O
                // ===================================
                MessageBox.Show(
                    $"? Ti?p t?c l�m vi?c!\n\n" +
                    $"Th?i gian ngh?: {breakDuration:mm\\:ss}\n" +
                    $"T?ng th?i gian ngh?: {totalBreakTime:mm\\:ss}", 
                    "Back to Work ??", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );
            }
        }

        // ============================================
        // MARK AS DONE BUTTON CLICK EVENT
        // ============================================
        private void BtnMarkDone_Click(object sender, EventArgs e)
        {
            timer.Stop();
            
            var result = MessageBox.Show(
                $"? B?n ?� ho�n th�nh c�ng vi?c!\n\n" +
                $"?? Th?ng k�:\n" +
                $"� Th?i gian l�m vi?c: {elapsedTime:hh\\:mm\\:ss}\n" +
                $"� S? l?n break: {breakCount}\n" +
                $"� T?ng th?i gian break: {totalBreakTime:mm\\:ss}\n" +
                $"� Th?i gian l�m vi?c th?c: {(elapsedTime - totalBreakTime):hh\\:mm\\:ss}\n\n" +
                $"?�nh d?u l� ho�n th�nh?",
                "?? Ho�n th�nh!", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question
            );
            
            if (result == DialogResult.Yes)
            {
                // TODO: L?u v�o database
                SaveToDatabase();
                
                MessageBox.Show(
                    "? ?� ?�nh d?u ho�n th�nh!\n\nC?m ?n b?n ?� l�m vi?c ch?m ch?! ??", 
                    "Th�nh c�ng", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                timer.Start();
            }
        }

        // ============================================
        // STOP BUTTON CLICK EVENT
        // ============================================
        private void BtnStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            
            var result = MessageBox.Show(
                $"? D?ng timer?\n\n" +
                $"?? Th?ng k�:\n" +
                $"� Th?i gian ?� l�m: {elapsedTime:hh\\:mm\\:ss}\n" +
                $"� S? l?n break: {breakCount}\n" +
                $"� T?ng th?i gian break: {totalBreakTime:mm\\:ss}",
                "D?ng Timer", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question
            );
            
            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                timer.Start();
            }
        }

        // ============================================
        // SAVE TO DATABASE
        // ============================================
        private void SaveToDatabase()
        {
            try
            {
                // TODO: Implement database saving logic
                // Example:
                // var focusSession = new FocusSession
                // {
                //     UserId = _userId,
                //     TaskId = _currentTask?.TaskId,
                //     StartTime = startTime,
                //     EndTime = DateTime.Now,
                //     DurationMinutes = (int)elapsedTime.TotalMinutes,
                //     Notes = $"Breaks: {breakCount}, Break time: {totalBreakTime:mm\\:ss}"
                // };
                // _context.FocusSessions.Add(focusSession);
                // await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i khi l?u d? li?u: {ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // DISPOSE
        // ============================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer?.Stop();
                timer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

// ============================================
// H??NG D?N S? D?NG
// ============================================
/*

1. Thay th? file CuculistMiniForm.cs hi?n t?i b?ng code n�y
   HO?C
2. Merge code n�y v�o file hi?n t?i:
   - Th�m bi?n isBreak, pausedTime, breakStartTime, totalBreakTime, breakCount
   - Th�m n�t btnBreak
   - Th�m Label lblBreakInfo
   - Th�m h�m BtnBreak_Click
   - C?p nh?t Timer_Tick ?? ki?m tra isBreak

3. S? d?ng:
   using (var timerForm = new CuculistMiniFormWithBreak(project))
   {
       timerForm.ShowDialog();
   }

4. K?t qu?:
   - N�t "Break": M�u cam, text "? Break"
   - Khi b?m Break: Text ??i th�nh "? Resume", m�u xanh l�
   - Timer d?ng l?i
   - Hi?n th? th�ng tin break
   - B?m Resume ?? ti?p t?c

*/
