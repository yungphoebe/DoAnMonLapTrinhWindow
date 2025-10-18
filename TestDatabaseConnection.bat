@echo off
echo ============================================
echo KIEM TRA KET NOI DATABASE
echo ============================================
echo.

sqlcmd -S DESKTOP-LN5QDF6\SQLEXPRESS -E -Q "SELECT name FROM sys.databases WHERE name = 'ToDoListApp'" -h -1

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [SUCCESS] Ket noi database thanh cong!
    echo Server: DESKTOP-LN5QDF6\SQLEXPRESS
    echo Database: ToDoListApp
    echo.
    
    echo Kiem tra cac bang:
    sqlcmd -S DESKTOP-LN5QDF6\SQLEXPRESS -d ToDoListApp -E -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME"
) else (
    echo.
    echo [ERROR] Khong the ket noi database!
    echo.
    echo Vui long kiem tra:
    echo 1. SQL Server da khoi dong chua?
    echo 2. Ten server co dung khong?
    echo 3. Database 'ToDoListApp' da ton tai chua?
)

echo.
pause
