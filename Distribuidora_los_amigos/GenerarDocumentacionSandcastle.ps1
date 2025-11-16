# =========================================================
# Script: Generar Documentación con Sandcastle
# Descripción: Compila y genera la documentación completa
# =========================================================

param(
    [string]$Configuration = "Release"
)

Write-Host "?? Generando Documentación con Sandcastle..." -ForegroundColor Cyan
Write-Host ""

# Verificar que exista el proyecto .shfbproj
$shfbproj = "Distribuidora_los_amigos.shfbproj"
if (-not (Test-Path $shfbproj)) {
    Write-Host "? ERROR: No se encontró el archivo $shfbproj" -ForegroundColor Red
    Write-Host ""
    Write-Host "Ejecuta primero:" -ForegroundColor Yellow
    Write-Host "   .\CrearProyectoSandcastle.ps1" -ForegroundColor Gray
    exit 1
}

Write-Host "? Proyecto Sandcastle encontrado: $shfbproj" -ForegroundColor Green
Write-Host ""

# Paso 1: Compilar la solución
Write-Host "?? Paso 1/3: Compilando la solución..." -ForegroundColor Yellow
Write-Host ""

$solutionFile = Get-ChildItem -Filter "*.sln" | Select-Object -First 1

if ($solutionFile) {
    Write-Host "   Solución: $($solutionFile.Name)" -ForegroundColor Gray
    Write-Host "   Configuración: $Configuration" -ForegroundColor Gray
    Write-Host ""
    
    $buildArgs = @(
        $solutionFile.FullName,
        "/t:Rebuild",
        "/p:Configuration=$Configuration",
        "/v:minimal",
        "/nologo"
    )
    
    try {
        $buildOutput = & msbuild $buildArgs 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ? Compilación exitosa" -ForegroundColor Green
        } else {
            Write-Host "   ? ERROR en la compilación" -ForegroundColor Red
            Write-Host $buildOutput
            exit 1
        }
    } catch {
        Write-Host "   ? ERROR: $_" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "   ??  No se encontró archivo .sln, se asume que los proyectos están compilados" -ForegroundColor Yellow
}

Write-Host ""

# Paso 2: Verificar archivos XML
Write-Host "?? Paso 2/3: Verificando archivos XML de documentación..." -ForegroundColor Yellow
Write-Host ""

$xmlFiles = @(
    "BLL\bin\$Configuration\BLL.xml",
    "DAL\bin\$Configuration\DAL.xml",
    "DOMAIN\bin\$Configuration\DOMAIN.xml",
    "Service\bin\$Configuration\Service.xml",
    "Distribuidora_los_amigos\bin\$Configuration\Distribuidora_los_amigos.xml"
)

$missingXml = @()
foreach ($xmlFile in $xmlFiles) {
    if (Test-Path $xmlFile) {
        Write-Host "   ? $xmlFile" -ForegroundColor Green
    } else {
        Write-Host "   ? NO ENCONTRADO: $xmlFile" -ForegroundColor Red
        $missingXml += $xmlFile
    }
}

Write-Host ""

if ($missingXml.Count -gt 0) {
    Write-Host "??  ADVERTENCIA: Faltan archivos XML" -ForegroundColor Yellow
    Write-Host "   Ejecuta: .\HabilitarXMLDocumentation.ps1" -ForegroundColor Gray
    Write-Host "   Luego recompila la solución" -ForegroundColor Gray
    Write-Host ""
    Write-Host "¿Deseas continuar de todos modos? (S/N)" -ForegroundColor Yellow
    $respuesta = Read-Host
    if ($respuesta -ne "S" -and $respuesta -ne "s") {
        exit 0
    }
}

# Paso 3: Generar documentación con Sandcastle
Write-Host "???  Paso 3/3: Generando documentación..." -ForegroundColor Yellow
Write-Host ""

Write-Host "   Esto puede tardar varios minutos..." -ForegroundColor Gray
Write-Host ""

try {
    $shfbArgs = @(
        $shfbproj,
        "/p:Configuration=$Configuration",
        "/v:normal",
        "/nologo"
    )
    
    $startTime = Get-Date
    $buildOutput = & msbuild $shfbArgs 2>&1
    $endTime = Get-Date
    $duration = $endTime - $startTime
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "================================" -ForegroundColor Green
        Write-Host "? DOCUMENTACIÓN GENERADA EXITOSAMENTE" -ForegroundColor Green
        Write-Host "================================" -ForegroundColor Green
        Write-Host ""
        Write-Host "??  Tiempo de generación: $($duration.ToString('mm\:ss'))" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "?? Archivos generados en:" -ForegroundColor Cyan
        Write-Host "   .\Help\" -ForegroundColor White
        Write-Host ""
        
        # Listar archivos principales
        if (Test-Path "Help") {
            $chm = Get-ChildItem "Help\*.chm" -ErrorAction SilentlyContinue
            $html = Get-ChildItem "Help\index.html" -ErrorAction SilentlyContinue
            
            if ($chm) {
                Write-Host "   ?? CHM: $($chm.Name) ($([math]::Round($chm.Length/1MB, 2)) MB)" -ForegroundColor Green
            }
            if ($html) {
                Write-Host "   ?? HTML: $($html.Name)" -ForegroundColor Green
            }
            
            $totalFiles = (Get-ChildItem "Help" -Recurse -File).Count
            Write-Host "   ?? Total de archivos: $totalFiles" -ForegroundColor Gray
        }
        
        Write-Host ""
        Write-Host "?? Para ver la documentación:" -ForegroundColor Yellow
        Write-Host "   .\VerDocumentacionSandcastle.ps1" -ForegroundColor White
        Write-Host ""
        
    } else {
        Write-Host ""
        Write-Host "? ERROR al generar documentación" -ForegroundColor Red
        Write-Host ""
        Write-Host "Revisa el log para más detalles:" -ForegroundColor Yellow
        
        $logFile = Get-ChildItem "Working\*.log" -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
        if ($logFile) {
            Write-Host "   $($logFile.FullName)" -ForegroundColor Gray
        }
        
        Write-Host ""
        Write-Host "Errores comunes:" -ForegroundColor Yellow
        Write-Host "   1. Archivos XML no generados ? Ejecuta .\HabilitarXMLDocumentation.ps1" -ForegroundColor Gray
        Write-Host "   2. DLLs no encontradas ? Recompila la solución" -ForegroundColor Gray
        Write-Host "   3. Referencias faltantes ? Verifica dependencias en bin\$Configuration" -ForegroundColor Gray
        Write-Host ""
        
        exit 1
    }
    
} catch {
    Write-Host "? ERROR CRÍTICO: $_" -ForegroundColor Red
    exit 1
}
