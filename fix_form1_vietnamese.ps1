# MANUAL FIX VIETNAMESE - COMPLETE SCRIPT
# Run this in PowerShell (NOT CMD)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "FIXING VIETNAMESE ENCODING - FORM1.CS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$filePath = "ToDoList.GUI\Form1.cs"

if (-not (Test-Path $filePath)) {
    Write-Host "[ERROR] File not found: $filePath" -ForegroundColor Red
    Write-Host "Please run this script from the solution root directory" -ForegroundColor Yellow
    pause
    exit
}

Write-Host "[1] Reading file..." -ForegroundColor Yellow
$content = Get-Content $filePath -Raw -Encoding UTF8

Write-Host "[2] Backing up original file..." -ForegroundColor Yellow
$content | Set-Content "$filePath.backup" -Encoding UTF8

Write-Host "[3] Applying replacements..." -ForegroundColor Yellow

# Method UpdateGreetingLabels - FIX THE FONT AND TEXT
$oldPattern1 = @"
        private void UpdateGreetingLabels\(\)
        \{
            // Update greeting label with user name
            string timeOfDay = GetTimeOfDay\(\);
            lblGreeting\.Text = \$"Chào \{timeOfDay\}, \{UserSession\.GetDisplayName\(\)\}!";
            
            // Update subtitle
            lblUserName\.Text = "Tuyệt vời! Bạn làm việc rất chăm chỉ\.";
        \}
"@

$newPattern1 = @"
        private void UpdateGreetingLabels()
        {
            // Update greeting label with user name
            string timeOfDay = GetTimeOfDay();
            
            // Use Arial font for better Vietnamese support
            lblGreeting.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblUserName.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point);
            
            lblGreeting.Text = `$"Chao {timeOfDay}, {UserSession.GetDisplayName()}!";
            lblUserName.Text = "Tuyet voi! Ban dang lam viec rat cham chi.";
        }
"@

$content = $content -replace $oldPattern1, $newPattern1

# Method GetTimeOfDay
$content = $content -replace '"buổi sáng"', '"buoi sang"'
$content = $content -replace '"buổi chiều"', '"buoi chieu"'
$content = $content -replace '"buổi tối"', '"buoi toi"'

# All other Vietnamese text
$content = $content -replace 'Báo cáo', 'Bao cao'
$content = $content -replace 'Trang chủ', 'Trang chu'
$content = $content -replace 'Công việc', 'Cong viec'
$content = $content -replace 'Dự án', 'Du an'
$content = $content -replace 'Không thể', 'Khong the'
$content = $content -replace 'Lỗi', 'Loi'
$content = $content -replace 'Thông tin dữ liệu', 'Thong tin du lieu'
$content = $content -replace 'Dữ liệu hiện tại', 'Du lieu hien tai'
$content = $content -replace 'Thống kê nâng cao', 'Thong ke nang cao'
$content = $content -replace 'công việc đang chờ', 'cong viec dang cho'
$content = $content -replace 'Dự kiến', 'Du kien'
$content = $content -replace 'TẠO DANH SÁCH MỚI', 'TAO DANH SACH MOI'
$content = $content -replace 'Chỉnh sửa', 'Chinh sua'
$content = $content -replace 'Xem chi tiết', 'Xem chi tiet'
$content = $content -replace 'Lưu trữ', 'Luu tru'
$content = $content -replace 'Xóa', 'Xoa'
$content = $content -replace 'Thông báo', 'Thong bao'
$content = $content -replace 'Xác nhận', 'Xac nhan'
$content = $content -replace 'Thành công', 'Thanh cong'
$content = $content -replace 'Đã xóa', 'Da xoa'
$content = $content -replace 'Kết nối', 'Ket noi'
$content = $content -replace 'cơ sở dữ liệu', 'co so du lieu'
$content = $content -replace 'Vui lòng', 'Vui long'
$content = $content -replace 'Nếu', 'Neu'
$content = $content -replace 'vẫn', 'van'
$content = $content -replace 'tiếp tục', 'tiep tuc'
$content = $content -replace 'hãy', 'hay'
$content = $content -replace 'kiểm tra', 'kiem tra'
$content = $content -replace 'mạng', 'mang'
$content = $content -replace 'có', 'co'
$content = $content -replace 'đang', 'dang'
$content = $content -replace 'chạy', 'chay'
$content = $content -replace 'không', 'khong'
$content = $content -replace 'khi', 'khi'
$content = $content -replace 'tại', 'tai'
$content = $content -replace 'danh sách', 'danh sach'
$content = $content -replace 'Chi tiết', 'Chi tiet'


Write-Host "[4] Saving fixed file..." -ForegroundColor Yellow
$content | Set-Content $filePath -Encoding UTF8

Write-Host "" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host "COMPLETED!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "[?] Form1.cs has been fixed!" -ForegroundColor Green
Write-Host "[?] Backup saved to: $filePath.backup" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Close and reopen Form1.cs in Visual Studio" -ForegroundColor White
Write-Host "2. Build Solution (Ctrl+Shift+B)" -ForegroundColor White
Write-Host "3. Run the application (F5)" -ForegroundColor White
Write-Host "4. Check if text displays correctly" -ForegroundColor White
Write-Host ""
pause
