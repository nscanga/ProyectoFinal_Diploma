# =========================================================
# Script: Generar Documentación Completa (XmlDoc2CmdletDoc)
# Descripción: Genera documentación HTML avanzada desde XML
# =========================================================

Write-Host "?? Generando Documentación Completa..." -ForegroundColor Cyan
Write-Host ""

# Verificar archivos XML
$projects = @(
    @{Name="BLL"; Path="BLL\bin\Debug\BLL"; Desc="Lógica de Negocio"},
    @{Name="DAL"; Path="DAL\bin\Debug\DAL"; Desc="Acceso a Datos"},
    @{Name="DOMAIN"; Path="DOMAIN\bin\Debug\DOMAIN"; Desc="Entidades del Dominio"},
    @{Name="Service"; Path="Service\bin\Debug\Service"; Desc="Servicios Transversales"},
    @{Name="UI"; Path="Distribuidora_los_amigos\bin\Debug\Distribuidora_los_amigos"; Desc="Interfaz de Usuario"; IsExe=$true}
)

Write-Host "?? Verificando archivos..." -ForegroundColor Yellow
$allFound = $true
foreach($proj in $projects) {
    $ext = if($proj.IsExe) { ".exe" } else { ".dll" }
    $dllPath = "$($proj.Path)$ext"
    $xmlPath = "$($proj.Path).xml"
    
    if((Test-Path $dllPath) -and (Test-Path $xmlPath)) {
        Write-Host "   ? $($proj.Name)" -ForegroundColor Green
    } else {
        Write-Host "   ? $($proj.Name) - DLL: $(Test-Path $dllPath), XML: $(Test-Path $xmlPath)" -ForegroundColor Red
        $allFound = $false
    }
}

Write-Host ""

if(-not $allFound) {
    Write-Host "? Faltan archivos. Recompila la solución." -ForegroundColor Red
    exit 1
}

# Crear estructura de carpetas
Write-Host "?? Creando estructura..." -ForegroundColor Yellow
$folders = @("Help", "Help\api", "Help\css", "Help\js")
foreach($folder in $folders) {
    if(!(Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
    }
}

# Generar CSS
Write-Host "?? Generando estilos..." -ForegroundColor Yellow
$css = @"
:root {
    --primary: #667eea;
    --secondary: #764ba2;
    --success: #10b981;
    --danger: #ef4444;
    --warning: #f59e0b;
    --info: #3b82f6;
    --dark: #1f2937;
    --light: #f3f4f6;
}

* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
    line-height: 1.6;
    color: #333;
    background: #f9fafb;
}

.header {
    background: linear-gradient(135deg, var(--primary) 0%, var(--secondary) 100%);
    color: white;
    padding: 3rem 2rem;
    box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}

.header h1 {
    font-size: 2.5rem;
    font-weight: 700;
    margin-bottom: 0.5rem;
    letter-spacing: -0.5px;
}

.header p {
    font-size: 1.2rem;
    opacity: 0.95;
}

.container {
    max-width: 1400px;
    margin: 0 auto;
    padding: 2rem;
}

.nav {
    background: white;
    padding: 1rem 2rem;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    position: sticky;
    top: 0;
    z-index: 100;
}

.nav ul {
    list-style: none;
    display: flex;
    gap: 2rem;
    flex-wrap: wrap;
}

.nav a {
    color: var(--dark);
    text-decoration: none;
    font-weight: 500;
    transition: color 0.2s;
    padding: 0.5rem 1rem;
    border-radius: 4px;
    display: inline-block;
}

.nav a:hover {
    color: var(--primary);
    background: var(--light);
}

.grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 1.5rem;
    margin: 2rem 0;
}

.card {
    background: white;
    border-radius: 8px;
    padding: 2rem;
    box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    transition: transform 0.2s, box-shadow 0.2s;
    border-top: 4px solid var(--primary);
}

.card:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 16px rgba(0,0,0,0.15);
}

.card h2 {
    color: var(--primary);
    margin-bottom: 1rem;
    font-size: 1.5rem;
}

.card h3 {
    color: var(--dark);
    margin: 1rem 0 0.5rem;
    font-size: 1.2rem;
}

.card h4 {
    color: var(--dark);
    margin: 1rem 0 0.5rem;
    font-size: 1.1rem;
    font-weight: 600;
}

.badge {
    display: inline-block;
    padding: 0.25rem 0.75rem;
    border-radius: 12px;
    font-size: 0.85rem;
    font-weight: 600;
    margin-right: 0.5rem;
}

.badge.primary { background: #e0e7ff; color: #4338ca; }
.badge.success { background: #d1fae5; color: #065f46; }
.badge.info { background: #dbeafe; color: #1e40af; }

.method {
    background: #f9fafb;
    border-left: 4px solid var(--info);
    padding: 1.5rem;
    margin: 1.5rem 0;
    border-radius: 4px;
}

.method h3 {
    color: var(--info);
    font-size: 1.3rem;
    margin-bottom: 1rem;
}

.method-signature {
    font-family: 'Consolas', 'Monaco', 'Courier New', monospace;
    background: #1f2937;
    color: #10b981;
    padding: 0.75rem 1rem;
    border-radius: 4px;
    overflow-x: auto;
    margin: 0.75rem 0;
    font-size: 0.9rem;
}

.param {
    margin-left: 1.5rem;
    margin-bottom: 0.75rem;
    line-height: 1.8;
}

.param strong {
    color: var(--primary);
    font-weight: 600;
}

.section {
    margin: 3rem 0;
}

.section-title {
    color: var(--dark);
    border-bottom: 3px solid var(--primary);
    padding-bottom: 0.75rem;
    margin-bottom: 2rem;
    font-size: 2rem;
    font-weight: 700;
}

.footer {
    background: white;
    text-align: center;
    padding: 2rem;
    margin-top: 4rem;
    box-shadow: 0 -2px 4px rgba(0,0,0,0.1);
    color: #666;
}

.footer p {
    margin: 0.5rem 0;
}

code {
    background: #f1f5f9;
    padding: 0.2rem 0.5rem;
    border-radius: 3px;
    font-family: 'Consolas', 'Monaco', monospace;
    font-size: 0.9em;
    color: #d63384;
}

pre {
    background: #1f2937;
    color: #e5e7eb;
    padding: 1.5rem;
    border-radius: 6px;
    overflow-x: auto;
    margin: 1rem 0;
    line-height: 1.5;
}

.toc {
    background: white;
    padding: 1.5rem;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    position: sticky;
    top: 80px;
}

.toc h3 {
    margin-bottom: 1rem;
    color: var(--primary);
}

.toc ul {
    list-style: none;
}

.toc li {
    margin: 0.5rem 0;
}

.toc a {
    color: var(--dark);
    text-decoration: none;
    transition: color 0.2s;
    padding: 0.25rem 0.5rem;
    display: block;
    border-radius: 4px;
}

.toc a:hover {
    color: var(--primary);
    background: var(--light);
}

a {
    color: var(--primary);
    text-decoration: none;
    transition: color 0.2s;
}

a:hover {
    color: var(--secondary);
    text-decoration: underline;
}

@media (max-width: 768px) {
    .grid {
        grid-template-columns: 1fr;
    }
    
    .header h1 {
        font-size: 2rem;
    }
    
    .nav ul {
        gap: 1rem;
    }
    
    .container {
        padding: 1rem;
    }
    
    .card {
        padding: 1.5rem;
    }
}

/* Animaciones */
@keyframes fadeIn {
    from {
        opacity: 0;
        transform: translateY(20px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}

.card, .method {
    animation: fadeIn 0.5s ease-out;
}
"@

$css | Out-File -FilePath "Help\css\styles.css" -Encoding UTF8

# Función para parsear XML y extraer documentación
function Parse-XmlDoc {
    param($XmlPath, $ProjectName, $Description)
    
    if(!(Test-Path $XmlPath)) { return @() }
    
    [xml]$xml = Get-Content $XmlPath
    $members = @()
    
    foreach($member in $xml.doc.members.member) {
        $name = $member.name
        $summary = ($member.summary | Out-String).Trim()
        $remarks = ($member.remarks | Out-String).Trim()
        $returns = ($member.returns | Out-String).Trim()
        $example = ($member.example | Out-String).Trim()
        
        $params = @()
        foreach($param in $member.param) {
            $params += @{
                Name = $param.name
                Description = ($param | Out-String).Trim()
            }
        }
        
        $exceptions = @()
        foreach($exc in $member.exception) {
            $exceptions += @{
                Type = $exc.cref
                Description = ($exc | Out-String).Trim()
            }
        }
        
        $members += @{
            Name = $name
            Summary = $summary
            Remarks = $remarks
            Returns = $returns
            Example = $example
            Params = $params
            Exceptions = $exceptions
        }
    }
    
    return @{
        Project = $ProjectName
        Description = $Description
        Members = $members
    }
}

# Parsear todos los proyectos
Write-Host "?? Procesando documentación XML..." -ForegroundColor Yellow
$allDocs = @()
foreach($proj in $projects) {
    $xmlPath = "$($proj.Path).xml"
    Write-Host "   Processing $($proj.Name)..." -ForegroundColor Gray
    $docs = Parse-XmlDoc -XmlPath $xmlPath -ProjectName $proj.Name -Description $proj.Desc
    $allDocs += $docs
}

# Generar index.html
Write-Host "?? Generando páginas HTML..." -ForegroundColor Yellow

$indexHtml = @"
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Distribuidora Los Amigos - API Documentation</title>
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <div class="header">
        <div class="container">
            <h1>&#128218; Distribuidora Los Amigos</h1>
            <p>Documentación Completa de API - .NET Framework 4.7.2</p>
        </div>
    </div>
    
    <nav class="nav">
        <ul>
            <li><a href="index.html">&#127968; Inicio</a></li>
            <li><a href="#projects">&#128230; Proyectos</a></li>
            <li><a href="#architecture">&#127959; Arquitectura</a></li>
            <li><a href="api/bll.html">BLL API</a></li>
            <li><a href="api/dal.html">DAL API</a></li>
            <li><a href="api/domain.html">DOMAIN API</a></li>
            <li><a href="api/service.html">Service API</a></li>
        </ul>
    </nav>
    
    <div class="container">
        <section id="projects" class="section">
            <h2 class="section-title">&#128230; Proyectos Documentados</h2>
            <div class="grid">
"@

foreach($proj in $projects) {
    $memberCount = ($allDocs | Where-Object { $_.Project -eq $proj.Name }).Members.Count
    $indexHtml += @"
                <div class="card">
                    <h2>$($proj.Name)</h2>
                    <p>$($proj.Desc)</p>
                    <span class="badge success">$memberCount miembros documentados</span>
                    <p style="margin-top: 1rem;">
                        <a href="api/$($proj.Name.ToLower()).html" style="color: var(--primary); font-weight: 600;">Ver documentación &rarr;</a>
                    </p>
                </div>
"@
}

$indexHtml += @"
            </div>
        </section>
        
        <section id="architecture" class="section">
            <h2 class="section-title">&#127959; Arquitectura del Sistema</h2>
            
            <!-- Patrón Principal -->
            <div class="card">
                <h3>&#128268; Patrón de Capas (Layered Architecture)</h3>
                <p>Arquitectura multicapa con separación de responsabilidades y bajo acoplamiento.</p>
                <div class="grid">
                    <div>
                        <h4>UI - Presentación</h4>
                        <p><strong>Tecnología:</strong> Windows Forms</p>
                        <p><strong>Responsabilidad:</strong> Interfaz de usuario, controles y vistas</p>
                        <p><strong>Componentes:</strong> Forms, UserControls, Dialogs</p>
                    </div>
                    <div>
                        <h4>BLL - Lógica de Negocio</h4>
                        <p><strong>Responsabilidad:</strong> Reglas de negocio y validaciones</p>
                        <p><strong>Componentes:</strong> Services, Commands, Validators</p>
                        <p><strong>Patrones:</strong> Command, Strategy, Factory</p>
                    </div>
                    <div>
                        <h4>DAL - Acceso a Datos</h4>
                        <p><strong>Responsabilidad:</strong> Persistencia de datos</p>
                        <p><strong>Componentes:</strong> Repositories, Unit of Work</p>
                        <p><strong>Patrones:</strong> Repository, UoW, Factory</p>
                    </div>
                    <div>
                        <h4>DOMAIN - Entidades</h4>
                        <p><strong>Responsabilidad:</strong> Modelos del dominio</p>
                        <p><strong>Componentes:</strong> Entities, DTOs, Value Objects</p>
                    </div>
                    <div>
                        <h4>Service - Transversal</h4>
                        <p><strong>Responsabilidad:</strong> Servicios compartidos</p>
                        <p><strong>Componentes:</strong> Logging, Security, Utilities</p>
                    </div>
                </div>
            </div>
            
            <!-- Patrones de Diseño -->
            <div class="card">
                <h3>&#128736; Patrones de Diseño Implementados</h3>
                
                <div class="method">
                    <h4>&#128736; Repository Pattern</h4>
                    <p><strong>Ubicación:</strong> DAL\Implementations\SqlServer\</p>
                    <p><strong>Propósito:</strong> Abstrae el acceso a datos y centraliza la lógica de consultas</p>
                    <p><strong>Implementación:</strong> IRepository&lt;T&gt;, SqlClienteRepository, SqlProductoRepository, etc.</p>
                    <p><strong>Beneficio:</strong> Desacoplamiento entre lógica de negocio y persistencia</p>
                </div>
                
                <div class="method">
                    <h4>&#128736; Unit of Work Pattern</h4>
                    <p><strong>Ubicación:</strong> DAL\Implementations\SqlServer\SqlUnitOfWork.cs</p>
                    <p><strong>Propósito:</strong> Gestiona transacciones y mantiene un contexto único</p>
                    <p><strong>Implementación:</strong> IUnitOfWork con BeginTransaction(), Commit(), Rollback()</p>
                    <p><strong>Beneficio:</strong> Consistencia transaccional y gestión centralizada de conexiones</p>
                </div>
                
                <div class="method">
                    <h4>&#128736; Command Pattern</h4>
                    <p><strong>Ubicación:</strong> BLL\Commands\</p>
                    <p><strong>Propósito:</strong> Encapsula operaciones como objetos</p>
                    <p><strong>Implementación:</strong> ICommand, CommandInvoker, CrearProductoCommand, etc.</p>
                    <p><strong>Beneficio:</strong> Deshacer/Rehacer, logging de operaciones, validación centralizada</p>
                </div>
                
                <div class="method">
                    <h4>&#128736; Factory Pattern</h4>
                    <p><strong>Ubicación:</strong> DAL\Factory\FactoryDAL.cs, Service\DAL\FactoryServices\</p>
                    <p><strong>Propósito:</strong> Centraliza la creación de objetos</p>
                    <p><strong>Implementación:</strong> FactoryDAL, LoggerFactory, FactoryServices</p>
                    <p><strong>Beneficio:</strong> Flexibilidad para cambiar implementaciones</p>
                </div>
                
                <div class="method">
                    <h4>&#128736; Strategy Pattern</h4>
                    <p><strong>Ubicación:</strong> BLL\Validators\</p>
                    <p><strong>Propósito:</strong> Define familia de algoritmos intercambiables</p>
                    <p><strong>Implementación:</strong> Validadores específicos por entidad</p>
                    <p><strong>Beneficio:</strong> Validaciones reutilizables y extensibles</p>
                </div>
                
                <div class="method">
                    <h4>&#128736; Facade Pattern</h4>
                    <p><strong>Ubicación:</strong> Service\Facade\BackUpService.cs, RecuperoPassService.cs</p>
                    <p><strong>Propósito:</strong> Simplifica interfaces complejas</p>
                    <p><strong>Implementación:</strong> Servicios que encapsulan múltiples operaciones</p>
                    <p><strong>Beneficio:</strong> API simplificada para operaciones complejas</p>
                </div>
                
                <div class="method">
                    <h4>&#128736; Singleton Pattern</h4>
                    <p><strong>Ubicación:</strong> Service\Logger\, Service\Security\</p>
                    <p><strong>Propósito:</strong> Garantiza una única instancia</p>
                    <p><strong>Implementación:</strong> Logger, SessionManager</p>
                    <p><strong>Beneficio:</strong> Control centralizado y recursos compartidos</p>
                </div>
            </div>
            
            <!-- Manejo de Excepciones -->
            <div class="card">
                <h3>&#9888; Sistema de Manejo de Excepciones</h3>
                
                <div class="method">
                    <h4>Jerarquía de Excepciones Personalizadas</h4>
                    <p><strong>Ubicación:</strong> BLL\Exceptions\</p>
                    <ul>
                        <li><code>BusinessException</code> - Excepción base de negocio</li>
                        <li><code>ClienteException</code> - Errores relacionados con clientes</li>
                        <li><code>ProductoException</code> - Errores de productos</li>
                        <li><code>PedidoException</code> - Errores de pedidos</li>
                        <li><code>StockException</code> - Errores de inventario</li>
                        <li><code>ProveedorException</code> - Errores de proveedores</li>
                        <li><code>DatabaseException</code> - Errores de base de datos</li>
                        <li><code>DALException</code> - Errores de capa de datos</li>
                    </ul>
                </div>
                
                <div class="method">
                    <h4>ErrorHandler y ExceptionMapper</h4>
                    <p><strong>Ubicación:</strong> Service\ManegerEx\ErrorHandler.cs, BLL\Helpers\ExceptionMapper.cs</p>
                    <p><strong>Funcionalidad:</strong></p>
                    <ul>
                        <li>Centraliza el manejo de errores</li>
                        <li>Traduce excepciones técnicas a mensajes de usuario</li>
                        <li>Logging automático de errores</li>
                        <li>Recuperación de errores cuando es posible</li>
                    </ul>
                </div>
            </div>
            
            <!-- Características Adicionales -->
            <div class="card">
                <h3>&#9889; Características y Componentes</h3>
                
                <div class="grid">
                    <div class="method">
                        <h4>&#128272; Seguridad</h4>
                        <ul>
                            <li>Gestión de sesiones</li>
                            <li>Control de permisos por rol</li>
                            <li>Encriptación de contraseñas</li>
                            <li>Recuperación de contraseña</li>
                            <li>Auditoría de accesos</li>
                        </ul>
                    </div>
                    
                    <div class="method">
                        <h4>&#128190; Backup y Restore</h4>
                        <ul>
                            <li>Backup automático programado</li>
                            <li>Backup manual</li>
                            <li>Restore de base de datos</li>
                            <li>Configuración flexible</li>
                            <li>Historial de backups</li>
                        </ul>
                    </div>
                    
                    <div class="method">
                        <h4>&#128221; Logging</h4>
                        <ul>
                            <li>Registro de operaciones</li>
                            <li>Niveles de log (Info, Warning, Error)</li>
                            <li>Logging a archivo</li>
                            <li>Rotación de logs</li>
                            <li>Contexto de usuario</li>
                        </ul>
                    </div>
                    
                    <div class="method">
                        <h4>&#10003; Validaciones</h4>
                        <ul>
                            <li>Validación de entidades</li>
                            <li>Validación de reglas de negocio</li>
                            <li>Validación en múltiples capas</li>
                            <li>Mensajes descriptivos</li>
                            <li>Validación asíncrona</li>
                        </ul>
                    </div>
                </div>
            </div>
            
            <!-- Flujo de Datos -->
            <div class="card">
                <h3>&#8594; Flujo de Datos y Comunicación</h3>
                
                <div class="method">
                    <h4>Flujo Típico de una Operación</h4>
                    <ol style="line-height: 2; margin-left: 2rem;">
                        <li><strong>UI Layer:</strong> Usuario interactúa con el formulario</li>
                        <li><strong>Validation:</strong> Validación inicial en UI</li>
                        <li><strong>BLL Layer:</strong> Service recibe la solicitud</li>
                        <li><strong>Command Pattern:</strong> Crea y ejecuta comando</li>
                        <li><strong>Business Rules:</strong> Aplica validaciones de negocio</li>
                        <li><strong>DAL Layer:</strong> Repository ejecuta operación</li>
                        <li><strong>Unit of Work:</strong> Gestiona transacción</li>
                        <li><strong>Database:</strong> Persiste cambios</li>
                        <li><strong>Response:</strong> Retorna resultado a UI</li>
                        <li><strong>Logging:</strong> Registra operación exitosa o error</li>
                    </ol>
                </div>
            </div>
            
            <!-- Tecnologías -->
            <div class="card">
                <h3>&#128187; Stack Tecnológico</h3>
                <div class="grid">
                    <div>
                        <h4>Framework</h4>
                        <ul>
                            <li>.NET Framework 4.7.2</li>
                            <li>C# 7.3</li>
                            <li>Windows Forms</li>
                        </ul>
                    </div>
                    <div>
                        <h4>Base de Datos</h4>
                        <ul>
                            <li>SQL Server</li>
                            <li>ADO.NET</li>
                            <li>Stored Procedures</li>
                        </ul>
                    </div>
                    <div>
                        <h4>Herramientas</h4>
                        <ul>
                            <li>Visual Studio 2022</li>
                            <li>SQL Server Management Studio</li>
                            <li>Git</li>
                        </ul>
                    </div>
                    <div>
                        <h4>Librerías</h4>
                        <ul>
                            <li>System.Data.SqlClient</li>
                            <li>System.Configuration</li>
                            <li>Custom Utilities</li>
                        </ul>
                    </div>
                </div>
            </div>
            
        </section>
        
        <section class="section">
            <h2 class="section-title">&#128202; Estadísticas</h2>
            <div class="grid">
                <div class="card">
                    <h3>$($projects.Count)</h3>
                    <p>Proyectos Documentados</p>
                </div>
                <div class="card">
                    <h3>$($allDocs.Members.Count)</h3>
                    <p>Miembros Totales</p>
                </div>
                <div class="card">
                    <h3>.NET Framework 4.7.2</h3>
                    <p>Plataforma</p>
                </div>
                <div class="card">
                    <h3>$(Get-Date -Format 'dd/MM/yyyy')</h3>
                    <p>Última Actualización</p>
                </div>
            </div>
        </section>
    </div>
    
    <footer class="footer">
        <p><strong>Distribuidora Los Amigos</strong> &copy; 2024</p>
        <p>Documentación generada automáticamente</p>
    </footer>
</body>
</html>
"@

$indexHtml | Out-File -FilePath "Help\index.html" -Encoding UTF8

# Generar páginas individuales por proyecto
foreach($projDoc in $allDocs) {
    $apiHtml = @"
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>$($projDoc.Project) API - Distribuidora Los Amigos</title>
    <link rel="stylesheet" href="../css/styles.css">
</head>
<body>
    <div class="header">
        <div class="container">
            <h1>$($projDoc.Project) API</h1>
            <p>$($projDoc.Description)</p>
        </div>
    </div>
    
    <nav class="nav">
        <ul>
            <li><a href="../index.html">&#127968; Inicio</a></li>
            <li><a href="bll.html">BLL</a></li>
            <li><a href="dal.html">DAL</a></li>
            <li><a href="domain.html">DOMAIN</a></li>
            <li><a href="service.html">Service</a></li>
        </ul>
    </nav>
    
    <div class="container">
        <section class="section">
            <h2 class="section-title">&#128203; Miembros Documentados</h2>
            <p>Total de miembros: <strong>$($projDoc.Members.Count)</strong></p>
        </section>
"@
    
    foreach($member in $projDoc.Members) {
        if($member.Summary) {
            $apiHtml += @"
        <div class="card method">
            <h3>$($member.Name -replace '^[TMF]:', '')</h3>
            <div class="method-signature">
                <code>$($member.Name)</code>
            </div>
"@
            if($member.Summary) {
                $apiHtml += "<p><strong>Resumen:</strong> $($member.Summary -replace '<[^>]+>','')</p>"
            }
            
            if($member.Params.Count -gt 0) {
                $apiHtml += "<h4>Parámetros:</h4><ul>"
                foreach($param in $member.Params) {
                    $apiHtml += "<li class='param'><strong>$($param.Name):</strong> $($param.Description -replace '<[^>]+>','')</li>"
                }
                $apiHtml += "</ul>"
            }
            
            if($member.Returns) {
                $apiHtml += "<p><strong>Retorna:</strong> $($member.Returns -replace '<[^>]+>','')</p>"
            }
            
            $apiHtml += "</div>"
        }
    }
    
    $apiHtml += @"
    </div>
    
    <footer class="footer">
        <p><strong>Distribuidora Los Amigos</strong> &copy; 2024</p>
    </footer>
</body>
</html>
"@
    
    $apiHtml | Out-File -FilePath "Help\api\$($projDoc.Project.ToLower()).html" -Encoding UTF8
}

Write-Host ""
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host "?         ? DOCUMENTACIÓN COMPLETA GENERADA              ?" -ForegroundColor Green
Write-Host "????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host ""
Write-Host "?? Ubicación: Help\" -ForegroundColor Cyan
Write-Host "   - index.html (página principal)" -ForegroundColor Gray
Write-Host "   - api/*.html (documentación por proyecto)" -ForegroundColor Gray
Write-Host ""
Write-Host "?? Abriendo navegador..." -ForegroundColor Yellow
Start-Process "Help\index.html"
Write-Host ""
Write-Host "? Documentación completa lista" -ForegroundColor Green
Write-Host ""
