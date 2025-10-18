# H??ng D?n Seed D? Li?u M?u

## ?? T?ng Quan

Database Seeder giúp b?n t?o d? li?u m?u ?? test và demo ch??ng trình ToDoList App.

## ?? D? Li?u M?u Bao G?m

### ?? Users (4 ng??i dùng)
- Nguy?n V?n An (nguyenvanan@example.com)
- Tr?n Th? Bình (tranthibinh@example.com)
- Lê Minh C??ng (leminhcuong@example.com)
- Ph?m Thu Duyên (phamthuduyen@example.com)

### ?? Projects (6 d? án)
1. **Website Redesign** - Thi?t k? l?i giao di?n website công ty
2. **Mobile App Development** - Phát tri?n ?ng d?ng di ??ng
3. **Marketing Campaign Q4** - Chi?n d?ch marketing quý 4
4. **Database Optimization** - T?i ?u hóa database
5. **API Integration** - Tích h?p API bên th? ba
6. **User Research** - Nghiên c?u ng??i dùng và UX

### ? Tasks (20+ công vi?c)
- Các task có ??y ?? thông tin: Title, Description, Priority, Status, DueDate
- Tr?ng thái: Completed, In Progress, Pending
- ?? ?u tiên: High, Medium, Low
- M?t s? task có EstimatedMinutes và ActualMinutes

### ??? Tags (8 tags)
- Urgent (??)
- Work (Xanh d??ng)
- Personal (Xanh lá)
- Meeting (Cam)
- Development (Tím)
- Bug Fix (H?ng)
- Feature (Xanh ng?c)
- Research (Vàng)

### ?? Reminders (5 nh?c nh?)
- Nh?c nh? cho các task quan tr?ng

### ?? User Settings
- Theme preferences (Dark/Light)
- Language (vi/en)
- Daily goals
- Notification settings

### ?? Activity Logs
- L?ch s? ho?t ??ng c?a ng??i dùng

### ?? Focus Sessions
- Các phiên làm vi?c t?p trung v?i ghi chú

## ?? Cách S? D?ng

### Cách 1: Dùng Batch File (Khuy?n ngh?)

1. **M? file `RunDatabaseSeeder.bat`**
   - Double-click vào file trong Explorer

2. **Ch?n hành ??ng:**
   ```
   1. Seed d? li?u m?u (thêm d? li?u m?i)
   2. Xóa t?t c? d? li?u
   3. Xóa và seed l?i (Reset - khuy?n ngh?)
   0. Thoát
   ```

3. **Nh?p s? t??ng ?ng và Enter**

### Cách 2: Dùng Command Line

```bash
# Build project
dotnet build TodoListApp.DAL\TodoListApp.DAL.csproj

# Ch?y seeder
dotnet run --project TodoListApp.DAL\TodoListApp.DAL.csproj
```

### Cách 3: T? Visual Studio

1. Set `TodoListApp.DAL` làm StartUp Project
2. Right-click project ? Set as StartUp Project
3. Nh?n F5 ho?c Ctrl+F5 ?? ch?y

## ?? L?u Ý Quan Tr?ng

### Tr??c Khi Seed

1. **Ki?m tra Connection String**
   - M? file `TodoListApp.DAL\appsettings.json`
   - ??m b?o connection string ?úng v?i SQL Server c?a b?n:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=ToDoListApp;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

2. **??m b?o Database ?ã T?o**
   - Database `ToDoListApp` ph?i t?n t?i
   - Các b?ng ?ã ???c migrate (ch?y migrations)

3. **Backup D? Li?u (N?u C?n)**
   - N?u database ?ã có d? li?u quan tr?ng, hãy backup tr??c

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
   - Ki?m tra xem d? li?u hi?n th? ?úng không

## ?? Chi Ti?t D? Li?u

### Task Status Distribution
- **Completed**: ~30% (các task ?ã hoàn thành)
- **In Progress**: ~35% (?ang th?c hi?n)
- **Pending**: ~35% (ch? th?c hi?n)

### Priority Distribution
- **High**: ~40% 
- **Medium**: ~45%
- **Low**: ~15%

### Time Estimates
- Tasks có EstimatedMinutes t? 90-600 phút
- M?t s? task ?ã có ActualMinutes ?? so sánh

## ?? Troubleshooting

### L?i: "Cannot connect to database"
**Gi?i pháp:**
- Ki?m tra SQL Server ?ang ch?y
- Ki?m tra connection string
- Th? k?t n?i b?ng SQL Server Management Studio

### L?i: "Table does not exist"
**Gi?i pháp:**
- Ch?y migrations tr??c:
  ```bash
  dotnet ef database update --project TodoListApp.DAL
  ```

### L?i: "Duplicate key"
**Gi?i pháp:**
- Database ?ã có d? li?u
- Ch?n option 3 (Xóa và seed l?i) ?? reset

### L?i: Foreign Key Constraint
**Gi?i pháp:**
- Dùng option 3 ?? xóa và seed l?i theo ?úng th? t?
- Seeder s? t? ??ng x? lý dependencies

## ?? Tips

1. **L?n ??u s? d?ng**: Ch?n option 3 (Reset) ?? có d? li?u s?ch

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

4. **Customization**: B?n có th? ch?nh s?a file `DatabaseSeeder.cs` ?? thêm/b?t d? li?u theo ý mu?n

## ?? H? Tr?

N?u g?p v?n ??:
1. Ki?m tra l?i các b??c trong h??ng d?n
2. Xem ph?n Troubleshooting
3. Check console output ?? bi?t l?i c? th?
4. ??m b?o ?ã build project thành công

---

**Chúc b?n test thành công! ??**
