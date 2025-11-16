# =========================================================
# Script: Instalación Completa de Sandcastle
# Descripción: Instala y configura todo automáticamente
# =========================================================

Write-Host ""
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "?                                                          ?" -ForegroundColor Cyan
Write-Host "?    ???  INSTALACIÓN COMPLETA - SANDCASTLE HELP FILE    ?" -ForegroundColor Cyan
Write-Host "?         Distribuidora Los Amigos                         ?" -ForegroundColor Cyan
Write-Host "?                                                          ?" -ForegroundColor Cyan
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Stop"

# ===== PASO 1: Verificar Sandcastle =====
Write-Host "?? PASO 1/5: Verificando Sandcastle Help File Builder..." -ForegroundColor Yellow
Write-Host ""

$shfbPath = "C:\Program Files (x86)\EWSoftware\Sandcastle Help File Builder\SandcastleBuilderGUI.exe"

if (Test-Path $shfbPath) {
    Write-Host "   ? Sandcastle ya está instalado" -ForegroundColor Green
    Write-Host "   ?? Ubicación: $shfbPath" -ForegroundColor Gray
} else {
    Write-Host "   ??  Sandcastle no está instalado" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "   Opciones de instalación:" -ForegroundColor Cyan
    Write-Host "   [1] Instalar con Chocolatey (automático - recomendado)" -ForegroundColor White
    Write-Host "   [2] Descargar manualmente" -ForegroundColor White
    Write-Host "   [3] Omitir (ya lo instalé)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "   Selecciona opción (1-3): " -ForegroundColor Yellow -NoNewline
    $opcion = Read-Host
    
    switch ($opcion) {
        "1" {
            Write-Host ""
            Write-Host "   Instalando Sandcastle con Chocolatey..." -ForegroundColor Cyan
            Write-Host ""
            
            # Verificar si Chocolatey está instalado
            if (!(Get-Command choco -ErrorAction SilentlyContinue)) {
                Write-Host "   ??  Chocolatey no está instalado. Instalando..." -ForegroundColor Yellow
                Write-Host ""
                
                Set-ExecutionPolicy Bypass -Scope Process -Force
                [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
                Invoke-Expression ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
                
                Write-Host ""
                Write-Host "   ? Chocolatey instalado" -ForegroundColor Green
                Write-Host ""
            }
            
            Write-Host "   Instalando Sandcastle..." -ForegroundColor Cyan
            choco install sandcastle -y
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "   ? Sandcastle instalado correctamente" -ForegroundColor Green
            } else {
                Write-Host "   ? Error al instalar Sandcastle" -ForegroundColor Red
                exit 1
            }
        }
        "2" {
            Write-Host ""
            Write-Host "   ?? Descarga Sandcastle desde:" -ForegroundColor Cyan
            Write-Host "   https://github.com/EWSoftware/SHFB/releases" -ForegroundColor White
            Write-Host ""
            Write-Host "   Presiona ENTER cuando hayas instalado..." -ForegroundColor Yellow
            Read-Host
        }
        "3" {
            Write-Host "   ??  Omitiendo instalación..." -ForegroundColor Gray
        }
        default {
            Write-Host "   ? Opción inválida" -ForegroundColor Red
            exit 1
        }
    }
}

Write-Host ""

# ===== PASO 2: Habilitar XML Documentation =====
Write-Host "?? PASO 2/5: Habilitando XML Documentation..." -ForegroundColor Yellow
Write-Host ""

if (Test-Path "HabilitarXMLDocumentation.ps1") {
    & .\HabilitarXMLDocumentation.ps1
} else {
    Write-Host "   ??  Script HabilitarXMLDocumentation.ps1 no encontrado" -ForegroundColor Yellow
    Write-Host "   Continuando..." -ForegroundColor Gray
}

Write-Host ""

# ===== PASO 3: Compilar Solución =====
Write-Host "?? PASO 3/5: Compilando solución..." -ForegroundColor Yellow
Write-Host ""

$solutionFile = Get-ChildItem -Filter "*.sln" | Select-Object -First 1

if ($solutionFile) {
    Write-Host "   Solución: $($solutionFile.Name)" -ForegroundColor Gray
    Write-Host "   Configuración: Release" -ForegroundColor Gray
    Write-Host ""
    
    $buildArgs = @(
        $solutionFile.FullName,
        "/t:Rebuild",
        "/p:Configuration=Release",
        "/v:minimal",
        "/nologo"
    )
    
    try {
        & msbuild $buildArgs | Out-Null
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ? Compilación exitosa" -ForegroundColor Green
        } else {
            Write-Host "   ? Error en la compilación" -ForegroundColor Red
            Write-Host ""
            Write-Host "   Intenta compilar manualmente desde Visual Studio" -ForegroundColor Yellow
            exit 1
        }
    } catch {
        Write-Host "   ? Error: $_" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "   ??  No se encontró archivo .sln" -ForegroundColor Yellow
    Write-Host "   Asegúrate de compilar la solución antes de continuar" -ForegroundColor Gray
}

Write-Host ""

# ===== PASO 4: Crear Proyecto Sandcastle =====
Write-Host "???  PASO 4/5: Creando proyecto Sandcastle..." -ForegroundColor Yellow
Write-Host ""

if (Test-Path "CrearProyectoSandcastle.ps1") {
    & .\CrearProyectoSandcastle.ps1
} else {
    Write-Host "   ??  Script CrearProyectoSandcastle.ps1 no encontrado" -ForegroundColor Yellow
    Write-Host "   Continuando..." -ForegroundColor Gray
}

Write-Host ""

# ===== PASO 5: Generar Documentación =====
Write-Host "?? PASO 5/5: Generando documentación..." -ForegroundColor Yellow
Write-Host ""

$respuesta = Read-Host "¿Deseas generar la documentación ahora? (S/N)"

if ($respuesta -eq "S" -or $respuesta -eq "s") {
    Write-Host ""
    if (Test-Path "GenerarDocumentacionSandcastle.ps1") {
        & .\GenerarDocumentacionSandcastle.ps1
    } else {
        Write-Host "   ??  Script GenerarDocumentacionSandcastle.ps1 no encontrado" -ForegroundColor Yellow
    }
} else {
    Write-Host ""
    Write-Host "   ??  Generación omitida" -ForegroundColor Gray
    Write-Host ""
    Write-Host "   Para generar después, ejecuta:" -ForegroundColor Yellow
    Write-Host "   .\GenerarDocumentacionSandcastle.ps1" -ForegroundColor White
}

Write-Host ""
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host "?                                                          ?" -ForegroundColor Green
Write-Host "?              ? INSTALACIÓN COMPLETADA                  ?" -ForegroundColor Green
Write-Host "?                                                          ?" -ForegroundColor Green
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host ""

Write-Host "?? Archivos creados:" -ForegroundColor Cyan
Write-Host "   ? Distribuidora_los_amigos.shfbproj" -ForegroundColor Gray
Write-Host "   ? Carpetas Help/ y Working/" -ForegroundColor Gray
Write-Host "   ? Scripts de automatización" -ForegroundColor Gray
Write-Host ""

Write-Host "?? Comandos útiles:" -ForegroundColor Cyan
Write-Host "   Generar documentación:" -ForegroundColor Gray
Write-Host "     .\GenerarDocumentacionSandcastle.ps1" -ForegroundColor White
Write-Host ""
Write-Host "   Ver documentación:" -ForegroundColor Gray
Write-Host "     .\VerDocumentacionSandcastle.ps1" -ForegroundColor White
Write-Host ""
Write-Host "   Limpiar archivos temporales:" -ForegroundColor Gray
Write-Host "     .\LimpiarDocumentacionSandcastle.ps1" -ForegroundColor White
Write-Host ""

Write-Host "?? Documentación:" -ForegroundColor Cyan
Write-Host "   Lee: INICIO-RAPIDO-SANDCASTLE.md" -ForegroundColor White
Write-Host "   Lee: INSTALACION-SANDCASTLE.md" -ForegroundColor White
Write-Host ""

Write-Host "? ¡Todo listo para generar documentación profesional! ?" -ForegroundColor Green
Write-Host ""
