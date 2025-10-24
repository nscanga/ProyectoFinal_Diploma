# ?? Configuración de Reportes - Guía Completa

## ? Estado de Implementación

| Componente | Estado | Observaciones |
|------------|--------|---------------|
| Formularios | ? Completado | ReporteStockBajoForm y ReporteProductosMasVendidosForm |
| Integración main.cs | ? Completado | Métodos Click configurados |
| Menú UI | ? Completado | Opciones agregadas al menú REPORTES |
| Traducciones | ? Completado | Español, Inglés y Portugués |
| Compilación | ? Exitosa | Sin errores |
| Permisos/Patentes | ? Pendiente | Requiere configuración manual |

---

## 1?? **CONFIGURACIÓN DE PERMISOS (PATENTES)**

### Scripts SQL para Crear las Patentes

Ejecuta los siguientes scripts en tu base de datos para crear las patentes necesarias:

```sql
-- ============================================
-- Script: Crear Patentes para Reportes
-- Base de datos: Distribuidora Los Amigos
-- Fecha: 2024
-- ============================================

USE [Distribuidora_Los_Amigos]
GO

-- 1. Crear patente para Reporte de Stock Bajo
IF NOT EXISTS (SELECT 1 FROM Patentes WHERE Nombre = 'REPORTE_STOCK_BAJO')
BEGIN
    INSERT INTO Patentes (IdPatente, Nombre, Descripcion)
    VALUES (
        NEWID(),
        'REPORTE_STOCK_BAJO',
        'Permite acceder al reporte de productos con stock bajo o crítico'
    )
    PRINT '? Patente REPORTE_STOCK_BAJO creada exitosamente'
END
ELSE
BEGIN
    PRINT '?? La patente REPORTE_STOCK_BAJO ya existe'
END
GO

-- 2. Crear patente para Reporte de Productos Más Vendidos
IF NOT EXISTS (SELECT 1 FROM Patentes WHERE Nombre = 'REPORTE_PRODUCTOS_MAS_VENDIDOS')
BEGIN
    INSERT INTO Patentes (IdPatente, Nombre, Descripcion)
    VALUES (
        NEWID(),
        'REPORTE_PRODUCTOS_MAS_VENDIDOS',
        'Permite acceder al reporte de productos más vendidos'
    )
    PRINT '? Patente REPORTE_PRODUCTOS_MAS_VENDIDOS creada exitosamente'
END
ELSE
BEGIN
    PRINT '?? La patente REPORTE_PRODUCTOS_MAS_VENDIDOS ya existe'
END
GO

-- 3. Verificar patentes creadas
SELECT 
    IdPatente,
    Nombre,
    Descripcion,
    GETDATE() AS FechaConsulta
FROM Patentes
WHERE Nombre IN ('REPORTE_STOCK_BAJO', 'REPORTE_PRODUCTOS_MAS_VENDIDOS')
GO
```

---

## 2?? **ASIGNACIÓN DE PERMISOS A ROLES**

### Opción A: Asignar a Todos los Roles (Recomendado para inicio)

```sql
-- ============================================
-- Asignar permisos de reportes a todos los roles existentes
-- ============================================

USE [Distribuidora_Los_Amigos]
GO

DECLARE @IdPatenteStockBajo UNIQUEIDENTIFIER
DECLARE @IdPatenteProductosMasVendidos UNIQUEIDENTIFIER

-- Obtener IDs de las patentes
SELECT @IdPatenteStockBajo = IdPatente 
FROM Patentes 
WHERE Nombre = 'REPORTE_STOCK_BAJO'

SELECT @IdPatenteProductosMasVendidos = IdPatente 
FROM Patentes 
WHERE Nombre = 'REPORTE_PRODUCTOS_MAS_VENDIDOS'

-- Asignar a todos los roles (Familias)
INSERT INTO FamiliasPatentes (IdFamiliaPatente, IdFamilia, IdPatente)
SELECT 
    NEWID(),
    f.IdFamilia,
    @IdPatenteStockBajo
FROM Familias f
WHERE NOT EXISTS (
    SELECT 1 FROM FamiliasPatentes fp
    WHERE fp.IdFamilia = f.IdFamilia 
    AND fp.IdPatente = @IdPatenteStockBajo
)

INSERT INTO FamiliasPatentes (IdFamiliaPatente, IdFamilia, IdPatente)
SELECT 
    NEWID(),
    f.IdFamilia,
    @IdPatenteProductosMasVendidos
FROM Familias f
WHERE NOT EXISTS (
    SELECT 1 FROM FamiliasPatentes fp
    WHERE fp.IdFamilia = f.IdFamilia 
    AND fp.IdPatente = @IdPatenteProductosMasVendidos
)

PRINT '? Permisos de reportes asignados a todos los roles'
GO
```

### Opción B: Asignar Solo a Roles Específicos

```sql
-- ============================================
-- Asignar permisos solo a roles específicos
-- ============================================

USE [Distribuidora_Los_Amigos]
GO

DECLARE @IdPatenteStockBajo UNIQUEIDENTIFIER
DECLARE @IdPatenteProductosMasVendidos UNIQUEIDENTIFIER
DECLARE @IdRolAdmin UNIQUEIDENTIFIER
DECLARE @IdRolGerente UNIQUEIDENTIFIER

-- Obtener IDs de las patentes
SELECT @IdPatenteStockBajo = IdPatente 
FROM Patentes 
WHERE Nombre = 'REPORTE_STOCK_BAJO'

SELECT @IdPatenteProductosMasVendidos = IdPatente 
FROM Patentes 
WHERE Nombre = 'REPORTE_PRODUCTOS_MAS_VENDIDOS'

-- Obtener IDs de roles (ajusta los nombres según tu BD)
SELECT @IdRolAdmin = IdFamilia FROM Familias WHERE Nombre = 'Administrador'
SELECT @IdRolGerente = IdFamilia FROM Familias WHERE Nombre = 'Gerente'

-- Asignar a Administrador
IF NOT EXISTS (SELECT 1 FROM FamiliasPatentes WHERE IdFamilia = @IdRolAdmin AND IdPatente = @IdPatenteStockBajo)
    INSERT INTO FamiliasPatentes VALUES (NEWID(), @IdRolAdmin, @IdPatenteStockBajo)

IF NOT EXISTS (SELECT 1 FROM FamiliasPatentes WHERE IdFamilia = @IdRolAdmin AND IdPatente = @IdPatenteProductosMasVendidos)
    INSERT INTO FamiliasPatentes VALUES (NEWID(), @IdRolAdmin, @IdPatenteProductosMasVendidos)

-- Asignar a Gerente
IF NOT EXISTS (SELECT 1 FROM FamiliasPatentes WHERE IdFamilia = @IdRolGerente AND IdPatente = @IdPatenteStockBajo)
    INSERT INTO FamiliasPatentes VALUES (NEWID(), @IdRolGerente, @IdPatenteStockBajo)

IF NOT EXISTS (SELECT 1 FROM FamiliasPatentes WHERE IdFamilia = @IdRolGerente AND IdPatente = @IdPatenteProductosMasVendidos)
    INSERT INTO FamiliasPatentes VALUES (NEWID(), @IdRolGerente, @IdPatenteProductosMasVendidos)

PRINT '? Permisos asignados a Administrador y Gerente'
GO
```

---

## 3?? **VERIFICACIÓN DE PERMISOS**

### Script para Verificar la Configuración

```sql
-- ============================================
-- Verificar permisos de reportes por rol
-- ============================================

USE [Distribuidora_Los_Amigos]
GO

SELECT 
    f.Nombre AS 'Rol',
    p.Nombre AS 'Patente',
    p.Descripcion AS 'Descripción',
    CASE 
        WHEN fp.IdFamiliaPatente IS NOT NULL THEN '? Asignado'
        ELSE '? No asignado'
    END AS 'Estado'
FROM Familias f
CROSS JOIN Patentes p
LEFT JOIN FamiliasPatentes fp ON f.IdFamilia = fp.IdFamilia AND p.IdPatente = fp.IdPatente
WHERE p.Nombre IN ('REPORTE_STOCK_BAJO', 'REPORTE_PRODUCTOS_MAS_VENDIDOS')
ORDER BY f.Nombre, p.Nombre
GO

-- Verificar usuarios con acceso
SELECT 
    u.UserName AS 'Usuario',
    f.Nombre AS 'Rol',
    p.Nombre AS 'Patente'
FROM Usuarios u
INNER JOIN UsuariosFamilias uf ON u.IdUsuario = uf.IdUsuario
INNER JOIN Familias f ON uf.IdFamilia = f.IdFamilia
INNER JOIN FamiliasPatentes fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patentes p ON fp.IdPatente = p.IdPatente
WHERE p.Nombre IN ('REPORTE_STOCK_BAJO', 'REPORTE_PRODUCTOS_MAS_VENDIDOS')
ORDER BY u.UserName, p.Nombre
GO
```

---

## 4?? **CONFIGURACIÓN EN EL CÓDIGO**

### Mapeo de Patentes en `AccesoService.cs`

Asegúrate de que el servicio de acceso mapee correctamente las patentes con los controles del menú:

```csharp
// Dentro del método que configura los permisos de menú
private static readonly Dictionary<string, string> MenuPatentesMap = new Dictionary<string, string>
{
    // ... mapeos existentes ...
    
    // ?? Nuevos reportes
    ["reporteStockBajoToolStripMenuItem"] = "REPORTE_STOCK_BAJO",
    ["reporteProductosMasVendidosToolStripMenuItem"] = "REPORTE_PRODUCTOS_MAS_VENDIDOS"
};
```

---

## 5?? **TESTING**

### Checklist de Pruebas

- [ ] **Crear Patentes**
  - [ ] Ejecutar script de creación de patentes
  - [ ] Verificar que las patentes existen en la tabla `Patentes`

- [ ] **Asignar Permisos**
  - [ ] Asignar patentes a roles correspondientes
  - [ ] Verificar con script de verificación

- [ ] **Pruebas de Usuario**
  - [ ] Iniciar sesión con usuario que tiene permisos
  - [ ] Verificar que las opciones del menú son visibles
  - [ ] Abrir ambos reportes
  - [ ] Verificar carga de datos correcta

- [ ] **Pruebas de Restricción**
  - [ ] Iniciar sesión con usuario sin permisos
  - [ ] Verificar que las opciones del menú NO son visibles
  - [ ] Intentar acceso directo (debería estar bloqueado)

- [ ] **Pruebas de Idioma**
  - [ ] Cambiar a Inglés - verificar traducciones
  - [ ] Cambiar a Portugués - verificar traducciones
  - [ ] Cambiar a Español - verificar traducciones

- [ ] **Pruebas de Funcionalidad**
  - [ ] **Reporte Stock Bajo:**
    - [ ] Cambiar umbral de stock
    - [ ] Verificar estadísticas
    - [ ] Exportar a CSV
  - [ ] **Reporte Productos Más Vendidos:**
    - [ ] Cambiar filtro de fechas
    - [ ] Modificar top de productos
    - [ ] Verificar estadísticas
    - [ ] Exportar a CSV

---

## 6?? **TROUBLESHOOTING**

### Problema: Las opciones del menú no aparecen

**Solución**:
1. Verificar que las patentes existen en la base de datos
2. Verificar que las patentes están asignadas al rol del usuario
3. Verificar que el usuario tiene el rol asignado
4. Reiniciar la aplicación

### Problema: Error al abrir el reporte

**Posibles causas**:
- No hay datos en la base de datos
- Error de conexión a la base de datos
- Permisos insuficientes en SQL Server

**Solución**:
1. Verificar logs en `LoggerService`
2. Verificar conexión a base de datos
3. Verificar que existen pedidos y productos

### Problema: Las traducciones no funcionan

**Solución**:
1. Verificar que el archivo `LanguajeRepository.cs` tiene las traducciones
2. Verificar que los controles tienen la propiedad `Tag` configurada
3. Reiniciar la aplicación

---

## 7?? **MANTENIMIENTO FUTURO**

### Agregar Nuevos Reportes

1. **Crear el formulario** en `Forms/Reportes/`
2. **Agregar método Click** en `main.cs`
3. **Agregar opción de menú** en `main.Designer.cs`
4. **Crear patente** en base de datos
5. **Asignar patente** a roles
6. **Agregar traducciones** en `LanguajeRepository.cs`
7. **Mapear en AccesoService** (si aplica)
8. **Testing completo**

### Modificar Permisos Existentes

```sql
-- Revocar permiso de un rol
DELETE FROM FamiliasPatentes
WHERE IdFamilia = (SELECT IdFamilia FROM Familias WHERE Nombre = 'NombreRol')
AND IdPatente = (SELECT IdPatente FROM Patentes WHERE Nombre = 'REPORTE_STOCK_BAJO')

-- Asignar permiso a un rol
INSERT INTO FamiliasPatentes (IdFamiliaPatente, IdFamilia, IdPatente)
VALUES (
    NEWID(),
    (SELECT IdFamilia FROM Familias WHERE Nombre = 'NombreRol'),
    (SELECT IdPatente FROM Patentes WHERE Nombre = 'REPORTE_STOCK_BAJO')
)
```

---

## 8?? **DOCUMENTACIÓN TÉCNICA**

### Estructura de Archivos

```
Distribuidora_los_amigos/
??? Forms/
?   ??? Reportes/
?   ?   ??? ReporteStockBajoForm.cs
?   ?   ??? ReporteStockBajoForm.Designer.cs
?   ?   ??? ReporteProductosMasVendidosForm.cs
?   ?   ??? ReporteProductosMasVendidosForm.Designer.cs
?   ??? main.cs (métodos Click agregados)
??? Service/
?   ??? DAL/
?       ??? Implementations/
?           ??? SqlServer/
?               ??? LanguajeRepository.cs (traducciones)
??? Docs/
    ??? CONFIGURACION_REPORTES.md (este archivo)
```

### Dependencias

- **BLL**: `StockService`, `PedidoService`, `DetallePedidoService`, `ProductoService`
- **DOMAIN**: `Stock`, `StockDTO`, `Pedido`, `DetallePedido`, `DetallePedidoDTO`, `Producto`
- **Service**: `IdiomaService`, `LoggerService`, `AccesoService`
- **Syncfusion**: `SfDataGrid`, `GridTextColumn`, `GridNumericColumn`

---

## ?? **SOPORTE**

Si encuentras problemas durante la configuración:

1. **Revisa los logs**: `LoggerService.WriteException(ex)`
2. **Verifica la base de datos**: Usa los scripts de verificación
3. **Consulta la documentación**: Este archivo y otros en `/Docs`
4. **Contacta soporte técnico**: Con capturas de pantalla y mensajes de error

---

**Última actualización**: 2024  
**Versión**: 1.0  
**Autor**: Sistema de Reportes - Distribuidora Los Amigos
