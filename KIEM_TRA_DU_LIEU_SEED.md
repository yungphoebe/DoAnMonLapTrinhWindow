# ?? KI?M TRA D? LI?U SEED - H??NG D?N

## ? V?n ??

Seeder báo thành công nh?ng **không ??ng nh?p ???c** ho?c **không th?y d? li?u**.

---

## ? B??C 1: Ki?m Tra Database Có D? Li?u Ch?a

### M? SQL Server Management Studio (SSMS)

1. Connect t?i `DESKTOP-LN5QDF6\SQLEXPRESS`
2. M? database `ToDoListApp`
3. **Cách 1:** Click **New Query** và ch?y file `CheckSeedData.sql`
4. **Cách 2:** Ch?y query nhanh:

```sql
USE ToDoListApp;

-- Ki?m tra s? l??ng
SELECT 
    'Users' as Table, COUNT(*) as Count FROM Users
UNION ALL
SELECT 'Projects', COUNT(*) FROM Projects
UNION ALL
SELECT 'Tasks', COUNT(*) FROM Tasks;
```

### K?t Qu? Mong ??i:

```
Table       Count
Users       4      ? PH?I LÀ 4!
Projects    6
Tasks       20
```

---

## ? N?U KHÔNG ?ÚNG (Ví d?: Users = 1)

### Nguyên Nhân:

1. **Seeder ch?y sai database** (connection string sai)
2. **Tool ki?m tra c?a b?n k?t n?i sai database**
3. **Seed ch?a hoàn t?t** (b? l?i gi?a ch?ng)

### Gi?i Pháp:

```cmd
ClearAndSeed.bat
```

Ch?n `Y` và ch? k?t qu?.

---

## ? N?U D? LI?U ?ÚNG - V?N ?? ??NG NH?P

### Passwords Trong Seed Data

Seed data s? d?ng **PLAIN TEXT passwords** (KHÔNG hash):

```csharp
User 1: Email = "nguyenvanan@example.com"
        Password = "hashed_password_1"  ? Plain text!

User 2: Email = "tranthibinh@example.com"
        Password = "hashed_password_2"  ? Plain text!

User 3: Email = "leminhcuong@example.com"
        Password = "hashed_password_3"  ? Plain text!

User 4: Email = "phamthuduyen@example.com"
        Password = "hashed_password_4"  ? Plain text!
```

### Cách Test ??ng Nh?p:

**Option 1:** Nh?p password chính xác nh? trong database:

```
Email: nguyenvanan@example.com
Password: hashed_password_1
```

**Option 2:** S?a code ??ng nh?p ?? **KHÔNG hash** password khi test:

```csharp
// Thay vì:
if (user.PasswordHash == HashPassword(inputPassword))

// Dùng:
if (user.PasswordHash == inputPassword)
```

---

## ?? S?A SEED DATA ?? DÙNG PASSWORD TH?T

N?u b?n mu?n dùng password **d? nh? h?n**, s?a `DatabaseSeeder.cs`:

```csharp
new User
{
    FullName = "Nguy?n V?n An",
    Email = "nguyenvanan@example.com",
    PasswordHash = "123456",  // ? Password ??n gi?n ?? test
    ...
}
```

Sau ?ó ch?y l?i:
```cmd
ClearAndSeed.bat
```

---

## ?? KI?M TRA THÔNG TIN USERS

Ch?y query này ?? xem thông tin ??ng nh?p:

```sql
USE ToDoListApp;

SELECT 
    UserID,
    FullName,
    Email,
    PasswordHash as 'Password ?? test'
FROM Users
ORDER BY UserID;
```

K?t qu? s? cho b?n bi?t **chính xác** email và password ?? test!

---

## ?? TÓM T?T - CHECKLIST

- [ ] **1. Ch?y `CheckSeedData.sql` trong SSMS**
- [ ] **2. Ki?m tra: Users = 4, Projects = 6, Tasks = 20**
- [ ] **3. N?u sai ? Ch?y `ClearAndSeed.bat`**
- [ ] **4. Copy email và password t? query**
- [ ] **5. Test ??ng nh?p v?i thông tin ?ó**
- [ ] **6. N?u v?n không ???c ? Xem code ??ng nh?p c?a b?n**

---

## ?? G?I CHO TÔI

N?u v?n không ???c, hãy g?i cho tôi:

1. **Screenshot k?t qu? query `CheckSeedData.sql`**
2. **Code form ??ng nh?p c?a b?n** (file LoginForm.cs ho?c t??ng t?)
3. **Error message c? th?** khi ??ng nh?p

Tôi s? giúp b?n fix chính xác!

---

**Files c?n ch?y:**
- `CheckSeedData.sql` - Ki?m tra d? li?u trong SSMS
- `ClearAndSeed.bat` - Seed l?i n?u c?n
