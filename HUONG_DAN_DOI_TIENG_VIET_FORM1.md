# H??ng d?n chuy?n ??i ti?ng Vi?t c� d?u sang kh�ng d?u trong Form1.cs

## Danh s�ch c�c text c?n thay ??i

### 1. UpdateGreetingLabels() method

**D�ng 41-42:**
```csharp
// T?:
lblGreeting.Text = $"Ch�o {timeOfDay}, {UserSession.GetDisplayName()}!";
lblUserName.Text = "Tuy?t v?i! B?n ?ang l�m vi?c r?t ch?m ch?.";

// ??I TH�NH:
lblGreeting.Text = $"Chao {timeOfDay}, {UserSession.GetDisplayName()}!";
lblUserName.Text = "Tuyet voi! Ban dang lam viec rat cham chi.";
```

### 2. GetTimeOfDay() method

**D�ng 47-53:**
```csharp
// T?:
if (hour < 12)
    return "bu?i s�ng";
else if (hour < 18)
    return "bu?i chi?u";
else
    return "bu?i t?i";

// ??I TH�NH:
if (hour < 12)
    return "buoi sang";
else if (hour < 18)
    return "buoi chieu";
else
    return "buoi toi";
```

### 3. Buttons trong AddTestButton() method

**D�ng 113:**
```csharp
// T?:
Text = "?? B�o c�o",

// ??I TH�NH:
Text = "Bao cao",
```

**D�ng 171:**
```csharp
// T?:
Text = "?? Trang ch?",

// ??I TH�NH:
Text = "Trang chu",
```

**D�ng 182:**
```csharp
// T?:
Text = "?? B�o c�o",

// ??I TH�NH:
Text = "Bao cao",
```

**D�ng 193:**
```csharp
// T?:
Text = "?? C�ng vi?c",

// ??I TH�NH:
Text = "Cong viec",
```

**D�ng 205:**
```csharp
// T?:
Text = "?? D? �n",

// ??I TH�NH:
Text = "Du an",
```

### 4. BtnTestDB_Click() method

**D�ng 249:**
```csharp
// T?:
MessageBox.Show("Kh�ng th? truy c?p d? li?u v� database ch?a ???c kh?i t?o.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show("Khong the truy cap du lieu vi database chua duoc khoi tao.", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

**D�ng 259:**
```csharp
// T?:
MessageBox.Show($"D? li?u hi?n t?i:\n" +
    $"- Projects: {totalProjects}\n" +
    $"- Tasks: {totalTasks}\n" +
    $"- Completed: {completedTasks}\n" +
    $"- In Progress: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "In Progress")}\n" +
    $"- Pending: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "Pending")}", 
    "Th�ng tin d? li?u", MessageBoxButtons.OK, MessageBoxIcon.Information);

// ??I TH�NH:
MessageBox.Show($"Du lieu hien tai:\n" +
    $"- Projects: {totalProjects}\n" +
    $"- Tasks: {totalTasks}\n" +
    $"- Completed: {completedTasks}\n" +
    $"- In Progress: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "In Progress")}\n" +
    $"- Pending: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "Pending")}", 
    "Thong tin du lieu", MessageBoxButtons.OK, MessageBoxIcon.Information);
```

**D�ng 267:**
```csharp
// T?:
MessageBox.Show($"L?i khi ki?m tra d? li?u: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show($"Loi khi kiem tra du lieu: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 5. BtnReports_Click() method

**D�ng 275:**
```csharp
// T?:
MessageBox.Show("Database context ch?a ???c kh?i t?o. Vui l�ng kh?i ??ng l?i ?ng d?ng.", 
    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show("Database context chua duoc khoi tao. Vui long khoi dong lai ung dung.", 
    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

**D�ng 285:**
```csharp
// T?:
MessageBox.Show($"L?i khi m? b�o c�o:\n{ex.Message}\n\nStack trace:\n{ex.StackTrace}", 
    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show($"Loi khi mo bao cao:\n{ex.Message}\n\nStack trace:\n{ex.StackTrace}", 
    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 6. ShowAdvancedStats() method

**D�ng 292:**
```csharp
// T?:
MessageBox.Show("Kh�ng th? m? th?ng k� n�ng cao v� database ch?a ???c kh?i t?o.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show("Khong the mo thong ke nang cao vi database chua duoc khoi tao.", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

**D�ng 305:**
```csharp
// T?:
MessageBox.Show($"L?i khi m? th?ng k� n�ng cao: {ex.Message}", "L?i",
    MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show($"Loi khi mo thong ke nang cao: {ex.Message}", "Loi",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 7. InitializeDatabase() method

**D�ng 490:**
```csharp
// T?:
MessageBox.Show($"L?i k?t n?i database: {ex.Message}", "L?i", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show($"Loi ket noi database: {ex.Message}", "Loi", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 8. LoadProjectsFromDatabase() method

**D�ng 515:**
```csharp
// T?:
MessageBox.Show("L?i: Kh�ng t�m th?y th�ng tin ng??i d�ng. Vui l�ng ??ng nh?p l?i.",
    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show("Loi: Khong tim thay thong tin nguoi dung. Vui long dang nhap lai.",
    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

**D�ng 545:**
```csharp
// T?:
MessageBox.Show("K?t n?i c? s? d? li?u b? timeout. Vui l�ng th? l?i.\n\n" +
    "N?u l?i v?n ti?p t?c, h�y ki?m tra:\n" +
    "- K?t n?i m?ng\n" +
    "- SQL Server c� ?ang ch?y kh�ng\n" +
    "- Connection string trong appsettings.json", 
    "L?i Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);

// ??I TH�NH:
MessageBox.Show("Ket noi co so du lieu bi timeout. Vui long thu lai.\n\n" +
    "Neu loi van tiep tuc, hay kiem tra:\n" +
    "- Ket noi mang\n" +
    "- SQL Server co dang chay khong\n" +
    "- Connection string trong appsettings.json", 
    "Loi Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
```

**D�ng 554:**
```csharp
// T?:
MessageBox.Show($"L?i khi t?i danh s�ch: {ex.Message}\n\n" +
    $"Chi ti?t: {ex.InnerException?.Message}", 
    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show($"Loi khi tai danh sach: {ex.Message}\n\n" +
    $"Chi tiet: {ex.InnerException?.Message}", 
    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 9. AddProjectCard() method - Footer labels

**D�ng 713:**
```csharp
// T?:
Text = $"{pendingTasks} c�ng vi?c ?ang ch?",

// ??I TH�NH:
Text = $"{pendingTasks} cong viec dang cho",
```

**D�ng 722:**
```csharp
// T?:
Text = $"D? ki?n: {estimatedMinutes}ph",

// ??I TH�NH:
Text = $"Du kien: {estimatedMinutes}ph",
```

### 10. AddCreateListCard() method

**D�ng 816:**
```csharp
// T?:
Text = "T?O DANH S�CH M?I",

// ??I TH�NH:
Text = "TAO DANH SACH MOI",
```

### 11. ShowProjectMenu() method

**D�ng 880:**
```csharp
// T?:
var editItem = menu.Items.Add("?? Ch?nh s?a", null, (s, e) => EditProject(project));
var viewItem = menu.Items.Add("??? Xem chi ti?t", null, (s, e) => OpenProjectDetails(project));
var statsItem = menu.Items.Add("?? Th?ng k� n�ng cao", null, (s, e) => ShowAdvancedStats());
var archiveItem = menu.Items.Add("?? L?u tr?", null, (s, e) => ArchiveProject(project));
var deleteItem = menu.Items.Add("??? X�a", null, (s, e) => DeleteProject(project));

// ??I TH�NH:
var editItem = menu.Items.Add("Chinh sua", null, (s, e) => EditProject(project));
var viewItem = menu.Items.Add("Xem chi tiet", null, (s, e) => OpenProjectDetails(project));
var statsItem = menu.Items.Add("Thong ke nang cao", null, (s, e) => ShowAdvancedStats());
var archiveItem = menu.Items.Add("Luu tru", null, (s, e) => ArchiveProject(project));
var deleteItem = menu.Items.Add("Xoa", null, (s, e) => DeleteProject(project));
```

### 12. EditProject() method

**D�ng 930:**
```csharp
// T?:
MessageBox.Show($"Ch?nh s?a project: {project.ProjectName}", "Th�ng b�o", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);

// ??I TH�NH:
MessageBox.Show($"Chinh sua project: {project.ProjectName}", "Thong bao", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);
```

### 13. ArchiveProject() method

**D�ng 940:**
```csharp
// T?:
var result = MessageBox.Show($"B?n c� mu?n l?u tr? project '{project.ProjectName}'?\n\nProject s? ???c ?n kh?i danh s�ch ch�nh.", 
    "X�c nh?n l?u tr?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

// ??I TH�NH:
var result = MessageBox.Show($"Ban co muon luu tru project '{project.ProjectName}'?\n\nProject se duoc an khoi danh sach chinh.", 
    "Xac nhan luu tru", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
```

**D�ng 949:**
```csharp
// T?:
MessageBox.Show("Project ?� ???c l?u tr? th�nh c�ng!", "Th�nh c�ng", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);

// ??I TH�NH:
MessageBox.Show("Project da duoc luu tru thanh cong!", "Thanh cong", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);
```

**D�ng 955:**
```csharp
// T?:
MessageBox.Show($"L?i khi l?u tr? project: {ex.Message}", "L?i", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show($"Loi khi luu tru project: {ex.Message}", "Loi", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 14. DeleteProject() method

**D�ng 964:**
```csharp
// T?:
var result = MessageBox.Show($"?? B?N C� CH?C CH?N MU?N X�A PROJECT '{project.ProjectName}'?\n\n" +
    "H�nh ??ng n�y s? x�a v?nh vi?n project v� T?T C? c�c task b�n trong!\n" +
    "Kh�ng th? ho�n t�c sau khi x�a!", 
    "?? X�C NH?N X�A V?NH VI?N", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

// ??I TH�NH:
var result = MessageBox.Show($"BAN CO CHAC CHAN MUON XOA PROJECT '{project.ProjectName}'?\n\n" +
    "Hanh dong nay se xoa vinh vien project va TAT CA cac task ben trong!\n" +
    "Khong the hoan tac sau khi xoa!", 
    "XAC NHAN XOA VINH VIEN", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
```

**D�ng 984:**
```csharp
// T?:
MessageBox.Show($"Project '{project.ProjectName}' v� {tasks.Count} task(s) ?� ???c x�a v?nh vi?n!", "?� x�a", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);

// ??I TH�NH:
MessageBox.Show($"Project '{project.ProjectName}' va {tasks.Count} task(s) da duoc xoa vinh vien!", "Da xoa", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);
```

**D�ng 990:**
```csharp
// T?:
MessageBox.Show($"L?i khi x�a project: {ex.Message}", "L?i", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I TH�NH:
MessageBox.Show($"Loi khi xoa project: {ex.Message}", "Loi", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

## C�ch th?c hi?n

1. M? file `ToDoList.GUI\Form1.cs` trong Visual Studio
2. S? d?ng Find & Replace (Ctrl+H) ?? t�m v� thay th? t?ng text
3. Ho?c t�m t?ng d�ng theo s? d�ng v� s?a th? c�ng
4. Save file v� rebuild project

## L?u �

- C�c emoji (??, ??, ??, etc.) c� th? gi? nguy�n ho?c x�a b? t�y �
- N�n backup file tr??c khi s?a
- Sau khi s?a xong, build l?i project ?? ki?m tra l?i

