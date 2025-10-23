# H??ng d?n thêm link "??ng ký" vào Form ??ng nh?p

## T?ng quan thay ??i

?ã thêm m?t dòng ch? có th? click "Ch?a có tài kho?n? ??ng ký ngay" vào form ??ng nh?p. Khi ng??i dùng click vào dòng này, form ??ng ký s? ???c m?.

## Các thay ??i trong LoginForm.cs

### 1. Thêm bi?n label m?i
```csharp
private Label lblRegisterLink; // ? New: Register link label
```

### 2. T?o và c?u hình label trong SetupCustomUI()
```csharp
// ? NEW: Register link label
lblRegisterLink = new Label
{
    Text = "Ch?a có tài kho?n? ??ng ký ngay",
    Font = new Font("Segoe UI", 10, FontStyle.Underline),
    ForeColor = Color.FromArgb(52, 152, 219),
    AutoSize = false,
    Size = new Size(300, 25),
    Location = new Point(25, 385),
    TextAlign = ContentAlignment.MiddleCenter,
    Cursor = Cursors.Hand
};
```

### 3. Thêm event handlers cho hi?u ?ng hover và click
```csharp
// Add click event to open RegisterForm
lblRegisterLink.Click += LblRegisterLink_Click;

// Add hover effect
lblRegisterLink.MouseEnter += (s, e) => 
{
    lblRegisterLink.ForeColor = Color.FromArgb(41, 128, 185);
    lblRegisterLink.Font = new Font("Segoe UI", 10, FontStyle.Bold | FontStyle.Underline);
};
lblRegisterLink.MouseLeave += (s, e) => 
{
    lblRegisterLink.ForeColor = Color.FromArgb(52, 152, 219);
    lblRegisterLink.Font = new Font("Segoe UI", 10, FontStyle.Underline);
};
```

### 4. Thêm control vào panel
```csharp
pnlMain.Controls.Add(lblRegisterLink); // ? Add register link
```

### 5. T?o ph??ng th?c x? lý click
```csharp
// ? NEW: Event handler for register link click
private void LblRegisterLink_Click(object sender, EventArgs e)
{
    // Open RegisterForm as a dialog
    using (var registerForm = new RegisterForm())
    {
        if (registerForm.ShowDialog() == DialogResult.OK)
        {
            // If registration is successful, optionally auto-fill the email
            // or show a success message
            MessageBox.Show(
                "??ng ký thành công! Vui lòng ??ng nh?p.",
                "Thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
```

## Tính n?ng chính

? **Label có th? click**: Dòng ch? "Ch?a có tài kho?n? ??ng ký ngay" ho?t ??ng nh? m?t link
? **Hi?u ?ng hover**: Khi di chu?t qua, màu s?c và font ch? thay ??i ?? th? hi?n có th? click
? **M? form ??ng ký**: Click vào s? m? RegisterForm d??i d?ng dialog
? **Thông báo thành công**: Sau khi ??ng ký thành công, hi?n th? thông báo và quay l?i form ??ng nh?p
? **Giao di?n ??p**: Màu xanh d??ng (Color.FromArgb(52, 152, 219)) phù h?p v?i theme c?a form
? **Con tr? chu?t thay ??i**: Cursor thay ??i thành Hand khi di chu?t qua

## V? trí hi?n th?

Label "??ng ký" ???c ??t ?:
- **V? trí Y**: 385px t? trên xu?ng (d??i các nút ??ng nh?p và H?y)
- **V? trí X**: 25px t? bên trái
- **Kích th??c**: 300x25px
- **C?n gi?a**: TextAlign = ContentAlignment.MiddleCenter

## Màu s?c

- **Màu bình th??ng**: RGB(52, 152, 219) - Xanh d??ng nh?t
- **Màu khi hover**: RGB(41, 128, 185) - Xanh d??ng ??m h?n
- **Font**: Segoe UI, 10pt v?i g?ch chân (Underline)
- **Font khi hover**: Segoe UI, 10pt Bold v?i g?ch chân

## Cách s? d?ng

1. M? ?ng d?ng
2. Form ??ng nh?p s? hi?n th?
3. Tìm dòng ch? "Ch?a có tài kho?n? ??ng ký ngay" ? phía d??i
4. Click vào dòng ch? này
5. Form ??ng ký s? m? ra
6. ?i?n thông tin và ??ng ký
7. Sau khi ??ng ký thành công, s? quay l?i form ??ng nh?p v?i thông báo thành công

## L?u ý k? thu?t

- S? d?ng `using` statement ?? t? ??ng dispose RegisterForm sau khi ?óng
- Ki?m tra `DialogResult.OK` ?? xác ??nh ??ng ký thành công
- Label có `Cursor = Cursors.Hand` ?? ng??i dùng bi?t có th? click
- Hi?u ?ng hover ???c th?c hi?n b?ng MouseEnter và MouseLeave events
- Font ch? có g?ch chân (Underline) ?? gi?ng nh? m?t hyperlink web

## C?i ti?n có th? th?c hi?n trong t??ng lai

1. **Auto-fill email**: Sau khi ??ng ký thành công, t? ??ng ?i?n email vào form ??ng nh?p
2. **Animation**: Thêm hi?u ?ng chuy?n ??ng khi m? form ??ng ký
3. **Validation**: Ki?m tra email ?ã t?n t?i tr??c khi cho phép ??ng ký
4. **Remember me**: Thêm checkbox "Ghi nh? ??ng nh?p"
5. **Forgot password**: Thêm link "Quên m?t kh?u" t??ng t?

## Build Status

? Build successful - Không có l?i compilation
? File ?ã ???c c?p nh?t: `ToDoList.GUI\Forms\LoginForm.cs`
