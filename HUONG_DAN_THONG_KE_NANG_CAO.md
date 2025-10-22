# ?? H??NG D?N S? D?NG FORM TH?NG KÊ NÂNG CAO

## T?ng quan

Form **AdvancedReportsForm** ???c thi?t k? theo m?u giao di?n trong ?nh v?i 4 box th?ng kê chính và các ph?n phân tích chi ti?t.

## C?u trúc Form

### 1. Header (Ph?n ??u)
- Tiêu ??: "?? Th?ng kê nâng cao - ToDoList Analytics"
- Nút ?óng (?) ? góc ph?i trên

### 2. Tab Control (4 tabs)

#### Tab 1: T?NG QUAN
**4 Box Th?ng Kê Chính:**

1. **?? T?ng Projects** (Màu xanh d??ng)
   - Hi?n th?: T?ng s? projects
   - D? li?u: ??m t? b?ng Projects (IsArchived != true)

2. **?? T?ng Tasks** (Màu xanh lá)
   - Hi?n th?: T?ng s? tasks
   - D? li?u: ??m t? b?ng Tasks (IsDeleted != true)

3. **? Hoàn thành** (Màu tím)
   - Hi?n th?: T? l? hoàn thành (%)
   - Công th?c: (S? tasks hoàn thành / T?ng tasks) × 100

4. **?? Th?i gian** (Màu vàng)
   - Hi?n th?: T?ng gi? làm vi?c
   - D? li?u: T?ng ActualMinutes / 60

**Ph?n Ho?t ??ng g?n ?ây:**
- Hi?n th? 8 tasks g?n ?ây nh?t
- Thông tin: Icon tr?ng thái, Tên task, Tên project, Th?i gian
- S?p x?p: Theo UpdatedAt ho?c CreatedAt (gi?m d?n)

**Ph?n Ti?n ?? theo d? án:**
- Hi?n th? 6 projects
- M?i project có:
  - Tên project
  - Progress bar màu (?? ? vàng ? xanh)
  - T? l? ph?n tr?m
  - S? l??ng tasks (completed/total)

#### Tab 2: N?NG SU?T
- ?ang phát tri?n
- S? có: Bi?u ?? n?ng su?t theo ngày/tu?n/tháng

#### Tab 3: PHÂN TÍCH D? ÁN
- ?ang phát tri?n
- S? có: Phân tích chi ti?t t?ng d? án

#### Tab 4: PHÂN TÍCH TH?I GIAN
- ?ang phát tri?n
- S? có: Phân tích th?i gian làm vi?c

## Cách s? d?ng trong Code

### 1. M? form t? menu
```csharp
using (var advancedReportsForm = new Forms.AdvancedReportsForm(_context))
{
    advancedReportsForm.ShowDialog();
}
```

### 2. Truy?n database context
```csharp
// _context ph?i là ToDoListContext ?ã ???c kh?i t?o
var form = new AdvancedReportsForm(_context);
form.ShowDialog();
```

## Tính n?ng

### ? ?ã hoàn thành:
1. 4 box th?ng kê chính v?i s? li?u th?c t? database
2. Ho?t ??ng g?n ?ây (8 tasks m?i nh?t)
3. Ti?n ?? theo d? án (6 projects)
4. Màu s?c ??ng theo t? l? hoàn thành
5. Responsive design
6. Dark theme ??ng b?

### ?? ?ang phát tri?n:
1. Bi?u ?? n?ng su?t
2. Phân tích d? án chi ti?t
3. Phân tích th?i gian
4. Export báo cáo
5. L?c theo th?i gian

## Màu s?c Progress Bar

- **0-24%**: ?? ?? (231, 76, 60) - C?n chú ý
- **25-49%**: ?? Cam (230, 126, 34) - ?ang ti?n tri?n
- **50-74%**: ?? Vàng (241, 196, 15) - T?t
- **75-100%**: ?? Xanh (46, 204, 113) - Xu?t s?c

## Hi?u ?ng UI

### Hover Effects:
- Stat boxes: ??i t? màu (35,35,35) ? (45,45,45)
- Cursor: Hand khi hover vào các box

### Border:
- T?t c? stat boxes có border màu (50,50,50)

### Font:
- Title: Segoe UI, 14-16F, Bold
- Values: Segoe UI, 22F, Bold
- Subtitles: Segoe UI, 8F
- Activity items: Segoe UI, 9F

## Kích th??c

### Form:
- Size: 1200 × 800
- MinimumSize: 1000 × 600
- Resizable: Yes

### Stat Boxes:
- Size: 270 × 130
- Spacing: 20px gi?a các boxes

### Activity Panel:
- Size: 540 × 400
- AutoScroll: Yes

### Progress Panel:
- Size: 540 × 400
- AutoScroll: Yes

## Database Queries

### Query 1: Total Projects
```csharp
var totalProjects = await _context.Projects.CountAsync(p => p.IsArchived != true);
```

### Query 2: Total Tasks
```csharp
var totalTasks = await _context.Tasks.CountAsync(t => t.IsDeleted != true);
```

### Query 3: Completed Tasks
```csharp
var completedTasks = await _context.Tasks.CountAsync(t => 
    t.IsDeleted != true && t.Status == "Completed");
```

### Query 4: Total Time
```csharp
var totalMinutes = await _context.Tasks
    .Where(t => t.IsDeleted != true && t.ActualMinutes.HasValue)
    .SumAsync(t => t.ActualMinutes.Value);
```

### Query 5: Recent Activity
```csharp
var recentTasks = await _context.Tasks
    .Where(t => t.IsDeleted != true)
    .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt)
    .Take(8)
    .Include(t => t.Project)
    .ToListAsync();
```

### Query 6: Project Progress
```csharp
var projects = await _context.Projects
    .Where(p => p.IsArchived != true)
    .Include(p => p.Tasks)
    .ToListAsync();
```

## Cách m? form

### T? Form1 (Main Dashboard):
1. Click vào menu c?a project card
2. Ch?n "?? Th?ng kê nâng cao"
3. Form s? m? d?ng Dialog

### T? code:
```csharp
private void BtnAdvancedStats_Click(object sender, EventArgs e)
{
    if (_context == null)
    {
        MessageBox.Show("Không th? m? th?ng kê.", "L?i");
        return;
    }
    
    using (var form = new Forms.AdvancedReportsForm(_context))
    {
        form.ShowDialog();
    }
}
```

## X? lý l?i

Form có x? lý l?i cho:
1. Database connection errors
2. Query errors
3. UI update errors

T?t c? l?i ??u ???c log ra Debug console và hi?n th? MessageBox n?u c?n thi?t.

## Performance

- S? d?ng async/await cho t?t c? database queries
- AutoScroll cho các panel dài
- Ch? load 6-8 items cho m?i section ?? tránh lag

## T??ng thích

- .NET 9
- Entity Framework Core
- Windows Forms
- SQL Server

## Future Enhancements

1. **Export to PDF/Excel**
2. **Date range filter**
3. **Real-time updates**
4. **More chart types**
5. **Custom color themes**
6. **Print support**
7. **Compare periods**
8. **Goal tracking**

## L?u ý k? thu?t

1. Form **KHÔNG** dispose _context vì context ???c truy?n t? bên ngoài
2. S? d?ng `Find control by tag/name` ?? update dynamic content
3. Progress bar colors tính toán ??ng
4. Time ago format: "now", "5m", "2h", "3d", "10/01"

## Troubleshooting

### L?i: "Cannot find AdvancedReportsForm"
- Ki?m tra namespace: `ToDoList.GUI.Forms`
- Rebuild solution

### L?i: "Context is null"
- ??m b?o truy?n _context khi kh?i t?o form
- Ki?m tra _context ?ã ???c initialize ch?a

### Boxes không hi?n th? s? li?u
- Ki?m tra database connection
- Xem Debug console ?? tìm l?i query

### Progress bars không màu
- Ki?m tra GetProgressColor() method
- Verify percentage calculation

## Contact

N?u có v?n ??, ki?m tra:
1. Database connection string
2. Entity Framework migrations
3. Project references
4. NuGet packages

---

**Version**: 1.0
**Created**: 2024
**Last Updated**: 2024
