@echo off
REM ============================================
REM Compilador CHM - SOLO COMPILAR
REM ============================================

echo.
echo ==========================================
echo  Compilador CHM - Distribuidora Los Amigos
echo ==========================================
echo.

REM Configuracion
set HHC="C:\Program Files (x86)\HTML Help Workshop\hhc.exe"
set BASE=C:\DistribuidoraLosAmigos\Manual
set SOURCE=%BASE%\source_es

echo Carpeta de compilacion: %BASE%
echo Archivos fuente: %SOURCE%
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

REM Verificar que exista el proyecto
if not exist "%SOURCE%\help_es.hhp" (
    echo [ERROR] No se encuentra el proyecto en:
    echo %SOURCE%\help_es.hhp
    echo.
    echo Verifique que los archivos existan en esa ubicacion.
    echo.
    pause
    goto :EOF
)

echo [OK] Proyecto encontrado
echo.

REM Verificar archivos HTML
if not exist "%SOURCE%\html" (
    echo [ERROR] No existe la carpeta html\ en:
    echo %SOURCE%\html
    echo.
    pause
    goto :EOF
)

echo [OK] Carpeta html\ encontrada
echo.

REM Compilar
echo ==========================================
echo Compilando help_es.chm...
echo ==========================================
echo.

%HHC% "%SOURCE%\help_es.hhp"

echo.
echo ==========================================
echo VERIFICANDO RESULTADO
echo ==========================================
echo.

if exist "%BASE%\help_es.chm" (
    echo [OK] help_es.chm compilado exitosamente
    echo.
    echo Archivo generado:
    echo %BASE%\help_es.chm
    echo.
    
    REM Mostrar tamaño del archivo
    for %%F in ("%BASE%\help_es.chm") do (
        echo Tamano: %%~zF bytes
    )
    echo.
    
    echo SIGUIENTE PASO:
    echo 1. Abrir el archivo para probar: %BASE%\help_es.chm
    echo 2. O copiar a la carpeta de su aplicacion
    echo.
) else (
    echo [ERROR] No se genero el archivo help_es.chm
    echo.
    echo Posibles causas:
    echo - Faltan archivos HTML en: %SOURCE%\html\
    echo - Error en el archivo .hhp
    echo - Rutas incorrectas en el proyecto
    echo.
    echo Revise el contenido de: %SOURCE%
)

echo ==========================================
echo.

pause
