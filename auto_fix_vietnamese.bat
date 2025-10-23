@echo off
chcp 65001 >nul
echo ========================================
echo AUTO FIX VIETNAMESE ENCODING
echo ========================================
echo.

echo [1/3] Fixing Program.cs...
powershell -Command "(Get-Content 'ToDoList.GUI\Program.cs' -Raw) -replace '??c c?u hình t?', 'Doc cau hinh tu' -replace 'Test k?t n?i database n?u ???c b?t', 'Test ket noi database neu duoc bat' -replace 'Dùng Custom Form v?i font ti?ng Vi?t ??p', 'Dung Custom Form voi font tieng Viet dep' -replace 'Dùng Console - C?n thi?t l?p', 'Dung Console - Can thiet lap' -replace 'Nh?n phím b?t k? ?? m? ?ng d?ng', 'Nhan phim bat ky de mo ung dung' -replace 'Hi?n th? màn hình ??ng nh?p tr??c', 'Hien thi man hinh dang nhap truoc' -replace 'N?u ??ng nh?p thành công, ch?y Form1', 'Neu dang nhap thanh cong, chay Form1' -replace 'N?u h?y ??ng nh?p, thoát ?ng d?ng', 'Neu huy dang nhap, thoat ung dung' | Set-Content 'ToDoList.GUI\Program.cs' -Encoding UTF8"
echo [OK] Program.cs fixed!

echo.
echo [2/3] Fixing Form1.cs - UpdateGreetingLabels()...
powershell -Command "$content = Get-Content 'ToDoList.GUI\Form1.cs' -Raw; $content = $content -replace 'lblGreeting\.Text = \$\"Chào \{timeOfDay\}, \{UserSession\.GetDisplayName\(\)\}!\";', 'lblGreeting.Font = new Font(\"Arial\", 16F, FontStyle.Bold, GraphicsUnit.Point);`r`n            lblUserName.Font = new Font(\"Arial\", 10F, FontStyle.Regular, GraphicsUnit.Point);`r`n            lblGreeting.Text = $\"Chao {timeOfDay}, {UserSession.GetDisplayName()}!\";'; $content = $content -replace 'lblUserName\.Text = \"Tuy?t v?i! B?n ?ang làm vi?c r?t ch?m ch?.\";', 'lblUserName.Text = \"Tuyet voi! Ban dang lam viec rat cham chi.\";'; $content = $content -replace '\"bu?i sáng\"', '\"buoi sang\"'; $content = $content -replace '\"bu?i chi?u\"', '\"buoi chieu\"'; $content = $content -replace '\"bu?i t?i\"', '\"buoi toi\"'; Set-Content 'ToDoList.GUI\Form1.cs' -Value $content -Encoding UTF8"
echo [OK] Form1.cs fixed!

echo.
echo [3/3] Fixing all other Vietnamese text in Form1.cs...
powershell -Command "$content = Get-Content 'ToDoList.GUI\Form1.cs' -Raw; $content = $content -replace 'Báo cáo', 'Bao cao'; $content = $content -replace 'Trang ch?', 'Trang chu'; $content = $content -replace 'Công vi?c', 'Cong viec'; $content = $content -replace 'D? án', 'Du an'; $content = $content -replace 'Không th?', 'Khong the'; $content = $content -replace 'L?i', 'Loi'; $content = $content -replace 'Thông tin d? li?u', 'Thong tin du lieu'; $content = $content -replace 'D? li?u hi?n t?i', 'Du lieu hien tai'; $content = $content -replace 'Th?ng kê nâng cao', 'Thong ke nang cao'; $content = $content -replace 'công vi?c ?ang ch?', 'cong viec dang cho'; $content = $content -replace 'D? ki?n', 'Du kien'; $content = $content -replace 'T?O DANH SÁCH M?I', 'TAO DANH SACH MOI'; $content = $content -replace 'Ch?nh s?a', 'Chinh sua'; $content = $content -replace 'Xem chi ti?t', 'Xem chi tiet'; $content = $content -replace 'L?u tr?', 'Luu tru'; $content = $content -replace 'Xóa', 'Xoa'; $content = $content -replace 'Thông báo', 'Thong bao'; $content = $content -replace 'Xác nh?n', 'Xac nhan'; $content = $content -replace 'Thành công', 'Thanh cong'; $content = $content -replace '?ã xóa', 'Da xoa'; Set-Content 'ToDoList.GUI\Form1.cs' -Value $content -Encoding UTF8"
echo [OK] All Vietnamese text fixed!

echo.
echo ========================================
echo COMPLETED! All files have been fixed.
echo ========================================
echo.
echo Next steps:
echo 1. Build Solution (Ctrl+Shift+B)
echo 2. Run the application (F5)
echo 3. Check if the display is correct
echo.
pause
