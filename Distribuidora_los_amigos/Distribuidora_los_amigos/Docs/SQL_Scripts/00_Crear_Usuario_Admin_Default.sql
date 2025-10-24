-- =============================================
-- Script: Crear Usuario Administrador por Defecto
-- Base de datos: Login
-- Descripci�n: Crea el usuario administrador cuando se requiere acceso de emergencia
-- =============================================

USE [Login]
GO

-- Verificar si el usuario admin ya existe
IF NOT EXISTS (SELECT 1 FROM Usuario WHERE UserName = 'admin')
BEGIN
    PRINT 'Creando usuario administrador por defecto...'
    
    -- Crear el usuario admin con contrase�a hasheada MD5
    -- Contrase�a en texto plano: Admin123!
    -- Hash MD5: e3afed0047b08059d0fada10f400c1e5
    INSERT INTO Usuario (IdUsuario, UserName, Password, Estado, Email, Lenguaje)
    VALUES (
        NEWID(),
        'admin',
        'e3afed0047b08059d0fada10f400c1e5',  -- Hash MD5 de "Admin123!"
        1,  -- Estado: Habilitado
        'admin@sistema.com',
        'es-ES'
    )
    
    PRINT 'Usuario administrador creado exitosamente.'
    PRINT 'Usuario: admin'
    PRINT 'Contrase�a: Admin123!'
    PRINT '?? IMPORTANTE: Cambie esta contrase�a inmediatamente despu�s del primer inicio de sesi�n.'
END
ELSE
BEGIN
    PRINT 'El usuario admin ya existe en el sistema.'
END
GO

-- Verificar que el usuario fue creado correctamente
SELECT 
    IdUsuario,
    UserName,
    Email,
    Estado,
    Lenguaje,
    'Creado correctamente' AS Estado_Creacion
FROM Usuario 
WHERE UserName = 'admin'
GO

-- =============================================
-- NOTAS IMPORTANTES:
-- =============================================
-- 1. Este script es para uso de emergencia o instalaci�n manual
-- 2. La aplicaci�n crea autom�ticamente este usuario si no existe ning�n usuario
-- 3. La contrase�a debe ser cambiada inmediatamente despu�s del primer uso
-- 4. Hash MD5 calculado: MD5("Admin123!") = e3afed0047b08059d0fada10f400c1e5
-- =============================================

-- =============================================
-- Script alternativo: Resetear contrase�a del admin
-- =============================================
-- Descomentar y ejecutar si necesitas resetear la contrase�a del admin
/*
UPDATE Usuario
SET Password = 'e3afed0047b08059d0fada10f400c1e5'  -- Resetea a "Admin123!"
WHERE UserName = 'admin'

PRINT 'Contrase�a del usuario admin reseteada a: Admin123!'
*/
GO

-- =============================================
-- Script: Habilitar usuario admin si fue deshabilitado
-- =============================================
-- Descomentar y ejecutar si el admin fue deshabilitado y necesitas acceso
/*
UPDATE Usuario
SET Estado = 1
WHERE UserName = 'admin'

PRINT 'Usuario admin habilitado correctamente.'
*/
GO

-- =============================================
-- Script: Verificar estado actual del usuario admin
-- =============================================
SELECT 
    UserName AS 'Usuario',
    Email AS 'Email',
    CASE Estado 
        WHEN 1 THEN 'Habilitado' 
        WHEN 0 THEN 'Deshabilitado'
        ELSE 'Desconocido'
    END AS 'Estado',
    Lenguaje AS 'Idioma'
FROM Usuario 
WHERE UserName = 'admin'
GO
