# ?? FIX ADVANCED REPORTS - QUICK START

## ? **C�CH FIX NHANH NH?T (5 B??C)**

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
- Click n�t **"?? B�o c�o"** (top menu)
- Ho?c: Click menu project ? **"?? Th?ng k� n�ng cao"**

### **B??C 5: Ki?m Tra**

? **Ph?i th?y:**
```
Title bar: "Thong ke nang cao - ToDoList Analytics"
Stats cards: "Tong Projects", "Tong Tasks", "Hoan thanh", "Thoi gian"
Sections: "Hoat dong gan day", "Tien do theo du an"
```

? **KH�NG ???c th?y:**
```
"?? Th?ng k�..." (d?u h?i l?i encoding)
"T?ng Projects" (d?u h?i trong text)
```

---

## ?? **N?U SCRIPT B? CH?N:**

### **C�ch 1: Unblock Script**
```powershell
Unblock-File -Path .\fix_advanced_reports_auto.ps1
.\fix_advanced_reports_auto.ps1
```

### **C�ch 2: Bypass Execution Policy**
```powershell
PowerShell -ExecutionPolicy Bypass -File .\fix_advanced_reports_auto.ps1
```

### **C�ch 3: Set Policy (Admin PowerShell)**
```powershell
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
.\fix_advanced_reports_auto.ps1
```

---

## ?? **N?U V?N L?I - S?A TH? C�NG:**

### **File: `ToDoList.GUI\Forms\AdvancedReportsForm.cs`**

#### **1. S?a Title (Line ~35):**
```csharp
// T�M:
this.Text = "Th?ng k� n�ng cao - ToDoList Analytics";

// THAY:
this.Text = "Thong ke nang cao - ToDoList Analytics";
```

#### **2. S?a Tab Names (~50-55):**
```csharp
// T�M:
new TabPage("T?ng quan")
new TabPage("Bi?u ??")
new TabPage("Th?ng k�")

// THAY:
new TabPage("Tong quan")
new TabPage("Bieu do")
new TabPage("Thong ke")
```

#### **3. S?a Stats Cards (~100-120):**
```csharp
// T�M:
"T?ng Projects" ? THAY: "Tong Projects"
"T?ng Tasks" ? THAY: "Tong Tasks"
"Ho�n th�nh" ? THAY: "Hoan thanh"
"Th?i gian" ? THAY: "Thoi gian"
```

#### **4. S?a Section Headers (~150-180):**
```csharp
// T�M:
"Ho?t ??ng g?n ?�y" ? THAY: "Hoat dong gan day"
"Ti?n ?? theo d? �n" ? THAY: "Tien do theo du an"
```

#### **5. ??i Font Arial (~200+):**
```csharp
// T�M:
new Font("Segoe UI", 18F, FontStyle.Bold)

// THAY:
new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point)
```

---

## ?? **TROUBLESHOOTING:**

### **V?n ??: Bi?u ?? kh�ng hi?n th?**

**Nguy�n nh�n:** Chart control ch?a ???c kh?i t?o ?�ng

**Gi?i ph�p:** T�m d�ng:
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

**Nguy�n nh�n:** File encoding kh�ng ph?i UTF-8

**Gi?i ph�p:**
1. M? file trong Visual Studio
2. File ? Advanced Save Options
3. Ch?n: **UTF-8 with signature - Codepage 65001**
4. Save
5. Build l?i

### **V?n ??: Script PowerShell kh�ng ch?y**

**Nguy�n nh�n:** Execution Policy b? ch?n

**Gi?i ph�p:**
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
- ? Bi?u ?? tr�n hi?n th?
- ? Zoom b?ng chu?t ho?t ??ng
- ? Pan b?ng drag ho?t ??ng
- ? Tooltip khi hover ho?t ??ng

---

## ? **CHECKLIST:**

- [ ] Script ?� ch?y th�nh c�ng
- [ ] Build successful (kh�ng c� errors)
- [ ] Form m? ???c (kh�ng crash)
- [ ] Title bar ?�ng (kh�ng c� ??)
- [ ] Stats cards ?�ng (6, 20, 30.0%, 34.2h)
- [ ] Sections header ?�ng
- [ ] Bi?u ?? hi?n th? ??y ??
- [ ] Zoom/Pan charts ho?t ??ng
- [ ] Kh�ng c� l?i encoding n�o

---

## ?? **NHANH CH�NG:**

```powershell
# All in one command:
cd C:\Users\ACER\source\repos\DoAnMonLapTrinhWindow
.\fix_advanced_reports_auto.ps1
# ? Ch? "COMPLETED!"
# ? Ctrl+Shift+B (Build)
# ? F5 (Run)
# ? Test Advanced Reports
```

**Th?i gian: < 2 ph�t!** ?

---

N?u c?n tr? gi�p, check file: `FIX_ADVANCED_REPORTS_COMPLETE_GUIDE.md`
