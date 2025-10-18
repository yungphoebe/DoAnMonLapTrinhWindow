using System;
using System.Drawing;
using System.Windows.Forms;
using ToDoList.GUI.Resources;

namespace ToDoList.GUI.Forms
{
    public partial class LanguageSettingsForm : Form
    {
        private ComboBox cboLanguage;
        private Button btnSave;
        private Button btnCancel;
        private Label lblLanguage;
        private Label lblInfo;

        public LanguageSettingsForm()
        {
            InitializeComponent();
            LoadCurrentLanguage();
        }

        private void InitializeComponent()
        {
            this.Text = Strings.Settings;
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Font font = new Font("Segoe UI", 10F);

            // Title Label
            Label lblTitle = new Label
            {
                Text = Strings.Language,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(360, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(0, 120, 215),
                Padding = new Padding(10, 10, 10, 10)
            };

            // Language Label
            lblLanguage = new Label
            {
                Text = $"{Strings.Language}:",
                Font = font,
                AutoSize = true,
                Location = new Point(30, 80)
            };

            // Language ComboBox
            cboLanguage = new ComboBox
            {
                Font = font,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(250, 30),
                Location = new Point(130, 77)
            };

            foreach (var lang in LanguageManager.GetAvailableLanguages())
            {
                cboLanguage.Items.Add(lang);
            }
            cboLanguage.DisplayMember = "Value";
            cboLanguage.ValueMember = "Key";

            // Info Label
            lblInfo = new Label
            {
                Text = "* ?ng d?ng s? ???c c?p nh?t sau khi l?u",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(350, 40),
                Location = new Point(30, 120),
                TextAlign = ContentAlignment.TopLeft
            };

            // Save Button
            btnSave = new Button
            {
                Text = Strings.Save,
                Font = font,
                Size = new Size(100, 35),
                Location = new Point(180, 170),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            // Cancel Button
            btnCancel = new Button
            {
                Text = Strings.Cancel,
                Font = font,
                Size = new Size(100, 35),
                Location = new Point(290, 170),
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            // Add controls
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblLanguage);
            this.Controls.Add(cboLanguage);
            this.Controls.Add(lblInfo);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void LoadCurrentLanguage()
        {
            var currentLanguage = LanguageManager.CurrentLanguage;
            for (int i = 0; i < cboLanguage.Items.Count; i++)
            {
                var item = (KeyValuePair<SupportedLanguage, string>)cboLanguage.Items[i];
                if (item.Key == currentLanguage)
                {
                    cboLanguage.SelectedIndex = i;
                    break;
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (cboLanguage.SelectedItem == null)
            {
                MessageBox.Show(
                    Strings.ValidationRequired,
                    Strings.Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var selectedLanguage = ((KeyValuePair<SupportedLanguage, string>)cboLanguage.SelectedItem).Key;

            if (selectedLanguage != LanguageManager.CurrentLanguage)
            {
                LanguageManager.SetLanguage(selectedLanguage);

                MessageBox.Show(
                    "Language changed successfully! The application will now update.\n\n" +
                    "?ã thay ??i ngôn ng? thành công! ?ng d?ng s? ???c c?p nh?t.",
                    Strings.Success,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}
