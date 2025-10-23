# ?? FIX L?I BI?U ?? KH�NG HI?N TH? - REPORTSFORM

## ? V?N ??

D?a v�o h�nh ?nh b?n cung c?p, ReportsForm hi?n th?:
- ? **Stat cards** ph�a tr�n (OK)
- ? **M�n h�nh ?en** ph�a d??i (bi?u ?? kh�ng hi?n th?)

## ?? NGUY�N NH�N TH??NG G?P

### 1. Charts ch?a ???c add v�o Panel
```csharp
// ? SAI: Ch? t?o chart nh?ng kh�ng add
Chart chart = new Chart();
// ... configure chart ...
// Qu�n: pnlCharts.Controls.Add(chart);
```

### 2. Chart size = 0 ho?c location sai
```csharp
// ? SAI: Chart b? ?n do size nh?
Chart chart = new Chart
{
    Size = new Size(0, 0)  // ? L?I: Size = 0
};
```

### 3. Panel kh�ng ???c add v�o Form
```csharp
// ? SAI: T?o panel nh?ng kh�ng add v�o form
Panel pnlCharts = new Panel();
// Qu�n: this.Controls.Add(pnlCharts);
```

### 4. Chart b? che b?i control kh�c
```csharp
// ? SAI: Control sau add s? ?� l�n
this.Controls.Add(pnlCharts);    // Charts ? d??i
this.Controls.Add(pnlHeader);    // Header ?� l�n charts
// ? ?�NG: D�ng BringToFront() ho?c ??i th? t?
```

### 5. Data ch?a ???c load
```csharp
// ? SAI: Chart kh�ng c� data
chart.Series[0].Points.Clear();
// Qu�n: chart.Series[0].Points.AddXY(...);
```

## ? GI?I PH�P

### B??c 1: Ki?m tra c?u tr�c file

```
ToDoList.GUI/Forms/
??? ReportsForm.cs           ? Logic + InitializeComponent()
??? ReportsForm.Designer.cs  ? Auto-generated (n?u c�)
??? ReportsForm.resx         ? Resources (n?u c�)
```

**QUAN TR?NG**: 
- N?u c� `.Designer.cs`, KH�NG ???c ??nh ngh?a l?i c�c member trong `.cs`
- N?u KH�NG c� `.Designer.cs`, t?t c? code trong `.cs`

### B??c 2: Fix l?i tr�ng member

#### Tr??ng h?p A: C� Designer.cs
```csharp
// ReportsForm.cs
public partial class ReportsForm : Form
{
    // ? KH�NG khai b�o l?i c�c member ?� c� trong Designer.cs
    // private Panel pnlCharts;  ? X�A d�ng n�y
    
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

#### Tr??ng h?p B: KH�NG c� Designer.cs
```csharp
// ReportsForm.cs
public class ReportsForm : Form  // ? KH�NG c� "partial"
{
    // ? Khai b�o T?T C? members ? ?�y
    private ToDoListContext _context;
    private Panel pnlCharts;
    private Chart chartStatus;
    
    public ReportsForm(ToDoListContext context)
    {
        _context = context;
        InitializeComponent();  // ? T? vi?t trong file n�y
        LoadData();
    }
    
    private void InitializeComponent()
    {
        // T?o T?T C? controls ? ?�y
    }
}
```

### B??c 3: ??m b?o charts ???c add ?�ng

```csharp
private void CreateCharts()
{
    // 1. T?O CHART
    Chart chartStatus = new Chart
    {
        Location = new Point(20, 20),
        Size = new Size(420, 350),  // ? Size r� r�ng
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
    
    // 4. ? QUAN TR?NG: ADD V�O PANEL
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

Khi debug, ki?m tra c�c ?i?m sau:

### 1. Form Load
- [ ] `InitializeComponent()` ???c g?i trong constructor
- [ ] `LoadData()` ???c g?i sau `InitializeComponent()`

### 2. Panel Charts
- [ ] `pnlCharts` ???c t?o
- [ ] `pnlCharts.Dock = DockStyle.Fill` ho?c c� Size/Location r� r�ng
- [ ] `this.Controls.Add(pnlCharts)` ???c g?i
- [ ] `pnlCharts.BackColor` kh�c m�u Form ?? debug

### 3. Charts
- [ ] Chart ???c t?o v?i Size > 0
- [ ] Chart c� ChartArea
- [ ] Chart c� Series
- [ ] `pnlCharts.Controls.Add(chart)` ???c g?i
- [ ] Chart.Location kh�ng �m, kh�ng v??t Panel

### 4. Data
- [ ] `Series.Points.AddXY()` ???c g?i
- [ ] Data kh�ng r?ng (c� �t nh?t 1 point)
- [ ] Kh�ng c� exception khi query database

### 5. Z-Order
- [ ] Panel charts KH�NG b? che b?i control kh�c
- [ ] `pnlCharts.BringToFront()` n?u c?n

## ?? DEBUG TECHNIQUES

### Technique 1: Debug Panel
```csharp
pnlCharts.BackColor = Color.Red;  // Xem panel c� hi?n kh�ng
MessageBox.Show($"Panel size: {pnlCharts.Size}");  // Check size
MessageBox.Show($"Charts count: {pnlCharts.Controls.Count}");  // Check charts
```

### Technique 2: Debug Chart
```csharp
// V? border ?? th?y chart
chart.BorderlineColor = Color.White;
chart.BorderlineWidth = 2;
chart.BorderlineDashStyle = ChartDashStyle.Solid;

// Th�m Title ?? check
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

## ?? M?U CODE HO�N CH?NH

```csharp
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI.Forms
{
    public class ReportsForm : Form  // ? KH�NG partial n?u kh�ng c� Designer
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
            this.Text = "?? B�o c�o & Th?ng k�";
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
                Text = "Tr?ng th�i Task",
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

1. **X�c ??nh c?u tr�c file** c?a b?n (c� Designer.cs kh�ng?)
2. **X�a code tr�ng l?p** n?u c�
3. **Ki?m tra t?ng b??c** theo checklist
4. **Debug b?ng m�u s?c** ?? xem control n�o hi?n th?
5. **Test v?i d? li?u m?u** tr??c khi query database

## ?? SUPPORT

N?u v?n l?i, cung c?p:
- Structure c?a folder `Forms/` (c� file n�o li�n quan ??n ReportsForm)
- Error messages (n?u c�)
- Screenshot r� h?n v? v? tr� m�n h�nh ?en

---

**?? Sau khi fix, b?n s? th?y bi?u ?? ??p nh? trong HUONG_DAN_BAO_CAO_THONG_KE.md!**
