# =========================================================
# Script: Limpiar Documentación Sandcastle
# Descripción: Elimina archivos generados y temporales
# =========================================================

param(
    [switch]$Full
)

Write-Host "?? Limpiando archivos de documentación..." -ForegroundColor Cyan
Write-Host ""

$eliminados = 0
$errores = 0

# Limpiar carpeta Help (documentación generada)
if (Test-Path "Help") {
    Write-Host "?? Limpiando carpeta 'Help'..." -ForegroundColor Yellow
    try {
        $archivos = (Get-ChildItem "Help" -Recurse -File).Count
        Remove-Item "Help\*" -Recurse -Force
        Write-Host "   ? $archivos archivos eliminados" -ForegroundColor Green
        $eliminados += $archivos
    } catch {
        Write-Host "   ? ERROR: $_" -ForegroundColor Red
        $errores++
    }
}

# Limpiar carpeta Working (archivos temporales)
if (Test-Path "Working") {
    Write-Host "?? Limpiando carpeta 'Working'..." -ForegroundColor Yellow
    try {
        $archivos = (Get-ChildItem "Working" -Recurse -File).Count
        Remove-Item "Working\*" -Recurse -Force
        Write-Host "   ? $archivos archivos temporales eliminados" -ForegroundColor Green
        $eliminados += $archivos
    } catch {
        Write-Host "   ? ERROR: $_" -ForegroundColor Red
        $errores++
    }
}

# Limpiar logs
$logFiles = Get-ChildItem -Filter "*.log" -ErrorAction SilentlyContinue
if ($logFiles) {
    Write-Host "?? Limpiando archivos de log..." -ForegroundColor Yellow
    foreach ($log in $logFiles) {
        try {
            Remove-Item $log.FullName -Force
            Write-Host "   ? $($log.Name) eliminado" -ForegroundColor Green
            $eliminados++
        } catch {
            Write-Host "   ? ERROR eliminando $($log.Name): $_" -ForegroundColor Red
            $errores++
        }
    }
}

# Si se especifica -Full, también limpiar archivos XML
if ($Full) {
    Write-Host ""
    Write-Host "?? Limpieza completa: eliminando archivos XML de documentación..." -ForegroundColor Yellow
    
    $xmlPatterns = @(
        "BLL\bin\*\*.xml",
        "DAL\bin\*\*.xml",
        "DOMAIN\bin\*\*.xml",
        "Service\bin\*\*.xml",
        "Distribuidora_los_amigos\bin\*\*.xml"
    )
    
    foreach ($pattern in $xmlPatterns) {
        $xmlFiles = Get-ChildItem $pattern -Recurse -ErrorAction SilentlyContinue
        foreach ($xml in $xmlFiles) {
            try {
                Remove-Item $xml.FullName -Force
                Write-Host "   ? $($xml.FullName) eliminado" -ForegroundColor Green
                $eliminados++
            } catch {
                Write-Host "   ? ERROR eliminando $($xml.Name): $_" -ForegroundColor Red
                $errores++
            }
        }
    }
}

Write-Host ""
Write-Host "================================" -ForegroundColor Cyan
Write-Host "?? RESUMEN DE LIMPIEZA:" -ForegroundColor Cyan
Write-Host "   ? Archivos eliminados: $eliminados" -ForegroundColor Green
Write-Host "   ? Errores: $errores" -ForegroundColor $(if($errores -gt 0){"Red"}else{"Green"})
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

if ($Full) {
    Write-Host "??  Se realizó limpieza completa (incluye archivos XML)" -ForegroundColor Yellow
    Write-Host "   Para regenerar documentación, primero recompila la solución:" -ForegroundColor Gray
    Write-Host "   msbuild Distribuidora_los_amigos.sln /t:Rebuild /p:Configuration=Release" -ForegroundColor White
    Write-Host ""
} else {
    Write-Host "?? Para limpieza completa (incluir archivos XML):" -ForegroundColor Yellow
    Write-Host "   .\LimpiarDocumentacionSandcastle.ps1 -Full" -ForegroundColor White
    Write-Host ""
}

Write-Host "? Limpieza completada" -ForegroundColor Green
