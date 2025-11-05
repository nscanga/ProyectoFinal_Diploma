-- ============================================
-- Script para verificar y corregir asignación de usuarios a familias
-- ============================================
-- Problema: Los usuarios no tienen familias asignadas ("Sin rol asignado")
-- Solución: Verificar y asignar usuarios a familias
-- ============================================

USE [Login]
GO

-- ==================================================
-- PASO 1: Ver usuarios y sus familias asignadas
-- ==================================================
PRINT '============================================'
PRINT 'USUARIOS Y SUS FAMILIAS ASIGNADAS'
PRINT '============================================'

SELECT 
    u.UserName AS Usuario,
    u.Email,
    u.Estado,
    f.Nombre AS Familia,
    CASE 
        WHEN f.Nombre IS NULL THEN '? Sin rol asignado'
        ELSE '? Con rol'
    END AS Estado_Rol
FROM [dbo].[Usuario] u
LEFT JOIN [dbo].[Usuario_Familia] uf ON u.IdUsuario = uf.IdUsuario
LEFT JOIN [dbo].[Familia] f ON uf.IdFamilia = f.IdFamilia
ORDER BY u.UserName
GO

-- ==================================================
-- PASO 2: Ver familias disponibles
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT 'FAMILIAS DISPONIBLES'
PRINT '============================================'

SELECT 
    IdFamilia,
    Nombre,
    (SELECT COUNT(*) FROM [dbo].[Familia_Patente] WHERE IdFamilia = f.IdFamilia) AS CantidadPatentes,
    (SELECT COUNT(*) FROM [dbo].[Usuario_Familia] WHERE IdFamilia = f.IdFamilia) AS UsuariosAsignados
FROM [dbo].[Familia] f
ORDER BY Nombre
GO

-- ==================================================
-- PASO 3: Ejemplo de cómo asignar un usuario a una familia
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT 'EJEMPLO: ASIGNAR USUARIO A FAMILIA'
PRINT '============================================'
PRINT 'Para asignar un usuario a una familia, ejecuta:'
PRINT ''
PRINT 'DECLARE @IdUsuario UNIQUEIDENTIFIER'
PRINT 'DECLARE @IdFamilia UNIQUEIDENTIFIER'
PRINT ''
PRINT '-- Obtener ID del usuario (cambia el nombre de usuario)'
PRINT 'SELECT @IdUsuario = IdUsuario FROM Usuario WHERE UserName = ''TU_USUARIO'''
PRINT ''
PRINT '-- Obtener ID de la familia (cambia el nombre del rol)'
PRINT 'SELECT @IdFamilia = IdFamilia FROM Familia WHERE Nombre = ''UI'' -- o ''Administrador'', etc.'
PRINT ''
PRINT '-- Verificar que no exista la asignación'
PRINT 'IF NOT EXISTS (SELECT 1 FROM Usuario_Familia WHERE IdUsuario = @IdUsuario AND IdFamilia = @IdFamilia)'
PRINT 'BEGIN'
PRINT '    INSERT INTO Usuario_Familia (IdUsuarioFamilia, IdUsuario, IdFamilia)'
PRINT '    VALUES (NEWID(), @IdUsuario, @IdFamilia)'
PRINT '    PRINT ''? Usuario asignado a la familia exitosamente'''
PRINT 'END'
PRINT 'ELSE'
PRINT 'BEGIN'
PRINT '    PRINT ''? El usuario ya está asignado a esta familia'''
PRINT 'END'
PRINT ''
GO

-- ==================================================
-- PASO 4: Script rápido para asignar usuarios sin familia
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT 'OPCIÓN: ASIGNAR TODOS LOS USUARIOS SIN ROL A "UI"'
PRINT '============================================'
PRINT 'Si deseas asignar automáticamente todos los usuarios sin rol a la familia "UI", ejecuta:'
PRINT ''
PRINT 'DECLARE @IdFamiliaUI UNIQUEIDENTIFIER'
PRINT 'SELECT @IdFamiliaUI = IdFamilia FROM Familia WHERE Nombre = ''UI'''
PRINT ''
PRINT 'INSERT INTO Usuario_Familia (IdUsuarioFamilia, IdUsuario, IdFamilia)'
PRINT 'SELECT NEWID(), u.IdUsuario, @IdFamiliaUI'
PRINT 'FROM Usuario u'
PRINT 'WHERE NOT EXISTS ('
PRINT '    SELECT 1 FROM Usuario_Familia uf WHERE uf.IdUsuario = u.IdUsuario'
PRINT ')'
PRINT ''
GO

-- ==================================================
-- PASO 5: Verificar patentes de cada familia
-- ==================================================
PRINT ''
PRINT '============================================'
PRINT 'PATENTES POR FAMILIA'
PRINT '============================================'

SELECT 
    f.Nombre AS Familia,
    COUNT(fp.IdPatente) AS TotalPatentes,
    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS Patentes_UI,
    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS Patentes_Control,
    SUM(CASE WHEN p.TipoAcceso = 2 THEN 1 ELSE 0 END) AS Patentes_UseCases
FROM [dbo].[Familia] f
LEFT JOIN [dbo].[Familia_Patente] fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN [dbo].[Patente] p ON fp.IdPatente = p.IdPatente
GROUP BY f.Nombre
ORDER BY f.Nombre
GO

PRINT ''
PRINT '? Verificación completada'
PRINT '============================================'
GO
