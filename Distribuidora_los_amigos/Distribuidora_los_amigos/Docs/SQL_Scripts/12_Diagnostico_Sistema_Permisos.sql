-- =============================================
-- Script: Diagnóstico Completo del Sistema de Permisos
-- Base de datos: Login
-- Descripción: Verifica el estado actual del sistema de permisos
--              y diagnostica problemas con usuarios sin permisos
-- =============================================

USE [Login]
GO

PRINT '============================================'
PRINT 'DIAGNÓSTICO DEL SISTEMA DE PERMISOS'
PRINT '============================================'
PRINT ''

-- ==================================================
-- PASO 1: Verificar Patentes en el Sistema
-- ==================================================
PRINT '1?? PATENTES EN EL SISTEMA'
PRINT '===================================='
PRINT ''

DECLARE @TotalPatentes INT
SELECT @TotalPatentes = COUNT(*) FROM Patente

IF @TotalPatentes = 0
BEGIN
    PRINT '? NO HAY PATENTES en la base de datos'
    PRINT '?? Esto es el problema principal'
    PRINT '?? Las patentes deben existir ANTES de crear el usuario admin'
    PRINT ''
END
ELSE
BEGIN
    PRINT '? Total de patentes en el sistema: ' + CAST(@TotalPatentes AS VARCHAR(10))
    PRINT ''
    
    SELECT 
        COUNT(*) AS Total,
        SUM(CASE WHEN TipoAcceso = 0 THEN 1 ELSE 0 END) AS UI_Visualizacion,
        SUM(CASE WHEN TipoAcceso = 1 THEN 1 ELSE 0 END) AS Control_Administracion,
        SUM(CASE WHEN TipoAcceso = 2 THEN 1 ELSE 0 END) AS UseCases_Especiales
    FROM Patente
    
    PRINT ''
    PRINT '?? Listado de patentes:'
    SELECT 
        ROW_NUMBER() OVER (ORDER BY TipoAcceso, Nombre) AS '#',
        Nombre,
        DataKey,
        CASE TipoAcceso
            WHEN 0 THEN 'UI (Visualización)'
            WHEN 1 THEN 'Control (Administración)'
            WHEN 2 THEN 'UseCases (Especiales)'
        END AS TipoAcceso
    FROM Patente
    ORDER BY TipoAcceso, Nombre
END
PRINT ''
PRINT '============================================'
PRINT ''
GO

-- ==================================================
-- PASO 2: Verificar Familias
-- ==================================================
PRINT '2?? FAMILIAS (ROLES) EN EL SISTEMA'
PRINT '===================================='
PRINT ''

DECLARE @TotalFamilias INT
SELECT @TotalFamilias = COUNT(*) FROM Familia

IF @TotalFamilias = 0
BEGIN
    PRINT '? NO HAY FAMILIAS en la base de datos'
    PRINT ''
END
ELSE
BEGIN
    PRINT '? Total de familias: ' + CAST(@TotalFamilias AS VARCHAR(10))
    PRINT ''
    
    SELECT 
        f.Nombre AS Familia,
        COUNT(fp.IdPatente) AS PatentesAsignadas
    FROM Familia f
    LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
    GROUP BY f.Nombre
END
PRINT ''
PRINT '============================================'
PRINT ''
GO

-- ==================================================
-- PASO 3: Verificar Usuarios
-- ==================================================
PRINT '3?? USUARIOS EN EL SISTEMA'
PRINT '===================================='
PRINT ''

DECLARE @TotalUsuarios INT
SELECT @TotalUsuarios = COUNT(*) FROM Usuario

IF @TotalUsuarios = 0
BEGIN
    PRINT '? NO HAY USUARIOS en la base de datos'
    PRINT '?? Al iniciar la aplicación, se creará el usuario admin automáticamente'
    PRINT ''
END
ELSE
BEGIN
    PRINT '? Total de usuarios: ' + CAST(@TotalUsuarios AS VARCHAR(10))
    PRINT ''
    
    SELECT 
        u.UserName AS Usuario,
        u.Email,
        CASE u.Estado 
            WHEN 1 THEN 'Habilitado' 
            ELSE 'Deshabilitado' 
        END AS Estado,
        COUNT(uf.IdFamilia) AS FamiliasAsignadas
    FROM Usuario u
    LEFT JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
    GROUP BY u.UserName, u.Email, u.Estado
END
PRINT ''
PRINT '============================================'
PRINT ''
GO

-- ==================================================
-- PASO 4: Verificar Usuario Admin Específicamente
-- ==================================================
PRINT '4?? ESTADO DEL USUARIO ADMIN'
PRINT '===================================='
PRINT ''

IF NOT EXISTS (SELECT 1 FROM Usuario WHERE UserName = 'admin')
BEGIN
    PRINT '? Usuario "admin" NO EXISTE'
    PRINT '?? Se creará automáticamente al iniciar la aplicación sin usuarios'
    PRINT ''
END
ELSE
BEGIN
    PRINT '? Usuario "admin" EXISTE'
    PRINT ''
    
    -- Información básica
    SELECT 
        UserName,
        Email,
        CASE Estado 
            WHEN 1 THEN 'Habilitado' 
            ELSE 'Deshabilitado' 
        END AS Estado
    FROM Usuario 
    WHERE UserName = 'admin'
    
    PRINT ''
    
    -- Familias asignadas
    PRINT '?? Familias asignadas al usuario admin:'
    IF NOT EXISTS (
        SELECT 1 FROM Usuario u
        INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
        WHERE u.UserName = 'admin'
    )
    BEGIN
        PRINT '? El usuario admin NO TIENE FAMILIAS ASIGNADAS'
        PRINT '?? Esto es un problema - el usuario no tendrá permisos'
    END
    ELSE
    BEGIN
        SELECT 
            f.Nombre AS Familia,
            COUNT(fp.IdPatente) AS PatentesEnLaFamilia
        FROM Usuario u
        INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
        INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
        LEFT JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
        WHERE u.UserName = 'admin'
        GROUP BY f.Nombre
    END
    
    PRINT ''
    
    -- Patentes accesibles (a través de familias)
    PRINT '?? Patentes accesibles por el usuario admin (a través de familias):'
    DECLARE @PatentesAdmin INT
    SELECT @PatentesAdmin = COUNT(DISTINCT p.IdPatente)
    FROM Usuario u
    INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
    INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
    INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
    INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
    WHERE u.UserName = 'admin'
    
    IF @PatentesAdmin = 0
    BEGIN
        PRINT '? El usuario admin NO TIENE PATENTES ACCESIBLES'
        PRINT '?? Problema: La familia asignada no tiene patentes'
        PRINT '?? O el usuario no tiene familia asignada'
    END
    ELSE
    BEGIN
        PRINT '? Total de patentes accesibles: ' + CAST(@PatentesAdmin AS VARCHAR(10))
        PRINT ''
        
        SELECT 
            p.Nombre AS Patente,
            CASE p.TipoAcceso
                WHEN 0 THEN 'UI'
                WHEN 1 THEN 'Control'
                WHEN 2 THEN 'UseCases'
            END AS Tipo,
            f.Nombre AS DesdeFamilia
        FROM Usuario u
        INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
        INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
        INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
        INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
        WHERE u.UserName = 'admin'
        ORDER BY p.TipoAcceso, p.Nombre
    END
END
PRINT ''
PRINT '============================================'
PRINT ''
GO

-- ==================================================
-- PASO 5: Verificar Relaciones
-- ==================================================
PRINT '5?? VERIFICAR RELACIONES DE PERMISOS'
PRINT '===================================='
PRINT ''

-- Usuario_Familia
DECLARE @TotalUsuarioFamilia INT
SELECT @TotalUsuarioFamilia = COUNT(*) FROM Usuario_Familia

PRINT '?? Relaciones Usuario_Familia: ' + CAST(@TotalUsuarioFamilia AS VARCHAR(10))

IF @TotalUsuarioFamilia > 0
BEGIN
    SELECT 
        u.UserName AS Usuario,
        f.Nombre AS Familia
    FROM Usuario_Familia uf
    INNER JOIN Usuario u ON uf.IdUsuario = u.IdUsuario
    INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
END

PRINT ''

-- Familia_Patente
DECLARE @TotalFamiliaPatente INT
SELECT @TotalFamiliaPatente = COUNT(*) FROM Familia_Patente

PRINT '?? Relaciones Familia_Patente: ' + CAST(@TotalFamiliaPatente AS VARCHAR(10))

IF @TotalFamiliaPatente > 0
BEGIN
    SELECT 
        f.Nombre AS Familia,
        COUNT(fp.IdPatente) AS TotalPatentes
    FROM Familia_Patente fp
    INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
    GROUP BY f.Nombre
END

PRINT ''

-- Usuario_Patente (no debería usarse en este diseño)
DECLARE @TotalUsuarioPatente INT
SELECT @TotalUsuarioPatente = COUNT(*) FROM Usuario_Patente

PRINT '?? Relaciones Usuario_Patente: ' + CAST(@TotalUsuarioPatente AS VARCHAR(10))

IF @TotalUsuarioPatente > 0
BEGIN
    PRINT '??  ADVERTENCIA: Esta tabla NO debería tener datos en el diseño actual'
    PRINT '?? El sistema usa Familias para asignar patentes, no directamente'
END
ELSE
BEGIN
    PRINT '? Correcto: Esta tabla está vacía (se usan familias)'
END

PRINT ''
PRINT '============================================'
PRINT ''
GO

-- ==================================================
-- PASO 6: Diagnóstico y Recomendaciones
-- ==================================================
PRINT '6?? DIAGNÓSTICO Y RECOMENDACIONES'
PRINT '===================================='
PRINT ''

-- Variables para el diagnóstico
DECLARE @DiagTotalPatentes INT
DECLARE @DiagTotalFamilias INT
DECLARE @DiagTotalUsuarios INT
DECLARE @DiagAdminExiste BIT = 0
DECLARE @DiagAdminTieneFamilia BIT = 0
DECLARE @DiagAdminTienePatentes INT = 0
DECLARE @DiagFamiliaAdminExiste BIT = 0
DECLARE @DiagFamiliaAdminTienePatentes INT = 0

SELECT @DiagTotalPatentes = COUNT(*) FROM Patente
SELECT @DiagTotalFamilias = COUNT(*) FROM Familia
SELECT @DiagTotalUsuarios = COUNT(*) FROM Usuario

IF EXISTS (SELECT 1 FROM Usuario WHERE UserName = 'admin')
    SET @DiagAdminExiste = 1

IF EXISTS (
    SELECT 1 FROM Usuario u
    INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
    WHERE u.UserName = 'admin'
)
    SET @DiagAdminTieneFamilia = 1

SELECT @DiagAdminTienePatentes = COUNT(DISTINCT p.IdPatente)
FROM Usuario u
INNER JOIN Usuario_Familia uf ON u.IdUsuario = uf.IdUsuario
INNER JOIN Familia f ON uf.IdFamilia = f.IdFamilia
INNER JOIN Familia_Patente fp ON f.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE u.UserName = 'admin'

IF EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'Administrador')
    SET @DiagFamiliaAdminExiste = 1

IF @DiagFamiliaAdminExiste = 1
BEGIN
    SELECT @DiagFamiliaAdminTienePatentes = COUNT(*)
    FROM Familia_Patente fp
    INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
    WHERE f.Nombre = 'Administrador'
END

-- Análisis y recomendaciones
PRINT '?? ANÁLISIS DEL ESTADO ACTUAL:'
PRINT '================================'
PRINT ''

IF @DiagTotalPatentes = 0
BEGIN
    PRINT '?? PROBLEMA CRÍTICO: No hay patentes en el sistema'
    PRINT '   Impacto: Los usuarios no tendrán ningún permiso'
    PRINT '   Solución: Insertar las 22 patentes en la tabla Patente'
    PRINT ''
    PRINT '   ?? Script recomendado: Ejecutar el script de creación de patentes'
    PRINT '      que contiene las 22 patentes estándar del sistema'
    PRINT ''
END
ELSE
BEGIN
    PRINT '? Hay ' + CAST(@DiagTotalPatentes AS VARCHAR(10)) + ' patentes en el sistema'
END

IF @DiagTotalUsuarios = 0
BEGIN
    PRINT '?? INFORMACIÓN: No hay usuarios en el sistema'
    PRINT '   Acción: Al iniciar la aplicación, se creará automáticamente'
    PRINT '           el usuario admin con la familia Administrador'
    PRINT ''
END

IF @DiagAdminExiste = 1
BEGIN
    PRINT '? Usuario admin existe'
    
    IF @DiagAdminTieneFamilia = 0
    BEGIN
        PRINT '?? PROBLEMA: Usuario admin NO tiene familia asignada'
        PRINT '   Impacto: El usuario no tiene ningún permiso'
        PRINT '   Solución: Ejecutar script 11_Asignar_Rol_Admin_A_Usuario_Existente.sql'
        PRINT ''
    END
    ELSE
    BEGIN
        PRINT '? Usuario admin tiene familia asignada'
        
        IF @DiagAdminTienePatentes = 0
        BEGIN
            PRINT '?? PROBLEMA: La familia del admin NO tiene patentes'
            PRINT '   Impacto: El usuario no puede ver ningún menú'
            PRINT '   Solución: Ejecutar script 11_Asignar_Rol_Admin_A_Usuario_Existente.sql'
            PRINT ''
        END
        ELSE
        BEGIN
            PRINT '? Usuario admin tiene ' + CAST(@DiagAdminTienePatentes AS VARCHAR(10)) + ' patentes accesibles'
            
            IF @DiagAdminTienePatentes < @DiagTotalPatentes
            BEGIN
                PRINT '?? ADVERTENCIA: Admin no tiene todas las patentes'
                PRINT '   (' + CAST(@DiagAdminTienePatentes AS VARCHAR(10)) + ' de ' + CAST(@DiagTotalPatentes AS VARCHAR(10)) + ')'
                PRINT '   Recomendación: Ejecutar script 11 para asignar todas'
                PRINT ''
            END
        END
    END
END

IF @DiagFamiliaAdminExiste = 1
BEGIN
    PRINT '? Familia "Administrador" existe'
    
    IF @DiagFamiliaAdminTienePatentes = 0
    BEGIN
        PRINT '?? PROBLEMA: Familia "Administrador" NO tiene patentes asignadas'
        PRINT '   Impacto: Los usuarios con rol Administrador no tendrán permisos'
        PRINT '   Solución: Ejecutar script 11_Asignar_Rol_Admin_A_Usuario_Existente.sql'
        PRINT ''
    END
    ELSE
    BEGIN
        PRINT '? Familia "Administrador" tiene ' + CAST(@DiagFamiliaAdminTienePatentes AS VARCHAR(10)) + ' patentes'
    END
END

PRINT ''
PRINT '============================================'
PRINT ''

-- Recomendación final
IF @DiagTotalPatentes = 0
BEGIN
    PRINT '?? ACCIÓN RECOMENDADA #1: CREAR PATENTES'
    PRINT '   Antes de crear usuarios, debe tener las 22 patentes en la BD'
    PRINT '   Ejecute un script que inserte todas las patentes necesarias'
    PRINT ''
END

IF @DiagAdminExiste = 1 AND (@DiagAdminTieneFamilia = 0 OR @DiagAdminTienePatentes = 0)
BEGIN
    PRINT '?? ACCIÓN RECOMENDADA #2: ASIGNAR PERMISOS AL ADMIN'
    PRINT '   Ejecutar: 11_Asignar_Rol_Admin_A_Usuario_Existente.sql'
    PRINT ''
END

IF @DiagTotalUsuarios = 0 AND @DiagTotalPatentes > 0
BEGIN
    PRINT '?? ACCIÓN RECOMENDADA #3: INICIAR LA APLICACIÓN'
    PRINT '   Con las patentes creadas, al iniciar la aplicación:'
    PRINT '   1. Se creará el usuario admin'
    PRINT '   2. Se creará/encontrará la familia Administrador'
    PRINT '   3. Se asignarán TODAS las patentes a la familia'
    PRINT '   4. Se asignará la familia al usuario admin'
    PRINT ''
END

PRINT '============================================'
PRINT '? DIAGNÓSTICO COMPLETADO'
PRINT '============================================'
GO
