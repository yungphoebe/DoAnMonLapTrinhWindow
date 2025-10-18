using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ToDoList.GUI.Resources;

namespace ToDoList.GUI.Tests
{
    public class DatabaseTestForm : Form
    {
        private RichTextBox txtResult;
        private Button btnOK;
        private Label lblTitle;
        
        public DatabaseTestForm()
        {
            InitializeComponents();
            RunTest();
        }
        
        private void InitializeComponents()
        {
            // Form settings
            this.Text = Strings.DbConnectionTitle;
            this.Size = new Size(650, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            
            // Font ti?ng Vi?t ??p
            Font vietnameseFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            Font titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            
            // Title Label
            lblTitle = new Label
            {
                Text = Strings.DbConnectionTitle.ToUpper(),
                Font = titleFont,
                AutoSize = false,
                Size = new Size(620, 40),
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White
            };
            
            // Result TextBox
            txtResult = new RichTextBox
            {
                Location = new Point(10, 60),
                Size = new Size(620, 410),
                Font = vietnameseFont,
                ReadOnly = true,
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                WordWrap = true
            };
            
            // OK Button
            btnOK = new Button
            {
                Text = Strings.OK,
                Size = new Size(100, 35),
                Location = new Point(270, 480),
                Font = vietnameseFont,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOK.Click += (s, e) => this.Close();
            
            // Add controls
            this.Controls.Add(lblTitle);
            this.Controls.Add(txtResult);
            this.Controls.Add(btnOK);
        }
        
        private void RunTest()
        {
            string connectionString = "Server=DESKTOP-LN5QDF6\\SQLEXPRESS;Database=ToDoListApp;Integrated Security=True;TrustServerCertificate=True";
            
            AppendText("================================================\n", Color.DarkBlue, true);
            AppendText($"      {Strings.DbConnectionTitle.ToUpper()}\n", Color.DarkBlue, true);
            AppendText("================================================\n\n", Color.DarkBlue, true);
            
            AppendText($"{Strings.DbServer}: DESKTOP-LN5QDF6\\SQLEXPRESS\n", Color.Black);
            AppendText($"{Strings.DbDatabase}: ToDoListApp\n", Color.Black);
            AppendText($"{Strings.DbAuthentication}: Windows Authentication\n\n", Color.Black);
            
            AppendText($"{Strings.DbConnecting}\n\n", Color.DarkOrange);
            Application.DoEvents();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    AppendText($"? {Strings.DbConnectionSuccess}\n\n", Color.Green, true);
                    
                    AppendText($"Server Version: {connection.ServerVersion}\n", Color.Black);
                    AppendText($"{Strings.DbDatabase}: {connection.Database}\n", Color.Black);
                    AppendText($"State: {connection.State}\n\n", Color.Black);
                    
                    AppendText($"{Strings.DbCheckingTables}\n\n", Color.DarkOrange);
                    Application.DoEvents();
                    
                    string[] tables = { "Users", "Tasks", "Projects", "Tags", "FocusSessions", 
                                       "Reminders", "ActivityLog", "UserSettings", "ProjectMembers" };
                    
                    int totalRecords = 0;
                    foreach (string table in tables)
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM {table}", connection))
                            {
                                int count = (int)command.ExecuteScalar();
                                AppendText($"? {table,-20} : {count,5} {Strings.DbRecords}\n", Color.Green);
                                totalRecords += count;
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendText($"? {table,-20} : {Strings.Error}\n", Color.Red);
                        }
                    }
                    
                    AppendText("\n================================================\n", Color.DarkBlue, true);
                    AppendText($"   ? {Strings.DbTotalRecords.ToUpper()}: {totalRecords} {Strings.DbRecords.ToUpper()}\n", Color.Green, true);
                    AppendText("================================================\n", Color.DarkBlue, true);
                }
            }
            catch (SqlException ex)
            {
                txtResult.Clear();
                AppendText($"? {Strings.DbErrorTitle}!\n\n", Color.Red, true);
                
                AppendText($"{Strings.DbErrorCode}: {ex.Number}\n", Color.DarkRed);
                AppendText($"{Strings.DbErrorMessage}: {ex.Message}\n\n", Color.DarkRed);
                
                AppendText($"?? {Strings.DbTroubleshooting}:\n\n", Color.DarkOrange, true);
                
                switch (ex.Number)
                {
                    case 2:
                    case 53:
                        AppendText($"? {Strings.DbServerNotFound}\n\n", Color.Red, true);
                        AppendText("Vui lòng ki?m tra:\n", Color.Black);
                        AppendText("? Tên server: DESKTOP-LN5QDF6\\SQLEXPRESS\n", Color.Black);
                        AppendText("? SQL Server ?ã ???c kh?i ??ng ch?a?\n", Color.Black);
                        AppendText("? SQL Server Configuration Manager\n", Color.Black);
                        AppendText("? SQL Server Browser service ?ang ch?y\n", Color.Black);
                        break;
                    case 4060:
                        AppendText($"? {Strings.DbDatabaseNotExist}\n\n", Color.Red, true);
                        AppendText("Gi?i pháp:\n", Color.Black);
                        AppendText("? T?o database m?i tên 'ToDoListApp'\n", Color.Black);
                        AppendText("? Ho?c ki?m tra l?i tên database\n", Color.Black);
                        AppendText("? Ch?y migration ?? t?o database\n", Color.Black);
                        break;
                    case 18456:
                        AppendText($"? {Strings.DbAuthenticationFailed}\n\n", Color.Red, true);
                        AppendText("Gi?i pháp:\n", Color.Black);
                        AppendText("? Ki?m tra Windows Authentication có ???c b?t\n", Color.Black);
                        AppendText("? Tài kho?n Windows có quy?n truy c?p SQL Server\n", Color.Black);
                        break;
                    default:
                        AppendText($"? L?i không xác ??nh: {ex.Message}\n", Color.Red);
                        break;
                }
            }
            catch (Exception ex)
            {
                txtResult.Clear();
                AppendText($"? {Strings.Error}:\n\n", Color.Red, true);
                AppendText(ex.Message, Color.DarkRed);
            }
        }
        
        private void AppendText(string text, Color color, bool bold = false)
        {
            int start = txtResult.TextLength;
            txtResult.AppendText(text);
            int end = txtResult.TextLength;
            
            txtResult.Select(start, end - start);
            txtResult.SelectionColor = color;
            if (bold)
            {
                txtResult.SelectionFont = new Font(txtResult.Font, FontStyle.Bold);
            }
            txtResult.SelectionLength = 0;
            txtResult.ScrollToCaret();
        }
    }
}
