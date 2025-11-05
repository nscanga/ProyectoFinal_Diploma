# ???? GUÍA RÁPIDA - Crear Manual en INGLÉS

## ?? Resumen Ejecutivo

Este documento es una **guía paso a paso** para crear el manual de ayuda en **INGLÉS** basándose en el manual en **ESPAÑOL** ya existente.

**Tiempo estimado:** 30-45 minutos (estructura) + tiempo de traducción de contenido

---

## ? Proceso Rápido (5 Pasos)

### **PASO 1: Copiar Estructura Completa**

```powershell
# Ir a la carpeta Manual
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual"

# Copiar source_es a source_en
Copy-Item "source_es" -Destination "source_en" -Recurse -Force

Write-Host "? Estructura copiada exitosamente" -ForegroundColor Green
```

**Resultado:**
```
Manual\
??? source_es\          ? Original en español
??? source_en\          ? Copia para inglés (recién creada)
```

---

### **PASO 2: Renombrar Archivos del Proyecto**

```powershell
cd "source_en"

# Renombrar archivos principales
Rename-Item "help_es.hhp" "help_en.hhp"
Rename-Item "help_es.hhc" "help_en.hhc"
Rename-Item "help_es.hhk" "help_en.hhk"

Write-Host "? Archivos renombrados" -ForegroundColor Green
```

**Resultado:**
```
source_en\
??? help_en.hhp         ? Proyecto CHM
??? help_en.hhc         ? Tabla de contenidos
??? help_en.hhk         ? Índice
??? css\
?   ??? styles.css
??? html\
    ??? [todos los archivos HTML]
```

---

### **PASO 3: Editar help_en.hhp**

Abrir `source_en\help_en.hhp` y cambiar **SOLO** estas líneas:

#### Antes (español):
```ini
[OPTIONS]
Compiled file=..\help_es.chm
Contents file=help_es.hhc
Index file=help_es.hhk
Language=0x0c0a Espanol (Espana - alfabetizacion internacional)
Title=Manual de Usuario - Distribuidora Los Amigos
```

#### Después (inglés):
```ini
[OPTIONS]
Compiled file=..\help_en.chm
Contents file=help_en.hhc
Index file=help_en.hhk
Language=0x0409 English (United States)
Title=User Manual - Distribuidora Los Amigos
```

#### También cambiar en [WINDOWS]:
```ini
[WINDOWS]
main="User Manual - Distribuidora Los Amigos","help_en.hhc","help_en.hhk","html\general\topic_20_main.html","html\general\topic_20_main.html",,,,,0x63520,,0x387e,,,,,,,,0
```

**?? IMPORTANTE:**
- **NO CAMBIAR** las secciones `[MAP]`, `[ALIAS]` ni `[FILES]`
- Los IDs y nombres de archivos deben ser **idénticos** al español

---

### **PASO 4: Verificar Antes de Compilar**

```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\source_en"

# Verificar que el archivo .hhp existe
if (Test-Path "help_en.hhp") {
    Write-Host "? help_en.hhp encontrado" -ForegroundColor Green
} else {
    Write-Host "? help_en.hhp NO encontrado" -ForegroundColor Red
}

# Contar archivos HTML
$count = (Get-ChildItem "html" -Recurse -Filter "*.html").Count
Write-Host "?? Archivos HTML: $count (debe ser 32)" -ForegroundColor Cyan
```

---

### **PASO 5: Compilar el CHM en Inglés**

```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\source_en"

# Compilar
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_en.hhp"

# Verificar resultado
cd ".."
Get-Item "help_en.chm" | Select-Object Name, Length, LastWriteTime
```

**Salida esperada:**
```
Created c:\Mac\...\Manual\help_en.chm, 26,XXX bytes
```

---

## ?? Verificación Rápida

### Checklist de 1 Minuto:

```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual"

Write-Host "`n=== VERIFICACIÓN MANUAL INGLÉS ===" -ForegroundColor Cyan

# 1. Verificar archivos del proyecto
if (Test-Path "source_en\help_en.hhp") { Write-Host "? help_en.hhp" -ForegroundColor Green } else { Write-Host "? help_en.hhp" -ForegroundColor Red }
if (Test-Path "source_en\help_en.hhc") { Write-Host "? help_en.hhc" -ForegroundColor Green } else { Write-Host "? help_en.hhc" -ForegroundColor Red }
if (Test-Path "source_en\help_en.hhk") { Write-Host "? help_en.hhk" -ForegroundColor Green } else { Write-Host "? help_en.hhk" -ForegroundColor Red }

# 2. Verificar archivos HTML
$htmlCount = (Get-ChildItem "source_en\html" -Recurse -Filter "*.html").Count
if ($htmlCount -eq 32) { 
    Write-Host "? Archivos HTML: $htmlCount" -ForegroundColor Green 
} else { 
    Write-Host "?? Archivos HTML: $htmlCount (esperados: 32)" -ForegroundColor Yellow 
}

# 3. Verificar CHM compilado
if (Test-Path "help_en.chm") { 
    $chm = Get-Item "help_en.chm"
    Write-Host "? help_en.chm generado ($($chm.Length) bytes)" -ForegroundColor Green 
} else { 
    Write-Host "? help_en.chm NO generado" -ForegroundColor Red 
}

Write-Host "`n=== FIN VERIFICACIÓN ===" -ForegroundColor Cyan
```

---

## ?? Traducir Contenido HTML

**AHORA** es momento de traducir el contenido de los archivos HTML.

### Qué traducir:
- ? Títulos (`<h1>`, `<h2>`, etc.)
- ? Párrafos (`<p>`)
- ? Listas (`<li>`)
- ? Textos dentro de `<div>`, `<span>`, etc.
- ? Atributo `title` de las páginas

### Qué NO traducir:
- ? Nombres de archivos
- ? Rutas de archivos
- ? Nombres de clases CSS
- ? IDs de elementos HTML
- ? Estructura HTML

### Ejemplo de Traducción:

#### ESPAÑOL - topic_31_login.html:
```html
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Iniciar Sesión - Distribuidora Los Amigos</title>
    <link rel="stylesheet" href="../../css/styles.css">
</head>
<body>
    <h1>Iniciar Sesión</h1>
    
    <section id="introduccion">
        <h2>Introducción</h2>
        <p>
            Este formulario permite iniciar sesión en el sistema.
        </p>
    </section>
    
    <section id="campos">
        <h2>Descripción de Campos</h2>
        <ul>
            <li><strong>Usuario:</strong> Nombre de usuario registrado</li>
            <li><strong>Contraseña:</strong> Contraseña del usuario</li>
        </ul>
    </section>
</body>
</html>
```

#### INGLÉS - topic_31_login.html:
```html
<!DOCTYPE html>
<html lang="en">                                    ? Cambiar idioma
<head>
    <meta charset="UTF-8">
    <title>Login - Distribuidora Los Amigos</title>  ? Traducir título
    <link rel="stylesheet" href="../../css/styles.css">  ? NO cambiar
</head>
<body>
    <h1>Login</h1>                                  ? Traducir
    
    <section id="introduccion">                     ? NO cambiar ID
        <h2>Introduction</h2>                       ? Traducir
        <p>
            This form allows you to log in to the system.  ? Traducir
        </p>
    </section>
    
    <section id="campos">                           ? NO cambiar ID
        <h2>Field Description</h2>                  ? Traducir
        <ul>
            <li><strong>Username:</strong> Registered username</li>  ? Traducir
            <li><strong>Password:</strong> User password</li>        ? Traducir
        </ul>
    </section>
</body>
</html>
```

---

## ?? Lista de Archivos a Traducir

### Prioridad ALTA (traducir primero):
1. `html\general\topic_20_main.html` - Página principal
2. `html\login\topic_31_login.html` - Login
3. `html\login\topic_32_recuperar.html` - Recuperar contraseña
4. `html\usuarios\topic_23_crear_usuario.html` - Crear usuario
5. `html\clientes\topic_40_crear_cliente.html` - Crear cliente

### Prioridad MEDIA:
6. `html\productos\topic_50_crear_producto.html` - Crear producto
7. `html\pedidos\topic_80_crear_pedido.html` - Crear pedido
8. `html\usuarios\topic_29_mostrar_usuarios.html` - Mostrar usuarios
9. `html\clientes\topic_42_mostrar_clientes.html` - Mostrar clientes
10. `html\productos\topic_52_mostrar_productos.html` - Mostrar productos

### Prioridad BAJA:
11-32. Resto de archivos (backup, reportes, etc.)

---

## ?? Recompilar Después de Traducir

Cada vez que traduzcas un grupo de archivos (ej: 5-10 archivos), recompila para verificar:

```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\source_en"
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_en.hhp"
```

Luego prueba el CHM:
```powershell
cd ".."
.\help_en.chm
```

---

## ?? Diferencias entre Español e Inglés

### Archivos del Proyecto:

| Archivo | Español | Inglés |
|---------|---------|--------|
| Proyecto CHM | `help_es.hhp` | `help_en.hhp` |
| Contenidos | `help_es.hhc` | `help_en.hhc` |
| Índice | `help_es.hhk` | `help_en.hhk` |
| CHM compilado | `help_es.chm` | `help_en.chm` |
| Código de idioma | `0x0c0a` | `0x0409` |

### Archivos HTML:

| Tipo | Español | Inglés |
|------|---------|--------|
| Nombre de archivo | `topic_31_login.html` | `topic_31_login.html` ? **MISMO** |
| Ubicación | `html\login\` | `html\login\` ? **MISMA** |
| ID del topic | `31` | `31` ? **MISMO** |
| Contenido | Español | Inglés ? **DIFERENTE** |
| Atributo lang | `<html lang="es">` | `<html lang="en">` |

---

## ?? Errores Comunes y Soluciones

### Error 1: "File not found: help_en.hhp"

**Causa:** No se renombró correctamente

**Solución:**
```powershell
cd "Manual\source_en"
if (Test-Path "help_es.hhp") {
    Rename-Item "help_es.hhp" "help_en.hhp"
}
```

---

### Error 2: El CHM se genera pero está en español

**Causa:** No se tradujo el contenido de los archivos HTML

**Solución:** Abrir cada archivo HTML y traducir el contenido

---

### Error 3: Warnings "HHC3015" al compilar

**Causa:** Los alias apuntan a archivos incorrectos (igual que en español)

**Solución:** Verificar que los alias en `help_en.hhp` sean correctos:
```ini
[ALIAS]
TOPIC_RECUPERAR_PASSWORD=html\login\topic_32_recuperar.html
TOPIC_CAMBIAR_PASSWORD=html\login\topic_33_cambiar_pass.html
TOPIC_ASIGNAR_ROL=html\usuarios\topic_24_asignar_rol.html
TOPIC_BITACORA=html\backup\topic_25_bitacora.html
TOPIC_REPORTE_PRODUCTOS_MAS_VENDIDOS=html\reportes\topic_91_reporte_mas_vendidos.html
```

---

## ?? Script de Automatización (Opcional)

Para acelerar el proceso, puedes usar este script:

```powershell
# crear_manual_ingles.ps1

Write-Host "??????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "?  Crear Manual en Inglés Automático   ?" -ForegroundColor Cyan
Write-Host "??????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

$baseDir = "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual"

# Paso 1: Copiar estructura
Write-Host "?? Copiando estructura..." -ForegroundColor Yellow
Copy-Item "$baseDir\source_es" -Destination "$baseDir\source_en" -Recurse -Force
Write-Host "? Estructura copiada" -ForegroundColor Green

# Paso 2: Renombrar archivos
Write-Host "?? Renombrando archivos..." -ForegroundColor Yellow
cd "$baseDir\source_en"
Rename-Item "help_es.hhp" "help_en.hhp" -Force -ErrorAction SilentlyContinue
Rename-Item "help_es.hhc" "help_en.hhc" -Force -ErrorAction SilentlyContinue
Rename-Item "help_es.hhk" "help_en.hhk" -Force -ErrorAction SilentlyContinue
Write-Host "? Archivos renombrados" -ForegroundColor Green

# Paso 3: Editar help_en.hhp
Write-Host "?? Editando help_en.hhp..." -ForegroundColor Yellow
$hhpContent = Get-Content "help_en.hhp" -Encoding UTF8
$hhpContent = $hhpContent -replace "help_es\.chm", "help_en.chm"
$hhpContent = $hhpContent -replace "help_es\.hhc", "help_en.hhc"
$hhpContent = $hhpContent -replace "help_es\.hhk", "help_en.hhk"
$hhpContent = $hhpContent -replace "0x0c0a Espanol.*", "0x0409 English (United States)"
$hhpContent = $hhpContent -replace "Manual de Usuario", "User Manual"
$hhpContent | Out-File "help_en.hhp" -Encoding UTF8 -Force
Write-Host "? help_en.hhp editado" -ForegroundColor Green

# Paso 4: Compilar
Write-Host "?? Compilando CHM..." -ForegroundColor Yellow
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_en.hhp"
Write-Host "? CHM compilado" -ForegroundColor Green

# Verificar resultado
cd $baseDir
if (Test-Path "help_en.chm") {
    $chm = Get-Item "help_en.chm"
    Write-Host ""
    Write-Host "??????????????????????????????????????????" -ForegroundColor Green
    Write-Host "?          ? ÉXITO TOTAL               ?" -ForegroundColor Green
    Write-Host "??????????????????????????????????????????" -ForegroundColor Green
    Write-Host ""
    Write-Host "?? Archivo: help_en.chm" -ForegroundColor White
    Write-Host "?? Tamaño: $($chm.Length) bytes" -ForegroundColor White
    Write-Host "?? Fecha: $($chm.LastWriteTime)" -ForegroundColor White
    Write-Host ""
    Write-Host "?? SIGUIENTE PASO:" -ForegroundColor Yellow
    Write-Host "   Traducir el contenido de los archivos HTML en:" -ForegroundColor White
    Write-Host "   $baseDir\source_en\html\" -ForegroundColor Cyan
} else {
    Write-Host "? Error al generar help_en.chm" -ForegroundColor Red
}
```

Guardar como `crear_manual_ingles.ps1` y ejecutar:
```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual"
.\crear_manual_ingles.ps1
```

---

## ? Resumen Final

### Lo que se copia automáticamente:
- ? Toda la estructura de carpetas
- ? Todos los archivos HTML (con contenido en español)
- ? Archivos CSS
- ? Archivos del proyecto (.hhp, .hhc, .hhk)

### Lo que debes cambiar manualmente:
- ?? Renombrar `help_es.*` a `help_en.*`
- ?? Editar `help_en.hhp` (5 líneas)
- ?? Traducir contenido de 32 archivos HTML

### Lo que NUNCA debes cambiar:
- ? IDs en la sección [MAP]
- ? Nombres de archivos HTML
- ? Rutas de archivos
- ? Sección [ALIAS] (excepto si hubo correcciones)
- ? Sección [FILES]

---

**Creado:** 11 de Enero 2025  
**Versión:** 1.0  
**Tiempo Estimado:** 30-45 min (estructura) + traducción  
**Nivel:** Intermedio
