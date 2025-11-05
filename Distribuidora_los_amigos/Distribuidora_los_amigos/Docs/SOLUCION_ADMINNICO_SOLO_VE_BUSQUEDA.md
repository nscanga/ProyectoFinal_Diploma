# ?? SOLUCIÓN: Usuario adminNico solo ve "BÚSQUEDA"

## ? PROBLEMA

El usuario `adminNico` con rol "Administrador" solo ve el menú **"BÚSQUEDA"** y ningún otro menú.

---

## ?? CAUSA PROBABLE

El rol "Administrador" **NO tiene todas las patentes asignadas** en la base de datos.

El menú "BÚSQUEDA" no requiere patentes específicas (por eso es el único visible), pero todos los demás menús SÍ requieren patentes:
- PEDIDOS ? Requiere `CREAR_PEDIDO`, `MOSTRAR_PEDIDOS`
- CLIENTE ? Requiere `Crear_cliente`, `Mostrar_clientes`
- PRODUCTOS ? Requiere `AGREGAR`, `MODIFICAR`, `VER_PRODUCTOS`
- etc.

---

## ? SOLUCIÓN PASO A PASO

### **Paso 1: Diagnosticar el problema**

Ejecuta en SQL Server Management Studio:

```sql
-- Archivo: 07_Diagnosticar_Usuario_AdminNico.sql
```

Este script mostrará:
- ? Si el usuario existe
- ? Si tiene familia asignada
- ? Cuántas patentes tiene la familia "Administrador"
- ? Qué patentes faltan

---

### **Paso 2: Asignar todas las patentes**

Ejecuta en SQL Server Management Studio:

```sql
-- Archivo: 08_Asignar_Todas_Patentes_Administrador.sql
```

Este script:
1. ? Verifica que la familia "Administrador" existe
2. ? Asigna **TODAS las 22 patentes** a la familia "Administrador"
3. ? Verifica que adminNico tenga la familia asignada
4. ? Muestra un resumen final

---

### **Paso 3: Reiniciar sesión**

1. **Cerrar sesión** de la aplicación (si está abierta)
2. **Volver a iniciar sesión** con `adminNico`
3. **Verificar** que ahora vea todos los menús

---

## ?? RESULTADO ESPERADO

Después de ejecutar los scripts, `adminNico` debería ver:

```
? PEDIDOS
   ? Crear Pedido
   ? Mostrar Pedidos

? CLIENTE
   ? Crear Cliente
   ? Mostrar Clientes

? PRODUCTOS
   ? Agregar
   ? Modificar
   ? Eliminar
   ? Ver Productos

? STOCK
   ? Mostrar Stock

? BÚSQUEDA
   ? (opciones de búsqueda)

? REPORTES
   ? Reporte Stock Bajo
   ? Productos Más Vendidos

? GESTIÓN DE USUARIOS
   ? Crear Usuario
   ? Ver Usuarios
   ? Asignar Rol
   ? Modificar Usuario
   ? Crear Rol
   ? Crear Patente

? PROVEEDORES
   ? Mostrar Proveedores
   ? Modificar Proveedor
   ? Crear Proveedor

? BACKUP Y RESTORE
   ? Generar Backup
   ? Restaurar Backup
```

---

## ??? SOLUCIÓN RÁPIDA (COPIAR Y PEGAR)

Si quieres solucionarlo INMEDIATAMENTE sin ejecutar los scripts de diagnóstico, copia y pega esto en SQL Server:

```sql
USE [Login]
GO

-- Asignar TODAS las patentes a Administrador
DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER
SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'Administrador'

INSERT INTO Familia_Patente (IdFamiliaPatente, IdFamilia, IdPatente)
SELECT 
    NEWID(),
    @IdFamiliaAdmin,
    p.IdPatente
FROM Patente p
WHERE p.IdPatente NOT IN (
    SELECT fp.IdPatente
    FROM Familia_Patente fp
    WHERE fp.IdFamilia = @IdFamiliaAdmin
)

PRINT '? Patentes asignadas a Administrador'

-- Verificar que adminNico tenga la familia
DECLARE @IdUsuarioAdminNico UNIQUEIDENTIFIER
SELECT @IdUsuarioAdminNico = IdUsuario FROM Usuario WHERE UserName = 'adminNico'

IF NOT EXISTS (
    SELECT 1 FROM Usuario_Familia 
    WHERE IdUsuario = @IdUsuarioAdminNico 
    AND IdFamilia = @IdFamiliaAdmin
)
BEGIN
    DELETE FROM Usuario_Familia WHERE IdUsuario = @IdUsuarioAdminNico
    
    INSERT INTO Usuario_Familia (IdUsuarioFamilia, IdUsuario, IdFamilia)
    VALUES (NEWID(), @IdUsuarioAdminNico, @IdFamiliaAdmin)
    
    PRINT '? Familia asignada a adminNico'
END

-- Verificar resultado
SELECT 
    u.UserName,
    f.Nombre AS Familia,
    COUNT(DISTINCT p.IdPatente) AS TotalPatentes
FROM Usuario u
INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE u.UserName = 'adminNico'
GROUP BY u.UserName, f.Nombre

PRINT ''
PRINT '? Listo! Ahora cierra sesión y vuelve a iniciar con adminNico'
GO
```

---

## ?? DESPUÉS DE EJECUTAR EL SCRIPT

### **IMPORTANTE:**
1. **Cierra la aplicación** completamente
2. **Vuelve a abrirla**
3. **Inicia sesión** con `adminNico`
4. **Deberías ver TODOS los menús**

---

## ?? SI SIGUE SIN FUNCIONAR

Si después de ejecutar el script y reiniciar la sesión **sigue viendo solo BÚSQUEDA**, puede haber un problema en el código C#.

Verifica en el código:

### **Archivo: `main.cs` ? Método `MainForm_Load`**

Debería tener estas líneas:

```csharp
List<Patente> patentesDelUsuario = usuarioLogueado.GetPatentes();
ConfigurarAccesosMenu(patentesDelUsuario);
```

Si no las tiene o están comentadas, agrégalas.

---

## ?? VERIFICACIÓN FINAL

Ejecuta esta consulta para confirmar que todo está correcto:

```sql
-- Ver patentes de adminNico
SELECT 
    u.UserName,
    f.Nombre AS Familia,
    p.Nombre AS Patente,
    p.TipoAcceso
FROM Usuario u
INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE u.UserName = 'adminNico'
ORDER BY p.TipoAcceso, p.Nombre
```

**Resultado esperado:** Deberías ver **22 filas** (una por cada patente).

---

## ?? ARCHIVOS DE AYUDA

| Script | Descripción |
|--------|-------------|
| `07_Diagnosticar_Usuario_AdminNico.sql` | Diagnóstico completo del problema |
| `08_Asignar_Todas_Patentes_Administrador.sql` | Solución automática |

---

**Última actualización:** 2024  
**Estado:** Pendiente de ejecución
