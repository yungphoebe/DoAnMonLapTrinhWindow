@echo off
echo ?? Setting up Python Charts for ToDoList...
echo.

REM Get the current directory
set "SCRIPT_DIR=%~dp0"
set "PROJECT_DIR=%SCRIPT_DIR%.."
set "BUILD_DIR=%PROJECT_DIR%\ToDoList.GUI\bin\Debug\net9.0-windows"
set "PYTHON_SOURCE=%PROJECT_DIR%\python_charts"
set "PYTHON_DEST=%BUILD_DIR%\python_charts"

echo ?? Project Directory: %PROJECT_DIR%
echo ?? Build Directory: %BUILD_DIR%
echo ?? Python Source: %PYTHON_SOURCE%
echo ?? Python Destination: %PYTHON_DEST%
echo.

REM Check if Python is installed
echo ?? Checking Python installation...
python --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ? Python not found! Please install Python first.
    echo ?? Download from: https://www.python.org/downloads/
    echo.
    echo ?? After installing Python, add it to your PATH and restart this script.
    pause
    exit /b 1
)

echo ? Python found!
echo.

REM Create build directory if it doesn't exist
if not exist "%BUILD_DIR%" (
    echo ?? Creating build directory...
    mkdir "%BUILD_DIR%"
)

REM Create python_charts directory in build folder
if not exist "%PYTHON_DEST%" (
    echo ?? Creating python_charts directory in build folder...
    mkdir "%PYTHON_DEST%"
)

REM Copy Python files to build directory
echo ?? Copying Python files to build directory...
xcopy "%PYTHON_SOURCE%\*" "%PYTHON_DEST%\" /Y /E /I
if %ERRORLEVEL% NEQ 0 (
    echo ? Failed to copy Python files
    pause
    exit /b 1
)

echo ? Python files copied successfully!
echo.

REM Install required packages
echo ?? Installing required Python packages...
cd /d "%PYTHON_DEST%"
pip install -r requirements.txt
if %ERRORLEVEL% NEQ 0 (
    echo ? Failed to install packages. Trying with --user flag...
    pip install -r requirements.txt --user
    if %ERRORLEVEL% NEQ 0 (
        echo ? Failed to install packages even with --user flag
        echo ?? Try running: pip install matplotlib seaborn pandas pyodbc plotly
        pause
        exit /b 1
    )
)

echo.
echo ? Python Charts setup completed successfully!
echo.
echo ?? Files installed in: %PYTHON_DEST%
echo.
echo ?? You can now use Python Charts from the C# application:
echo    1. Run the ToDoList application
echo    2. Right-click on any project card
echo    3. Select "?? Python Charts"  
echo    4. Beautiful charts will be generated!
echo.
echo ?? Available files:
dir "%PYTHON_DEST%" /B

echo.
pause