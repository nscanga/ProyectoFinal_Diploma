@echo off
REM ============================================
REM Diagnostico del Sistema CHM
REM ============================================

echo.
echo ==========================================
echo  DIAGNOSTICO - Sistema CHM
echo ==========================================
echo.

set ORIGEN=%~dp0source_es
set DESTINO=C:\DistribuidoraLosAmigos\Manual\source_es

echo [1] Verificando HTML Help Workshop...
if exist "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" (
    echo [OK] HTML Help Workshop encontrado
    echo Ruta: C:\Program Files (x86)\HTML Help Workshop\hhc.exe
) else (
    echo [ERROR] HTML Help Workshop NO encontrado
    echo.
    echo Debe instalarlo desde:
    echo https://www.microsoft.com/en-us/download/details.aspx?id=21138
)
echo.

echo [2] Verificando archivos fuente (tu proyecto)...
echo Carpeta: %ORIGEN%
echo.

if exist "%ORIGEN%\help_es.hhp" (
    echo [OK] help_es.hhp existe
) else (
    echo [ERROR] help_es.hhp NO existe
)

if exist "%ORIGEN%\help_es.hhc" (
    echo [OK] help_es.hhc existe
) else (
    echo [ERROR] help_es.hhc NO existe
)

if exist "%ORIGEN%\help_es.hhk" (
    echo [OK] help_es.hhk existe
) else (
    echo [ERROR] help_es.hhk NO existe
)

if exist "%ORIGEN%\css\styles.css" (
    echo [OK] css\styles.css existe
) else (
    echo [ERROR] css\styles.css NO existe
)

if exist "%ORIGEN%\html" (
    echo [OK] Carpeta html\ existe
) else (
    echo [ERROR] Carpeta html\ NO existe
)
echo.

echo [3] Verificando carpeta de compilacion...
echo Carpeta: %DESTINO%
echo.

if exist "%DESTINO%" (
    echo [OK] Carpeta destino existe
    
    if exist "%DESTINO%\help_es.hhp" (
        echo [OK] Archivos ya copiados al destino
    ) else (
        echo [AVISO] Archivos NO copiados al destino todavia
        echo Solucion: Ejecutar compilar_simple.bat (copiara automaticamente)
        echo          O ejecutar copiar_archivos.bat
    )
) else (
    echo [AVISO] Carpeta destino NO existe (se creara automaticamente)
)
echo.

echo [4] Verificando archivos HTML en origen...
if exist "%ORIGEN%\html" (
    echo Contenido de html\:
    dir /B "%ORIGEN%\html" 2>nul
    echo.
    
    REM Contar archivos HTML
    dir /B /S "%ORIGEN%\html\*.html" 2>nul | find /C ".html" > temp_count.txt
    set /p HTML_COUNT=<temp_count.txt
    del temp_count.txt
    
    echo Carpetas encontradas arriba
) else (
    echo [ERROR] Carpeta html\ NO existe
)
echo.

echo [5] Verificando archivos CHM compilados...
if exist "C:\DistribuidoraLosAmigos\Manual\*.chm" (
    echo Archivos CHM encontrados:
    dir /B "C:\DistribuidoraLosAmigos\Manual\*.chm" 2>nul
) else (
    echo [AVISO] No hay archivos CHM compilados aun
)
echo.

echo ==========================================
echo  RESUMEN Y RECOMENDACIONES
echo ==========================================
echo.

REM Verificar estado
set TODO_OK=1

if not exist "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" set TODO_OK=0
if not exist "%ORIGEN%\help_es.hhp" set TODO_OK=0
if not exist "%ORIGEN%\help_es.hhc" set TODO_OK=0
if not exist "%ORIGEN%\help_es.hhk" set TODO_OK=0

if %TODO_OK%==1 (
    echo [OK] Todo listo para compilar
    echo.
    echo SIGUIENTE PASO:
    echo   Ejecutar: compilar_simple.bat
    echo.
    echo   El script copiara automaticamente los archivos
    echo   y compilara el CHM.
) else (
    echo [ERROR] Hay problemas que resolver:
    echo.
    
    if not exist "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" (
        echo 1. Instalar HTML Help Workshop
        echo    https://www.microsoft.com/en-us/download/details.aspx?id=21138
        echo.
    )
    
    if not exist "%ORIGEN%\help_es.hhp" (
        echo 2. Faltan archivos del proyecto
        echo    Verificar que existan en: %ORIGEN%
        echo.
    )
)

echo ==========================================
echo.

pause
