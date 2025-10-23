# H??ng d?n chuy?n ??i ti?ng Vi?t có d?u sang không d?u trong Form1.cs

## Danh sách các text c?n thay ??i

### 1. UpdateGreetingLabels() method

**Dòng 41-42:**
```csharp
// T?:
lblGreeting.Text = $"Chào {timeOfDay}, {UserSession.GetDisplayName()}!";
lblUserName.Text = "Tuy?t v?i! B?n ?ang làm vi?c r?t ch?m ch?.";

// ??I THÀNH:
lblGreeting.Text = $"Chao {timeOfDay}, {UserSession.GetDisplayName()}!";
lblUserName.Text = "Tuyet voi! Ban dang lam viec rat cham chi.";
```

### 2. GetTimeOfDay() method

**Dòng 47-53:**
```csharp
// T?:
if (hour < 12)
    return "bu?i sáng";
else if (hour < 18)
    return "bu?i chi?u";
else
    return "bu?i t?i";

// ??I THÀNH:
if (hour < 12)
    return "buoi sang";
else if (hour < 18)
    return "buoi chieu";
else
    return "buoi toi";
```

### 3. Buttons trong AddTestButton() method

**Dòng 113:**
```csharp
// T?:
Text = "?? Báo cáo",

// ??I THÀNH:
Text = "Bao cao",
```

**Dòng 171:**
```csharp
// T?:
Text = "?? Trang ch?",

// ??I THÀNH:
Text = "Trang chu",
```

**Dòng 182:**
```csharp
// T?:
Text = "?? Báo cáo",

// ??I THÀNH:
Text = "Bao cao",
```

**Dòng 193:**
```csharp
// T?:
Text = "?? Công vi?c",

// ??I THÀNH:
Text = "Cong viec",
```

**Dòng 205:**
```csharp
// T?:
Text = "?? D? án",

// ??I THÀNH:
Text = "Du an",
```

### 4. BtnTestDB_Click() method

**Dòng 249:**
```csharp
// T?:
MessageBox.Show("Không th? truy c?p d? li?u vì database ch?a ???c kh?i t?o.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show("Khong the truy cap du lieu vi database chua duoc khoi tao.", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

**Dòng 259:**
```csharp
// T?:
MessageBox.Show($"D? li?u hi?n t?i:\n" +
    $"- Projects: {totalProjects}\n" +
    $"- Tasks: {totalTasks}\n" +
    $"- Completed: {completedTasks}\n" +
    $"- In Progress: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "In Progress")}\n" +
    $"- Pending: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "Pending")}", 
    "Thông tin d? li?u", MessageBoxButtons.OK, MessageBoxIcon.Information);

// ??I THÀNH:
MessageBox.Show($"Du lieu hien tai:\n" +
    $"- Projects: {totalProjects}\n" +
    $"- Tasks: {totalTasks}\n" +
    $"- Completed: {completedTasks}\n" +
    $"- In Progress: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "In Progress")}\n" +
    $"- Pending: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "Pending")}", 
    "Thong tin du lieu", MessageBoxButtons.OK, MessageBoxIcon.Information);
```

**Dòng 267:**
```csharp
// T?:
MessageBox.Show($"L?i khi ki?m tra d? li?u: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show($"Loi khi kiem tra du lieu: {ex.Message}", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 5. BtnReports_Click() method

**Dòng 275:**
```csharp
// T?:
MessageBox.Show("Database context ch?a ???c kh?i t?o. Vui lòng kh?i ??ng l?i ?ng d?ng.", 
    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show("Database context chua duoc khoi tao. Vui long khoi dong lai ung dung.", 
    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

**Dòng 285:**
```csharp
// T?:
MessageBox.Show($"L?i khi m? báo cáo:\n{ex.Message}\n\nStack trace:\n{ex.StackTrace}", 
    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show($"Loi khi mo bao cao:\n{ex.Message}\n\nStack trace:\n{ex.StackTrace}", 
    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 6. ShowAdvancedStats() method

**Dòng 292:**
```csharp
// T?:
MessageBox.Show("Không th? m? th?ng kê nâng cao vì database ch?a ???c kh?i t?o.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show("Khong the mo thong ke nang cao vi database chua duoc khoi tao.", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

**Dòng 305:**
```csharp
// T?:
MessageBox.Show($"L?i khi m? th?ng kê nâng cao: {ex.Message}", "L?i",
    MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show($"Loi khi mo thong ke nang cao: {ex.Message}", "Loi",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 7. InitializeDatabase() method

**Dòng 490:**
```csharp
// T?:
MessageBox.Show($"L?i k?t n?i database: {ex.Message}", "L?i", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show($"Loi ket noi database: {ex.Message}", "Loi", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 8. LoadProjectsFromDatabase() method

**Dòng 515:**
```csharp
// T?:
MessageBox.Show("L?i: Không tìm th?y thông tin ng??i dùng. Vui lòng ??ng nh?p l?i.",
    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show("Loi: Khong tim thay thong tin nguoi dung. Vui long dang nhap lai.",
    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

**Dòng 545:**
```csharp
// T?:
MessageBox.Show("K?t n?i c? s? d? li?u b? timeout. Vui lòng th? l?i.\n\n" +
    "N?u l?i v?n ti?p t?c, hãy ki?m tra:\n" +
    "- K?t n?i m?ng\n" +
    "- SQL Server có ?ang ch?y không\n" +
    "- Connection string trong appsettings.json", 
    "L?i Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);

// ??I THÀNH:
MessageBox.Show("Ket noi co so du lieu bi timeout. Vui long thu lai.\n\n" +
    "Neu loi van tiep tuc, hay kiem tra:\n" +
    "- Ket noi mang\n" +
    "- SQL Server co dang chay khong\n" +
    "- Connection string trong appsettings.json", 
    "Loi Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
```

**Dòng 554:**
```csharp
// T?:
MessageBox.Show($"L?i khi t?i danh sách: {ex.Message}\n\n" +
    $"Chi ti?t: {ex.InnerException?.Message}", 
    "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show($"Loi khi tai danh sach: {ex.Message}\n\n" +
    $"Chi tiet: {ex.InnerException?.Message}", 
    "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 9. AddProjectCard() method - Footer labels

**Dòng 713:**
```csharp
// T?:
Text = $"{pendingTasks} công vi?c ?ang ch?",

// ??I THÀNH:
Text = $"{pendingTasks} cong viec dang cho",
```

**Dòng 722:**
```csharp
// T?:
Text = $"D? ki?n: {estimatedMinutes}ph",

// ??I THÀNH:
Text = $"Du kien: {estimatedMinutes}ph",
```

### 10. AddCreateListCard() method

**Dòng 816:**
```csharp
// T?:
Text = "T?O DANH SÁCH M?I",

// ??I THÀNH:
Text = "TAO DANH SACH MOI",
```

### 11. ShowProjectMenu() method

**Dòng 880:**
```csharp
// T?:
var editItem = menu.Items.Add("?? Ch?nh s?a", null, (s, e) => EditProject(project));
var viewItem = menu.Items.Add("??? Xem chi ti?t", null, (s, e) => OpenProjectDetails(project));
var statsItem = menu.Items.Add("?? Th?ng kê nâng cao", null, (s, e) => ShowAdvancedStats());
var archiveItem = menu.Items.Add("?? L?u tr?", null, (s, e) => ArchiveProject(project));
var deleteItem = menu.Items.Add("??? Xóa", null, (s, e) => DeleteProject(project));

// ??I THÀNH:
var editItem = menu.Items.Add("Chinh sua", null, (s, e) => EditProject(project));
var viewItem = menu.Items.Add("Xem chi tiet", null, (s, e) => OpenProjectDetails(project));
var statsItem = menu.Items.Add("Thong ke nang cao", null, (s, e) => ShowAdvancedStats());
var archiveItem = menu.Items.Add("Luu tru", null, (s, e) => ArchiveProject(project));
var deleteItem = menu.Items.Add("Xoa", null, (s, e) => DeleteProject(project));
```

### 12. EditProject() method

**Dòng 930:**
```csharp
// T?:
MessageBox.Show($"Ch?nh s?a project: {project.ProjectName}", "Thông báo", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);

// ??I THÀNH:
MessageBox.Show($"Chinh sua project: {project.ProjectName}", "Thong bao", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);
```

### 13. ArchiveProject() method

**Dòng 940:**
```csharp
// T?:
var result = MessageBox.Show($"B?n có mu?n l?u tr? project '{project.ProjectName}'?\n\nProject s? ???c ?n kh?i danh sách chính.", 
    "Xác nh?n l?u tr?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

// ??I THÀNH:
var result = MessageBox.Show($"Ban co muon luu tru project '{project.ProjectName}'?\n\nProject se duoc an khoi danh sach chinh.", 
    "Xac nhan luu tru", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
```

**Dòng 949:**
```csharp
// T?:
MessageBox.Show("Project ?ã ???c l?u tr? thành công!", "Thành công", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);

// ??I THÀNH:
MessageBox.Show("Project da duoc luu tru thanh cong!", "Thanh cong", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);
```

**Dòng 955:**
```csharp
// T?:
MessageBox.Show($"L?i khi l?u tr? project: {ex.Message}", "L?i", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show($"Loi khi luu tru project: {ex.Message}", "Loi", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 14. DeleteProject() method

**Dòng 964:**
```csharp
// T?:
var result = MessageBox.Show($"?? B?N CÓ CH?C CH?N MU?N XÓA PROJECT '{project.ProjectName}'?\n\n" +
    "Hành ??ng này s? xóa v?nh vi?n project và T?T C? các task bên trong!\n" +
    "Không th? hoàn tác sau khi xóa!", 
    "?? XÁC NH?N XÓA V?NH VI?N", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

// ??I THÀNH:
var result = MessageBox.Show($"BAN CO CHAC CHAN MUON XOA PROJECT '{project.ProjectName}'?\n\n" +
    "Hanh dong nay se xoa vinh vien project va TAT CA cac task ben trong!\n" +
    "Khong the hoan tac sau khi xoa!", 
    "XAC NHAN XOA VINH VIEN", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
```

**Dòng 984:**
```csharp
// T?:
MessageBox.Show($"Project '{project.ProjectName}' và {tasks.Count} task(s) ?ã ???c xóa v?nh vi?n!", "?ã xóa", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);

// ??I THÀNH:
MessageBox.Show($"Project '{project.ProjectName}' va {tasks.Count} task(s) da duoc xoa vinh vien!", "Da xoa", 
    MessageBoxButtons.OK, MessageBoxIcon.Information);
```

**Dòng 990:**
```csharp
// T?:
MessageBox.Show($"L?i khi xóa project: {ex.Message}", "L?i", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);

// ??I THÀNH:
MessageBox.Show($"Loi khi xoa project: {ex.Message}", "Loi", 
    MessageBoxButtons.OK, MessageBoxIcon.Error);
```

## Cách th?c hi?n

1. M? file `ToDoList.GUI\Form1.cs` trong Visual Studio
2. S? d?ng Find & Replace (Ctrl+H) ?? tìm và thay th? t?ng text
3. Ho?c tìm t?ng dòng theo s? dòng và s?a th? công
4. Save file và rebuild project

## L?u ý

- Các emoji (??, ??, ??, etc.) có th? gi? nguyên ho?c xóa b? tùy ý
- Nên backup file tr??c khi s?a
- Sau khi s?a xong, build l?i project ?? ki?m tra l?i

