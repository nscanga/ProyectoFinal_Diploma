@echo off
REM ============================================
REM Compilador CHM - UNIVERSAL
REM ============================================

echo.
echo ==========================================
echo  Compilador CHM - Distribuidora Los Amigos
echo ==========================================
echo.

REM Configuracion
set HHC="C:\Program Files (x86)\HTML Help Workshop\hhc.exe"
set ORIGEN=C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Docs\Manual_CHM\source_es
set DESTINO=C:\DistribuidoraLosAmigos\Manual\source_es
set BASE=C:\DistribuidoraLosAmigos\Manual

echo Origen:  %ORIGEN%
echo Destino: %DESTINO%
echo.

REM Verificar HTML Help Workshop
if not exist %HHC% (
    echo [ERROR] HTML Help Workshop no encontrado en:
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

REM Verificar que existan archivos en el origen
if not exist "%ORIGEN%\help_es.hhp" (
    echo [ERROR] No se encuentran archivos del proyecto en:
    echo %ORIGEN%
    echo.
    echo SOLUCION:
    echo Si su proyecto esta en otra ubicacion, edite este script
    echo y cambie la linea que dice: set ORIGEN=...
    echo.
    pause
    goto :EOF
)

REM Crear carpetas destino
if not exist "C:\DistribuidoraLosAmigos" mkdir "C:\DistribuidoraLosAmigos"
if not exist "%BASE%" mkdir "%BASE%"
if not exist "%DESTINO%" mkdir "%DESTINO%"

REM Verificar si existen archivos del proyecto en el destino
if not exist "%DESTINO%\help_es.hhp" (
    echo ==========================================
    echo COPIANDO ARCHIVOS DEL PROYECTO...
    echo ==========================================
    echo.
    
    REM Copiar archivos principales
    copy /Y "%ORIGEN%\help_es.hhp" "%DESTINO%\" >nul 2>&1
    copy /Y "%ORIGEN%\help_es.hhc" "%DESTINO%\" >nul 2>&1
    copy /Y "%ORIGEN%\help_es.hhk" "%DESTINO%\" >nul 2>&1
    copy /Y "%ORIGEN%\template.html" "%DESTINO%\" >nul 2>&1
    
    REM Copiar carpetas
    xcopy /Y /E /I "%ORIGEN%\css" "%DESTINO%\css" >nul 2>&1
    xcopy /Y /E /I "%ORIGEN%\html" "%DESTINO%\html" >nul 2>&1
    
    if exist "%DESTINO%\help_es.hhp" (
        echo [OK] Archivos copiados exitosamente
    ) else (
        echo [ERROR] No se pudieron copiar los archivos
        pause
        goto :EOF
    )
    echo.
)

REM Compilar version espanol
echo ==========================================
echo Compilando: help_es.chm
echo ==========================================
if exist "%DESTINO%\help_es.hhp" (
    %HHC% "%DESTINO%\help_es.hhp" 2>nul
    
    if exist "%BASE%\help_es.chm" (
        echo [OK] help_es.chm compilado exitosamente
        echo.
        echo Archivo generado en:
        echo %BASE%\help_es.chm
    ) else (
        echo [ERROR] No se genero help_es.chm
        echo.
        echo Posibles causas:
        echo - Faltan archivos HTML en: %DESTINO%\html\
        echo - Error en el proyecto .hhp
    )
) else (
    echo [ERROR] No existe: %DESTINO%\help_es.hhp
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
    echo.
    echo SIGUIENTE PASO:
    echo Copie el archivo help_es.chm a la carpeta de su aplicacion
    echo o pruebe el archivo abriendolo directamente.
) else (
    echo [ERROR] No se generaron archivos CHM
)
echo.
echo ==========================================
echo.

pause
