-- =============================================
-- Script: Asignar Rol Administrador a Usuario Admin Existente
-- Base de datos: Login
-- Descripción: Asigna el rol de Administrador al usuario admin si no lo tiene
--              Este script soluciona el problema de usuarios creados sin rol
-- =============================================

USE [Login]
GO

PRINT '============================================'
PRINT 'ASIGNAR ROL ADMINISTRADOR A USUARIO ADMIN'
PRINT '============================================'
PRINT ''

-- ==================================================
-- PASO 1: Verificar que el usuario admin existe
-- ==================================================
PRINT '1?? VERIFICAR USUARIO ADMIN'
PRINT '===================================='

IF NOT EXISTS (SELECT 1 FROM Usuario WHERE UserName = 'admin')
BEGIN
    PRINT '? El usuario "admin" NO EXISTE'
    PRINT '?? Ejecute el script 00_Crear_Usuario_Admin_Default.sql primero'
    PRINT ''
    PRINT '============================================'
    PRINT 'SCRIPT DETENIDO'
    PRINT '============================================'
    RETURN
END
ELSE
BEGIN
    PRINT '? Usuario "admin" existe'
END
PRINT ''
GO

-- ==================================================
-- PASO 2: Verificar o crear familia Administrador
-- ==================================================
PRINT '2?? VERIFICAR FAMILIA ADMINISTRADOR'
PRINT '===================================='

IF NOT EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'Administrador')
BEGIN
    PRINT '??  La familia "Administrador" NO EXISTE'
    PRINT '?? Creando familia Administrador...'
    
    DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER = NEWID()
    
    INSERT INTO Familia (IdFamilia, Nombre)
    VALUES (@IdFamiliaAdmin, 'Administrador')
    
    PRINT '? Familia "Administrador" creada con ID: ' + CAST(@IdFamiliaAdmin AS VARCHAR(50))
    
    -- Asignar TODAS las patentes a la familia Administrador
    PRINT '?? Asignando todas las patentes a Administrador...'
    
    INSERT INTO Familia_Patente (IdFamilia, IdPatente)
    SELECT @IdFamiliaAdmin, IdPatente FROM Patente
    
    DECLARE @PatentesAsignadas INT = @@ROWCOUNT
    PRINT '? ' + CAST(@PatentesAsignadas AS VARCHAR(10)) + ' patentes asignadas a Administrador'
END
ELSE
BEGIN
    PRINT '? Familia "Administrador" existe'
    
    -- Verificar que tenga patentes asignadas
    DECLARE @CantidadPatentes INT
    SELECT @CantidadPatentes = COUNT(*)
    FROM Familia_Patente fp
    INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
    WHERE f.Nombre = 'Administrador'
    
    IF @CantidadPatentes = 0
    BEGIN
        PRINT '??  La familia Administrador NO tiene patentes asignadas'
        PRINT '?? Asignando todas las patentes...'
        
        DECLARE @IdFamiliaAdminExistente UNIQUEIDENTIFIER
        SELECT @IdFamiliaAdminExistente = IdFamilia FROM Familia WHERE Nombre = 'Administrador'
        
        INSERT INTO Familia_Patente (IdFamilia, IdPatente)
        SELECT @IdFamiliaAdminExistente, IdPatente FROM Patente
        WHERE IdPatente NOT IN (
            SELECT IdPatente FROM Familia_Patente WHERE IdFamilia = @IdFamiliaAdminExistente
        )
        
        SET @PatentesAsignadas = @@ROWCOUNT
        PRINT '? ' + CAST(@PatentesAsignadas AS VARCHAR(10)) + ' patentes asignadas'
    END
    ELSE
    BEGIN
        PRINT '? Familia Administrador tiene ' + CAST(@CantidadPatentes AS VARCHAR(10)) + ' patentes asignadas'
    END
END
PRINT ''
GO

-- ==================================================
-- PASO 3: Verificar si el usuario ya tiene la familia
-- ==================================================
PRINT '3?? VERIFICAR ASIGNACIÓN ACTUAL'
PRINT '===================================='

DECLARE @IdUsuarioAdmin UNIQUEIDENTIFIER
DECLARE @IdFamilia UNIQUEIDENTIFIER
DECLARE @TieneFamilia BIT = 0

SELECT @IdUsuarioAdmin = IdUsuario FROM Usuario WHERE UserName = 'admin'
SELECT @IdFamilia = IdFamilia FROM Familia WHERE Nombre = 'Administrador'

IF EXISTS (
    SELECT 1 
    FROM Usuario_Familia 
    WHERE IdUsuario = @IdUsuarioAdmin AND IdFamilia = @IdFamilia
)
BEGIN
    PRINT '? El usuario "admin" YA TIENE el rol de Administrador asignado'
    SET @TieneFamilia = 1
END
ELSE
BEGIN
    PRINT '??  El usuario "admin" NO TIENE el rol de Administrador asignado'
    SET @TieneFamilia = 0
END
PRINT ''
GO

-- ==================================================
-- PASO 4: Asignar familia Administrador al usuario admin
-- ==================================================
PRINT '4?? ASIGNAR ROL'
PRINT '===================================='

DECLARE @IdUsuarioAdmin2 UNIQUEIDENTIFIER
DECLARE @IdFamilia2 UNIQUEIDENTIFIER

SELECT @IdUsuarioAdmin2 = IdUsuario FROM Usuario WHERE UserName = 'admin'
SELECT @IdFamilia2 = IdFamilia FROM Familia WHERE Nombre = 'Administrador'

IF NOT EXISTS (
    SELECT 1 
    FROM Usuario_Familia 
    WHERE IdUsuario = @IdUsuarioAdmin2 AND IdFamilia = @IdFamilia2
)
BEGIN
    PRINT '?? Asignando rol Administrador al usuario admin...'
    
    -- Primero, eliminar cualquier otra familia asignada (opcional)
    -- Descomentar las siguientes líneas si quieres que admin solo tenga el rol de Administrador
    /*
    DELETE FROM Usuario_Familia WHERE IdUsuario = @IdUsuarioAdmin2
    PRINT '?? Roles anteriores eliminados'
    */
    
    -- Asignar la familia Administrador
    INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
    VALUES (@IdUsuarioAdmin2, @IdFamilia2)
    
    PRINT '? Rol Administrador asignado exitosamente al usuario admin'
END
ELSE
BEGIN
    PRINT '? El usuario admin ya tiene el rol de Administrador'
END
PRINT ''
GO

-- ==================================================
-- PASO 5: Verificación final
-- ==================================================
PRINT '5?? VERIFICACIÓN FINAL'
PRINT '===================================='

SELECT 
    u.UserName AS Usuario,
    u.Email AS Email,
    CASE u.Estado 
        WHEN 1 THEN 'Habilitado' 
        ELSE 'Deshabilitado' 
    END AS Estado,
    f.Nombre AS Familia,
    COUNT(DISTINCT p.IdPatente) AS TotalPatentes
FROM Usuario u
LEFT JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
LEFT JOIN Familia f ON uf.IdFamilia = f.IdFamilia
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE u.UserName = 'admin'
GROUP BY u.UserName, u.Email, u.Estado, f.Nombre

PRINT ''
PRINT '============================================'
PRINT '? SCRIPT COMPLETADO EXITOSAMENTE'
PRINT '============================================'
PRINT ''
PRINT 'INSTRUCCIONES FINALES:'
PRINT '1?? Si la aplicación está abierta, ciérrela completamente'
PRINT '2?? Vuelva a iniciar la aplicación'
PRINT '3?? Inicie sesión con:'
PRINT '   Usuario: admin'
PRINT '   Contraseña: Admin123!'
PRINT '4?? Verifique que puede ver TODOS los menús del sistema'
PRINT ''
PRINT '??  IMPORTANTE: Cambie la contraseña del usuario admin'
PRINT '    después del primer inicio de sesión'
PRINT ''
PRINT '============================================'
GO
