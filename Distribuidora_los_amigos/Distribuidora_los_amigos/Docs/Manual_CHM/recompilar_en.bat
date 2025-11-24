@echo off
cls
echo.
echo ============================================================
echo           RECOMPILAR MANUAL EN INGLES
echo           Distribuidora Los Amigos
echo ============================================================
echo.

REM Verificar que existe source_en
if not exist "source_en\" (
    echo ERROR: No se encontro la carpeta source_en
    echo.
    echo Asegurate de ejecutar este script desde la carpeta Manual
    pause
    exit /b 1
)

REM Verificar que existe help_en.hhp
if not exist "source_en\help_en.hhp" (
    echo ERROR: No se encontro help_en.hhp
    pause
    exit /b 1
)

echo [1/3] Verificando HTML Help Workshop...
set HHC_PATH=C:\Program Files (x86)\HTML Help Workshop\hhc.exe

if not exist "%HHC_PATH%" (
    echo ERROR: No se encontro HTML Help Workshop
    echo.
    echo Instala HTML Help Workshop desde:
    echo https://www.microsoft.com/en-us/download/details.aspx?id=21138
    pause
    exit /b 1
)

echo       Encontrado: %HHC_PATH%
echo.

echo [2/3] Compilando help_en.chm...
echo.

cd source_en
"%HHC_PATH%" "help_en.hhp"
cd ..

echo.

echo [3/3] Verificando resultado...
echo.

if exist "help_en.chm" (
    echo ============================================================
    echo              COMPILACION EXITOSA
    echo ============================================================
    echo.
    
    REM Mostrar informacion del archivo
    for %%A in ("help_en.chm") do (
        echo Archivo: help_en.chm
        echo Tamanio: %%~zA bytes
        echo Fecha:   %%~tA
    )
    
    echo.
    echo ============================================================
    
    echo.
    choice /C SN /M "Deseas abrir el manual para verificar"
    if errorlevel 2 goto :fin
    if errorlevel 1 goto :abrir
    
    :abrir
    echo.
    echo Abriendo help_en.chm...
    start help_en.chm
    goto :fin
    
) else (
    echo ============================================================
    echo              ERROR EN LA COMPILACION
    echo ============================================================
    echo.
    echo No se genero el archivo help_en.chm
    echo.
    echo Revisa los mensajes de error anteriores.
    echo.
)

:fin
echo.
pause
