# ? SOLUCIÓN RÁPIDA: adminNico solo ve BÚSQUEDA

## ?? PROBLEMA SOLUCIONADO

Los scripts tenían errores porque las tablas `Familia_Patente` y `Usuario_Familia` **NO tienen columnas de ID propias** (como `IdFamiliaPatente` o `IdUsuarioFamilia`). 

Son tablas de relación **muchos a muchos** que solo contienen las claves foráneas.

---

## ? SCRIPTS CORREGIDOS

Los siguientes scripts han sido corregidos:

1. ? `08_Asignar_Todas_Patentes_Administrador.sql`
2. ? `06_Probar_Control_Acceso_Granular.sql`

---

## ?? EJECUTA ESTO AHORA

Copia y pega este script **corregido** en SQL Server Management Studio:

```sql
USE [Login]
GO

-- 1. Asignar TODAS las patentes a Administrador
DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER
SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'Administrador'

-- ? CORREGIDO: Sin IdFamiliaPatente
INSERT INTO Familia_Patente (IdFamilia, IdPatente)
SELECT @IdFamiliaAdmin, p.IdPatente
FROM Patente p
WHERE p.IdPatente NOT IN (
    SELECT fp.IdPatente
    FROM Familia_Patente fp
    WHERE fp.IdFamilia = @IdFamiliaAdmin
)

PRINT '? Patentes asignadas a Administrador: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- 2. Asignar familia a adminNico
DECLARE @IdUsuarioAdminNico UNIQUEIDENTIFIER
SELECT @IdUsuarioAdminNico = IdUsuario FROM Usuario WHERE UserName = 'adminNico'

-- Eliminar familias anteriores
DELETE FROM Usuario_Familia WHERE IdUsuario = @IdUsuarioAdminNico

-- ? CORREGIDO: Sin IdUsuarioFamilia
INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
VALUES (@IdUsuarioAdminNico, @IdFamiliaAdmin)

PRINT '? Familia Administrador asignada a adminNico'

-- 3. Verificar resultado
SELECT 
    u.UserName AS Usuario,
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
PRINT '============================================'
PRINT '? LISTO!'
PRINT '1. Cierra la aplicación'
PRINT '2. Vuelve a abrirla'
PRINT '3. Inicia sesión con adminNico'
PRINT '4. Deberías ver TODOS los menús'
PRINT '============================================'
GO
```

---

## ?? RESULTADO ESPERADO

Deberías ver algo como:

```
? Patentes asignadas a Administrador: 0
? Familia Administrador asignada a adminNico

Usuario     Familia         TotalPatentes
-------     -----------     -------------
adminNico   Administrador   22
```

Si `TotalPatentes = 22` ? ? **CORRECTO**

Si `TotalPatentes < 22` ? ? Ejecuta el script de nuevo

---

## ?? DESPUÉS DE EJECUTAR

1. **CIERRA** la aplicación por completo
2. **ABRE** la aplicación nuevamente
3. **INICIA SESIÓN** con `adminNico`
4. ? **DEBERÍAS VER TODOS LOS MENÚS:**
   - PEDIDOS
   - CLIENTE
   - PRODUCTOS
   - STOCK
   - BÚSQUEDA
   - REPORTES
   - GESTIÓN DE USUARIOS
   - PROVEEDORES
   - BACKUP Y RESTORE

---

## ?? NOTA TÉCNICA

### ? **INCORRECTO** (lo que tenía antes):
```sql
INSERT INTO Familia_Patente (IdFamiliaPatente, IdFamilia, IdPatente)
VALUES (NEWID(), @IdFamilia, @IdPatente)
```

### ? **CORRECTO** (lo que tiene ahora):
```sql
INSERT INTO Familia_Patente (IdFamilia, IdPatente)
VALUES (@IdFamilia, @IdPatente)
```

**Razón:** La tabla `Familia_Patente` es una tabla de relación que **NO tiene una columna de ID propia**. Solo tiene:
- `IdFamilia` (FK hacia Familia)
- `IdPatente` (FK hacia Patente)

Lo mismo aplica para `Usuario_Familia`:
- `IdUsuario` (FK hacia Usuario)
- `IdFamilia` (FK hacia Familia)

---

**Última actualización:** 2024  
**Estado:** ? Scripts corregidos y listos para ejecutar
