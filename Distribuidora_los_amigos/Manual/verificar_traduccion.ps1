# ============================================
# Script: Verificar Progreso de Traducción
# ============================================

Write-Host ""
Write-Host "??????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "?                                                                ?" -ForegroundColor Cyan
Write-Host "?       VERIFICADOR DE PROGRESO - MANUAL EN INGLÉS              ?" -ForegroundColor Cyan
Write-Host "?       Distribuidora Los Amigos                                 ?" -ForegroundColor Cyan
Write-Host "?                                                                ?" -ForegroundColor Cyan
Write-Host "??????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

# Verificar que estamos en el directorio correcto
if (-not (Test-Path "source_en") -or -not (Test-Path "source_es")) {
    Write-Host "? Error: Este script debe ejecutarse desde la carpeta 'Manual'" -ForegroundColor Red
    Write-Host "   Asegúrate de estar en:" -ForegroundColor Yellow
    Write-Host "   C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual" -ForegroundColor White
    exit 1
}

# ============================================
# Función para comparar archivos
# ============================================
function Test-FileTranslated {
    param(
        [string]$relativePath
    )
    
    $esFile = Join-Path "source_es" $relativePath
    $enFile = Join-Path "source_en" $relativePath
    
    if (-not (Test-Path $esFile) -or -not (Test-Path $enFile)) {
        return $false
    }
    
    $esContent = Get-Content $esFile -Raw -Encoding UTF8
    $enContent = Get-Content $enFile -Raw -Encoding UTF8
    
    # Si el contenido es diferente, consideramos que está traducido
    # Buscamos palabras clave en español que NO deberían estar en inglés
    $spanishKeywords = @(
        "Introducci[oó]n",
        "C[oó]mo Acceder",
        "Descripci[oó]n de Campos",
        "Procedimiento",
        "Validaciones del Sistema",
        "Consejos y Recomendaciones",
        "Errores Comunes",
        "Permisos Necesarios",
        "Ver Tambi[eé]n"
    )
    
    $hasSpanish = $false
    foreach ($keyword in $spanishKeywords) {
        if ($enContent -match $keyword) {
            $hasSpanish = $true
            break
        }
    }
    
    return -not $hasSpanish
}

# ============================================
# Lista de archivos a verificar
# ============================================
$archivos = @(
    @{Categoria="General"; Path="html\general\topic_20_main.html"; Nombre="Main Page / Home"; Prioridad="Alta"},
    @{Categoria="General"; Path="html\general\topic_21_intro.html"; Nombre="Introduction"; Prioridad="Media"},
    
    @{Categoria="Login"; Path="html\login\topic_31_login.html"; Nombre="Login"; Prioridad="Alta"},
    @{Categoria="Login"; Path="html\login\topic_32_recuperar.html"; Nombre="Password Recovery"; Prioridad="Alta"},
    @{Categoria="Login"; Path="html\login\topic_33_cambiar_pass.html"; Nombre="Change Password"; Prioridad="Media"},
    
    @{Categoria="Usuarios"; Path="html\usuarios\topic_23_crear_usuario.html"; Nombre="Create User"; Prioridad="Alta"},
    @{Categoria="Usuarios"; Path="html\usuarios\topic_24_asignar_rol.html"; Nombre="Assign Role"; Prioridad="Media"},
    @{Categoria="Usuarios"; Path="html\usuarios\topic_28_modificar_usuario.html"; Nombre="Modify User"; Prioridad="Media"},
    @{Categoria="Usuarios"; Path="html\usuarios\topic_29_mostrar_usuarios.html"; Nombre="Show Users"; Prioridad="Media"},
    
    @{Categoria="Clientes"; Path="html\clientes\topic_40_crear_cliente.html"; Nombre="Create Customer"; Prioridad="Alta"},
    @{Categoria="Clientes"; Path="html\clientes\topic_41_modificar_cliente.html"; Nombre="Modify Customer"; Prioridad="Media"},
    @{Categoria="Clientes"; Path="html\clientes\topic_42_mostrar_clientes.html"; Nombre="Show Customers"; Prioridad="Media"},
    
    @{Categoria="Productos"; Path="html\productos\topic_50_crear_producto.html"; Nombre="Create Product"; Prioridad="Alta"},
    @{Categoria="Productos"; Path="html\productos\topic_51_modificar_producto.html"; Nombre="Modify Product"; Prioridad="Media"},
    @{Categoria="Productos"; Path="html\productos\topic_52_mostrar_productos.html"; Nombre="Show Products"; Prioridad="Media"},
    @{Categoria="Productos"; Path="html\productos\topic_53_eliminar_producto.html"; Nombre="Delete Product"; Prioridad="Baja"},
    
    @{Categoria="Proveedores"; Path="html\proveedores\topic_60_crear_proveedor.html"; Nombre="Create Supplier"; Prioridad="Media"},
    @{Categoria="Proveedores"; Path="html\proveedores\topic_61_modificar_proveedor.html"; Nombre="Modify Supplier"; Prioridad="Baja"},
    @{Categoria="Proveedores"; Path="html\proveedores\topic_62_mostrar_proveedores.html"; Nombre="Show Suppliers"; Prioridad="Baja"},
    
    @{Categoria="Stock"; Path="html\stock\topic_70_mostrar_stock.html"; Nombre="Show Stock"; Prioridad="Media"},
    @{Categoria="Stock"; Path="html\stock\topic_71_modificar_stock.html"; Nombre="Modify Stock"; Prioridad="Media"},
    
    @{Categoria="Pedidos"; Path="html\pedidos\topic_80_crear_pedido.html"; Nombre="Create Order"; Prioridad="Alta"},
    @{Categoria="Pedidos"; Path="html\pedidos\topic_81_modificar_pedido.html"; Nombre="Modify Order"; Prioridad="Media"},
    @{Categoria="Pedidos"; Path="html\pedidos\topic_82_mostrar_pedidos.html"; Nombre="Show Orders"; Prioridad="Media"},
    @{Categoria="Pedidos"; Path="html\pedidos\topic_83_detalle_pedido.html"; Nombre="Order Details"; Prioridad="Media"},
    
    @{Categoria="Reportes"; Path="html\reportes\topic_90_reporte_stock_bajo.html"; Nombre="Low Stock Report"; Prioridad="Baja"},
    @{Categoria="Reportes"; Path="html\reportes\topic_91_reporte_mas_vendidos.html"; Nombre="Best Sellers Report"; Prioridad="Baja"},
    
    @{Categoria="Roles"; Path="html\roles\topic_26_crear_rol.html"; Nombre="Create Role"; Prioridad="Media"},
    @{Categoria="Roles"; Path="html\roles\topic_27_crear_patente.html"; Nombre="Create Permission"; Prioridad="Baja"},
    
    @{Categoria="Backup"; Path="html\backup\topic_22_backup.html"; Nombre="Backup"; Prioridad="Baja"},
    @{Categoria="Backup"; Path="html\backup\topic_25_bitacora.html"; Nombre="Event Log"; Prioridad="Baja"},
    @{Categoria="Backup"; Path="html\backup\topic_30_restore.html"; Nombre="Restore"; Prioridad="Baja"}
)

# ============================================
# Verificar cada archivo
# ============================================
$traducidos = 0
$pendientes = 0
$categoriaActual = ""

Write-Host "Verificando archivos..." -ForegroundColor Yellow
Write-Host ""

foreach ($archivo in $archivos) {
    # Mostrar encabezado de categoría
    if ($archivo.Categoria -ne $categoriaActual) {
        Write-Host ""
        Write-Host "??? $($archivo.Categoria) ???" -ForegroundColor Cyan
        $categoriaActual = $archivo.Categoria
    }
    
    $traducido = Test-FileTranslated -relativePath $archivo.Path
    
    if ($traducido) {
        Write-Host "  ? " -NoNewline -ForegroundColor Green
        Write-Host "$($archivo.Nombre) " -NoNewline -ForegroundColor White
        Write-Host "[$($archivo.Prioridad)]" -ForegroundColor Gray
        $traducidos++
    } else {
        Write-Host "  ? " -NoNewline -ForegroundColor Yellow
        Write-Host "$($archivo.Nombre) " -NoNewline -ForegroundColor White
        Write-Host "[$($archivo.Prioridad)]" -ForegroundColor Gray
        $pendientes++
    }
}

# ============================================
# Resumen
# ============================================
Write-Host ""
Write-Host "????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "  RESUMEN" -ForegroundColor Cyan
Write-Host "????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

$total = $archivos.Count
$porcentaje = [math]::Round(($traducidos / $total) * 100, 1)

Write-Host "?? Progreso: " -NoNewline -ForegroundColor White
Write-Host "$traducidos / $total archivos traducidos ($porcentaje%)" -ForegroundColor Cyan
Write-Host ""

# Barra de progreso
$barraLongitud = 50
$barraCompletada = [math]::Floor(($traducidos / $total) * $barraLongitud)
$barraRestante = $barraLongitud - $barraCompletada

Write-Host "   [" -NoNewline
Write-Host ("?" * $barraCompletada) -NoNewline -ForegroundColor Green
Write-Host ("?" * $barraRestante) -NoNewline -ForegroundColor DarkGray
Write-Host "]"
Write-Host ""

# Archivos por prioridad
$altaTraducidos = ($archivos | Where-Object { $_.Prioridad -eq "Alta" -and (Test-FileTranslated -relativePath $_.Path) }).Count
$altaTotal = ($archivos | Where-Object { $_.Prioridad -eq "Alta" }).Count

$mediaTraducidos = ($archivos | Where-Object { $_.Prioridad -eq "Media" -and (Test-FileTranslated -relativePath $_.Path) }).Count
$mediaTotal = ($archivos | Where-Object { $_.Prioridad -eq "Media" }).Count

$bajaTraducidos = ($archivos | Where-Object { $_.Prioridad -eq "Baja" -and (Test-FileTranslated -relativePath $_.Path) }).Count
$bajaTotal = ($archivos | Where-Object { $_.Prioridad -eq "Baja" }).Count

Write-Host "Desglose por prioridad:" -ForegroundColor White
Write-Host "   ?? Alta:   $altaTraducidos / $altaTotal" -ForegroundColor Red
Write-Host "   ?? Media:  $mediaTraducidos / $mediaTotal" -ForegroundColor Yellow
Write-Host "   ?? Baja:   $bajaTraducidos / $bajaTotal" -ForegroundColor Green
Write-Host ""

# ============================================
# Próximos archivos a traducir
# ============================================
Write-Host "????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "  PRÓXIMOS ARCHIVOS RECOMENDADOS (Prioridad Alta)" -ForegroundColor Cyan
Write-Host "????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

$proximosAlta = $archivos | Where-Object { 
    $_.Prioridad -eq "Alta" -and -not (Test-FileTranslated -relativePath $_.Path)
} | Select-Object -First 5

if ($proximosAlta.Count -gt 0) {
    foreach ($archivo in $proximosAlta) {
        Write-Host "   ?? $($archivo.Nombre)" -ForegroundColor Yellow
        Write-Host "      Archivo: source_en\$($archivo.Path)" -ForegroundColor Gray
        Write-Host ""
    }
} else {
    Write-Host "   ? ¡Todos los archivos de alta prioridad están traducidos!" -ForegroundColor Green
    Write-Host ""
}

# ============================================
# Comandos útiles
# ============================================
Write-Host "????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "  COMANDOS ÚTILES" -ForegroundColor Cyan
Write-Host "????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

Write-Host "Para recompilar el manual en inglés:" -ForegroundColor White
Write-Host '   cd source_en' -ForegroundColor Gray
Write-Host '   & "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_en.hhp"' -ForegroundColor Gray
Write-Host ""

Write-Host "Para abrir el manual compilado:" -ForegroundColor White
Write-Host '   .\help_en.chm' -ForegroundColor Gray
Write-Host ""

Write-Host "Para ejecutar este script nuevamente:" -ForegroundColor White
Write-Host '   .\verificar_traduccion.ps1' -ForegroundColor Gray
Write-Host ""

Write-Host "????????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
