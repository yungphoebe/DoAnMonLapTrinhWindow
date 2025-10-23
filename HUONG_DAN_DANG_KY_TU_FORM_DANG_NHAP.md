# H??ng d?n th�m link "??ng k�" v�o Form ??ng nh?p

## T?ng quan thay ??i

?� th�m m?t d�ng ch? c� th? click "Ch?a c� t�i kho?n? ??ng k� ngay" v�o form ??ng nh?p. Khi ng??i d�ng click v�o d�ng n�y, form ??ng k� s? ???c m?.

## C�c thay ??i trong LoginForm.cs

### 1. Th�m bi?n label m?i
```csharp
private Label lblRegisterLink; // ? New: Register link label
```

### 2. T?o v� c?u h�nh label trong SetupCustomUI()
```csharp
// ? NEW: Register link label
lblRegisterLink = new Label
{
    Text = "Ch?a c� t�i kho?n? ??ng k� ngay",
    Font = new Font("Segoe UI", 10, FontStyle.Underline),
    ForeColor = Color.FromArgb(52, 152, 219),
    AutoSize = false,
    Size = new Size(300, 25),
    Location = new Point(25, 385),
    TextAlign = ContentAlignment.MiddleCenter,
    Cursor = Cursors.Hand
};
```

### 3. Th�m event handlers cho hi?u ?ng hover v� click
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

### 4. Th�m control v�o panel
```csharp
pnlMain.Controls.Add(lblRegisterLink); // ? Add register link
```

### 5. T?o ph??ng th?c x? l� click
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
                "??ng k� th�nh c�ng! Vui l�ng ??ng nh?p.",
                "Th�nh c�ng",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
```

## T�nh n?ng ch�nh

? **Label c� th? click**: D�ng ch? "Ch?a c� t�i kho?n? ??ng k� ngay" ho?t ??ng nh? m?t link
? **Hi?u ?ng hover**: Khi di chu?t qua, m�u s?c v� font ch? thay ??i ?? th? hi?n c� th? click
? **M? form ??ng k�**: Click v�o s? m? RegisterForm d??i d?ng dialog
? **Th�ng b�o th�nh c�ng**: Sau khi ??ng k� th�nh c�ng, hi?n th? th�ng b�o v� quay l?i form ??ng nh?p
? **Giao di?n ??p**: M�u xanh d??ng (Color.FromArgb(52, 152, 219)) ph� h?p v?i theme c?a form
? **Con tr? chu?t thay ??i**: Cursor thay ??i th�nh Hand khi di chu?t qua

## V? tr� hi?n th?

Label "??ng k�" ???c ??t ?:
- **V? tr� Y**: 385px t? tr�n xu?ng (d??i c�c n�t ??ng nh?p v� H?y)
- **V? tr� X**: 25px t? b�n tr�i
- **K�ch th??c**: 300x25px
- **C?n gi?a**: TextAlign = ContentAlignment.MiddleCenter

## M�u s?c

- **M�u b�nh th??ng**: RGB(52, 152, 219) - Xanh d??ng nh?t
- **M�u khi hover**: RGB(41, 128, 185) - Xanh d??ng ??m h?n
- **Font**: Segoe UI, 10pt v?i g?ch ch�n (Underline)
- **Font khi hover**: Segoe UI, 10pt Bold v?i g?ch ch�n

## C�ch s? d?ng

1. M? ?ng d?ng
2. Form ??ng nh?p s? hi?n th?
3. T�m d�ng ch? "Ch?a c� t�i kho?n? ??ng k� ngay" ? ph�a d??i
4. Click v�o d�ng ch? n�y
5. Form ??ng k� s? m? ra
6. ?i?n th�ng tin v� ??ng k�
7. Sau khi ??ng k� th�nh c�ng, s? quay l?i form ??ng nh?p v?i th�ng b�o th�nh c�ng

## L?u � k? thu?t

- S? d?ng `using` statement ?? t? ??ng dispose RegisterForm sau khi ?�ng
- Ki?m tra `DialogResult.OK` ?? x�c ??nh ??ng k� th�nh c�ng
- Label c� `Cursor = Cursors.Hand` ?? ng??i d�ng bi?t c� th? click
- Hi?u ?ng hover ???c th?c hi?n b?ng MouseEnter v� MouseLeave events
- Font ch? c� g?ch ch�n (Underline) ?? gi?ng nh? m?t hyperlink web

## C?i ti?n c� th? th?c hi?n trong t??ng lai

1. **Auto-fill email**: Sau khi ??ng k� th�nh c�ng, t? ??ng ?i?n email v�o form ??ng nh?p
2. **Animation**: Th�m hi?u ?ng chuy?n ??ng khi m? form ??ng k�
3. **Validation**: Ki?m tra email ?� t?n t?i tr??c khi cho ph�p ??ng k�
4. **Remember me**: Th�m checkbox "Ghi nh? ??ng nh?p"
5. **Forgot password**: Th�m link "Qu�n m?t kh?u" t??ng t?

## Build Status

? Build successful - Kh�ng c� l?i compilation
? File ?� ???c c?p nh?t: `ToDoList.GUI\Forms\LoginForm.cs`
