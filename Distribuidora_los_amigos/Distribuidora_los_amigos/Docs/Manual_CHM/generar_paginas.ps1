# ============================================
# Generador Automático de Páginas HTML
# Distribuidora Los Amigos
# ============================================

param(
    [string]$BaseDir = "C:\DistribuidoraLosAmigos\Manual\source_es"
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Generador de Páginas HTML" -ForegroundColor Cyan
Write-Host "  Distribuidora Los Amigos" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Definición de todas las páginas
$paginas = @(
    # General
    @{TopicID=20; File="html/general/topic_20_main.html"; Title="Pantalla Principal"; Categoria="General"; Menu="Inicio"; Submenu="Pantalla Principal"},
    @{TopicID=21; File="html/general/topic_21_intro.html"; Title="Introducción al Sistema"; Categoria="General"; Menu="Inicio"; Submenu="Introducción"},
    
    # Login
    @{TopicID=31; File="html/login/topic_31_login.html"; Title="Iniciar Sesión"; Categoria="Seguridad"; Menu="Login"; Submenu="Iniciar Sesión"},
    @{TopicID=32; File="html/login/topic_32_recuperar.html"; Title="Recuperar Contraseña"; Categoria="Seguridad"; Menu="Login"; Submenu="Recuperar Contraseña"},
    @{TopicID=33; File="html/login/topic_33_cambiar_pass.html"; Title="Cambiar Contraseña"; Categoria="Seguridad"; Menu="Login"; Submenu="Cambiar Contraseña"},
    
    # Usuarios
    @{TopicID=23; File="html/usuarios/topic_23_crear_usuario.html"; Title="Crear Usuario"; Categoria="Gestión de Usuarios"; Menu="Gestión ? Usuarios"; Submenu="Crear Usuario"},
    @{TopicID=24; File="html/usuarios/topic_24_asignar_rol.html"; Title="Asignar Rol a Usuario"; Categoria="Gestión de Usuarios"; Menu="Gestión ? Usuarios"; Submenu="Asignar Rol"},
    @{TopicID=28; File="html/usuarios/topic_28_modificar_usuario.html"; Title="Modificar Usuario"; Categoria="Gestión de Usuarios"; Menu="Gestión ? Usuarios"; Submenu="Modificar Usuario"},
    @{TopicID=29; File="html/usuarios/topic_29_mostrar_usuarios.html"; Title="Consultar Usuarios"; Categoria="Gestión de Usuarios"; Menu="Gestión ? Usuarios"; Submenu="Mostrar Usuarios"},
    
    # Roles
    @{TopicID=26; File="html/roles/topic_26_crear_rol.html"; Title="Crear Rol (Familia)"; Categoria="Roles y Permisos"; Menu="Gestión ? Roles"; Submenu="Crear Rol"},
    @{TopicID=27; File="html/roles/topic_27_crear_patente.html"; Title="Crear Patente"; Categoria="Roles y Permisos"; Menu="Gestión ? Permisos"; Submenu="Crear Patente"},
    
    # Backup
    @{TopicID=22; File="html/backup/topic_22_backup.html"; Title="Realizar Backup"; Categoria="Backup y Restore"; Menu="Backup"; Submenu="Realizar Backup"},
    @{TopicID=25; File="html/backup/topic_25_bitacora.html"; Title="Bitácora del Sistema"; Categoria="Auditoría"; Menu="Gestión"; Submenu="Bitácora"},
    @{TopicID=30; File="html/backup/topic_30_restore.html"; Title="Restaurar Base de Datos"; Categoria="Backup y Restore"; Menu="Backup"; Submenu="Restore"},
    
    # Clientes
    @{TopicID=40; File="html/clientes/topic_40_crear_cliente.html"; Title="Crear Cliente"; Categoria="Gestión de Clientes"; Menu="Clientes"; Submenu="Crear Cliente"},
    @{TopicID=41; File="html/clientes/topic_41_modificar_cliente.html"; Title="Modificar Cliente"; Categoria="Gestión de Clientes"; Menu="Clientes"; Submenu="Modificar Cliente"},
    @{TopicID=42; File="html/clientes/topic_42_mostrar_clientes.html"; Title="Consultar Clientes"; Categoria="Gestión de Clientes"; Menu="Clientes"; Submenu="Mostrar Clientes"},
    
    # Productos
    @{TopicID=50; File="html/productos/topic_50_crear_producto.html"; Title="Crear Producto"; Categoria="Gestión de Productos"; Menu="Productos"; Submenu="Crear Producto"},
    @{TopicID=51; File="html/productos/topic_51_modificar_producto.html"; Title="Modificar Producto"; Categoria="Gestión de Productos"; Menu="Productos"; Submenu="Modificar Producto"},
    @{TopicID=52; File="html/productos/topic_52_mostrar_productos.html"; Title="Consultar Productos"; Categoria="Gestión de Productos"; Menu="Productos"; Submenu="Mostrar Productos"},
    @{TopicID=53; File="html/productos/topic_53_eliminar_producto.html"; Title="Eliminar Producto"; Categoria="Gestión de Productos"; Menu="Productos"; Submenu="Eliminar Producto"},
    
    # Proveedores
    @{TopicID=60; File="html/proveedores/topic_60_crear_proveedor.html"; Title="Crear Proveedor"; Categoria="Gestión de Proveedores"; Menu="Proveedores"; Submenu="Crear Proveedor"},
    @{TopicID=61; File="html/proveedores/topic_61_modificar_proveedor.html"; Title="Modificar Proveedor"; Categoria="Gestión de Proveedores"; Menu="Proveedores"; Submenu="Modificar Proveedor"},
    @{TopicID=62; File="html/proveedores/topic_62_mostrar_proveedores.html"; Title="Consultar Proveedores"; Categoria="Gestión de Proveedores"; Menu="Proveedores"; Submenu="Mostrar Proveedores"},
    
    # Stock
    @{TopicID=70; File="html/stock/topic_70_mostrar_stock.html"; Title="Consultar Stock"; Categoria="Gestión de Stock"; Menu="Stock"; Submenu="Mostrar Stock"},
    @{TopicID=71; File="html/stock/topic_71_modificar_stock.html"; Title="Modificar Stock"; Categoria="Gestión de Stock"; Menu="Stock"; Submenu="Modificar Stock"},
    
    # Pedidos
    @{TopicID=80; File="html/pedidos/topic_80_crear_pedido.html"; Title="Crear Pedido"; Categoria="Gestión de Pedidos"; Menu="Pedidos"; Submenu="Crear Pedido"},
    @{TopicID=81; File="html/pedidos/topic_81_modificar_pedido.html"; Title="Modificar Pedido"; Categoria="Gestión de Pedidos"; Menu="Pedidos"; Submenu="Modificar Pedido"},
    @{TopicID=82; File="html/pedidos/topic_82_mostrar_pedidos.html"; Title="Consultar Pedidos"; Categoria="Gestión de Pedidos"; Menu="Pedidos"; Submenu="Mostrar Pedidos"},
    @{TopicID=83; File="html/pedidos/topic_83_detalle_pedido.html"; Title="Detalle del Pedido"; Categoria="Gestión de Pedidos"; Menu="Pedidos"; Submenu="Detalle Pedido"},
    
    # Reportes
    @{TopicID=90; File="html/reportes/topic_90_reporte_stock_bajo.html"; Title="Reporte Stock Bajo"; Categoria="Reportes"; Menu="Reportes"; Submenu="Stock Bajo"},
    @{TopicID=91; File="html/reportes/topic_91_reporte_mas_vendidos.html"; Title="Productos Más Vendidos"; Categoria="Reportes"; Menu="Reportes"; Submenu="Más Vendidos"}
)

function New-HTMLPage {
    param($Pagina)
    
    $filepath = Join-Path $BaseDir $Pagina.File
    
    # Si ya existe, preguntar si sobrescribir
    if (Test-Path $filepath) {
        Write-Host "??  Ya existe: $($Pagina.File)" -ForegroundColor Yellow
        return
    }
    
    # Calcular ruta relativa para CSS
    $depth = ($Pagina.File -split '/').Count - 2
    $cssPath = "../" * $depth + "css/styles.css"
    
    # Generar contenido HTML
    $html = @"
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>$($Pagina.Title) - Distribuidora Los Amigos</title>
    <link rel="stylesheet" href="$cssPath">
</head>
<body>
    
    <div class="breadcrumb">
        <a href="../general/topic_20_main.html">Inicio</a>
        <span>›</span>
        <a href="#">$($Pagina.Categoria)</a>
        <span>›</span>
        <span>$($Pagina.Title)</span>
    </div>

    <h1>$($Pagina.Title)</h1>
    
    <section id="introduccion">
        <h2>Introducción</h2>
        <p>
            Esta funcionalidad permite [DESCRIBIR LA FUNCIONALIDAD].
        </p>
        <p>
            [EXPLICAR PARA QUÉ SIRVE Y CUÁNDO USARLA].
        </p>
    </section>
    
    <section id="acceso">
        <h2>Cómo Acceder</h2>
        <p>Para acceder a esta funcionalidad:</p>
        <ol>
            <li>Desde el menú principal, ir a <strong>$($Pagina.Menu)</strong></li>
            <li>Seleccionar <strong>$($Pagina.Submenu)</strong></li>
            <li>Se abrirá la ventana de <strong>$($Pagina.Title)</strong></li>
        </ol>
    </section>
    
    <section id="campos">
        <h2>Descripción de Campos</h2>
        <p>A continuación se describe cada campo del formulario:</p>
        
        <table>
            <thead>
                <tr>
                    <th>Campo</th>
                    <th>Descripción</th>
                    <th>Tipo</th>
                    <th>Obligatorio</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><strong>[Nombre del Campo]</strong></td>
                    <td>[Descripción del campo]</td>
                    <td>[Tipo de dato]</td>
                    <td>Sí / No</td>
                </tr>
                <!-- AGREGAR MÁS CAMPOS AQUÍ -->
            </tbody>
        </table>
    </section>
    
    <section id="procedimiento">
        <h2>Procedimiento</h2>
        
        <div class="step">
            <span class="step-number">1</span>
            <strong>[Título del Paso 1]</strong>
            <p>[Descripción del paso 1]</p>
        </div>
        
        <div class="step">
            <span class="step-number">2</span>
            <strong>[Título del Paso 2]</strong>
            <p>[Descripción del paso 2]</p>
        </div>
        
        <!-- AGREGAR MÁS PASOS SEGÚN NECESIDAD -->
    </section>
    
    <section id="validaciones">
        <h2>Validaciones del Sistema</h2>
        <p>El sistema realiza las siguientes validaciones:</p>
        <ul>
            <li>[Validación 1]</li>
            <li>[Validación 2]</li>
            <li>[Validación 3]</li>
        </ul>
    </section>
    
    <section id="consejos">
        <h2>Consejos y Recomendaciones</h2>
        
        <div class="tip">
            [Consejo útil para el usuario]
        </div>
        
        <div class="warning">
            <strong>?? Advertencia:</strong> [Información importante]
        </div>
    </section>
    
    <section id="errores">
        <h2>Errores Comunes y Soluciones</h2>
        
        <h3>Error: "[Mensaje de error]"</h3>
        <div class="error">
            <strong>Causa:</strong> [Explicación del error]
        </div>
        <div class="success">
            <strong>? Solución:</strong> [Pasos para resolverlo]
        </div>
    </section>
    
    <section id="permisos">
        <h2>Permisos Necesarios</h2>
        <p>Para utilizar esta funcionalidad, el usuario debe tener los siguientes permisos:</p>
        <ul>
            <li><code>[NombrePatente]</code> - [Descripción del permiso]</li>
        </ul>
        
        <div class="warning">
            <strong>?? Importante:</strong> Si no tiene el permiso necesario, contacte al administrador del sistema.
        </div>
    </section>
    
    <section id="relacionados">
        <h2>Ver También</h2>
        <ul class="no-bullets">
            <li>?? <a href="../general/topic_20_main.html">Pantalla Principal</a></li>
            <!-- AGREGAR LINKS RELACIONADOS -->
        </ul>
    </section>
    
    <footer>
        <p>© 2024 Distribuidora Los Amigos - Sistema de Gestión</p>
        <p>Todos los derechos reservados</p>
    </footer>

</body>
</html>
"@

    # Crear el archivo
    $html | Out-File -FilePath $filepath -Encoding UTF8
    Write-Host "? Creado: $($Pagina.File)" -ForegroundColor Green
}

# Generar todas las páginas
Write-Host "?? Generando páginas HTML..." -ForegroundColor Cyan
Write-Host ""

$creadas = 0
$existentes = 0

foreach ($pagina in $paginas) {
    $filepath = Join-Path $BaseDir $pagina.File
    if (Test-Path $filepath) {
        $existentes++
        Write-Host "??  Ya existe: $($pagina.File)" -ForegroundColor Yellow
    } else {
        New-HTMLPage -Pagina $pagina
        $creadas++
    }
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "?? RESUMEN" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "? Páginas creadas:    $creadas" -ForegroundColor Green
Write-Host "??  Ya existían:       $existentes" -ForegroundColor Yellow
Write-Host "?? Total de páginas:   $($paginas.Count)" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? Ubicación: $BaseDir" -ForegroundColor White
Write-Host ""
Write-Host "?? Próximos pasos:" -ForegroundColor Cyan
Write-Host "  1. Editar cada archivo HTML con el contenido específico" -ForegroundColor White
Write-Host "  2. Reemplazar los textos [ENTRE CORCHETES]" -ForegroundColor White
Write-Host "  3. Agregar capturas de pantalla si es necesario" -ForegroundColor White
Write-Host "  4. Compilar el CHM usando compilar_chm.ps1" -ForegroundColor White
Write-Host ""
Write-Host "? ¡Generación completada!" -ForegroundColor Green
