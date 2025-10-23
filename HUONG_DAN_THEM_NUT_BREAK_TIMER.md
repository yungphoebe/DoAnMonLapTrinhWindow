# ? H??ng d?n thêm nút "Break" cho Timer

## ?? Mô t?

Thêm ch?c n?ng **Break** (t?m d?ng) cho timer trong Cuculist Form, cho phép ng??i dùng t?m d?ng th?i gian làm vi?c m?t cách gi?a ch?ng ?? ngh? ng?i.

## ?? M?c tiêu

Khi b?m nút "Break":
- ? Timer s? T?M D?NG (không ??m n?a)
- ?? Nút "Break" chuy?n thành nút "Resume"  
- ?? Màu s?c timer chuy?n sang màu cam ?? bi?u th? ?ang ngh?
- ?? Hi?n th? thông báo "Break Time"
- ? B?m "Resume" ?? ti?p t?c ??m t? th?i ?i?m ?ã d?ng

## ?? File c?n ch?nh s?a

```
ToDoList.GUI\Forms\CuculistMiniForm.cs
```
HO?C
```
ToDoList.GUI\Forms\CuculistDetailForm.cs
```

## ?? Cách th?c hi?n

### B??c 1: Thêm bi?n tr?ng thái

Thêm vào ??u class (cùng v?i các bi?n khác):

```csharp
private bool isPaused = false;
private bool isBreak = false;
private TimeSpan pausedTime = TimeSpan.Zero;
private Button btnBreak; // Nút Break m?i
```

### B??c 2: T?o nút Break trong InitializeComponent()

Thêm sau nút "Mark me as done":

```csharp
// ? NEW: Break Button (T?m d?ng)
btnBreak = new Button
{
    Text = "? Break",
    Location = new Point(180, 100),  // ?i?u ch?nh v? trí cho phù h?p
    Size = new Size(100, 35),
    FlatStyle = FlatStyle.Flat,
    BackColor = Color.FromArgb(255, 152, 0),  // Màu cam
    ForeColor = Color.White,
    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
    Cursor = Cursors.Hand
};
btnBreak.FlatAppearance.BorderSize = 0;
btnBreak.Click += BtnBreak_Click;

this.Controls.Add(btnBreak);
```

### B??c 3: C?p nh?t s? ki?n Timer_Tick

S?a l?i hàm `Timer_Tick` ?? ki?m tra tr?ng thái break:

```csharp
private void Timer_Tick(object sender, EventArgs e)
{
    // ? CH? ??m th?i gian khi KHÔNG ? tr?ng thái Break
    if (!isPaused && !isBreak)
    {
        elapsedTime = DateTime.Now - startTime + pausedTime;
        lblTimer.Text = elapsedTime.ToString(@"hh\:mm\:ss");
    }
    // N?u ?ang break thì KHÔNG c?p nh?t th?i gian
}
```

### B??c 4: Thêm hàm x? lý s? ki?n BtnBreak_Click

```csharp
// ? NEW: Break Button Handler
private void BtnBreak_Click(object sender, EventArgs e)
{
    if (!isBreak)
    {
        // ===== B?T ??U BREAK (T?m d?ng) =====
        isPaused = true;
        isBreak = true;
        
        // L?u l?i th?i gian ?ã ch?y
        pausedTime = elapsedTime;
        
        // ===== ??I GIAO DI?N =====
        btnBreak.Text = "? Resume";
        btnBreak.BackColor = Color.FromArgb(76, 175, 80); // Màu xanh
        lblTimer.ForeColor = Color.FromArgb(255, 152, 0); // Màu cam khi break
        
        // ===== THÔNG BÁO =====
        this.Text = "Cuculist Timer - Break";
        
        MessageBox.Show(
            "? B?n ?ang ngh? gi?i lao!\n\nNh?n 'Resume' ?? ti?p t?c làm vi?c.", 
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
        
        // C?p nh?t l?i startTime ?? tính toán ?úng
        startTime = DateTime.Now;
        
        // ===== ??I GIAO DI?N TR? L?I =====
        btnBreak.Text = "? Break";
        btnBreak.BackColor = Color.FromArgb(255, 152, 0); // Màu cam
        lblTimer.ForeColor = Color.FromArgb(100, 149, 237); // Màu xanh d??ng
        
        this.Text = "Cuculist Timer";
    }
}
```

## ?? Tùy ch?nh giao di?n (Optional)

### Thêm hi?u ?ng nháy (Blinking) khi Break

Thêm vào Timer_Tick:

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
        // ? Làm nháy timer khi ?ang break
        lblTimer.Visible = !lblTimer.Visible;
    }
}
```

### Hi?n th? th?i gian ngh?

Thêm Label riêng ?? hi?n th? th?i gian ngh?:

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

## ?? V? trí các nút (G?i ý Layout)

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
   - Nút hi?n th?: `? Break` (màu cam)

2. **Sau khi b?m Break:**
   - Timer d?ng l?i ?: `00:09:18` (không ??m n?a)
   - Nút hi?n th?: `? Resume` (màu xanh)
   - Thông báo: "B?n ?ang ngh? gi?i lao!"
   - Màu timer: Cam

3. **Sau khi b?m Resume:**
   - Timer ti?p t?c ??m t?: `00:09:18` ? `00:09:19`...
   - Nút hi?n th?: `? Break` (màu cam)
   - Màu timer: Xanh d??ng

## ?? X? lý l?i th??ng g?p

### L?i 1: Timer không d?ng
**Nguyên nhân:** Ch?a ki?m tra `isBreak` trong `Timer_Tick`
**Gi?i pháp:** Thêm ?i?u ki?n `if (!isBreak)` tr??c khi c?p nh?t th?i gian

### L?i 2: Th?i gian nh?y cóc khi Resume
**Nguyên nhân:** Không c?p nh?t l?i `startTime` khi Resume
**Gi?i pháp:** Thêm dòng `startTime = DateTime.Now;` khi Resume

### L?i 3: Nút không ??i text
**Nguyên nhân:** Quên gán l?i `btnBreak.Text`
**Gi?i pháp:** Ki?m tra l?i code ??i text trong `BtnBreak_Click`

## ?? C?i ti?n thêm (Advanced)

### 1. L?u l?ch s? Break vào database
```csharp
// L?u th?i gian break vào FocusSession
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
    MessageBox.Show("B?n ?ã ngh? ?? 3 l?n r?i. Hãy t?p trung làm vi?c!");
    return;
}
breakCount++;
```

### 3. Th?ng kê th?i gian Break
```csharp
private TimeSpan totalBreakTime = TimeSpan.Zero;

// Khi Resume:
totalBreakTime += DateTime.Now - breakStartTime;
lblStats.Text = $"Total break: {totalBreakTime:hh\\:mm\\:ss}";
```

## ?? Tham kh?o

- **Pomodoro Technique:** 25 phút làm vi?c + 5 phút ngh?
- **Break Types:** Short break (5-10 phút), Long break (15-30 phút)
- **Focus Best Practices:** Ngh? ng?i giúp t?ng n?ng su?t

## ? Demo Code hoàn ch?nh

Xem file m?u: `CuculistMiniForm_WithBreak.cs` (s? t?o riêng n?u c?n)
