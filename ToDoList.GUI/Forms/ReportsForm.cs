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
        
        // ? NEW: Export button
        private Button btnExport;

        public ReportsForm(ToDoListContext context)
        {
            _context = context;
            
            // ? Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            InitializeComponent();
            LoadFilters();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "?? Báo cáo thống kê- ToDoList";
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
                Font = new Font("Segoe UI", 10F),
                ForeColor = DrawingColor.Gray,
                Cursor = Cursors.Hand
            };
            lblBack.Click += (s, e) => this.Close();

            Label lblTitle = new Label
            {
                Text = "Reports",
                Location = new Point(450, 15),
                Size = new Size(200, 35),
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = DrawingColor.White,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnUpgrade = new Button
            {
                Text = "? update",
                Location = new Point(900, 20),
                Size = new Size(100, 35),
                BackColor = DrawingColor.FromArgb(100, 200, 150),
                ForeColor = DrawingColor.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnUpgrade.FlatAppearance.BorderSize = 0;

            Button btnHelp = new Button
            {
                Text = "?",
                Location = new Point(1020, 20),
                Size = new Size(40, 35),
                BackColor = DrawingColor.FromArgb(40, 40, 40),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnHelp.FlatAppearance.BorderSize = 0;

            Button btnUser = new Button
            {
                Text = "D",
                Location = new Point(1100, 20),
                Size = new Size(40, 35),
                BackColor = DrawingColor.FromArgb(100, 149, 237),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnUser.FlatAppearance.BorderSize = 0;

            // ===== FILTERS ROW =====
            Label lblFilter = new Label
            {
                Text = "?? Tất cả dự án",
                Location = new Point(20, 70),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = DrawingColor.Gray
            };

            cmbProject = new ComboBox
            {
                Location = new Point(20, 95),
                Size = new Size(160, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = DrawingColor.FromArgb(40, 40, 40),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblQuestion = new Label
            {
                Text = "?",
                Location = new Point(190, 95),
                Size = new Size(20, 25),
                Font = new Font("Segoe UI", 12F),
                ForeColor = DrawingColor.Gray
            };

            cmbMonth = new ComboBox
            {
                Location = new Point(250, 95),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = DrawingColor.FromArgb(40, 40, 40),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblDash = new Label
            {
                Text = "-",
                Location = new Point(380, 95),
                Size = new Size(20, 25),
                Font = new Font("Segoe UI", 14F),
                ForeColor = DrawingColor.Gray
            };

            Label lblAy = new Label
            {
                Text = "ay,",
                Location = new Point(400, 95),
                Size = new Size(30, 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = DrawingColor.Gray
            };

            cmbDay = new ComboBox
            {
                Location = new Point(440, 95),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = DrawingColor.FromArgb(100, 149, 237),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnUpdate = new Button
            {
                Text = "Update",
                Location = new Point(560, 90),
                Size = new Size(80, 35),
                BackColor = DrawingColor.FromArgb(100, 149, 237),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += (s, e) => LoadData();

            // ? NEW: Export button
            btnExport = new Button
            {
                Text = " Export",
                Location = new Point(660, 90),
                Size = new Size(100, 35),
                BackColor = DrawingColor.FromArgb(80, 200, 120),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatAppearance.MouseOverBackColor = DrawingColor.FromArgb(100, 220, 140);
            btnExport.Click += BtnExport_Click;

            Label lblTime = new Label
            {
                Text = "?? Mui gio: " + DateTime.Now.ToString("ddd, dd MMM HH:mm tt"),
                Location = new Point(900, 95),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 9F),
                ForeColor = DrawingColor.Gray,
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlHeader.Controls.AddRange(new Control[] {
                lblBack, lblTitle, btnUpgrade, btnHelp, btnUser,
                lblFilter, cmbProject, lblQuestion, cmbMonth, lblDash, lblAy, cmbDay, 
                btnUpdate, btnExport, lblTime  // ? Added btnExport
            });

            // ===== STATS PANEL =====
            pnlStats = new Panel
            {
                Location = new Point(0, 140),
                Size = new Size(1200, 100),
                BackColor = DrawingColor.FromArgb(20, 20, 20)
            };

            // Stats boxes
            var stats = new[]  {
                new { Label = "TỔNG SỐ TASK NGÀY LÀM", Value = "23 ngày", SubText = (string?)null, X = 50 },
                new { Label = "TASK HOÀN THÀNH", Value = "03/17", SubText = (string?)"0.1 / ngày", X = 300 },
                new { Label = "TỔNG GIỜ LÀM VIỆC", Value = "87h", SubText = (string?)"3.8h / ngày", X = 590 },
                new { Label = "THỜI GIAN TB/TASK", Value = "348 phút", SubText = (string?)"17.6% hoàn thành", X = 900 }
            };

            foreach (var stat in stats)
            {
                Panel statBox = new Panel
                {
                    Location = new Point(stat.X, 20),
                    Size = new Size(250, 70),
                    BackColor = DrawingColor.FromArgb(30, 30, 30)
                };

                Label lblStat = new Label
                {
                    Text = stat.Label,
                    Location = new Point(10, 10),
                    Size = new Size(230, 20),
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = DrawingColor.Gray
                };

                Label lblValue = new Label
                {
                    Text = stat.Value,
                    Location = new Point(10, 30),
                    Size = new Size(150, 25),
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
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
                        Size = new Size(80, 20),
                        Font = new Font("Segoe UI", 7F),
                        ForeColor = DrawingColor.Gray,
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    statBox.Controls.Add(lblSub);
                }

                pnlStats.Controls.Add(statBox);
            }

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
                Text = "Tiến độ theo",
                Location = new Point(20, 10),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = DrawingColor.White
            };
            pnlChart.Controls.Add(lblAdvanced);

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlStats);
            this.Controls.Add(pnlChart);
        }

        // ? NEW: Export button click handler
        private void BtnExport_Click(object? sender, EventArgs e)
        {
            // Show export options menu
            ContextMenuStrip exportMenu = new ContextMenuStrip
            {
                BackColor = DrawingColor.FromArgb(40, 40, 40),
                ForeColor = DrawingColor.White,
                RenderMode = ToolStripRenderMode.Professional
            };

            var pdfItem = exportMenu.Items.Add("Export to PDF");
            pdfItem.BackColor = DrawingColor.FromArgb(40, 40, 40);
            pdfItem.ForeColor = DrawingColor.White;
            pdfItem.Click += (s, args) => ExportToPDF();

            var excelItem = exportMenu.Items.Add("Export to Excel");
            excelItem.BackColor = DrawingColor.FromArgb(40, 40, 40);
            excelItem.ForeColor = DrawingColor.White;
            excelItem.Click += (s, args) => ExportToExcel();

            exportMenu.Show(btnExport, new Point(0, btnExport.Height));
        }

        // ? NEW: Export to PDF
        private void ExportToPDF()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files|*.pdf",
                    Title = "Export Báo cáo to PDF",
                    FileName = $"BaoCao_ToDoList_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get statistics data
                    var stats = GetStatisticsData();

                    // Create PDF
                    using (var writer = new PdfWriter(saveFileDialog.FileName))
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);
                        
                        // Title
                        var title = new Paragraph("BÁO CÁO THỐNG KÊ - TODOLIST")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(20)
                            .SetBold()
                            .SetMarginBottom(20);
                        document.Add(title);

                        // Date
                        var dateInfo = new Paragraph($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(10)
                            .SetMarginBottom(30);
                        document.Add(dateInfo);

                        // Statistics Table
                        Table table = new Table(2);
                        table.SetWidth(UnitValue.CreatePercentValue(100));

                        // Header
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Chữ số").SetBold()));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Giá trị").SetBold()));

                        // Data rows
                        table.AddCell("Tổng số ngày làm");
                        table.AddCell(stats.TotalWorkingDays.ToString() + " ngày");

                        table.AddCell("Task hoàn thành");
                        table.AddCell($"{stats.CompletedTasks}/{stats.TotalTasks}");

                        table.AddCell("Tổng giờ làm việc");
                        table.AddCell(stats.TotalWorkingHours.ToString("F1") + "h");

                        table.AddCell("Thời gian TB/Task");
                        table.AddCell(stats.AverageTimePerTask.ToString("F0") + " phút");

                        table.AddCell("Tỷ lệ hoàn thành");
                        table.AddCell(stats.CompletionRate.ToString("F1") + "%");

                        document.Add(table);

                        // Add task details if available
                        if (stats.TaskDetails != null && stats.TaskDetails.Any())
                        {
                            document.Add(new Paragraph("\n\nCHI TIẾT TASKS")
                                .SetBold()
                                .SetFontSize(14)
                                .SetMarginTop(20));

                            Table taskTable = new Table(4);
                            taskTable.SetWidth(UnitValue.CreatePercentValue(100));

                            taskTable.AddHeaderCell("Tên Task");
                            taskTable.AddHeaderCell("Trạng thái");
                            taskTable.AddHeaderCell("Dự kiến");
                            taskTable.AddHeaderCell("ưu tiên");

                            foreach (var task in stats.TaskDetails)
                            {
                                taskTable.AddCell(task.Title);
                                taskTable.AddCell(task.Status);
                                taskTable.AddCell((task.EstimatedMinutes ?? 0) + " phút");
                                taskTable.AddCell(task.Priority ?? "Medium");
                            }

                            document.Add(taskTable);
                        }

                        document.Close();
                    }

                    MessageBox.Show($"? Export PDF thành công!\n\nFile: {saveFileDialog.FileName}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Ask to open file
                    var result = MessageBox.Show("Bạn có muốn xuất file PDF không?", 
                        "Mờ file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                MessageBox.Show($"lỗi khi export PDF:\n\n{ex.Message}", 
                    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ? NEW: Export to Excel
        private void ExportToExcel()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Export Báo cáo to Excel",
                    FileName = $"BaoCao_ToDoList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var stats = GetStatisticsData();

                    using (var package = new ExcelPackage())
                    {
                        // Summary Sheet
                        var summarySheet = package.Workbook.Worksheets.Add("Tổng quan");

                        // Title
                        summarySheet.Cells["A1"].Value = "BÁO CÁO THỐNG KÊ - TODOLIST";
                        summarySheet.Cells["A1:D1"].Merge = true;
                        summarySheet.Cells["A1"].Style.Font.Size = 16;
                        summarySheet.Cells["A1"].Style.Font.Bold = true;
                        summarySheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Date
                        summarySheet.Cells["A2"].Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                        summarySheet.Cells["A2:D2"].Merge = true;
                        summarySheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Statistics
                        int row = 4;
                        summarySheet.Cells[row, 1].Value = "Chữ số";
                        summarySheet.Cells[row, 2].Value = "Giá trị";
                        summarySheet.Cells[row, 1, row, 2].Style.Font.Bold = true;
                        summarySheet.Cells[row, 1, row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        summarySheet.Cells[row, 1, row, 2].Style.Fill.BackgroundColor.SetColor(DrawingColor.FromArgb(100, 149, 237));
                        summarySheet.Cells[row, 1, row, 2].Style.Font.Color.SetColor(DrawingColor.White);

                        row++;
                        summarySheet.Cells[row, 1].Value = "Tổng số ngày làm";
                        summarySheet.Cells[row, 2].Value = stats.TotalWorkingDays + " ngày";

                        row++;
                        summarySheet.Cells[row, 1].Value = "Task hoàn thành";
                        summarySheet.Cells[row, 2].Value = $"{stats.CompletedTasks}/{stats.TotalTasks}";

                        row++;
                        summarySheet.Cells[row, 1].Value = "Tổng giờ làm việc";
                        summarySheet.Cells[row, 2].Value = stats.TotalWorkingHours.ToString("F1") + "h";

                        row++;
                        summarySheet.Cells[row, 1].Value = "Thời gian TB/Task";
                        summarySheet.Cells[row, 2].Value = stats.AverageTimePerTask.ToString("F0") + " phút";

                        row++;
                        summarySheet.Cells[row, 1].Value = "Tỷ lệ hoàn thành";
                        summarySheet.Cells[row, 2].Value = stats.CompletionRate.ToString("F1") + "%";

                        summarySheet.Columns[1].Width = 30;
                        summarySheet.Columns[2].Width = 20;

                        // Task Details Sheet
                        if (stats.TaskDetails != null && stats.TaskDetails.Any())
                        {
                            var taskSheet = package.Workbook.Worksheets.Add("Chi tiết Tasks");

                            // Headers
                            taskSheet.Cells[1, 1].Value = "Tên Task";
                            taskSheet.Cells[1, 2].Value = "Trạng thái";
                            taskSheet.Cells[1, 3].Value = "Dự kiến (phút)";
                            taskSheet.Cells[1, 4].Value = "ưu tiên";
                            taskSheet.Cells[1, 5].Value = "Ngày tạo";
                            taskSheet.Cells[1, 6].Value = "Deadline";

                            taskSheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;
                            taskSheet.Cells[1, 1, 1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            taskSheet.Cells[1, 1, 1, 6].Style.Fill.BackgroundColor.SetColor(DrawingColor.FromArgb(100, 149, 237));
                            taskSheet.Cells[1, 1, 1, 6].Style.Font.Color.SetColor(DrawingColor.White);

                            // Data
                            int taskRow = 2;
                            foreach (var task in stats.TaskDetails)
                            {
                                taskSheet.Cells[taskRow, 1].Value = task.Title;
                                taskSheet.Cells[taskRow, 2].Value = task.Status;
                                taskSheet.Cells[taskRow, 3].Value = task.EstimatedMinutes ?? 0;
                                taskSheet.Cells[taskRow, 4].Value = task.Priority ?? "Medium";
                                taskSheet.Cells[taskRow, 5].Value = task.CreatedAt?.ToString("dd/MM/yyyy");
                                taskSheet.Cells[taskRow, 6].Value = task.DueDate?.ToString("dd/MM/yyyy") ?? "N/A";

                                // Color code by status
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

                    MessageBox.Show($" Export Excel thành công!\n\nFile: {saveFileDialog.FileName}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Ask to open file
                    var result = MessageBox.Show("Bạn có muốn xuất file Excel không?", 
                        "Mờ file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                MessageBox.Show($" Lỗi khi export Excel:\n\n{ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ? NEW: Get statistics data
        private StatisticsData GetStatisticsData()
        {
            try
            {
                var tasks = _context.Tasks
                    .Where(t => t.IsDeleted != true)
                    .ToList();

                var completedTasks = tasks.Count(t => t.Status == "Completed");
                var totalTasks = tasks.Count;

                var totalMinutes = tasks.Sum(t => t.EstimatedMinutes ?? 0);
                var totalHours = totalMinutes / 60.0;

                var avgTimePerTask = totalTasks > 0 ? (double)totalMinutes / totalTasks : 0;

                var completionRate = totalTasks > 0 ? ((double)completedTasks / totalTasks) * 100 : 0;

                // Get working days (days with tasks)
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
                MessageBox.Show($"Lỗi khi lây dữ liệu thống kê: {ex.Message}", 
                    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new StatisticsData();
            }
        }

        private void PnlChart_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Chart data
            var data = new[] {
                new { Completed = 1, Total = 4, X = 50 },
                new { Completed = 2, Total = 6, X = 270 },
                new { Completed = 0, Total = 4, X = 490 },
                new { Completed = 0, Total = 1, X = 710 },
                new { Completed = 0, Total = 2, X = 930 }
            };

            int barWidth = 200;
            int maxHeight = 280;
            int baseY = 380;

            // Draw bars
            for (int i = 0; i < data.Length; i++)
            {
                var d = data[i];
                int totalHeight = (int)(maxHeight * (d.Total / 6.0)); // Max total is 6
                int completedHeight = (int)(maxHeight * (d.Completed / 6.0));

                // Gray bar (total)
                Rectangle rectTotal = new Rectangle(d.X, baseY - totalHeight, barWidth, totalHeight);
                using (SolidBrush brush = new SolidBrush(DrawingColor.FromArgb(80, 80, 80)))
                {
                    g.FillRectangle(brush, rectTotal);
                }

                // Green bar (completed)
                if (completedHeight > 0)
                {
                    Rectangle rectCompleted = new Rectangle(d.X, baseY - completedHeight, barWidth, completedHeight);
                    using (SolidBrush brush = new SolidBrush(DrawingColor.FromArgb(100, 200, 150)))
                    {
                        g.FillRectangle(brush, rectCompleted);
                    }
                }

                // Label above bar
                string label = $"{d.Completed}/{d.Total}";
                SizeF textSize = g.MeasureString(label, new Font("Segoe UI", 10F));
                g.DrawString(label, new Font("Segoe UI", 10F), Brushes.White,
                    d.X + barWidth / 2 - textSize.Width / 2, baseY - totalHeight - 25);
            }

            // Draw line chart
            using (Pen pen = new Pen(DrawingColor.FromArgb(100, 149, 237), 3))
            {
                PointF[] points = new PointF[]
                {
                    new PointF(150, baseY - 200),  // 1/4
                    new PointF(370, baseY - 240),  // 2/6
                    new PointF(590, baseY - 80),   // 0/4
                    new PointF(810, baseY - 40),   // 0/1
                    new PointF(1030, baseY - 20)   // 0/2
                };

                g.DrawLines(pen, points);

                // Draw points
                foreach (var point in points)
                {
                    g.FillEllipse(new SolidBrush(DrawingColor.FromArgb(100, 149, 237)), 
                        point.X - 4, point.Y - 4, 8, 8);
                }
            }
        }

        private void LoadFilters()
        {
            // Projects
            cmbProject.Items.Add("Tất cả dự án");
            try
            {
                var projects = _context.Projects
                    .Where(p => p.IsArchived != true)
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
            cmbDay.SelectedIndex = 9; // October
        }

        private void LoadData()
        {
            // Refresh chart
            pnlChart.Invalidate();
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

    // ? NEW: Statistics data class
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
