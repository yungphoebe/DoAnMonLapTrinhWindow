# FIX ADVANCED REPORTS FORM - COMPLETE AUTOMATIC FIX
# Run this in PowerShell (NOT CMD)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "FIXING ADVANCED REPORTS FORM - FULL AUTO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$filePath = "ToDoList.GUI\Forms\AdvancedReportsForm.cs"

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

Write-Host "[3] Fixing Form Title..." -ForegroundColor Yellow
# Fix form title - remove question marks
$content = $content -replace 'this\.Text\s*=\s*"[^"]*Th?ng kê[^"]*"', 'this.Text = "Thong ke nang cao - ToDoList Analytics"'

Write-Host "[4] Fixing Tab Names..." -ForegroundColor Yellow
# Fix tab names
$content = $content -replace '"T?ng quan"', '"Tong quan"'
$content = $content -replace '"Bi?u ??"', '"Bieu do"'
$content = $content -replace '"Th?ng kê"', '"Thong ke"'
$content = $content -replace '"Báo cáo"', '"Bao cao"'

Write-Host "[5] Fixing Stats Card Labels..." -ForegroundColor Yellow
# Fix stats card text - IMPORTANT: Replace exact patterns
$content = $content -replace '"T?ng Projects"', '"Tong Projects"'
$content = $content -replace '"T\?ng Projects"', '"Tong Projects"'
$content = $content -replace '"T?ng Tasks"', '"Tong Tasks"'
$content = $content -replace '"T\?ng Tasks"', '"Tong Tasks"'
$content = $content -replace '"Hoàn thành"', '"Hoan thanh"'
$content = $content -replace '"Hoàn th[^"]*nh"', '"Hoan thanh"'
$content = $content -replace '"Th?i gian"', '"Thoi gian"'
$content = $content -replace '"Th\?i gian"', '"Thoi gian"'

Write-Host "[6] Fixing Section Headers..." -ForegroundColor Yellow
# Fix section headers with question marks
$content = $content -replace '"[^"]*Ho?t ??ng g?n ?ây[^"]*"', '"Hoat dong gan day"'
$content = $content -replace '"[^"]*Ho\?t [^"]*ng g\?n [^"]*"', '"Hoat dong gan day"'
$content = $content -replace '"[^"]*Ti?n ?? theo d? án[^"]*"', '"Tien do theo du an"'
$content = $content -replace '"[^"]*Ti\?n [^"]* theo d\? [^"]*"', '"Tien do theo du an"'

Write-Host "[7] Fixing All Vietnamese Text..." -ForegroundColor Yellow
# All Vietnamese text replacements
$vietnameseReplacements = @{
    'Không có d? li?u' = 'Khong co du lieu'
    'D? li?u' = 'Du lieu'
    'Công vi?c' = 'Cong viec'
    'D? án' = 'Du an'
    'Th?ng kê' = 'Thong ke'
    'Báo cáo' = 'Bao cao'
    'T?ng s?' = 'Tong so'
    'Hoàn thành' = 'Hoan thanh'
    '?ang làm' = 'Dang lam'
    'Ch?a làm' = 'Chua lam'
    'Ti?n ??' = 'Tien do'
    'Ho?t ??ng' = 'Hoat dong'
    'G?n ?ây' = 'Gan day'
    'Theo d? án' = 'Theo du an'
    'Phát tri?n' = 'Phat trien'
    'Thi?t k?' = 'Thiet ke'
    'Testing' = 'Testing'
    'Implement' = 'Implement'
    'Ch?y' = 'Chay'
    'T?i ?u' = 'Toi uu'
    'Giao di?n' = 'Giao dien'
}

foreach ($key in $vietnameseReplacements.Keys) {
    $value = $vietnameseReplacements[$key]
    $content = $content -replace [regex]::Escape($key), $value
}

Write-Host "[8] Fixing Fonts to Arial..." -ForegroundColor Yellow
# Replace Segoe UI with Arial for better Vietnamese support
$content = $content -replace 'new Font\("Segoe UI",\s*(\d+)F,\s*FontStyle\.Bold\)', 'new Font("Arial", $1F, FontStyle.Bold, GraphicsUnit.Point)'
$content = $content -replace 'new Font\("Segoe UI",\s*(\d+)F,\s*FontStyle\.Regular\)', 'new Font("Arial", $1F, FontStyle.Regular, GraphicsUnit.Point)'
$content = $content -replace 'new Font\("Segoe UI",\s*(\d+)F\)', 'new Font("Arial", $1F, FontStyle.Regular, GraphicsUnit.Point)'

Write-Host "[9] Ensuring Chart Initialization..." -ForegroundColor Yellow
# Ensure charts are properly initialized
$chartInitPattern = 'var chart = new InteractiveChartControl\(\);'
$chartInitReplacement = @'
var chart = new InteractiveChartControl
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(25, 25, 25),
                Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point)
            };
'@
$content = $content -replace [regex]::Escape($chartInitPattern), $chartInitReplacement

Write-Host "[10] Saving fixed file..." -ForegroundColor Yellow
$content | Set-Content $filePath -Encoding UTF8

Write-Host "" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host "COMPLETED!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "[OK] AdvancedReportsForm.cs has been fixed!" -ForegroundColor Green
Write-Host "[OK] Backup saved to: $filePath.backup" -ForegroundColor Green
Write-Host ""
Write-Host "Changes made:" -ForegroundColor Yellow
Write-Host "- Fixed form title encoding" -ForegroundColor White
Write-Host "- Fixed tab names (Tong quan, Bieu do, etc.)" -ForegroundColor White
Write-Host "- Fixed stats cards (Tong Projects, Hoan thanh, etc.)" -ForegroundColor White
Write-Host "- Fixed section headers (Hoat dong gan day, etc.)" -ForegroundColor White
Write-Host "- Changed fonts from Segoe UI to Arial" -ForegroundColor White
Write-Host "- Ensured chart controls are properly initialized" -ForegroundColor White
Write-Host "- Fixed ALL Vietnamese text with diacritics" -ForegroundColor White
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Close AdvancedReportsForm.cs in Visual Studio if open" -ForegroundColor White
Write-Host "2. Build Solution (Ctrl+Shift+B)" -ForegroundColor White
Write-Host "3. Run the application (F5)" -ForegroundColor White
Write-Host "4. Open Advanced Reports (Thong ke nang cao)" -ForegroundColor White
Write-Host "5. Verify all text displays correctly WITHOUT question marks" -ForegroundColor White
Write-Host ""
Write-Host "Expected results:" -ForegroundColor Cyan
Write-Host "Title: 'Thong ke nang cao - ToDoList Analytics'" -ForegroundColor White
Write-Host "Stats: 'Tong Projects', 'Tong Tasks', 'Hoan thanh', 'Thoi gian'" -ForegroundColor White
Write-Host "Sections: 'Hoat dong gan day', 'Tien do theo du an'" -ForegroundColor White
Write-Host ""
pause
