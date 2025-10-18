# H??ng D?n Seed D? Li?u M?u

## ?? T?ng Quan

Database Seeder gi�p b?n t?o d? li?u m?u ?? test v� demo ch??ng tr�nh ToDoList App.

## ?? D? Li?u M?u Bao G?m

### ?? Users (4 ng??i d�ng)
- Nguy?n V?n An (nguyenvanan@example.com)
- Tr?n Th? B�nh (tranthibinh@example.com)
- L� Minh C??ng (leminhcuong@example.com)
- Ph?m Thu Duy�n (phamthuduyen@example.com)

### ?? Projects (6 d? �n)
1. **Website Redesign** - Thi?t k? l?i giao di?n website c�ng ty
2. **Mobile App Development** - Ph�t tri?n ?ng d?ng di ??ng
3. **Marketing Campaign Q4** - Chi?n d?ch marketing qu� 4
4. **Database Optimization** - T?i ?u h�a database
5. **API Integration** - T�ch h?p API b�n th? ba
6. **User Research** - Nghi�n c?u ng??i d�ng v� UX

### ? Tasks (20+ c�ng vi?c)
- C�c task c� ??y ?? th�ng tin: Title, Description, Priority, Status, DueDate
- Tr?ng th�i: Completed, In Progress, Pending
- ?? ?u ti�n: High, Medium, Low
- M?t s? task c� EstimatedMinutes v� ActualMinutes

### ??? Tags (8 tags)
- Urgent (??)
- Work (Xanh d??ng)
- Personal (Xanh l�)
- Meeting (Cam)
- Development (T�m)
- Bug Fix (H?ng)
- Feature (Xanh ng?c)
- Research (V�ng)

### ?? Reminders (5 nh?c nh?)
- Nh?c nh? cho c�c task quan tr?ng

### ?? User Settings
- Theme preferences (Dark/Light)
- Language (vi/en)
- Daily goals
- Notification settings

### ?? Activity Logs
- L?ch s? ho?t ??ng c?a ng??i d�ng

### ?? Focus Sessions
- C�c phi�n l�m vi?c t?p trung v?i ghi ch�

## ?? C�ch S? D?ng

### C�ch 1: D�ng Batch File (Khuy?n ngh?)

1. **M? file `RunDatabaseSeeder.bat`**
   - Double-click v�o file trong Explorer

2. **Ch?n h�nh ??ng:**
   ```
   1. Seed d? li?u m?u (th�m d? li?u m?i)
   2. X�a t?t c? d? li?u
   3. X�a v� seed l?i (Reset - khuy?n ngh?)
   0. Tho�t
   ```

3. **Nh?p s? t??ng ?ng v� Enter**

### C�ch 2: D�ng Command Line

```bash
# Build project
dotnet build TodoListApp.DAL\TodoListApp.DAL.csproj

# Ch?y seeder
dotnet run --project TodoListApp.DAL\TodoListApp.DAL.csproj
```

### C�ch 3: T? Visual Studio

1. Set `TodoListApp.DAL` l�m StartUp Project
2. Right-click project ? Set as StartUp Project
3. Nh?n F5 ho?c Ctrl+F5 ?? ch?y

## ?? L?u � Quan Tr?ng

### Tr??c Khi Seed

1. **Ki?m tra Connection String**
   - M? file `TodoListApp.DAL\appsettings.json`
   - ??m b?o connection string ?�ng v?i SQL Server c?a b?n:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=ToDoListApp;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

2. **??m b?o Database ?� T?o**
   - Database `ToDoListApp` ph?i t?n t?i
   - C�c b?ng ?� ???c migrate (ch?y migrations)

3. **Backup D? Li?u (N?u C?n)**
   - N?u database ?� c� d? li?u quan tr?ng, h�y backup tr??c

### Sau Khi Seed

1. **Ki?m tra d? li?u**
   - M? SQL Server Management Studio
   - Query ?? xem d? li?u:
   ```sql
   SELECT * FROM Users
   SELECT * FROM Projects
   SELECT * FROM Tasks
   SELECT * FROM Tags
   ```

2. **Test ?ng d?ng**
   - Ch?y ToDoList.GUI
   - Ki?m tra xem d? li?u hi?n th? ?�ng kh�ng

## ?? Chi Ti?t D? Li?u

### Task Status Distribution
- **Completed**: ~30% (c�c task ?� ho�n th�nh)
- **In Progress**: ~35% (?ang th?c hi?n)
- **Pending**: ~35% (ch? th?c hi?n)

### Priority Distribution
- **High**: ~40% 
- **Medium**: ~45%
- **Low**: ~15%

### Time Estimates
- Tasks c� EstimatedMinutes t? 90-600 ph�t
- M?t s? task ?� c� ActualMinutes ?? so s�nh

## ?? Troubleshooting

### L?i: "Cannot connect to database"
**Gi?i ph�p:**
- Ki?m tra SQL Server ?ang ch?y
- Ki?m tra connection string
- Th? k?t n?i b?ng SQL Server Management Studio

### L?i: "Table does not exist"
**Gi?i ph�p:**
- Ch?y migrations tr??c:
  ```bash
  dotnet ef database update --project TodoListApp.DAL
  ```

### L?i: "Duplicate key"
**Gi?i ph�p:**
- Database ?� c� d? li?u
- Ch?n option 3 (X�a v� seed l?i) ?? reset

### L?i: Foreign Key Constraint
**Gi?i ph�p:**
- D�ng option 3 ?? x�a v� seed l?i theo ?�ng th? t?
- Seeder s? t? ??ng x? l� dependencies

## ?? Tips

1. **L?n ??u s? d?ng**: Ch?n option 3 (Reset) ?? c� d? li?u s?ch

2. **Test nhi?u l?n**: Ch?n option 2 (Clear) tr??c khi seed l?i

3. **Ki?m tra nhanh**: Sau khi seed, ch?y query:
   ```sql
   SELECT 
       (SELECT COUNT(*) FROM Users) as Users,
       (SELECT COUNT(*) FROM Projects) as Projects,
       (SELECT COUNT(*) FROM Tasks) as Tasks,
       (SELECT COUNT(*) FROM Tags) as Tags
   ```
   K?t qu? mong ??i: Users=4, Projects=6, Tasks=20, Tags=8

4. **Customization**: B?n c� th? ch?nh s?a file `DatabaseSeeder.cs` ?? th�m/b?t d? li?u theo � mu?n

## ?? H? Tr?

N?u g?p v?n ??:
1. Ki?m tra l?i c�c b??c trong h??ng d?n
2. Xem ph?n Troubleshooting
3. Check console output ?? bi?t l?i c? th?
4. ??m b?o ?� build project th�nh c�ng

---

**Ch�c b?n test th�nh c�ng! ??**
