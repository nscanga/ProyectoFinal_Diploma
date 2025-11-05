# ?? Guía Rápida de Compilación CHM

## ?? Ubicación de Archivos

### **Estructura Actual:**
```
Distribuidora_los_amigos/
??? Docs/
    ??? Manual_CHM/
        ??? source_es/                    ? Archivos fuente del manual
        ?   ??? help_es.hhp              ? Proyecto CHM (CON MAP Y ALIAS)
        ?   ??? help_es.hhc              ? Tabla de contenidos
        ?   ??? help_es.hhk              ? Índice
        ?   ??? template.html            ? Plantilla para nuevas páginas
        ?   ??? css/
        ?   ?   ??? styles.css           ? Estilos CSS
        ?   ??? html/                    ? Páginas HTML
        ?       ??? general/
        ?       ?   ??? topic_20_main.html
        ?       ??? login/
        ?       ?   ??? topic_31_login.html
        ?       ?   ??? topic_32_recuperar.html  ? FALTA CREAR CONTENIDO
        ?       ??? clientes/
        ?       ??? productos/
        ?       ??? proveedores/
        ?       ??? stock/
        ?       ??? pedidos/
        ?
        ??? help_es.chm                  ? CHM generado (compilado)
        ?
        ??? Scripts de compilación:
            ??? compilar_simple.bat      ? MÁS SIMPLE
            ??? compilar_directo.bat
            ??? compilar_universal.bat
```

### **Ubicación donde la App busca el CHM:**
```
C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\help_es.chm
```

---

## ? Método Más Rápido (RECOMENDADO)

### **Opción 1: Compilar desde el workspace**

```powershell
# 1. Ir a la carpeta del proyecto CHM
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Distribuidora_los_amigos\Docs\Manual_CHM\source_es"

# 2. Compilar el CHM
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_es.hhp"

# 3. El CHM se genera en la carpeta padre: Distribuidora_los_amigos\Docs\Manual_CHM\help_es.chm

# 4. Copiar a donde la app lo busca
Copy-Item "..\help_es.chm" -Destination "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\help_es.chm" -Force
```

### **Opción 2: Usar el script automático**

```powershell
# Desde la carpeta Docs\Manual_CHM
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Distribuidora_los_amigos\Docs\Manual_CHM"

# Ejecutar el script que compila y copia automáticamente
.\compilar_simple.bat
```

---

## ?? Crear Contenido HTML

### **Plantilla Base:**

Usa el archivo `source_es/template.html` como base para crear nuevas páginas.

**Ejemplo para topic_32_recuperar.html:**

```html
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Recuperar Contraseña - Manual de Usuario</title>
    <link rel="stylesheet" href="../../css/styles.css">
</head>
<body>
    <div class="container">
        <header>
            <h1>?? Recuperar Contraseña</h1>
        </header>

        <nav class="breadcrumb">
            <a href="../general/topic_20_main.html">Inicio</a> &raquo; 
            <a href="topic_31_login.html">Login</a> &raquo; 
            <span>Recuperar Contraseña</span>
        </nav>

        <section class="content">
            <h2>?? Descripción</h2>
            <p>
                Esta funcionalidad permite recuperar el acceso a tu cuenta cuando 
                has olvidado tu contraseña.
            </p>

            <h2>?? Pasos para Recuperar Contraseña</h2>
            <ol>
                <li>Desde la pantalla de login, haz clic en "¿Olvidaste tu contraseña?"</li>
                <li>Ingresa tu nombre de usuario</li>
                <li>Haz clic en "Enviar código de recuperación"</li>
                <li>Revisa tu correo electrónico y copia el código recibido</li>
                <li>Ingresa el código en el formulario</li>
                <li>Ingresa tu nueva contraseña (mínimo 6 caracteres)</li>
                <li>Confirma la nueva contraseña</li>
                <li>Haz clic en "Cambiar contraseña"</li>
            </ol>

            <div class="note">
                <strong>?? Nota:</strong> El código de recuperación tiene una 
                validez limitada por seguridad.
            </div>

            <h2>?? Problemas Comunes</h2>
            <ul>
                <li><strong>No recibo el correo:</strong> Verifica tu carpeta de spam</li>
                <li><strong>Código inválido:</strong> El código puede haber expirado, solicita uno nuevo</li>
                <li><strong>Las contraseñas no coinciden:</strong> Asegúrate de escribir la misma contraseña en ambos campos</li>
            </ul>
        </section>

        <footer>
            <p>Sistema Distribuidora Los Amigos - Manual de Usuario v1.0</p>
        </footer>
    </div>
</body>
</html>
```

---

## ??? Archivos HTML que FALTAN Crear

Según el archivo `.hhp`, estos archivos necesitan contenido:

```
html/general/topic_21_intro.html
html/login/topic_32_recuperar.html          ? ESTE ES EL QUE COMPROBAMOS QUE FUNCIONA
html/login/topic_33_cambiar_pass.html
html/usuarios/topic_23_crear_usuario.html
html/usuarios/topic_24_asignar_rol.html
html/usuarios/topic_28_modificar_usuario.html
html/usuarios/topic_29_mostrar_usuarios.html
html/roles/topic_26_crear_rol.html
html/roles/topic_27_crear_patente.html
html/backup/topic_22_backup.html
html/backup/topic_30_restore.html
html/backup/topic_25_bitacora.html
html/clientes/topic_41_modificar_cliente.html
html/clientes/topic_42_mostrar_clientes.html
html/productos/topic_50_crear_producto.html
html/productos/topic_51_modificar_producto.html
html/productos/topic_52_mostrar_productos.html
html/productos/topic_53_eliminar_producto.html
html/proveedores/topic_60_crear_proveedor.html
html/proveedores/topic_61_modificar_proveedor.html
html/proveedores/topic_62_mostrar_proveedores.html
html/stock/topic_70_mostrar_stock.html
html/stock/topic_71_modificar_stock.html
html/pedidos/topic_80_crear_pedido.html
html/pedidos/topic_81_modificar_pedido.html
html/pedidos/topic_82_mostrar_pedidos.html
html/pedidos/topic_83_detalle_pedido.html
html/reportes/topic_90_reporte_stock_bajo.html
html/reportes/topic_91_reporte_mas_vendidos.html
```

---

## ? Workflow Completo

### **1. Crear una nueva página HTML:**
```bash
# Copiar la plantilla
cd source_es/html/login
copy ..\..\template.html topic_32_recuperar.html
```

### **2. Editar el contenido:**
- Abre `topic_32_recuperar.html` en tu editor
- Modifica el contenido según la funcionalidad
- Guarda los cambios

### **3. Compilar el CHM:**
```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Distribuidora_los_amigos\Docs\Manual_CHM\source_es"
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_es.hhp"
```

### **4. Copiar a la ubicación correcta:**
```powershell
Copy-Item "..\help_es.chm" -Destination "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\help_es.chm" -Force
```

### **5. Probar en la aplicación:**
- Ejecuta la aplicación
- Abre el formulario correspondiente
- Presiona F1
- Verifica que se abre la página correcta

---

## ?? Recursos Disponibles

### **CSS Styles:**
El archivo `source_es/css/styles.css` contiene los estilos para:
- `.container` - Contenedor principal
- `.breadcrumb` - Navegación de migas de pan
- `.note` - Cuadros de nota/información
- `.warning` - Cuadros de advertencia
- `code` - Bloques de código

### **Ejemplos Funcionales:**
- `html/general/topic_20_main.html` - Página principal
- `html/login/topic_31_login.html` - Login
- `html/clientes/topic_40_crear_cliente.html` - Crear cliente

---

## ?? Verificación

### **Comprobar que el CHM tiene MAP y ALIAS:**

1. Abre el archivo `source_es/help_es.hhp`
2. Busca la sección `[MAP]` - debe estar presente
3. Busca la sección `[ALIAS]` - debe estar presente
4. Verifica que el TopicID que necesitas esté listado

**Ejemplo:**
```ini
[MAP]
#define TOPIC_RECUPERAR_PASS 32

[ALIAS]
TOPIC_RECUPERAR_PASS=html\login\topic_32_recuperar.html
```

### **Comprobar que el CHM se compiló correctamente:**

```powershell
# Verificar fecha de modificación
Get-Item "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\help_es.chm" | Select-Object LastWriteTime

# Debería mostrar la fecha/hora de cuando lo compilaste
```

---

## ?? Documentación Adicional

Para más detalles, consulta estos archivos en la carpeta `Docs/Manual_CHM/`:

- **INICIO_RAPIDO.md** - Guía rápida de inicio
- **GUIA_CREACION_CHM.md** - Guía detallada de creación
- **GUIA_VISUAL_HTML_HELP_WORKSHOP.md** - Guía visual con capturas
- **README.md** - Información general del proyecto
- **SOLUCION_FINAL_F1_RECUPERAR_PASSWORD.md** - Solución al problema del F1

---

## ?? Próximos Pasos

1. **Crear contenido HTML** para las páginas faltantes usando `template.html`
2. **Compilar el CHM** con cada cambio
3. **Copiar** el CHM a la ubicación donde la app lo busca
4. **Probar** presionando F1 en cada formulario
5. **Iterar** hasta completar todas las páginas

---

## ?? Tips Importantes

### **Errores de Compilación:**
- ? Los errores "HHC5003: Error: Compilation failed" son **normales**
- ? El CHM se genera aunque aparezcan esos errores
- ? Los errores indican que faltan archivos HTML, pero no impiden la compilación

### **Rutas Relativas:**
- En los archivos HTML, usa rutas relativas: `../../css/styles.css`
- En el archivo .hhp, usa rutas relativas: `html\login\topic_32_recuperar.html`

### **Codificación:**
- Los archivos HTML deben usar UTF-8
- Incluye siempre: `<meta charset="UTF-8">`

---

**Última actualización:** 4 de Enero 2025 - 23:30  
**Estado:** ? Sistema F1 funcionando - Contenido HTML pendiente
