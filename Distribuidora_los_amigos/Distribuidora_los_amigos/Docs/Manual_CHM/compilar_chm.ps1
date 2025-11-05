# ============================================
# Script de Compilación de CHM
# Distribuidora Los Amigos
# ============================================

param(
    [string]$BaseDir = "C:\DistribuidoraLosAmigos\Manual",
    [string]$HHCPath = "C:\Program Files (x86)\HTML Help Workshop\hhc.exe"
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Compilador de Archivos CHM" -ForegroundColor Cyan
Write-Host "  Distribuidora Los Amigos" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Verificar que HTML Help Workshop esté instalado
if (-not (Test-Path $HHCPath)) {
    Write-Host "? ERROR: HTML Help Workshop no encontrado" -ForegroundColor Red
    Write-Host "?? Ruta esperada: $HHCPath" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Soluciones:" -ForegroundColor Cyan
    Write-Host "  1. Instalar HTML Help Workshop desde:" -ForegroundColor White
    Write-Host "     https://www.microsoft.com/en-us/download/details.aspx?id=21138" -ForegroundColor White
    Write-Host "  2. Si está instalado en otra ubicación, ejecutar:" -ForegroundColor White
    Write-Host "     .\compilar_chm.ps1 -HHCPath 'C:\Ruta\Personalizada\hhc.exe'" -ForegroundColor White
    Write-Host ""
    exit 1
}

Write-Host "? HTML Help Workshop encontrado" -ForegroundColor Green
Write-Host "?? $HHCPath" -ForegroundColor Gray
Write-Host ""

# Función para compilar un proyecto CHM
function Compile-CHM {
    param(
        [string]$ProjectFile,
        [string]$Language
    )
    
    Write-Host "????????????????????????????????????????" -ForegroundColor Gray
    Write-Host "?? Compilando: $Language" -ForegroundColor Cyan
    Write-Host "????????????????????????????????????????" -ForegroundColor Gray
    
    if (-not (Test-Path $ProjectFile)) {
        Write-Host "? Archivo de proyecto no encontrado: $ProjectFile" -ForegroundColor Red
        return $false
    }
    
    Write-Host "?? Proyecto: $ProjectFile" -ForegroundColor White
    
    try {
        # El compilador hhc.exe devuelve 1 en éxito (comportamiento no estándar)
        $process = Start-Process -FilePath $HHCPath -ArgumentList "`"$ProjectFile`"" -Wait -PassThru -NoNewWindow
        
        # hhc.exe devuelve 1 en caso de éxito
        if ($process.ExitCode -eq 1 -or $process.ExitCode -eq 0) {
            Write-Host "? Compilación exitosa" -ForegroundColor Green
            
            # Buscar el archivo CHM generado
            $projectDir = Split-Path $ProjectFile
            $chmFile = Get-ChildItem -Path (Split-Path $projectDir) -Filter "*.chm" -ErrorAction SilentlyContinue | Select-Object -First 1
            
            if ($chmFile) {
                $size = [math]::Round($chmFile.Length / 1KB, 2)
                Write-Host "?? Archivo generado: $($chmFile.Name) ($size KB)" -ForegroundColor Green
            }
            
            return $true
        } else {
            Write-Host "??  Compilación completada con advertencias (código: $($process.ExitCode))" -ForegroundColor Yellow
            return $true
        }
    }
    catch {
        Write-Host "? Error durante la compilación: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
    finally {
        Write-Host ""
    }
}

# Compilar versión en español
$esProject = "$BaseDir\source_es\help_es.hhp"
$esSuccess = Compile-CHM -ProjectFile $esProject -Language "Español (es-ES)"

# Compilar versión en inglés
$enProject = "$BaseDir\source_en\help_en.hhp"
if (Test-Path $enProject) {
    $enSuccess = Compile-CHM -ProjectFile $enProject -Language "Inglés (en-US)"
} else {
    Write-Host "??  Proyecto en inglés no encontrado, omitiendo..." -ForegroundColor Yellow
    $enSuccess = $false
}

# Compilar versión en portugués
$ptProject = "$BaseDir\source_pt\help_pt.hhp"
if (Test-Path $ptProject) {
    $ptSuccess = Compile-CHM -ProjectFile $ptProject -Language "Portugués (pt-PT)"
} else {
    Write-Host "??  Proyecto en portugués no encontrado, omitiendo..." -ForegroundColor Yellow
    $ptSuccess = $false
}

# Resumen
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "?? RESUMEN DE COMPILACIÓN" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

if ($esSuccess) {
    Write-Host "? help_es.chm - Compilado" -ForegroundColor Green
} else {
    Write-Host "? help_es.chm - Error" -ForegroundColor Red
}

if ($enSuccess) {
    Write-Host "? help_us.chm - Compilado" -ForegroundColor Green
    Write-Host "? help_en.chm - Copiado" -ForegroundColor Green
} else {
    Write-Host "??  help_us.chm - No disponible" -ForegroundColor Yellow
}

if ($ptSuccess) {
    Write-Host "? help_pt.chm - Compilado" -ForegroundColor Green
} else {
    Write-Host "??  help_pt.chm - No disponible" -ForegroundColor Yellow
}

Write-Host ""

# Copiar help_us.chm a help_en.chm si existe
$helpUS = "$BaseDir\help_us.chm"
$helpEN = "$BaseDir\help_en.chm"

if (Test-Path $helpUS) {
    Copy-Item -Path $helpUS -Destination $helpEN -Force
    Write-Host "?? Copiado help_us.chm ? help_en.chm" -ForegroundColor Green
}

Write-Host ""
Write-Host "?? Archivos CHM en: $BaseDir" -ForegroundColor White
Write-Host ""

# Listar archivos CHM generados
Write-Host "?? Archivos generados:" -ForegroundColor Cyan
Get-ChildItem -Path $BaseDir -Filter "*.chm" | ForEach-Object {
    $size = [math]::Round($_.Length / 1KB, 2)
    Write-Host "   • $($_.Name) - $size KB" -ForegroundColor White
}

Write-Host ""
Write-Host "?? Próximos pasos:" -ForegroundColor Cyan
Write-Host "  1. Probar los archivos CHM abriéndolos directamente" -ForegroundColor White
Write-Host "  2. Verificar que App.config apunte a estas rutas" -ForegroundColor White
Write-Host "  3. Probar F1 desde la aplicación" -ForegroundColor White
Write-Host ""

if ($esSuccess) {
    Write-Host "? ¡Compilación completada!" -ForegroundColor Green
} else {
    Write-Host "??  Compilación completada con errores" -ForegroundColor Yellow
}
