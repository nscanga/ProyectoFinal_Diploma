# =========================================================
# Script: Habilitar XML Documentation en todos los proyectos
# Descripción: Modifica todos los .csproj para generar XML docs
# =========================================================

Write-Host "?? Habilitando XML Documentation en todos los proyectos..." -ForegroundColor Cyan
Write-Host ""

$proyectos = @(
    "BLL\BLL.csproj",
    "DAL\DAL.csproj",
    "DOMAIN\DOMAIN.csproj",
    "Service\Service.csproj",
    "Distribuidora_los_amigos\UI.csproj"
)

$modificados = 0
$errores = 0

foreach ($proyecto in $proyectos) {
    Write-Host "?? Procesando: $proyecto" -ForegroundColor Yellow
    
    if (-not (Test-Path $proyecto)) {
        Write-Host "   ? ERROR: Proyecto no encontrado" -ForegroundColor Red
        $errores++
        continue
    }
    
    try {
        # Leer contenido del proyecto
        [xml]$xml = Get-Content $proyecto
        
        # Buscar PropertyGroup existente
        $propertyGroup = $xml.Project.PropertyGroup | Where-Object { $_.Configuration -eq $null } | Select-Object -First 1
        
        if ($propertyGroup -eq $null) {
            # Crear nuevo PropertyGroup si no existe
            $propertyGroup = $xml.CreateElement("PropertyGroup", $xml.Project.NamespaceURI)
            $xml.Project.AppendChild($propertyGroup) | Out-Null
        }
        
        # Verificar si ya tiene DocumentationFile
        $docFile = $propertyGroup.DocumentationFile
        
        if ([string]::IsNullOrEmpty($docFile)) {
            # Agregar DocumentationFile
            $docFileNode = $xml.CreateElement("DocumentationFile", $xml.Project.NamespaceURI)
            $docFileNode.InnerText = 'bin\$(Configuration)\$(AssemblyName).xml'
            $propertyGroup.AppendChild($docFileNode) | Out-Null
            
            Write-Host "   ? XML Documentation habilitado" -ForegroundColor Green
            $modificados++
        } else {
            Write-Host "   ??  XML Documentation ya estaba habilitado" -ForegroundColor Gray
        }
        
        # Guardar cambios
        $xml.Save((Resolve-Path $proyecto))
        
    } catch {
        Write-Host "   ? ERROR: $_" -ForegroundColor Red
        $errores++
    }
    
    Write-Host ""
}

Write-Host "================================" -ForegroundColor Cyan
Write-Host "?? RESUMEN:" -ForegroundColor Cyan
Write-Host "   ? Proyectos modificados: $modificados" -ForegroundColor Green
Write-Host "   ??  Errores: $errores" -ForegroundColor $(if($errores -gt 0){"Red"}else{"Green"})
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

if ($modificados -gt 0) {
    Write-Host "? IMPORTANTE: Rebuild la solución para generar archivos XML" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Ejecuta:" -ForegroundColor White
    Write-Host "   msbuild Distribuidora_los_amigos.sln /t:Rebuild /p:Configuration=Release" -ForegroundColor Gray
    Write-Host ""
}

Write-Host "? Proceso completado" -ForegroundColor Green
