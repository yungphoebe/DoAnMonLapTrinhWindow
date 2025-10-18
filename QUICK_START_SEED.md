# ?? Quick Start - Seed D? Li?u M?u

## Cách Nhanh Nh?t

1. **Double-click file `RunDatabaseSeeder.bat`**

2. **Ch?n option 3** (Xóa và seed l?i)

3. **Gõ `yes`** ?? xác nh?n

4. **Xong!** ??

---

## K?t Qu? Mong ??i

Sau khi seed thành công, b?n s? có:

? **4 Users** - Ng??i dùng m?u  
? **6 Projects** - D? án ?a d?ng  
? **20+ Tasks** - Công vi?c v?i nhi?u tr?ng thái  
? **8 Tags** - Nhãn màu s?c  
? **5 Reminders** - Nh?c nh?  
? **5 Focus Sessions** - Phiên làm vi?c  
? **Activity Logs** - L?ch s? ho?t ??ng  

---

## Test Ngay

Sau khi seed xong:

1. Ch?y ?ng d?ng **ToDoList.GUI**
2. B?n s? th?y các projects và tasks ?ã ???c t?o s?n
3. Test các ch?c n?ng: thêm/s?a/xóa task, filter, search, reports...

---

## ?? N?u G?p L?i

### "Cannot connect to database"
?? M? `TodoListApp.DAL\appsettings.json`  
?? S?a connection string cho ?úng v?i SQL Server c?a b?n

### "Table does not exist"  
?? Ch?y migrations tr??c:
```bash
dotnet ef database update --project TodoListApp.DAL
```

---

**Chi ti?t ??y ??**: Xem file `HUONG_DAN_SEED_DATABASE.md`
