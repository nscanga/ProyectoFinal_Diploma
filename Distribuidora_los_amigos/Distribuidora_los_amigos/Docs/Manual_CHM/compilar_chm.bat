@echo off
REM ============================================
REM Compilador de Archivos CHM
REM Distribuidora Los Amigos
REM ============================================

setlocal enabledelayedexpansion

echo ============================================
echo   Compilador de Archivos CHM
echo   Distribuidora Los Amigos
echo ============================================
echo:

REM Configuracion de rutas
set "HHC=C:\Program Files (x86)\HTML Help Workshop\hhc.exe"
set "BASE_DIR=C:\DistribuidoraLosAmigos\Manual"

REM Verificar que HTML Help Workshop este instalado
if not exist "%HHC%" (
    echo [ERROR] HTML Help Workshop no encontrado
    echo Ubicacion esperada: %HHC%
    echo:
    echo Descargue e instale desde:
    echo https://www.microsoft.com/en-us/download/details.aspx?id=21138
    echo:
    pause
    exit /b 1
)

echo [OK] HTML Help Workshop encontrado
echo:

REM Compilar version en espanol
echo ============================================
echo Compilando version en ESPANOL...
echo ============================================
if exist "%BASE_DIR%\source_es\help_es.hhp" (
    "%HHC%" "%BASE_DIR%\source_es\help_es.hhp"
    if exist "%BASE_DIR%\help_es.chm" (
        echo [OK] help_es.chm compilado exitosamente
    ) else (
        echo [ERROR] No se pudo generar help_es.chm
    )
) else (
    echo [ERROR] Proyecto source_es\help_es.hhp no encontrado
)
echo:

REM Compilar version en ingles
echo ============================================
echo Compilando version en INGLES...
echo ============================================
if exist "%BASE_DIR%\source_en\help_en.hhp" (
    "%HHC%" "%BASE_DIR%\source_en\help_en.hhp"
    if exist "%BASE_DIR%\help_us.chm" (
        echo [OK] help_us.chm compilado exitosamente
        REM Copiar como help_en.chm
        copy /Y "%BASE_DIR%\help_us.chm" "%BASE_DIR%\help_en.chm" >nul 2>&1
        echo [OK] Copiado a help_en.chm
    ) else (
        echo [ERROR] No se pudo generar help_us.chm
    )
) else (
    echo [AVISO] Proyecto source_en\help_en.hhp no encontrado
)
echo:

REM Compilar version en portugues
echo ============================================
echo Compilando version en PORTUGUES...
echo ============================================
if exist "%BASE_DIR%\source_pt\help_pt.hhp" (
    "%HHC%" "%BASE_DIR%\source_pt\help_pt.hhp"
    if exist "%BASE_DIR%\help_pt.chm" (
        echo [OK] help_pt.chm compilado exitosamente
    ) else (
        echo [ERROR] No se pudo generar help_pt.chm
    )
) else (
    echo [AVISO] Proyecto source_pt\help_pt.hhp no encontrado
)
echo:

REM Mostrar resumen
echo ============================================
echo RESUMEN DE COMPILACION
echo ============================================
echo:
echo Archivos generados en: %BASE_DIR%
echo:

if exist "%BASE_DIR%\*.chm" (
    dir /B "%BASE_DIR%\*.chm" 2>nul
    echo:
    echo [OK] Compilacion completada
) else (
    echo [ERROR] No se generaron archivos CHM
)

echo:
echo ============================================
echo:

pause
endlocal
