@echo off
chcp 65001 > nul
echo ========================================
echo   TEST DATABASE SEEDER
echo ========================================
echo.

cd /d "%~dp0"

echo [1] Ki?m tra .NET SDK...
dotnet --version
if %errorlevel% neq 0 (
    echo ? .NET SDK kh�ng t�m th?y!
    pause
    exit /b 1
)
echo ? .NET SDK OK
echo.

echo [2] Ki?m tra project file...
if not exist "TodoListApp.DAL\TodoListApp.DAL.csproj" (
    echo ? Kh�ng t�m th?y TodoListApp.DAL.csproj
    pause
    exit /b 1
)
echo ? Project file t?n t?i
echo.

echo [3] Ki?m tra SeedProgram.cs...
if not exist "TodoListApp.DAL\SeedProgram.cs" (
    echo ? Kh�ng t�m th?y SeedProgram.cs
    pause
    exit /b 1
)
echo ? SeedProgram.cs t?n t?i
echo.

echo [4] Build project...
dotnet build TodoListApp.DAL\TodoListApp.DAL.csproj --configuration Debug --verbosity detailed
if %errorlevel% neq 0 (
    echo.
    echo ? Build th?t b?i!
    echo Xem chi ti?t l?i ? tr�n
    pause
    exit /b %errorlevel%
)
echo ? Build th�nh c�ng
echo.

echo [5] Ch?y seeder...
echo.
echo Nh?n Ctrl+C ?? h?y, ho?c Enter ?? ti?p t?c...
pause > nul

dotnet run --project TodoListApp.DAL\TodoListApp.DAL.csproj --verbosity detailed

echo.
echo ========================================
echo Ki?m tra xem c� l?i g� ? tr�n kh�ng
echo ========================================
pause
