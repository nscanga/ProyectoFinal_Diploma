@echo off
REM ============================================
REM Ejecutar generador de proyecto CHM
REM ============================================

echo.
echo ==========================================
echo  GENERAR PROYECTO CHM
echo ==========================================
echo.

REM Ir a la carpeta del script
cd /d "%~dp0"

echo Ejecutando desde: %~dp0
echo.

REM Verificar que existe el script PowerShell
if not exist "%~dp0generar_proyecto_chm.ps1" (
    echo [ERROR] No se encuentra el script:
    echo %~dp0generar_proyecto_chm.ps1
    echo.
    pause
    exit /b 1
)

REM Ejecutar script PowerShell
PowerShell.exe -ExecutionPolicy Bypass -File "%~dp0generar_proyecto_chm.ps1"

echo.
pause
