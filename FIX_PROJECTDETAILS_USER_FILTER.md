# ? FIX: PROJECTDETAILSFORM - FILTER TASKS THEO USER

## ? **V?N ??**

**Hi?n t??ng:** Khi xem chi ti?t project, t?t c? users ??u th?y cùng m?t danh sách tasks (c?a user ??u tiên).

**Nguyên nhân:** `ProjectDetailsForm` ?ang dùng `FirstOrDefaultAsync()` ?? l?y user thay vì l?y user hi?n t?i t? session:

```csharp
// ? SAI trong ProjectDetailsForm.cs (line 63-75)
var user = await _context.Users.FirstOrDefaultAsync(); // L?y user ??u tiên!
_userId = user.UserId;

var tasks = await _context.Tasks
    .Where(t => t.ProjectId == _project.ProjectId && t.IsDeleted != true) // Không filter UserId!
    .ToListAsync();
```

**K?t qu?:**
- User A login ? Xem project ? Th?y tasks c?a user ??u tiên ?
- User B login ? Xem project ? V?N th?y tasks c?a user ??u tiên ?
- User C login ? Xem project ? V?N th?y tasks c?a user ??u tiên ?

---

## ? **GI?I PHÁP**

### **B??c 1: Thêm `using ToDoList.GUI.Helpers;`**

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using TaskModel = TodoListApp.DAL.Models.Task;
using Microsoft.EntityFrameworkCore;
using ToDoList.GUI.Helpers; // ? THÊM DÒNG NÀY
```

### **B??c 2: S?a `LoadTasks()` Method**

#### **Before (SAI):**
```csharp
private async void LoadTasks()
{
    try
    {
        if (_context == null) return;

        // ? L?y user ??u tiên (SAI!)
        var user = await _context.Users.FirstOrDefaultAsync();
        if (user == null)
        {
            user = new User
            {
                FullName = "Default User",
                Email = "user@example.com",
                PasswordHash = "default",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        _userId = user.UserId; // ? Luôn là user ??u tiên!

        // ? Không filter theo UserId!
        var tasks = await _context.Tasks
            .Where(t => t.ProjectId == _project.ProjectId && t.IsDeleted != true)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();

        pnlTasksContainer.Controls.Clear();

        foreach (var task in tasks)
        {
            AddTaskCard(task);
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"L?i khi t?i danh sách công vi?c: {ex.Message}", "L?i",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

#### **After (?ÚNG):**
```csharp
private async void LoadTasks()
{
    try
    {
        if (_context == null) return;

        // ? L?y UserId t? UserSession (?ÚNG!)
        _userId = UserSession.GetUserId();
        
        if (_userId == 0)
        {
            MessageBox.Show("L?i: Không tìm th?y thông tin ng??i dùng. Vui lòng ??ng nh?p l?i.",
                "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
            return;
        }

        // ? Filter theo CHU?N: ProjectId VÀ UserId!
        var tasks = await _context.Tasks
            .Where(t => t.ProjectId == _project.ProjectId && t.UserId == _userId && t.IsDeleted != true)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();

        pnlTasksContainer.Controls.Clear();

        foreach (var task in tasks)
        {
            AddTaskCard(task);
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"L?i khi t?i danh sách công vi?c: {ex.Message}", "L?i",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

---

## ?? **SO SÁNH TR??C/SAU**

### **Tr??c Khi Fix:**

| User Login | Project | UserId ???c Load | Tasks Hi?n Th? |
|------------|---------|------------------|----------------|
| User A (ID: 1) | Website Redesign | 1 | Tasks c?a user ??u tiên ? |
| User B (ID: 2) | Website Redesign | **1** ? | Tasks c?a user ??u tiên ? |
| User C (ID: 3) | Website Redesign | **1** ? | Tasks c?a user ??u tiên ? |

**V?n ??:** M?i user ??u th?y tasks c?a user ID = 1!

### **Sau Khi Fix:**

| User Login | Project | UserId ???c Load | Tasks Hi?n Th? |
|------------|---------|------------------|----------------|
| User A (ID: 1) | Website Redesign | 1 | Tasks c?a User A ? |
| User B (ID: 2) | Marketing Campaign | 2 | Tasks c?a User B ? |
| User C (ID: 3) | Database Optimization | 3 | Tasks c?a User C ? |

**Gi?i pháp:** M?i user CH? th?y tasks **c?a chính mình** trong project!

---

## ?? **D? Li?u Th?c T?**

### **Project: Website Redesign (ProjectId: 1)**

| Task | UserId | User Name |
|------|--------|-----------|
| Thi?t k? mockup trang ch? | 1 | Nguy?n V?n An |
| Phát tri?n giao di?n responsive | 1 | Nguy?n V?n An |
| Tích h?p CMS | 1 | Nguy?n V?n An |

**Before Fix:**
- User A login ? Th?y 3 tasks ?
- User B login ? V?N th?y 3 tasks c?a User A ?
- User C login ? V?N th?y 3 tasks c?a User A ?

**After Fix:**
- User A login ? Th?y 3 tasks ?
- User B login ? Th?y 0 tasks (vì project này không có tasks c?a User B) ?
- User C login ? Th?y 0 tasks (vì project này không có tasks c?a User C) ?

---

## ?? **Cách Ki?m Tra**

### **Test 1: User A (Nguy?n V?n An)**
```
1. Login: nguyenvanan@example.com / password123
2. M? project "Website Redesign"
3. Ki?m tra tasks hi?n th?:
   ? Ph?i th?y: 3 tasks
   - Thi?t k? mockup trang ch?
   - Phát tri?n giao di?n responsive
   - Tích h?p CMS
```

### **Test 2: User B (Tr?n Th? Bình)**
```
1. Logout User A
2. Login: tranthibinh@example.com / password123
3. M? project "Marketing Campaign Q4"
4. Ki?m tra tasks hi?n th?:
   ? Ph?i th?y: 3 tasks
   - L?p k? ho?ch content
   - Ch?y Facebook Ads
   - Phân tích k?t qu? campaign
   
5. M? project "Website Redesign" (c?a User A)
   ? Ph?i th?y: 0 tasks (vì không có tasks c?a User B trong project này)
```

### **Test 3: Task Report**
```
1. Login: leminhcuong@example.com / password123
2. M? project "Database Optimization"
3. Click vào task "Phân tích slow queries"
4. Ki?m tra TaskReportForm:
   ? Task Title: "Phân tích slow queries"
   ? UserId: 3 (Lê Minh C??ng)
   ? Thông tin ?úng v?i task c?a User C
```

---

## ?? **Lý Do V?n ?? X?y Ra**

### **1. FirstOrDefaultAsync() Luôn L?y User ??u Tiên**

```csharp
// Query này không có ORDER BY, nên l?y b?n ghi "??u tiên" trong b?ng
var user = await _context.Users.FirstOrDefaultAsync();

// Trong database:
// UserId = 1: Nguy?n V?n An (???c insert ??u tiên)
// UserId = 2: Tr?n Th? Bình
// UserId = 3: Lê Minh C??ng

// ? FirstOrDefaultAsync() luôn tr? v? UserId = 1!
```

### **2. Không Filter Tasks Theo UserId**

```csharp
// ? Query này ch? filter ProjectId, không filter UserId
var tasks = await _context.Tasks
    .Where(t => t.ProjectId == _project.ProjectId && t.IsDeleted != true)
    .ToListAsync();

// K?t qu?: L?y T?T C? tasks c?a project, b?t k? UserId
```

**Ví d?:**
- Project "Website Redesign" có:
  - 3 tasks c?a User A (UserId = 1)
  - 2 tasks c?a User D (UserId = 4) ? N?u h? c?ng tác

**Without Filter:** L?y 5 tasks (t?t c?)  
**With Filter:** L?y 3 tasks (ch? c?a User A) ?

---

## ? **K?t Qu? Sau Fix**

### **1. UserSession.GetUserId() Ho?t ??ng ?úng**

```csharp
// Khi User A login:
UserSession.SetUser(1, "Nguy?n V?n An", "nguyenvanan@example.com");

// Trong ProjectDetailsForm:
_userId = UserSession.GetUserId(); // ? 1 ?

// Khi User B login:
UserSession.SetUser(2, "Tr?n Th? Bình", "tranthibinh@example.com");

// Trong ProjectDetailsForm:
_userId = UserSession.GetUserId(); // ? 2 ?
```

### **2. Filter Tasks Chính Xác**

```csharp
// ? Query v?i 3 ?i?u ki?n
var tasks = await _context.Tasks
    .Where(t => 
        t.ProjectId == _project.ProjectId &&  // ?i?u ki?n 1: ?úng project
        t.UserId == _userId &&                 // ?i?u ki?n 2: ?úng user
        t.IsDeleted != true                    // ?i?u ki?n 3: Không b? xóa
    )
    .ToListAsync();
```

**K?t qu?:**
- ? User A CH? th?y tasks c?a User A
- ? User B CH? th?y tasks c?a User B
- ? User C CH? th?y tasks c?a User C
- ? TaskReportForm hi?n th? ?úng thông tin task

---

## ?? **L?u Ý Quan Tr?ng**

### **1. Shared Projects (T??ng Lai)**

N?u b?n mu?n nhi?u users cùng làm vi?c trên 1 project:

**Option A:** Hi?n T?T C? tasks trong project
```csharp
var tasks = await _context.Tasks
    .Where(t => t.ProjectId == _project.ProjectId && t.IsDeleted != true)
    // Không filter UserId
    .ToListAsync();
```

**Option B:** CH? hi?n tasks c?a user hi?n t?i
```csharp
var tasks = await _context.Tasks
    .Where(t => t.ProjectId == _project.ProjectId && t.UserId == _userId && t.IsDeleted != true)
    // Filter UserId ? ?ang dùng cái này
    .ToListAsync();
```

**Khuy?n ngh?:** Dùng Option B cho ??n khi có tính n?ng "Chia s? project"

### **2. TaskReportForm C?n UserId ?úng**

```csharp
// Khi m? TaskReportForm, truy?n ?úng UserId
private void ShowTaskReport(TaskModel task)
{
    try
    {
        using (var reportForm = new TaskReportForm(_context, task, _userId))
        {
            reportForm.ShowDialog();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"L?i khi m? báo cáo: {ex.Message}", "L?i",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

**Quan tr?ng:** `_userId` ph?i là UserId c?a user hi?n t?i, không ph?i user ??u tiên!

### **3. Check UserId == 0**

```csharp
_userId = UserSession.GetUserId();

if (_userId == 0)
{
    MessageBox.Show("L?i: Không tìm th?y thông tin ng??i dùng. Vui lòng ??ng nh?p l?i.",
        "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
    this.Close(); // ?óng form n?u không có user
    return;
}
```

**Lý do:** N?u session b? m?t ho?c user ch?a login, `GetUserId()` tr? v? 0. C?n thông báo và ?óng form.

---

## ?? **Files ?ã S?a**

### ? **ToDoList.GUI/Forms/ProjectDetailsForm.cs**

**Changes:**
1. **Line 11:** Thêm `using ToDoList.GUI.Helpers;`
2. **Line 53-89:** S?a method `LoadTasks()`
   - ??i t? `FirstOrDefaultAsync()` ? `UserSession.GetUserId()`
   - Thêm check `if (_userId == 0)`
   - Thêm filter `.Where(t => ... && t.UserId == _userId ...)`

---

## ?? **K?t Qu?**

Bây gi?:
- ? M?i user CH? th?y tasks **c?a chính mình** trong project
- ? TaskReportForm hi?n th? ?úng thông tin task c?a user hi?n t?i
- ? Không còn tình tr?ng "tasks c?a t?t c? users"
- ? D? li?u ???c phân quy?n chính xác

**Workspace c?a m?i user hoàn toàn ??c l?p!** ??

---

## ?? **Tài Li?u Liên Quan**

- `FIX_USER_SPECIFIC_TASKS.md` - Fix t??ng t? cho Form1.cs
- `TaskReportForm.cs` - Form báo cáo task
- `UserSession.cs` - Class qu?n lý session user

---

**?? ?Ã FIX XONG! M?I USER CH? TH?Y TASKS C?A MÌNH!** ??
