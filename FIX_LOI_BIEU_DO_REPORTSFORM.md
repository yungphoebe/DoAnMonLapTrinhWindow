# ?? FIX L?I BI?U ?? KHÔNG HI?N TH? - REPORTSFORM

## ? V?N ??

D?a vào hình ?nh b?n cung c?p, ReportsForm hi?n th?:
- ? **Stat cards** phía trên (OK)
- ? **Màn hình ?en** phía d??i (bi?u ?? không hi?n th?)

## ?? NGUYÊN NHÂN TH??NG G?P

### 1. Charts ch?a ???c add vào Panel
```csharp
// ? SAI: Ch? t?o chart nh?ng không add
Chart chart = new Chart();
// ... configure chart ...
// Quên: pnlCharts.Controls.Add(chart);
```

### 2. Chart size = 0 ho?c location sai
```csharp
// ? SAI: Chart b? ?n do size nh?
Chart chart = new Chart
{
    Size = new Size(0, 0)  // ? L?I: Size = 0
};
```

### 3. Panel không ???c add vào Form
```csharp
// ? SAI: T?o panel nh?ng không add vào form
Panel pnlCharts = new Panel();
// Quên: this.Controls.Add(pnlCharts);
```

### 4. Chart b? che b?i control khác
```csharp
// ? SAI: Control sau add s? ?è lên
this.Controls.Add(pnlCharts);    // Charts ? d??i
this.Controls.Add(pnlHeader);    // Header ?è lên charts
// ? ?ÚNG: Dùng BringToFront() ho?c ??i th? t?
```

### 5. Data ch?a ???c load
```csharp
// ? SAI: Chart không có data
chart.Series[0].Points.Clear();
// Quên: chart.Series[0].Points.AddXY(...);
```

## ? GI?I PHÁP

### B??c 1: Ki?m tra c?u trúc file

```
ToDoList.GUI/Forms/
??? ReportsForm.cs           ? Logic + InitializeComponent()
??? ReportsForm.Designer.cs  ? Auto-generated (n?u có)
??? ReportsForm.resx         ? Resources (n?u có)
```

**QUAN TR?NG**: 
- N?u có `.Designer.cs`, KHÔNG ???c ??nh ngh?a l?i các member trong `.cs`
- N?u KHÔNG có `.Designer.cs`, t?t c? code trong `.cs`

### B??c 2: Fix l?i trùng member

#### Tr??ng h?p A: Có Designer.cs
```csharp
// ReportsForm.cs
public partial class ReportsForm : Form
{
    // ? KHÔNG khai báo l?i các member ?ã có trong Designer.cs
    // private Panel pnlCharts;  ? XÓA dòng này
    
    public ReportsForm(ToDoListContext context)
    {
        _context = context;
        InitializeComponent();  // ? G?i t? Designer.cs
        LoadData();
    }
    
    // Ch? vi?t logic methods
    private void LoadData() { }
    private void LoadCharts() { }
}
```

#### Tr??ng h?p B: KHÔNG có Designer.cs
```csharp
// ReportsForm.cs
public class ReportsForm : Form  // ? KHÔNG có "partial"
{
    // ? Khai báo T?T C? members ? ?ây
    private ToDoListContext _context;
    private Panel pnlCharts;
    private Chart chartStatus;
    
    public ReportsForm(ToDoListContext context)
    {
        _context = context;
        InitializeComponent();  // ? T? vi?t trong file này
        LoadData();
    }
    
    private void InitializeComponent()
    {
        // T?o T?T C? controls ? ?ây
    }
}
```

### B??c 3: ??m b?o charts ???c add ?úng

```csharp
private void CreateCharts()
{
    // 1. T?O CHART
    Chart chartStatus = new Chart
    {
        Location = new Point(20, 20),
        Size = new Size(420, 350),  // ? Size rõ ràng
        BackColor = Color.FromArgb(40, 40, 40)
    };
    
    // 2. T?O CHARTAREA
    ChartArea chartArea = new ChartArea();
    chartStatus.ChartAreas.Add(chartArea);
    
    // 3. T?O SERIES
    Series series = new Series
    {
        Name = "Status",
        ChartType = SeriesChartType.Doughnut
    };
    chartStatus.Series.Add(series);
    
    // 4. ? QUAN TR?NG: ADD VÀO PANEL
    pnlCharts.Controls.Add(chartStatus);
}
```

### B??c 4: Load d? li?u

```csharp
private async void LoadStatusChart()
{
    try
    {
        var completed = await _context.Tasks
            .CountAsync(t => t.IsDeleted != true && t.Status == "Completed");
        var inProgress = await _context.Tasks
            .CountAsync(t => t.IsDeleted != true && t.Status == "In Progress");
        var pending = await _context.Tasks
            .CountAsync(t => t.IsDeleted != true && t.Status == "Pending");

        // ? ADD DATA POINTS
        chartStatus.Series[0].Points.Clear();
        chartStatus.Series[0].Points.AddXY("Completed", completed);
        chartStatus.Series[0].Points.AddXY("In Progress", inProgress);
        chartStatus.Series[0].Points.AddXY("Pending", pending);

        // ? SET COLORS
        chartStatus.Series[0].Points[0].Color = Color.FromArgb(76, 175, 80);
        chartStatus.Series[0].Points[1].Color = Color.FromArgb(255, 152, 0);
        chartStatus.Series[0].Points[2].Color = Color.FromArgb(244, 67, 54);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"L?i load chart: {ex.Message}");
    }
}
```

## ?? CHECK LIST

Khi debug, ki?m tra các ?i?m sau:

### 1. Form Load
- [ ] `InitializeComponent()` ???c g?i trong constructor
- [ ] `LoadData()` ???c g?i sau `InitializeComponent()`

### 2. Panel Charts
- [ ] `pnlCharts` ???c t?o
- [ ] `pnlCharts.Dock = DockStyle.Fill` ho?c có Size/Location rõ ràng
- [ ] `this.Controls.Add(pnlCharts)` ???c g?i
- [ ] `pnlCharts.BackColor` khác màu Form ?? debug

### 3. Charts
- [ ] Chart ???c t?o v?i Size > 0
- [ ] Chart có ChartArea
- [ ] Chart có Series
- [ ] `pnlCharts.Controls.Add(chart)` ???c g?i
- [ ] Chart.Location không âm, không v??t Panel

### 4. Data
- [ ] `Series.Points.AddXY()` ???c g?i
- [ ] Data không r?ng (có ít nh?t 1 point)
- [ ] Không có exception khi query database

### 5. Z-Order
- [ ] Panel charts KHÔNG b? che b?i control khác
- [ ] `pnlCharts.BringToFront()` n?u c?n

## ?? DEBUG TECHNIQUES

### Technique 1: Debug Panel
```csharp
pnlCharts.BackColor = Color.Red;  // Xem panel có hi?n không
MessageBox.Show($"Panel size: {pnlCharts.Size}");  // Check size
MessageBox.Show($"Charts count: {pnlCharts.Controls.Count}");  // Check charts
```

### Technique 2: Debug Chart
```csharp
// V? border ?? th?y chart
chart.BorderlineColor = Color.White;
chart.BorderlineWidth = 2;
chart.BorderlineDashStyle = ChartDashStyle.Solid;

// Thêm Title ?? check
chart.Titles.Add(new Title {
    Text = "TEST CHART",
    ForeColor = Color.White,
    Font = new Font("Arial", 20F)
});
```

### Technique 3: Debug Data
```csharp
// Log data points
foreach (var point in chart.Series[0].Points)
{
    MessageBox.Show($"X: {point.XValue}, Y: {point.YValues[0]}");
}
```

## ?? M?U CODE HOÀN CH?NH

```csharp
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Forms
{
    public class ReportsForm : Form  // ? KHÔNG partial n?u không có Designer
    {
        private ToDoListContext _context;
        private Panel pnlCharts;
        private Chart chartStatus;

        public ReportsForm(ToDoListContext context)
        {
            _context = context;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1400, 900);
            this.Text = "?? Báo cáo & Th?ng kê";
            this.BackColor = Color.FromArgb(30, 30, 30);

            // ===== CHARTS PANEL =====
            pnlCharts = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(20)
            };

            // ===== CREATE CHART =====
            chartStatus = new Chart
            {
                Location = new Point(20, 20),
                Size = new Size(420, 350),
                BackColor = Color.FromArgb(40, 40, 40)
            };

            // ChartArea
            ChartArea chartArea = new ChartArea
            {
                BackColor = Color.FromArgb(40, 40, 40)
            };
            chartStatus.ChartAreas.Add(chartArea);

            // Series
            Series series = new Series
            {
                Name = "Status",
                ChartType = SeriesChartType.Doughnut
            };
            chartStatus.Series.Add(series);

            // Title
            Title title = new Title
            {
                Text = "Tr?ng thái Task",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold)
            };
            chartStatus.Titles.Add(title);

            // Legend
            Legend legend = new Legend
            {
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            chartStatus.Legends.Add(legend);

            // ? ADD CHART TO PANEL
            pnlCharts.Controls.Add(chartStatus);

            // ? ADD PANEL TO FORM
            this.Controls.Add(pnlCharts);
        }

        private async void LoadData()
        {
            try
            {
                var completed = await _context.Tasks.CountAsync(t => t.Status == "Completed");
                var inProgress = await _context.Tasks.CountAsync(t => t.Status == "In Progress");
                var pending = await _context.Tasks.CountAsync(t => t.Status == "Pending");

                chartStatus.Series[0].Points.Clear();
                chartStatus.Series[0].Points.AddXY("Completed", completed);
                chartStatus.Series[0].Points.AddXY("In Progress", inProgress);
                chartStatus.Series[0].Points.AddXY("Pending", pending);

                chartStatus.Series[0].Points[0].Color = Color.Green;
                chartStatus.Series[0].Points[1].Color = Color.Orange;
                chartStatus.Series[0].Points[2].Color = Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i: {ex.Message}");
            }
        }
    }
}
```

## ?? NEXT STEPS

1. **Xác ??nh c?u trúc file** c?a b?n (có Designer.cs không?)
2. **Xóa code trùng l?p** n?u có
3. **Ki?m tra t?ng b??c** theo checklist
4. **Debug b?ng màu s?c** ?? xem control nào hi?n th?
5. **Test v?i d? li?u m?u** tr??c khi query database

## ?? SUPPORT

N?u v?n l?i, cung c?p:
- Structure c?a folder `Forms/` (có file nào liên quan ??n ReportsForm)
- Error messages (n?u có)
- Screenshot rõ h?n v? v? trí màn hình ?en

---

**?? Sau khi fix, b?n s? th?y bi?u ?? ??p nh? trong HUONG_DAN_BAO_CAO_THONG_KE.md!**
