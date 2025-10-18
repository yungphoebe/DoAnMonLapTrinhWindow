# ?? Seed D? Li?u M?u - ToDoList App

## ?? T?ng Quan

Repository n�y cung c?p **3 c�ch** ?? seed d? li?u m?u v�o database ToDoListApp:

1. ? **Quick Batch** - Nhanh nh?t, t? ??ng (Khuy?n ngh?)
2. ??? **Interactive Batch** - C� menu l?a ch?n
3. ?? **SQL Script** - Ch?y tr?c ti?p trong SSMS

---

## ?? C�ch 1: Quick Seed (Khuy?n ngh?)

### C�ch d�ng:
```
1. Double-click: QuickSeedDatabase.bat
2. Nh?n Y ?? x�c nh?n
3. Ch? 10-20 gi�y
4. Xong! ??
```

**?u ?i?m:**
- ? Nhanh nh?t, t? ??ng ho�n to�n
- ?? T? ??ng x�a v� seed l?i
- ? Kh�ng c?n g� g� th�m

**File:** `QuickSeedDatabase.bat`

---

## ??? C�ch 2: Interactive Seeder

### C�ch d�ng:
```
1. Double-click: RunDatabaseSeeder.bat
2. Ch?n option:
   1 - Seed d? li?u m?i (gi? d? li?u c?)
   2 - X�a t?t c? d? li?u
   3 - X�a v� seed l?i (khuy?n ngh?)
   0 - Tho�t
3. Nh?p l?a ch?n v� Enter
4. X�c nh?n n?u c?n
```

**?u ?i?m:**
- ?? Linh ho?t, nhi?u t�y ch?n
- ?? Xem t?ng b??c ?ang l�m g�
- ?? Ki?m so�t t?t h?n

**File:** `RunDatabaseSeeder.bat`

---

## ?? C�ch 3: SQL Script Manual

### C�ch d�ng:
```
1. M? SQL Server Management Studio (SSMS)
2. Connect t?i SQL Server
3. M? file: SeedData_Manual.sql
4. Ch?n database ToDoListApp
5. Nh?n Execute (F5)
```

**?u ?i?m:**
- ?? Ki?m so�t ho�n to�n
- ??? Xem r� t?ng c�u SQL
- ?? D? customize

**File:** `SeedData_Manual.sql`

---

## ?? D? Li?u ???c Seed

| Lo?i D? Li?u | S? L??ng | M� T? |
|---------------|----------|-------|
| **Users** | 4 | Ng??i d�ng m?u |
| **Projects** | 6 | D? �n ?a d?ng |
| **Tasks** | 20 | Tasks v?i nhi?u tr?ng th�i |
| **Tags** | 8 | Nh�n c� m�u s?c |
| **TaskTags** | 15 | Li�n k?t Task-Tag |
| **Reminders** | 5 | Nh?c nh? |
| **UserSettings** | 4 | C?u h�nh ng??i d�ng |
| **ActivityLogs** | 5 | L?ch s? ho?t ??ng |
| **FocusSessions** | 5 | Phi�n l�m vi?c |

### Chi ti?t Users:
```
1. Nguy?n V?n An (nguyenvanan@example.com)
2. Tr?n Th? B�nh (tranthibinh@example.com)
3. L� Minh C??ng (leminhcuong@example.com)
4. Ph?m Thu Duy�n (phamthuduyen@example.com)
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

## ?? Y�u C?u H? Th?ng

### Tr??c khi Seed:

1. **SQL Server ?ang ch?y**
2. **Database ?� t?o:**
   ```sql
   CREATE DATABASE ToDoListApp;
   ```
3. **?� ch?y migrations:**
   ```bash
   dotnet ef database update --project TodoListApp.DAL
   ```
4. **Connection string ?�ng** trong `TodoListApp.DAL\appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=ToDoListApp;..."
     }
   }
   ```

---

## ?? Ki?m Tra K?t Qu?

### Sau khi seed, ch?y query n�y:
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
**Nguy�n nh�n:** Connection string sai ho?c SQL Server kh�ng ch?y

**Gi?i ph�p:**
1. Ki?m tra SQL Server ?ang ch?y (SQL Server Configuration Manager)
2. S?a connection string trong `appsettings.json`
3. Test k?t n?i b?ng SSMS

---

### "Table does not exist"
**Nguy�n nh�n:** Ch?a ch?y migrations

**Gi?i ph�p:**
```bash
dotnet ef database update --project TodoListApp.DAL
```

---

### "Duplicate key error"
**Nguy�n nh�n:** Database ?� c� d? li?u v?i ID tr�ng

**Gi?i ph�p:**
- D�ng **C�ch 1** ho?c **C�ch 2** v?i option 3 (X�a v� seed l?i)
- Ho?c ch?y SQL: `DELETE FROM [TableName]` cho t?t c? tables

---

### Build failed
**Nguy�n nh�n:** Thi?u dependencies ho?c code l?i

**Gi?i ph�p:**
```bash
dotnet restore
dotnet build
```

---

## ?? T�i Li?u Li�n Quan

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

1. **L?n ??u d�ng?** ? Ch?n **C�ch 1** (QuickSeedDatabase.bat)
2. **Mu?n ki?m so�t?** ? Ch?n **C�ch 2** (RunDatabaseSeeder.bat)
3. **Bi?t SQL?** ? Ch?n **C�ch 3** (SeedData_Manual.sql)
4. **Seed nhi?u l?n?** ? Lu�n d�ng option "X�a v� seed l?i" ?? tr�nh l?i

---

## ?? Sau Khi Seed

1. ? Ch?y ToDoList.GUI app
2. ? Login v?i m?t trong 4 users (email ? tr�n)
3. ? Test c�c ch?c n?ng
4. ? Xem d? li?u m?u ?� ???c t?o

---

**Happy Coding! ??**
