# ? FIX: PROJECTDETAILSFORM - FILTER TASKS THEO USER

## ? **V?N ??**

**Hi?n t??ng:** Khi xem chi ti?t project, t?t c? users ??u th?y c�ng m?t danh s�ch tasks (c?a user ??u ti�n).

**Nguy�n nh�n:** `ProjectDetailsForm` ?ang d�ng `FirstOrDefaultAsync()` ?? l?y user thay v� l?y user hi?n t?i t? session:

```csharp
// ? SAI trong ProjectDetailsForm.cs (line 63-75)
var user = await _context.Users.FirstOrDefaultAsync(); // L?y user ??u ti�n!
_userId = user.UserId;

var tasks = await _context.Tasks
    .Where(t => t.ProjectId == _project.ProjectId && t.IsDeleted != true) // Kh�ng filter UserId!
    .ToListAsync();
```

**K?t qu?:**
- User A login ? Xem project ? Th?y tasks c?a user ??u ti�n ?
- User B login ? Xem project ? V?N th?y tasks c?a user ??u ti�n ?
- User C login ? Xem project ? V?N th?y tasks c?a user ??u ti�n ?

---

## ? **GI?I PH�P**

### **B??c 1: Th�m `using ToDoList.GUI.Helpers;`**

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
using ToDoList.GUI.Helpers; // ? TH�M D�NG N�Y
```

### **B??c 2: S?a `LoadTasks()` Method**

#### **Before (SAI):**
```csharp
private async void LoadTasks()
{
    try
    {
        if (_context == null) return;

        // ? L?y user ??u ti�n (SAI!)
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
        _userId = user.UserId; // ? Lu�n l� user ??u ti�n!

        // ? Kh�ng filter theo UserId!
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
        MessageBox.Show($"L?i khi t?i danh s�ch c�ng vi?c: {ex.Message}", "L?i",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

#### **After (?�NG):**
```csharp
private async void LoadTasks()
{
    try
    {
        if (_context == null) return;

        // ? L?y UserId t? UserSession (?�NG!)
        _userId = UserSession.GetUserId();
        
        if (_userId == 0)
        {
            MessageBox.Show("L?i: Kh�ng t�m th?y th�ng tin ng??i d�ng. Vui l�ng ??ng nh?p l?i.",
                "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
            return;
        }

        // ? Filter theo CHU?N: ProjectId V� UserId!
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
        MessageBox.Show($"L?i khi t?i danh s�ch c�ng vi?c: {ex.Message}", "L?i",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

---

## ?? **SO S�NH TR??C/SAU**

### **Tr??c Khi Fix:**

| User Login | Project | UserId ???c Load | Tasks Hi?n Th? |
|------------|---------|------------------|----------------|
| User A (ID: 1) | Website Redesign | 1 | Tasks c?a user ??u ti�n ? |
| User B (ID: 2) | Website Redesign | **1** ? | Tasks c?a user ??u ti�n ? |
| User C (ID: 3) | Website Redesign | **1** ? | Tasks c?a user ??u ti�n ? |

**V?n ??:** M?i user ??u th?y tasks c?a user ID = 1!

### **Sau Khi Fix:**

| User Login | Project | UserId ???c Load | Tasks Hi?n Th? |
|------------|---------|------------------|----------------|
| User A (ID: 1) | Website Redesign | 1 | Tasks c?a User A ? |
| User B (ID: 2) | Marketing Campaign | 2 | Tasks c?a User B ? |
| User C (ID: 3) | Database Optimization | 3 | Tasks c?a User C ? |

**Gi?i ph�p:** M?i user CH? th?y tasks **c?a ch�nh m�nh** trong project!

---

## ?? **D? Li?u Th?c T?**

### **Project: Website Redesign (ProjectId: 1)**

| Task | UserId | User Name |
|------|--------|-----------|
| Thi?t k? mockup trang ch? | 1 | Nguy?n V?n An |
| Ph�t tri?n giao di?n responsive | 1 | Nguy?n V?n An |
| T�ch h?p CMS | 1 | Nguy?n V?n An |

**Before Fix:**
- User A login ? Th?y 3 tasks ?
- User B login ? V?N th?y 3 tasks c?a User A ?
- User C login ? V?N th?y 3 tasks c?a User A ?

**After Fix:**
- User A login ? Th?y 3 tasks ?
- User B login ? Th?y 0 tasks (v� project n�y kh�ng c� tasks c?a User B) ?
- User C login ? Th?y 0 tasks (v� project n�y kh�ng c� tasks c?a User C) ?

---

## ?? **C�ch Ki?m Tra**

### **Test 1: User A (Nguy?n V?n An)**
```
1. Login: nguyenvanan@example.com / password123
2. M? project "Website Redesign"
3. Ki?m tra tasks hi?n th?:
   ? Ph?i th?y: 3 tasks
   - Thi?t k? mockup trang ch?
   - Ph�t tri?n giao di?n responsive
   - T�ch h?p CMS
```

### **Test 2: User B (Tr?n Th? B�nh)**
```
1. Logout User A
2. Login: tranthibinh@example.com / password123
3. M? project "Marketing Campaign Q4"
4. Ki?m tra tasks hi?n th?:
   ? Ph?i th?y: 3 tasks
   - L?p k? ho?ch content
   - Ch?y Facebook Ads
   - Ph�n t�ch k?t qu? campaign
   
5. M? project "Website Redesign" (c?a User A)
   ? Ph?i th?y: 0 tasks (v� kh�ng c� tasks c?a User B trong project n�y)
```

### **Test 3: Task Report**
```
1. Login: leminhcuong@example.com / password123
2. M? project "Database Optimization"
3. Click v�o task "Ph�n t�ch slow queries"
4. Ki?m tra TaskReportForm:
   ? Task Title: "Ph�n t�ch slow queries"
   ? UserId: 3 (L� Minh C??ng)
   ? Th�ng tin ?�ng v?i task c?a User C
```

---

## ?? **L� Do V?n ?? X?y Ra**

### **1. FirstOrDefaultAsync() Lu�n L?y User ??u Ti�n**

```csharp
// Query n�y kh�ng c� ORDER BY, n�n l?y b?n ghi "??u ti�n" trong b?ng
var user = await _context.Users.FirstOrDefaultAsync();

// Trong database:
// UserId = 1: Nguy?n V?n An (???c insert ??u ti�n)
// UserId = 2: Tr?n Th? B�nh
// UserId = 3: L� Minh C??ng

// ? FirstOrDefaultAsync() lu�n tr? v? UserId = 1!
```

### **2. Kh�ng Filter Tasks Theo UserId**

```csharp
// ? Query n�y ch? filter ProjectId, kh�ng filter UserId
var tasks = await _context.Tasks
    .Where(t => t.ProjectId == _project.ProjectId && t.IsDeleted != true)
    .ToListAsync();

// K?t qu?: L?y T?T C? tasks c?a project, b?t k? UserId
```

**V� d?:**
- Project "Website Redesign" c�:
  - 3 tasks c?a User A (UserId = 1)
  - 2 tasks c?a User D (UserId = 4) ? N?u h? c?ng t�c

**Without Filter:** L?y 5 tasks (t?t c?)  
**With Filter:** L?y 3 tasks (ch? c?a User A) ?

---

## ? **K?t Qu? Sau Fix**

### **1. UserSession.GetUserId() Ho?t ??ng ?�ng**

```csharp
// Khi User A login:
UserSession.SetUser(1, "Nguy?n V?n An", "nguyenvanan@example.com");

// Trong ProjectDetailsForm:
_userId = UserSession.GetUserId(); // ? 1 ?

// Khi User B login:
UserSession.SetUser(2, "Tr?n Th? B�nh", "tranthibinh@example.com");

// Trong ProjectDetailsForm:
_userId = UserSession.GetUserId(); // ? 2 ?
```

### **2. Filter Tasks Ch�nh X�c**

```csharp
// ? Query v?i 3 ?i?u ki?n
var tasks = await _context.Tasks
    .Where(t => 
        t.ProjectId == _project.ProjectId &&  // ?i?u ki?n 1: ?�ng project
        t.UserId == _userId &&                 // ?i?u ki?n 2: ?�ng user
        t.IsDeleted != true                    // ?i?u ki?n 3: Kh�ng b? x�a
    )
    .ToListAsync();
```

**K?t qu?:**
- ? User A CH? th?y tasks c?a User A
- ? User B CH? th?y tasks c?a User B
- ? User C CH? th?y tasks c?a User C
- ? TaskReportForm hi?n th? ?�ng th�ng tin task

---

## ?? **L?u � Quan Tr?ng**

### **1. Shared Projects (T??ng Lai)**

N?u b?n mu?n nhi?u users c�ng l�m vi?c tr�n 1 project:

**Option A:** Hi?n T?T C? tasks trong project
```csharp
var tasks = await _context.Tasks
    .Where(t => t.ProjectId == _project.ProjectId && t.IsDeleted != true)
    // Kh�ng filter UserId
    .ToListAsync();
```

**Option B:** CH? hi?n tasks c?a user hi?n t?i
```csharp
var tasks = await _context.Tasks
    .Where(t => t.ProjectId == _project.ProjectId && t.UserId == _userId && t.IsDeleted != true)
    // Filter UserId ? ?ang d�ng c�i n�y
    .ToListAsync();
```

**Khuy?n ngh?:** D�ng Option B cho ??n khi c� t�nh n?ng "Chia s? project"

### **2. TaskReportForm C?n UserId ?�ng**

```csharp
// Khi m? TaskReportForm, truy?n ?�ng UserId
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
        MessageBox.Show($"L?i khi m? b�o c�o: {ex.Message}", "L?i",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

**Quan tr?ng:** `_userId` ph?i l� UserId c?a user hi?n t?i, kh�ng ph?i user ??u ti�n!

### **3. Check UserId == 0**

```csharp
_userId = UserSession.GetUserId();

if (_userId == 0)
{
    MessageBox.Show("L?i: Kh�ng t�m th?y th�ng tin ng??i d�ng. Vui l�ng ??ng nh?p l?i.",
        "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
    this.Close(); // ?�ng form n?u kh�ng c� user
    return;
}
```

**L� do:** N?u session b? m?t ho?c user ch?a login, `GetUserId()` tr? v? 0. C?n th�ng b�o v� ?�ng form.

---

## ?? **Files ?� S?a**

### ? **ToDoList.GUI/Forms/ProjectDetailsForm.cs**

**Changes:**
1. **Line 11:** Th�m `using ToDoList.GUI.Helpers;`
2. **Line 53-89:** S?a method `LoadTasks()`
   - ??i t? `FirstOrDefaultAsync()` ? `UserSession.GetUserId()`
   - Th�m check `if (_userId == 0)`
   - Th�m filter `.Where(t => ... && t.UserId == _userId ...)`

---

## ?? **K?t Qu?**

B�y gi?:
- ? M?i user CH? th?y tasks **c?a ch�nh m�nh** trong project
- ? TaskReportForm hi?n th? ?�ng th�ng tin task c?a user hi?n t?i
- ? Kh�ng c�n t�nh tr?ng "tasks c?a t?t c? users"
- ? D? li?u ???c ph�n quy?n ch�nh x�c

**Workspace c?a m?i user ho�n to�n ??c l?p!** ??

---

## ?? **T�i Li?u Li�n Quan**

- `FIX_USER_SPECIFIC_TASKS.md` - Fix t??ng t? cho Form1.cs
- `TaskReportForm.cs` - Form b�o c�o task
- `UserSession.cs` - Class qu?n l� session user

---

**?? ?� FIX XONG! M?I USER CH? TH?Y TASKS C?A M�NH!** ??
