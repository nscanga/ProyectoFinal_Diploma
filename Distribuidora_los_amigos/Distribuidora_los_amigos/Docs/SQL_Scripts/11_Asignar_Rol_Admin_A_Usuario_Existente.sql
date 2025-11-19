-- =============================================
-- Script: Asignar Rol Administrador a Usuario Admin Existente
-- Base de datos: Login
-- Descripción: Asigna el rol de Administrador al usuario admin si no lo tiene
--              Y asegura que la familia Administrador tenga TODAS las patentes
--              Este script soluciona el problema de usuarios creados sin rol
--              o familias sin patentes asignadas
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

DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER
DECLARE @FamiliaCreada BIT = 0

IF NOT EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'Administrador')
BEGIN
    PRINT '??  La familia "Administrador" NO EXISTE'
    PRINT '?? Creando familia Administrador...'
    
    SET @IdFamiliaAdmin = NEWID()
    
    INSERT INTO Familia (IdFamilia, Nombre)
    VALUES (@IdFamiliaAdmin, 'Administrador')
    
    PRINT '? Familia "Administrador" creada con ID: ' + CAST(@IdFamiliaAdmin AS VARCHAR(50))
    SET @FamiliaCreada = 1
END
ELSE
BEGIN
    PRINT '? Familia "Administrador" existe'
    SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'Administrador'
    SET @FamiliaCreada = 0
END
PRINT ''
GO

-- ==================================================
-- PASO 3: Verificar y asignar TODAS las patentes
-- ==================================================
PRINT '3?? VERIFICAR PATENTES DE LA FAMILIA'
PRINT '===================================='

DECLARE @IdFamiliaAdminExistente UNIQUEIDENTIFIER
SELECT @IdFamiliaAdminExistente = IdFamilia FROM Familia WHERE Nombre = 'Administrador'

-- Contar patentes actuales de la familia
DECLARE @CantidadPatentesActuales INT
SELECT @CantidadPatentesActuales = COUNT(*)
FROM Familia_Patente
WHERE IdFamilia = @IdFamiliaAdminExistente

-- Contar total de patentes en el sistema
DECLARE @TotalPatentesDisponibles INT
SELECT @TotalPatentesDisponibles = COUNT(*) FROM Patente

PRINT '?? Patentes actuales de Administrador: ' + CAST(@CantidadPatentesActuales AS VARCHAR(10))
PRINT '?? Total de patentes disponibles: ' + CAST(@TotalPatentesDisponibles AS VARCHAR(10))
PRINT ''

IF @TotalPatentesDisponibles = 0
BEGIN
    PRINT '??  ADVERTENCIA CRÍTICA: No hay patentes en el sistema'
    PRINT '?? El administrador tendrá acceso limitado hasta que se creen patentes'
    PRINT '?? Ejecute los scripts de creación de patentes o cree patentes desde la aplicación'
    PRINT ''
END
ELSE IF @CantidadPatentesActuales = 0
BEGIN
    PRINT '??  La familia Administrador NO tiene patentes asignadas'
    PRINT '?? Asignando TODAS las patentes disponibles...'
    
    INSERT INTO Familia_Patente (IdFamilia, IdPatente)
    SELECT @IdFamiliaAdminExistente, IdPatente FROM Patente
    
    DECLARE @PatentesAsignadas INT = @@ROWCOUNT
    PRINT '? ' + CAST(@PatentesAsignadas AS VARCHAR(10)) + ' patentes asignadas a Administrador'
    PRINT ''
END
ELSE IF @CantidadPatentesActuales < @TotalPatentesDisponibles
BEGIN
    PRINT '??  La familia Administrador solo tiene ' + CAST(@CantidadPatentesActuales AS VARCHAR(10)) + ' de ' + CAST(@TotalPatentesDisponibles AS VARCHAR(10)) + ' patentes'
    PRINT '?? Asignando patentes faltantes...'
    
    -- Insertar solo las patentes que faltan
    INSERT INTO Familia_Patente (IdFamilia, IdPatente)
    SELECT @IdFamiliaAdminExistente, p.IdPatente
    FROM Patente p
    WHERE p.IdPatente NOT IN (
        SELECT IdPatente 
        FROM Familia_Patente 
        WHERE IdFamilia = @IdFamiliaAdminExistente
    )
    
    SET @PatentesAsignadas = @@ROWCOUNT
    PRINT '? ' + CAST(@PatentesAsignadas AS VARCHAR(10)) + ' patentes adicionales asignadas'
    PRINT ''
END
ELSE
BEGIN
    PRINT '? Familia Administrador tiene TODAS las patentes asignadas (' + CAST(@CantidadPatentesActuales AS VARCHAR(10)) + ')'
    PRINT ''
END
GO

-- ==================================================
-- PASO 4: Verificar asignación del usuario
-- ==================================================
PRINT '4?? VERIFICAR ASIGNACIÓN ACTUAL DEL USUARIO'
PRINT '===================================='

DECLARE @IdUsuarioAdmin UNIQUEIDENTIFIER
DECLARE @IdFamilia UNIQUEIDENTIFIER

SELECT @IdUsuarioAdmin = IdUsuario FROM Usuario WHERE UserName = 'admin'
SELECT @IdFamilia = IdFamilia FROM Familia WHERE Nombre = 'Administrador'

IF EXISTS (
    SELECT 1 
    FROM Usuario_Familia 
    WHERE IdUsuario = @IdUsuarioAdmin AND IdFamilia = @IdFamilia
)
BEGIN
    PRINT '? El usuario "admin" YA TIENE el rol de Administrador asignado'
END
ELSE
BEGIN
    PRINT '??  El usuario "admin" NO TIENE el rol de Administrador asignado'
END
PRINT ''
GO

-- ==================================================
-- PASO 5: Asignar familia Administrador al usuario admin
-- ==================================================
PRINT '5?? ASIGNAR ROL AL USUARIO'
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
    
    -- Opcional: Eliminar cualquier otra familia asignada
    -- Descomenta estas líneas si quieres que admin solo tenga el rol de Administrador
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
-- PASO 6: Verificación final detallada
-- ==================================================
PRINT '6?? VERIFICACIÓN FINAL COMPLETA'
PRINT '===================================='
PRINT ''

-- Información del usuario
PRINT '?? USUARIO ADMIN:'
SELECT 
    u.UserName AS Usuario,
    u.Email AS Email,
    CASE u.Estado 
        WHEN 1 THEN 'Habilitado' 
        ELSE 'Deshabilitado' 
    END AS Estado
FROM Usuario u
WHERE u.UserName = 'admin'

PRINT ''
PRINT '?? ROL ASIGNADO:'
SELECT 
    u.UserName AS Usuario,
    f.Nombre AS Familia
FROM Usuario u
INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
WHERE u.UserName = 'admin'

PRINT ''
PRINT '?? PATENTES DISPONIBLES:'
SELECT 
    f.Nombre AS Familia,
    COUNT(DISTINCT p.IdPatente) AS TotalPatentes,
    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS Patentes_UI,
    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS Patentes_Control,
    SUM(CASE WHEN p.TipoAcceso = 2 THEN 1 ELSE 0 END) AS Patentes_UseCases
FROM Familia f
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Administrador'
GROUP BY f.Nombre

PRINT ''
PRINT '?? DETALLE DE PATENTES:'
SELECT 
    ROW_NUMBER() OVER (ORDER BY p.TipoAcceso, p.Nombre) AS '#',
    p.Nombre AS Patente,
    CASE p.TipoAcceso
        WHEN 0 THEN 'UI (Visualización)'
        WHEN 1 THEN 'Control (Administración)'
        WHEN 2 THEN 'UseCases (Casos especiales)'
        ELSE 'Desconocido'
    END AS TipoAcceso
FROM Familia f
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Administrador'
ORDER BY p.TipoAcceso, p.Nombre

PRINT ''
GO

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
PRINT '4?? Verifique que puede ver los menús del sistema'
PRINT ''
PRINT '?? MENÚS ESPERADOS (si hay patentes):'
PRINT '   ? PEDIDOS'
PRINT '   ? CLIENTES'
PRINT '   ? PRODUCTOS'
PRINT '   ? STOCK'
PRINT '   ? BÚSQUEDA'
PRINT '   ? REPORTES'
PRINT '   ? GESTIÓN DE USUARIOS'
PRINT '   ? PROVEEDORES'
PRINT '   ? BACKUP Y RESTORE'
PRINT ''
PRINT '??  IMPORTANTE:'
PRINT '   • Cambie la contraseña del usuario admin después del primer inicio de sesión'
PRINT '   • Si no ve todos los menús, verifique que existan patentes en la base de datos'
PRINT '   • Consulte los logs de la aplicación en C:\Logs\ para más detalles'
PRINT ''
PRINT '============================================'
GO
