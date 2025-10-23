# ? H??NG D?N S?A L?I HI?N TH? "Nguy?n V?n An"

## ?? V?N ??
Ch? "Nguy?n V?n An" hi?n th? thành **"Nguy?n V?n An!"** - l?i encoding font

## ? GI?I PHÁP ??N GI?N NH?T

### **Ph??ng án 1: Ch?y Script T? ??ng (KHUY?N NGH?)**

1. **Double-click file:**
   ```
   fix_form1_vietnamese.ps1
   ```
   
2. **Ho?c n?u b? ch?n, ch?y:**
   ```
   quick_fix.bat
   ```

3. **Script s? t? ??ng:**
   - ? Backup file g?c
   - ? ??i font sang Arial
   - ? Chuy?n text sang không d?u
   - ? L?u l?i file

### **Ph??ng án 2: S?a Th? Công (N?u script l?i)**

**M? file:** `ToDoList.GUI\Form1.cs`

**Tìm và thay th? (Ctrl + H):**

#### 1. S?a method `UpdateGreetingLabels()` (dòng 41-48):

**TÌM:**
```csharp
private void UpdateGreetingLabels()
{
    string timeOfDay = GetTimeOfDay();
    lblGreeting.Text = $"Chào {timeOfDay}, {UserSession.GetDisplayName()}!";
    lblUserName.Text = "Tuy?t v?i! B?n ?ang làm vi?c r?t ch?m ch?.";
}
```

**THAY B?NG:**
```csharp
private void UpdateGreetingLabels()
{
    string timeOfDay = GetTimeOfDay();
    
    // Use Arial font for Vietnamese support
    lblGreeting.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point);
    lblUserName.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point);
    
    lblGreeting.Text = $"Chao {timeOfDay}, {UserSession.GetDisplayName()}!";
    lblUserName.Text = "Tuyet voi! Ban dang lam viec rat cham chi.";
}
```

#### 2. S?a method `GetTimeOfDay()` (dòng 50-58):

**TÌM và THAY:**
- `"bu?i sáng"` ? `"buoi sang"`
- `"bu?i chi?u"` ? `"buoi chieu"`
- `"bu?i t?i"` ? `"buoi toi"`

#### 3. S?a các text khác (Find & Replace All):

| Tìm | Thay |
|-----|------|
| Báo cáo | Bao cao |
| Trang ch? | Trang chu |
| Công vi?c | Cong viec |
| D? án | Du an |
| Không th? | Khong the |
| L?i | Loi |
| D? li?u | Du lieu |
| Th?ng kê | Thong ke |
| công vi?c ?ang ch? | cong viec dang cho |
| D? ki?n | Du kien |
| T?O DANH SÁCH M?I | TAO DANH SACH MOI |
| Ch?nh s?a | Chinh sua |
| Xem chi ti?t | Xem chi tiet |
| L?u tr? | Luu tru |
| Xóa | Xoa |
| Thông báo | Thong bao |
| Xác nh?n | Xac nhan |
| Thành công | Thanh cong |

## ?? KI?M TRA SAU KHI S?A

### 1. Build l?i
```
Ctrl + Shift + B
ho?c
Build > Rebuild Solution
```

### 2. Ch?y ?ng d?ng
```
F5 (Start Debugging)
```

### 3. Ki?m tra màn hình chính
- ? Ch? "Chao buoi chieu, Nguyen Van An!" ph?i hi?n th? rõ ràng
- ? KHÔNG có d?u ? hay ký t? l?i
- ? Font ch? d? ??c (Arial)

## ?? T?I SAO PH?I ??I FONT?

| Font | Hỗ trợ tiếng việt| Kết QUả |
|------|-------------------|---------|
| Segoe UI | ? Không t?t | Nguyễn Văn An |
| Arial | ? T?t | Nguyen Van An ? |
| Tahoma | ? T?t | Nguyen Van An ? |
| Verdana | ? T?t | Nguyen Van An ? |

**=> ??i font t? "Segoe UI" ? "Arial"**

## ?? CÁC FILE ?Ã T?O

1. **fix_form1_vietnamese.ps1** - Script PowerShell t? ??ng s?a
2. **quick_fix.bat** - File batch ch?y script d? dàng
3. **FIX_LOI_TEN_NGUOI_DUNG_MANUAL.md** - File này (h??ng d?n th? công)

## ?? N?U V?N L?I SAU KHI S?A

### Ki?m tra file backup:
```
ToDoList.GUI\Form1.cs.backup
```

### Khôi ph?c n?u c?n:
1. Xóa file Form1.cs
2. Rename Form1.cs.backup ? Form1.cs
3. Th? l?i

### Ho?c liên h? support v?i thông tin:
- Screenshot màn hình l?i
- N?i dung file Form1.cs (dòng 41-58)
- Version Visual Studio ?ang dùng

## ? K?T QU? MONG ??I

Sau khi fix xong:

```
Màn hình chính:
????????????????????????????????????????????
?  Chao buoi chieu, Nguyen Van An!     ?  ?
?  Tuyet voi! Ban dang lam viec rat...  ?  ?
????????????????????????????????????????????
```

**KHÔNG còn ký t? ? hay l?i encoding!** ??
