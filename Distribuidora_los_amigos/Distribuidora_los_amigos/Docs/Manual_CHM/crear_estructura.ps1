# ============================================
# Script de Creación de Estructura CHM
# Distribuidora Los Amigos
# ============================================

param(
    [string]$BaseDir = "C:\DistribuidoraLosAmigos\Manual"
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Creador de Estructura CHM" -ForegroundColor Cyan
Write-Host "  Distribuidora Los Amigos" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Función para crear directorios
function Create-Directory {
    param([string]$Path)
    if (-not (Test-Path $Path)) {
        New-Item -ItemType Directory -Force -Path $Path | Out-Null
        Write-Host "? Creado: $Path" -ForegroundColor Green
    } else {
        Write-Host "??  Ya existe: $Path" -ForegroundColor Yellow
    }
}

# Crear estructura base
Write-Host "?? Creando estructura de carpetas..." -ForegroundColor Cyan
Write-Host ""

# Español
Create-Directory "$BaseDir\source_es"
Create-Directory "$BaseDir\source_es\html"
Create-Directory "$BaseDir\source_es\html\general"
Create-Directory "$BaseDir\source_es\html\login"
Create-Directory "$BaseDir\source_es\html\usuarios"
Create-Directory "$BaseDir\source_es\html\roles"
Create-Directory "$BaseDir\source_es\html\backup"
Create-Directory "$BaseDir\source_es\html\clientes"
Create-Directory "$BaseDir\source_es\html\productos"
Create-Directory "$BaseDir\source_es\html\proveedores"
Create-Directory "$BaseDir\source_es\html\stock"
Create-Directory "$BaseDir\source_es\html\pedidos"
Create-Directory "$BaseDir\source_es\html\reportes"
Create-Directory "$BaseDir\source_es\css"
Create-Directory "$BaseDir\source_es\images"
Create-Directory "$BaseDir\source_es\images\screenshots"

# Inglés
Create-Directory "$BaseDir\source_en"
Create-Directory "$BaseDir\source_en\html"
Create-Directory "$BaseDir\source_en\html\general"
Create-Directory "$BaseDir\source_en\html\login"
Create-Directory "$BaseDir\source_en\html\usuarios"
Create-Directory "$BaseDir\source_en\html\roles"
Create-Directory "$BaseDir\source_en\html\backup"
Create-Directory "$BaseDir\source_en\html\clientes"
Create-Directory "$BaseDir\source_en\html\productos"
Create-Directory "$BaseDir\source_en\html\proveedores"
Create-Directory "$BaseDir\source_en\html\stock"
Create-Directory "$BaseDir\source_en\html\pedidos"
Create-Directory "$BaseDir\source_en\html\reportes"
Create-Directory "$BaseDir\source_en\css"
Create-Directory "$BaseDir\source_en\images"

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "?? Listado de Páginas HTML a Crear" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Definir todas las páginas HTML necesarias
$paginas = @{
    "General" = @(
        @{ID=20; Name="topic_20_main.html"; Title="Pantalla Principal"},
        @{ID=21; Name="topic_21_intro.html"; Title="Introducción al Sistema"}
    )
    "Login" = @(
        @{ID=31; Name="topic_31_login.html"; Title="Iniciar Sesión"},
        @{ID=32; Name="topic_32_recuperar.html"; Title="Recuperar Contraseña"},
        @{ID=33; Name="topic_33_cambiar_pass.html"; Title="Cambiar Contraseña"}
    )
    "Usuarios" = @(
        @{ID=23; Name="topic_23_crear_usuario.html"; Title="Crear Usuario"},
        @{ID=24; Name="topic_24_asignar_rol.html"; Title="Asignar Rol a Usuario"},
        @{ID=28; Name="topic_28_modificar_usuario.html"; Title="Modificar Usuario"},
        @{ID=29; Name="topic_29_mostrar_usuarios.html"; Title="Mostrar Usuarios"}
    )
    "Roles" = @(
        @{ID=26; Name="topic_26_crear_rol.html"; Title="Crear Rol (Familia)"},
        @{ID=27; Name="topic_27_crear_patente.html"; Title="Crear Patente"}
    )
    "Backup" = @(
        @{ID=22; Name="topic_22_backup.html"; Title="Realizar Backup"},
        @{ID=25; Name="topic_25_bitacora.html"; Title="Bitácora del Sistema"},
        @{ID=30; Name="topic_30_restore.html"; Title="Restaurar Base de Datos"}
    )
    "Clientes" = @(
        @{ID=40; Name="topic_40_crear_cliente.html"; Title="Crear Cliente"},
        @{ID=41; Name="topic_41_modificar_cliente.html"; Title="Modificar Cliente"},
        @{ID=42; Name="topic_42_mostrar_clientes.html"; Title="Consultar Clientes"}
    )
    "Productos" = @(
        @{ID=50; Name="topic_50_crear_producto.html"; Title="Crear Producto"},
        @{ID=51; Name="topic_51_modificar_producto.html"; Title="Modificar Producto"},
        @{ID=52; Name="topic_52_mostrar_productos.html"; Title="Consultar Productos"},
        @{ID=53; Name="topic_53_eliminar_producto.html"; Title="Eliminar Producto"}
    )
    "Proveedores" = @(
        @{ID=60; Name="topic_60_crear_proveedor.html"; Title="Crear Proveedor"},
        @{ID=61; Name="topic_61_modificar_proveedor.html"; Title="Modificar Proveedor"},
        @{ID=62; Name="topic_62_mostrar_proveedores.html"; Title="Consultar Proveedores"}
    )
    "Stock" = @(
        @{ID=70; Name="topic_70_mostrar_stock.html"; Title="Consultar Stock"},
        @{ID=71; Name="topic_71_modificar_stock.html"; Title="Modificar Stock"}
    )
    "Pedidos" = @(
        @{ID=80; Name="topic_80_crear_pedido.html"; Title="Crear Pedido"},
        @{ID=81; Name="topic_81_modificar_pedido.html"; Title="Modificar Pedido"},
        @{ID=82; Name="topic_82_mostrar_pedidos.html"; Title="Consultar Pedidos"},
        @{ID=83; Name="topic_83_detalle_pedido.html"; Title="Detalle del Pedido"}
    )
    "Reportes" = @(
        @{ID=90; Name="topic_90_reporte_stock_bajo.html"; Title="Reporte Stock Bajo"},
        @{ID=91; Name="topic_91_reporte_mas_vendidos.html"; Title="Productos Más Vendidos"}
    )
}

$totalPaginas = 0
foreach ($categoria in $paginas.Keys) {
    Write-Host "?? $categoria" -ForegroundColor Yellow
    foreach ($pagina in $paginas[$categoria]) {
        Write-Host "   ?? Topic $($pagina.ID): $($pagina.Name) - $($pagina.Title)" -ForegroundColor Gray
        $totalPaginas++
    }
    Write-Host ""
}

Write-Host "Total de páginas a crear: $totalPaginas" -ForegroundColor Cyan
Write-Host ""

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "? Estructura creada exitosamente" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? Ubicación: $BaseDir" -ForegroundColor White
Write-Host ""
Write-Host "?? Próximos pasos:" -ForegroundColor Cyan
Write-Host "  1. Copiar los archivos .hhp, .hhc, .hhk a la carpeta source_es" -ForegroundColor White
Write-Host "  2. Copiar el archivo styles.css a source_es\css\" -ForegroundColor White
Write-Host "  3. Crear las páginas HTML usando la plantilla template.html" -ForegroundColor White
Write-Host "  4. Abrir el archivo .hhp en HTML Help Workshop" -ForegroundColor White
Write-Host "  5. Compilar el CHM" -ForegroundColor White
Write-Host ""

# Crear archivo de referencia
$referenceFile = "$BaseDir\PAGINAS_A_CREAR.txt"
"============================================" | Out-File $referenceFile
"PÁGINAS HTML A CREAR" | Out-File $referenceFile -Append
"Distribuidora Los Amigos" | Out-File $referenceFile -Append
"============================================" | Out-File $referenceFile -Append
"" | Out-File $referenceFile -Append

foreach ($categoria in $paginas.Keys) {
    "[$categoria]" | Out-File $referenceFile -Append
    foreach ($pagina in $paginas[$categoria]) {
        "  - Topic $($pagina.ID): $($pagina.Name) - $($pagina.Title)" | Out-File $referenceFile -Append
    }
    "" | Out-File $referenceFile -Append
}

"Total: $totalPaginas páginas" | Out-File $referenceFile -Append

Write-Host "?? Archivo de referencia creado: $referenceFile" -ForegroundColor Green
Write-Host ""
Write-Host "¡Proceso completado!" -ForegroundColor Green
