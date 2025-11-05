-- ============================================
-- GUÍA COMPLETA: Configuración Final del Sistema de Permisos
-- ============================================
-- Base de datos: Login
-- Propósito: Verificar y corregir toda la configuración de permisos
-- ============================================

USE [Login]
GO

PRINT '============================================'
PRINT 'GUÍA DE CONFIGURACIÓN DE PERMISOS'
PRINT 'Sistema de Distribuidora Los Amigos'
PRINT '============================================'
PRINT ''

-- ============================================
-- PARTE 1: VERIFICACIÓN ACTUAL
-- ============================================

PRINT '--------------------------------------------'
PRINT 'PARTE 1: ESTADO ACTUAL DEL SISTEMA'
PRINT '--------------------------------------------'
PRINT ''

-- 1.1 Familias (Roles) disponibles
PRINT '1.1 FAMILIAS (ROLES) DISPONIBLES:'
PRINT ''
SELECT 
    ROW_NUMBER() OVER (ORDER BY Nombre) AS '#',
    Nombre AS 'Familia/Rol',
    IdFamilia AS 'ID'
FROM [dbo].[Familia]
ORDER BY Nombre
GO

-- 1.2 Usuarios y sus familias asignadas
PRINT ''
PRINT '1.2 USUARIOS Y SUS FAMILIAS:'
PRINT ''
SELECT 
    u.UserName AS 'Usuario',
    ISNULL(STRING_AGG(f.Nombre, ', '), '? SIN ROL ASIGNADO') AS 'Familias Asignadas',
    u.Estado AS 'Estado'
FROM [dbo].[Usuario] u
LEFT JOIN [dbo].[Usuario_Familia] uf ON u.IdUsuario = uf.IdUsuario
LEFT JOIN [dbo].[Familia] f ON uf.IdFamilia = f.IdFamilia
GROUP BY u.UserName, u.Estado
ORDER BY u.UserName
GO

-- 1.3 Resumen de patentes por tipo
PRINT ''
PRINT '1.3 RESUMEN DE PATENTES POR TIPO DE ACCESO:'
PRINT ''
SELECT 
    CASE TipoAcceso
        WHEN 0 THEN 'UI (Visualización)'
        WHEN 1 THEN 'Control (Administración)'
        WHEN 2 THEN 'UseCases'
        ELSE 'Desconocido'
    END AS 'Tipo de Acceso',
    COUNT(*) AS 'Cantidad de Patentes'
FROM [dbo].[Patente]
GROUP BY TipoAcceso
ORDER BY TipoAcceso
GO

-- 1.4 Patentes por familia
PRINT ''
PRINT '1.4 PATENTES POR FAMILIA:'
PRINT ''
SELECT 
    f.Nombre AS 'Familia',
    COUNT(DISTINCT fp.IdPatente) AS 'Total Patentes',
    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS 'UI',
    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS 'Control',
    SUM(CASE WHEN p.TipoAcceso = 2 THEN 1 ELSE 0 END) AS 'UseCases'
FROM [dbo].[Familia] f
LEFT JOIN [dbo].[Familia_Patente] fp ON f.IdFamilia = fp.IdFamilia
LEFT JOIN [dbo].[Patente] p ON fp.IdPatente = p.IdPatente
GROUP BY f.Nombre
ORDER BY f.Nombre
GO

-- ============================================
-- PARTE 2: DIAGNÓSTICO DE PROBLEMAS
-- ============================================

PRINT ''
PRINT '--------------------------------------------'
PRINT 'PARTE 2: DIAGNÓSTICO DE PROBLEMAS'
PRINT '--------------------------------------------'
PRINT ''

-- 2.1 Usuarios sin familias asignadas
PRINT '2.1 USUARIOS SIN FAMILIAS ASIGNADAS (Problema: "Sin rol asignado"):'
PRINT ''
SELECT 
    u.UserName AS 'Usuario',
    u.Email,
    '? SIN ROL' AS 'Estado'
FROM [dbo].[Usuario] u
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Usuario_Familia] uf WHERE uf.IdUsuario = u.IdUsuario
)
GO

-- 2.2 Familias sin patentes
PRINT ''
PRINT '2.2 FAMILIAS SIN PATENTES ASIGNADAS:'
PRINT ''
SELECT 
    f.Nombre AS 'Familia',
    '? Sin patentes asignadas' AS 'Estado'
FROM [dbo].[Familia] f
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Familia_Patente] fp WHERE fp.IdFamilia = f.IdFamilia
)
GO

-- ============================================
-- PARTE 3: SOLUCIONES RECOMENDADAS
-- ============================================

PRINT ''
PRINT '--------------------------------------------'
PRINT 'PARTE 3: SOLUCIONES'
PRINT '--------------------------------------------'
PRINT ''

-- 3.1 Ejemplo: Asignar un usuario a la familia "Administrador"
PRINT '3.1 EJEMPLO: ASIGNAR USUARIO A FAMILIA ADMINISTRADOR'
PRINT ''
PRINT '-- Copia y ejecuta este código, reemplazando ''nombre_usuario'' con el usuario real:'
PRINT ''
PRINT 'DECLARE @IdUsuario UNIQUEIDENTIFIER'
PRINT 'DECLARE @IdFamilia UNIQUEIDENTIFIER'
PRINT ''
PRINT '-- Reemplaza ''nombre_usuario'' con el nombre del usuario'
PRINT 'SELECT @IdUsuario = IdUsuario FROM Usuario WHERE UserName = ''nombre_usuario'''
PRINT ''
PRINT '-- Obtener ID de la familia Administrador'
PRINT 'SELECT @IdFamilia = IdFamilia FROM Familia WHERE Nombre = ''Administrador'''
PRINT ''
PRINT 'IF @IdUsuario IS NULL'
PRINT 'BEGIN'
PRINT '    PRINT ''? ERROR: Usuario no encontrado'''
PRINT '    RETURN'
PRINT 'END'
PRINT ''
PRINT 'IF @IdFamilia IS NULL'
PRINT 'BEGIN'
PRINT '    PRINT ''? ERROR: Familia Administrador no encontrada'''
PRINT '    RETURN'
PRINT 'END'
PRINT ''
PRINT '-- Verificar que no exista la asignación'
PRINT 'IF NOT EXISTS (SELECT 1 FROM Usuario_Familia WHERE IdUsuario = @IdUsuario AND IdFamilia = @IdFamilia)'
PRINT 'BEGIN'
PRINT '    INSERT INTO Usuario_Familia (IdUsuarioFamilia, IdUsuario, IdFamilia)'
PRINT '    VALUES (NEWID(), @IdUsuario, @IdFamilia)'
PRINT '    PRINT ''? Usuario asignado a Administrador exitosamente'''
PRINT 'END'
PRINT 'ELSE'
PRINT 'BEGIN'
PRINT '    PRINT ''? El usuario ya está asignado a Administrador'''
PRINT 'END'
PRINT ''
GO

-- 3.2 Ejemplo: Asignar un usuario a la familia "UI"
PRINT ''
PRINT '3.2 EJEMPLO: ASIGNAR USUARIO A FAMILIA UI'
PRINT ''
PRINT '-- Usa el mismo código anterior, pero cambia la línea:'
PRINT '-- SELECT @IdFamilia = IdFamilia FROM Familia WHERE Nombre = ''UI'''
PRINT ''
GO

-- ============================================
-- PARTE 4: SCRIPTS RÁPIDOS (OPCIONAL)
-- ============================================

PRINT ''
PRINT '--------------------------------------------'
PRINT 'PARTE 4: SCRIPTS RÁPIDOS (OPCIONAL)'
PRINT '--------------------------------------------'
PRINT ''

PRINT '4.1 OPCIÓN: Asignar TODOS los usuarios sin rol a "UI"'
PRINT ''
PRINT '-- ? CUIDADO: Esto asignará automáticamente todos los usuarios sin rol a la familia UI'
PRINT '-- Solo ejecuta esto si estás seguro'
PRINT ''
PRINT '/*'
PRINT 'DECLARE @IdFamiliaUI UNIQUEIDENTIFIER'
PRINT 'SELECT @IdFamiliaUI = IdFamilia FROM Familia WHERE Nombre = ''UI'''
PRINT ''
PRINT 'IF @IdFamiliaUI IS NULL'
PRINT 'BEGIN'
PRINT '    PRINT ''? ERROR: Familia UI no encontrada'''
PRINT '    RETURN'
PRINT 'END'
PRINT ''
PRINT 'INSERT INTO Usuario_Familia (IdUsuarioFamilia, IdUsuario, IdFamilia)'
PRINT 'SELECT NEWID(), u.IdUsuario, @IdFamiliaUI'
PRINT 'FROM Usuario u'
PRINT 'WHERE NOT EXISTS ('
PRINT '    SELECT 1 FROM Usuario_Familia uf WHERE uf.IdUsuario = u.IdUsuario'
PRINT ')'
PRINT ''
PRINT 'PRINT ''? Usuarios asignados a familia UI'''
PRINT '*/'
PRINT ''
GO

-- ============================================
-- PARTE 5: VERIFICACIÓN FINAL
-- ============================================

PRINT ''
PRINT '--------------------------------------------'
PRINT 'PARTE 5: VERIFICACIÓN FINAL'
PRINT '--------------------------------------------'
PRINT ''

PRINT '5.1 DESPUÉS DE ASIGNAR USUARIOS, EJECUTA ESTE QUERY PARA VERIFICAR:'
PRINT ''
PRINT '/*'
PRINT 'SELECT '
PRINT '    u.UserName AS ''Usuario'','
PRINT '    STRING_AGG(f.Nombre, '', '') AS ''Familias'','
PRINT '    COUNT(DISTINCT p.IdPatente) AS ''Total Patentes'','
PRINT '    SUM(CASE WHEN p.TipoAcceso = 0 THEN 1 ELSE 0 END) AS ''Patentes UI'','
PRINT '    SUM(CASE WHEN p.TipoAcceso = 1 THEN 1 ELSE 0 END) AS ''Patentes Control'''
PRINT 'FROM Usuario u'
PRINT 'LEFT JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario'
PRINT 'LEFT JOIN Familia f ON uf.IdFamilia = f.IdFamilia'
PRINT 'LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia'
PRINT 'LEFT JOIN Patente p ON fp.IdPatente = p.IdPatente'
PRINT 'GROUP BY u.UserName'
PRINT 'ORDER BY u.UserName'
PRINT '*/'
PRINT ''
GO

-- ============================================
-- RESUMEN Y RECOMENDACIONES
-- ============================================

PRINT ''
PRINT '============================================'
PRINT 'RESUMEN Y PRÓXIMOS PASOS'
PRINT '============================================'
PRINT ''
PRINT '? ESTADO ACTUAL VERIFICADO'
PRINT ''
PRINT 'PRÓXIMOS PASOS:'
PRINT '1. Revisa la sección 2.1 para ver qué usuarios NO tienen familias'
PRINT '2. Usa el código de la sección 3.1 o 3.2 para asignar usuarios a familias'
PRINT '3. Ejecuta el query de la sección 5.1 para verificar'
PRINT '4. Prueba el login en la aplicación'
PRINT ''
PRINT 'FAMILIAS DISPONIBLES:'
PRINT '- Administrador: Acceso completo (UI + Control)'
PRINT '- UI: Acceso solo de visualización'
PRINT ''
PRINT 'RECUERDA:'
PRINT '- Los cambios en la BD son INMEDIATOS'
PRINT '- El usuario debe CERRAR SESIÓN y volver a iniciarla para ver los cambios'
PRINT '- Verifica que el código C# esté actualizado (MenuItemDecorator corregido)'
PRINT ''
PRINT '============================================'
PRINT 'FIN DEL DIAGNÓSTICO'
PRINT '============================================'
GO
