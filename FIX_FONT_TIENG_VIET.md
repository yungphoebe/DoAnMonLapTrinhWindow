# H??ng d?n s?a l?i hi?n th? ti?ng Vi?t (Font Encoding)

## V?n ??
- ?ng d?ng hi?n th? d?u `??` thay v� ch? ti?ng Vi?t
- Nguy�n nh�n: File source code b? l?i encoding ho?c font kh�ng h? tr? ti?ng Vi?t

## Gi?i ph�p ?� �p d?ng

### 1. Thay ??i Font ch?
?� thay ??i t? `Segoe UI` sang `Arial Unicode MS` - font h? tr? ??y ?? ti?ng Vi?t:

```csharp
// Tr??c (l?i):
Font = new Font("Segoe UI", 16F, FontStyle.Bold)

// Sau (?� s?a):
Font = new Font("Arial Unicode MS", 16F, FontStyle.Bold)
```

### 2. File ?� ???c s?a
- ? `AdvancedReportsForm.cs` - ?� c?p nh?t to�n b? font v� text ti?ng Vi?t

### 3. C�c thay ??i ch�nh

#### Tr??c (hi?n th? sai):
```csharp
Text = "Th?ng k� n�ng cao - ToDoList Analytics"
Text = "T?ng Projects"
Text = "T?ng quan"
```

#### Sau (hi?n th? ?�ng):
```csharp
Text = "Th?ng k� n�ng cao - ToDoList Analytics"
Text = "T?ng Projects"
Text = "T?ng quan"
```

## Ki?m tra sau khi s?a

1. **Build l?i project**: ? Successful
2. **Ch?y ?ng d?ng** v� ki?m tra:
   - Form "Th?ng k� n�ng cao" hi?n th? ?�ng ti?ng Vi?t
   - C�c tab: "T?ng quan", "N?ng su?t", "Ti?n ??", "Th?i gian" hi?n th? ?�ng
   - C�c label stat cards hi?n th? ?�ng

## N?u v?n g?p l?i font ? form kh�c

### B??c 1: T�m ki?m text b? l?i
```csharp
// T�m c�c pattern sau trong code:
"Th?ng"  // Th?ng
"T?ng"   // T?ng
"???"    // B?t k? d?u ? n�o
```

### B??c 2: Thay ??i font
```csharp
// Thay ??i t? Segoe UI sang Arial Unicode MS
Font = new Font("Arial Unicode MS", [size]F, FontStyle.[style])
```

### B??c 3: S?a text encoding
- ??m b?o file .cs ???c save v?i encoding UTF-8 BOM
- Trong Visual Studio: File > Advanced Save Options > UTF-8 with BOM

## Font khuy?n ngh? cho ti?ng Vi?t

1. **Arial Unicode MS** ? (?ang d�ng)
   - H? tr? ??y ?? ti?ng Vi?t
   - C� s?n trong Windows
   - R� r�ng, d? ??c

2. **Segoe UI** (C?n c�i ??t g�i ng�n ng?)
   - Font m?c ??nh c?a Windows
   - C?n c�i g�i ti?ng Vi?t

3. **Tahoma**
   - C? h?n nh?ng t??ng th�ch t?t
   - H? tr? ti?ng Vi?t

## Checklist sau khi s?a

- [x] Build successful
- [x] Kh�ng c� l?i compilation
- [x] Ti?ng Vi?t hi?n th? ?�ng trong AdvancedReportsForm
- [ ] Ki?m tra c�c form kh�c
- [ ] Test tr�n m�y ng??i d�ng cu?i

## L?u � quan tr?ng

?? **Lu�n s? d?ng Arial Unicode MS ho?c font h? tr? ti?ng Vi?t khi t?o form m?i**

?? **Save file v?i encoding UTF-8 BOM trong Visual Studio**

?? **Test tr�n nhi?u m�i tr??ng kh�c nhau**

## Li�n h? h? tr?

N?u v?n g?p v?n ??, h�y:
1. Ki?m tra font ?� ???c c�i ??t tr�n m�y ch?a
2. Rebuild to�n b? solution
3. X�a folder `bin` v� `obj` r?i build l?i

---
*C?p nh?t: $(Get-Date -Format "dd/MM/yyyy HH:mm")*
