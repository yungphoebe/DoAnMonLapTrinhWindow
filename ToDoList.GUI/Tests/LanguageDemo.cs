using System;
using System.Windows.Forms;
using ToDoList.GUI.Resources;

namespace ToDoList.GUI.Tests
{
    public class LanguageDemo : Form
    {
        private Label lblWelcome;
        private Label lblTaskTitle;
        private Button btnCreateTask;
        private Button btnSave;
        private Button btnCancel;
        private Button btnChangeLanguage;
        private GroupBox grpDemo;

        public LanguageDemo()
        {
            InitializeComponent();
            ApplyLanguage();
            
            // Subscribe to language change
            LanguageManager.LanguageChanged += (s, e) => ApplyLanguage();
        }

        private void InitializeComponent()
        {
            this.Text = "Language Demo";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            var font = new System.Drawing.Font("Segoe UI", 10F);

            // Welcome Label
            lblWelcome = new Label
            {
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                AutoSize = false,
                Size = new System.Drawing.Size(450, 40),
                Location = new System.Drawing.Point(20, 20),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White
            };

            // Group Demo
            grpDemo = new GroupBox
            {
                Location = new System.Drawing.Point(20, 80),
                Size = new System.Drawing.Size(450, 200),
                Font = font
            };

            // Task Title Label
            lblTaskTitle = new Label
            {
                Font = font,
                AutoSize = true,
                Location = new System.Drawing.Point(20, 40)
            };

            // Create Task Button
            btnCreateTask = new Button
            {
                Font = font,
                Size = new System.Drawing.Size(150, 35),
                Location = new System.Drawing.Point(20, 80),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCreateTask.Click += (s, e) => MessageBox.Show(
                Strings.SuccessTaskCreated,
                Strings.Success,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Save Button
            btnSave = new Button
            {
                Font = font,
                Size = new System.Drawing.Size(100, 35),
                Location = new System.Drawing.Point(20, 130),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Cancel Button
            btnCancel = new Button
            {
                Font = font,
                Size = new System.Drawing.Size(100, 35),
                Location = new System.Drawing.Point(130, 130),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };

            grpDemo.Controls.Add(lblTaskTitle);
            grpDemo.Controls.Add(btnCreateTask);
            grpDemo.Controls.Add(btnSave);
            grpDemo.Controls.Add(btnCancel);

            // Change Language Button
            btnChangeLanguage = new Button
            {
                Text = "?? Change Language / ??i Ngôn Ng?",
                Font = font,
                Size = new System.Drawing.Size(450, 40),
                Location = new System.Drawing.Point(20, 300),
                BackColor = System.Drawing.Color.FromArgb(255, 140, 0),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnChangeLanguage.Click += BtnChangeLanguage_Click;

            this.Controls.Add(lblWelcome);
            this.Controls.Add(grpDemo);
            this.Controls.Add(btnChangeLanguage);
        }

        private void ApplyLanguage()
        {
            lblWelcome.Text = $"?? {Strings.AppName} {Strings.AppVersion}";
            grpDemo.Text = Strings.Tasks;
            lblTaskTitle.Text = $"{Strings.TaskTitle}: {Strings.CreateTask}";
            btnCreateTask.Text = Strings.CreateTask;
            btnSave.Text = Strings.Save;
            btnCancel.Text = Strings.Cancel;

            var langName = LanguageManager.GetLanguageDisplayName(LanguageManager.CurrentLanguage);
            this.Text = $"Language Demo - Current: {langName}";
        }

        private void BtnChangeLanguage_Click(object? sender, EventArgs e)
        {
            var currentLang = LanguageManager.CurrentLanguage;
            var newLang = currentLang == SupportedLanguage.Vietnamese 
                ? SupportedLanguage.English 
                : SupportedLanguage.Vietnamese;

            LanguageManager.SetLanguage(newLang);

            MessageBox.Show(
                $"Language changed to: {LanguageManager.GetLanguageDisplayName(newLang)}\n\n" +
                $"?ã ??i ngôn ng? sang: {LanguageManager.GetLanguageDisplayName(newLang)}",
                "Language / Ngôn ng?",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        [STAThread]
        public static void ShowDemo()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LanguageManager.Initialize();
            Application.Run(new LanguageDemo());
        }
    }
}
