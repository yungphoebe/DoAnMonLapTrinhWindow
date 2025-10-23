# DANH SACH FULL TAT CA CHU TIENG VIET CAN SUA

## CAC FILE CAN SUA NGAY

### 1. ToDoList.GUI\Program.cs
**D�ng 31-32:** Comment
```csharp
// T?: // ??c c?u h�nh t? appsettings.json
// ??I: // Doc cau hinh tu appsettings.json

// T?: // Test k?t n?i database n?u ???c b?t  
// ??I: // Test ket noi database neu duoc bat

// T?: // D�ng Custom Form v?i font ti?ng Vi?t ??p
// ??I: // Dung Custom Form voi font tieng Viet dep

// T?: // D�ng Console - C?n thi?t l?p UTF-8
// ??I: // Dung Console - Can thiet lap UTF-8

// T?: Console.WriteLine("\n\nNh?n ph�m b?t k? ?? m? ?ng d?ng...");
// ??I: Console.WriteLine("\n\nNhan phim bat ky de mo ung dung...");

// T?: // Hi?n th? m�n h�nh ??ng nh?p tr??c
// ??I: // Hien thi man hinh dang nhap truoc

// T?: // N?u ??ng nh?p th�nh c�ng, ch?y Form1
// ??I: // Neu dang nhap thanh cong, chay Form1

// T?: // N?u h?y ??ng nh?p, tho�t ?ng d?ng
// ??I: // Neu huy dang nhap, thoat ung dung
```

### 2. ToDoList.GUI\Form1.cs - FULL LIST (60+ V? TR�)

#### A. UpdateGreetingLabels() - D�ng 41-42
```csharp
lblGreeting.Text = $"Chao {timeOfDay}, {UserSession.GetDisplayName()}!";
lblUserName.Text = "Tuyet voi! Ban dang lam viec rat cham chi.";
```

#### B. GetTimeOfDay() - D�ng 47-53
```csharp
return "buoi sang";
return "buoi chieu";
return "buoi toi";
```

#### C. AddTestButton() - D�ng 113
```csharp
Text = "Bao cao",
```

#### D. CreateBottomNavigationPanel() - D�ng 171, 182, 193, 205
```csharp
Text = "Trang chu",
Text = "Bao cao",
Text = "Cong viec",
Text = "Du an",
```

#### E. BtnTestData_Click() - D�ng 249
```csharp
MessageBox.Show("Khong the truy cap du lieu vi database chua duoc khoi tao.", "Loi", ...);
```

#### F. BtnTestData_Click() - D�ng 259
```csharp
MessageBox.Show($"Du lieu hien tai:\n" +
    $"- Projects: {totalProjects}\n" +
    $"- Tasks: {totalTasks}\n" +
    $"- Completed: {completedTasks}\n" +
    $"- In Progress: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "In Progress")}\n" +
    $"- Pending: {_context.Tasks.Count(t => t.IsDeleted != true && t.Status == "Pending")}", 
    "Thong tin du lieu", ...);
```

#### G. BtnTestData_Click() - D�ng 267
```csharp
MessageBox.Show($"Loi khi kiem tra du lieu: {ex.Message}", "Loi", ...);
```

#### H. BtnReports_Click() - D�ng 275, 285
```csharp
MessageBox.Show("Database context chua duoc khoi tao. Vui long khoi dong lai ung dung.", "Loi", ...);
MessageBox.Show($"Loi khi mo bao cao:\n{ex.Message}\n\nStack trace:\n{ex.StackTrace}", "Loi", ...);
```

#### I. ShowAdvancedStats() - D�ng 292, 305
```csharp
MessageBox.Show("Khong the mo thong ke nang cao vi database chua duoc khoi tao.", "Loi", ...);
MessageBox.Show($"Loi khi mo thong ke nang cao: {ex.Message}", "Loi", ...);
```

#### J. InitializeDatabase() - D�ng 490
```csharp
MessageBox.Show($"Loi ket noi database: {ex.Message}", "Loi", ...);
```

#### K. LoadProjectsFromDatabase() - D�ng 515, 545, 554
```csharp
MessageBox.Show("Loi: Khong tim thay thong tin nguoi dung. Vui long dang nhap lai.", "Loi", ...);
MessageBox.Show("Ket noi co so du lieu bi timeout. Vui long thu lai.\n\n" +
    "Neu loi van tiep tuc, hay kiem tra:\n" +
    "- Ket noi mang\n" +
    "- SQL Server co dang chay khong\n" +
    "- Connection string trong appsettings.json", "Loi Timeout", ...);
MessageBox.Show($"Loi khi tai danh sach: {ex.Message}\n\n" +
    $"Chi tiet: {ex.InnerException?.Message}", "Loi", ...);
```

#### L. AddProjectCard() - D�ng 713, 722
```csharp
Text = $"{pendingTasks} cong viec dang cho",
Text = $"Du kien: {estimatedMinutes}ph",
```

#### M. AddListCard() - D�ng 786, 793
```csharp
Text = $"{pendingTasks} cong viec dang cho",
Text = $"Du kien: {estimatedMinutes}phut",
```

#### N. AddCreateListCard() - D�ng 816
```csharp
Text = "TAO DANH SACH MOI",
```

#### O. ShowProjectMenu() - D�ng 880-884
```csharp
var editItem = menu.Items.Add("Chinh sua", null, ...);
var viewItem = menu.Items.Add("Xem chi tiet", null, ...);
var statsItem = menu.Items.Add("Thong ke nang cao", null, ...);
var archiveItem = menu.Items.Add("Luu tru", null, ...);
var deleteItem = menu.Items.Add("Xoa", null, ...);
```

#### P. EditProject() - D�ng 930
```csharp
MessageBox.Show($"Chinh sua project: {project.ProjectName}", "Thong bao", ...);
```

#### Q. ArchiveProject() - D�ng 940, 949, 955
```csharp
MessageBox.Show($"Ban co muon luu tru project '{project.ProjectName}'?\n\nProject se duoc an khoi danh sach chinh.", "Xac nhan luu tru", ...);
MessageBox.Show("Project da duoc luu tru thanh cong!", "Thanh cong", ...);
MessageBox.Show($"Loi khi luu tru project: {ex.Message}", "Loi", ...);
```

#### R. DeleteProject() - D�ng 964, 984, 990
```csharp
MessageBox.Show($"BAN CO CHAC CHAN MUON XOA PROJECT '{project.ProjectName}'?\n\n" +
    "Hanh dong nay se xoa vinh vien project va TAT CA cac task ben trong!\n" +
    "Khong the hoan tac sau khi xoa!", "XAC NHAN XOA VINH VIEN", ...);
MessageBox.Show($"Project '{project.ProjectName}' va {tasks.Count} task(s) da duoc xoa vinh vien!", "Da xoa", ...);
MessageBox.Show($"Loi khi xoa project: {ex.Message}", "Loi", ...);
```

### 3. ToDoList.GUI\Forms\ProjectDetailsForm.cs

#### D�ng 28:
```csharp
MessageBox.Show($"Loi ket noi database: {ex.Message}", "Loi", ...);
```

#### D�ng 40:
```csharp
lblProjectDescription.Text = _project.Description ?? "Khong co mo ta";
```

#### D�ng 62:
```csharp
MessageBox.Show("Loi: Khong tim thay thong tin nguoi dung. Vui long dang nhap lai.", "Loi", ...);
```

#### D�ng 82:
```csharp
MessageBox.Show($"Loi khi tai danh sach cong viec: {ex.Message}", "Loi", ...);
```

#### D�ng 116:
```csharp
Text = task.DueDate?.ToString("dd/MM/yyyy") ?? "Khong co",
```

#### D�ng 138:
```csharp
toolTip.SetToolTip(btnReport, "Xem bao cao chi tiet task");
toolTip.SetToolTip(btnEdit, "Chinh sua task");
toolTip.SetToolTip(btnMenu, "Them tuy chon");
```

#### D�ng 191:
```csharp
MessageBox.Show($"Loi khi cap nhat trang thai: {ex.Message}", "Loi", ...);
```

#### D�ng 203:
```csharp
var toggleText = task.Status == "Completed" ? "Danh dau chua hoan thanh" : "Danh dau hoan thanh";
```

#### D�ng 214:
```csharp
var deleteItem = menu.Items.Add("Xoa task", null, ...);
```

#### D�ng 229:
```csharp
MessageBox.Show($"Loi khi mo bao cao: {ex.Message}", "Loi", ...);
```

#### D�ng 237:
```csharp
MessageBox.Show($"Chinh sua task: {task.Title}", "Thong bao", ...);
```

#### D�ng 245:
```csharp
MessageBox.Show($"Ban co chac chan muon xoa task '{task.Title}'?", "Xac nhan xoa", ...);
```

#### D�ng 258:
```csharp
MessageBox.Show($"Loi khi xoa task: {ex.Message}", "Loi", ...);
```

### 4. ToDoList.GUI\Tests\ProjectManagementTest.cs

#### Comments (Multiple lines):
```csharp
/// Test class ?? ki?m tra ch?c n?ng qu?n l� project
// ID test ?? kh�ng ?nh h??ng d? li?u th?t
/// Test t?o project m?i
/// Test t?o task m?i  
/// Test l?y danh s�ch project
/// Test c?p nh?t task
/// Test x�a task (soft delete)
/// Ch?y t?t c? test
/// D?n d?p d? li?u test
```

#### MessageBox (Multiple lines):
```csharp
MessageBox.Show($"Loi ket noi database: {ex.Message}", "Loi", ...);
MessageBox.Show($"Tao project thanh cong! ID: {project.ProjectId}", "Test", ...);
MessageBox.Show($"Loi tao project: {ex.Message}", "Test", ...);
MessageBox.Show($"Tao task thanh cong! ID: {task.TaskId}", "Test", ...);
MessageBox.Show($"Loi tao task: {ex.Message}", "Test", ...);
MessageBox.Show($"Tim thay {projects.Count} project(s)", "Test", ...);
MessageBox.Show($"Loi lay danh sach project: {ex.Message}", "Test", ...);
MessageBox.Show($"Cap nhat task thanh cong!", "Test", ...);
MessageBox.Show("Khong tim thay task", "Test", ...);
MessageBox.Show($"Loi cap nhat task: {ex.Message}", "Test", ...);
MessageBox.Show($"Xoa task thanh cong!", "Test", ...);
MessageBox.Show($"Loi xoa task: {ex.Message}", "Test", ...);
MessageBox.Show("Bat dau chay test...", "Test", ...);
MessageBox.Show("Hoan thanh tat ca test!", "Test", ...);
MessageBox.Show("Don dep du lieu test thanh cong!", "Test", ...);
MessageBox.Show($"Loi don dep: {ex.Message}", "Test", ...);
```

### 5. ToDoList.GUI\Tests\DatabaseConnectionTest.cs

#### Project/Task properties:
```csharp
ProjectName = "Du an mau",
Description = "Day la du an mau de test",
Title = "Cong viec mau",
Description = "Day la cong viec mau",
```

#### MessageBox:
```csharp
MessageBox.Show($"Da tao du lieu mau:\n" +
    $"� User: {user.FullName} (ID: {user.UserId})\n" +
    $"� Project: {project.ProjectName} (ID: {project.ProjectId})\n" +
    $"� Task: {task.Title} (ID: {task.TaskId})", ...);
MessageBox.Show($"Loi test database:\n\n{ex.Message}\n\nChi tiet: {ex.InnerException?.Message}", ...);
```

### 6. ToDoList.GUI\Tests\SimpleDatabaseTest.cs

#### Comments:
```csharp
/// Test ??n gi?n ?? ki?m tra k?t n?i database
```

#### MessageBox:
```csharp
MessageBox.Show("Ket noi database thanh cong!", "Test Database", ...);
MessageBox.Show("Khong the ket noi database!", "Test Database", ...);
MessageBox.Show($"Loi ket noi database:\n\n{ex.Message}\n\nChi tiet: {ex.InnerException?.Message}", ...);
MessageBox.Show($"Tao project thanh cong!\nID: {project.ProjectId}\nTen: {project.ProjectName}", ...);
MessageBox.Show($"Loi tao project:\n\n{ex.Message}\n\nChi tiet: {ex.InnerException?.Message}", ...);
```

## CACH SUA NHANH

### Option 1: Find & Replace trong Visual Studio
1. Ctrl + Shift + H (Replace in Files)
2. Copy t?ng c?p text t? file n�y
3. Replace All

### Option 2: S? d?ng Script PowerShell
T?o file `fix_vietnamese.ps1`:

```powershell
# List of files to fix
$files = @(
    "ToDoList.GUI\Program.cs",
    "ToDoList.GUI\Form1.cs",
    "ToDoList.GUI\Forms\ProjectDetailsForm.cs",
    "ToDoList.GUI\Tests\ProjectManagementTest.cs",
    "ToDoList.GUI\Tests\DatabaseConnectionTest.cs",
    "ToDoList.GUI\Tests\SimpleDatabaseTest.cs"
)

# Vietnamese replacement pairs
$replacements = @{
    "??c" = "Doc"
    "D�ng" = "Dung"
    "C?n" = "Can"
    "Nh?n" = "Nhan"
    "Hi?n" = "Hien"
    "N?u" = "Neu"
    "??ng" = "dang"
    "h?y" = "huy"
    # Add more pairs...
}

foreach ($file in $files) {
    if (Test-Path $file) {
        $content = Get-Content $file -Raw
        foreach ($key in $replacements.Keys) {
            $content = $content -replace $key, $replacements[$key]
        }
        Set-Content $file $content
        Write-Host "Fixed: $file"
    }
}
```

### Option 3: S?a th? c�ng theo file
1. M? t?ng file
2. T�m text c� d?u
3. Replace b?ng text kh�ng d?u

## TOTAL COUNT
- **Program.cs**: 8 v? tr�
- **Form1.cs**: 65+ v? tr�  
- **ProjectDetailsForm.cs**: 20+ v? tr�
- **ProjectManagementTest.cs**: 30+ v? tr�
- **DatabaseConnectionTest.cs**: 10+ v? tr�
- **SimpleDatabaseTest.cs**: 8 v? tr�

**T?NG: 140+ V? TR� C?N S?A**

## L?U �
- Backup code tr??c khi s?a
- Test sau m?i file s?a xong
- Build ?? check l?i
- C� th? c� th�m file kh�c c?n s?a (LoginForm, RegisterForm, etc.)
