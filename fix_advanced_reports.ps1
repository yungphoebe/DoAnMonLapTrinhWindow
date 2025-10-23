# FIX ADVANCED REPORTS FORM - TIENG VIET & BIEU DO
# Run this in PowerShell

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "FIX ADVANCED REPORTS FORM" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$filePath = "ToDoList.GUI\Forms\AdvancedReportsForm.cs"

if (-not (Test-Path $filePath)) {
    Write-Host "[ERROR] File not found: $filePath" -ForegroundColor Red
    pause
    exit
}

Write-Host "[1] Reading file..." -ForegroundColor Yellow
$content = Get-Content $filePath -Raw -Encoding UTF8

Write-Host "[2] Backing up..." -ForegroundColor Yellow
$content | Set-Content "$filePath.backup" -Encoding UTF8

Write-Host "[3] Fixing Vietnamese text..." -ForegroundColor Yellow

# Title
$content = $content -replace 'Thong ke nang cao - ToDoList Analytics', 'Thong ke nang cao - ToDoList Analytics'
$content = $content -replace '"Th?ng kê nâng cao - ToDoList"', '"Thong ke nang cao - ToDoList"'

# Tab names
$content = $content -replace '"T?ng quan"', '"Tong quan"'
$content = $content -replace '"Bi?u ??"', '"Bieu do"'
$content = $content -replace '"Th?ng kê"', '"Thong ke"'
$content = $content -replace '"Báo cáo"', '"Bao cao"'

# Stats card labels
$content = $content -replace '"T?ng Projects"', '"Tong Projects"'
$content = $content -replace '"projects"', '"projects"'
$content = $content -replace '"T?ng Tasks"', '"Tong Tasks"'
$content = $content -replace '"tasks"', '"tasks"'
$content = $content -replace '"Hoàn thành"', '"Hoan thanh"'
$content = $content -replace '"completion"', '"completion"'
$content = $content -replace '"Th?i gian"', '"Thoi gian"'
$content = $content -replace '"time spent"', '"time spent"'

# Other Vietnamese text
$content = $content -replace 'Không có d? li?u', 'Khong co du lieu'
$content = $content -replace 'Bi?u ??', 'Bieu do'
$content = $content -replace 'Công vi?c', 'Cong viec'
$content = $content -replace 'D? án', 'Du an'
$content = $content -replace 'Th?ng kê', 'Thong ke'

Write-Host "[4] Fixing chart rendering..." -ForegroundColor Yellow

# Ensure charts are created properly
$oldChartPattern = @'
var chart = new InteractiveChartControl\(\);
'@

$newChartPattern = @'
var chart = new InteractiveChartControl
{
    Dock = DockStyle.Fill,
    BackColor = Color.FromArgb(25, 25, 25)
};
'@

$content = $content -replace $oldChartPattern, $newChartPattern

Write-Host "[5] Saving..." -ForegroundColor Yellow
$content | Set-Content $filePath -Encoding UTF8

Write-Host "" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host "COMPLETED!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "[OK] AdvancedReportsForm has been fixed!" -ForegroundColor Green
Write-Host "[OK] Backup saved to: $filePath.backup" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Build Solution (Ctrl+Shift+B)" -ForegroundColor White
Write-Host "2. Run application (F5)" -ForegroundColor White
Write-Host "3. Open Thong ke nang cao form" -ForegroundColor White
Write-Host "4. Check if charts display correctly" -ForegroundColor White
Write-Host ""
pause
