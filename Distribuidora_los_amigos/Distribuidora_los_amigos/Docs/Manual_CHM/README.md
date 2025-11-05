# ?? Manual de Usuario CHM - Distribuidora Los Amigos

## ?? Contenido de esta Carpeta

```
Manual_CHM/
??? ?? GUIA_CREACION_CHM.md          ? Guía completa paso a paso
??? ?? README.md                     ? Este archivo
??? ?? crear_estructura.ps1          ? Script PowerShell para crear carpetas
??? ?? generar_paginas.ps1           ? Script PowerShell para generar HTML
??? ?? compilar_chm.ps1              ? Script PowerShell para compilar
??? ?? compilar_chm.bat              ? Script Batch para compilar (avanzado)
??? ?? compilar_simple.bat           ? Script Batch simplificado ? RECOMENDADO
??? ?? diagnostico.bat               ? Verificar que todo esté listo
?
??? ?? source_es/                    ? Archivos fuente en español
?   ??? help_es.hhp                  ? Proyecto HTML Help
?   ??? help_es.hhc                  ? Tabla de contenidos
?   ??? help_es.hhk                  ? Índice de búsqueda
?   ??? template.html                ? Plantilla para nuevas páginas
?   ??? css/
?   ?   ??? styles.css               ? Estilos CSS
?   ??? html/
?       ??? general/
?       ?   ??? topic_20_main.html   ? Ejemplo: Página principal
?       ??? login/
?       ?   ??? topic_31_login.html  ? Ejemplo: Login
?       ??? clientes/
?       ?   ??? topic_40_crear_cliente.html ? Ejemplo: Crear cliente
?       ??? ... (otras carpetas)
?
??? ?? source_en/                    ? Archivos fuente en inglés (opcional)
    ??? ... (misma estructura)
```

---

## ?? Inicio Rápido (3 Pasos) - ACTUALIZADO

### **Paso 1: Instalar HTML Help Workshop**

1. Descargar de: https://www.microsoft.com/en-us/download/details.aspx?id=21138
2. Ejecutar el instalador
3. Ruta típica de instalación: `C:\Program Files (x86)\HTML Help Workshop\`

### **Paso 2: Crear la Estructura de Carpetas**

**? MÉTODO RECOMENDADO - Doble Click:**
```
1. Abrir la carpeta: Distribuidora_los_amigos\Docs\Manual_CHM\
2. Doble click en: diagnostico.bat
3. Verificar que todo esté OK
```

**Método Alternativo - PowerShell:**
```powershell
# Abrir PowerShell en la carpeta Manual_CHM
# Click derecho ? "Abrir en Terminal"

# Permitir ejecución de scripts (una sola vez)
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Crear estructura
.\crear_estructura.ps1

# Generar páginas HTML
.\generar_paginas.ps1
```

### **Paso 3: Compilar el CHM**

**? MÉTODO MÁS SIMPLE - Doble Click:**
```
1. Doble click en: compilar_simple.bat
2. Esperar a que termine
3. Presionar cualquier tecla para cerrar
```

**Método Alternativo - Desde CMD:**
```cmd
# Abrir CMD (Símbolo del sistema)
# Win + R ? escribir "cmd" ? Enter

# Ir a la carpeta
cd C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Docs\Manual_CHM

# Ejecutar
compilar_simple.bat
```

**Método Alternativo - PowerShell:**
```powershell
.\compilar_chm.ps1
```

**Método Alternativo - HTML Help Workshop (GUI):**
```
1. Abrir HTML Help Workshop
2. File ? Open ? Seleccionar help_es.hhp
3. Presionar F9 o click en "Compile"
```

---

## ??? Solución de Problemas

### **Problema 1: "echo." no se reconoce**

? **Solución:** Usar `compilar_simple.bat` en lugar de `compilar_chm.bat`

### **Problema 2: El script se ejecuta pero no hace nada**

? **Solución:**
```
1. Ejecutar: diagnostico.bat
2. Ver qué está faltando
3. Ejecutar los scripts de PowerShell primero:
   - crear_estructura.ps1
   - generar_paginas.ps1
```

### **Problema 3: "HTML Help Workshop no encontrado"**

? **Solución:**
```
1. Descargar: https://www.microsoft.com/en-us/download/details.aspx?id=21138
2. Instalar
3. Ejecutar de nuevo compilar_simple.bat
```

### **Problema 4: "Proyecto help_es.hhp no encontrado"**

? **Solución:**
```
Los archivos del proyecto están en una ubicación diferente.
Copiar toda la carpeta source_es a:
C:\DistribuidoraLosAmigos\Manual\
```

---

## ?? Crear Contenido

### **Método 1: Usar la Plantilla (Más Rápido)**

1. Copiar `source_es/template.html`
2. Renombrar según TopicID: `topic_XX_nombre.html`
3. Editar el contenido:
   - Reemplazar `[TÍTULO DE LA PÁGINA]`
   - Actualizar breadcrumb
   - Completar secciones
4. Guardar en la carpeta correspondiente

### **Método 2: Crear desde Cero**

Ver ejemplos completos en:
- `html/general/topic_20_main.html` - Pantalla principal
- `html/login/topic_31_login.html` - Login con seguridad
- `html/clientes/topic_40_crear_cliente.html` - Crear cliente

### **Lista de 32 Páginas a Crear**

? = Ya creadas como ejemplo
? = Pendientes de crear

#### **General (2 páginas)**
- ? topic_20_main.html - Pantalla Principal
- ? topic_21_intro.html - Introducción al Sistema

#### **Login y Seguridad (3 páginas)**
- ? topic_31_login.html - Iniciar Sesión
- ? topic_32_recuperar.html - Recuperar Contraseña
- ? topic_33_cambiar_pass.html - Cambiar Contraseña

#### **Gestión de Usuarios (4 páginas)**
- ? topic_23_crear_usuario.html - Crear Usuario
- ? topic_24_asignar_rol.html - Asignar Rol
- ? topic_28_modificar_usuario.html - Modificar Usuario
- ? topic_29_mostrar_usuarios.html - Mostrar Usuarios

#### **Roles y Permisos (2 páginas)**
- ? topic_26_crear_rol.html - Crear Rol
- ? topic_27_crear_patente.html - Crear Patente

#### **Backup y Restore (3 páginas)**
- ? topic_22_backup.html - Backup
- ? topic_25_bitacora.html - Bitácora
- ? topic_30_restore.html - Restore

#### **Clientes (3 páginas)**
- ? topic_40_crear_cliente.html - Crear Cliente
- ? topic_41_modificar_cliente.html - Modificar Cliente
- ? topic_42_mostrar_clientes.html - Consultar Clientes

#### **Productos (4 páginas)**
- ? topic_50_crear_producto.html
- ? topic_51_modificar_producto.html
- ? topic_52_mostrar_productos.html
- ? topic_53_eliminar_producto.html

#### **Proveedores (3 páginas)**
- ? topic_60_crear_proveedor.html
- ? topic_61_modificar_proveedor.html
- ? topic_62_mostrar_proveedores.html

#### **Stock (2 páginas)**
- ? topic_70_mostrar_stock.html
- ? topic_71_modificar_stock.html

#### **Pedidos (4 páginas)**
- ? topic_80_crear_pedido.html
- ? topic_81_modificar_pedido.html
- ? topic_82_mostrar_pedidos.html
- ? topic_83_detalle_pedido.html

#### **Reportes (2 páginas)**
- ? topic_90_reporte_stock_bajo.html
- ? topic_91_reporte_mas_vendidos.html

---

## ?? Personalización de Estilos

Editar: `source_es/css/styles.css`

### **Clases CSS Disponibles:**

```html
<!-- Cajas de información -->
<div class="info-box">?? Información</div>
<div class="warning">?? Advertencia</div>
<div class="error">? Error</div>
<div class="success">? Éxito</div>
<div class="tip">?? Consejo automático</div>

<!-- Pasos numerados -->
<div class="step">
    <span class="step-number">1</span>
    <strong>Título</strong>
    <p>Descripción...</p>
</div>

<!-- Tablas (automáticamente estilizadas) -->
<table>
    <thead><tr><th>Encabezado</th></tr></thead>
    <tbody><tr><td>Dato</td></tr></tbody>
</table>

<!-- Código -->
<code>código inline</code>
<pre><code>bloque de código</code></pre>

<!-- Utilidades -->
<span class="highlight">texto resaltado</span>
<span class="required">Campo obligatorio *</span>
```

---

## ?? Compilación

### **Opciones de Compilación:**

1. **Batch (Más Simple):**
   ```batch
   compilar_chm.bat
   ```

2. **PowerShell (Más Control):**
   ```powershell
   .\compilar_chm.ps1 -BaseDir "C:\Ruta\Personalizada"
   ```

3. **Manual desde HTML Help Workshop:**
   - Abrir `help_es.hhp`
   - Presionar F9 o click en "Compile"

### **Verificar Compilación Exitosa:**

```powershell
# Verificar que existen los archivos
Test-Path "C:\DistribuidoraLosAmigos\Manual\help_es.chm"
Test-Path "C:\DistribuidoraLosAmigos\Manual\help_us.chm"
Test-Path "C:\DistribuidoraLosAmigos\Manual\help_en.chm"

# Abrir el CHM para probarlo
& "C:\DistribuidoraLosAmigos\Manual\help_es.chm"
```

---

## ?? Probar el CHM

### **Prueba 1: Abrir Directamente**
```powershell
& "C:\DistribuidoraLosAmigos\Manual\help_es.chm"
```

### **Prueba 2: Abrir con TopicID Específico**
```powershell
hh.exe "ms-its:C:\DistribuidoraLosAmigos\Manual\help_es.chm::/html/login/topic_31_login.html"
```

### **Prueba 3: Desde la Aplicación**
1. Compilar y ejecutar Distribuidora_los_amigos
2. Abrir cualquier formulario
3. Presionar F1
4. Verificar que se abre la ayuda correcta

---

## ?? Multi-Idioma

### **Para crear versión en inglés:**

1. **Copiar estructura:**
   ```powershell
   Copy-Item -Recurse source_es source_en
   ```

2. **Renombrar archivos:**
   - `help_es.hhp` ? `help_en.hhp`
   - `help_es.hhc` ? `help_en.hhc`
   - `help_es.hhk` ? `help_en.hhk`

3. **Editar help_en.hhp:**
   ```ini
   Compiled file=..\help_us.chm
   Language=0x409 English (United States)
   Title=Distribution System Los Amigos - User Manual
   ```

4. **Traducir contenido HTML**

5. **Compilar:**
   ```powershell
   .\compilar_chm.ps1
   ```

---

## ?? Solución de Problemas

### **Problema: CHM se abre en blanco**

**Solución:**
```
1. Click derecho en el archivo .chm
2. Propiedades
3. En la parte inferior: "Desbloquear" ?
4. Aplicar ? Aceptar
```

### **Problema: "Cannot compile"**

**Soluciones:**
- Verificar que HTML Help Workshop esté instalado
- Mover el proyecto a una ruta más corta (ej: C:\CHM\)
- Verificar que los archivos HTML no tengan errores de sintaxis

### **Problema: Links rotos en el CHM**

**Causa:** Rutas relativas incorrectas

**Solución:**
```html
<!-- ? Correcto -->
<link rel="stylesheet" href="../../css/styles.css">
<a href="../general/topic_20_main.html">Inicio</a>

<!-- ? Incorrecto -->
<link rel="stylesheet" href="C:\...\styles.css">
```

### **Problema: El índice no aparece**

**Solución:**
- Verificar que `help_es.hhk` esté bien formado
- Abrir el .hhp y verificar: `Index file=help_es.hhk`

---

## ?? Checklist Final

Antes de distribuir los archivos CHM:

- [ ] Todas las 32 páginas HTML creadas
- [ ] CSS aplicado y funcionando
- [ ] Tabla de contenidos (.hhc) completa
- [ ] Índice de búsqueda (.hhk) completo
- [ ] CHM compilado sin errores
- [ ] Probado abriendo directamente
- [ ] Probado con TopicIDs específicos
- [ ] Probado desde la aplicación (F1)
- [ ] Links internos funcionando
- [ ] Imágenes cargando correctamente (si las hay)
- [ ] Búsqueda de texto completo funciona
- [ ] Versión en inglés creada (si aplica)
- [ ] Archivos copiados a `C:\DistribuidoraLosAmigos\Manual\`
- [ ] App.config apuntando a las rutas correctas

---

## ?? Recursos Adicionales

- **Guía Completa:** Ver `GUIA_CREACION_CHM.md`
- **Plantilla HTML:** `source_es/template.html`
- **Ejemplos:** Carpeta `source_es/html/`
- **HTML Help Workshop:** [Descargar](https://www.microsoft.com/en-us/download/details.aspx?id=21138)
- **Documentación CHM:** [Microsoft Docs](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/htmlhelp/microsoft-html-help-1-4-sdk)

---

## ? Resultado Final

Al completar todos los pasos, tendrás:

```
C:\DistribuidoraLosAmigos\Manual\
??? help_es.chm   ? Manual en español
??? help_us.chm   ? Manual en inglés (US)
??? help_en.chm   ? Manual en inglés (default)
```

Y podrás:
- ? Presionar F1 en cualquier formulario
- ? Obtener ayuda contextual automática
- ? Ayuda en el idioma del usuario
- ? Búsqueda de texto completo
- ? Navegación por índice y contenidos

---

**Fecha de Creación:** 2024
**Versión:** 1.0
**Estado:** ?? Listo para usar

