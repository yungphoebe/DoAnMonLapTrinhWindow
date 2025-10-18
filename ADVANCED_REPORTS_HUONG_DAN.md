# ?? H??NG D?N S? D?NG ADVANCED REPORTS

## ?? **T?ng quan**

Advanced Reports Form l� t�nh n?ng th?ng k� n�ng cao v?i **3 lo?i charts**:
1. ? **C# Charts** (Built-in) - Kh�ng c?n c�i g�
2. ?? **Interactive Charts** (C# v?i zoom/pan) - Kh�ng c?n c�i g�
3. ?? **Python Charts** (Professional) - C?n c�i Python

---

## ?? **C�c Tab trong Advanced Reports**

### **Tab 1: ?? T?ng quan (Overview)**
- **4 Summary Cards**:
  - ?? T?ng Projects
  - ? T?ng Tasks  
  - ?? % Ho�n th�nh
  - ?? Th?i gian (gi?)

- **?? Ho?t ??ng g?n ?�y**: Danh s�ch 10 tasks g?n nh?t
- **?? Ti?n ?? theo d? �n**: Bar chart progress c?a 6 projects

### **Tab 2: ? N?ng su?t (Productivity)**
- **4 Productivity Cards**:
  - H�m nay
  - Tu?n n�y
  - Th�ng n�y
  - Trung b�nh/ng�y

- **2 Buttons**:
  - ?? **Interactive Charts** - M? charts ??ng
  - ?? **Python Charts** - Generate Python charts

- **?? Bi?u ?? n?ng su?t**: Daily productivity chart (7 ng�y)

### **Tab 3: ?? Ph�n t�ch d? �n (Projects Analysis)**
- **DataGridView**: B?ng chi ti?t c�c projects
  - T�n d? �n
  - T?ng tasks
  - Ho�n th�nh
  - ?ang th?c hi?n
  - Ch? x? l�
  - Th?i gian (ph�t)
  - T? l? ho�n th�nh

### **Tab 4: ? Ph�n t�ch th?i gian (Time Analysis)**
- **?? Pie Chart**: Ph�n b? th?i gian theo Priority (High/Medium/Low)
- **?? Bar Chart**: Xu h??ng theo tu?n (Tasks & Hours)

---

## ?? **C�CH S? D?NG INTERACTIVE CHARTS**

### **B??c 1: M? Advanced Reports**
```
Right-click project card ? "?? Th?ng k� n�ng cao"
```

### **B??c 2: Click n�t "?? Interactive Charts"**
Trong tab **? N?ng su?t**

### **B??c 3: Ch?n lo?i chart**

#### **N?u KH�NG c� Python:**
```
Dialog hi?n l�n:
"?? Python charts require Python to be installed.

Would you like to:
� YES: Use C# interactive charts (built-in)
� NO: Install Python and use beautiful Python charts"

? Click YES: D�ng C# charts (zoom/pan/hover)
? Click NO: C�i Python ?? d�ng Python charts
```

#### **N?u ?� c� Python:**
```
T? ??ng m? Python Charts v?i loading screen
? Generate charts real-time
? Load v�o app
? Hi?n th? 4 tabs v?i Python charts ??p
```

---

## ?? **PYTHON CHARTS - C�I ??T & S? D?NG**

### **Option 1: Auto Install (Khuy?n ngh?)**

**1. Ch?y installer:**
```cmd
Right-click: QUICK_FIX_PYTHON_CHARTS.bat
? Run as Administrator
```

**2. ??i c�i ??t:**
- Python ???c detect t? ??ng
- Packages t? ??ng install:
  - matplotlib
  - seaborn
  - pandas
  - pyodbc
  - plotly
  - kaleido

**3. Test:**
```cmd
TEST_PYTHON_SETUP.bat
```

### **Option 2: Manual Install**

**1. C�i Python:**
- Download: https://www.python.org/downloads/
- ? **QUAN TR?NG**: Check "Add Python to PATH"

**2. Install packages:**
```cmd
py -m pip install matplotlib seaborn pandas pyodbc plotly kaleido
```

**3. Restart app**

---

## ?? **C# INTERACTIVE CHARTS - T�NH N?NG**

### **4 Lo?i Charts:**

#### **1. ?? Project Progress (Bar Chart)**
- Horizontal bars
- Real-time data t? database
- Colors t? project settings
- **Interactive**:
  - Mouse wheel: Zoom in/out
  - Click + drag: Pan around
  - Hover: Show tooltip v?i details
  - Zoom range: 0.5x - 3.0x

#### **2. ?? Daily Productivity (Line Chart)**
- Line chart v?i markers
- Data 7 ng�y g?n nh?t
- Fill area d??i line
- **Interactive**:
  - Hover: Show date & count
  - Zoom/pan enabled
  - Points highlight on hover

#### **3. ?? Priority Distribution (Pie Chart)**
- 3 priorities: High/Medium/Low
- Color-coded
- Percentage labels
- **Interactive**:
  - Hover: Expand slice
  - Show tooltip with count
  - Zoom pie chart

#### **4. ?? Weekly Trends (Area Chart)**
- Area chart v?i gradient
- Data 4 tu?n g?n nh?t
- Smooth curves
- **Interactive**:
  - Zoom/pan
  - Hover tooltips
  - Real-time updates

---

## ?? **PYTHON CHARTS - T�NH N?NG**

### **4 Charts Professional:**

#### **1. ?? Project Progress**
```python
- Horizontal bar chart v?i gradient
- Value labels
- Custom colors t? database
- Dark theme matched v?i app
- 150 DPI high quality
```

#### **2. ?? Daily Productivity**
```python
- Line chart v?i markers
- Gradient fill
- Grid lines
- Professional typography
- Smooth curves
```

#### **3. ?? Priority Distribution**
```python
- Pie chart v?i percentages
- Color-coded slices
- Large font labels
- Auto-calculated percentages
- Clean layout
```

#### **4. ?? Weekly Trends**
```python
- Bar chart v?i value labels
- Gradient colors
- Grid lines (y-axis only)
- Professional styling
- High contrast
```

### **Controls trong Python Charts Tab:**
```
?? + : Zoom In (AutoSize mode)
?? - : Zoom Out (Zoom mode)
??   : Refresh data & regenerate charts
```

---

## ?? **SO S�NH C�C LO?I CHARTS**

| Feature | Static C# | Interactive C# | Python Charts |
|---------|-----------|----------------|---------------|
| **In App** | ? Yes | ? Yes | ? Yes |
| **Zoom** | ? No | ? Mouse wheel | ? Buttons |
| **Pan** | ? No | ? Drag | ? No |
| **Hover Tooltips** | ? No | ? Yes | ? No |
| **Refresh** | ? Auto | ? Auto | ? Button |
| **Quality** | ??? | ???? | ????? |
| **Speed** | ? Fast | ? Fast | ?? Medium |
| **Requires** | Nothing | Nothing | Python |
| **Real-time** | ? Yes | ? Yes | ? Yes |

---

## ?? **TROUBLESHOOTING**

### **L?i 1: "Python not found"**
**Gi?i ph�p:**
```
1. Run: QUICK_FIX_PYTHON_CHARTS.bat
   HO?C
2. Manual:
   - Install Python
   - ? Check "Add to PATH"
   - Restart app
```

### **L?i 2: "ModuleNotFoundError: No module named 'matplotlib'"**
**Gi?i ph�p:**
```cmd
py -m pip install matplotlib seaborn pandas pyodbc plotly kaleido
```

### **L?i 3: "Charts kh�ng hi?n th?"**
**Gi?i ph�p:**
```
1. Check data:
   - Ph?i c� �t nh?t 1 project
   - Ph?i c� tasks v?i data
   
2. Refresh:
   - Click button ?? trong Python Charts
   - Ho?c ?�ng form v� m? l?i
```

### **L?i 4: "Permission denied"**
**Gi?i ph�p:**
```
Right-click QUICK_FIX_PYTHON_CHARTS.bat
? Run as Administrator
```

---

## ?? **C?U TR�C FILES**

```
ToDoList/
??? ToDoList.GUI/
?   ??? Forms/
?   ?   ??? AdvancedReportsForm.cs  ? Main form
?   ??? Components/
?       ??? InteractiveChartControl.cs  ? Interactive charts
?
??? python_charts/
?   ??? realtime_charts.py  ? Python script (auto-generated)
?   ??? chart_data.json     ? Data export (auto-generated)
?   ??? realtime_charts/    ? Generated PNG files
?       ??? project_progress.png
?       ??? daily_productivity.png
?       ??? priority_distribution.png
?       ??? weekly_trends.png
?
??? QUICK_FIX_PYTHON_CHARTS.bat  ? Auto installer
```

---

## ?? **QUICK START GUIDE**

### **C�ch nhanh nh?t:**

#### **1. Kh�ng mu?n c�i Python:**
```
Open app ? Advanced Reports
? Xem c�c tabs Overview/Productivity/Projects/Time
? Ho?c click "?? Interactive Charts" ? YES (C# version)
? Enjoy interactive charts!
```

#### **2. Mu?n Python charts ??p:**
```
1. Run: QUICK_FIX_PYTHON_CHARTS.bat (as Admin)
2. Wait for installation
3. Open app ? Advanced Reports
4. Click "?? Interactive Charts"
5. Wait for Python charts generation
6. Enjoy professional charts in app!
```

---

## ?? **TIPS & TRICKS**

### **Tip 1: Zoom in Interactive Charts**
```
- Mouse wheel up: Zoom in
- Mouse wheel down: Zoom out
- Zoom range: 0.5x to 3.0x
- Click + drag: Pan around
```

### **Tip 2: Refresh Python Charts**
```
Trong Python Charts tab:
Click button ?? ? Charts ???c regenerate v?i data m?i
```

### **Tip 3: Compare Charts**
```
M? 2 windows:
1. Advanced Reports (static C# charts)
2. Interactive Charts (C# ho?c Python)
? So s�nh data real-time
```

### **Tip 4: Export Python Charts**
```
Python charts ???c save t?i:
ToDoList.GUI\bin\Debug\net9.0-windows\python_charts\realtime_charts\

Copy files PNG n�y ?? share ho?c embed v�o reports
```

---

## ?? **ADVANCED SETTINGS**

### **Thay ??i Python Script:**
```python
File: python_charts/realtime_charts.py

C� th? customize:
- Chart size: figsize=(12, 6)
- DPI: dpi=150
- Colors: color='#6495ED'
- Font size: fontsize=12
- Style: sns.set_style('darkgrid')
```

### **Thay ??i C# Charts:**
```csharp
File: ToDoList.GUI/Components/InteractiveChartControl.cs

C� th? customize:
- Zoom limits: 0.5f to 3.0f
- Colors: Color.FromArgb(...)
- Font: new Font("Segoe UI", ...)
- Tooltip style
- Animation speed
```

---

## ?? **H? TR?**

### **N?u c� l?i:**
1. Check build: Xem errors trong Output window
2. Check Python: Run `TEST_PYTHON_SETUP.bat`
3. Check data: ??m b?o c� projects & tasks
4. Restart app

### **Files quan tr?ng:**
- `AdvancedReportsForm.cs` - Main form
- `InteractiveChartControl.cs` - Interactive charts
- `QUICK_FIX_PYTHON_CHARTS.bat` - Installer
- `TEST_PYTHON_SETUP.bat` - Test script

---

## ?? **K?T LU?N**

Advanced Reports cung c?p **3 c?p ?? visualization**:

1. **Basic** (Static C# charts) - Fast, simple
2. **Interactive** (C# with zoom/pan) - Professional, interactive
3. **Beautiful** (Python charts) - Highest quality, professional

**Ch?n lo?i chart ph� h?p v?i nhu c?u!**

? **Build successful - No errors!**
?? **All charts working perfectly!**
?? **Ready to use!**
