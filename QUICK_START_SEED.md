# ?? Quick Start - Seed D? Li?u M?u

## C�ch Nhanh Nh?t

1. **Double-click file `RunDatabaseSeeder.bat`**

2. **Ch?n option 3** (X�a v� seed l?i)

3. **G� `yes`** ?? x�c nh?n

4. **Xong!** ??

---

## K?t Qu? Mong ??i

Sau khi seed th�nh c�ng, b?n s? c�:

? **4 Users** - Ng??i d�ng m?u  
? **6 Projects** - D? �n ?a d?ng  
? **20+ Tasks** - C�ng vi?c v?i nhi?u tr?ng th�i  
? **8 Tags** - Nh�n m�u s?c  
? **5 Reminders** - Nh?c nh?  
? **5 Focus Sessions** - Phi�n l�m vi?c  
? **Activity Logs** - L?ch s? ho?t ??ng  

---

## Test Ngay

Sau khi seed xong:

1. Ch?y ?ng d?ng **ToDoList.GUI**
2. B?n s? th?y c�c projects v� tasks ?� ???c t?o s?n
3. Test c�c ch?c n?ng: th�m/s?a/x�a task, filter, search, reports...

---

## ?? N?u G?p L?i

### "Cannot connect to database"
?? M? `TodoListApp.DAL\appsettings.json`  
?? S?a connection string cho ?�ng v?i SQL Server c?a b?n

### "Table does not exist"  
?? Ch?y migrations tr??c:
```bash
dotnet ef database update --project TodoListApp.DAL
```

---

**Chi ti?t ??y ??**: Xem file `HUONG_DAN_SEED_DATABASE.md`
