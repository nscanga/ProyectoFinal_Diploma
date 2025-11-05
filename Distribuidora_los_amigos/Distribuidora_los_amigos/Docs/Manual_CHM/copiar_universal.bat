@echo off
REM ============================================
REM Copiar archivos del proyecto CHM - UNIVERSAL
REM ============================================

echo.
echo ==========================================
echo  COPIANDO ARCHIVOS DEL PROYECTO CHM
echo ==========================================
echo.

REM IMPORTANTE: Este script debe ejecutarse desde:
REM C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Docs\Manual_CHM\

REM Ruta ABSOLUTA de origen (tu proyecto)
set ORIGEN=C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Docs\Manual_CHM\source_es

REM Ruta destino (donde se compilan los CHM)
set DESTINO=C:\DistribuidoraLosAmigos\Manual\source_es

echo Origen:  %ORIGEN%
echo Destino: %DESTINO%
echo.

REM Verificar que existan archivos en el origen
if not exist "%ORIGEN%\help_es.hhp" (
    echo [ERROR] No se encuentran archivos del proyecto en:
    echo %ORIGEN%
    echo.
    echo SOLUCION:
    echo 1. Verifique que la ruta del origen sea correcta
    echo 2. Si su proyecto esta en otra ubicacion, edite este script
    echo    y cambie la linea que dice: set ORIGEN=...
    echo.
    pause
    goto :EOF
)

echo [OK] Archivos encontrados en origen
echo.

REM Crear carpeta destino si no existe
if not exist "C:\DistribuidoraLosAmigos" mkdir "C:\DistribuidoraLosAmigos"
if not exist "C:\DistribuidoraLosAmigos\Manual" mkdir "C:\DistribuidoraLosAmigos\Manual"
if not exist "%DESTINO%" mkdir "%DESTINO%"

echo Iniciando copia de archivos...
echo.

REM Copiar archivos del proyecto
echo [1/5] Copiando archivos de proyecto (.hhp, .hhc, .hhk)...
copy /Y "%ORIGEN%\help_es.hhp" "%DESTINO%\" >nul 2>&1
if exist "%DESTINO%\help_es.hhp" (
    echo   [OK] help_es.hhp
) else (
    echo   [ERROR] help_es.hhp
)

copy /Y "%ORIGEN%\help_es.hhc" "%DESTINO%\" >nul 2>&1
if exist "%DESTINO%\help_es.hhc" (
    echo   [OK] help_es.hhc
) else (
    echo   [ERROR] help_es.hhc
)

copy /Y "%ORIGEN%\help_es.hhk" "%DESTINO%\" >nul 2>&1
if exist "%DESTINO%\help_es.hhk" (
    echo   [OK] help_es.hhk
) else (
    echo   [ERROR] help_es.hhk
)

copy /Y "%ORIGEN%\template.html" "%DESTINO%\" >nul 2>&1
if exist "%DESTINO%\template.html" (
    echo   [OK] template.html
) else (
    echo   [AVISO] template.html (opcional)
)

REM Copiar carpeta CSS
echo.
echo [2/5] Copiando CSS...
if not exist "%DESTINO%\css" mkdir "%DESTINO%\css"
xcopy /Y /E /I "%ORIGEN%\css" "%DESTINO%\css" >nul 2>&1
if exist "%DESTINO%\css\styles.css" (
    echo   [OK] css\styles.css
) else (
    echo   [ERROR] css\styles.css
)

REM Copiar carpeta HTML
echo.
echo [3/5] Copiando archivos HTML...
if not exist "%DESTINO%\html" mkdir "%DESTINO%\html"
xcopy /Y /E /I "%ORIGEN%\html" "%DESTINO%\html" >nul 2>&1
if exist "%DESTINO%\html" (
    echo   [OK] Carpeta html\ copiada
    dir /B "%DESTINO%\html" 2>nul
) else (
    echo   [ERROR] Carpeta html\
)

REM Copiar carpeta images (si existe)
echo.
echo [4/5] Copiando imagenes...
if exist "%ORIGEN%\images" (
    if not exist "%DESTINO%\images" mkdir "%DESTINO%\images"
    xcopy /Y /E /I "%ORIGEN%\images" "%DESTINO%\images" >nul 2>&1
    echo   [OK] Carpeta images\ copiada
) else (
    echo   [AVISO] No hay carpeta de imagenes (opcional)
)

REM Verificar archivos copiados
echo.
echo [5/5] Verificando copia completa...
echo.

set ERROR=0

if exist "%DESTINO%\help_es.hhp" (
    echo [OK] help_es.hhp
) else (
    echo [ERROR] help_es.hhp NO copiado
    set ERROR=1
)

if exist "%DESTINO%\help_es.hhc" (
    echo [OK] help_es.hhc
) else (
    echo [ERROR] help_es.hhc NO copiado
    set ERROR=1
)

if exist "%DESTINO%\help_es.hhk" (
    echo [OK] help_es.hhk
) else (
    echo [ERROR] help_es.hhk NO copiado
    set ERROR=1
)

if exist "%DESTINO%\css\styles.css" (
    echo [OK] css\styles.css
) else (
    echo [ERROR] css\styles.css NO copiado
    set ERROR=1
)

if exist "%DESTINO%\html" (
    echo [OK] Carpeta html\
) else (
    echo [ERROR] Carpeta html\ NO copiada
    set ERROR=1
)

echo.
echo ==========================================

if %ERROR%==0 (
    echo [OK] ARCHIVOS COPIADOS EXITOSAMENTE
    echo.
    echo Archivos copiados a: %DESTINO%
    echo.
    echo SIGUIENTE PASO:
    echo Ejecutar: compilar_universal.bat
) else (
    echo [ERROR] Hubo problemas al copiar archivos
    echo.
    echo Verifique que los archivos existan en: %ORIGEN%
)

echo ==========================================
echo.

pause
