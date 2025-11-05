# ============================================
# Generar Proyecto CHM desde HTML existentes
# ============================================

Write-Host ""
Write-Host "=========================================="
Write-Host "  GENERADOR DE PROYECTO CHM"
Write-Host "=========================================="
Write-Host ""

# Configuracion
$baseDir = "C:\DistribuidoraLosAmigos\Manual"
$sourceDir = "$baseDir\source_es"
$htmlDir = "$sourceDir\html"
$cssDir = "$sourceDir\css"

Write-Host "Directorio base: $baseDir"
Write-Host "Directorio fuente: $sourceDir"
Write-Host ""

# Verificar que exista la carpeta HTML
if (-not (Test-Path $htmlDir)) {
    Write-Host "[ERROR] No existe la carpeta HTML en: $htmlDir" -ForegroundColor Red
    Write-Host ""
    pause
    exit
}

Write-Host "[OK] Carpeta HTML encontrada" -ForegroundColor Green
Write-Host ""

# Crear archivo CSS si no existe
if (-not (Test-Path $cssDir)) {
    New-Item -ItemType Directory -Path $cssDir -Force | Out-Null
}

$cssFile = "$cssDir\styles.css"
if (-not (Test-Path $cssFile)) {
    Write-Host "Creando archivo CSS..."
    $cssContent = @"
body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    margin: 20px;
    background-color: #f5f5f5;
    line-height: 1.6;
}

h1 {
    color: #2c3e50;
    border-bottom: 3px solid #3498db;
    padding-bottom: 10px;
}

h2 {
    color: #34495e;
    margin-top: 20px;
}

.info {
    background-color: #e3f2fd;
    padding: 15px;
    border-left: 4px solid #2196f3;
    margin: 10px 0;
}

.warning {
    background-color: #fff3cd;
    padding: 15px;
    border-left: 4px solid #ffc107;
    margin: 10px 0;
}

code {
    background-color: #f4f4f4;
    padding: 2px 6px;
    border-radius: 3px;
    font-family: 'Courier New', monospace;
}

table {
    border-collapse: collapse;
    width: 100%;
    margin: 15px 0;
}

th, td {
    border: 1px solid #ddd;
    padding: 12px;
    text-align: left;
}

th {
    background-color: #3498db;
    color: white;
}

tr:nth-child(even) {
    background-color: #f9f9f9;
}
"@
    $cssContent | Out-File -FilePath $cssFile -Encoding UTF8
    Write-Host "[OK] Archivo CSS creado" -ForegroundColor Green
}

# Obtener todos los archivos HTML
Write-Host "Escaneando archivos HTML..."
$htmlFiles = Get-ChildItem -Path $htmlDir -Filter "*.html" -Recurse | Sort-Object FullName
Write-Host "Archivos HTML encontrados: $($htmlFiles.Count)" -ForegroundColor Cyan
Write-Host ""

if ($htmlFiles.Count -eq 0) {
    Write-Host "[ERROR] No se encontraron archivos HTML" -ForegroundColor Red
    pause
    exit
}

# Mostrar archivos encontrados
foreach ($file in $htmlFiles) {
    $relativePath = $file.FullName.Replace("$htmlDir\", "")
    Write-Host "  - $relativePath" -ForegroundColor Gray
}
Write-Host ""

# ============================================
# 1. CREAR ARCHIVO .HHP (Proyecto)
# ============================================
Write-Host "[1/3] Creando archivo help_es.hhp..." -ForegroundColor Yellow

$hhpContent = "[OPTIONS]`r`n"
$hhpContent += "Compatibility=1.1 or later`r`n"
$hhpContent += "Compiled file=..\help_es.chm`r`n"
$hhpContent += "Contents file=help_es.hhc`r`n"
$hhpContent += "Default topic=html\general\topic_20_main.html`r`n"
$hhpContent += "Display compile progress=Yes`r`n"
$hhpContent += "Full-text search=Yes`r`n"
$hhpContent += "Index file=help_es.hhk`r`n"
$hhpContent += "Language=0x0c0a Espanol (Espana - alfabetizacion internacional)`r`n"
$hhpContent += "Title=Manual de Usuario - Distribuidora Los Amigos`r`n"
$hhpContent += "Default Window=main`r`n"
$hhpContent += "`r`n"
$hhpContent += "[WINDOWS]`r`n"
$hhpContent += "main=`"Manual de Usuario - Distribuidora Los Amigos`",`"help_es.hhc`",`"help_es.hhk`",`"html\general\topic_20_main.html`",`"html\general\topic_20_main.html`",,,,,0x63520,,0x387e,,,,,,,,0`r`n"
$hhpContent += "`r`n"

# AGREGAR SECCION MAP para ayuda contextual (F1)
$hhpContent += "[MAP]`r`n"
$hhpContent += "#define TOPIC_MAIN 20`r`n"
$hhpContent += "#define TOPIC_LOGIN 31`r`n"
$hhpContent += "#define TOPIC_RECUPERAR_PASSWORD 32`r`n"
$hhpContent += "#define TOPIC_CAMBIAR_PASSWORD 33`r`n"
$hhpContent += "#define TOPIC_CREAR_USUARIO 23`r`n"
$hhpContent += "#define TOPIC_MODIFICAR_USUARIO 28`r`n"
$hhpContent += "#define TOPIC_MOSTRAR_USUARIOS 29`r`n"
$hhpContent += "#define TOPIC_ASIGNAR_ROL 24`r`n"
$hhpContent += "#define TOPIC_CREAR_ROL 26`r`n"
$hhpContent += "#define TOPIC_CREAR_PATENTE 27`r`n"
$hhpContent += "#define TOPIC_BACKUP 22`r`n"
$hhpContent += "#define TOPIC_RESTORE 30`r`n"
$hhpContent += "#define TOPIC_BITACORA 25`r`n"
$hhpContent += "#define TOPIC_CREAR_CLIENTE 40`r`n"
$hhpContent += "#define TOPIC_MODIFICAR_CLIENTE 41`r`n"
$hhpContent += "#define TOPIC_MOSTRAR_CLIENTES 42`r`n"
$hhpContent += "#define TOPIC_CREAR_PRODUCTO 50`r`n"
$hhpContent += "#define TOPIC_MODIFICAR_PRODUCTO 51`r`n"
$hhpContent += "#define TOPIC_MOSTRAR_PRODUCTOS 52`r`n"
$hhpContent += "#define TOPIC_ELIMINAR_PRODUCTO 53`r`n"
$hhpContent += "#define TOPIC_CREAR_PROVEEDOR 60`r`n"
$hhpContent += "#define TOPIC_MODIFICAR_PROVEEDOR 61`r`n"
$hhpContent += "#define TOPIC_MOSTRAR_PROVEEDORES 62`r`n"
$hhpContent += "#define TOPIC_MOSTRAR_STOCK 70`r`n"
$hhpContent += "#define TOPIC_MODIFICAR_STOCK 71`r`n"
$hhpContent += "#define TOPIC_CREAR_PEDIDO 80`r`n"
$hhpContent += "#define TOPIC_MODIFICAR_PEDIDO 81`r`n"
$hhpContent += "#define TOPIC_MOSTRAR_PEDIDOS 82`r`n"
$hhpContent += "#define TOPIC_DETALLE_PEDIDO 83`r`n"
$hhpContent += "#define TOPIC_REPORTE_STOCK_BAJO 90`r`n"
$hhpContent += "#define TOPIC_REPORTE_PRODUCTOS_MAS_VENDIDOS 91`r`n"
$hhpContent += "`r`n"

# AGREGAR SECCION ALIAS que mapea IDs a archivos HTML
$hhpContent += "[ALIAS]`r`n"
$hhpContent += "TOPIC_MAIN=html\general\topic_20_main.html`r`n"
$hhpContent += "TOPIC_LOGIN=html\login\topic_31_login.html`r`n"
$hhpContent += "TOPIC_RECUPERAR_PASSWORD=html\login\topic_32_recuperar_password.html`r`n"
$hhpContent += "TOPIC_CAMBIAR_PASSWORD=html\login\topic_33_cambiar_password.html`r`n"
$hhpContent += "TOPIC_CREAR_USUARIO=html\usuarios\topic_23_crear_usuario.html`r`n"
$hhpContent += "TOPIC_MODIFICAR_USUARIO=html\usuarios\topic_28_modificar_usuario.html`r`n"
$hhpContent += "TOPIC_MOSTRAR_USUARIOS=html\usuarios\topic_29_mostrar_usuarios.html`r`n"
$hhpContent += "TOPIC_ASIGNAR_ROL=html\roles\topic_24_asignar_rol.html`r`n"
$hhpContent += "TOPIC_CREAR_ROL=html\roles\topic_26_crear_rol.html`r`n"
$hhpContent += "TOPIC_CREAR_PATENTE=html\roles\topic_27_crear_patente.html`r`n"
$hhpContent += "TOPIC_BACKUP=html\backup\topic_22_backup.html`r`n"
$hhpContent += "TOPIC_RESTORE=html\backup\topic_30_restore.html`r`n"
$hhpContent += "TOPIC_BITACORA=html\reportes\topic_25_bitacora.html`r`n"
$hhpContent += "TOPIC_CREAR_CLIENTE=html\clientes\topic_40_crear_cliente.html`r`n"
$hhpContent += "TOPIC_MODIFICAR_CLIENTE=html\clientes\topic_41_modificar_cliente.html`r`n"
$hhpContent += "TOPIC_MOSTRAR_CLIENTES=html\clientes\topic_42_mostrar_clientes.html`r`n"
$hhpContent += "TOPIC_CREAR_PRODUCTO=html\productos\topic_50_crear_producto.html`r`n"
$hhpContent += "TOPIC_MODIFICAR_PRODUCTO=html\productos\topic_51_modificar_producto.html`r`n"
$hhpContent += "TOPIC_MOSTRAR_PRODUCTOS=html\productos\topic_52_mostrar_productos.html`r`n"
$hhpContent += "TOPIC_ELIMINAR_PRODUCTO=html\productos\topic_53_eliminar_producto.html`r`n"
$hhpContent += "TOPIC_CREAR_PROVEEDOR=html\proveedores\topic_60_crear_proveedor.html`r`n"
$hhpContent += "TOPIC_MODIFICAR_PROVEEDOR=html\proveedores\topic_61_modificar_proveedor.html`r`n"
$hhpContent += "TOPIC_MOSTRAR_PROVEEDORES=html\proveedores\topic_62_mostrar_proveedores.html`r`n"
$hhpContent += "TOPIC_MOSTRAR_STOCK=html\stock\topic_70_mostrar_stock.html`r`n"
$hhpContent += "TOPIC_MODIFICAR_STOCK=html\stock\topic_71_modificar_stock.html`r`n"
$hhpContent += "TOPIC_CREAR_PEDIDO=html\pedidos\topic_80_crear_pedido.html`r`n"
$hhpContent += "TOPIC_MODIFICAR_PEDIDO=html\pedidos\topic_81_modificar_pedido.html`r`n"
$hhpContent += "TOPIC_MOSTRAR_PEDIDOS=html\pedidos\topic_82_mostrar_pedidos.html`r`n"
$hhpContent += "TOPIC_DETALLE_PEDIDO=html\pedidos\topic_83_detalle_pedido.html`r`n"
$hhpContent += "TOPIC_REPORTE_STOCK_BAJO=html\reportes\topic_90_reporte_stock_bajo.html`r`n"
$hhpContent += "TOPIC_REPORTE_PRODUCTOS_MAS_VENDIDOS=html\reportes\topic_91_reporte_productos_mas_vendidos.html`r`n"
$hhpContent += "`r`n"

$hhpContent += "[FILES]`r`n"
$hhpContent += "css\styles.css`r`n"

# Agregar todos los archivos HTML al proyecto
foreach ($file in $htmlFiles) {
    $relativePath = $file.FullName.Replace("$sourceDir\", "").Replace("\", "\")
    $hhpContent += "$relativePath`r`n"
}

$hhpFile = "$sourceDir\help_es.hhp"
$hhpContent | Out-File -FilePath $hhpFile -Encoding UTF8 -NoNewline
Write-Host "[OK] help_es.hhp creado con mapeo de IDs para F1" -ForegroundColor Green
Write-Host ""

# ============================================
# 2. CREAR ARCHIVO .HHC (Tabla de Contenidos)
# ============================================
Write-Host "[2/3] Creando archivo help_es.hhc..." -ForegroundColor Yellow

# Funcion para extraer el titulo de un archivo HTML
function Get-HtmlTitle {
    param($filePath)
    
    try {
        $content = Get-Content $filePath -Raw -Encoding UTF8
        if ($content -match '<title>(.*?)</title>') {
            return $matches[1]
        }
        if ($content -match '<h1>(.*?)</h1>') {
            return $matches[1]
        }
    } catch {
        # Ignorar errores
    }
    
    return [System.IO.Path]::GetFileNameWithoutExtension($filePath)
}

$hhcContent = "<!DOCTYPE HTML PUBLIC `"-//IETF//DTD HTML//EN`">`r`n"
$hhcContent += "<HTML>`r`n"
$hhcContent += "<HEAD>`r`n"
$hhcContent += "<meta name=`"GENERATOR`" content=`"Microsoft&reg; HTML Help Workshop 4.1`">`r`n"
$hhcContent += "<!-- Sitemap 1.0 -->`r`n"
$hhcContent += "</HEAD><BODY>`r`n"
$hhcContent += "<OBJECT type=`"text/site properties`">`r`n"
$hhcContent += "`t<param name=`"Window Styles`" value=`"0x800025`">`r`n"
$hhcContent += "`t<param name=`"ImageType`" value=`"Folder`">`r`n"
$hhcContent += "</OBJECT>`r`n"
$hhcContent += "<UL>`r`n"

# Organizar archivos por carpeta
$folders = $htmlFiles | Group-Object { Split-Path $_.DirectoryName -Leaf }

foreach ($folder in $folders | Sort-Object Name) {
    $folderName = switch ($folder.Name) {
        "general" { "Informacion General" }
        "login" { "Inicio de Sesion" }
        "clientes" { "Gestion de Clientes" }
        "productos" { "Gestion de Productos" }
        "pedidos" { "Gestion de Pedidos" }
        "proveedores" { "Gestion de Proveedores" }
        "stock" { "Control de Stock" }
        "backup" { "Backup y Restore" }
        "reportes" { "Reportes" }
        "roles" { "Gestion de Roles" }
        "usuarios" { "Gestion de Usuarios" }
        default { $folder.Name }
    }
    
    $hhcContent += "`t<LI> <OBJECT type=`"text/sitemap`">`r`n"
    $hhcContent += "`t`t<param name=`"Name`" value=`"$folderName`">`r`n"
    $hhcContent += "`t`t</OBJECT>`r`n"
    $hhcContent += "`t<UL>`r`n"
    
    foreach ($file in $folder.Group | Sort-Object Name) {
        $title = Get-HtmlTitle $file.FullName
        $relativePath = $file.FullName.Replace("$sourceDir\", "").Replace("\", "\")
        
        $hhcContent += "`t`t<LI> <OBJECT type=`"text/sitemap`">`r`n"
        $hhcContent += "`t`t`t<param name=`"Name`" value=`"$title`">`r`n"
        $hhcContent += "`t`t`t<param name=`"Local`" value=`"$relativePath`">`r`n"
        $hhcContent += "`t`t`t</OBJECT>`r`n"
    }
    
    $hhcContent += "`t</UL>`r`n"
}

$hhcContent += "</UL>`r`n"
$hhcContent += "</BODY></HTML>`r`n"

$hhcFile = "$sourceDir\help_es.hhc"
$hhcContent | Out-File -FilePath $hhcFile -Encoding UTF8 -NoNewline
Write-Host "[OK] help_es.hhc creado" -ForegroundColor Green
Write-Host ""

# ============================================
# 3. CREAR ARCHIVO .HHK (Indice)
# ============================================
Write-Host "[3/3] Creando archivo help_es.hhk..." -ForegroundColor Yellow

$hhkContent = "<!DOCTYPE HTML PUBLIC `"-//IETF//DTD HTML//EN`">`r`n"
$hhkContent += "<HTML>`r`n"
$hhkContent += "<HEAD>`r`n"
$hhkContent += "<meta name=`"GENERATOR`" content=`"Microsoft&reg; HTML Help Workshop 4.1`">`r`n"
$hhkContent += "<!-- Sitemap 1.0 -->`r`n"
$hhkContent += "</HEAD><BODY>`r`n"
$hhkContent += "<UL>`r`n"

# Crear indice de palabras clave
$keywords = @(
    @{ Name = "Cliente"; Files = @("html\clientes\topic_40_crear_cliente.html", "html\clientes\topic_41_modificar_cliente.html") }
    @{ Name = "Producto"; Files = @("html\productos\topic_50_crear_producto.html", "html\productos\topic_51_modificar_producto.html") }
    @{ Name = "Pedido"; Files = @("html\pedidos\topic_80_crear_pedido.html", "html\pedidos\topic_81_modificar_pedido.html") }
    @{ Name = "Usuario"; Files = @("html\usuarios\topic_23_crear_usuario.html", "html\usuarios\topic_28_modificar_usuario.html") }
    @{ Name = "Backup"; Files = @("html\backup\topic_22_backup.html", "html\backup\topic_30_restore.html") }
    @{ Name = "Login"; Files = @("html\login\topic_31_login.html") }
    @{ Name = "Stock"; Files = @("html\stock\topic_70_mostrar_stock.html") }
    @{ Name = "Proveedor"; Files = @("html\proveedores\topic_60_crear_proveedor.html") }
)

foreach ($keyword in $keywords) {
    foreach ($file in $keyword.Files) {
        if (Test-Path "$sourceDir\$file") {
            $hhkContent += "`t<LI> <OBJECT type=`"text/sitemap`">`r`n"
            $hhkContent += "`t`t<param name=`"Name`" value=`"$($keyword.Name)`">`r`n"
            $hhkContent += "`t`t<param name=`"Local`" value=`"$file`">`r`n"
            $hhkContent += "`t`t</OBJECT>`r`n"
        }
    }
}

$hhkContent += "</UL>`r`n"
$hhkContent += "</BODY></HTML>`r`n"

$hhkFile = "$sourceDir\help_es.hhk"
$hhkContent | Out-File -FilePath $hhkFile -Encoding UTF8 -NoNewline
Write-Host "[OK] help_es.hhk creado" -ForegroundColor Green
Write-Host ""

# ============================================
# RESUMEN
# ============================================
Write-Host "=========================================="
Write-Host "  RESUMEN"
Write-Host "=========================================="
Write-Host ""
Write-Host "Archivos generados:" -ForegroundColor Cyan
Write-Host "  [OK] $hhpFile (con seccion MAP y ALIAS para F1)" -ForegroundColor Green
Write-Host "  [OK] $hhcFile" -ForegroundColor Green
Write-Host "  [OK] $hhkFile" -ForegroundColor Green
Write-Host ""
Write-Host "Archivos HTML incluidos: $($htmlFiles.Count)" -ForegroundColor Cyan
Write-Host ""
Write-Host "NOTA IMPORTANTE:" -ForegroundColor Yellow
Write-Host "El archivo .hhp incluye mapeo de IDs para ayuda contextual (F1)" -ForegroundColor White
Write-Host "IDs mapeados: 20, 22-33, 40-42, 50-53, 60-62, 70-71, 80-83, 90-91" -ForegroundColor White
Write-Host ""
Write-Host "=========================================="
Write-Host ""
Write-Host "SIGUIENTE PASO:" -ForegroundColor Yellow
Write-Host "Ejecutar: compilar_directo.bat" -ForegroundColor White
Write-Host ""
Write-Host "=========================================="
Write-Host ""

pause
