# ? H??ng d?n th�m n�t "Break" cho Timer

## ?? M� t?

Th�m ch?c n?ng **Break** (t?m d?ng) cho timer trong Cuculist Form, cho ph�p ng??i d�ng t?m d?ng th?i gian l�m vi?c m?t c�ch gi?a ch?ng ?? ngh? ng?i.

## ?? M?c ti�u

Khi b?m n�t "Break":
- ? Timer s? T?M D?NG (kh�ng ??m n?a)
- ?? N�t "Break" chuy?n th�nh n�t "Resume"  
- ?? M�u s?c timer chuy?n sang m�u cam ?? bi?u th? ?ang ngh?
- ?? Hi?n th? th�ng b�o "Break Time"
- ? B?m "Resume" ?? ti?p t?c ??m t? th?i ?i?m ?� d?ng

## ?? File c?n ch?nh s?a

```
ToDoList.GUI\Forms\CuculistMiniForm.cs
```
HO?C
```
ToDoList.GUI\Forms\CuculistDetailForm.cs
```

## ?? C�ch th?c hi?n

### B??c 1: Th�m bi?n tr?ng th�i

Th�m v�o ??u class (c�ng v?i c�c bi?n kh�c):

```csharp
private bool isPaused = false;
private bool isBreak = false;
private TimeSpan pausedTime = TimeSpan.Zero;
private Button btnBreak; // N�t Break m?i
```

### B??c 2: T?o n�t Break trong InitializeComponent()

Th�m sau n�t "Mark me as done":

```csharp
// ? NEW: Break Button (T?m d?ng)
btnBreak = new Button
{
    Text = "? Break",
    Location = new Point(180, 100),  // ?i?u ch?nh v? tr� cho ph� h?p
    Size = new Size(100, 35),
    FlatStyle = FlatStyle.Flat,
    BackColor = Color.FromArgb(255, 152, 0),  // M�u cam
    ForeColor = Color.White,
    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
    Cursor = Cursors.Hand
};
btnBreak.FlatAppearance.BorderSize = 0;
btnBreak.Click += BtnBreak_Click;

this.Controls.Add(btnBreak);
```

### B??c 3: C?p nh?t s? ki?n Timer_Tick

S?a l?i h�m `Timer_Tick` ?? ki?m tra tr?ng th�i break:

```csharp
private void Timer_Tick(object sender, EventArgs e)
{
    // ? CH? ??m th?i gian khi KH�NG ? tr?ng th�i Break
    if (!isPaused && !isBreak)
    {
        elapsedTime = DateTime.Now - startTime + pausedTime;
        lblTimer.Text = elapsedTime.ToString(@"hh\:mm\:ss");
    }
    // N?u ?ang break th� KH�NG c?p nh?t th?i gian
}
```

### B??c 4: Th�m h�m x? l� s? ki?n BtnBreak_Click

```csharp
// ? NEW: Break Button Handler
private void BtnBreak_Click(object sender, EventArgs e)
{
    if (!isBreak)
    {
        // ===== B?T ??U BREAK (T?m d?ng) =====
        isPaused = true;
        isBreak = true;
        
        // L?u l?i th?i gian ?� ch?y
        pausedTime = elapsedTime;
        
        // ===== ??I GIAO DI?N =====
        btnBreak.Text = "? Resume";
        btnBreak.BackColor = Color.FromArgb(76, 175, 80); // M�u xanh
        lblTimer.ForeColor = Color.FromArgb(255, 152, 0); // M�u cam khi break
        
        // ===== TH�NG B�O =====
        this.Text = "Cuculist Timer - Break";
        
        MessageBox.Show(
            "? B?n ?ang ngh? gi?i lao!\n\nNh?n 'Resume' ?? ti?p t?c l�m vi?c.", 
            "Break Time", 
            MessageBoxButtons.OK, 
            MessageBoxIcon.Information
        );
    }
    else
    {
        // ===== RESUME L?I TIMER =====
        isBreak = false;
        isPaused = false;
        
        // C?p nh?t l?i startTime ?? t�nh to�n ?�ng
        startTime = DateTime.Now;
        
        // ===== ??I GIAO DI?N TR? L?I =====
        btnBreak.Text = "? Break";
        btnBreak.BackColor = Color.FromArgb(255, 152, 0); // M�u cam
        lblTimer.ForeColor = Color.FromArgb(100, 149, 237); // M�u xanh d??ng
        
        this.Text = "Cuculist Timer";
    }
}
```

## ?? T�y ch?nh giao di?n (Optional)

### Th�m hi?u ?ng nh�y (Blinking) khi Break

Th�m v�o Timer_Tick:

```csharp
private void Timer_Tick(object sender, EventArgs e)
{
    if (!isPaused && !isBreak)
    {
        elapsedTime = DateTime.Now - startTime + pausedTime;
        lblTimer.Text = elapsedTime.ToString(@"hh\:mm\:ss");
    }
    else if (isBreak)
    {
        // ? L�m nh�y timer khi ?ang break
        lblTimer.Visible = !lblTimer.Visible;
    }
}
```

### Hi?n th? th?i gian ngh?

Th�m Label ri�ng ?? hi?n th? th?i gian ngh?:

```csharp
private Label lblBreakTime;
private DateTime breakStartTime;

// Trong InitializeComponent():
lblBreakTime = new Label
{
    Text = "Break: 00:00:00",
    Location = new Point(220, 50),
    Size = new Size(150, 30),
    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
    ForeColor = Color.FromArgb(255, 152, 0),
    BackColor = Color.Transparent,
    Visible = false
};
this.Controls.Add(lblBreakTime);

// Trong BtnBreak_Click khi b?t ??u break:
breakStartTime = DateTime.Now;
lblBreakTime.Visible = true;

// Trong Timer_Tick khi ?ang break:
if (isBreak)
{
    var breakDuration = DateTime.Now - breakStartTime;
    lblBreakTime.Text = $"Break: {breakDuration:hh\\:mm\\:ss}";
}

// Khi Resume:
lblBreakTime.Visible = false;
```

## ?? V? tr� c�c n�t (G?i � Layout)

```
???????????????????????????????????????
?  Task Title: "Coding Project"      ?
?                                     ?
?  Timer: 00:09:18                   ?
?                                     ?
?  [? Mark me as done] [? Break] [? Stop]  ?
???????????????????????????????????????
```

## ? K?t qu? mong ??i

1. **Tr??c khi b?m Break:**
   - Timer ?ang ??m: `00:09:18`
   - N�t hi?n th?: `? Break` (m�u cam)

2. **Sau khi b?m Break:**
   - Timer d?ng l?i ?: `00:09:18` (kh�ng ??m n?a)
   - N�t hi?n th?: `? Resume` (m�u xanh)
   - Th�ng b�o: "B?n ?ang ngh? gi?i lao!"
   - M�u timer: Cam

3. **Sau khi b?m Resume:**
   - Timer ti?p t?c ??m t?: `00:09:18` ? `00:09:19`...
   - N�t hi?n th?: `? Break` (m�u cam)
   - M�u timer: Xanh d??ng

## ?? X? l� l?i th??ng g?p

### L?i 1: Timer kh�ng d?ng
**Nguy�n nh�n:** Ch?a ki?m tra `isBreak` trong `Timer_Tick`
**Gi?i ph�p:** Th�m ?i?u ki?n `if (!isBreak)` tr??c khi c?p nh?t th?i gian

### L?i 2: Th?i gian nh?y c�c khi Resume
**Nguy�n nh�n:** Kh�ng c?p nh?t l?i `startTime` khi Resume
**Gi?i ph�p:** Th�m d�ng `startTime = DateTime.Now;` khi Resume

### L?i 3: N�t kh�ng ??i text
**Nguy�n nh�n:** Qu�n g�n l?i `btnBreak.Text`
**Gi?i ph�p:** Ki?m tra l?i code ??i text trong `BtnBreak_Click`

## ?? C?i ti?n th�m (Advanced)

### 1. L?u l?ch s? Break v�o database
```csharp
// L?u th?i gian break v�o FocusSession
var breakLog = new FocusSession
{
    UserId = _userId,
    TaskId = _currentTask?.TaskId,
    StartTime = breakStartTime,
    EndTime = DateTime.Now,
    Notes = "Break time"
};
_context.FocusSessions.Add(breakLog);
await _context.SaveChangesAsync();
```

### 2. Gi?i h?n s? l?n Break
```csharp
private int breakCount = 0;
private const int MAX_BREAKS = 3;

if (breakCount >= MAX_BREAKS)
{
    MessageBox.Show("B?n ?� ngh? ?? 3 l?n r?i. H�y t?p trung l�m vi?c!");
    return;
}
breakCount++;
```

### 3. Th?ng k� th?i gian Break
```csharp
private TimeSpan totalBreakTime = TimeSpan.Zero;

// Khi Resume:
totalBreakTime += DateTime.Now - breakStartTime;
lblStats.Text = $"Total break: {totalBreakTime:hh\\:mm\\:ss}";
```

## ?? Tham kh?o

- **Pomodoro Technique:** 25 ph�t l�m vi?c + 5 ph�t ngh?
- **Break Types:** Short break (5-10 ph�t), Long break (15-30 ph�t)
- **Focus Best Practices:** Ngh? ng?i gi�p t?ng n?ng su?t

## ? Demo Code ho�n ch?nh

Xem file m?u: `CuculistMiniForm_WithBreak.cs` (s? t?o ri�ng n?u c?n)
