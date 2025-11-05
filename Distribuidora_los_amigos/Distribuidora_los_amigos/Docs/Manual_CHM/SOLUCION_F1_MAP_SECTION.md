# ?? Solución: Ayuda Contextual (F1) - "HH_HELP_CONTEXT called without a [MAP] section"

## ?? Problema

Al presionar **F1** en la aplicación, aparece el error:
```
HTML Help Author Message
HH_HELP_CONTEXT called without a [MAP] section.
```

## ? Causa

El archivo CHM no tiene configuradas las secciones `[MAP]` y `[ALIAS]` que permiten mapear IDs numéricos a archivos HTML específicos.

---

## ??? Solución Implementada

He actualizado el script `generar_proyecto_chm.ps1` para que **automáticamente** genere el archivo `.hhp` con las secciones necesarias.

### **Archivos Modificados:**

1. ? `generar_proyecto_chm.ps1` - Ahora incluye secciones `[MAP]` y `[ALIAS]`

---

## ?? Cómo Funciona la Ayuda Contextual (F1)

### **1. Código C# (ManualRepository.cs)**

```csharp
public void AbrirAyudaLogin()
{
    Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "31");
    //                                                         ^^^^
    //                                                    ID numérico
}
```

### **2. Archivo CHM (help_es.hhp) - Sección [MAP]**

```ini
[MAP]
#define TOPIC_LOGIN 31
```

Esto define que el ID `31` corresponde al identificador `TOPIC_LOGIN`.

### **3. Archivo CHM (help_es.hhp) - Sección [ALIAS]**

```ini
[ALIAS]
TOPIC_LOGIN=html\login\topic_31_login.html
```

Esto mapea `TOPIC_LOGIN` al archivo HTML específico.

### **Resultado:**

```
ID: 31 ? TOPIC_LOGIN ? html\login\topic_31_login.html
```

---

## ?? IDs Mapeados en el Sistema

| ID | Identificador | Archivo HTML | Función |
|----|---------------|--------------|---------|
| 20 | TOPIC_MAIN | topic_20_main.html | Pantalla Principal |
| 31 | TOPIC_LOGIN | topic_31_login.html | Inicio de Sesión |
| 32 | TOPIC_RECUPERAR_PASSWORD | topic_32_recuperar_password.html | Recuperar Contraseña |
| 33 | TOPIC_CAMBIAR_PASSWORD | topic_33_cambiar_password.html | Cambiar Contraseña |
| 22 | TOPIC_BACKUP | topic_22_backup.html | Backup |
| 30 | TOPIC_RESTORE | topic_30_restore.html | Restore |
| 23 | TOPIC_CREAR_USUARIO | topic_23_crear_usuario.html | Crear Usuario |
| 28 | TOPIC_MODIFICAR_USUARIO | topic_28_modificar_usuario.html | Modificar Usuario |
| 29 | TOPIC_MOSTRAR_USUARIOS | topic_29_mostrar_usuarios.html | Mostrar Usuarios |
| 24 | TOPIC_ASIGNAR_ROL | topic_24_asignar_rol.html | Asignar Rol |
| 26 | TOPIC_CREAR_ROL | topic_26_crear_rol.html | Crear Rol |
| 27 | TOPIC_CREAR_PATENTE | topic_27_crear_patente.html | Crear Patente |
| 25 | TOPIC_BITACORA | topic_25_bitacora.html | Bitácora |
| 40 | TOPIC_CREAR_CLIENTE | topic_40_crear_cliente.html | Crear Cliente |
| 41 | TOPIC_MODIFICAR_CLIENTE | topic_41_modificar_cliente.html | Modificar Cliente |
| 42 | TOPIC_MOSTRAR_CLIENTES | topic_42_mostrar_clientes.html | Mostrar Clientes |
| 50 | TOPIC_CREAR_PRODUCTO | topic_50_crear_producto.html | Crear Producto |
| 51 | TOPIC_MODIFICAR_PRODUCTO | topic_51_modificar_producto.html | Modificar Producto |
| 52 | TOPIC_MOSTRAR_PRODUCTOS | topic_52_mostrar_productos.html | Mostrar Productos |
| 53 | TOPIC_ELIMINAR_PRODUCTO | topic_53_eliminar_producto.html | Eliminar Producto |
| 60 | TOPIC_CREAR_PROVEEDOR | topic_60_crear_proveedor.html | Crear Proveedor |
| 61 | TOPIC_MODIFICAR_PROVEEDOR | topic_61_modificar_proveedor.html | Modificar Proveedor |
| 62 | TOPIC_MOSTRAR_PROVEEDORES | topic_62_mostrar_proveedores.html | Mostrar Proveedores |
| 70 | TOPIC_MOSTRAR_STOCK | topic_70_mostrar_stock.html | Mostrar Stock |
| 71 | TOPIC_MODIFICAR_STOCK | topic_71_modificar_stock.html | Modificar Stock |
| 80 | TOPIC_CREAR_PEDIDO | topic_80_crear_pedido.html | Crear Pedido |
| 81 | TOPIC_MODIFICAR_PEDIDO | topic_81_modificar_pedido.html | Modificar Pedido |
| 82 | TOPIC_MOSTRAR_PEDIDOS | topic_82_mostrar_pedidos.html | Mostrar Pedidos |
| 83 | TOPIC_DETALLE_PEDIDO | topic_83_detalle_pedido.html | Detalle de Pedido |
| 90 | TOPIC_REPORTE_STOCK_BAJO | topic_90_reporte_stock_bajo.html | Reporte Stock Bajo |
| 91 | TOPIC_REPORTE_PRODUCTOS_MAS_VENDIDOS | topic_91_reporte_productos_mas_vendidos.html | Productos Más Vendidos |

---

## ?? Pasos para Solucionar

### **Paso 1: Regenerar el Proyecto CHM**

**Doble click en:**
```
generar_proyecto.bat
```

Esto creará el archivo `help_es.hhp` con las secciones `[MAP]` y `[ALIAS]`.

### **Paso 2: Recompilar el CHM**

**Doble click en:**
```
compilar_directo.bat
```

Esto generará `help_es.chm` con soporte para F1.

### **Paso 3: Copiar el CHM a la Ubicación Configurada**

Copiar el archivo compilado:
```
Desde: C:\DistribuidoraLosAmigos\Manual\help_es.chm
Hacia:  C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\help_es.chm
```

O actualizar `App.config`:
```xml
<add key="HelpFilePath_es-ES" value="C:\DistribuidoraLosAmigos\Manual\help_es.chm"/>
```

### **Paso 4: Probar en la Aplicación**

1. Ejecutar la aplicación
2. Ir a cualquier formulario
3. Presionar **F1**
4. Debería abrir la ayuda contextual correspondiente

---

## ?? Ejemplo de Archivo .HHP Generado

```ini
[OPTIONS]
Compiled file=..\help_es.chm
Contents file=help_es.hhc
Default topic=html\general\topic_20_main.html
Display compile progress=Yes
Full-text search=Yes
Index file=help_es.hhk
Language=0x0c0a Espanol (Espana)
Title=Manual de Usuario - Distribuidora Los Amigos

[MAP]
#define TOPIC_MAIN 20
#define TOPIC_LOGIN 31
#define TOPIC_CREAR_CLIENTE 40
... (más definiciones)

[ALIAS]
TOPIC_MAIN=html\general\topic_20_main.html
TOPIC_LOGIN=html\login\topic_31_login.html
TOPIC_CREAR_CLIENTE=html\clientes\topic_40_crear_cliente.html
... (más mapeos)

[FILES]
css\styles.css
html\general\topic_20_main.html
html\login\topic_31_login.html
... (más archivos)
```

---

## ? Verificación

### **Verificar que el .hhp tiene las secciones correctas:**

1. Abrir: `C:\DistribuidoraLosAmigos\Manual\source_es\help_es.hhp`
2. Buscar la sección `[MAP]`
3. Buscar la sección `[ALIAS]`
4. Verificar que existan los IDs usados en el código

### **Probar Ayuda Contextual:**

```csharp
// En cualquier formulario
protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
{
    if (keyData == Keys.F1)
    {
        // Debería abrir la ayuda sin error
        manualService.AbrirAyudaLogin(); // o el método correspondiente
        return true;
    }
    return base.ProcessCmdKey(ref msg, keyData);
}
```

---

## ?? Debugging

Si aún no funciona, verificar:

1. ? El archivo CHM existe en la ruta configurada
2. ? El archivo CHM tiene las secciones [MAP] y [ALIAS]
3. ? Los IDs en ManualRepository.cs coinciden con los del .hhp
4. ? Los archivos HTML existen en las rutas especificadas

### **Ver el contenido del CHM:**

1. Doble click en `help_es.chm`
2. Verificar que se abre correctamente
3. Probar la búsqueda y navegación

---

## ?? Resultado Esperado

Al presionar **F1** en cualquier formulario:

? Se abre el archivo CHM
? Se muestra la página específica del formulario
? Sin mensajes de error

---

**Fecha:** 2024  
**Estado:** ? Solucionado  
**Archivos Modificados:** `generar_proyecto_chm.ps1`
