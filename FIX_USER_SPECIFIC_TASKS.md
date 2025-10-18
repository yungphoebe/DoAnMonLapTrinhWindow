# ? FIX: M?I USER C� TASKS RI�NG BI?T

## ? **V?N ??**

**Hi?n t??ng:** T?t c? users ??u th?y c�ng m?t danh s�ch tasks gi?ng nhau thay v� m?i user c� tasks ri�ng.

**Nguy�n nh�n:** Trong `Form1.cs`, code ?ang l?y **user ??u ti�n** trong database thay v� l?y user hi?n t?i ?ang ??ng nh?p:

```csharp
// ? SAI: L?y user ??u ti�n trong DB
var user = await _context.Users.FirstOrDefaultAsync();
_userId = user.UserId;  // ? Lu�n l� UserId c?a user ??u ti�n!
```

**K?t qu?:** M?i user ??u th?y projects v� tasks c?a **user ??u ti�n** (Nguy?n V?n An).

---

## ? **GI?I PH�P**

### **Thay ??i trong `Form1.cs`:**

#### Before (Sai):
```csharp
private async void LoadProjectsFromDatabase()
{
    try
    {
        if (_context == null) return;
        pnlListsContainer.Controls.Clear();

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
        _userId = user.UserId;  // ? Lu�n l� user ??u ti�n!

        // Load projects
        var projects = await _context.Projects
            .Where(p => p.UserId == _userId && p.IsArchived != true)
            .Include(p => p.Tasks)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
```

#### After (?�ng):
```csharp
private async void LoadProjectsFromDatabase()
{
    try
    {
        if (_context == null) return;
        pnlListsContainer.Controls.Clear();

        // ? L?y UserId t? UserSession (?�NG!)
        _userId = UserSession.GetUserId();
        
        if (_userId == 0)
        {
            MessageBox.Show("L?i: Kh�ng t�m th?y th�ng tin ng??i d�ng. Vui l�ng ??ng nh?p l?i.",
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

## ?? **PH�N T�CH CHI TI?T**

### **V?n ?? 1: Load User Sai**

```csharp
// ? FirstOrDefaultAsync() - L?y user ??U TI�N trong b?ng
var user = await _context.Users.FirstOrDefaultAsync();
```

**K?t qu?:**
- User 1 (Nguy?n V?n An) login ? Th?y projects c?a Nguy?n V?n An ?
- User 2 (Tr?n Th? B�nh) login ? V?N th?y projects c?a Nguy?n V?n An ?
- User 3 (L� Minh C??ng) login ? V?N th?y projects c?a Nguy?n V?n An ?

**T?i sao?** V� `FirstOrDefaultAsync()` lu�n tr? v? b?n ghi ??u ti�n, kh�ng ph? thu?c v�o ai ?ang login!

### **V?n ?? 2: Tasks Kh�ng Filter**

```csharp
// ? Include(p => p.Tasks) - L?y T?T C? tasks c?a project
.Include(p => p.Tasks)
```

Ngay c? khi project thu?c v? user ?�ng, tasks c� th? thu?c v? nhi?u users kh�c nhau (n?u l� shared project). C?n filter:

```csharp
// ? Ch? l?y tasks c?a user hi?n t?i
.Include(p => p.Tasks.Where(t => t.UserId == _userId))
```

---

## ?? **SO S�NH TR??C/SAU**

### **Tr??c Khi Fix:**

| User ??ng Nh?p | UserId ???c Load | Projects Hi?n Th? |
|----------------|------------------|-------------------|
| Nguy?n V?n An (ID: 1) | 1 | Website Redesign, Mobile App ? |
| Tr?n Th? B�nh (ID: 2) | **1** ? | Website Redesign, Mobile App ? |
| L� Minh C??ng (ID: 3) | **1** ? | Website Redesign, Mobile App ? |

**V?n ??:** M?i user ??u th?y projects c?a user ID = 1!

### **Sau Khi Fix:**

| User ??ng Nh?p | UserId ???c Load | Projects Hi?n Th? |
|----------------|------------------|-------------------|
| Nguy?n V?n An (ID: 1) | 1 | Website Redesign, Mobile App ? |
| Tr?n Th? B�nh (ID: 2) | 2 | Marketing Campaign Q4 ? |
| L� Minh C??ng (ID: 3) | 3 | Database Optimization, API Integration ? |

**Gi?i ph�p:** M?i user ch? th?y projects **c?a ch�nh m�nh**!

---

## ?? **C�CH HO?T ??NG C?A FIX**

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

    public static int GetUserId() => _userId;  // ? L?y UserId ?� l?u
}
```

### **2. LoginForm Set User**

Khi user login th�nh c�ng:
```csharp
// LoginForm.cs
var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

if (user != null)
{
    UserSession.SetUser(user.UserId, user.FullName, user.Email);  // ? L?u th�ng tin
    this.DialogResult = DialogResult.OK;
}
```

### **3. Form1 S? D?ng**

```csharp
// Form1.cs
_userId = UserSession.GetUserId();  // ? L?y UserId ?� l?u t? b??c login

// Load ?�NG projects c?a user hi?n t?i
var projects = await _context.Projects
    .Where(p => p.UserId == _userId && p.IsArchived != true)
    .Include(p => p.Tasks.Where(t => t.UserId == _userId))
    .ToListAsync();
```

---

## ?? **C�CH KI?M TRA**

### **Test 1: Login v?i User 1**
1. Email: `nguyenvanan@example.com`
2. Password: `password123`
3. **K?t qu? mong ??i:**
   - Th?y: "Website Redesign", "Mobile App Development"
   - Tasks: "Thi?t k? mockup", "Ph�t tri?n giao di?n", "Thi?t k? UI/UX", "Ph�t tri?n ch?c n?ng chat"

### **Test 2: Logout v� Login v?i User 2**
1. Email: `tranthibinh@example.com`
2. Password: `password123`
3. **K?t qu? mong ??i:**
   - Th?y: "Marketing Campaign Q4"
   - Tasks: "L?p k? ho?ch content", "Ch?y Facebook Ads", "Ph�n t�ch k?t qu?"

### **Test 3: Logout v� Login v?i User 3**
1. Email: `leminhcuong@example.com`
2. Password: `password123`
3. **K?t qu? mong ??i:**
   - Th?y: "Database Optimization", "API Integration"
   - Tasks: "Ph�n t�ch slow queries", "T?o indexes m?i", "Implement authentication flow"

---

## ?? **D? LI?U M?U THEO USER**

### **User 1: Nguy?n V?n An (ID: 1)**
**Projects:**
- Website Redesign (3 tasks)
- Mobile App Development (3 tasks)

**Total:** 6 tasks

### **User 2: Tr?n Th? B�nh (ID: 2)**
**Projects:**
- Marketing Campaign Q4 (3 tasks)

**Total:** 3 tasks

### **User 3: L� Minh C??ng (ID: 3)**
**Projects:**
- Database Optimization (3 tasks)
- API Integration (3 tasks)

**Total:** 6 tasks

### **User 4: Ph?m Thu Duy�n (ID: 4)**
**Projects:**
- User Research (3 tasks)

**Total:** 3 tasks

---

## ?? **L?U �**

### **1. N?u UserSession.GetUserId() Tr? V? 0**

C� th? user ch?a login ho?c session b? m?t. Hi?n th? message v� y�u c?u login l?i:

```csharp
if (_userId == 0)
{
    MessageBox.Show("L?i: Kh�ng t�m th?y th�ng tin ng??i d�ng. Vui l�ng ??ng nh?p l?i.",
        "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return;
}
```

### **2. Tasks C?a Shared Projects**

N?u trong t??ng lai b?n mu?n c� "shared projects" (nhi?u users c�ng l�m vi?c), b?n c?n:

**Option 1:** Hi?n t?t c? tasks trong shared project
```csharp
.Include(p => p.Tasks)  // Kh�ng filter
```

**Option 2:** Ch? hi?n tasks c?a user hi?n t?i trong shared project
```csharp
.Include(p => p.Tasks.Where(t => t.UserId == _userId))  // Filter
```

### **3. CreateListForm C?ng C?n D�ng UserSession**

Khi t?o project m?i:
```csharp
// CreateListForm.cs
var newProject = new Project
{
    UserId = UserSession.GetUserId(),  // ? D�ng user hi?n t?i
    ProjectName = txtProjectName.Text,
    // ...
};
```

---

## ? **CHECKLIST**

- [x] ? S?a `LoadProjectsFromDatabase()` d�ng `UserSession.GetUserId()`
- [x] ? Th�m filter tasks theo `UserId` trong Include
- [x] ? Th�m check n?u `_userId == 0`
- [x] ? Build successful

---

## ?? **K?T QU?**

B�y gi?:
- ? **Nguy?n V?n An** ch? th?y projects v� tasks c?a m�nh
- ? **Tr?n Th? B�nh** ch? th?y projects v� tasks c?a m�nh
- ? **L� Minh C??ng** ch? th?y projects v� tasks c?a m�nh
- ? **Ph?m Thu Duy�n** ch? th?y projects v� tasks c?a m�nh

M?i user c� workspace **ho�n to�n ri�ng bi?t**!

---

## ?? **FILES ?� S?A**

### ? ToDoList.GUI/Form1.cs

**Changes:**
1. Line ~356: ??i t? `FirstOrDefaultAsync()` ? `UserSession.GetUserId()`
2. Line ~364: Th�m check `if (_userId == 0)`
3. Line ~372: Th�m filter `.Where(t => t.UserId == _userId)` cho Tasks

---

## ?? **T�I LI?U THAM KH?O**

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

**?? ?� FIX XONG! M?I USER C� TASKS RI�NG!** ??
