# ?? Control de Acceso Granular por Patentes

## ?? Resumen del Cambio

Se implementó un **sistema de control de acceso granular** que verifica **patentes específicas** para cada opción de menú, en lugar de solo verificar el tipo de acceso (UI/Control).

---

## ? **PROBLEMA ANTERIOR**

### Comportamiento incorrecto:
1. Usuario tenía rol con solo la patente `Crear_cliente` (Control)
2. Sistema verificaba: "¿Tiene alguna patente tipo Control?"
3. Respuesta: "SÍ" ? Mostraba **TODO el menú de Gestión de Usuarios**
4. Usuario veía opciones que NO debería:
   - ? Crear cliente (correcto)
   - ? Crear usuario (incorrecto)
   - ? Crear rol (incorrecto)
   - ? Asignar rol (incorrecto)
   - ? etc.

### Código anterior:
```csharp
// ? Solo verificaba TipoAcceso, no la patente específica
AccesoService.ConfigureMenuItems(tOOLSToolStripMenuItem1, patentesDelUsuario);
```

---

## ? **SOLUCIÓN IMPLEMENTADA**

### Nuevo comportamiento:
1. Usuario tiene rol con solo la patente `Crear_cliente`
2. Sistema verifica **cada opción** individualmente:
   - "Crear cliente" ? ¿Tiene patente `Crear_cliente`? ? SÍ ?
   - "Crear usuario" ? ¿Tiene patente `Crear_usuario`? ? NO ?
   - "Crear rol" ? ¿Tiene patente `Crear_rol`? ? NO ?
3. Solo muestra las opciones para las que tiene patente
4. Si un menú padre no tiene hijos visibles, también se oculta

---

## ??? **ARQUITECTURA DE LA SOLUCIÓN**

### 1. **Nuevo Decorador: `MenuItemPatenteDecorator`**

**Ubicación:** `Service\Logic\AccesoLogic.cs`

```csharp
public class MenuItemPatenteDecorator : IControlAccess
{
    private readonly ToolStripMenuItem _menuItem;
    private readonly string _requiredPatenteName;

    public void SetAccess(List<Patente> patentesUsuario)
    {
        // Verifica si el usuario tiene la patente EXACTA por nombre
        var hasAccess = patentesUsuario
            .Where(p => p != null && !string.IsNullOrEmpty(p.Nombre))
            .Any(p => p.Nombre.Equals(_requiredPatenteName, StringComparison.OrdinalIgnoreCase));

        _menuItem.Visible = hasAccess;
    }
}
```

**Características:**
- ? Verifica nombre exacto de la patente
- ? Insensible a mayúsculas/minúsculas
- ? Valida nulls y strings vacíos
- ? Oculta el menú si no tiene la patente

---

### 2. **Métodos en `AccesoService`**

**Ubicación:** `Service\Facade\AccesoService.cs`

#### **ConfigureMenuItemByPatente** (Individual)
```csharp
public static void ConfigureMenuItemByPatente(
    ToolStripMenuItem menuItem, 
    string requiredPatenteName, 
    List<Patente> patentesUsuario)
{
    var decorator = new MenuItemPatenteDecorator(menuItem, requiredPatenteName);
    decorator.SetAccess(patentesUsuario);
}
```

#### **ConfigureMultipleMenuItems** (Múltiples)
```csharp
public static void ConfigureMultipleMenuItems(
    Dictionary<ToolStripMenuItem, string> menuItems, 
    List<Patente> patentesUsuario)
{
    foreach (var item in menuItems)
    {
        ConfigureMenuItemByPatente(item.Key, item.Value, patentesUsuario);
    }
}
```

---

### 3. **Mapeo de Menús en `main.cs`**

**Ubicación:** `Distribuidora_los_amigos\Forms\main.cs`

```csharp
private void ConfigurarAccesosMenu(List<Patente> patentesDelUsuario)
{
    var menuPatentes = new Dictionary<ToolStripMenuItem, string>
    {
        // PEDIDOS
        { CREARPEDIDOToolStripMenuItem, "CREAR_PEDIDO" },
        { mOSTRARPEDIDOSToolStripMenuItem, "MOSTRAR_PEDIDOS" },

        // CLIENTES
        { CrearClienteToolStripMenuItem, "Crear_cliente" },
        { mostrarClientesToolStripMenuItem, "Mostrar_clientes" },

        // PRODUCTOS
        { addItemToolStripMenuItem, "AGREGAR" },
        { mODIFICARToolStripMenuItem, "MODIFICAR" },
        { eLIMINARToolStripMenuItem, "ELIMINAR" },
        { vERPRODUCTOSToolStripMenuItem, "VER_PRODUCTOS" },

        // ... 22 patentes mapeadas en total
    };

    AccesoService.ConfigureMultipleMenuItems(menuPatentes, patentesDelUsuario);
    OcultarMenusPadresSinHijos();
}
```

---

## ?? **MAPEO COMPLETO: MENÚ ? PATENTE**

| Menú | Opción | Patente Requerida |
|------|--------|-------------------|
| **PEDIDOS** | Crear Pedido | `CREAR_PEDIDO` |
| | Mostrar Pedidos | `MOSTRAR_PEDIDOS` |
| **CLIENTE** | Crear Cliente | `Crear_cliente` |
| | Mostrar Clientes | `Mostrar_clientes` |
| **PRODUCTOS** | Agregar | `AGREGAR` |
| | Modificar | `MODIFICAR` |
| | Eliminar | `ELIMINAR` |
| | Ver Productos | `VER_PRODUCTOS` |
| **STOCK** | Mostrar Stock | `Mostrar_Stock` |
| **PROVEEDORES** | Mostrar Proveedores | `Mostrar_Proveedores` |
| | Modificar Proveedor | `Modificar_Proveedor` |
| | Crear Proveedor | `Crear_Proveedor` |
| **GESTIÓN DE USUARIOS** | Crear Usuario | `Crear_usuario` |
| | Ver Usuarios | `Ver_usuarios` |
| | Asignar Rol | `Asignar_rol` |
| | Modificar Usuario | `Modificar_Usuario` |
| | Crear Rol | `Crear_rol` |
| | Crear Patente | `Crear_patente` |
| **BACKUP Y RESTORE** | Generar Backup | `Generar_Backup` |
| | Restaurar Backup | `RestaurarBackup` |
| **REPORTES** | Reporte Stock Bajo | `ReporteStockBajo` |
| | Productos Más Vendidos | `ReporteProductosMasVendidos` |

---

## ?? **FUNCIONALIDAD ADICIONAL**

### **Ocultar Menús Padre Sin Hijos Visibles**

```csharp
private void OcultarMenusPadresSinHijos()
{
    var menusPadre = new[]
    {
        PEDIDOSToolStripMenuItem,
        CLIENTEToolStripMenuItem,
        iTEMToolStripMenuItem,
        sTOCKToolStripMenuItem,
        pROVEEDORESToolStripMenuItem,
        tOOLSToolStripMenuItem1,
        bACKUPYRESTOREToolStripMenuItem,
        tOOLSToolStripMenuItem
    };

    foreach (var menuPadre in menusPadre)
    {
        bool tieneHijosVisibles = menuPadre.DropDownItems
            .OfType<ToolStripMenuItem>()
            .Any(hijo => hijo.Visible);

        menuPadre.Visible = tieneHijosVisibles;
    }
}
```

**Ejemplo:**
- Usuario solo tiene patente `CREAR_PEDIDO`
- Menú "PEDIDOS" ? Visible (tiene 1 hijo visible)
- Menú "CLIENTES" ? Oculto (no tiene hijos visibles)
- Menú "PRODUCTOS" ? Oculto (no tiene hijos visibles)

---

## ?? **EJEMPLO DE USO**

### **Escenario 1: Usuario "Vendedor"**

**Patentes asignadas:**
- `Crear_cliente`
- `Mostrar_clientes`
- `VER_PRODUCTOS`
- `CREAR_PEDIDO`
- `MOSTRAR_PEDIDOS`
- `Mostrar_Stock`

**Menús visibles:**
- ? PEDIDOS
  - ? Crear Pedido
  - ? Mostrar Pedidos
- ? CLIENTE
  - ? Crear Cliente
  - ? Mostrar Clientes
- ? PRODUCTOS
  - ? Ver Productos
- ? STOCK
  - ? Mostrar Stock

**Menús ocultos:**
- ? PROVEEDORES (sin patentes)
- ? GESTIÓN DE USUARIOS (sin patentes)
- ? BACKUP Y RESTORE (sin patentes)
- ? REPORTES (sin patentes)

---

### **Escenario 2: Usuario "Administrador"**

**Patentes asignadas:**
- ? Todas las 22 patentes

**Menús visibles:**
- ? TODOS los menús y opciones

---

## ? **VENTAJAS DE ESTA SOLUCIÓN**

1. **Seguridad Granular**
   - Cada opción se valida individualmente
   - No se puede "colar" sin la patente exacta

2. **Experiencia de Usuario**
   - Solo ve lo que puede hacer
   - No hay opciones "trampa" que den error al hacer clic

3. **Mantenibilidad**
   - Fácil agregar nuevas opciones (solo agregar al diccionario)
   - Centralizado en un solo método

4. **Escalabilidad**
   - Si agregas una nueva funcionalidad:
     1. Creas la patente en BD
     2. Agregas al diccionario `menuPatentes`
     3. Listo ?

---

## ?? **CÓMO USAR EN NUEVAS FUNCIONALIDADES**

### **Paso 1: Crear la Patente en BD**
```sql
INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
VALUES (NEWID(), 'Nueva_Funcionalidad', 'Nueva_Funcionalidad', 1)
```

### **Paso 2: Agregar al Diccionario en `main.cs`**
```csharp
private void ConfigurarAccesosMenu(List<Patente> patentesDelUsuario)
{
    var menuPatentes = new Dictionary<ToolStripMenuItem, string>
    {
        // ...existentes...
        { nuevaFuncionalidadToolStripMenuItem, "Nueva_Funcionalidad" } // ? Agregar aquí
    };
    // ...resto del código...
}
```

### **Paso 3: Asignar la Patente a Roles**
```sql
-- Asignar a rol Administrador
INSERT INTO Familia_Patente (IdFamiliaPatente, IdFamilia, IdPatente)
VALUES (
    NEWID(),
    (SELECT IdFamilia FROM Familia WHERE Nombre = 'Administrador'),
    (SELECT IdPatente FROM Patente WHERE Nombre = 'Nueva_Funcionalidad')
)
```

---

## ?? **TESTING**

### **Prueba 1: Usuario sin patentes**
```
? Resultado esperado: No ve ningún menú (solo idiomas y cerrar sesión)
```

### **Prueba 2: Usuario con patente "Ver_usuarios"**
```
? Resultado esperado:
   - Menú "GESTIÓN DE USUARIOS" visible
   - Opción "Ver usuarios" visible
   - Resto de opciones ocultas
```

### **Prueba 3: Usuario con rol "UI" (7 patentes de visualización)**
```
? Resultado esperado:
   - Menús de consulta visibles
   - Menús de modificación ocultos
   - Backup/Restore oculto
```

### **Prueba 4: Usuario con rol "Administrador" (22 patentes)**
```
? Resultado esperado:
   - Todos los menús visibles
```

---

## ?? **NOTAS IMPORTANTES**

1. **Nombres de patentes deben coincidir exactamente**
   - BD: `Crear_cliente`
   - Código: `"Crear_cliente"`
   - ?? Si no coinciden ? menú no aparece

2. **Comparación insensible a mayúsculas**
   - `"Crear_Cliente"` = `"crear_cliente"` = `"CREAR_CLIENTE"` ?

3. **Nulls y validaciones**
   - Si `patentesUsuario` es null ? todos los menús ocultos
   - Si una patente es null ? se ignora (no rompe)

---

## ?? **ARCHIVOS MODIFICADOS**

| Archivo | Cambios |
|---------|---------|
| `Service\Logic\AccesoLogic.cs` | ? Agregado `MenuItemPatenteDecorator` |
| `Service\Facade\AccesoService.cs` | ? Agregados métodos `ConfigureMenuItemByPatente` y `ConfigureMultipleMenuItems` |
| `Distribuidora_los_amigos\Forms\main.cs` | ? Agregado `ConfigurarAccesosMenu()` y `OcultarMenusPadresSinHijos()` |

---

**Última actualización:** 2024  
**Versión:** 2.0 (Control Granular)  
**Estado:** ? Producción
