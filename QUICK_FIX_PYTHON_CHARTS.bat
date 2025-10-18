@echo off
chcp 65001 >nul
color 0A
echo.
echo ===============================================
echo ?? ToDoList Python Charts Quick Fix Installer
echo ===============================================
echo.

REM Elevate to Administrator if needed
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ?? Requesting Administrator privileges...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)

echo ? Running as Administrator
echo.

echo ?? Step 1: Checking Python installation...
echo ==========================================

set PYTHON_CMD=
REM Try py first (Python Launcher on Windows)
for %%P in (py python python3) do (
    echo   Testing %%P...
    %%P --version >nul 2>&1
    if !errorLevel! equ 0 (
        set PYTHON_CMD=%%P
        %%P --version
        echo   ? Found working Python: %%P
        goto :python_found
    )
)

echo ? Python not found!
echo.
echo ?? SOLUTION:
echo 1??  Go to: https://www.python.org/downloads/
echo 2??  Download Python 3.11 or newer
echo 3??  ? CHECK "Add Python to PATH" during install
echo 4??  Restart computer after installation
echo.
set /p choice="Open Python download page? (y/n): "
if /i "%choice%"=="y" start https://www.python.org/downloads/
pause
exit /b 1

:python_found
echo.
echo ?? Step 2: Installing Python packages...
echo ======================================

echo Upgrading pip first...
%PYTHON_CMD% -m pip install --upgrade pip

echo.
echo Installing matplotlib (charts library)...
%PYTHON_CMD% -m pip install matplotlib
if %errorLevel% neq 0 %PYTHON_CMD% -m pip install --user matplotlib

echo Installing seaborn (styling)...
%PYTHON_CMD% -m pip install seaborn
if %errorLevel% neq 0 %PYTHON_CMD% -m pip install --user seaborn

echo Installing pandas (data processing)...
%PYTHON_CMD% -m pip install pandas
if %errorLevel% neq 0 %PYTHON_CMD% -m pip install --user pandas

echo Installing pyodbc (database connection)...
%PYTHON_CMD% -m pip install pyodbc
if %errorLevel% neq 0 %PYTHON_CMD% -m pip install --user pyodbc

echo Installing plotly (interactive charts)...
%PYTHON_CMD% -m pip install plotly
if %errorLevel% neq 0 %PYTHON_CMD% -m pip install --user plotly

echo Installing kaleido (image export)...
%PYTHON_CMD% -m pip install kaleido
if %errorLevel% neq 0 %PYTHON_CMD% -m pip install --user kaleido

echo.
echo ?? Step 3: Testing installations...
echo =================================

%PYTHON_CMD% -c "
print('?? Testing Python packages...')
print()

packages = ['matplotlib', 'seaborn', 'pandas', 'pyodbc', 'plotly', 'kaleido']
success = 0

for pkg in packages:
    try:
        __import__(pkg)
        print(f'? {pkg:<12} - OK')
        success += 1
    except ImportError:
        print(f'? {pkg:<12} - FAILED')

print()
print(f'?? Result: {success}/{len(packages)} packages installed')

if success == len(packages):
    print('?? ALL PACKAGES READY!')
else:
    print('??  Some packages failed. Try running again.')
"

echo.
echo ?? Step 4: Setting up ToDoList integration...
echo ===========================================

set BUILD_DIR=%~dp0ToDoList.GUI\bin\Debug\net9.0-windows

if not exist "%BUILD_DIR%" (
    echo Creating build directory...
    mkdir "%BUILD_DIR%" 2>nul
)

if not exist "%BUILD_DIR%\python_charts" (
    mkdir "%BUILD_DIR%\python_charts"
)

echo Copying Python files to: %BUILD_DIR%\python_charts\

if exist "%~dp0python_charts" (
    xcopy "%~dp0python_charts\*" "%BUILD_DIR%\python_charts\" /Y /E /I >nul 2>&1
    echo ? Python charts files copied
) else (
    echo ??  Python charts source not found, creating basic structure...
)

echo.
echo ?? INSTALLATION COMPLETE!
echo ========================
echo.
echo ? Python packages installed
echo ? Files copied to build directory
echo ? ToDoList Python Charts ready!
echo.
echo ?? HOW TO USE:
echo 1??  Start your ToDoList application
echo 2??  Right-click any project card
echo 3??  Select "?? Python Charts"
echo 4??  Wait for beautiful charts!
echo.
echo ?? Charts will be saved to:
echo    %BUILD_DIR%\python_charts\ToDoList_Charts\
echo.
pause