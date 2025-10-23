# ?? FIX ADVANCED REPORTS - QUICK START

## ? **CÁCH FIX NHANH NH?T (5 B??C)**

### **B??C 1: Ch?y Script PowerShell**

**M? PowerShell t?i th? m?c solution:**
```powershell
cd C:\Users\ACER\source\repos\DoAnMonLapTrinhWindow
.\fix_advanced_reports_auto.ps1
```

**HO?C: Right-click file `fix_advanced_reports_auto.ps1` ? "Run with PowerShell"**

### **B??C 2: Build Solution**
```
Ctrl + Shift + B
```
Ho?c: **Build > Rebuild Solution**

### **B??C 3: Ch?y ?ng D?ng**
```
F5
```

### **B??C 4: M? Advanced Reports**
- Click nút **"?? Báo cáo"** (top menu)
- Ho?c: Click menu project ? **"?? Th?ng kê nâng cao"**

### **B??C 5: Ki?m Tra**

? **Ph?i th?y:**
```
Title bar: "Thong ke nang cao - ToDoList Analytics"
Stats cards: "Tong Projects", "Tong Tasks", "Hoan thanh", "Thoi gian"
Sections: "Hoat dong gan day", "Tien do theo du an"
```

? **KHÔNG ???c th?y:**
```
"?? Th?ng kê..." (d?u h?i l?i encoding)
"T?ng Projects" (d?u h?i trong text)
```

---

## ?? **N?U SCRIPT B? CH?N:**

### **Cách 1: Unblock Script**
```powershell
Unblock-File -Path .\fix_advanced_reports_auto.ps1
.\fix_advanced_reports_auto.ps1
```

### **Cách 2: Bypass Execution Policy**
```powershell
PowerShell -ExecutionPolicy Bypass -File .\fix_advanced_reports_auto.ps1
```

### **Cách 3: Set Policy (Admin PowerShell)**
```powershell
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
.\fix_advanced_reports_auto.ps1
```

---

## ?? **N?U V?N L?I - S?A TH? CÔNG:**

### **File: `ToDoList.GUI\Forms\AdvancedReportsForm.cs`**

#### **1. S?a Title (Line ~35):**
```csharp
// TÌM:
this.Text = "Th?ng kê nâng cao - ToDoList Analytics";

// THAY:
this.Text = "Thong ke nang cao - ToDoList Analytics";
```

#### **2. S?a Tab Names (~50-55):**
```csharp
// TÌM:
new TabPage("T?ng quan")
new TabPage("Bi?u ??")
new TabPage("Th?ng kê")

// THAY:
new TabPage("Tong quan")
new TabPage("Bieu do")
new TabPage("Thong ke")
```

#### **3. S?a Stats Cards (~100-120):**
```csharp
// TÌM:
"T?ng Projects" ? THAY: "Tong Projects"
"T?ng Tasks" ? THAY: "Tong Tasks"
"Hoàn thành" ? THAY: "Hoan thanh"
"Th?i gian" ? THAY: "Thoi gian"
```

#### **4. S?a Section Headers (~150-180):**
```csharp
// TÌM:
"Ho?t ??ng g?n ?ây" ? THAY: "Hoat dong gan day"
"Ti?n ?? theo d? án" ? THAY: "Tien do theo du an"
```

#### **5. ??i Font Arial (~200+):**
```csharp
// TÌM:
new Font("Segoe UI", 18F, FontStyle.Bold)

// THAY:
new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point)
```

---

## ?? **TROUBLESHOOTING:**

### **V?n ??: Bi?u ?? không hi?n th?**

**Nguyên nhân:** Chart control ch?a ???c kh?i t?o ?úng

**Gi?i pháp:** Tìm dòng:
```csharp
var chart = new InteractiveChartControl();
```

Thay b?ng:
```csharp
var chart = new InteractiveChartControl
{
    Dock = DockStyle.Fill,
    BackColor = Color.FromArgb(25, 25, 25),
    Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point)
};
```

### **V?n ??: V?n th?y d?u "?"**

**Nguyên nhân:** File encoding không ph?i UTF-8

**Gi?i pháp:**
1. M? file trong Visual Studio
2. File ? Advanced Save Options
3. Ch?n: **UTF-8 with signature - Codepage 65001**
4. Save
5. Build l?i

### **V?n ??: Script PowerShell không ch?y**

**Nguyên nhân:** Execution Policy b? ch?n

**Gi?i pháp:**
```powershell
# M? PowerShell as Admin
Set-ExecutionPolicy RemoteSigned -Scope LocalMachine

# Ch?y l?i script
.\fix_advanced_reports_auto.ps1
```

---

## ?? **K?T QU? MONG ??I:**

### **Title Bar:**
```
Thong ke nang cao - ToDoList Analytics
```

### **Stats Cards:**
```
????????????????  ????????????????  ????????????????  ????????????????
? Tong Projects?  ? Tong Tasks   ?  ? Hoan thanh   ?  ? Thoi gian    ?
?      6       ?  ?     20       ?  ?    30.0%     ?  ?   34.2h      ?
?  projects    ?  ?   tasks      ?  ? completion   ?  ?   hours      ?
????????????????  ????????????????  ????????????????  ????????????????
```

### **Sections:**
```
Hoat dong gan day             Tien do theo du an
?????????????????             ??????????????????
[List of tasks]               [Progress bars]
```

### **Charts:**
- ? Bi?u ?? c?t hi?n th?
- ? Bi?u ?? ???ng hi?n th?
- ? Bi?u ?? tròn hi?n th?
- ? Zoom b?ng chu?t ho?t ??ng
- ? Pan b?ng drag ho?t ??ng
- ? Tooltip khi hover ho?t ??ng

---

## ? **CHECKLIST:**

- [ ] Script ?ã ch?y thành công
- [ ] Build successful (không có errors)
- [ ] Form m? ???c (không crash)
- [ ] Title bar ?úng (không có ??)
- [ ] Stats cards ?úng (6, 20, 30.0%, 34.2h)
- [ ] Sections header ?úng
- [ ] Bi?u ?? hi?n th? ??y ??
- [ ] Zoom/Pan charts ho?t ??ng
- [ ] Không có l?i encoding nào

---

## ?? **NHANH CHÓNG:**

```powershell
# All in one command:
cd C:\Users\ACER\source\repos\DoAnMonLapTrinhWindow
.\fix_advanced_reports_auto.ps1
# ? Ch? "COMPLETED!"
# ? Ctrl+Shift+B (Build)
# ? F5 (Run)
# ? Test Advanced Reports
```

**Th?i gian: < 2 phút!** ?

---

N?u c?n tr? giúp, check file: `FIX_ADVANCED_REPORTS_COMPLETE_GUIDE.md`
