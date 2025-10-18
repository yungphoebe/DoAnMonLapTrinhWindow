# ?? Seed D? Li?u M?u - ToDoList App

## ?? T?ng Quan

Repository này cung c?p **3 cách** ?? seed d? li?u m?u vào database ToDoListApp:

1. ? **Quick Batch** - Nhanh nh?t, t? ??ng (Khuy?n ngh?)
2. ??? **Interactive Batch** - Có menu l?a ch?n
3. ?? **SQL Script** - Ch?y tr?c ti?p trong SSMS

---

## ?? Cách 1: Quick Seed (Khuy?n ngh?)

### Cách dùng:
```
1. Double-click: QuickSeedDatabase.bat
2. Nh?n Y ?? xác nh?n
3. Ch? 10-20 giây
4. Xong! ??
```

**?u ?i?m:**
- ? Nhanh nh?t, t? ??ng hoàn toàn
- ?? T? ??ng xóa và seed l?i
- ? Không c?n gõ gì thêm

**File:** `QuickSeedDatabase.bat`

---

## ??? Cách 2: Interactive Seeder

### Cách dùng:
```
1. Double-click: RunDatabaseSeeder.bat
2. Ch?n option:
   1 - Seed d? li?u m?i (gi? d? li?u c?)
   2 - Xóa t?t c? d? li?u
   3 - Xóa và seed l?i (khuy?n ngh?)
   0 - Thoát
3. Nh?p l?a ch?n và Enter
4. Xác nh?n n?u c?n
```

**?u ?i?m:**
- ?? Linh ho?t, nhi?u tùy ch?n
- ?? Xem t?ng b??c ?ang làm gì
- ?? Ki?m soát t?t h?n

**File:** `RunDatabaseSeeder.bat`

---

## ?? Cách 3: SQL Script Manual

### Cách dùng:
```
1. M? SQL Server Management Studio (SSMS)
2. Connect t?i SQL Server
3. M? file: SeedData_Manual.sql
4. Ch?n database ToDoListApp
5. Nh?n Execute (F5)
```

**?u ?i?m:**
- ?? Ki?m soát hoàn toàn
- ??? Xem rõ t?ng câu SQL
- ?? D? customize

**File:** `SeedData_Manual.sql`

---

## ?? D? Li?u ???c Seed

| Lo?i D? Li?u | S? L??ng | Mô T? |
|---------------|----------|-------|
| **Users** | 4 | Ng??i dùng m?u |
| **Projects** | 6 | D? án ?a d?ng |
| **Tasks** | 20 | Tasks v?i nhi?u tr?ng thái |
| **Tags** | 8 | Nhãn có màu s?c |
| **TaskTags** | 15 | Liên k?t Task-Tag |
| **Reminders** | 5 | Nh?c nh? |
| **UserSettings** | 4 | C?u hình ng??i dùng |
| **ActivityLogs** | 5 | L?ch s? ho?t ??ng |
| **FocusSessions** | 5 | Phiên làm vi?c |

### Chi ti?t Users:
```
1. Nguy?n V?n An (nguyenvanan@example.com)
2. Tr?n Th? Bình (tranthibinh@example.com)
3. Lê Minh C??ng (leminhcuong@example.com)
4. Ph?m Thu Duyên (phamthuduyen@example.com)
```

### Chi ti?t Projects:
```
1. Website Redesign
2. Mobile App Development
3. Marketing Campaign Q4
4. Database Optimization
5. API Integration
6. User Research
```

### Task Status Distribution:
- ? **Completed**: 30%
- ?? **In Progress**: 35%
- ?? **Pending**: 35%

---

## ?? Yêu C?u H? Th?ng

### Tr??c khi Seed:

1. **SQL Server ?ang ch?y**
2. **Database ?ã t?o:**
   ```sql
   CREATE DATABASE ToDoListApp;
   ```
3. **?ã ch?y migrations:**
   ```bash
   dotnet ef database update --project TodoListApp.DAL
   ```
4. **Connection string ?úng** trong `TodoListApp.DAL\appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=ToDoListApp;..."
     }
   }
   ```

---

## ?? Ki?m Tra K?t Qu?

### Sau khi seed, ch?y query này:
```sql
USE ToDoListApp;
GO

SELECT 
    'Users' as TableName, COUNT(*) as Count FROM Users
UNION ALL
SELECT 'Projects', COUNT(*) FROM Projects
UNION ALL
SELECT 'Tasks', COUNT(*) FROM Tasks
UNION ALL
SELECT 'Tags', COUNT(*) FROM Tags
UNION ALL
SELECT 'TaskTags', COUNT(*) FROM TaskTags
UNION ALL
SELECT 'Reminders', COUNT(*) FROM Reminders
UNION ALL
SELECT 'UserSettings', COUNT(*) FROM UserSettings
UNION ALL
SELECT 'ActivityLog', COUNT(*) FROM ActivityLog
UNION ALL
SELECT 'FocusSessions', COUNT(*) FROM FocusSessions;
```

### K?t qu? mong ??i:
```
TableName       Count
Users           4
Projects        6
Tasks           20
Tags            8
TaskTags        15
Reminders       5
UserSettings    4
ActivityLog     5
FocusSessions   5
```

---

## ? Troubleshooting

### "Cannot connect to database"
**Nguyên nhân:** Connection string sai ho?c SQL Server không ch?y

**Gi?i pháp:**
1. Ki?m tra SQL Server ?ang ch?y (SQL Server Configuration Manager)
2. S?a connection string trong `appsettings.json`
3. Test k?t n?i b?ng SSMS

---

### "Table does not exist"
**Nguyên nhân:** Ch?a ch?y migrations

**Gi?i pháp:**
```bash
dotnet ef database update --project TodoListApp.DAL
```

---

### "Duplicate key error"
**Nguyên nhân:** Database ?ã có d? li?u v?i ID trùng

**Gi?i pháp:**
- Dùng **Cách 1** ho?c **Cách 2** v?i option 3 (Xóa và seed l?i)
- Ho?c ch?y SQL: `DELETE FROM [TableName]` cho t?t c? tables

---

### Build failed
**Nguyên nhân:** Thi?u dependencies ho?c code l?i

**Gi?i pháp:**
```bash
dotnet restore
dotnet build
```

---

## ?? Tài Li?u Liên Quan

- `QUICK_START_SEED.md` - H??ng d?n nhanh
- `HUONG_DAN_SEED_DATABASE.md` - H??ng d?n chi ti?t
- `SeedData_Manual.sql` - SQL script ?? seed manual

---

## ?? Code Structure

```
?? TodoListApp.DAL/
??? ?? Data/
?   ??? DatabaseSeeder.cs      # Core seeder logic
??? ?? Models/
?   ??? *.cs                   # Entity models
??? SeedProgram.cs             # Console app entry point
??? appsettings.json           # Connection string

?? QuickSeedDatabase.bat       # Quick auto seed
?? RunDatabaseSeeder.bat       # Interactive seed
?? SeedData_Manual.sql         # Manual SQL seed
```

---

## ?? Tips

1. **L?n ??u dùng?** ? Ch?n **Cách 1** (QuickSeedDatabase.bat)
2. **Mu?n ki?m soát?** ? Ch?n **Cách 2** (RunDatabaseSeeder.bat)
3. **Bi?t SQL?** ? Ch?n **Cách 3** (SeedData_Manual.sql)
4. **Seed nhi?u l?n?** ? Luôn dùng option "Xóa và seed l?i" ?? tránh l?i

---

## ?? Sau Khi Seed

1. ? Ch?y ToDoList.GUI app
2. ? Login v?i m?t trong 4 users (email ? trên)
3. ? Test các ch?c n?ng
4. ? Xem d? li?u m?u ?ã ???c t?o

---

**Happy Coding! ??**
