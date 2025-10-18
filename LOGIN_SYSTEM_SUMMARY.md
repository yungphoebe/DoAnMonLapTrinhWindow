# ?? H? Th?ng ??ng Nh?p & Ch�o M?ng User

## ? **?� HO�N TH�NH**

### ?? **C�c T�nh N?ng ?� T?o:**

1. **LoginForm** - M�n h�nh ??ng nh?p ??p  
2. **UserSession** - Qu?n l� phi�n ??ng nh?p
3. **Welcome Message** - Hi?n th? "Xin ch�o, [T�n User]!"
4. **Program.cs** - Y�u c?u ??ng nh?p tr??c khi v�o app

---

## ?? **M�n H�nh ??ng Nh?p**

### File Created:
- `ToDoList.GUI/Forms/LoginForm.cs`
- `ToDoList.GUI/Helpers/UserSession.cs`

### T�nh N?ng:
- ? Giao di?n ??p, hi?n ??i
- ? Validation input
- ? Show/Hide password
- ? Enter key support
- ? Error messages
- ? Hover effects

---

## ?? **Th�ng Tin ??ng Nh?p Test**

Sau khi ch?y `ClearAndSeed.bat`, b?n c� 4 users:

### User 1:
```
Email: nguyenvanan@example.com
Password: hashed_password_1
T�n: Nguy?n V?n An
```

### User 2:
```
Email: tranthibinh@example.com
Password: hashed_password_2
T�n: Tr?n Th? B�nh
```

### User 3:
```
Email: leminhcuong@example.com
Password: hashed_password_3
T�n: L� Minh C??ng
```

### User 4:
```
Email: phamthuduyen@example.com
Password: hashed_password_4
T�n: Ph?m Thu Duy�n
```

---

## ?? **C�ch S? D?ng**

### 1. Ch?y Application

```cmd
# Ch?y t? Visual Studio (F5)
# Ho?c ch?y file .exe trong bin/Debug/net9.0-windows/
```

### 2. M�n H�nh ??ng Nh?p Xu?t Hi?n

- Nh?p email: `nguyenvanan@example.com`
- Nh?p password: `hashed_password_1`
- Click **??NG NH?P**

### 3. V�o App

Sau khi ??ng nh?p th�nh c�ng:
- ? Th?y message box: "Xin ch�o, Nguy?n V?n An! ??ng nh?p th�nh c�ng!"
- ? V�o Form1
- ? Th?y label: "Xin ch�o, Nguy?n V?n An! ??" ? g�c tr�n b�n ph?i

### 4. Th? V?i User Kh�c

- Close app
- Ch?y l?i
- ??ng nh?p v?i user kh�c
- Th?y t�n user kh�c!

---

## ?? **Code Quan Tr?ng**

### UserSession.cs
```csharp
// L?y user hi?n t?i
var currentUser = UserSession.CurrentUser;

// L?y t�n hi?n th?
var displayName = UserSession.GetDisplayName();

// L?y UserId
var userId = UserSession.GetUserId();

// Ki?m tra ?� ??ng nh?p ch?a
if (UserSession.IsLoggedIn)
{
    // User ?� ??ng nh?p
}

// ??ng xu?t
UserSession.Logout();
```

### LoginForm.cs
```csharp
// Khi login th�nh c�ng:
UserSession.Login(user);

// Show welcome
MessageBox.Show(
    $"Xin ch�o, {user.FullName}!
??ng nh?p th�nh c�ng!",
    "Ch�o m?ng"
);
```

### Form1.cs
```csharp
// Hi?n th? t�n user
lblWelcome.Text = $"Xin ch�o, {UserSession.GetDisplayName()}! ??";
```

---

## ?? **T�y Ch?nh**

### Thay ??i V? Tr� Welcome Label

Trong `Form1.cs`:

```csharp
lblWelcome = new Label
{
    Text = $"Xin ch�o, {UserSession.GetDisplayName()}! ??",
    Location = new Point(370, 15), // Thay ??i ? ?�y
    Size = new Size(400, 30),
    // ...
};
```

### Thay ??i Giao Di?n Login Form

Trong `LoginForm.cs`, method `SetupCustomUI()`:

```csharp
// Thay ??i m�u s?c
btnLogin.BackColor = Color.FromArgb(52, 152, 219); // M�u xanh

// Thay ??i font
lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);

// Thay ??i k�ch th??c
this.ClientSize = new Size(450, 550);
```

---

## ?? **B?o M?t (Production)**

### ?? L?U �:

Code hi?n t?i d�ng **PLAIN TEXT PASSWORD** ?? test!

```csharp
// ? KH�NG AN TO�N (hi?n t?i)
if (user.PasswordHash != txtPassword.Text.Trim())

// ? N�N D�NG (production)
if (user.PasswordHash != HashPassword(txtPassword.Text.Trim()))
```

### ?? Hash Password Trong Production:

```csharp
using System.Security.Cryptography;
using System.Text;

public static string HashPassword(string password)
{
    using (SHA256 sha256 = SHA256.Create())
    {
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
```

Sau ?� update seeder ?? hash password:

```csharp
new User
{
    FullName = "Nguy?n V?n An",
    Email = "nguyenvanan@example.com",
    PasswordHash = HashPassword("123456"), // Hash password
    // ...
}
```

---

## ?? **T�nh N?ng B? Sung (T�y Ch?n)**

### 1. Th�m "Remember Me"

```csharp
// Trong LoginForm.cs
private CheckBox chkRememberMe;

// L?u email
Properties.Settings.Default.RememberedEmail = txtEmail.Text;
Properties.Settings.Default.Save();

// Load email
txtEmail.Text = Properties.Settings.Default.RememberedEmail;
```

### 2. Th�m N�t "??ng Xu?t"

```csharp
// Trong Form1.cs
Button btnLogout = new Button
{
    Text = "??ng xu?t",
    Location = new Point(780, 15),
    // ...
};
btnLogout.Click += (s, e) =>
{
    UserSession.Logout();
    Application.Restart();
};
```

### 3. Th�m "Forgot Password"

```csharp
// Trong LoginForm.cs
LinkLabel lnkForgotPassword = new LinkLabel
{
    Text = "Qu�n m?t kh?u?",
    Location = new Point(25, 285),
    // ...
};
lnkForgotPassword.Click += (s, e) =>
{
    // TODO: Implement forgot password
    MessageBox.Show("T�nh n?ng ?ang ph�t tri?n!");
};
```

---

## ? **Ki?m Tra Ho�n T?t**

- [x] Login form hi?n th? khi ch?y app
- [x] Validate email & password
- [x] Ki?m tra user trong database
- [x] Set UserSession khi login th�nh c�ng
- [x] Hi?n th? "Xin ch�o, [T�n]!" trong Form1
- [x] Build th�nh c�ng

---

## ?? **HO�N TH�NH!**

B�y gi? b?n c�:
- ? M�n h�nh ??ng nh?p ??p
- ? Qu?n l� session user
- ? Welcome message v?i t�n user
- ? S?n s�ng ?? ph�t tri?n th�m!

---

## ?? **Files ?� Thay ??i/T?o:**

### M?i:
1. `ToDoList.GUI/Forms/LoginForm.cs`
2. `ToDoList.GUI/Helpers/UserSession.cs`

### S?a:
1. `ToDoList.GUI/Program.cs` - Th�m login form
2. `ToDoList.GUI/Form1.cs` - Th�m welcome label

---

**?? Ch?y app v� test ??ng nh?p v?i 4 users kh�c nhau!**
