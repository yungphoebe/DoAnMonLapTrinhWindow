# ?? H??NG D?N S? D?NG FORM TH?NG K� N�NG CAO

## T?ng quan

Form **AdvancedReportsForm** ???c thi?t k? theo m?u giao di?n trong ?nh v?i 4 box th?ng k� ch�nh v� c�c ph?n ph�n t�ch chi ti?t.

## C?u tr�c Form

### 1. Header (Ph?n ??u)
- Ti�u ??: "?? Th?ng k� n�ng cao - ToDoList Analytics"
- N�t ?�ng (?) ? g�c ph?i tr�n

### 2. Tab Control (4 tabs)

#### Tab 1: T?NG QUAN
**4 Box Th?ng K� Ch�nh:**

1. **?? T?ng Projects** (M�u xanh d??ng)
   - Hi?n th?: T?ng s? projects
   - D? li?u: ??m t? b?ng Projects (IsArchived != true)

2. **?? T?ng Tasks** (M�u xanh l�)
   - Hi?n th?: T?ng s? tasks
   - D? li?u: ??m t? b?ng Tasks (IsDeleted != true)

3. **? Ho�n th�nh** (M�u t�m)
   - Hi?n th?: T? l? ho�n th�nh (%)
   - C�ng th?c: (S? tasks ho�n th�nh / T?ng tasks) � 100

4. **?? Th?i gian** (M�u v�ng)
   - Hi?n th?: T?ng gi? l�m vi?c
   - D? li?u: T?ng ActualMinutes / 60

**Ph?n Ho?t ??ng g?n ?�y:**
- Hi?n th? 8 tasks g?n ?�y nh?t
- Th�ng tin: Icon tr?ng th�i, T�n task, T�n project, Th?i gian
- S?p x?p: Theo UpdatedAt ho?c CreatedAt (gi?m d?n)

**Ph?n Ti?n ?? theo d? �n:**
- Hi?n th? 6 projects
- M?i project c�:
  - T�n project
  - Progress bar m�u (?? ? v�ng ? xanh)
  - T? l? ph?n tr?m
  - S? l??ng tasks (completed/total)

#### Tab 2: N?NG SU?T
- ?ang ph�t tri?n
- S? c�: Bi?u ?? n?ng su?t theo ng�y/tu?n/th�ng

#### Tab 3: PH�N T�CH D? �N
- ?ang ph�t tri?n
- S? c�: Ph�n t�ch chi ti?t t?ng d? �n

#### Tab 4: PH�N T�CH TH?I GIAN
- ?ang ph�t tri?n
- S? c�: Ph�n t�ch th?i gian l�m vi?c

## C�ch s? d?ng trong Code

### 1. M? form t? menu
```csharp
using (var advancedReportsForm = new Forms.AdvancedReportsForm(_context))
{
    advancedReportsForm.ShowDialog();
}
```

### 2. Truy?n database context
```csharp
// _context ph?i l� ToDoListContext ?� ???c kh?i t?o
var form = new AdvancedReportsForm(_context);
form.ShowDialog();
```

## T�nh n?ng

### ? ?� ho�n th�nh:
1. 4 box th?ng k� ch�nh v?i s? li?u th?c t? database
2. Ho?t ??ng g?n ?�y (8 tasks m?i nh?t)
3. Ti?n ?? theo d? �n (6 projects)
4. M�u s?c ??ng theo t? l? ho�n th�nh
5. Responsive design
6. Dark theme ??ng b?

### ?? ?ang ph�t tri?n:
1. Bi?u ?? n?ng su?t
2. Ph�n t�ch d? �n chi ti?t
3. Ph�n t�ch th?i gian
4. Export b�o c�o
5. L?c theo th?i gian

## M�u s?c Progress Bar

- **0-24%**: ?? ?? (231, 76, 60) - C?n ch� �
- **25-49%**: ?? Cam (230, 126, 34) - ?ang ti?n tri?n
- **50-74%**: ?? V�ng (241, 196, 15) - T?t
- **75-100%**: ?? Xanh (46, 204, 113) - Xu?t s?c

## Hi?u ?ng UI

### Hover Effects:
- Stat boxes: ??i t? m�u (35,35,35) ? (45,45,45)
- Cursor: Hand khi hover v�o c�c box

### Border:
- T?t c? stat boxes c� border m�u (50,50,50)

### Font:
- Title: Segoe UI, 14-16F, Bold
- Values: Segoe UI, 22F, Bold
- Subtitles: Segoe UI, 8F
- Activity items: Segoe UI, 9F

## K�ch th??c

### Form:
- Size: 1200 � 800
- MinimumSize: 1000 � 600
- Resizable: Yes

### Stat Boxes:
- Size: 270 � 130
- Spacing: 20px gi?a c�c boxes

### Activity Panel:
- Size: 540 � 400
- AutoScroll: Yes

### Progress Panel:
- Size: 540 � 400
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

## C�ch m? form

### T? Form1 (Main Dashboard):
1. Click v�o menu c?a project card
2. Ch?n "?? Th?ng k� n�ng cao"
3. Form s? m? d?ng Dialog

### T? code:
```csharp
private void BtnAdvancedStats_Click(object sender, EventArgs e)
{
    if (_context == null)
    {
        MessageBox.Show("Kh�ng th? m? th?ng k�.", "L?i");
        return;
    }
    
    using (var form = new Forms.AdvancedReportsForm(_context))
    {
        form.ShowDialog();
    }
}
```

## X? l� l?i

Form c� x? l� l?i cho:
1. Database connection errors
2. Query errors
3. UI update errors

T?t c? l?i ??u ???c log ra Debug console v� hi?n th? MessageBox n?u c?n thi?t.

## Performance

- S? d?ng async/await cho t?t c? database queries
- AutoScroll cho c�c panel d�i
- Ch? load 6-8 items cho m?i section ?? tr�nh lag

## T??ng th�ch

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

## L?u � k? thu?t

1. Form **KH�NG** dispose _context v� context ???c truy?n t? b�n ngo�i
2. S? d?ng `Find control by tag/name` ?? update dynamic content
3. Progress bar colors t�nh to�n ??ng
4. Time ago format: "now", "5m", "2h", "3d", "10/01"

## Troubleshooting

### L?i: "Cannot find AdvancedReportsForm"
- Ki?m tra namespace: `ToDoList.GUI.Forms`
- Rebuild solution

### L?i: "Context is null"
- ??m b?o truy?n _context khi kh?i t?o form
- Ki?m tra _context ?� ???c initialize ch?a

### Boxes kh�ng hi?n th? s? li?u
- Ki?m tra database connection
- Xem Debug console ?? t�m l?i query

### Progress bars kh�ng m�u
- Ki?m tra GetProgressColor() method
- Verify percentage calculation

## Contact

N?u c� v?n ??, ki?m tra:
1. Database connection string
2. Entity Framework migrations
3. Project references
4. NuGet packages

---

**Version**: 1.0
**Created**: 2024
**Last Updated**: 2024
