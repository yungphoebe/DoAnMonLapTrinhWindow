# ? FIX: M?I USER CÓ TASKS RIÊNG BI?T

## ? **V?N ??**

**Hi?n t??ng:** T?t c? users ??u th?y cùng m?t danh sách tasks gi?ng nhau thay vì m?i user có tasks riêng.

**Nguyên nhân:** Trong `Form1.cs`, code ?ang l?y **user ??u tiên** trong database thay vì l?y user hi?n t?i ?ang ??ng nh?p:

```csharp
// ? SAI: L?y user ??u tiên trong DB
var user = await _context.Users.FirstOrDefaultAsync();
_userId = user.UserId;  // ? Luôn là UserId c?a user ??u tiên!
```

**K?t qu?:** M?i user ??u th?y projects và tasks c?a **user ??u tiên** (Nguy?n V?n An).

---

## ? **GI?I PHÁP**

### **Thay ??i trong `Form1.cs`:**

#### Before (Sai):
```csharp
private async void LoadProjectsFromDatabase()
{
    try
    {
        if (_context == null) return;
        pnlListsContainer.Controls.Clear();

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
        _userId = user.UserId;  // ? Luôn là user ??u tiên!

        // Load projects
        var projects = await _context.Projects
            .Where(p => p.UserId == _userId && p.IsArchived != true)
            .Include(p => p.Tasks)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
```

#### After (?úng):
```csharp
private async void LoadProjectsFromDatabase()
{
    try
    {
        if (_context == null) return;
        pnlListsContainer.Controls.Clear();

        // ? L?y UserId t? UserSession (?ÚNG!)
        _userId = UserSession.GetUserId();
        
        if (_userId == 0)
        {
            MessageBox.Show("L?i: Không tìm th?y thông tin ng??i dùng. Vui lòng ??ng nh?p l?i.",
                "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Load projects - CH? C?A USER HI?N T?I
        var projects = await _context.Projects
            .Where(p => p.UserId == _userId && p.IsArchived != true)
            .Include(p => p.Tasks.Where(t => t.UserId == _userId))  // ? Filter tasks
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
```

---

## ?? **PHÂN TÍCH CHI TI?T**

### **V?n ?? 1: Load User Sai**

```csharp
// ? FirstOrDefaultAsync() - L?y user ??U TIÊN trong b?ng
var user = await _context.Users.FirstOrDefaultAsync();
```

**K?t qu?:**
- User 1 (Nguy?n V?n An) login ? Th?y projects c?a Nguy?n V?n An ?
- User 2 (Tr?n Th? Bình) login ? V?N th?y projects c?a Nguy?n V?n An ?
- User 3 (Lê Minh C??ng) login ? V?N th?y projects c?a Nguy?n V?n An ?

**T?i sao?** Vì `FirstOrDefaultAsync()` luôn tr? v? b?n ghi ??u tiên, không ph? thu?c vào ai ?ang login!

### **V?n ?? 2: Tasks Không Filter**

```csharp
// ? Include(p => p.Tasks) - L?y T?T C? tasks c?a project
.Include(p => p.Tasks)
```

Ngay c? khi project thu?c v? user ?úng, tasks có th? thu?c v? nhi?u users khác nhau (n?u là shared project). C?n filter:

```csharp
// ? Ch? l?y tasks c?a user hi?n t?i
.Include(p => p.Tasks.Where(t => t.UserId == _userId))
```

---

## ?? **SO SÁNH TR??C/SAU**

### **Tr??c Khi Fix:**

| User ??ng Nh?p | UserId ???c Load | Projects Hi?n Th? |
|----------------|------------------|-------------------|
| Nguy?n V?n An (ID: 1) | 1 | Website Redesign, Mobile App ? |
| Tr?n Th? Bình (ID: 2) | **1** ? | Website Redesign, Mobile App ? |
| Lê Minh C??ng (ID: 3) | **1** ? | Website Redesign, Mobile App ? |

**V?n ??:** M?i user ??u th?y projects c?a user ID = 1!

### **Sau Khi Fix:**

| User ??ng Nh?p | UserId ???c Load | Projects Hi?n Th? |
|----------------|------------------|-------------------|
| Nguy?n V?n An (ID: 1) | 1 | Website Redesign, Mobile App ? |
| Tr?n Th? Bình (ID: 2) | 2 | Marketing Campaign Q4 ? |
| Lê Minh C??ng (ID: 3) | 3 | Database Optimization, API Integration ? |

**Gi?i pháp:** M?i user ch? th?y projects **c?a chính mình**!

---

## ?? **CÁCH HO?T ??NG C?A FIX**

### **1. UserSession.GetUserId()**

File `UserSession.cs`:
```csharp
public static class UserSession
{
    private static int _userId = 0;
    private static string _userName = "";
    private static string _email = "";

    public static void SetUser(int userId, string userName, string email)
    {
        _userId = userId;
        _userName = userName;
        _email = email;
    }

    public static int GetUserId() => _userId;  // ? L?y UserId ?ã l?u
}
```

### **2. LoginForm Set User**

Khi user login thành công:
```csharp
// LoginForm.cs
var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

if (user != null)
{
    UserSession.SetUser(user.UserId, user.FullName, user.Email);  // ? L?u thông tin
    this.DialogResult = DialogResult.OK;
}
```

### **3. Form1 S? D?ng**

```csharp
// Form1.cs
_userId = UserSession.GetUserId();  // ? L?y UserId ?ã l?u t? b??c login

// Load ?ÚNG projects c?a user hi?n t?i
var projects = await _context.Projects
    .Where(p => p.UserId == _userId && p.IsArchived != true)
    .Include(p => p.Tasks.Where(t => t.UserId == _userId))
    .ToListAsync();
```

---

## ?? **CÁCH KI?M TRA**

### **Test 1: Login v?i User 1**
1. Email: `nguyenvanan@example.com`
2. Password: `password123`
3. **K?t qu? mong ??i:**
   - Th?y: "Website Redesign", "Mobile App Development"
   - Tasks: "Thi?t k? mockup", "Phát tri?n giao di?n", "Thi?t k? UI/UX", "Phát tri?n ch?c n?ng chat"

### **Test 2: Logout và Login v?i User 2**
1. Email: `tranthibinh@example.com`
2. Password: `password123`
3. **K?t qu? mong ??i:**
   - Th?y: "Marketing Campaign Q4"
   - Tasks: "L?p k? ho?ch content", "Ch?y Facebook Ads", "Phân tích k?t qu?"

### **Test 3: Logout và Login v?i User 3**
1. Email: `leminhcuong@example.com`
2. Password: `password123`
3. **K?t qu? mong ??i:**
   - Th?y: "Database Optimization", "API Integration"
   - Tasks: "Phân tích slow queries", "T?o indexes m?i", "Implement authentication flow"

---

## ?? **D? LI?U M?U THEO USER**

### **User 1: Nguy?n V?n An (ID: 1)**
**Projects:**
- Website Redesign (3 tasks)
- Mobile App Development (3 tasks)

**Total:** 6 tasks

### **User 2: Tr?n Th? Bình (ID: 2)**
**Projects:**
- Marketing Campaign Q4 (3 tasks)

**Total:** 3 tasks

### **User 3: Lê Minh C??ng (ID: 3)**
**Projects:**
- Database Optimization (3 tasks)
- API Integration (3 tasks)

**Total:** 6 tasks

### **User 4: Ph?m Thu Duyên (ID: 4)**
**Projects:**
- User Research (3 tasks)

**Total:** 3 tasks

---

## ?? **L?U Ý**

### **1. N?u UserSession.GetUserId() Tr? V? 0**

Có th? user ch?a login ho?c session b? m?t. Hi?n th? message và yêu c?u login l?i:

```csharp
if (_userId == 0)
{
    MessageBox.Show("L?i: Không tìm th?y thông tin ng??i dùng. Vui lòng ??ng nh?p l?i.",
        "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return;
}
```

### **2. Tasks C?a Shared Projects**

N?u trong t??ng lai b?n mu?n có "shared projects" (nhi?u users cùng làm vi?c), b?n c?n:

**Option 1:** Hi?n t?t c? tasks trong shared project
```csharp
.Include(p => p.Tasks)  // Không filter
```

**Option 2:** Ch? hi?n tasks c?a user hi?n t?i trong shared project
```csharp
.Include(p => p.Tasks.Where(t => t.UserId == _userId))  // Filter
```

### **3. CreateListForm C?ng C?n Dùng UserSession**

Khi t?o project m?i:
```csharp
// CreateListForm.cs
var newProject = new Project
{
    UserId = UserSession.GetUserId(),  // ? Dùng user hi?n t?i
    ProjectName = txtProjectName.Text,
    // ...
};
```

---

## ? **CHECKLIST**

- [x] ? S?a `LoadProjectsFromDatabase()` dùng `UserSession.GetUserId()`
- [x] ? Thêm filter tasks theo `UserId` trong Include
- [x] ? Thêm check n?u `_userId == 0`
- [x] ? Build successful

---

## ?? **K?T QU?**

Bây gi?:
- ? **Nguy?n V?n An** ch? th?y projects và tasks c?a mình
- ? **Tr?n Th? Bình** ch? th?y projects và tasks c?a mình
- ? **Lê Minh C??ng** ch? th?y projects và tasks c?a mình
- ? **Ph?m Thu Duyên** ch? th?y projects và tasks c?a mình

M?i user có workspace **hoàn toàn riêng bi?t**!

---

## ?? **FILES ?Ã S?A**

### ? ToDoList.GUI/Form1.cs

**Changes:**
1. Line ~356: ??i t? `FirstOrDefaultAsync()` ? `UserSession.GetUserId()`
2. Line ~364: Thêm check `if (_userId == 0)`
3. Line ~372: Thêm filter `.Where(t => t.UserId == _userId)` cho Tasks

---

## ?? **TÀI LI?U THAM KH?O**

### **UserSession Pattern**

```csharp
// 1. Login - Save user info
UserSession.SetUser(userId, userName, email);

// 2. Anywhere - Get current user
int currentUserId = UserSession.GetUserId();
string currentUserName = UserSession.GetDisplayName();

// 3. Logout - Clear session
UserSession.Clear();
```

### **Entity Framework Filter Pattern**

```csharp
// Load v?i filter
var data = await _context.Projects
    .Where(p => p.UserId == currentUserId)  // Filter 1
    .Include(p => p.Tasks.Where(t => t.UserId == currentUserId))  // Filter 2
    .ToListAsync();
```

---

**?? ?Ã FIX XONG! M?I USER CÓ TASKS RIÊNG!** ??
