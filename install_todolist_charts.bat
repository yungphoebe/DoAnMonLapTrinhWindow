@echo off
chcp 65001 >nul
echo ?? ToDoList Python Charts Auto-Installer
echo ==========================================
echo.

REM Check if running as administrator
net session >nul 2>&1
if %errorLevel% == 0 (
    echo ? Running as Administrator
) else (
    echo ??  Requesting Administrator privileges...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)

echo ?? Checking Python installation...

REM Try different Python commands
set PYTHON_CMD=
for %%P in (python py python3) do (
    echo   Testing %%P command...
    %%P --version >nul 2>&1
    if !errorLevel! equ 0 (
        set PYTHON_CMD=%%P
        echo   ? Found: %%P
        goto :python_found
    ) else (
        echo   ? %%P not found
    )
)

:python_not_found
echo.
echo ? Python not found on your system!
echo.
echo ?? To fix this:
echo 1??  Download Python from: https://www.python.org/downloads/
echo 2??  ? IMPORTANT: Check "Add Python to PATH" during installation
echo 3??  Restart your computer after installation
echo 4??  Run this script again
echo.
set /p open_browser="Open Python download page? (y/n): "
if /i "%open_browser%"=="y" (
    start https://www.python.org/downloads/
)
pause
exit /b 1

:python_found
echo.
echo ? Python found: %PYTHON_CMD%
%PYTHON_CMD% --version
echo.

echo ?? Upgrading pip...
%PYTHON_CMD% -m pip install --upgrade pip
echo.

echo ?? Installing required packages...
echo.

REM List of required packages
set packages=matplotlib seaborn pandas pyodbc plotly kaleido

echo Installing packages: %packages%
echo.

REM Install packages one by one for better error handling
for %%p in (%packages%) do (
    echo ?? Installing %%p...
    %PYTHON_CMD% -m pip install %%p
    if !errorLevel! neq 0 (
        echo   ??  Failed with pip, trying with --user flag...
        %PYTHON_CMD% -m pip install --user %%p
        if !errorLevel! neq 0 (
            echo   ? Failed to install %%p
            set /a failed_count+=1
        ) else (
            echo   ? %%p installed with --user flag
        )
    ) else (
        echo   ? %%p installed successfully
    )
    echo.
)

echo.
echo ?? Testing installations...
echo ==============================

%PYTHON_CMD% -c "
import sys
print('?? Python version:', sys.version.split()[0])
print()

packages = {
    'matplotlib': 'Charts and plotting',
    'seaborn': 'Statistical visualization', 
    'pandas': 'Data manipulation',
    'pyodbc': 'Database connectivity',
    'plotly': 'Interactive charts',
    'kaleido': 'Static image export'
}

success_count = 0
for pkg, desc in packages.items():
    try:
        module = __import__(pkg)
        version = getattr(module, '__version__', 'unknown')
        print(f'? {pkg:<12} {version:<8} - {desc}')
        success_count += 1
    except ImportError as e:
        print(f'? {pkg:<12} MISSING   - {desc}')

print()
print(f'?? Installation Summary: {success_count}/{len(packages)} packages installed')

if success_count == len(packages):
    print('?? All packages installed successfully!')
    print('? ToDoList Python Charts are ready to use!')
else:
    print('??  Some packages failed to install. Try running as Administrator.')
"

echo.
echo ?? Copying Python files to application directory...

REM Get the directory where this script is located
set SCRIPT_DIR=%~dp0
set BUILD_DIR=%SCRIPT_DIR%ToDoList.GUI\bin\Debug\net9.0-windows

if not exist "%BUILD_DIR%" (
    echo Creating build directory: %BUILD_DIR%
    mkdir "%BUILD_DIR%" 2>nul
)

if not exist "%BUILD_DIR%\python_charts" (
    mkdir "%BUILD_DIR%\python_charts"
)

REM Copy Python files
if exist "%SCRIPT_DIR%python_charts" (
    echo Copying Python charts files...
    xcopy "%SCRIPT_DIR%python_charts\*" "%BUILD_DIR%\python_charts\" /Y /E /I >nul 2>&1
    if %errorLevel% equ 0 (
        echo ? Python files copied successfully
    ) else (
        echo ??  Warning: Could not copy some Python files
    )
) else (
    echo ??  Python charts source folder not found
)

echo.
echo ?? Setup completed!
echo.
echo ?? Next steps:
echo 1??  Start your ToDoList application
echo 2??  Right-click on any project card
echo 3??  Select "?? Python Charts"
echo 4??  Enjoy beautiful charts!
echo.
echo ?? Charts will be saved to: %BUILD_DIR%\python_charts\ToDoList_Charts
echo.
pause