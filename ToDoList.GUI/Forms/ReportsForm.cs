using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ToDoList.GUI.Helpers;

// Use alias to avoid conflicts
using DrawingColor = System.Drawing.Color;
using PdfColor = iText.Kernel.Colors.Color;

namespace ToDoList.GUI.Forms
{
    public partial class ReportsForm : Form
    {
        private ToDoListContext _context;
        private Panel pnlHeader;
        private Panel pnlStats;
        private Panel pnlChart;
        private ComboBox cmbProject;
        private ComboBox cmbMonth;
        private ComboBox cmbDay;
        private Button btnUpdate;
        private Button btnExport;
        
        // ✅ NEW: Store current statistics
        private StatisticsData _currentStats;
        private int _userId;

        public ReportsForm(ToDoListContext context)
        {
            _context = context;
            _userId = UserSession.GetUserId();
            
            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            InitializeComponent();
            LoadFilters();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Bao cao thong ke - ToDoList";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = DrawingColor.FromArgb(20, 20, 20);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // ===== HEADER PANEL =====
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 140,
                BackColor = DrawingColor.FromArgb(20, 20, 20)
            };

            Label lblBack = new Label
            {
                Text = "BACK",
                Location = new Point(20, 20),
                Size = new Size(80, 25),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = DrawingColor.Gray,
                Cursor = Cursors.Hand
            };
            lblBack.Click += (s, e) => this.Close();

            Label lblTitle = new Label
            {
                Text = "Reports",
                Location = new Point(450, 15),
                Size = new Size(200, 35),
                Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = DrawingColor.White,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnUpgrade = new Button
            {
                Text = "Cap nhat",
                Location = new Point(900, 20),
                Size = new Size(100, 35),
                BackColor = DrawingColor.FromArgb(100, 200, 150),
                ForeColor = DrawingColor.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point),
                Cursor = Cursors.Hand
            };
            btnUpgrade.FlatAppearance.BorderSize = 0;

            // ===== FILTERS ROW =====
            Label lblFilter = new Label
            {
                Text = "Tat ca du an",
                Location = new Point(20, 70),
                Size = new Size(120, 25),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = DrawingColor.Gray
            };

            cmbProject = new ComboBox
            {
                Location = new Point(20, 95),
                Size = new Size(160, 30),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = DrawingColor.FromArgb(40, 40, 40),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // ✅ NEW: Add event handler for project selection change
            cmbProject.SelectedIndexChanged += CmbProject_SelectedIndexChanged;

            Label lblQuestion = new Label
            {
                Text = "?",
                Location = new Point(190, 95),
                Size = new Size(20, 25),
                Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = DrawingColor.Gray
            };

            cmbMonth = new ComboBox
            {
                Location = new Point(250, 95),
                Size = new Size(120, 30),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = DrawingColor.FromArgb(40, 40, 40),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMonth.SelectedIndexChanged += (s, e) => LoadData();

            Label lblDash = new Label
            {
                Text = "-",
                Location = new Point(380, 95),
                Size = new Size(20, 25),
                Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = DrawingColor.Gray
            };

            Label lblAy = new Label
            {
                Text = "ay,",
                Location = new Point(400, 95),
                Size = new Size(30, 25),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = DrawingColor.Gray
            };

            cmbDay = new ComboBox
            {
                Location = new Point(440, 95),
                Size = new Size(100, 30),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = DrawingColor.FromArgb(100, 149, 237),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDay.SelectedIndexChanged += (s, e) => LoadData();

            btnUpdate = new Button
            {
                Text = "Update",
                Location = new Point(560, 90),
                Size = new Size(80, 35),
                BackColor = DrawingColor.FromArgb(100, 149, 237),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point),
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += (s, e) => LoadData();

            btnExport = new Button
            {
                Text = "Export",
                Location = new Point(660, 90),
                Size = new Size(100, 35),
                BackColor = DrawingColor.FromArgb(80, 200, 120),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point),
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatAppearance.MouseOverBackColor = DrawingColor.FromArgb(100, 220, 140);
            btnExport.Click += BtnExport_Click;

            Label lblTime = new Label
            {
                Text = "Mui gio: " + DateTime.Now.ToString("ddd, dd MMM HH:mm tt"),
                Location = new Point(900, 95),
                Size = new Size(250, 25),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = DrawingColor.Gray,
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlHeader.Controls.AddRange(new Control[] {
                lblBack, lblTitle, btnUpgrade,
                lblFilter, cmbProject, lblQuestion, cmbMonth, lblDash, lblAy, cmbDay, 
                btnUpdate, btnExport, lblTime
            });

            // ===== STATS PANEL =====
            pnlStats = new Panel
            {
                Location = new Point(0, 140),
                Size = new Size(1200, 100),
                BackColor = DrawingColor.FromArgb(20, 20, 20)
            };

            // ✅ MODIFIED: Will be dynamically updated
            UpdateStatsPanel();

            // ===== CHART PANEL =====
            pnlChart = new Panel
            {
                Location = new Point(0, 240),
                Size = new Size(1200, 460),
                BackColor = DrawingColor.FromArgb(20, 20, 20)
            };
            pnlChart.Paint += PnlChart_Paint;

            Label lblAdvanced = new Label
            {
                Text = "Tien do theo ngay",
                Location = new Point(20, 10),
                Size = new Size(200, 25),
                Font = new Font("Arial", 11F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = DrawingColor.White
            };
            pnlChart.Controls.Add(lblAdvanced);

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlStats);
            this.Controls.Add(pnlChart);
        }

        // ✅ NEW: Event handler for project selection change
        private void CmbProject_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadData();
        }

        // ✅ NEW: Update stats panel with dynamic data
        private void UpdateStatsPanel()
        {
            pnlStats.Controls.Clear();

            if (_currentStats == null)
            {
                _currentStats = GetStatisticsData();
            }

            var stats = new[]  {
                new { Label = "TONG SO NGAY LAM", Value = $"{_currentStats.TotalWorkingDays} ngay", SubText = (string?)null, X = 20 },
                new { Label = "TASK HOAN THANH", Value = $"{_currentStats.CompletedTasks}/{_currentStats.TotalTasks}", SubText = (string?)$"{(_currentStats.TotalTasks > 0 ? (double)_currentStats.CompletedTasks / _currentStats.TotalTasks : 0):F1} / ngay", X = 300 },
                new { Label = "TONG GIO LAM VIEC", Value = $"{_currentStats.TotalWorkingHours:F1}h", SubText = (string?)$"{(_currentStats.TotalWorkingDays > 0 ? _currentStats.TotalWorkingHours / _currentStats.TotalWorkingDays : 0):F1}h / ngay", X = 590 },
                new { Label = "THOI GIAN TB/TASK", Value = $"{_currentStats.AverageTimePerTask:F0} phut", SubText = (string?)$"{_currentStats.CompletionRate:F1}% hoan thanh", X = 880 }
            };

            foreach (var stat in stats)
            {
                Panel statBox = new Panel
                {
                    Location = new Point(stat.X, 20),
                    Size = new Size(260, 70),
                    BackColor = DrawingColor.FromArgb(30, 30, 30)
                };

                Label lblStat = new Label
                {
                    Text = stat.Label,
                    Location = new Point(10, 10),
                    Size = new Size(240, 20),
                    Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = DrawingColor.Gray
                };

                Label lblValue = new Label
                {
                    Text = stat.Value,
                    Location = new Point(10, 30),
                    Size = new Size(150, 25),
                    Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = DrawingColor.White
                };

                statBox.Controls.Add(lblStat);
                statBox.Controls.Add(lblValue);

                if (!string.IsNullOrEmpty(stat.SubText))
                {
                    Label lblSub = new Label
                    {
                        Text = stat.SubText,
                        Location = new Point(160, 35),
                        Size = new Size(90, 20),
                        Font = new Font("Arial", 7F, FontStyle.Regular, GraphicsUnit.Point),
                        ForeColor = DrawingColor.Gray,
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    statBox.Controls.Add(lblSub);
                }

                pnlStats.Controls.Add(statBox);
            }
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            ContextMenuStrip exportMenu = new ContextMenuStrip
            {
                BackColor = DrawingColor.FromArgb(40, 40, 40),
                ForeColor = DrawingColor.White,
                RenderMode = ToolStripRenderMode.Professional
            };

            var pdfItem = exportMenu.Items.Add("📄 Export to PDF");
            pdfItem.BackColor = DrawingColor.FromArgb(40, 40, 40);
            pdfItem.ForeColor = DrawingColor.White;
            pdfItem.Click += (s, args) => ExportToPDF();

            var excelItem = exportMenu.Items.Add("📊 Export to Excel");
            excelItem.BackColor = DrawingColor.FromArgb(40, 40, 40);
            excelItem.ForeColor = DrawingColor.White;
            excelItem.Click += (s, args) => ExportToExcel();

            exportMenu.Show(btnExport, new Point(0, btnExport.Height));
        }

        private void ExportToPDF()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files|*.pdf",
                    Title = "Export Bao cao to PDF",
                    FileName = $"BaoCao_ToDoList_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var stats = _currentStats ?? GetStatisticsData();

                    using (var writer = new PdfWriter(saveFileDialog.FileName))
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);
                        
                        var title = new Paragraph("BAO CAO THONG KE - TODOLIST")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(20)
                            .SetBold()
                            .SetMarginBottom(20);
                        document.Add(title);

                        var dateInfo = new Paragraph($"Ngay xuat: {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(10)
                            .SetMarginBottom(30);
                        document.Add(dateInfo);

                        Table table = new Table(2);
                        table.SetWidth(UnitValue.CreatePercentValue(100));

                        table.AddHeaderCell(new Cell().Add(new Paragraph("Chu so").SetBold()));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Gia tri").SetBold()));

                        table.AddCell("Tong so ngay lam");
                        table.AddCell(stats.TotalWorkingDays.ToString() + " ngay");

                        table.AddCell("Task hoan thanh");
                        table.AddCell($"{stats.CompletedTasks}/{stats.TotalTasks}");

                        table.AddCell("Tong gio lam viec");
                        table.AddCell(stats.TotalWorkingHours.ToString("F1") + "h");

                        table.AddCell("Thoi gian TB/Task");
                        table.AddCell(stats.AverageTimePerTask.ToString("F0") + " phut");

                        table.AddCell("Ti le hoan thanh");
                        table.AddCell(stats.CompletionRate.ToString("F1") + "%");

                        document.Add(table);

                        if (stats.TaskDetails != null && stats.TaskDetails.Any())
                        {
                            document.Add(new Paragraph("\n\nCHI TIET TASKS")
                                .SetBold()
                                .SetFontSize(14)
                                .SetMarginTop(20));

                            Table taskTable = new Table(4);
                            taskTable.SetWidth(UnitValue.CreatePercentValue(100));

                            taskTable.AddHeaderCell("Ten Task");
                            taskTable.AddHeaderCell("Trang thai");
                            taskTable.AddHeaderCell("Du kien");
                            taskTable.AddHeaderCell("Uu tien");

                            foreach (var task in stats.TaskDetails)
                            {
                                taskTable.AddCell(task.Title);
                                taskTable.AddCell(task.Status);
                                taskTable.AddCell((task.EstimatedMinutes ?? 0) + " phut");
                                taskTable.AddCell(task.Priority ?? "Medium");
                            }

                            document.Add(taskTable);
                        }

                        document.Close();
                    }

                    MessageBox.Show($"Export PDF thanh cong!\n\nFile: {saveFileDialog.FileName}", 
                        "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var result = MessageBox.Show("Ban co muon mo file PDF khong?", 
                        "Mo file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveFileDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi export PDF:\n\n{ex.Message}", 
                    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Export Bao cao to Excel",
                    FileName = $"BaoCao_ToDoList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var stats = _currentStats ?? GetStatisticsData();

                    using (var package = new ExcelPackage())
                    {
                        var summarySheet = package.Workbook.Worksheets.Add("Tong quan");

                        summarySheet.Cells["A1"].Value = "BAO CAO THONG KE - TODOLIST";
                        summarySheet.Cells["A1:D1"].Merge = true;
                        summarySheet.Cells["A1"].Style.Font.Size = 16;
                        summarySheet.Cells["A1"].Style.Font.Bold = true;
                        summarySheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        summarySheet.Cells["A2"].Value = $"Ngay xuat: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                        summarySheet.Cells["A2:D2"].Merge = true;
                        summarySheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        int row = 4;
                        summarySheet.Cells[row, 1].Value = "Chu so";
                        summarySheet.Cells[row, 2].Value = "Gia tri";
                        summarySheet.Cells[row, 1, row, 2].Style.Font.Bold = true;
                        summarySheet.Cells[row, 1, row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        summarySheet.Cells[row, 1, row, 2].Style.Fill.BackgroundColor.SetColor(DrawingColor.FromArgb(100, 149, 237));
                        summarySheet.Cells[row, 1, row, 2].Style.Font.Color.SetColor(DrawingColor.White);

                        row++;
                        summarySheet.Cells[row, 1].Value = "Tong so ngay lam";
                        summarySheet.Cells[row, 2].Value = stats.TotalWorkingDays + " ngay";

                        row++;
                        summarySheet.Cells[row, 1].Value = "Task hoan thanh";
                        summarySheet.Cells[row, 2].Value = $"{stats.CompletedTasks}/{stats.TotalTasks}";

                        row++;
                        summarySheet.Cells[row, 1].Value = "Tong gio lam viec";
                        summarySheet.Cells[row, 2].Value = stats.TotalWorkingHours.ToString("F1") + "h";

                        row++;
                        summarySheet.Cells[row, 1].Value = "Thoi gian TB/Task";
                        summarySheet.Cells[row, 2].Value = stats.AverageTimePerTask.ToString("F0") + " phut";

                        row++;
                        summarySheet.Cells[row, 1].Value = "Ti le hoan thanh";
                        summarySheet.Cells[row, 2].Value = stats.CompletionRate.ToString("F1") + "%";

                        summarySheet.Columns[1].Width = 30;
                        summarySheet.Columns[2].Width = 20;

                        if (stats.TaskDetails != null && stats.TaskDetails.Any())
                        {
                            var taskSheet = package.Workbook.Worksheets.Add("Chi tiet Tasks");

                            taskSheet.Cells[1, 1].Value = "Ten Task";
                            taskSheet.Cells[1, 2].Value = "Trang thai";
                            taskSheet.Cells[1, 3].Value = "Du kien (phut)";
                            taskSheet.Cells[1, 4].Value = "Uu tien";
                            taskSheet.Cells[1, 5].Value = "Ngay tao";
                            taskSheet.Cells[1, 6].Value = "Deadline";

                            taskSheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;
                            taskSheet.Cells[1, 1, 1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            taskSheet.Cells[1, 1, 1, 6].Style.Fill.BackgroundColor.SetColor(DrawingColor.FromArgb(100, 149, 237));
                            taskSheet.Cells[1, 1, 1, 6].Style.Font.Color.SetColor(DrawingColor.White);

                            int taskRow = 2;
                            foreach (var task in stats.TaskDetails)
                            {
                                taskSheet.Cells[taskRow, 1].Value = task.Title;
                                taskSheet.Cells[taskRow, 2].Value = task.Status;
                                taskSheet.Cells[taskRow, 3].Value = task.EstimatedMinutes ?? 0;
                                taskSheet.Cells[taskRow, 4].Value = task.Priority ?? "Medium";
                                taskSheet.Cells[taskRow, 5].Value = task.CreatedAt?.ToString("dd/MM/yyyy");
                                taskSheet.Cells[taskRow, 6].Value = task.DueDate?.ToString("dd/MM/yyyy") ?? "N/A";

                                if (task.Status == "Completed")
                                {
                                    taskSheet.Cells[taskRow, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    taskSheet.Cells[taskRow, 2].Style.Fill.BackgroundColor.SetColor(DrawingColor.LightGreen);
                                }
                                else if (task.Status == "In Progress")
                                {
                                    taskSheet.Cells[taskRow, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    taskSheet.Cells[taskRow, 2].Style.Fill.BackgroundColor.SetColor(DrawingColor.LightBlue);
                                }

                                taskRow++;
                            }

                            taskSheet.Cells[taskSheet.Dimension.Address].AutoFitColumns();
                        }

                        package.SaveAs(new FileInfo(saveFileDialog.FileName));
                    }

                    MessageBox.Show($"Export Excel thanh cong!\n\nFile: {saveFileDialog.FileName}", 
                        "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var result = MessageBox.Show("Ban co muon mo file Excel khong?", 
                        "Mo file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveFileDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi export Excel:\n\n{ex.Message}", 
                    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ MODIFIED: Get statistics data with filters
        private StatisticsData GetStatisticsData()
        {
            try
            {
                var query = _context.Tasks
                    .Where(t => t.IsDeleted != true && t.UserId == _userId);

                // ✅ Filter by selected project
                if (cmbProject.SelectedIndex > 0) // ✅ FIXED: Added closing parenthesis
                {
                    string selectedProject = cmbProject.SelectedItem.ToString() ?? "";
                    var project = _context.Projects.FirstOrDefault(p => p.ProjectName == selectedProject);
                    if (project != null)
                    {
                        query = query.Where(t => t.ProjectId == project.ProjectId);
                    }
                }

                // ✅ Filter by selected month
                if (cmbMonth.SelectedIndex >= 0)
                {
                    int selectedMonth = cmbMonth.SelectedIndex + 1;
                    int currentYear = DateTime.Now.Year;
                    query = query.Where(t => t.CreatedAt.HasValue && 
                                            t.CreatedAt.Value.Month == selectedMonth &&
                                            t.CreatedAt.Value.Year == currentYear);
                }

                var tasks = query.ToList();

                var completedTasks = tasks.Count(t => t.Status == "Completed");
                var totalTasks = tasks.Count;

                var totalMinutes = tasks.Sum(t => t.EstimatedMinutes ?? 0);
                var totalHours = totalMinutes / 60.0;

                var avgTimePerTask = totalTasks > 0 ? (double)totalMinutes / totalTasks : 0;

                var completionRate = totalTasks > 0 ? ((double)completedTasks / totalTasks) * 100 : 0;

                var workingDays = tasks
                    .Where(t => t.CreatedAt.HasValue)
                    .Select(t => t.CreatedAt.Value.Date)
                    .Distinct()
                    .Count();

                return new StatisticsData
                {
                    TotalWorkingDays = workingDays,
                    CompletedTasks = completedTasks,
                    TotalTasks = totalTasks,
                    TotalWorkingHours = totalHours,
                    AverageTimePerTask = avgTimePerTask,
                    CompletionRate = completionRate,
                    TaskDetails = tasks.OrderByDescending(t => t.CreatedAt).ToList()
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi lay du lieu thong ke: {ex.Message}", 
                    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new StatisticsData();
            }
        }

        // ✅ MODIFIED: Paint chart with real data
        private void PnlChart_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (_currentStats == null || _currentStats.TaskDetails == null || !_currentStats.TaskDetails.Any())
            {
                // Show "No data" message
                string noDataMsg = "Khong co du lieu de hien thi";
                SizeF textSize = g.MeasureString(noDataMsg, new Font("Arial", 14F));
                g.DrawString(noDataMsg, new Font("Arial", 14F), Brushes.Gray,
                    (pnlChart.Width - textSize.Width) / 2, pnlChart.Height / 2);
                return;
            }

            // ✅ Get real data grouped by day
            var tasksByDay = _currentStats.TaskDetails
                .Where(t => t.CreatedAt.HasValue)
                .GroupBy(t => t.CreatedAt.Value.Date)
                .OrderBy(g => g.Key)
                .Take(7) // Show last 7 days
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Count(),
                    Completed = g.Count(t => t.Status == "Completed")
                })
                .ToList();

            if (!tasksByDay.Any())
            {
                string noDataMsg = "Khong co du lieu theo ngay";
                SizeF dateLabelSize = g.MeasureString(noDataMsg, new Font("Arial", 14F));
                g.DrawString(noDataMsg, new Font("Arial", 14F), Brushes.Gray,
                    (pnlChart.Width - dateLabelSize.Width) / 2, pnlChart.Height / 2);
                return;
            }

            int dataCount = tasksByDay.Count;
            int spacing = Math.Min(1100 / dataCount, 200);
            int barWidth = Math.Min(spacing - 20, 180);
            int maxHeight = 280;
            int baseY = 380;
            int startX = 50;

            int maxTotal = tasksByDay.Max(d => d.Total);
            if (maxTotal == 0) maxTotal = 1;

            // Draw bars and collect points for line chart
            var linePoints = new List<PointF>();

            for (int i = 0; i < tasksByDay.Count; i++)
            {
                var day = tasksByDay[i];
                int x = startX + (i * spacing);
                
                int totalHeight = (int)(maxHeight * ((double)day.Total / maxTotal));
                int completedHeight = (int)(maxHeight * ((double)day.Completed / maxTotal));

                // Gray bar (total)
                Rectangle rectTotal = new Rectangle(x, baseY - totalHeight, barWidth, totalHeight);
                using (SolidBrush brush = new SolidBrush(DrawingColor.FromArgb(80, 80, 80)))
                {
                    g.FillRectangle(brush, rectTotal);
                }

                // Green bar (completed)
                if (completedHeight > 0)
                {
                    Rectangle rectCompleted = new Rectangle(x, baseY - completedHeight, barWidth, completedHeight);
                    using (SolidBrush brush = new SolidBrush(DrawingColor.FromArgb(100, 200, 150)))
                    {
                        g.FillRectangle(brush, rectCompleted);
                    }
                }

                // Label above bar
                string label = $"{day.Completed}/{day.Total}";
                SizeF textSize = g.MeasureString(label, new Font("Arial", 10F));
                g.DrawString(label, new Font("Arial", 10F), Brushes.White,
                    x + barWidth / 2 - textSize.Width / 2, baseY - totalHeight - 25);

                // Date label below
                string dateLabel = day.Date.ToString("dd/MM");
                SizeF dateLabelSize = g.MeasureString(dateLabel, new Font("Arial", 8F));
                g.DrawString(dateLabel, new Font("Arial", 8F), Brushes.Gray,
                    x + barWidth / 2 - dateLabelSize.Width / 2, baseY + 10);

                // Add point for line chart
                linePoints.Add(new PointF(x + barWidth / 2, baseY - completedHeight));
            }

            // Draw line chart
            if (linePoints.Count > 1)
            {
                using (Pen pen = new Pen(DrawingColor.FromArgb(100, 149, 237), 3))
                {
                    g.DrawLines(pen, linePoints.ToArray());

                    // Draw points
                    foreach (var point in linePoints)
                    {
                        g.FillEllipse(new SolidBrush(DrawingColor.FromArgb(100, 149, 237)), 
                            point.X - 4, point.Y - 4, 8, 8);
                    }
                }
            }
        }

        private void LoadFilters()
        {
            // Projects
            cmbProject.Items.Add("Tat ca du an");
            try
            {
                var projects = _context.Projects
                    .Where(p => p.IsArchived != true && p.UserId == _userId)
                    .Select(p => p.ProjectName)
                    .ToList();
                foreach (var p in projects)
                    cmbProject.Items.Add(p);
            }
            catch { }
            cmbProject.SelectedIndex = 0;

            // Months
            string[] months = { "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December" };
            cmbMonth.Items.AddRange(months);
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;

            // Days
            cmbDay.Items.AddRange(new[] { "Monday", "Tuesday", "Wednesday", "Thursday",
                "Friday", "Saturday", "Sunday", "All days", "Weekdays", "Weekends"
               });
            cmbDay.SelectedIndex = 7; // All days
        }

        // ✅ MODIFIED: Load data and update UI
        private void LoadData()
        {
            try
            {
                _currentStats = GetStatisticsData();
                UpdateStatsPanel();
                pnlChart.Invalidate(); // Redraw chart
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Loi khi tai du lieu: {ex.Message}", 
                    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }

    public class StatisticsData
    {
        public int TotalWorkingDays { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalTasks { get; set; }
        public double TotalWorkingHours { get; set; }
        public double AverageTimePerTask { get; set; }
        public double CompletionRate { get; set; }
        public List<TodoListApp.DAL.Models.Task>? TaskDetails { get; set; }
    }
}
