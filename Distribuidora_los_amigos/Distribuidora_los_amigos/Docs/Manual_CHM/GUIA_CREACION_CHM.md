# ?? Guía Completa para Crear Archivos CHM con HTML Help Workshop

## ?? Objetivo
Crear archivos CHM (Compiled HTML Help) multi-idioma para el sistema de ayuda contextual.

---

## ?? Requisitos Previos

### **Software Necesario:**
1. ? **HTML Help Workshop** (instalado)
   - Descargar de: https://www.microsoft.com/en-us/download/details.aspx?id=21138
   - Si ya está instalado: `C:\Program Files (x86)\HTML Help Workshop\hhw.exe`

### **Archivos Necesarios:**
- ? Proyecto HTML Help (.hhp)
- ? Tabla de contenidos (.hhc)
- ? Índice (.hhk)
- ? Archivos HTML con el contenido
- ? CSS para estilos (opcional)
- ? Imágenes (opcional)

---

## ?? Estructura de Directorios

```
C:\DistribuidoraLosAmigos\Manual\
??? source_es\                      ? Archivos fuente en español
?   ??? help_es.hhp                 ? Proyecto principal
?   ??? help_es.hhc                 ? Tabla de contenidos
?   ??? help_es.hhk                 ? Índice
?   ??? html\                       ? Páginas HTML
?   ?   ??? general\
?   ?   ?   ??? topic_20_main.html
?   ?   ?   ??? topic_21_intro.html
?   ?   ??? login\
?   ?   ?   ??? topic_31_login.html
?   ?   ?   ??? topic_32_recuperar.html
?   ?   ?   ??? topic_33_cambiar_pass.html
?   ?   ??? usuarios\
?   ?   ?   ??? topic_23_crear.html
?   ?   ?   ??? topic_28_modificar.html
?   ?   ?   ??? topic_29_mostrar.html
?   ?   ??? roles\
?   ?   ??? clientes\
?   ?   ??? productos\
?   ?   ??? proveedores\
?   ?   ??? stock\
?   ?   ??? pedidos\
?   ?   ??? reportes\
?   ??? css\
?   ?   ??? styles.css
?   ??? images\
?       ??? screenshots\
?
??? source_en\                      ? Archivos fuente en inglés
?   ??? ... (misma estructura)
?
??? source_pt\                      ? Archivos fuente en portugués
?   ??? ... (misma estructura)
?
??? help_es.chm                     ? Archivo compilado español ?
??? help_us.chm                     ? Archivo compilado inglés ?
??? help_en.chm                     ? Archivo compilado inglés default ?
```

---

## ??? Paso a Paso: Crear el Proyecto CHM

### **Paso 1: Crear la Estructura de Carpetas**

Ejecutar este script en PowerShell:

```powershell
# Crear estructura base
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\general"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\login"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\usuarios"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\roles"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\backup"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\clientes"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\productos"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\proveedores"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\stock"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\pedidos"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\html\reportes"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\css"
New-Item -ItemType Directory -Force -Path "C:\DistribuidoraLosAmigos\Manual\source_es\images"

Write-Host "? Estructura de carpetas creada exitosamente"
```

### **Paso 2: Abrir HTML Help Workshop**

1. Ejecutar: `C:\Program Files (x86)\HTML Help Workshop\hhw.exe`
2. Ir a **File** > **New**
3. Seleccionar **Project**
4. Click en **Next**

### **Paso 3: Configurar el Nuevo Proyecto**

1. **Destination:**
   - Seleccionar: `C:\DistribuidoraLosAmigos\Manual\source_es\help_es.hhp`
   
2. **HTML Files:**
   - Click en **Add** para agregar archivos HTML existentes (más tarde)
   
3. **Click Finish**

### **Paso 4: Configurar Opciones del Proyecto**

1. En HTML Help Workshop, ir a **Project** > **Options**

2. **Pestaña "General":**
   ```
   Title: Sistema Distribuidora Los Amigos - Manual de Usuario
   Default file: html\general\topic_20_main.html
   Language: Spanish (Spain)
   ```

3. **Pestaña "Files":**
   - Click **Add** para agregar todos los archivos HTML

4. **Pestaña "Compiler":**
   ```
   Compiled file: ..\help_es.chm
   Full-text search: ? Enabled
   Contents file: help_es.hhc
   Index file: help_es.hhk
   Default Window: main
   ```

---

## ?? Archivos de Configuración Generados

Los archivos ya están creados en:
- `Distribuidora_los_amigos/Docs/Manual_CHM/source_es/`

### **Archivos Principales:**

1. ? **help_es.hhp** - Proyecto principal
2. ? **help_es.hhc** - Tabla de contenidos
3. ? **help_es.hhk** - Índice de búsqueda
4. ? **template.html** - Plantilla para nuevas páginas
5. ? **styles.css** - Estilos CSS

---

## ?? Crear Páginas de Contenido

### **Opción 1: Usar la Plantilla (Recomendado)**

1. Copiar `template.html` a la carpeta correspondiente
2. Renombrar según TopicID: `topic_XX_nombre.html`
3. Editar el contenido:

```html
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Título de la Página</title>
    <link rel="stylesheet" href="../../css/styles.css">
</head>
<body>
    <h1>Título Principal</h1>
    
    <section id="introduccion">
        <h2>Introducción</h2>
        <p>Descripción del módulo...</p>
    </section>
    
    <section id="acceso">
        <h2>Cómo Acceder</h2>
        <ol>
            <li>Paso 1...</li>
            <li>Paso 2...</li>
        </ol>
    </section>
    
    <section id="campos">
        <h2>Descripción de Campos</h2>
        <table>
            <tr>
                <th>Campo</th>
                <th>Descripción</th>
                <th>Obligatorio</th>
            </tr>
            <tr>
                <td>Nombre</td>
                <td>Descripción del campo</td>
                <td>Sí</td>
            </tr>
        </table>
    </section>
    
    <section id="errores">
        <h2>Errores Comunes</h2>
        <div class="warning">
            <strong>?? Advertencia:</strong> Mensaje...
        </div>
    </section>
    
    <footer>
        <p>© 2024 Distribuidora Los Amigos - Todos los derechos reservados</p>
    </footer>
</body>
</html>
```

### **Opción 2: Generar Automáticamente**

Usa el script PowerShell incluido para generar todas las páginas base.

---

## ?? Compilar el CHM

### **Método 1: Desde HTML Help Workshop (GUI)**

1. Abrir el archivo `.hhp` en HTML Help Workshop
2. Click en el botón **?? Save All** (Ctrl+Shift+S)
3. Click en el botón **?? Compile** (o presionar F9)
4. Esperar a que termine la compilación
5. El archivo `help_es.chm` se generará en `C:\DistribuidoraLosAmigos\Manual\`

### **Método 2: Línea de Comandos (Automatizado)**

```batch
@echo off
REM Compilar CHM Español
"C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "C:\DistribuidoraLosAmigos\Manual\source_es\help_es.hhp"

REM Compilar CHM Inglés
"C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "C:\DistribuidoraLosAmigos\Manual\source_en\help_en.hhp"

REM Compilar CHM Portugués
"C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "C:\DistribuidoraLosAmigos\Manual\source_pt\help_pt.hhp"

echo ? Compilación completada
pause
```

Guardar como: `compilar_todos.bat`

---

## ?? Probar el CHM

### **1. Probar el Archivo Compilado:**

```powershell
# Abrir el CHM directamente
& "C:\DistribuidoraLosAmigos\Manual\help_es.chm"

# O con un TopicID específico
hh.exe "ms-its:C:\DistribuidoraLosAmigos\Manual\help_es.chm::/html/usuarios/topic_23_crear.html"
```

### **2. Probar desde la Aplicación:**

1. Compilar y ejecutar Distribuidora_los_amigos
2. Abrir cualquier formulario que tenga F1 implementado
3. Presionar **F1**
4. Verificar que se abre la ayuda correcta

---

## ?? Crear Versiones Multi-Idioma

### **Para crear help_us.chm (Inglés):**

1. Copiar toda la carpeta `source_es` a `source_en`
2. Renombrar archivos:
   - `help_es.hhp` ? `help_en.hhp`
   - `help_es.hhc` ? `help_en.hhc`
   - `help_es.hhk` ? `help_en.hhk`
3. Editar `help_en.hhp`:
   ```
   Compiled file=..\help_us.chm
   Language=0x409 English (United States)
   Title=Distribution System Los Amigos - User Manual
   ```
4. Traducir todos los archivos HTML al inglés
5. Compilar

### **Para crear help_en.chm (Default):**

Copiar `help_us.chm` a `help_en.chm`

---

## ?? Solución de Problemas

### **Problema 1: "Cannot compile CHM"**

**Causa:** Rutas muy largas o caracteres especiales

**Solución:**
```
1. Mover el proyecto a C:\CHM_Temp\
2. Compilar
3. Copiar el .chm resultante a C:\DistribuidoraLosAmigos\Manual\
```

### **Problema 2: "El CHM se abre pero está en blanco"**

**Causa:** Windows bloqueó el archivo

**Solución:**
```
1. Click derecho en el archivo .chm
2. Propiedades
3. En la parte inferior: "Desbloquear" ?
4. Aplicar y Aceptar
```

### **Problema 3: "No encuentra los archivos HTML"**

**Causa:** Rutas relativas incorrectas

**Solución:**
Verificar en el .hhp que las rutas sean relativas:
```
html\general\topic_20_main.html    ? Correcto
C:\...\topic_20_main.html          ? Incorrecto
```

### **Problema 4: "La tabla de contenidos no aparece"**

**Causa:** Archivo .hhc mal formado

**Solución:**
Usar el archivo `help_es.hhc` generado como plantilla.

---

## ?? Lista de Páginas HTML a Crear

Según el mapeo de TopicIDs:

### **General (1 página)**
- ? topic_20_main.html - Pantalla Principal

### **Login y Seguridad (3 páginas)**
- ? topic_31_login.html
- ? topic_32_recuperar.html
- ? topic_33_cambiar_pass.html

### **Gestión de Usuarios (4 páginas)**
- ? topic_23_crear_usuario.html
- ? topic_24_asignar_rol.html
- ? topic_28_modificar_usuario.html
- ? topic_29_mostrar_usuarios.html

### **Roles y Permisos (2 páginas)**
- ? topic_26_crear_rol.html
- ? topic_27_crear_patente.html

### **Backup y Restore (2 páginas)**
- ? topic_22_backup.html
- ? topic_30_restore.html

### **Bitácora (1 página)**
- ? topic_25_bitacora.html

### **Clientes (3 páginas)**
- ? topic_40_crear_cliente.html
- ? topic_41_modificar_cliente.html
- ? topic_42_mostrar_clientes.html

### **Productos (4 páginas)**
- ? topic_50_crear_producto.html
- ? topic_51_modificar_producto.html
- ? topic_52_mostrar_productos.html
- ? topic_53_eliminar_producto.html

### **Proveedores (3 páginas)**
- ? topic_60_crear_proveedor.html
- ? topic_61_modificar_proveedor.html
- ? topic_62_mostrar_proveedores.html

### **Stock (2 páginas)**
- ? topic_70_mostrar_stock.html
- ? topic_71_modificar_stock.html

### **Pedidos (4 páginas)**
- ? topic_80_crear_pedido.html
- ? topic_81_modificar_pedido.html
- ? topic_82_mostrar_pedidos.html
- ? topic_83_detalle_pedido.html

### **Reportes (2 páginas)**
- ? topic_90_reporte_stock_bajo.html
- ? topic_91_reporte_mas_vendidos.html

**Total: 32 páginas HTML**

---

## ?? Workflow Recomendado

1. ? **Día 1-2:** Crear estructura de carpetas y archivos base
2. ? **Día 3-5:** Escribir contenido HTML para español (32 páginas)
3. ? **Día 6:** Compilar y probar help_es.chm
4. ? **Día 7-9:** Traducir al inglés (32 páginas)
5. ? **Día 10:** Compilar help_us.chm y help_en.chm
6. ? **Día 11:** Traducir al portugués (opcional)
7. ? **Día 12:** Testing final y ajustes

---

## ? Checklist Final

- [ ] HTML Help Workshop instalado
- [ ] Estructura de carpetas creada
- [ ] Archivos .hhp, .hhc, .hhk configurados
- [ ] CSS y plantilla HTML listos
- [ ] 32 páginas HTML creadas (español)
- [ ] CHM compilado exitosamente (help_es.chm)
- [ ] Contenido traducido al inglés
- [ ] CHM compilado (help_us.chm y help_en.chm)
- [ ] Archivos CHM copiados a C:\DistribuidoraLosAmigos\Manual\
- [ ] Probado F1 desde la aplicación
- [ ] Probado en los 3 idiomas

---

## ?? Resultado Final

Al terminar, tendrás:

```
C:\DistribuidoraLosAmigos\Manual\
??? help_es.chm   ? (Español)
??? help_us.chm   ? (Inglés US)
??? help_en.chm   ? (Inglés Default)
```

Y podrás presionar **F1** en cualquier formulario para obtener ayuda contextual en el idioma del usuario.

---

**Fecha de Creación:** ${new Date().toLocaleDateString('es-ES')}
**Estado:** ?? Guía Completa Lista

