@echo off
echo ?? Setting up Python Charts for ToDoList...
echo.

REM Check if Python is installed
python --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ? Python not found! Please install Python first.
    echo ?? Download from: https://www.python.org/downloads/
    pause
    exit /b 1
)

echo ? Python found!
echo.

REM Install required packages
echo ?? Installing required Python packages...
pip install -r requirements.txt

if %ERRORLEVEL% NEQ 0 (
    echo ? Failed to install packages
    pause
    exit /b 1
)

echo.
echo ? Python Charts setup completed!
echo.
echo ?? You can now use Python Charts from the C# application:
echo    1. Right-click on any project card
echo    2. Select "?? Python Charts"
echo    3. Beautiful charts will be generated!
echo.
pause