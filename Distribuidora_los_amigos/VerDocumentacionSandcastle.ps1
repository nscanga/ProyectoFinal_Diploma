# =========================================================
# Script: Ver Documentación Sandcastle Generada
# Descripción: Abre la documentación en CHM o HTML
# =========================================================

param(
    [ValidateSet("CHM", "HTML", "Auto")]
    [string]$Formato = "Auto"
)

Write-Host "?? Abriendo documentación..." -ForegroundColor Cyan
Write-Host ""

# Verificar que exista la carpeta Help
if (-not (Test-Path "Help")) {
    Write-Host "? ERROR: No se encontró la carpeta 'Help'" -ForegroundColor Red
    Write-Host ""
    Write-Host "Primero genera la documentación:" -ForegroundColor Yellow
    Write-Host "   .\GenerarDocumentacionSandcastle.ps1" -ForegroundColor Gray
    exit 1
}

# Buscar archivos de documentación
$chmFile = Get-ChildItem "Help\*.chm" -ErrorAction SilentlyContinue | Select-Object -First 1
$htmlFile = Get-ChildItem "Help\index.html" -ErrorAction SilentlyContinue

# Determinar qué abrir
if ($Formato -eq "Auto") {
    if ($chmFile) {
        $Formato = "CHM"
    } elseif ($htmlFile) {
        $Formato = "HTML"
    } else {
        Write-Host "? ERROR: No se encontraron archivos de documentación" -ForegroundColor Red
        Write-Host ""
        Write-Host "Archivos esperados:" -ForegroundColor Yellow
        Write-Host "   - Help\Distribuidora_los_amigos.chm (Windows Help)" -ForegroundColor Gray
        Write-Host "   - Help\index.html (Website)" -ForegroundColor Gray
        Write-Host ""
        Write-Host "Genera la documentación primero:" -ForegroundColor Yellow
        Write-Host "   .\GenerarDocumentacionSandcastle.ps1" -ForegroundColor Gray
        exit 1
    }
}

# Abrir según el formato
switch ($Formato) {
    "CHM" {
        if ($chmFile) {
            Write-Host "?? Abriendo archivo CHM..." -ForegroundColor Green
            Write-Host "   $($chmFile.FullName)" -ForegroundColor Gray
            Write-Host ""
            
            # Desbloquear el archivo CHM (por si Windows lo bloqueó)
            Unblock-File -Path $chmFile.FullName -ErrorAction SilentlyContinue
            
            try {
                Start-Process $chmFile.FullName
                Write-Host "? Documentación CHM abierta" -ForegroundColor Green
            } catch {
                Write-Host "? ERROR al abrir CHM: $_" -ForegroundColor Red
                Write-Host ""
                Write-Host "Intenta abrir manualmente:" -ForegroundColor Yellow
                Write-Host "   $($chmFile.FullName)" -ForegroundColor Gray
            }
        } else {
            Write-Host "? ERROR: No se encontró archivo CHM" -ForegroundColor Red
            Write-Host ""
            Write-Host "Verifica que el proyecto Sandcastle esté configurado para generar formato HtmlHelp1" -ForegroundColor Yellow
        }
    }
    
    "HTML" {
        if ($htmlFile) {
            Write-Host "?? Abriendo sitio web HTML..." -ForegroundColor Green
            Write-Host "   $($htmlFile.FullName)" -ForegroundColor Gray
            Write-Host ""
            
            try {
                Start-Process $htmlFile.FullName
                Write-Host "? Documentación HTML abierta en navegador" -ForegroundColor Green
            } catch {
                Write-Host "? ERROR al abrir HTML: $_" -ForegroundColor Red
                Write-Host ""
                Write-Host "Intenta abrir manualmente:" -ForegroundColor Yellow
                Write-Host "   $($htmlFile.FullName)" -ForegroundColor Gray
            }
        } else {
            Write-Host "? ERROR: No se encontró archivo index.html" -ForegroundColor Red
            Write-Host ""
            Write-Host "Verifica que el proyecto Sandcastle esté configurado para generar formato Website" -ForegroundColor Yellow
        }
    }
}

Write-Host ""
Write-Host "?? Información adicional:" -ForegroundColor Cyan

if ($chmFile) {
    Write-Host "   CHM: $($chmFile.Name) - $([math]::Round($chmFile.Length/1MB, 2)) MB" -ForegroundColor Gray
}

if ($htmlFile) {
    $htmlCount = (Get-ChildItem "Help" -Filter "*.html" -Recurse).Count
    Write-Host "   HTML: $htmlCount páginas" -ForegroundColor Gray
}

Write-Host ""
Write-Host "?? Consejo:" -ForegroundColor Yellow
Write-Host "   Para alternar entre formatos, usa:" -ForegroundColor Gray
Write-Host "   .\VerDocumentacionSandcastle.ps1 -Formato CHM" -ForegroundColor White
Write-Host "   .\VerDocumentacionSandcastle.ps1 -Formato HTML" -ForegroundColor White
Write-Host ""
