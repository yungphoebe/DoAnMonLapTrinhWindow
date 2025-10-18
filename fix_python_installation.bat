@echo off
echo ?? Fixing Python execution issues...
echo.

REM Method 1: Try to find Python installation
echo ?? Searching for Python installations...
where python 2>nul
if %ERRORLEVEL% EQU 0 (
    echo ? Python found in PATH
    python --version
    goto :python_found
)

where py 2>nul
if %ERRORLEVEL% EQU 0 (
    echo ? Python Launcher found
    py --version
    goto :python_launcher
)

echo ? Python not found in PATH
echo.
echo ?? Checking common Python installation locations...

REM Check common installation paths
set PYTHON_PATHS[0]=%LOCALAPPDATA%\Programs\Python\Python312\python.exe
set PYTHON_PATHS[1]=%LOCALAPPDATA%\Programs\Python\Python311\python.exe
set PYTHON_PATHS[2]=%LOCALAPPDATA%\Programs\Python\Python310\python.exe
set PYTHON_PATHS[3]=C:\Python312\python.exe
set PYTHON_PATHS[4]=C:\Python311\python.exe
set PYTHON_PATHS[5]=C:\Python310\python.exe

for /L %%i in (0,1,5) do (
    call set "PYTHON_PATH=%%PYTHON_PATHS[%%i]%%"
    if exist "!PYTHON_PATH!" (
        echo ? Found Python at: !PYTHON_PATH!
        "!PYTHON_PATH!" --version
        set FOUND_PYTHON=!PYTHON_PATH!
        goto :found_python_manual
    )
)

echo ? No Python installation found
echo.
echo ?? Please install Python from: https://www.python.org/downloads/
echo ? IMPORTANT: Check "Add Python to PATH" during installation
echo.
pause
exit /b 1

:python_found
echo ?? Using python command
set PYTHON_CMD=python
goto :install_packages

:python_launcher
echo ?? Using py launcher
set PYTHON_CMD=py
goto :install_packages

:found_python_manual
echo ?? Using manually found Python
set PYTHON_CMD="%FOUND_PYTHON%"
goto :install_packages

:install_packages
echo.
echo ?? Installing required packages...
%PYTHON_CMD% -m pip install --upgrade pip
%PYTHON_CMD% -m pip install matplotlib seaborn pandas pyodbc plotly kaleido

if %ERRORLEVEL% NEQ 0 (
    echo ? Failed to install packages with pip
    echo ?? Trying with --user flag...
    %PYTHON_CMD% -m pip install --user matplotlib seaborn pandas pyodbc plotly kaleido
)

echo.
echo ? Python setup completed!
echo ?? Python command: %PYTHON_CMD%
echo.
pause