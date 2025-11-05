-- ============================================
-- SCRIPT DE LIMPIEZA Y REORGANIZACIÓN (CORREGIDO)
-- ============================================
-- Base de datos: Login
-- Propósito: Limpiar datos de prueba y reorganizar el sistema de permisos
-- NOTA: La tabla Usuario_Familia solo tiene IdUsuario e IdFamilia (sin ID propio)
-- ============================================

USE [Login]
GO

PRINT '============================================'
PRINT 'LIMPIEZA Y REORGANIZACIÓN DE LA BASE DE DATOS'
PRINT '============================================'
PRINT ''

-- ============================================
-- PARTE 1: BACKUP DE SEGURIDAD (IMPORTANTE)
-- ============================================

PRINT 'IMPORTANTE: Antes de ejecutar este script, haz un backup de tu base de datos'
PRINT '¿Estás seguro de que quieres continuar? Comenta esta línea para proceder:'
PRINT ''
-- RAISERROR('Script detenido. Descomenta para continuar.', 16, 1)
-- RETURN
GO

-- ============================================
-- PARTE 2: ELIMINAR USUARIOS DE PRUEBA
-- ============================================

PRINT ''
PRINT '--------------------------------------------'
PRINT 'PASO 1: ELIMINAR USUARIOS DE PRUEBA'
PRINT '--------------------------------------------'
PRINT ''

-- Lista de usuarios a eliminar (usuarios de prueba sin asignaciones importantes)
DECLARE @UsuariosEliminar TABLE (UserName NVARCHAR(100))

INSERT INTO @UsuariosEliminar VALUES 
    ('admin1'),
    ('admin5'),
    ('Juan'),
    ('ddddd'),
    ('toroo'),
    ('RamirV')

-- Eliminar relaciones Usuario_Familia
DELETE uf
FROM Usuario_Familia uf
INNER JOIN Usuario u ON uf.IdUsuario = u.IdUsuario
WHERE u.UserName IN (SELECT UserName FROM @UsuariosEliminar)

PRINT '? Relaciones Usuario_Familia eliminadas'

-- Eliminar usuarios
DELETE FROM Usuario
WHERE UserName IN (SELECT UserName FROM @UsuariosEliminar)

PRINT '? Usuarios de prueba eliminados'
PRINT ''

SELECT 
    UserName AS 'Usuario',
    Email,
    Estado
FROM Usuario
ORDER BY UserName

PRINT ''

-- ============================================
-- PARTE 3: ELIMINAR FAMILIAS VACÍAS
-- ============================================

PRINT ''
PRINT '--------------------------------------------'
PRINT 'PASO 2: ELIMINAR FAMILIAS SIN PATENTES'
PRINT '--------------------------------------------'
PRINT ''

-- Lista de familias a eliminar (sin patentes asignadas)
DECLARE @FamiliasEliminar TABLE (Nombre NVARCHAR(100))

INSERT INTO @FamiliasEliminar VALUES 
    ('AdminMM'),
    ('asd'),
    ('Gerente'),
    ('PruebaRol'),
    ('Reportes'),
    ('RolUi'),
    ('Vendedor'),
    ('WorkerMM')

-- Eliminar relaciones Usuario_Familia
DELETE uf
FROM Usuario_Familia uf
INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
WHERE f.Nombre IN (SELECT Nombre FROM @FamiliasEliminar)

PRINT '? Relaciones Usuario_Familia con familias vacías eliminadas'

-- Eliminar familias
DELETE FROM Familia
WHERE Nombre IN (SELECT Nombre FROM @FamiliasEliminar)

PRINT '? Familias vacías eliminadas'
PRINT ''

SELECT 
    Nombre AS 'Familia',
    IdFamilia
FROM Familia
ORDER BY Nombre

PRINT ''

-- ============================================
-- PARTE 4: ASIGNAR USUARIOS RESTANTES
-- ============================================

PRINT ''
PRINT '--------------------------------------------'
PRINT 'PASO 3: ASIGNAR USUARIOS A FAMILIAS CORRECTAS'
PRINT '--------------------------------------------'
PRINT ''

DECLARE @IdAdministrador UNIQUEIDENTIFIER
DECLARE @IdUI UNIQUEIDENTIFIER

SELECT @IdAdministrador = IdFamilia FROM Familia WHERE Nombre = 'Administrador'
SELECT @IdUI = IdFamilia FROM Familia WHERE Nombre = 'UI'

-- Limpiar asignaciones incorrectas
DELETE FROM Usuario_Familia
WHERE IdFamilia NOT IN (@IdAdministrador, @IdUI)

PRINT '? Asignaciones incorrectas eliminadas'

-- Asignar 'admin' a Administrador (si no está asignado)
DECLARE @IdAdmin UNIQUEIDENTIFIER
SELECT @IdAdmin = IdUsuario FROM Usuario WHERE UserName = 'admin'

IF @IdAdmin IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuario_Familia WHERE IdUsuario = @IdAdmin AND IdFamilia = @IdAdministrador)
    BEGIN
        INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
        VALUES (@IdAdmin, @IdAdministrador)
        PRINT '? Usuario ''admin'' asignado a Administrador'
    END
    ELSE
        PRINT '? Usuario ''admin'' ya está asignado a Administrador'
END

-- Asignar 'adminNico' a Administrador (ya debería estar)
DECLARE @IdAdminNico UNIQUEIDENTIFIER
SELECT @IdAdminNico = IdUsuario FROM Usuario WHERE UserName = 'adminNico'

IF @IdAdminNico IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuario_Familia WHERE IdUsuario = @IdAdminNico AND IdFamilia = @IdAdministrador)
    BEGIN
        INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
        VALUES (@IdAdminNico, @IdAdministrador)
        PRINT '? Usuario ''adminNico'' asignado a Administrador'
    END
    ELSE
        PRINT '? Usuario ''adminNico'' ya está asignado a Administrador'
END

-- Asignar 'admin2' a UI
DECLARE @IdAdmin2 UNIQUEIDENTIFIER
SELECT @IdAdmin2 = IdUsuario FROM Usuario WHERE UserName = 'admin2'

IF @IdAdmin2 IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuario_Familia WHERE IdUsuario = @IdAdmin2 AND IdFamilia = @IdUI)
    BEGIN
        INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
        VALUES (@IdAdmin2, @IdUI)
        PRINT '? Usuario ''admin2'' asignado a UI'
    END
    ELSE
        PRINT '? Usuario ''admin2'' ya está asignado a UI'
END

-- Asignar 'adminadmin' a Administrador
DECLARE @IdAdminadmin UNIQUEIDENTIFIER
SELECT @IdAdminadmin = IdUsuario FROM Usuario WHERE UserName = 'adminadmin'

IF @IdAdminadmin IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuario_Familia WHERE IdUsuario = @IdAdminadmin AND IdFamilia = @IdAdministrador)
    BEGIN
        INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
        VALUES (@IdAdminadmin, @IdAdministrador)
        PRINT '? Usuario ''adminadmin'' asignado a Administrador'
    END
    ELSE
        PRINT '? Usuario ''adminadmin'' ya está asignado a Administrador'
END

-- Asignar 'Agustin123' a UI
DECLARE @IdAgustin UNIQUEIDENTIFIER
SELECT @IdAgustin = IdUsuario FROM Usuario WHERE UserName = 'Agustin123'

IF @IdAgustin IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuario_Familia WHERE IdUsuario = @IdAgustin AND IdFamilia = @IdUI)
    BEGIN
        INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
        VALUES (@IdAgustin, @IdUI)
        PRINT '? Usuario ''Agustin123'' asignado a UI'
    END
    ELSE
        PRINT '? Usuario ''Agustin123'' ya está asignado a UI'
END

PRINT ''

-- ============================================
-- PARTE 5: VERIFICACIÓN FINAL
-- ============================================

PRINT ''
PRINT '--------------------------------------------'
PRINT 'VERIFICACIÓN FINAL'
PRINT '--------------------------------------------'
PRINT ''

PRINT 'USUARIOS RESTANTES Y SUS FAMILIAS:'
PRINT ''

SELECT 
    u.UserName AS 'Usuario',
    STRING_AGG(f.Nombre, ', ') AS 'Familias',
    u.Estado AS 'Estado'
FROM Usuario u
LEFT JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
LEFT JOIN Familia f ON uf.IdFamilia = f.IdFamilia
GROUP BY u.UserName, u.Estado
ORDER BY u.UserName

PRINT ''
PRINT 'FAMILIAS RESTANTES Y SUS PATENTES:'
PRINT ''

SELECT 
    f.Nombre AS 'Familia',
    COUNT(DISTINCT fp.IdPatente) AS 'Total Patentes',
    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS 'UI',
    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS 'Control'
FROM Familia f
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
GROUP BY f.Nombre
ORDER BY f.Nombre

PRINT ''
PRINT '============================================'
PRINT 'LIMPIEZA COMPLETADA'
PRINT '============================================'
PRINT ''
PRINT 'RESUMEN:'
PRINT '- Usuarios de prueba eliminados'
PRINT '- Familias vacías eliminadas'
PRINT '- Usuarios restantes asignados a familias correctas'
PRINT ''
PRINT 'FAMILIAS FINALES:'
PRINT '- Administrador: Acceso completo (UI + Control)'
PRINT '- UI: Acceso de visualización'
PRINT ''
PRINT 'USUARIOS ADMINISTRADORES:'
PRINT '- admin'
PRINT '- adminNico'
PRINT '- adminadmin'
PRINT ''
PRINT 'USUARIOS CON ROL UI:'
PRINT '- admin2'
PRINT '- Agustin123'
PRINT ''
PRINT '? Sistema limpio y organizado'
PRINT '============================================'
GO
