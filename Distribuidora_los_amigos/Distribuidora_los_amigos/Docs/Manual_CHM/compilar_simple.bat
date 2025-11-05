@echo off
REM ============================================
REM Script Simplificado para Compilar CHM
REM ============================================

echo.
echo ==========================================
echo  Compilador CHM - Distribuidora Los Amigos
echo ==========================================
echo.

REM Ir a la carpeta del script
cd /d "%~dp0"

REM Configuracion
set HHC="C:\Program Files (x86)\HTML Help Workshop\hhc.exe"
set BASE=C:\DistribuidoraLosAmigos\Manual
set ORIGEN=%~dp0source_es

echo Carpeta del script: %~dp0
echo Carpeta origen: %ORIGEN%
echo Carpeta destino: %BASE%
echo.

REM Verificar HTML Help Workshop
if not exist %HHC% (
    echo ERROR: HTML Help Workshop no encontrado en:
    echo %HHC%
    echo.
    echo Descargue desde:
    echo https://www.microsoft.com/en-us/download/details.aspx?id=21138
    echo.
    pause
    goto :EOF
)

echo [OK] HTML Help Workshop encontrado
echo.

REM Crear carpeta destino si no existe
if not exist "%BASE%" mkdir "%BASE%"
if not exist "%BASE%\source_es" mkdir "%BASE%\source_es"

REM Verificar si existen archivos del proyecto en el ORIGEN
if not exist "%ORIGEN%\help_es.hhp" (
    echo [ERROR] No se encuentran archivos del proyecto en:
    echo %ORIGEN%
    echo.
    echo Verifique que los archivos .hhp, .hhc, .hhk existan en esa carpeta.
    echo.
    pause
    goto :EOF
)

REM Verificar si existen archivos del proyecto en el destino
if not exist "%BASE%\source_es\help_es.hhp" (
    echo ==========================================
    echo COPIANDO ARCHIVOS DEL PROYECTO...
    echo ==========================================
    echo.
    echo Origen:  %ORIGEN%
    echo Destino: %BASE%\source_es
    echo.
    
    REM Copiar archivos principales
    echo Copiando archivos del proyecto...
    copy /Y "%ORIGEN%\help_es.hhp" "%BASE%\source_es\" >nul 2>&1
    copy /Y "%ORIGEN%\help_es.hhc" "%BASE%\source_es\" >nul 2>&1
    copy /Y "%ORIGEN%\help_es.hhk" "%BASE%\source_es\" >nul 2>&1
    copy /Y "%ORIGEN%\template.html" "%BASE%\source_es\" >nul 2>&1
    
    REM Copiar carpetas
    echo Copiando carpeta CSS...
    xcopy /Y /E /I "%ORIGEN%\css" "%BASE%\source_es\css" >nul 2>&1
    
    echo Copiando carpeta HTML...
    xcopy /Y /E /I "%ORIGEN%\html" "%BASE%\source_es\html" >nul 2>&1
    
    REM Copiar images si existe
    if exist "%ORIGEN%\images" (
        echo Copiando carpeta images...
        xcopy /Y /E /I "%ORIGEN%\images" "%BASE%\source_es\images" >nul 2>&1
    )
    
    if exist "%BASE%\source_es\help_es.hhp" (
        echo.
        echo [OK] Archivos copiados exitosamente
    ) else (
        echo.
        echo [ERROR] No se pudieron copiar los archivos
        echo.
        echo Verifique:
        echo 1. Que los archivos existan en: %ORIGEN%
        echo 2. Que tenga permisos de escritura en: %BASE%
        echo.
        pause
        goto :EOF
    )
    echo.
)

REM Compilar version espanol
echo ==========================================
echo Compilando: help_es.chm
echo ==========================================
if exist "%BASE%\source_es\help_es.hhp" (
    %HHC% "%BASE%\source_es\help_es.hhp" 2>nul
    if exist "%BASE%\help_es.chm" (
        echo [OK] help_es.chm compilado
    ) else (
        echo [ERROR] No se genero help_es.chm
        echo.
        echo Posibles causas:
        echo - Faltan archivos HTML en: %BASE%\source_es\html\
        echo - Error en el proyecto .hhp
        echo.
        echo Verifique el contenido de: %BASE%\source_es\
    )
) else (
    echo [ERROR] No existe: %BASE%\source_es\help_es.hhp
)
echo.

REM Compilar version ingles (si existe)
echo ==========================================
echo Compilando: help_us.chm
echo ==========================================
if exist "%BASE%\source_en\help_en.hhp" (
    %HHC% "%BASE%\source_en\help_en.hhp" 2>nul
    if exist "%BASE%\help_us.chm" (
        echo [OK] help_us.chm compilado
        copy /Y "%BASE%\help_us.chm" "%BASE%\help_en.chm" >nul 2>&1
        echo [OK] Copiado a help_en.chm
    ) else (
        echo [ERROR] No se genero help_us.chm
    )
) else (
    echo [AVISO] No existe proyecto en ingles (opcional)
)
echo.

REM Resumen
echo ==========================================
echo RESUMEN
echo ==========================================
if exist "%BASE%\*.chm" (
    echo Archivos CHM generados:
    dir /B "%BASE%\*.chm" 2>nul
    echo.
    echo [OK] Proceso completado
    echo.
    echo Ubicacion: %BASE%
) else (
    echo [ERROR] No se generaron archivos CHM
    echo.
    echo Ejecute: diagnostico.bat para mas informacion
)
echo.
echo ==========================================
echo.

pause
