@echo off
chcp 65001 > nul
echo ========================================
echo   ToDoList App - Seed Database
echo ========================================
echo.

cd /d "%~dp0"

echo Chu?n b? môi tr??ng...
REM Copy appsettings.json n?u ch?a có
if not exist "appsettings.json" (
    if exist "TodoListApp.DAL\appsettings.json" (
        copy "TodoListApp.DAL\appsettings.json" "appsettings.json" > nul
        echo ? ?ã copy appsettings.json
    )
)
echo.

echo ?ang build project...
dotnet build TodoListApp.DAL\TodoListApp.DAL.csproj --configuration Debug
if %errorlevel% neq 0 (
    echo.
    echo ? Build failed!
    pause
    exit /b %errorlevel%
)

echo.
echo ?ang ch?y Database Seeder...
echo.

dotnet run --project TodoListApp.DAL\TodoListApp.DAL.csproj

echo.
pause
