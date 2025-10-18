# Test Database Seeder - PowerShell Version
$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   TEST DATABASE SEEDER" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Set location
Set-Location $PSScriptRoot

# Test 1: .NET SDK
Write-Host "[1] Ki?m tra .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host "? .NET SDK: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "? .NET SDK không tìm th?y!" -ForegroundColor Red
    pause
    exit 1
}

# Test 2: Project file
Write-Host "[2] Ki?m tra project file..." -ForegroundColor Yellow
if (Test-Path "TodoListApp.DAL\TodoListApp.DAL.csproj") {
    Write-Host "? Project file t?n t?i" -ForegroundColor Green
} else {
    Write-Host "? Không tìm th?y TodoListApp.DAL.csproj" -ForegroundColor Red
    pause
    exit 1
}

# Test 3: SeedProgram.cs
Write-Host "[3] Ki?m tra SeedProgram.cs..." -ForegroundColor Yellow
if (Test-Path "TodoListApp.DAL\SeedProgram.cs") {
    Write-Host "? SeedProgram.cs t?n t?i" -ForegroundColor Green
} else {
    Write-Host "? Không tìm th?y SeedProgram.cs" -ForegroundColor Red
    pause
    exit 1
}

# Test 4: DatabaseSeeder.cs
Write-Host "[4] Ki?m tra DatabaseSeeder.cs..." -ForegroundColor Yellow
if (Test-Path "TodoListApp.DAL\Data\DatabaseSeeder.cs") {
    Write-Host "? DatabaseSeeder.cs t?n t?i" -ForegroundColor Green
} else {
    Write-Host "? Không tìm th?y DatabaseSeeder.cs" -ForegroundColor Red
    pause
    exit 1
}

# Test 5: Connection String
Write-Host "[5] Ki?m tra appsettings.json..." -ForegroundColor Yellow
if (Test-Path "TodoListApp.DAL\appsettings.json") {
    $appSettings = Get-Content "TodoListApp.DAL\appsettings.json" | ConvertFrom-Json
    $connectionString = $appSettings.ConnectionStrings.DefaultConnection
    Write-Host "? Connection String: $connectionString" -ForegroundColor Green
} else {
    Write-Host "??  appsettings.json không tìm th?y" -ForegroundColor Yellow
}

# Test 6: Build
Write-Host ""
Write-Host "[6] Build project..." -ForegroundColor Yellow
try {
    dotnet build TodoListApp.DAL\TodoListApp.DAL.csproj --configuration Debug
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Build thành công" -ForegroundColor Green
    } else {
        throw "Build failed with exit code $LASTEXITCODE"
    }
} catch {
    Write-Host "? Build th?t b?i: $_" -ForegroundColor Red
    pause
    exit 1
}

# Test 7: Run
Write-Host ""
Write-Host "[7] Ch?y seeder..." -ForegroundColor Yellow
Write-Host "Nh?n Enter ?? ti?p t?c ho?c Ctrl+C ?? h?y..." -ForegroundColor Cyan
Read-Host

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   ?ANG CH?Y SEEDER..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

try {
    # T? ??ng nh?p input
    "3`nyes" | dotnet run --project TodoListApp.DAL\TodoListApp.DAL.csproj
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Green
        Write-Host "   ? THÀNH CÔNG!" -ForegroundColor Green
        Write-Host "========================================" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Red
        Write-Host "   ? CÓ L?I X?Y RA" -ForegroundColor Red
        Write-Host "========================================" -ForegroundColor Red
    }
} catch {
    Write-Host "? L?i: $_" -ForegroundColor Red
}

Write-Host ""
pause
