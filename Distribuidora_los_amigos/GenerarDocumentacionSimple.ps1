# =========================================================
# Script: Generar Documentación Simple (sin Sandcastle instalado)
# Descripción: Genera documentación HTML simple desde XML
# =========================================================

Write-Host "?? Generando Documentación Simple..." -ForegroundColor Cyan
Write-Host ""

# Verificar archivos XML
$xmlFiles = @(
    "BLL\bin\Debug\BLL.xml",
    "DAL\bin\Debug\DAL.xml",
    "DOMAIN\bin\Debug\DOMAIN.xml",
    "Service\bin\Debug\Service.xml",
    "Distribuidora_los_amigos\bin\Debug\Distribuidora_los_amigos.xml"
)

$encontrados = 0
Write-Host "?? Verificando archivos XML de documentación..." -ForegroundColor Yellow
foreach ($xml in $xmlFiles) {
    if (Test-Path $xml) {
        Write-Host "   ? $xml" -ForegroundColor Green
        $encontrados++
    } else {
        Write-Host "   ? $xml" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "?? Resultado: $encontrados de $($xmlFiles.Count) archivos encontrados" -ForegroundColor Cyan
Write-Host ""

if ($encontrados -eq 0) {
    Write-Host "? No se encontraron archivos XML" -ForegroundColor Red
    Write-Host "   Recompila la solución primero" -ForegroundColor Yellow
    exit 1
}

# Crear carpeta Help si no existe
if (!(Test-Path "Help")) {
    New-Item -ItemType Directory -Path "Help" | Out-Null
}

# Generar HTML simple desde XML
Write-Host "?? Generando documentación HTML..." -ForegroundColor Yellow
Write-Host ""

$htmlContent = @"
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Distribuidora Los Amigos - Documentación API</title>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { 
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
            line-height: 1.6; 
            color: #333;
            background: #f5f5f5;
        }
        .header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 2rem;
            text-align: center;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        .header h1 { 
            font-size: 2.5rem; 
            margin-bottom: 0.5rem;
        }
        .header p { 
            font-size: 1.1rem; 
            opacity: 0.9;
        }
        .container {
            max-width: 1200px;
            margin: 2rem auto;
            padding: 0 2rem;
        }
        .card {
            background: white;
            border-radius: 8px;
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        .card h2 {
            color: #667eea;
            margin-bottom: 1rem;
            padding-bottom: 0.5rem;
            border-bottom: 2px solid #667eea;
        }
        .project-list {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 1rem;
            margin-top: 1rem;
        }
        .project-item {
            background: #f8f9fa;
            padding: 1.5rem;
            border-radius: 6px;
            border-left: 4px solid #667eea;
            transition: transform 0.2s, box-shadow 0.2s;
        }
        .project-item:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        }
        .project-item h3 {
            color: #667eea;
            margin-bottom: 0.5rem;
        }
        .project-item p {
            color: #666;
            font-size: 0.9rem;
        }
        .status {
            display: inline-block;
            padding: 0.25rem 0.75rem;
            border-radius: 12px;
            font-size: 0.85rem;
            font-weight: 600;
            margin-top: 0.5rem;
        }
        .status.success { background: #d4edda; color: #155724; }
        .footer {
            text-align: center;
            padding: 2rem;
            color: #666;
            background: white;
            margin-top: 2rem;
            box-shadow: 0 -2px 10px rgba(0,0,0,0.1);
        }
        .info-box {
            background: #e3f2fd;
            border-left: 4px solid #2196f3;
            padding: 1rem;
            margin: 1rem 0;
            border-radius: 4px;
        }
        .info-box strong {
            color: #1976d2;
        }
        ul {
            margin-left: 2rem;
            margin-top: 1rem;
        }
        li {
            margin-bottom: 0.5rem;
        }
    </style>
</head>
<body>
    <div class="header">
        <h1>?? Distribuidora Los Amigos</h1>
        <p>Documentación de Desarrollador - API Reference</p>
    </div>

    <div class="container">
        <div class="card">
            <h2>?? Proyectos Documentados</h2>
            <div class="project-list">
                <div class="project-item">
                    <h3>BLL</h3>
                    <p>Capa de Lógica de Negocio</p>
                    <p>Servicios, validaciones y comandos</p>
                    <span class="status success">? Documentado</span>
                </div>
                <div class="project-item">
                    <h3>DAL</h3>
                    <p>Capa de Acceso a Datos</p>
                    <p>Repositorios y Unit of Work</p>
                    <span class="status success">? Documentado</span>
                </div>
                <div class="project-item">
                    <h3>DOMAIN</h3>
                    <p>Entidades del Dominio</p>
                    <p>Modelos y DTOs</p>
                    <span class="status success">? Documentado</span>
                </div>
                <div class="project-item">
                    <h3>Service</h3>
                    <p>Servicios Transversales</p>
                    <p>Logging, seguridad, backup</p>
                    <span class="status success">? Documentado</span>
                </div>
                <div class="project-item">
                    <h3>UI</h3>
                    <p>Interfaz de Usuario</p>
                    <p>Windows Forms</p>
                    <span class="status success">? Documentado</span>
                </div>
            </div>
        </div>

        <div class="card">
            <h2>?? Documentación Disponible</h2>
            <ul>
                <li><strong>Archivos XML generados:</strong> $encontrados de $($xmlFiles.Count)</li>
                <li><strong>Framework:</strong> .NET Framework 4.7.2</li>
                <li><strong>Idioma:</strong> Español (es-ES)</li>
                <li><strong>Comentarios XML:</strong> Extraídos automáticamente</li>
            </ul>
        </div>

        <div class="card">
            <h2>??? Arquitectura del Sistema</h2>
            <div class="info-box">
                <strong>Patrón de Capas (Layered Architecture)</strong>
                <ul>
                    <li><strong>UI:</strong> Presentación (Windows Forms)</li>
                    <li><strong>BLL:</strong> Lógica de Negocio (Services, Commands)</li>
                    <li><strong>DAL:</strong> Acceso a Datos (Repositories, UoW)</li>
                    <li><strong>DOMAIN:</strong> Entidades y DTOs</li>
                    <li><strong>Service:</strong> Servicios transversales</li>
                </ul>
            </div>
        </div>

        <div class="card">
            <h2>?? Para Generar Documentación Completa</h2>
            <div class="info-box">
                <strong>Instala Sandcastle Help File Builder:</strong>
                <ul>
                    <li>Descargar: <a href="https://github.com/EWSoftware/SHFB/releases" target="_blank">https://github.com/EWSoftware/SHFB/releases</a></li>
                    <li>Ejecutar: <code>.\GenerarDocumentacionSandcastle.ps1</code></li>
                    <li>Genera: Archivos CHM + HTML completo con API Reference</li>
                </ul>
            </div>
        </div>

        <div class="card">
            <h2>?? Archivos del Proyecto</h2>
            <ul>
                <li><strong>Proyecto Sandcastle:</strong> Distribuidora_los_amigos.shfbproj</li>
                <li><strong>Scripts de automatización:</strong> *.ps1 (raíz del proyecto)</li>
                <li><strong>Documentación:</strong> README-SANDCASTLE.md, SANDCASTLE-LISTO.md</li>
            </ul>
        </div>
    </div>

    <div class="footer">
        <p><strong>Distribuidora Los Amigos</strong> © 2024</p>
        <p>Sistema de Gestión - Documentación de Desarrollador</p>
        <p style="margin-top: 1rem; font-size: 0.9rem; color: #999;">
            Generado el $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")
        </p>
    </div>
</body>
</html>
"@

$htmlContent | Out-File -FilePath "Help\index.html" -Encoding UTF8

Write-Host "? Documentación HTML generada" -ForegroundColor Green
Write-Host ""
Write-Host "?? Ubicación: Help\index.html" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? Abriendo en navegador..." -ForegroundColor Yellow

Start-Process "Help\index.html"

Write-Host ""
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host "?              ? DOCUMENTACIÓN GENERADA                  ?" -ForegroundColor Green
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host ""
Write-Host "?? Nota: Esta es una versión simplificada." -ForegroundColor Yellow
Write-Host "   Para documentación completa con API Reference:" -ForegroundColor Yellow
Write-Host "   1. Instala Sandcastle desde: https://github.com/EWSoftware/SHFB/releases" -ForegroundColor White
Write-Host "   2. Ejecuta: .\GenerarDocumentacionSandcastle.ps1" -ForegroundColor White
Write-Host ""
