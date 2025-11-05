# ?? Sistema de Permisos y Control de Acceso

## ?? Resumen Ejecutivo

Este documento explica el funcionamiento completo del sistema de permisos implementado en la aplicación Distribuidora Los Amigos.

---

## ??? Arquitectura del Sistema

### Componentes Principales

```
Usuario
  ?? Familias (Roles)
       ?? Patentes (Permisos)
            ?? TipoAcceso (UI/Control/UseCases)
```

### Tipos de Acceso

| TipoAcceso | Valor | Descripción | Uso |
|------------|-------|-------------|-----|
| **UI** | 0 | Interfaz de Usuario | Usuarios que solo pueden visualizar/consultar |
| **Control** | 1 | Administración | Usuarios que pueden gestionar y modificar |
| **UseCases** | 2 | Casos de Uso | Permisos especiales para funcionalidades específicas |

---

## ?? Roles Configurados

### 1. Familia "Administrador"
- **Total Patentes:** 22
- **Patentes UI (visualización):** 8
  - Ver_usuarios
  - VER_PRODUCTOS
  - Mostrar_Stock
  - Mostrar_clientes
  - Mostrar_Proveedores
  - MOSTRAR_PEDIDOS
  - ReporteStockBajo
  - ReporteProductosMasVendidos

- **Patentes Control (administración):** 14
  - Crear_usuario
  - MODIFICAR
  - Crear_rol
  - Asignar_rol
  - CREAR_PEDIDO
  - AGREGAR
  - Modificar_Usuario
  - Modificar_Proveedor
  - General_Backup
  - RestaurarBackup
  - Crear_patente
  - ELIMINAR
  - Crear_Proveedor
  - Crear_cliente

**Acceso Completo:** ? Gestión de Usuarios, ? Backup/Restore, ? Todas las funcionalidades

### 2. Familia "UI"
- **Total Patentes:** 2
- **Patentes UI:** 1 (Ver_usuarios)
- **Patentes Control:** 1 (Crear_usuario)

**Acceso Limitado:** ? NO puede ver menús de gestión avanzada

---

## ?? Menús Protegidos

### Menús que Requieren TipoAcceso.Control

| Menú | Visible para Administrador | Visible para UI |
|------|---------------------------|-----------------|
| **GESTION DE USUARIOS** | ? SÍ | ? NO |
| **BACKUP Y RESTORE** | ? SÍ | ? NO |

### Menús Públicos (Sin Restricción)

- PEDIDOS
- CLIENTE
- PRODUCTOS
- STOCK
- BUSQUEDA
- REPORTES
- PROVEEDORES

---

## ?? Implementación Técnica

### 1. Decorador de Menús (`MenuItemDecorator`)

**Ubicación:** `Service\Logic\AccesoLogic.cs`

```csharp
public class MenuItemDecorator : IControlAccess
{
    private readonly ToolStripMenuItem _menuItem;
    private readonly TipoAcceso _requiredAccess;

    public void SetAccess(List<Patente> patentesUsuario)
    {
        // Validar lista null
        if (patentesUsuario == null || patentesUsuario.Count == 0)
        {
            _menuItem.Visible = false;
            return;
        }

        // Verificar si tiene patentes del tipo requerido
        var hasAccess = patentesUsuario
            .Where(p => p != null)
            .Any(p => p.TipoAcceso == _requiredAccess);

        _menuItem.Visible = hasAccess;
    }
}
```

**Características:**
- ? Valida que la lista de patentes no sea null
- ? Filtra elementos null de la lista
- ? Oculta el menú si el usuario no tiene el tipo de acceso requerido

### 2. Servicio de Acceso (`AccesoService`)

**Ubicación:** `Service\Facade\AccesoService.cs`

```csharp
public static void ConfigureMenuItems(
    ToolStripMenuItem menuItem, 
    List<Patente> patentesUsuario)
{
    var decorator = new MenuItemDecorator(menuItem, TipoAcceso.Control);
    decorator.SetAccess(patentesUsuario);
}
```

### 3. Aplicación en el Formulario Principal

**Ubicación:** `Distribuidora_los_amigos\Forms\main.cs`

```csharp
private void MainForm_Load(object sender, EventArgs e)
{
    // Obtener patentes del usuario
    List<Patente> patentesDelUsuario = usuarioLogueado.GetPatentes();
    
    // Aplicar control de acceso
    AccesoService.ConfigureMenuItems(tOOLSToolStripMenuItem1, patentesDelUsuario); 
    AccesoService.ConfigureMenuItems(bACKUPYRESTOREToolStripMenuItem, patentesDelUsuario);
}
```

---

## ??? Estructura de Base de Datos

### Tablas Principales

```sql
-- Usuarios
Usuario (IdUsuario, UserName, Password, Email, Estado)

-- Familias (Roles)
Familia (IdFamilia, Nombre)

-- Patentes (Permisos)
Patente (IdPatente, Nombre, DataKey, TipoAcceso)

-- Relaciones
Usuario_Familia (IdUsuario, IdFamilia)
Familia_Patente (IdFamilia, IdPatente)
```

### Consultas Útiles

**Ver usuarios y sus roles:**
```sql
SELECT 
    u.UserName,
    STRING_AGG(f.Nombre, ', ') AS Familias
FROM Usuario u
LEFT JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
LEFT JOIN Familia f ON uf.IdFamilia = f.IdFamilia
GROUP BY u.UserName
```

**Ver patentes por familia:**
```sql
SELECT 
    f.Nombre AS Familia,
    COUNT(DISTINCT fp.IdPatente) AS TotalPatentes,
    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS UI,
    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS Control
FROM Familia f
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
GROUP BY f.Nombre
```

---

## ?? Gestión de Permisos

### Asignar Usuario a Familia

**Opción 1: Desde la UI**
1. Iniciar sesión como Administrador
2. Ir a: **GESTION DE USUARIOS** ? **Asignar rol**
3. Seleccionar usuario y familia
4. Hacer clic en "Asignar"

**Opción 2: Desde SQL**
```sql
DECLARE @IdUsuario UNIQUEIDENTIFIER
DECLARE @IdFamilia UNIQUEIDENTIFIER

SELECT @IdUsuario = IdUsuario FROM Usuario WHERE UserName = 'nombre_usuario'
SELECT @IdFamilia = IdFamilia FROM Familia WHERE Nombre = 'Administrador'

INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
VALUES (@IdUsuario, @IdFamilia)
```

### Crear Nueva Patente

**Desde la UI:**
1. Iniciar sesión como Administrador
2. Ir a: **GESTION DE USUARIOS** ? **Crear patente**
3. Completar formulario
4. Guardar

**Desde SQL:**
```sql
INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
VALUES (NEWID(), 'Nombre_Patente', 'DataKey_Patente', 1) -- 1 = Control
```

### Crear Nueva Familia (Rol)

**Desde la UI:**
1. Iniciar sesión como Administrador
2. Ir a: **GESTION DE USUARIOS** ? **Crear rol**
3. Ingresar nombre del rol
4. Seleccionar patentes
5. Guardar

---

## ?? Testing del Sistema de Permisos

### Checklist de Pruebas

- [ ] **Usuario Administrador**
  - [ ] Puede ver menú "GESTION DE USUARIOS"
  - [ ] Puede ver menú "BACKUP Y RESTORE"
  - [ ] Puede acceder a todas las funcionalidades
  - [ ] Muestra "Rol: Administrador" al iniciar sesión

- [ ] **Usuario UI**
  - [ ] NO puede ver menú "GESTION DE USUARIOS"
  - [ ] NO puede ver menú "BACKUP Y RESTORE"
  - [ ] Puede ver menús públicos (PEDIDOS, CLIENTES, etc.)
  - [ ] Muestra "Rol: UI" al iniciar sesión

- [ ] **Usuario Sin Rol**
  - [ ] Muestra "Sin rol asignado"
  - [ ] NO puede ver menús restringidos
  - [ ] Debe ser asignado a un rol por un administrador

### Scripts de Prueba

**Verificar estado actual:**
```sql
-- Ejecutar: 04_Guia_Completa_Configuracion_Permisos.sql
```

**Limpiar y reorganizar:**
```sql
-- Ejecutar: 05_Limpiar_Base_Datos_Fixed.sql
```

---

## ?? Mantenimiento

### Agregar Nuevo Menú Protegido

1. **En `main.Designer.cs`**: Agregar el nuevo `ToolStripMenuItem`

2. **En `main.cs` ? `MainForm_Load`**: Agregar control de acceso
   ```csharp
   AccesoService.ConfigureMenuItems(nuevoMenu, patentesDelUsuario);
   ```

3. **Crear patente en BD** (si no existe)
   ```sql
   INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso)
   VALUES (NEWID(), 'Nueva_Funcionalidad', 'DataKey', 1)
   ```

4. **Asignar patente a roles** que deban tener acceso

### Cambiar Tipo de Acceso de una Patente

```sql
UPDATE Patente
SET TipoAcceso = 0  -- 0=UI, 1=Control, 2=UseCases
WHERE Nombre = 'Nombre_Patente'
```

---

## ?? Troubleshooting

### Problema: Menú no se oculta para usuarios UI

**Solución:**
1. Verificar que el menú tenga `AccesoService.ConfigureMenuItems()` en `MainForm_Load`
2. Verificar que el usuario esté asignado correctamente a la familia UI
3. Cerrar sesión y volver a iniciar

### Problema: "Sin rol asignado"

**Solución:**
```sql
-- Asignar usuario a familia
SELECT @IdUsuario = IdUsuario FROM Usuario WHERE UserName = 'usuario'
SELECT @IdFamilia = IdFamilia FROM Familia WHERE Nombre = 'UI'

INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
VALUES (@IdUsuario, @IdFamilia)
```

### Problema: Usuario con rol Administrador no ve menús

**Solución:**
1. Verificar que la familia "Administrador" tenga patentes de tipo Control
2. Ejecutar script de verificación: `04_Guia_Completa_Configuracion_Permisos.sql`
3. Verificar que `TipoAcceso = 1` en las patentes administrativas

---

## ?? Documentación Relacionada

- [Configuración Inicial](CONFIGURACION_INICIAL.md)
- [Backup y Restore](BACKUP_Y_RESTORE.md)
- [Usuario Administrador por Defecto](USUARIO_ADMINISTRADOR_DEFAULT.md)
- Scripts SQL en: `Docs/SQL_Scripts/`

---

## ? Resumen de Cambios Implementados

### Código C#
1. ? `AccesoLogic.cs` - Decorador con validación null
2. ? `Usuario.cs` - GetPatentes() con validación null
3. ? `AccesoService.cs` - Método ConfigureMenuItems
4. ? `main.cs` - Aplicación de permisos en MainForm_Load

### Base de Datos
1. ? Patentes con TipoAcceso correcto (0=UI, 1=Control)
2. ? Familias "Administrador" y "UI" configuradas
3. ? Relaciones Usuario_Familia establecidas
4. ? Datos de prueba limpiados

### Documentación
1. ? Scripts SQL de limpieza y configuración
2. ? Guías de troubleshooting
3. ? Documentación completa del sistema

---

**Última actualización:** 2024  
**Versión:** 1.0  
**Estado:** ? Producción

