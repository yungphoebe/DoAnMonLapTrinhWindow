@echo off
echo ?? Testing Python Charts Setup
echo ==============================
echo.

echo 1?? Testing Python command...
py --version
if %errorLevel% neq 0 (
    echo ? Python not found
    pause
    exit /b 1
)
echo ? Python found
echo.

echo 2?? Testing required packages...
py -c "import matplotlib; import seaborn; import pandas; import pyodbc; import plotly; print('? All packages OK')"
if %errorLevel% neq 0 (
    echo ? Some packages missing
    pause
    exit /b 1
)
echo.

echo 3?? Checking Python script location...
set SCRIPT_PATH=ToDoList.GUI\bin\Debug\net9.0-windows\python_charts\todolist_charts.py
if exist "%SCRIPT_PATH%" (
    echo ? Script found at: %SCRIPT_PATH%
) else (
    echo ??  Script not found at: %SCRIPT_PATH%
    echo    Run the app once to copy files
)
echo.

echo ?? Setup looks good!
echo.
echo ?? Next steps:
echo 1?? Start ToDoList application
echo 2?? Open Advanced Reports (?? Th?ng kê nâng cao)
echo 3?? Click "?? Generate Python Charts"
echo 4?? Wait for beautiful charts!
echo.
pause