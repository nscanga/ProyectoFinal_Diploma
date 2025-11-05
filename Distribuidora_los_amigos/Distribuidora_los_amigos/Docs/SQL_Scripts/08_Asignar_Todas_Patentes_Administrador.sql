-- ============================================
-- Script para ASIGNAR TODAS LAS PATENTES al rol Administrador
-- ============================================
-- Soluciona el problema de adminNico que solo ve "BÚSQUEDA"
-- ============================================

USE [Login]
GO

PRINT '============================================'
PRINT 'ASIGNAR TODAS LAS PATENTES A ADMINISTRADOR'
PRINT '============================================'
PRINT ''

-- ==================================================
-- PASO 1: Verificar familia Administrador
-- ==================================================
PRINT '1?? VERIFICAR FAMILIA ADMINISTRADOR'
PRINT '===================================='

IF NOT EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'Administrador')
BEGIN
    PRINT '? La familia "Administrador" NO EXISTE'
    PRINT '?? Creando familia...'
    
    INSERT INTO Familia (IdFamilia, Nombre)
    VALUES (NEWID(), 'Administrador')
    
    PRINT '? Familia "Administrador" creada'
END
ELSE
BEGIN
    PRINT '? Familia "Administrador" existe'
END
PRINT ''
GO

-- ==================================================
-- PASO 2: Contar patentes actuales
-- ==================================================
PRINT '2?? ESTADO ACTUAL'
PRINT '===================================='

DECLARE @PatentesActuales INT
SELECT @PatentesActuales = COUNT(DISTINCT fp.IdPatente)
FROM Familia f
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
WHERE f.Nombre = 'Administrador'

PRINT 'Patentes actuales en Administrador: ' + CAST(@PatentesActuales AS VARCHAR(10))
PRINT ''
GO

-- ==================================================
-- PASO 3: Asignar TODAS las patentes a Administrador
-- ==================================================
PRINT '3?? ASIGNANDO TODAS LAS PATENTES'
PRINT '===================================='

DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER
SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'Administrador'

-- Insertar todas las patentes que NO están asignadas
INSERT INTO Familia_Patente (IdFamilia, IdPatente)
SELECT 
    @IdFamiliaAdmin,
    p.IdPatente
FROM Patente p
WHERE p.IdPatente NOT IN (
    SELECT fp.IdPatente
    FROM Familia_Patente fp
    WHERE fp.IdFamilia = @IdFamiliaAdmin
)

DECLARE @PatentesAgregadas INT = @@ROWCOUNT

PRINT 'Patentes agregadas: ' + CAST(@PatentesAgregadas AS VARCHAR(10))
PRINT ''
GO

-- ==================================================
-- PASO 4: Verificar patentes finales
-- ==================================================
PRINT '4?? VERIFICACIÓN FINAL'
PRINT '===================================='

SELECT 
    f.Nombre AS Familia,
    COUNT(DISTINCT p.IdPatente) AS TotalPatentes,
    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS Patentes_UI,
    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS Patentes_Control
FROM Familia f
LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Administrador'
GROUP BY f.Nombre

PRINT ''
PRINT '?? LISTA COMPLETA DE PATENTES:'
PRINT '=============================='

SELECT 
    ROW_NUMBER() OVER (ORDER BY p.TipoAcceso, p.Nombre) AS '#',
    p.Nombre AS Patente,
    CASE p.TipoAcceso
        WHEN 0 THEN 'UI'
        WHEN 1 THEN 'Control'
        WHEN 2 THEN 'UseCases'
    END AS TipoAcceso
FROM Familia f
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Administrador'
ORDER BY p.TipoAcceso, p.Nombre

PRINT ''
GO

-- ==================================================
-- PASO 5: Asignar familia a adminNico (si no la tiene)
-- ==================================================
PRINT '5?? VERIFICAR USUARIO ADMINNICO'
PRINT '===================================='

IF EXISTS (SELECT 1 FROM Usuario WHERE UserName = 'adminNico')
BEGIN
    PRINT '? Usuario adminNico existe'
    
    -- Verificar si ya tiene la familia asignada
    IF NOT EXISTS (
        SELECT 1 
        FROM Usuario u
        INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
        INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
        WHERE u.UserName = 'adminNico' AND f.Nombre = 'Administrador'
    )
    BEGIN
        PRINT '??  Usuario NO tiene familia Administrador asignada'
        PRINT '?? Asignando familia...'
        
        DECLARE @IdUsuarioAdminNico UNIQUEIDENTIFIER
        DECLARE @IdFamiliaAdministrador UNIQUEIDENTIFIER
        
        SELECT @IdUsuarioAdminNico = IdUsuario FROM Usuario WHERE UserName = 'adminNico'
        SELECT @IdFamiliaAdministrador = IdFamilia FROM Familia WHERE Nombre = 'Administrador'
        
        -- Primero eliminar otras familias (si las tiene)
        DELETE FROM Usuario_Familia WHERE IdUsuario = @IdUsuarioAdminNico
        
        -- Asignar Administrador
        INSERT INTO Usuario_Familia (IdUsuario, IdFamilia)
        VALUES (@IdUsuarioAdminNico, @IdFamiliaAdministrador)
        
        PRINT '? Familia Administrador asignada a adminNico'
    END
    ELSE
    BEGIN
        PRINT '? Usuario ya tiene familia Administrador asignada'
    END
END
ELSE
BEGIN
    PRINT '? Usuario adminNico NO EXISTE'
    PRINT '?? Verificar el nombre exacto del usuario'
END
PRINT ''
GO

-- ==================================================
-- PASO 6: Resumen final
-- ==================================================
PRINT '============================================'
PRINT '?? RESUMEN FINAL'
PRINT '============================================'
PRINT ''

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
PRINT 'INSTRUCCIONES FINALES'
PRINT '============================================'
PRINT ''
PRINT '1?? CERRAR la sesión actual de adminNico (si está abierta)'
PRINT '2?? VOLVER A INICIAR sesión con adminNico'
PRINT '3?? VERIFICAR que ahora vea TODOS los menús'
PRINT ''
PRINT 'Menús esperados:'
PRINT '? PEDIDOS'
PRINT '? CLIENTE'
PRINT '? PRODUCTOS'
PRINT '? STOCK'
PRINT '? BÚSQUEDA'
PRINT '? REPORTES'
PRINT '? GESTIÓN DE USUARIOS'
PRINT '? PROVEEDORES'
PRINT '? BACKUP Y RESTORE'
PRINT ''
PRINT '? Script ejecutado correctamente'
PRINT '============================================'
GO
